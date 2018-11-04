using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace VSTMC
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class OpenSDKFolderCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4193;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid(PackageGuids.guidPackageCmdSetString);

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenSDKFolderCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private OpenSDKFolderCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
            menuItem.BeforeQueryStatus += menuItem_BeforeQueryStatus;
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Check for command visibility.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            OleMenuCommand menuCommand = sender as OleMenuCommand;
            menuCommand.Visible = Utilities.IsCONNECTProject;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static OpenSDKFolderCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in OpenSDKFolder's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new OpenSDKFolderCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            Utilities utilities = new Utilities();
            try
            {
                if (Directory.Exists(utilities.MSCESDKPath))
                {
                    utilities.OpenFolderLocation(utilities.MSCESDKPath);
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(
                    "Unable to Open Bentley SDK Folder. Verify that it exist.",
                    "Error Opening Folder",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
