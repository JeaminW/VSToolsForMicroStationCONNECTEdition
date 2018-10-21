using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace VSTMC
{
    public partial class OptionsControlPage : UserControl
    {
        #region Fields

        internal OptionsPage msceOptionsPage;
        private bool isMSCEToolTipShown = false;
        private bool isSDKToolTipShown = false;
        private bool isMDLToolTipShown = false;
        private bool isBatchToolTipShown = false;

        #endregion

        #region Constructors

        public OptionsControlPage()
        {
            InitializeComponent();
        }

        #endregion

        public void Initialize()
        {
            tbMSCEPath.Text = msceOptionsPage.MSCEPath;
            tbSDKPath.Text = msceOptionsPage.MSCESDKPath;
            tbMDLAPPSPath.Text = msceOptionsPage.MDLAPPSPath;
            tbBuildPath.Text = msceOptionsPage.BentleyBuildFilePath;
            string BentleyApp = msceOptionsPage.BentleyApp;
        }

        #region OptionPageMSCE Methods

        private void OptionsControlsMSCE_Load(object sender, EventArgs e)
        {
            SetMSCEComboBox();
            SetMDLPadlockImage();
            SetBatchPadlockImage();
            btnMDLAppsFolder.Enabled = PathExist(tbMDLAPPSPath);
            btnBentleyBuildFolder.Enabled = isFileExist(tbBuildPath);
        }

        private void OptionsControlsMSCE_MouseMove(object sender, MouseEventArgs e)
        {
            if (tbMSCEPath.TextLength > 60)
            {
                if (tbMSCEPath == GetChildAtPoint(e.Location))
                {
                    if (!isMSCEToolTipShown)
                    {
                        toolTip1.Show(tbMSCEPath.Text, tbMSCEPath, 0, 25, 5000);
                        isMSCEToolTipShown = true;
                    }
                }
                else
                {
                    toolTip1.Hide(tbMSCEPath);
                    isMSCEToolTipShown = false;
                }
            }

            if (tbSDKPath.TextLength > 60)
            {
                if (tbSDKPath == GetChildAtPoint(e.Location))
                {
                    if (!isSDKToolTipShown)
                    {
                        toolTip1.Show(tbSDKPath.Text, tbSDKPath, 0, 25, 5000);
                        isSDKToolTipShown = true;
                    }
                }
                else
                {
                    toolTip1.Hide(tbSDKPath);
                    isSDKToolTipShown = false;
                }
            }

            if (!tbMDLAPPSPath.Enabled)
            {
                if (tbMDLAPPSPath.TextLength > 60)
                {
                    if (tbMDLAPPSPath == GetChildAtPoint(e.Location))
                    {
                        if (!isMDLToolTipShown)
                        {
                            toolTip1.Show(tbMDLAPPSPath.Text, tbMDLAPPSPath, 0, 25, 5000);
                            isMDLToolTipShown = true;
                        }
                    }
                    else
                    {
                        toolTip1.Hide(tbMDLAPPSPath);
                        isMDLToolTipShown = false;
                    }
                }
            }

            if (!tbBuildPath.Enabled)
            {
                if (tbBuildPath.TextLength > 60)
                {
                    if (tbBuildPath == GetChildAtPoint(e.Location))
                    {
                        if (!isBatchToolTipShown)
                        {
                            toolTip1.Show(tbBuildPath.Text, tbBuildPath, 0, 25, 5000);
                            isBatchToolTipShown = true;
                        }
                    }
                    else
                    {
                        toolTip1.Hide(tbBuildPath);
                        isBatchToolTipShown = false;
                    }
                }
            }
        }

        private void cboBentleyProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ICollection bentleyKeyCollection = BentleyDataCollector.BentleyProducts.Keys;
                ICollection bentleyValueCollection = BentleyDataCollector.BentleyProducts.Values;

                string[] bentleyKeys = new string[BentleyDataCollector.BentleyProducts.Count];
                string[] bentleyValues = new string[BentleyDataCollector.BentleyProducts.Count];
                bentleyKeyCollection.CopyTo(bentleyKeys, 0);
                bentleyValueCollection.CopyTo(bentleyValues, 0);
                for (int i = 0; i < BentleyDataCollector.BentleyProducts.Count; i++)
                {
                    if (!string.IsNullOrEmpty(cboBentleyProduct.Text))
                    {
                        if (bentleyKeys[i] == cboBentleyProduct.Text)
                        {
                            tbMSCEPath.Text = bentleyValues[i];
                            break;
                        }
                    }
                }

                tbSDKPath.Text = BentleyDataCollector.GetSDKPath(tbMSCEPath.Text);
                msceOptionsPage.BentleyApp = BentleyDataCollector.GetBentleyApp(tbMSCEPath.Text);

                if (!msceOptionsPage.MDLAPPSLock)
                {
                    tbMDLAPPSPath.Text = BentleyDataCollector.GetMdlappsPath(tbMSCEPath.Text);
                }

                if (!msceOptionsPage.BatchLock)
                {
                    tbBuildPath.Text = BentleyDataCollector.BentleyBuildBatchFilePath(msceOptionsPage.BentleyApp);
                }

                //if (!msceOptionsPage.LockSDK)
                //{
                //    msceOptionsPage.MSCESDKPath = BentleyDataCollector.SDKPath("microstation connect edition");
                //    tbSDKPath.Text = GetSDKPath;
                //}

                //if (!msceOptionsPage.LockMDL)
                //{
                //    if (tbMSCEPath.Text == "Not Installed")
                //        tbMDLAPPSPath.Text = "";
                //    else
                //        tbMDLAPPSPath.Text = tbMSCEPath.Text + "mdlapps\\";
                //}

            }
            catch (Exception)
            {
                //Do nothing when exception occurs.
            }
        }

        private void tbMSCEPath_TextChanged(object sender, EventArgs e)
        {
            msceOptionsPage.MSCEPath = tbMSCEPath.Text;
        }

        private void tbSDKPath_TextChanged(object sender, EventArgs e)
        {
            msceOptionsPage.MSCESDKPath = tbSDKPath.Text;
        }

        private void tbMDLAPPSPath_TextChanged(object sender, EventArgs e)
        {
            msceOptionsPage.MDLAPPSPath = tbMDLAPPSPath.Text;
            btnMDLAppsFolder.Enabled = PathExist(tbMDLAPPSPath);
        }

        private void tbBuildPath_TextChanged(object sender, EventArgs e)
        {
            msceOptionsPage.BentleyBuildFilePath = tbBuildPath.Text;
            btnBentleyBuildFolder.Enabled = isFileExist(tbBuildPath);
        }

        private void btnMSFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = tbMSCEPath.Text,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void btnSDKFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = tbSDKPath.Text,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void btnMDLAppsFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = tbMDLAPPSPath.Text,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void btnBentleyBuildFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = Path.GetDirectoryName(tbBuildPath.Text),
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void btnMDLAPPSPathBrowser_Click(object sender, EventArgs e)
        {
            tbMDLAPPSPath.Text = getFolderPath("MDLAPPS Build Path", Environment.SpecialFolder.MyComputer, this.tbMDLAPPSPath.Text);
        }

        private void btnBatchBrowser_Click(object sender, EventArgs e)
        {
            tbBuildPath.Text = getFilePath(tbBuildPath);
        }

        private void btnMDLAPPSLock_Click(object sender, EventArgs e)
        {
            if (btnMDLAPPSLock.Image.Tag.ToString() == nameof(Properties.Resources.OpenedPadlock))
            {
                btnMDLAPPSLock.Image = Properties.Resources.LockedPadlock;
                btnMDLAPPSLock.Image.Tag = nameof(Properties.Resources.LockedPadlock);
                btnMDLAPPSPathBrowser.Enabled = false;
                tbMDLAPPSPath.Enabled = false;
            }
            else
            {
                btnMDLAPPSLock.Image = Properties.Resources.OpenedPadlock;
                btnMDLAPPSLock.Image.Tag = nameof(Properties.Resources.OpenedPadlock);
                btnMDLAPPSPathBrowser.Enabled = true;
                tbMDLAPPSPath.Enabled = true;
            }
            msceOptionsPage.MDLAPPSLock = !msceOptionsPage.MDLAPPSLock;
        }

        private void btnBatchLock_Click(object sender, EventArgs e)
        {
            if (btnBatchLock.Image.Tag.ToString() == nameof(Properties.Resources.OpenedPadlock))
            {
                btnBatchLock.Image = Properties.Resources.LockedPadlock;
                btnBatchLock.Image.Tag = nameof(Properties.Resources.LockedPadlock);
                btnBatchBrowser.Enabled = false;
                tbBuildPath.Enabled = false;
            }
            else
            {
                btnBatchLock.Image = Properties.Resources.OpenedPadlock;
                btnBatchLock.Image.Tag = nameof(Properties.Resources.OpenedPadlock);
                btnBatchBrowser.Enabled = true;
                tbBuildPath.Enabled = true;
            }
            msceOptionsPage.BatchLock = !msceOptionsPage.BatchLock;
        }

        //private void tbMSCEPath_MouseHover(object sender, EventArgs e)
        //{
        //    if (tbMSCEPath.TextLength > 60)
        //    {
        //        toolTip1.Show(tbMSCEPath.Text, tbMSCEPath, 0, 25, 3000);
        //    }
        //}

        //private void tbMSCEPath_MouseLeave(object sender, EventArgs e)
        //{
        //    toolTip1.Hide(tbMSCEPath);
        //}

        //private void tbSDKPath_MouseHover(object sender, EventArgs e)
        //{
        //    if (tbSDKPath.TextLength > 60)
        //    {
        //        toolTip1.Show(tbSDKPath.Text, tbSDKPath, 0, 25, 3000);
        //    }
        //}

        //private void tbSDKPath_MouseLeave(object sender, EventArgs e)
        //{
        //    toolTip1.Hide(tbSDKPath);
        //}

        private void tbMDLAPPSPath_MouseHover(object sender, EventArgs e)
        {
            if (tbMDLAPPSPath.TextLength > 60)
            {
                toolTip1.Show(tbMDLAPPSPath.Text, tbMDLAPPSPath, 0, 25, 3000);
            }
        }

        private void tbMDLAPPSPath_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(tbMDLAPPSPath);
        }

        private void tbBuildPath_MouseHover(object sender, EventArgs e)
        {
            if (tbBuildPath.TextLength > 60)
            {
                toolTip1.Show(tbBuildPath.Text, tbBuildPath, 0, 25, 3000);
            }
        }

        private void tbBuildPath_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(tbBuildPath);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbMSCEPath.Text = BentleyDataCollector.MSCEPath();
            foreach (var item in BentleyDataCollector.BentleyProducts)
            {
                if (tbMSCEPath.Text == item.Value.ToString())
                {
                    cboBentleyProduct.Text = item.Key.ToString();
                }
            }

            tbSDKPath.Text = BentleyDataCollector.GetSDKPath(tbMSCEPath.Text);
            msceOptionsPage.BentleyApp = BentleyDataCollector.GetBentleyApp(tbMSCEPath.Text);
            if (!msceOptionsPage.MDLAPPSLock)
            {
                tbMDLAPPSPath.Text = BentleyDataCollector.GetMdlappsPath(tbMSCEPath.Text);
            }

            if (!msceOptionsPage.BatchLock)
            {
                tbBuildPath.Text = BentleyDataCollector.BentleyBuildBatchFilePath(msceOptionsPage.BentleyApp);
            }
        }

        #endregion

        #region Methods

        private void SetMSCEComboBox()
        {
            cboBentleyProduct.Items.Clear();
            foreach (var item in BentleyDataCollector.BentleyProducts)
            {
                cboBentleyProduct.Items.Add(item.Key.ToString());
            }
            int i = 0;
            foreach (var item in BentleyDataCollector.BentleyProducts)
            {
                if (msceOptionsPage.MSCEPath == item.Value.ToString())
                {
                    cboBentleyProduct.SelectedIndex = i;
                    break;
                }
                i += 1;
            }
        }

        private string getFolderPath(string title, System.Environment.SpecialFolder environment, string path)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = title;
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = environment;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.SelectedPath + "\\";
                }
            }
            return path;
        }

        private string getFilePath(TextBox textBox)
        {
            Stream myStream = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "All files (*.bat)|*.bat";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Title = "Select Bentley build batch file";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                return ((FileStream)myStream).Name;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
                else
                {
                    return textBox.Text;
                }
            }
            return textBox.Text;
        }

        private bool PathExist(TextBox textbox)
        {
            if (!string.IsNullOrEmpty(textbox.Text))
            {
                if (System.IO.Directory.Exists(textbox.Text) && textbox.Text.Substring(textbox.Text.Length - 1, 1) == "\\")
                {
                    errorProvider1.SetError(textbox, string.Empty);
                    textbox.ForeColor = Color.Black;
                    return true;
                }
                else
                {
                    if (textbox.Text != "Not Installed")
                        errorProvider1.SetError(textbox, "Path not Found");
                    else
                    {
                        errorProvider1.SetError(textbox, string.Empty);
                        textbox.ForeColor = Color.Black;
                        return false;
                    }
                    textbox.ForeColor = Color.Red;
                }
            }
            return false;
        }

        private bool isFileExist(TextBox textbox)
        {
            if (!string.IsNullOrEmpty(textbox.Text))
            {
                if (File.Exists(textbox.Text))
                {
                    errorProvider1.SetError(textbox, string.Empty);
                    textbox.ForeColor = Color.Black;
                    return true;
                }
                else
                {
                    if (textbox.Text != "Not Installed")
                        errorProvider1.SetError(textbox, "Path not Found");
                    else
                    {
                        errorProvider1.SetError(textbox, string.Empty);
                        textbox.ForeColor = Color.Black;
                        return false;
                    }
                    textbox.ForeColor = Color.Red;
                }
            }
            return false;
        }

        private void SetMDLPadlockImage()
        {
            if (msceOptionsPage.MDLAPPSLock)
            {
                btnMDLAPPSLock.Image = Properties.Resources.LockedPadlock;
                btnMDLAPPSLock.Image.Tag = nameof(Properties.Resources.LockedPadlock);
                btnMDLAPPSPathBrowser.Enabled = false;
                tbMDLAPPSPath.Enabled = false;
            }
            else
            {
                btnMDLAPPSLock.Image = Properties.Resources.OpenedPadlock;
                btnMDLAPPSLock.Image.Tag = nameof(Properties.Resources.OpenedPadlock);
                btnMDLAPPSPathBrowser.Enabled = true;
                tbMDLAPPSPath.Enabled = true;
            }
        }

        private void SetBatchPadlockImage()
        {
            if (msceOptionsPage.BatchLock)
            {
                btnBatchLock.Image = Properties.Resources.LockedPadlock;
                btnBatchLock.Image.Tag = nameof(Properties.Resources.LockedPadlock);
                btnBatchBrowser.Enabled = false;
                tbBuildPath.Enabled = false;
            }
            else
            {
                btnBatchLock.Image = Properties.Resources.OpenedPadlock;
                btnBatchLock.Image.Tag = nameof(Properties.Resources.OpenedPadlock);
                btnBatchBrowser.Enabled = true;
                tbBuildPath.Enabled = true;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the reference to the underlying OptionsPage object.
        /// </summary>
        public OptionsPage OptionsPage
        {
            get
            {
                return msceOptionsPage;
            }
            set
            {
                msceOptionsPage = value;
            }
        }

        #endregion
    }
}
