using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Writes console output to a richtextbox
/// </summary>
public class ConsoleWriter : TextWriter
{
    private RichTextBox textBox;

    public ConsoleWriter(RichTextBox txtBox)
    {
        this.textBox = txtBox;
    }

    public override void Write(char value)
    {
        if (textBox.InvokeRequired)
        {
            textBox.Invoke(new MethodInvoker(delegate { this.Write(value); }));
        }
        else
        {
            textBox.AppendText(value.ToString() + Environment.NewLine);
        }
    }

    public override void Write(string value)
    {
        if (textBox.InvokeRequired)
        {
            textBox.Invoke(new MethodInvoker(delegate { this.Write(value); }));
        }
        else
        {
            textBox.AppendText(value + Environment.NewLine);
        }
    }

    public override void WriteLine(string value)
    {
        if (textBox.InvokeRequired)
        {
            textBox.Invoke(new MethodInvoker(delegate { this.WriteLine(value); }));
        }
        else
        {
            textBox.AppendText(value + Environment.NewLine);
        }
    }

    public override Encoding Encoding
    {
        get { return Encoding.UTF8; }
    }
}
