using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Task = System.Threading.Tasks.Task;

namespace VSTMC
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class BentleyHelpCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4225;
        public const int ListCommandId = 4195;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid(PackageGuids.guidPackageCmdSetString);

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyHelpCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private BentleyHelpCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            // NOTE: For further explanantions of the various types of combos and their differences see the .vsct file where they are declared.
            //
            //   A DropDownCombo combobox requires two commands:
            //     One command (cmdidMyCombo) is used to ask for the current value for the display area of the combo box
            //     and to set the new value when the user makes a choice in the combo box.
            //
            //     The second command (cmdidMyComboGetList) is used to retrieve the list of choices for the combo box drop
            //     down area.
            //
            // Normally IOleCommandTarget::QueryStatus is used to determine the state of a command, e.g.
            // enable vs. disable, shown vs. hidden, etc. The QueryStatus method does not have enough
            // flexibility for combos which need to be able to indicate a currently selected (displayed)
            // item as well as provide a list of items for their dropdown area. In order to communicate
            // this information actually IOleCommandTarget::Exec is used with a non-NULL varOut parameter.
            // You can think of these Exec calls as extended QueryStatus calls. There are two pieces of
            // information needed for a combo, thus it takes two commands to retrieve this information.
            // The main command id for the command is used to retrieve the current value and the second
            // command is used to retrieve the full list of choices to be displayed as an array of strings.
            CommandID menuMyDropDownComboCommandID = new CommandID(CommandSet, CommandId);
            OleMenuCommand menuMyDropDownComboCommand = new OleMenuCommand(new EventHandler(OnMenuMyDropDownCombo), menuMyDropDownComboCommandID);
            menuMyDropDownComboCommand.BeforeQueryStatus += menuMyDropDownComboCommand_BeforeQueryStatus;
            commandService.AddCommand(menuMyDropDownComboCommand);

            CommandID menuMyDropDownComboGetListCommandID = new CommandID(CommandSet, ListCommandId);
            MenuCommand menuMyDropDownComboGetListCommand = new OleMenuCommand(new EventHandler(OnMenuMyDropDownComboGetList), menuMyDropDownComboGetListCommandID);
            commandService.AddCommand(menuMyDropDownComboGetListCommand);
        }

        /// <summary>
        /// Check for command visibility.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuMyDropDownComboCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            OleMenuCommand menuCommand = sender as OleMenuCommand;
            menuCommand.Visible = IsValidHelpPath;
        }

        public bool IsValidHelpPath
        {
            get
            {
                string SDKPath = Environment.GetEnvironmentVariable("MSCESDKPath", EnvironmentVariableTarget.Process);

                if (string.IsNullOrEmpty(SDKPath))
                {
                    return false;
                }
                if (GetBentleySDKHelpFiles().Length == 1 || string.IsNullOrEmpty(GetBentleySDKHelpFiles()[0]))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static BentleyHelpCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in BentleyHelpCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new BentleyHelpCommand(package, commandService);
        }

        #region Combo Box Commands
        private static string[] dropDownComboChoices = GetBentleySDKHelpFiles();
        private string currentDropDownComboChoice = dropDownComboChoices[0];

        /// <summary>
        /// Get Bentley SDK help files.
        /// </summary>
        /// <returns></returns>
        private static string[] GetBentleySDKHelpFiles()
        {
            string SDKPath = Environment.GetEnvironmentVariable("MSCESDKPath", EnvironmentVariableTarget.Process);
            List<string> files = new List<string>();
            bool isValidDocDirectory = true;
            bool isValidDocumentationDirectory = true;

            if (Directory.Exists(SDKPath + "\\Doc\\"))
            {
                foreach (var file in Directory.GetFiles(SDKPath + "\\Doc\\", "*.chm"))
                {
                    files.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            else
                isValidDocDirectory = false;

            if (Directory.Exists(SDKPath + "\\Documentation\\"))
            {
                foreach (var file in Directory.GetFiles(SDKPath + "\\Documentation\\", "*.chm"))
                {
                    files.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            else
                isValidDocumentationDirectory = false;

            if (!isValidDocDirectory && !isValidDocumentationDirectory)
            {
                string[] emptyString = { "" };
                return emptyString;
            }

            return files.ToArray();
        }

        /// <summary>
        /// Event handler when selecting item from dropdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMenuMyDropDownCombo(object sender, EventArgs e)
        {
            OleMenuCmdEventArgs eventArgs = e as OleMenuCmdEventArgs;

            if (eventArgs != null)
            {
                string newChoice = eventArgs.InValue as string;
                IntPtr vOut = eventArgs.OutValue;

                if (vOut != IntPtr.Zero)
                {
                    // when vOut is non-NULL, the IDE is requesting the current value for the combo
                    Marshal.GetNativeVariantForObject(currentDropDownComboChoice, vOut);
                }

                else if (newChoice != null)
                {
                    // new value was selected or typed in
                    // see if it is one of our items
                    bool validInput = false;
                    int indexInput = -1;
                    for (indexInput = 0; indexInput < dropDownComboChoices.Length; indexInput++)
                    {
                        if (string.Compare(dropDownComboChoices[indexInput], newChoice, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            validInput = true;
                            break;
                        }
                    }

                    if (validInput)
                    {
                        currentDropDownComboChoice = dropDownComboChoices[indexInput];
                        string SDKPath = Environment.GetEnvironmentVariable("MSCESDKPath", EnvironmentVariableTarget.Process);
                        if (File.Exists(SDKPath + "\\Doc\\" + currentDropDownComboChoice + ".chm"))
                            System.Windows.Forms.Help.ShowHelp(new System.Windows.Forms.Button(), SDKPath + "\\Doc\\" + currentDropDownComboChoice + ".chm");
                        else if (File.Exists(SDKPath + "\\Documentation\\" + currentDropDownComboChoice + ".chm"))
                            System.Windows.Forms.Help.ShowHelp(new System.Windows.Forms.Button(), SDKPath + "\\Documentation\\" + currentDropDownComboChoice + ".chm");
                    }
                    else
                    {
                        throw (new ArgumentException("Parameter must be valid string in list")); // force an exception to be thrown
                    }
                }
            }
            else
            {
                // We should never get here; EventArgs are required.
                throw (new ArgumentException("EventArgs are required")); // force an exception to be thrown
            }
        }

        /// <summary>
        ///  Event handler to get the ComboBox list items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMenuMyDropDownComboGetList(object sender, EventArgs e)
        {
            OleMenuCmdEventArgs eventArgs = e as OleMenuCmdEventArgs;

            if (eventArgs != null)
            {
                object inParam = eventArgs.InValue;
                IntPtr vOut = eventArgs.OutValue;

                if (inParam != null)
                {
                    throw (new ArgumentException("In parameter may not be specified")); // force an exception to be thrown
                }
                else if (vOut != IntPtr.Zero)
                {
                    Marshal.GetNativeVariantForObject(dropDownComboChoices, vOut);
                }
                else
                {
                    throw (new ArgumentException("Out parameter can not be NULL")); // force an exception to be thrown
                }
            }

        }

        #endregion
    }
}
