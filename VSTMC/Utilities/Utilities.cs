using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VSTMC
{
    public class Utilities
    {
        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        public void SearchBentleyForums()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (dte.ActiveDocument != null)
            {
                var selection = (TextSelection)dte.ActiveDocument.Selection;
                if (selection != null)
                    if (!string.IsNullOrEmpty(selection.Text))
                    {
                        ItemOperations itemOp;
                        itemOp = dte.ItemOperations;
                        itemOp.Navigate("https://communities.bentley.com/search?q=" + selection.Text + "#serpsort=date%20desc&serpgroup=444");
                    }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="package"></param>
        public void ImportNativeApps(Package package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.package = package;
            string projectLocation = dte.get_Properties("Environment", "ProjectsAndSolution").Item("ProjectsLocation").Value.ToString();
            string file = GetFilesDialog(
                        "Select CONNECT Edition Make File",
                        "Make File (*.mke)|*.mke",
                        MSCESDKPath);

            if (null != file)
            {
                FileInfo originalFileInfo = new FileInfo(file);
                InitialDirectory = originalFileInfo.DirectoryName;
                string[] pathSplit = originalFileInfo.DirectoryName.Split('\\');
                bool isCanceled = false;
                string solutionFile = null;
                string solutionFileName = null;
                string solutionPath = null;
                if (string.IsNullOrEmpty(dte.Solution.FullName))
                {
                    solutionFile = GetFilesDialog(
                        "Select Solution or Create New Solution to add Project",
                        "Solution (*.sln)|*.sln",
                        projectLocation,
                        originalFileInfo.Name.Replace(originalFileInfo.Extension, "") + ".sln"
                        );
                    if (!string.IsNullOrEmpty(solutionFile))
                    {
                        try
                        {
                            FileInfo solutionFileInfo = new FileInfo(solutionFile);
                            solutionFileName = solutionFileInfo.Name.Replace(".sln", "");
                            solutionPath = solutionFileInfo.DirectoryName + "\\" + solutionFileName;
                            dte.Solution.Open(solutionFile);
                        }
                        catch (Exception)
                        {
                            FileInfo solutionFileInfo = new FileInfo(solutionFile);
                            solutionFileName = solutionFileInfo.Name.Replace(".sln", "");
                            solutionPath = solutionFileInfo.DirectoryName + "\\" + solutionFileName;
                            dte.Solution.Create(solutionPath, solutionFileInfo.Name);
                        }
                    }
                    else
                    {
                        isCanceled = true;
                    }
                }
                else
                {
                    FileInfo solutionFileInfo = new FileInfo(dte.Solution.FullName);
                    solutionFileName = solutionFileInfo.Name.Replace(".sln", "");
                    solutionPath = solutionFileInfo.DirectoryName;
                    solutionFile = dte.Solution.FullName;
                }

                if (!isCanceled)
                {
                    if (!Directory.Exists(solutionPath))
                    {
                        Directory.CreateDirectory(solutionPath);
                        dte.Solution.SaveAs(solutionPath + "\\" + solutionFileName + ".sln");
                    }

                    if (null != solutionFile)
                    {
                        string vsDir = Environment.GetEnvironmentVariable("VisualStudioDir");
                        string destinationDir = solutionPath + "\\" + originalFileInfo.Name.Replace(originalFileInfo.Extension, "") + "\\";

                        if (!Directory.Exists(destinationDir))
                        {
                            Directory.CreateDirectory(destinationDir);

                            // Get our files (recursive and any of them, based on the 2nd param of the Directory.GetFiles() method
                            string[] originalFiles = Directory.GetFiles(originalFileInfo.DirectoryName, "*", SearchOption.AllDirectories);
                            Array.ForEach(originalFiles, (originalFileLocation) =>
                            {
                                // Get the FileInfo for both of our files
                                FileInfo originalFile = new FileInfo(originalFileLocation);
                                FileInfo destFile = new FileInfo(originalFileLocation.Replace(originalFileInfo.DirectoryName, destinationDir));
                                // ^^ We can fill the FileInfo() constructor with files that don't exist...

                                // ... because we check it here
                                if (destFile.Exists)
                                {
                                    // Logic for files that exist applied here; if the original is larger, replace the updated files...
                                    if (originalFile.Length > destFile.Length)
                                    {
                                        originalFile.CopyTo(destFile.FullName, true);
                                    }
                                }
                                else // ... otherwise create any missing directories and copy the folder over
                                {
                                    Directory.CreateDirectory(destFile.DirectoryName); // Does nothing on directories that already exist
                                    originalFile.CopyTo(destFile.FullName, false); // Copy but don't over-write
                                }
                            }
                            );

                            DirectoryInfo directoryInfo = new DirectoryInfo(destinationDir);

                            string result = Properties.Resources.CONNECTvcproj;
                            string resultFilters = Properties.Resources.CONNECTFilters;
                            result = result
                                .Replace("$safeprojectname$", pathSplit[pathSplit.GetUpperBound(0)])
                                .Replace("$guid1$", "{" + Guid.NewGuid().ToString() + "}")
                                .Replace("$platformtoolset$", PlatFormToolSet(dte.Version));

                            resultFilters = resultFilters
                               .Replace("$guid1$", "{" + Guid.NewGuid().ToString() + "}")
                               .Replace("$guid2$", "{" + Guid.NewGuid().ToString() + "}")
                               .Replace("$guid3$", "{" + Guid.NewGuid().ToString() + "}");

                            foreach (var item in directoryInfo.GetFiles("*", SearchOption.AllDirectories).OrderBy(f => f.Extension))
                            {
                                if (item.Extension.ToLower() == ".cpp" || item.Extension.ToLower() == ".h")
                                {
                                    result += "<ClCompile Include=";
                                }
                                else
                                {
                                    result += "<ClInclude Include=";
                                }
                                result += "\"" + item.FullName.Replace(destinationDir, "") + "\"/>\n";
                            }
                            if (directoryInfo.GetDirectories().Count() >= 0)
                            {
                                foreach (var directories in directoryInfo.GetDirectories())
                                {
                                    resultFilters += "<ItemGroup>\n<Filter Include=\"" + directories.Name + "\">\n" +
                                                "<UniqueIdentifier>{" + Guid.NewGuid().ToString() + "}</UniqueIdentifier>\n" +
                                                "</Filter>\n" +
                                                "</ItemGroup>\n";

                                    foreach (var files in directoryInfo.GetFiles("*", SearchOption.AllDirectories).OrderBy(f => f.Extension))
                                    {
                                        if (files.FullName.Contains(directories.Name))
                                        {
                                            if (files.Extension.ToLower() == ".cpp" || files.Extension.ToLower() == ".h")
                                            {
                                                resultFilters += "<ItemGroup>\n" +
                                                                "<ClCompile Include=\"" + files.FullName.Replace(destinationDir, "") + "\">\n" +
                                                                "<Filter>" + directories.Name + "</Filter>\n" +
                                                                "</ClCompile>\n" +
                                                                "</ItemGroup>\n";
                                            }
                                            else if (files.Extension.ToLower() == ".r" || files.Extension.ToLower() == ".rc")
                                            {
                                                resultFilters += "<ItemGroup>\n" +
                                                                "<ClInclude Include=\"" + files.FullName.Replace(destinationDir, "") + "\">\n" +
                                                                "<Filter>" + directories.Name + "</Filter>\n" +
                                                                "</ClInclude>\n" +
                                                                "</ItemGroup>\n";
                                            }
                                        }
                                    }
                                }
                            }
                            resultFilters += "</Project>\n";
                            result += "</ItemGroup >\n" +
                            "<Import Project = \"$(VCTargetsPath)\\Microsoft.Cpp.targets\"/>\n" +
                            "<ImportGroup Label = \"ExtensionTargets\" >\n" +
                            "</ImportGroup>\n" +
                            "</Project>\n";

                            string projectFile = destinationDir + originalFileInfo.Name.Replace(originalFileInfo.Extension, "") + ".vcxproj";
                            string filterFile = destinationDir + originalFileInfo.Name.Replace(originalFileInfo.Extension, "") + ".vcxproj.filters";

                            File.WriteAllText(projectFile, result);
                            File.WriteAllText(filterFile, resultFilters);
                            UpdateStatusBar(originalFileInfo.Name.Replace(originalFileInfo.Extension, "") + " successfully upgraded to Visual Studio Tools format.");
                            dte.Solution.AddFromFile(projectFile);
                        }
                        else
                            UpdateStatusBar(originalFileInfo.Name.Replace(originalFileInfo.Extension, "") + " project creation failed: Project already exist in Visual Studio projects folder.");
                    }
                }

            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get files dialog box.
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="filter"></param>
        /// <param name="initialDirectory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFilesDialog(string Title, string filter, string initialDirectory, string fileName = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = Title;
            openFileDialog.CheckFileExists = false;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = filter;
            if (!string.IsNullOrEmpty(fileName))
            {
                openFileDialog.InitialDirectory = initialDirectory;
                openFileDialog.FileName = fileName;
            }
            else
            {
                if (string.IsNullOrEmpty(InitialDirectory))
                {
                    InitialDirectory = MSCESDKPath;
                }
                openFileDialog.InitialDirectory = InitialDirectory;
            }

            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Update status bar.
        /// </summary>
        /// <param name="message"></param>
        private void UpdateStatusBar(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IVsStatusbar statusBar = (IVsStatusbar)ServiceProvider.GetService(typeof(SVsStatusbar));
            Assumes.Present(statusBar);

            // Make sure the status bar is not frozen

            statusBar.IsFrozen(out int frozen);

            if (frozen != 0)
            {
                statusBar.FreezeOutput(0);
            }

            // Set the status bar text and make its display static.
            statusBar.SetText(message);

            // Freeze the status bar.
            statusBar.FreezeOutput(1);

            // Clear the status bar text.
            statusBar.FreezeOutput(0);
            statusBar.Clear();
        }
        #endregion

        #region Public Properties

        // Summary:
        //     Gets the active document project.
        //
        // Returns:
        //     Returns the active document project.
        public Project ActiveDocumentProject { get { ThreadHelper.ThrowIfNotOnUIThread(); return dte.ActiveDocument.ProjectItem.ContainingProject; } }

        // Summary:
        //     Gets the active project.
        //
        // Returns:
        //     Returns the active project.
        public Project ActiveProject { get { ThreadHelper.ThrowIfNotOnUIThread(); return (Project)ActiveSolutionProjects.GetValue(0); } }

        /// <summary>
        /// (Read-only) Determine if the active document is in a Bentley project.
        /// Returns true if the active document is in a Bentley project, otherwise returns false.
        /// </summary>
        public bool IsActiveDocumentBentleyProject
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                if (null != ActiveProject)
                {
                    using (StreamReader sr = new StreamReader(ActiveDocumentProject.FileName))
                    {
                        if (sr.ReadToEnd().ToLower().Contains("bentley"))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// (Read-only) Determine if project is a Bentley project.
        /// Returns true if project is a Bentley project, otherwise returns false.
        /// </summary>
        public bool IsCONNECTProject
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                if (null != ActiveProject)
                {
                    using (StreamReader sr = new StreamReader(ActiveProject.FileName))
                    {
                        if (sr.ReadToEnd().ToLower().Contains("msce"))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Get Bentley application path. Returns string of the Bentley product.
        /// </summary>
        public string MSCEPath
        {
            get
            {
                return Environment.GetEnvironmentVariable("MSCEPath");
            }
        }

        /// <summary>
        /// Get Bentley SDK path. Returns string of the Bentley product.
        /// </summary>
        public string MSCESDKPath
        {
            get
            {
                return Environment.GetEnvironmentVariable("MSCESDKPath");
            }
        }

        /// <summary>
        /// Get Bentley MDL path. Returns string of the Bentley product.
        /// </summary>
        public string MSCEMDLPath
        {
            get
            {
                return Environment.GetEnvironmentVariable("MSCEMdlappsPath");
            }
        }

        /// <summary>
        /// Open folder location.
        /// </summary>
        /// <param name="folderLocation"></param>
        public void OpenFolderLocation(String folderLocation)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo()
            {
                FileName = folderLocation,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Get or set Visual Studio environment DTE (EnvDTE.DTE).
        /// </summary>
        private DTE dte
        {
            get { ThreadHelper.ThrowIfNotOnUIThread(); return Package.GetGlobalService(typeof(SDTE)) as DTE; }
        }

        /// <summary>
        /// Get ActiveSolutionProjects. Read only.
        /// </summary>
        private Array ActiveSolutionProjects { get { ThreadHelper.ThrowIfNotOnUIThread(); return (Array)dte.ActiveSolutionProjects; } }

        /// <summary>
        /// Set or get initial directory
        /// </summary>
        private string InitialDirectory { get; set; }

        /// <summary>
        /// Get platformtoolset. Read only.
        /// </summary>
        private string PlatFormToolSet(string version)
        {
            double platformtoolset = 0.00;
            string platformtool = "";
            Double.TryParse(version, out platformtoolset);
            if (platformtoolset == 0.00)
                platformtool = "120";
            else
                platformtool = (platformtoolset * 10).ToString().Trim();

            return platformtool;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private System.IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private Package package { get; set; }

        #endregion
    }
}
