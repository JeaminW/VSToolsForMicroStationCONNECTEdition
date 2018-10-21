using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SolutionEvents = Microsoft.VisualStudio.Shell.Events.SolutionEvents;
using Task = System.Threading.Tasks.Task;

namespace VSTMC
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "5.0.0.1", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuids.guidPackageString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionsPage), "Bentley", "MicroStation CONNECT", 100, 102, true, new string[] { "Bentley Developer Network - Bentley CONNECT Edition" })]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class VSTMCPackage : AsyncPackage
    {
        private static OptionsPage _options;
        private static readonly object _syncRoot = new object();

        public static OptionsPage Options
        {
            get
            {
                if (_options == null)
                {
                    lock (_syncRoot)
                    {
                        if (_options == null)
                        {
                            LoadPackage();
                        }
                    }
                }
                return _options;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSTMCPackage"/> class.
        /// </summary>
        public VSTMCPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members
   
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            _options = (OptionsPage)GetDialogPage(typeof(OptionsPage));

            await base.InitializeAsync(cancellationToken, progress);

            SetEnvironment();

            await NativeImportCommand.InitializeAsync(this);
            await OpenFolderCommand.InitializeAsync(this);
            await OpenMdlAppsFolderCommand.InitializeAsync(this);
            await OpenSDKFolderCommand.InitializeAsync(this);
            await SearchBentleyForumsCommand.InitializeAsync(this);
        }

        private static void LoadPackage()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var shell = (IVsShell)GetGlobalService(typeof(SVsShell));

            if (shell.IsPackageLoaded(ref PackageGuids.guidPackage, out IVsPackage package) != VSConstants.S_OK)
                ErrorHandler.Succeeded(shell.LoadPackage(ref PackageGuids.guidPackage, out package));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Set Visual Studio environment MicroStation.
        /// </summary>
        private void SetEnvironment()
        {
            string MSPath = Options.MSCEPath;
            string SDKPath = Options.MSCESDKPath;

            Utilities utilities = new Utilities();

            #region Set environment from options.

            Environment.SetEnvironmentVariable("MSCEPath", MSPath, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("MSCESDKPath", SDKPath, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("MSCEMdlappsPath", Options.MDLAPPSPath, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("MSCEBentleyApp", Options.BentleyApp, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("MSCENativeBuildFile", Options.BentleyBuildFilePath, EnvironmentVariableTarget.Process);

            #endregion

            if (Directory.Exists(MSPath))
            {
                string path = Path.GetDirectoryName(MSPath);
                if (Directory.Exists(SDKPath))
                {
                    #region Set MSCE_Include

                    string msceInclude = SDKPath + "include;" + Environment.GetEnvironmentVariable("Temp") + "\\Bentley\\MicroStationSDK\\objects";

                    if (MSPath.Contains("AECOsim"))
                    {
                        msceInclude += ";" + SDKPath + "ABDSDK\\include";
                    }

                    if (MSPath.Contains("OpenRoads") || MSPath.Contains("OpenRail"))
                    {
                        msceInclude += ";" + SDKPath + "Objects";
                    }

                    Environment.SetEnvironmentVariable("MSCE_IncludePath", msceInclude, EnvironmentVariableTarget.Process);

                    #endregion

                    #region Set MSCE_Library

                    string msceLibrary = SDKPath + "Library";

                    if (MSPath.Contains("AECOsim"))
                        msceLibrary += ";" + SDKPath + "ABDSDK\\Library";

                    Environment.SetEnvironmentVariable("MSCE_LibraryPath", msceLibrary, EnvironmentVariableTarget.Process);
                    #endregion

                    #region Set MSCE_ReferencePaths_x64

                    string msceReferencePaths = MSPath + ";" + MSPath + "Assemblies\\;" + MSPath + "Assemblies\\ECFramework\\";
                    if (Directory.Exists(MSPath + "Map\\bin\\assemblies"))
                        msceReferencePaths += ";" + MSPath + "Map\\bin\\assemblies\\";

                    if (Directory.Exists(MSPath + "Descartes\\Assemblies"))
                        msceReferencePaths += ";" + MSPath + "Descartes\\Assemblies\\";

                    if (Directory.Exists(MSPath + "OpenRoads"))
                        msceReferencePaths += ";" + MSPath + "OpenRoads\\";

                    if (Directory.Exists(MSPath + "Cif"))
                        msceReferencePaths += ";" + MSPath + "Cif\\";

                    if (Directory.Exists(MSPath + "Subsurface"))
                        msceReferencePaths += ";" + MSPath + "Subsurface\\";

                    if (Directory.Exists(MSPath + "Subsurface\\SUDA"))
                        msceReferencePaths += ";" + MSPath + "Subsurface\\SUDA\\";

                    if (Directory.Exists(MSPath + "Assemblies\\Cmf"))
                        msceReferencePaths += ";" + MSPath + "Assemblies\\Cmf\\";

                    if (Directory.Exists(MSPath + "Assemblies\\Ism"))
                        msceReferencePaths += ";" + MSPath + "Assemblies\\Ism\\";

                    if (Directory.Exists(MSPath + "Assemblies\\OpenStaad"))
                        msceReferencePaths += ";" + MSPath + "Assemblies\\OpenStaad\\";

                    if (Directory.Exists(MSPath + "OBMAssemblies"))
                        msceReferencePaths += ";" + MSPath + "OBMAssemblies\\";

                    if (Directory.Exists(MSPath + "DevExpress"))
                        msceReferencePaths += ";" + MSPath + "DevExpress\\";

                    if (Directory.Exists(MSPath + "Assemblies\\Telerik"))
                        msceReferencePaths += ";" + MSPath + "Assemblies\\Telerik\\";

                    if (Directory.Exists(MSPath + "OpenRail"))
                        msceReferencePaths += ";" + MSPath + "OpenRail\\";

                    if (Directory.Exists(MSPath + "OverheadLine"))
                        msceReferencePaths += ";" + MSPath + "OverheadLine\\";

                    Environment.SetEnvironmentVariable("MSCE_ReferencePaths_x64", msceReferencePaths, EnvironmentVariableTarget.Process);

                    #endregion
                }
            }
        }

        #endregion
    }
}
