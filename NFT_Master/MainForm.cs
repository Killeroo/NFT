using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using NFT.Core;
using NFT.Comms;
using NFT.Rsync;
using NFT.Logger;

namespace NFT_Master
{
    public partial class MainForm : Form
    {
        // Version info
        static Version v = Assembly.GetExecutingAssembly().GetName().Version;
        string version = Assembly.GetExecutingAssembly().GetName().Name + " v" + v.Major + "." + v.Minor + "." + v.Build + " (r" + v.Revision + ")";

        public MainForm()
        {
            InitializeComponent();
            
            // Setup ui defaults
            txtRange.Text = Helper.GetLocalIPAddress(); 
            txtWorkingDirectory_TextChanged(new object(), new EventArgs()); // Load list view
            SetLogText(version); // Print current version in log window
        }

        #region Event Handlers

        private void btnScan_Click(object sender, EventArgs e)
        {
            // Start scan task in new thread
            /// TODO: Background thread
            new Thread(() =>
            {
                // Setup UI stuff
                SetLogText("Scanning for NFT_Slaves...");
                SetSlaveCount(0);
                SetStatusLabel("Scanning");
                ToggleProgressBar();
                ClearSlaveList();

                // Scan for slaves
                Slave.Scan(txtRange.Text);

                SetLogText(Slave.slaves.Count + " Slaves found, starting listeners...");

                // Start listening to each connected slave
                /// TODO: add to scan? or move somewhere else
                foreach (var slave in Slave.slaves)
                {
                    SlaveListener sl = new SlaveListener(slave);
                    Thread listeningThread = new Thread(new ThreadStart(sl.Start));
                    listeningThread.Start();

                    // Add to slave list box
                    AddToSlaveList(slave.client.Client.RemoteEndPoint.ToString().Split(':')[0]);
                }

                // Update UI stuff
                SetLogText("Done.");
                SetSlaveCount(Slave.slaves.Count);
                SetStatusLabel("Ready");
                ToggleProgressBar();

            }).Start();
            
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            Command c = new Command(NFT.Core.CommandType.Info);
            c.message = txtMessage.Text;

            // Send to all connected slaves
            Slave.SendAll(c);
        }
        private void btnFolderBrowse_Click(object sender, EventArgs e)
        {
            // Create folder browser dialog
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = txtWorkingDirectory.Text;
                if (fbd.ShowDialog(this) == DialogResult.OK)
                    txtWorkingDirectory.Text = fbd.SelectedPath;
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Slave.DisconnectAll();
        }
        private void txtWorkingDirectory_TextChanged(object sender, EventArgs e)
        {
            // Clear the tree
            dirsTreeView.Nodes.Clear();

            // If entered directory doesnt exist, dont bother rendering tree
            if (!Directory.Exists(txtWorkingDirectory.Text))
                return;

            // Get folders and files
            string[] dirs = Directory.GetDirectories(txtWorkingDirectory.Text);
            string[] files = Directory.GetFiles(txtWorkingDirectory.Text);

            foreach (string dir in dirs)
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                TreeNode node = new TreeNode(di.Name, 0, 1);

                try
                {
                    node.Tag = dir;  //keep the directory's full path in the tag for use later

                    //if the directory has any sub directories add the place holder
                    if (di.GetFiles().Count() > 0 || di.GetDirectories().Count() > 0)//GetDirectories().Count() > 0) di.GetDirectories().Count() > 0)
                        node.Nodes.Add(null, "...", 0, 0);
                }
                catch (UnauthorizedAccessException)
                {
                    //if an unauthorized access exception occured display a locked folder
                    node.ImageIndex = 12;
                    node.SelectedImageIndex = 12;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DirectoryLister", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    dirsTreeView.Nodes.Add(node);
                }
            }

            foreach (string file in files)
            {
                // Creat new node with file name
                TreeNode node = new TreeNode(Path.GetFileName(file), 0, 1);

                // Display file image on node
                node.ImageIndex = 13;
                node.SelectedImageIndex = 13;

                // Add to node
                dirsTreeView.Nodes.Add(node);
            }
        }
        private void dirsTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Nodes[0].Text == "..." && e.Node.Nodes[0].Tag == null)
                {
                    e.Node.Nodes.Clear();

                    // get the list of sub directories & files
                    string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());
                    string[] files = Directory.GetFiles(e.Node.Tag.ToString());

                    foreach (string dir in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(dir);
                        TreeNode node = new TreeNode(di.Name, 0, 1);

                        try
                        {
                            //keep the directory's full path in the tag for use later
                            node.Tag = dir;  

                            //if the directory has any sub directories add the place holder
                            if (di.GetFiles().Count() > 0 || di.GetDirectories().Count() > 0)//GetDirectories().Count() > 0)
                                node.Nodes.Add(null, "...", 0, 0);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //if an unauthorized access exception occured display a locked folder
                            node.ImageIndex = 12;
                            node.SelectedImageIndex = 12;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "DirectoryLister", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        finally
                        {
                            e.Node.Nodes.Add(node);
                        }
                    }

                    foreach (string file in files)
                    {
                        // Creat new node with file name
                        TreeNode node = new TreeNode(Path.GetFileName(file), 0, 1);

                        // Display file image on node
                        node.ImageIndex = 13;
                        node.SelectedImageIndex = 13;

                        // Add to node
                        e.Node.Nodes.Add(node);
                    }
                }
            }
        }

        #endregion

        #region Threadsafe Methods

        private void SetLogText(string text)
        {
            // Check if calling thread is in same thread as ui
            if (this.txtLog.InvokeRequired)
            {
                // If not callback method in the ui thread
                MethodInvoker d = (MethodInvoker)delegate
                {
                    SetLogText(text);
                };

                this.Invoke(d);
            }
            else
            {
                // If in same thread set text normally
                this.txtLog.AppendText(text + "\n");
            }
        }
        private void SetSlaveCount(int count)
        {
            if (this.slaveGroupBox.InvokeRequired)
            {
                // Create delegate for callback
                MethodInvoker d = (MethodInvoker)delegate
                {
                    SetSlaveCount(count);
                };

                // Invoke delegate on UI Thread
                this.Invoke(d);
            }
            else
            {
                // If in same thread set text normally
                this.slaveGroupBox.Text = "Connected Slaves: " + count.ToString();
            }
        }
        private void SetStatusLabel(string text)
        {
            if (this.lblStatus.GetCurrentParent().InvokeRequired)
            {
                // Create delegate for callback
                MethodInvoker d = (MethodInvoker)delegate
                {
                    SetStatusLabel(text);
                };

                // Invoke delegate on UI Thread
                this.Invoke(d);
            }
            else
            {
                lblStatus.Text = text;
            }
        }
        private void ToggleProgressBar()
        {
            if (this.progressBar.GetCurrentParent().InvokeRequired)
            {
                // Create delegate for callback
                MethodInvoker d = (MethodInvoker)delegate
                {
                    ToggleProgressBar();
                };
                
                // Invoke delegate on UI Thread
                this.Invoke(d);
            }
            else
            {
                // Already on UI thread 
                if (progressBar.Style == ProgressBarStyle.Continuous)
                    // Start progress bar
                    progressBar.Style = ProgressBarStyle.Marquee;
                else
                    // Stop progress bar
                    progressBar.Style = ProgressBarStyle.Continuous;
            }
        }
        private void AddToSlaveList(string item)
        {
            if (this.listBoxSlaves.InvokeRequired)
            {
                // Create delegate for callback
                MethodInvoker d = (MethodInvoker)delegate
                {
                    AddToSlaveList(item);
                };

                // Invoke delegate on UI Thread
                this.Invoke(d);
            }
            else
            {
                // Already on UI thread 
                listBoxSlaves.Items.Add(item);
            }
        }
        private void ClearSlaveList()
        {
            if (this.listBoxSlaves.InvokeRequired)
            {
                // Create delegate for callback
                MethodInvoker d = (MethodInvoker)delegate
                {
                    ClearSlaveList();
                };

                // Invoke delegate on UI Thread
                this.Invoke(d);
            }
            else
            {
                // Already on UI thread 
                listBoxSlaves.Items.Clear();
            }
        }

        #endregion

    }
}
