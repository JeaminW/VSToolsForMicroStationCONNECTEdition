using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VSTMC
{
    /// <summary>
    /// Extends the standard dialog functionality for implementing ToolsOptions pages,
    /// with support for the Visual Studio automation model, Windows Forms, and state
    /// persistence through the Visual Studio settings mechanism.
    /// </summary>
    [ComVisible(true)]
    [ToolboxItem(false)]
    [Guid(PackageGuids.guidPageString)]
    public class OptionsPage : DialogPage
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        private OptionsControlPage optionsControl;

        #endregion

        #region Constructors

        public OptionsPage()
        {
            MSCEPath = BentleyDataCollector.MSCEPath();
            MSCESDKPath = BentleyDataCollector.GetSDKPath(MSCEPath);
            BentleyApp = BentleyDataCollector.GetBentleyApp(MSCEPath);
            MDLAPPSPath = BentleyDataCollector.GetMdlappsPath(MSCEPath);
            BentleyBuildFilePath = BentleyDataCollector.BentleyBuildBatchFilePath(BentleyApp);
            BatchLock = false;
            MDLAPPSLock = false;
        }

        #endregion

        #region Properties

        [Category("Bentley")]
        [Description("MSCE Installation Path")]
        public string MSCEPath { get; set; }

        [Category("Bentley")]
        [Description("MSCE SDK Installation Path")]
        public string MSCESDKPath { get; set; }

        [Category("Bentley")]
        [Description("MSCE Bentley App")]
        public string BentleyApp { get; set; }

        [Category("Bentley")]
        [Description("MSCE MDLAPPS Path")]
        public string MDLAPPSPath { get; set; }

        [Category("Bentley")]
        [Description("MicroStation CONNECT Bentley Build Path")]
        public string BentleyBuildFilePath { get; set; }

        [Category("Bentley")]
        [Description("Bentley build batch file path lock")]
        public bool BatchLock { get; set; }

        [Category("Bentley")]
        [Description("MDLApps path lock")]
        public bool MDLAPPSLock { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the window an instance of DialogPage that it uses as its user interface.
        /// </summary>
        /// <devdoc>
        /// The window this dialog page will use for its UI.
        /// This window handle must be constant, so if you are
        /// returning a Windows Forms control you must make sure
        /// it does not recreate its handle.  If the window object
        /// implements IComponent it will be sited by the
        /// dialog page so it can get access to global services.
        /// </devdoc>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get
            {
                if (optionsControl == null)
                {
                    optionsControl = new OptionsControlPage();
                    optionsControl.Location = new Point(0, 0);
                    optionsControl.OptionsPage = this;
                    optionsControl.Initialize();
                }
                return optionsControl;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (optionsControl != null)
                {
                    optionsControl.Dispose();
                    optionsControl = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles "activate" messages from the Visual Studio environment.
        /// </summary>
        /// <devdoc>
        /// This method is called when Visual Studio wants to activate this page.
        /// </devdoc>
        /// <remarks>If this handler sets e.Cancel to true, the activation will not occur.</remarks>
        protected override void OnActivate(CancelEventArgs e)
        {

            base.OnActivate(e);
        }

        /// <summary>
        /// Handles "close" messages from the Visual Studio environment.
        /// </summary>
        /// <devdoc>
        /// This event is raised when the page is closed.
        /// </devdoc>
        protected override void OnClosed(EventArgs e)
        {

        }

        /// <summary>
        /// Handles "deactivate" messages from the Visual Studio environment.
        /// </summary>
        /// <devdoc>
        /// This method is called when VS wants to deactivate this
        /// page.  If this handler sets e.Cancel, the deactivation will not occur.
        /// </devdoc>
        /// <remarks>
        /// A "deactivate" message is sent when focus changes to a different page in
        /// the dialog.
        /// </remarks>
        protected override void OnDeactivate(CancelEventArgs e)
        {
            //int result = VsShellUtilities.ShowMessageBox(Site, Properties.Resources.MessageOnDeactivateEntered, null /*title*/, OLEMSGICON.OLEMSGICON_QUERY, OLEMSGBUTTON.OLEMSGBUTTON_OKCANCEL, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            //if (result == (int)VSConstants.MessageBoxResult.IDCANCEL)
            //{
            //    e.Cancel = true;
            //}
        }

        /// <summary>
        /// Handles "apply" messages from the Visual Studio environment.
        /// </summary>
        /// <devdoc>
        /// This method is called when VS wants to save the user's
        /// changes (for example, when the user clicks OK in the dialog).
        /// </devdoc>
        protected override void OnApply(PageApplyEventArgs e)
        {

            base.OnApply(e);
        }

        #endregion Event Handlers
    }
}
