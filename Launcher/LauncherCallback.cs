using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class LauncherCallback
    {
        Form _form;
        WebBrowser _browser;
        bool _inLauncher = true;

        public LauncherCallback (Form form, WebBrowser browser)
        {
            _form = form;
            _browser = browser;
            _browser.Navigating += _browser_Navigating;
        }

        private void _browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            String url = e.Url.ToString();
            // if links are to be loaded outside the launcher and the link isn't actually a call to the LauncherCallback
            if (!_inLauncher && (url.Length <= 26 || url.Substring(0,26)!="javascript:window.external"))
            {
                e.Cancel = true;
                launch(url);
            };
        }

        public void showMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        public void setTitle(string title)
        {
            _form.Text = title;
        }

        public void setSize(int width, int height, bool center = true)
        {
            if (width < 0)
            {
                MessageBox.Show(String.Format("setSize width ({0}) needs to be a positive number.", width));
                return;
            }
            if (height < 0)
            {
                MessageBox.Show(String.Format("setSize height ({0}) needs to be a positive number.", height));
                return;
            }

            _form.Width = width;
            _form.Height = height;
            if (center)
            {
                int boundWidth = Screen.PrimaryScreen.Bounds.Width;
                int boundHeight = Screen.PrimaryScreen.Bounds.Height;
                int x = boundWidth - _form.Width;
                int y = boundHeight - _form.Height;

                // Position the form
                _form.Location = new Point(x / 2, y / 2);
            }
        }

        public void setRelativeSize(int width, int height, bool center = true)
        {
            if (width < 0 || width > 100)
            {
                MessageBox.Show(String.Format("setRelativeSize width ({0}) needs to be a percentage, between 0 and 100.", width));
                return;
            }
            if (height < 0 || height > 100)
            {
                MessageBox.Show(String.Format("setRelativeSize height ({0}) needs to be a percentage, between 0 and 100.", height));
                return;
            }

            width = width * Screen.PrimaryScreen.Bounds.Width / 100;
            height = height * Screen.PrimaryScreen.Bounds.Height / 100;

            setSize(width, height, center);
        }

        public void launchLocal(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                MessageBox.Show(String.Format("Path passed to launchLocal ({0}) needs to refer to an existing file.", path));
                return;
            }

            System.Diagnostics.Process.Start(Path.GetFullPath(path));
        }

        public void launch(string uri)
        {
            Uri uriResult;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
            {
                MessageBox.Show(String.Format("URI passed to launch ({0}) needs to be a well-formed URL.", uri));
                return;
            }

            System.Diagnostics.Process.Start(uri);
        }

        public void navigateInLauncher (bool inLauncher = true)
        {
            _inLauncher = inLauncher;
        }

        public void enableScrollbars(bool enable = true)
        {
            _browser.ScrollBarsEnabled = enable;
        }

        public void restore(bool restore = true)
        {
            if (restore)
            {
                _form.WindowState = FormWindowState.Normal;
            }
            else
            {
                _form.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
