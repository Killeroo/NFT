using System;
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
        string version = Assembly.GetExecutingAssembly().GetName().Name + " Version " + v.Major + "." + v.Minor + "." + v.Build + " (r" + v.Revision + ")";

        // Delegate for adding text to log from another thread
        delegate void StringArgReturningVoidDelegate(string text);

        public MainForm()
        {
            InitializeComponent();
            
            txtRange.Text = Helper.GetLocalIPAddress();

            SetLogText(version);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            // Start scan task in new thread
            new Thread(() =>
            {
                SetLogText("Scanning for NFT_Slaves...");
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

                SetLogText("Done.");
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
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Slave.DisconnectAll();
        }

        /// <summary>
        /// Thread safe access to Log text field
        /// </summary>
        /// <param name="text"></param>
        private void SetLogText(string text)
        {
            // Check if calling thread is in same thread as ui
            if (this.txtLog.InvokeRequired)
            {
                // If not callback method in the ui thread
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetLogText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // If in same thread set text normally
                this.txtLog.AppendText(text + "\n");
            }
        }
        private void ToggleProgressBar()
        {
            if (this.progressBar.InvokeRequired)
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
    }
}
