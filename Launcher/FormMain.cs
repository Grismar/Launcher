using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Launcher
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length==1)
            {
                MessageBox.Show("Pass a compatible html5 file as a command line parameter to the launcher.", "Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            if (!File.Exists(args[1]))
            {
                MessageBox.Show(String.Format("File not found \"{0}\". Pass a compatible html5 file as a command line parameter to the launcher.", args[1]), "Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(2);
            }

            webBrowser1.ObjectForScripting = new LauncherCallback(this, webBrowser1);
            webBrowser1.Url = new Uri(String.Format("file:///{0}", Path.GetFullPath(args[1])));

            WindowState = FormWindowState.Minimized;
        }
    }
}
