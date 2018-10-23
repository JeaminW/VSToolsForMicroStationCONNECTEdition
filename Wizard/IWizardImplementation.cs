using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]
namespace innovoCAD.Bentley.CONNECT
{
    public class IWizardImplementation : IWizard
    {
        #region IWizard Methods

        /// <summary>
        /// This method is called before opening any item that
        /// has the OpenInEditor attribute.
        /// </summary>
        /// <param name="projectItem"></param>
        [CLSCompliant(false)]
        public void BeforeOpeningFile(ProjectItem projectItem)
        {

        }

        /// <summary>
        /// This method is called when the project is finished
        /// generating.
        /// </summary>
        /// <param name="project"></param>
        [CLSCompliant(false)]
        public void ProjectFinishedGenerating(Project project)
        {

        }

        /// <summary>
        /// This method is only called for item templates,
        /// not for project templates.
        /// </summary>
        /// <param name="projectItem"></param>
        [CLSCompliant(false)]
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// This method is called after the project is created.
        /// </summary>
        public void RunFinished()
        {
            if (IsNewItem)
            {
                // Create function name from project items
                FunctionName = ReplacementsDictionary["$safeitemname$"];

                if (IsItemAddedToKeyins)
                {
                    AddCommand();
                }
                ActiveProject.Save();
            }
            //else
            //    RequestRestart();

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// This method is called after the project is started.
        /// </summary>
        /// <param name="automationObject"></param>
        /// <param name="replacementsDictionary"></param>
        /// <param name="runKind"></param>
        /// <param name="customParams"></param>
        [CLSCompliant(false)]
        public void RunStarted(object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            ReplacementsDictionary = replacementsDictionary;

            // Make sure default namespace has no space or dash
            if (ReplacementsDictionary.ContainsKey("$safeprojectname$"))
            {
                ReplacementsDictionary["$safeprojectname$"] =
                    ReplacementsDictionary["$safeprojectname$"].
                    Replace(" ", "_").Replace("-", "_").Replace("+", "_");
            }

            //Get project information
            ProjectFileInfo = new FileInfo(customParams[0].ToString());

            // Check if this a project item or a project.
            IsNewItem = (runKind.ToString() == "AsNewItem");

            Dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

            IsAllowed = true;
            IsItemAddedToKeyins = false;

            //Activates and ensures Solution Explorer is visible.
            Dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Activate();
            Dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Visible = true;
            if (IsNewItem)
                ProcessNewItem();
            else
                ProcessNewProject(ProjectFileInfo.Name);

        }

        /// <summary>
        /// This method is only called for item templates,
        /// not for project templates.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool ShouldAddProjectItem(string filePath)
        {
            if (IsNewItem)
            {
                try
                {
                    foreach (ProjectItem item in ActiveProject.ProjectItems)
                    {
                        if (item.Name == filePath)
                            return false;
                    }
                }
                catch (Exception)
                {
                    //Do Nothing
                }
            }
            return true;
        }

        #endregion

        #region Helper Methods

        private void ProcessNewItem()
        {
            ProcessProjectItemTemplates();
        }

        private void ProcessNewProject(string projectFileName)
        {
            if (projectFileName == "MSCE.Keyin.vstemplate" || projectFileName== "MSCE.Addin.vstemplate")
            {
                ProcessEmbeddedResource();
            }

            ReplacementsDictionary.Add("$ToolVersion$", Version);
            ReplacementsDictionary.Add("$platformtoolset$", PlatFormToolSet);
            ReplacementsDictionary.Add("$AddinAttribute$", GetAddinAttribute());
            ReplacementsDictionary.Add("$reference$", GetReferences(projectFileName));
        }

        private void ProcessEmbeddedResource()
        {
            string[] embeddedResources = Assembly.GetAssembly(typeof(IWizardImplementation)).GetManifestResourceNames();
            var assembly = Assembly.GetExecutingAssembly();

            // Build the string of resources.
            foreach (string resource in embeddedResources)
            {
                //PDebug(GetCurrentMethod());
                using (Stream stream = assembly.GetManifestResourceStream(resource))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string bentleyApp = BentleyApp;
                    string result = "";
                    if (bentleyApp.Contains("OpenRoads") || bentleyApp.Contains("OpenRail"))
                    {
                        if (resource.Contains("{F7AD6FF5-34E0-4FBE-B5CF}"))
                            result = GetSubstituteResult("innovoCAD.Bentley.CONNECT.EmbeddedResources.MicroStationUtilities.{F7AD6FF5-34E0-4FBE-ORD}.cpp");
                        else if (resource.Contains("{FAD22EAD-82A1-4DB6-9718-B91226D9A42A}"))
                            result = GetSubstituteResult("innovoCAD.Bentley.CONNECT.EmbeddedResources.native.{FAD22EAD-82A1-4DB6-9718-B91226D9-ORD}.h");
                        //else
                        //    result = reader.ReadToEnd();
                    }
                    else
                    {
                        if (resource.Contains("{F7AD6FF5-34E0-4FBE-B5CF}"))
                            result = GetSubstituteResult("innovoCAD.Bentley.CONNECT.EmbeddedResources.MicroStationUtilities.{F7AD6FF5-34E0-4FBE-B5CF}.cpp");
                        else if (resource.Contains("{FAD22EAD-82A1-4DB6-9718-B91226D9A42A}"))
                            result = GetSubstituteResult("innovoCAD.Bentley.CONNECT.EmbeddedResources.native.{FAD22EAD-82A1-4DB6-9718-B91226D9A42A}.h");
                    }
                    //result = reader.ReadToEnd();

                    string[] resourceItemCollection = resource.Split('.');
                    foreach (var resourceItem in resourceItemCollection)
                    {
                        if (resourceItem.Contains("{"))
                            if (!ReplacementsDictionary.ContainsKey("$" + resourceItem + "$"))
                            {
                                ReplacementsDictionary.Add("$" + resourceItem + "$", result);
                            }
                    }
                }
            }
        }

        private string GetSubstituteResult(string resource)
        {
            try
            {
                string[] embeddedResources = Assembly.GetAssembly(typeof(IWizardImplementation)).GetManifestResourceNames();
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(resource))
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message + "/n" + ex.StackTrace + "/n" + ex.InnerException,"Bentley CONNECT Edition Template",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return ex.Message;
            }
        }

        /// <summary>
        /// Helper method to process BackOutException.
        /// </summary>
        private void BackOutException(int type)
        {
            try
            {
                throw new WizardBackoutException(BackoutError);
            }
            catch (WizardBackoutException ex)
            {
                //Display exception message to user.
                MessageBox.Show(ex.Message, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                DeleteSolution();

                throw;
            }
            catch (Exception)
            {
                // Display other exception message.
            }
        }

        private void DeleteSolution()
        {
            // Try to delete project file.
            try
            {
                Directory.Delete(DestinationDirectory);
            }
            catch (Exception)
            {
                // If it fails (does not exist/contains files/read-only), let the directory stay.
            }

            // Try to delete solution folder. Should occur if no other projects exist in solution. This should stop incremental
            // increase in solution name in New Project dialog box.
            try
            {
                Directory.Delete(SolutionDirectory);
            }
            catch (Exception)
            {
                // If it fails (does not exist/contains files/read-only), let the directory stay.
            }
            dd
        }

        /// <summary>
        /// Helper method to add locate command, placement command, toolbar, winforms, and wpf function name to
        /// KeyinCommands.cs or KeyinCommands.vb and commands.xml.
        /// </summary>
        private void AddCommand()
        {
            foreach (ProjectItem projectItem in ActiveProject.ProjectItems)
            {
                try
                {
                    Window objWindow;
                    Boolean bHasOpenWindow;
                    bHasOpenWindow = projectItem.IsOpen;

                    if (
                        projectItem.Name.ToLower() == "commands.xml" ||
                        projectItem.Name.ToLower() == "keyincommands.cs" ||
                        projectItem.Name.ToLower() == "keyincommands.vb" ||
                        projectItem.Name.ToLower() == "keyincommands.cpp")
                    {
                        if (!bHasOpenWindow)
                        {
                            objWindow = projectItem.Open(EnvDTE.Constants.vsViewKindCode);
                        }
                    }

                    Document document;
                    if (projectItem.Name != "Properties" && projectItem.Name != "My Project")
                    {
                        document = projectItem.Document;
                        if (document != null)
                        {
                            if (document.Name.ToLower() == "commands.xml")
                            {
                                ModifyCommandTable(projectItem);
                            }
                            else if
                                (document.Name.ToLower() == "keyincommands.cs" ||
                                document.Name.ToLower() == "keyincommands.vb" ||
                                document.Name.ToLower() == "keyincommands.cpp")
                            {
                                string keyinCommandFunctionCS = "";
                                string keyinCommandFunctionVB = "";
                                string keyinCommandFunctionCPP = "";
                                switch (Item)
                                {
                                    case "MicroStation Selection Tool":
                                        keyinCommandFunctionCS = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Selection Tool Settings":
                                        keyinCommandFunctionCS = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Selection Tool Settings WPF":
                                        keyinCommandFunctionCS = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation IPrimitive Tool Settings":
                                        keyinCommandFunctionCS = FunctionCode(FunctionName, "tool", "Run(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode(FunctionName, "tool", "Run(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation IPrimitive Tool Settings WPF":
                                        keyinCommandFunctionCS = FunctionCode(FunctionName, "tool", "Run(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode(FunctionName, "tool", "Run(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Placement Tool":
                                        keyinCommandFunctionCS = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Placement Tool Settings":
                                        keyinCommandFunctionCS = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Placement Tool Settings WPF":
                                        keyinCommandFunctionCS = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Tool Settings WPF":
                                        keyinCommandFunctionCS = FunctionCode(FunctionName, "tool", "ShowWindow(Program.Addin, unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode(FunctionName, "tool", "ShowWindow(Program.Addin, unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Tool Settings":
                                        keyinCommandFunctionCS = FunctionCode(FunctionName, "tool", "ShowForm(Program.Addin, unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode(FunctionName, "tool", "ShowForm(Program.Addin, unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Toolbar WPF":
                                        keyinCommandFunctionCS = FunctionCode(FunctionName, "toolbar", "ShowToolbar(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode(FunctionName, "toolbar", "ShowToolbar(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Windows Form":
                                        keyinCommandFunctionCS = FunctionCode(FunctionName, "form", "ShowForm(unparsed)", Lang.CS);
                                        keyinCommandFunctionCPP = FunctionName + "::ShowForm(Program::MSAddin);";
                                        keyinCommandFunctionVB = FunctionCode(FunctionName, "form", "ShowForm(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation UserControl (WPF)":
                                        keyinCommandFunctionCS = FunctionCode(FunctionName, "usercontrol", "ShowWindow(unparsed)", Lang.CS);
                                        keyinCommandFunctionVB = FunctionCode(FunctionName, "usercontrol", "ShowWindow(unparsed)", Lang.VB);
                                        break;
                                    case "MicroStation Class":
                                        keyinCommandFunctionCS = FunctionName + " " + FunctionName.ToLower() + " = new " + FunctionName + "();";
                                        keyinCommandFunctionVB = "Dim " + FunctionName.ToLower() + " As " + FunctionName + " =  new " + FunctionName + "()";
                                        break;
                                    default:
                                        break;
                                }
                                ModifyKeyinsCommands(projectItem,
                                    keyinCommandFunctionCS,
                                    keyinCommandFunctionVB,
                                    keyinCommandFunctionCPP);
                            }
                        }
                        ActiveProject.Save();
                    }
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
        }

        private enum Lang
        {
            CS,
            VB
        }

        /// <summary>
        /// Get function code for MicroStation CONNECT Edition Iprimitive Tool, Windows Form and WPF Tool Settings and applications.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string FunctionCode(string functionName, string obj, string method, Lang lang)
        {
            switch (lang)
            {
                case Lang.CS:
                    return functionName + " " + obj + " = new " + functionName + "();\n" + obj + "." + method + ";";
                case Lang.VB:
                    return "Dim " + obj + " As " + functionName + " = New " + functionName + "\n" + obj + "." + method;
                default:
                    return functionName + " " + obj + " = new " + functionName + "();\n" + obj + "." + method + ";";
            }
        }

        /// <summary>
        /// Get function code for MicroStation CONNECT Edition Selection and Placement Tool.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private static string FunctionCode2(string functionName, string obj, string method, Lang lang)
        {
            switch (lang)
            {
                case Lang.CS:
                    return functionName + " " + obj + " = new " + functionName + "(0, 0);\n" + obj + "." + method + ";";
                case Lang.VB:
                    return "Dim " + obj + " As " + functionName + " = New " + functionName + "(0, 0)\n" + obj + "." + method;
                default:
                    return functionName + " " + obj + " = new " + functionName + "(0, 0);\n" + obj + "." + method + ";";
            }
        }

        /// <summary>
        /// Helper method to modify commands.xml.
        /// </summary>
        /// <param name="projectItem"></param>
        private void ModifyCommandTable(ProjectItem projectItem)
        {
            Document activeDoc = projectItem.Document;
            projectItem.Open();
            TextDocument editDoc = (TextDocument)activeDoc.Object("TextDocument");
            EditPoint objEditPt = editDoc.CreateEditPoint();
            EditPoint objMovePt = editDoc.EndPoint.CreateEditPoint();
            objEditPt.StartOfDocument();
            activeDoc.ReadOnly = false;

            objEditPt.FindPattern("</KeyinTable>");
            objEditPt.Insert("\n");
            objEditPt.LineUp(1);
            objEditPt.Indent(objEditPt, 1);
            objEditPt.Insert("<Keyword CommandWord=\"" + FunctionName + "\"></Keyword>");
            objEditPt.LineDown(1);
            objEditPt.Indent(objEditPt, 2);

            objEditPt.FindPattern("</KeyinHandlers>");
            objEditPt.Insert("\n");
            objEditPt.LineUp(1);
            objEditPt.Indent(objEditPt, 1);
            objEditPt.Insert("<KeyinHandler Keyin=\"" + RootNamespace + " " + FunctionName + "\"\n");
            objEditPt.Indent(objEditPt, 4);

            if (!IsVBProject)
                objEditPt.Insert("Function=\"" + RootNamespace + ".KeyinCommands." + FunctionName + "Keyin\"/>");
            else
                objEditPt.Insert("Function=\"" + RootNamespace + ".KeyinCommands." + FunctionName + "Keyin\"/>");
        }

        /// <summary>
        /// Help method to modify KeyinCommands.cs or KeyinCommands.vb.
        /// </summary>
        /// <param name="projectItem"></param>
        /// <param name="keyinCommandFunctionCS"></param>
        /// <param name="keyinCommandFunctionvb"></param>
        private void ModifyKeyinsCommands(ProjectItem projectItem,
            string keyinCommandFunctionCS,
            string keyinCommandFunctionvb,
            string keyinCommandFunctionCPP)
        {
            Document activeDoc = projectItem.Document;

            if (activeDoc == null)
                return;
            ProjectItem activeDocumentProjectItem = activeDoc.ProjectItem;
            if (activeDocumentProjectItem == null)
                return;
            FileCodeModel fileCodeModel = activeDocumentProjectItem.FileCodeModel;
            if (fileCodeModel == null)
                return;

            CodeElements codeElements = fileCodeModel.CodeElements;
            CodeClass codeClass = null;

            // look for the namespace in the active document
            CodeNamespace codeNamespace = null;
            foreach (CodeElement codeElement in codeElements)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementNamespace)
                {
                    codeNamespace = codeElement as CodeNamespace;
                    break;
                }
            }
            if (codeNamespace == null)
                if (IsVBProject)
                {
                    codeElements = fileCodeModel.CodeElements;
                }
                else
                {
                    return;
                }
            else
                codeElements = codeNamespace.Members;

            if (codeElements == null)
                return;

            // look for the first class
            foreach (CodeElement codeElement in codeElements)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementClass)
                {
                    codeClass = codeElement as CodeClass;
                    break;
                }
            }
            if (codeClass == null)
                return;

            if (IsCSProject)
            {
                CodeFunction codeFunction = codeClass.AddFunction(FunctionName + "Keyin", vsCMFunction.vsCMFunctionFunction, vsCMTypeRef.vsCMTypeRefVoid, -1, vsCMAccess.vsCMAccessPublic);
                codeFunction.AddParameter("unparsed", vsCMTypeRef.vsCMTypeRefString, -1);
                TextPoint textPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartBody);
                EditPoint editPoint = textPoint.CreateEditPoint();
                EditPoint objMovePt = textPoint.CreateEditPoint();
                EditPoint UtilEditPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartHeader).CreateEditPoint();
                UtilEditPoint.ReplaceText(6, "public static", 0);
                editPoint.Insert
                    (
                        keyinCommandFunctionCS
                    );
                editPoint.StartOfDocument();
                objMovePt.EndOfDocument();
                editPoint.SmartFormat(objMovePt);
            }
            else if (IsVBProject)
            {
                CodeFunction codeFunction = codeClass.AddFunction(FunctionName + "Keyin", vsCMFunction.vsCMFunctionSub, vsCMTypeRef.vsCMTypeRefVoid, -1, vsCMAccess.vsCMAccessPublic);
                codeFunction.AddParameter("unparsed", vsCMTypeRef.vsCMTypeRefString, -1);
                TextPoint textPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartBody);
                EditPoint editPoint = textPoint.CreateEditPoint();
                EditPoint objMovePt = textPoint.CreateEditPoint();
                EditPoint UtilEditPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartHeader).CreateEditPoint();
                UtilEditPoint.ReplaceText(6, "Public Shared", 0);
                editPoint.Insert
                (
                    keyinCommandFunctionvb
                );
                editPoint.StartOfDocument();
                objMovePt.EndOfDocument();
                editPoint.SmartFormat(objMovePt);
            }
            else if (IsVCProject)
            {
                TextDocument editDoc = (TextDocument)activeDoc.Object("TextDocument");
                EditPoint objEditPt = editDoc.CreateEditPoint();
                EditPoint objMovePt = editDoc.EndPoint.CreateEditPoint();
                objEditPt.StartOfDocument();
                activeDoc.ReadOnly = false;

                if (objEditPt.FindPattern("#include"))
                {
                    objEditPt.LineDown(1);
                    objEditPt.Insert("#include \"" + FunctionName + ".h\"\n");
                }
                else if ((objEditPt.FindPattern("#using")))
                {
                    objEditPt.LineUp(1);
                    objEditPt.Insert("#include \"" + FunctionName + ".h\"\n");
                }
                else
                {
                    objEditPt.FindPattern("namespace");
                    objEditPt.LineUp(1);
                    objEditPt.Insert("#include \"" + FunctionName + ".h\"\n");
                }

                CodeFunction codeFunction = codeClass.AddFunction(FunctionName + "Keyin", vsCMFunction.vsCMFunctionFunction, vsCMTypeRef.vsCMTypeRefVoid, -1, vsCMAccess.vsCMAccessPublic);
                codeFunction.AddParameter("unparsed", "System::String^", -1);
                TextPoint textPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartBody);
                EditPoint editPoint = textPoint.CreateEditPoint();
                objMovePt = textPoint.CreateEditPoint();
                EditPoint UtilEditPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartHeader).CreateEditPoint();
                UtilEditPoint.ReplaceText(4, "public:static", 0);

                editPoint.Insert(keyinCommandFunctionCPP);
                if (objEditPt.FindPattern("throw gcnew System::NotImplementedException();"))
                {
                    editPoint.Delete(52);
                }

                editPoint.StartOfDocument();
                objMovePt.EndOfDocument();
                editPoint.SmartFormat(objMovePt);
            }
        }

        /// <summary>
        /// Helper method to process project items.
        /// </summary>
        private void ProcessProjectItemTemplates()
        {
            foreach (ProjectItem projectItem in ActiveProject.ProjectItems)
            {
                if (projectItem.Name.ToLower() == "commands.xml")
                {
                    if (ReplacementsDictionary["$safeitemname$"].ToLower().Contains("keyincommands"))
                    {
                        MessageBox.Show(Properties.Resources.CommandTableError, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IsAllowed = false;
                        break;
                    }
                    else
                    {
                        if (!ReplacementsDictionary["$safeitemname$"].ToLower().Contains("scancriteriaextensions"))
                        {
                            DialogResult response = MessageBox.Show(Properties.Resources.AddItem, Properties.Resources.AddItemMessageTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                            if (response == DialogResult.Yes)
                            {
                                IsItemAddedToKeyins = true;
                            }
                        }
                        break;
                    }
                }
            }

            foreach (ProjectItem projectItem in ActiveProject.ProjectItems)
            {
                if (projectItem.Name.ToLower().Contains("scancriteriaextensions"))
                {
                    if (ReplacementsDictionary["$safeitemname$"].ToLower().Contains("scancriteriaextensions"))
                    {
                        MessageBox.Show(Properties.Resources.ScanCriteriaMsg, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IsAllowed = false;
                    }
                    break;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string Language
        {
            get
            {
                if (ReplacementsDictionary.ContainsKey("$language$"))
                {
                    if (string.IsNullOrEmpty(ReplacementsDictionary["$language$"]))
                    {
                        return "Language not specified";
                    }
                    return ReplacementsDictionary["$language$"];
                }
                else
                    return "Dictionary does not contain language";
            }
        }


        public bool IsLanguageCS
        {
            get
            {
                return Language == "CSharp";
            }
        }

        public bool IsLanguageVB
        {
            get
            {
                return Language == "VisualBasic";
            }
        }

        public bool IsLanguageCPP
        {
            get
            {
                return Language == "CPP";
            }
        }

        public bool IsLanguageVC
        {
            get
            {
                return Language == "VC";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string Item
        {
            get
            {
                if (ReplacementsDictionary.ContainsKey("$item$"))
                {
                    if (string.IsNullOrEmpty(ReplacementsDictionary["$item$"]))
                    {
                        return "Item description not specified";
                    }
                    return ReplacementsDictionary["$item$"];
                }
                else
                    return "NoKey";
            }
        }

        /// <summary>
        /// Get or Set file information.
        /// </summary>
        public FileInfo ProjectFileInfo { get; set; }

        /// <summary>
        /// Get assembly Version.
        /// </summary>
        public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Name.ToUpper() + " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Get addin attribute.
        /// </summary>
        /// <returns></returns>
        private string GetAddinAttribute()
        {
            string bentleyApp = BentleyApp;
            string safeprojectname = ReplacementsDictionary["$safeprojectname$"];
            if (IsLanguageCS)
            {
                if (bentleyApp.Contains("PowerDraft"))
                    return
                        "/// <summary>" +
                        "\r\n/// Main entry point class for this addin application." +
                        "\r\n/// When loading an AddIn MicroStation looks for a class" +
                        "\r\n/// derived from AddIn." +
                        "\r\n///Sample password shown, Request a password at https://www.bentley.com/en/software-developers/bdn-inquiry-form." +
                        "\r\n/// </summary>" +
                        "\r\n[BM.AddIn(MdlTaskID = \"" + safeprojectname + "\"," +
                        "\r\nPassword=\"0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF01234567\")]";
                else
                    return
                        "/// <summary>" +
                        "\r\n/// Main entry point class for this addin application." +
                        "\r\n/// When loading an AddIn MicroStation looks for a class" +
                        "\r\n/// derived from AddIn." +
                        "\r\n/// </summary>" +
                        "\r\n[BM.AddIn(MdlTaskID = \"" + safeprojectname + "\")]";
            }
            else if (IsLanguageVB)
            {
                if (bentleyApp.Contains("PowerDraft"))
                    return
                        "\r\n" +
                        "\r\n''' <summary>" +
                        "\r\n''' When loading an AddIn MicroStation looks for a class" +
                        "\r\n''' derived from AddIn." +
                        "\r\n''' Sample password shown, Request a password at https://www.bentley.com/en/software-developers/bdn-inquiry-form." +
                        "\r\n''' </summary>" +
                        "\r\n<BM.AddIn(MdlTaskID:=\"" + safeprojectname + "\", _" +
                        "\r\nPassword:=\"0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF01234567\")>";
                else
                    return
                        "\r\n" +
                        "\r\n''' <summary>" +
                        "\r\n''' When loading an AddIn MicroStation looks for a class" +
                        "\r\n''' derived from AddIn." +
                        "\r\n''' </summary>" +
                        "\r\n<BM.AddIn(MdlTaskID:=\"" + safeprojectname + "\")>";
            }
            else if (IsLanguageCPP)
            {
                if (bentleyApp.Contains("PowerDraft"))
                    return
                        "#define         DLM_PASSWORD(x) \\" +
                        "\r\nBEGIN_EXTERN_C \\" +
                        "\r\n     __declspec(dllexport) void            dlmPassword \\" +
                        "\r\n     ( \\" +
                        "\r\n     char    *passwordP, \\" +
                        "\r\n     WCharCP  appNameP \\" +
                        "\r\n     ) \\" +
                        "\r\n         { \\" +
                        "\r\n         strcpy (passwordP, (x)); \\" +
                        "\r\n         } \\" +
                        "\r\nEND_EXTERN_C";
                else
                    return "";
            }
            else if (IsLanguageVC)
            {
                if (bentleyApp.Contains("PowerDraft"))
                    return
                        "/// <summary>" +
                        "\r\n/// MicroStation looks for this class that is" +
                        "\r\n/// derived from Bentley.MicroStation.AddIn." +
                        "\r\n/// Sample Password shown. Request a password at https://www.bentley.com/en/software-developers/bdn-inquiry-form." +
                        "\r\n/// </summary>" +
                        "\r\n[Bentley::MstnPlatformNET::AddInAttribute(MdlTaskID = L\"" + safeprojectname + "\"," +
                        "\r\nPassword=L\"0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF01234567\")]";
                else
                    return
                        "/// <summary>" +
                        "\r\n/// MicroStation looks for this class that is" +
                        "\r\n/// derived from Bentley.MicroStation.AddIn." +
                        "\r\n/// </summary>" +
                        "\r\n[Bentley::MstnPlatformNET::AddInAttribute(MdlTaskID = L\"" + safeprojectname + "\")]";
            }
            return "";
        }

        public string GetReferences(string projectFileName)
        {
            string bentleyApp = BentleyApp;
            if (bentleyApp.Contains("AECOsim"))
            {
                if (projectFileName.Contains("Form") || projectFileName.Contains("WPF"))
                    return
                        "    <Reference Include=\"Bentley.Interop.TFCom\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.Building.Api\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.Interop.STFCom\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.Interop.ATFCom\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>";
                else
                    return
                        "    <Reference Include=\"Bentley.Interop.TFCom\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.Building.Api\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.Interop.STFCom\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.Interop.ATFCom\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>";
            }
            else if (bentleyApp.Contains("OpenPlant"))
            {
                return projectFileName.Contains("Form") || projectFileName.Contains("WPF")
                    ? "    <Reference Include=\"Bentley.OpenPlantModeler.SDK\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>"
                    : "    <Reference Include=\"Bentley.OpenPlantModeler.SDK\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>";
            }
            else if (bentleyApp.Contains("OpenBridge"))
            {
                return projectFileName.Contains("Form") || projectFileName.Contains("WPF")
                    ? "    <Reference Include=\"Bentley.ObmNET.Model\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>"
                    : "    <Reference Include=\"Bentley.ObmNET.Model\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>";
            }
            else if (bentleyApp.Contains("OpenRail"))
            {
                return projectFileName.Contains("Form") || projectFileName.Contains("WPF")
                    ? "    <Reference Include=\"Bentley.Civil.Rail.GeometryModel\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.CifNET.GeometryModel.SDK.4.0\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>"
                    : "    <Reference Include=\"Bentley.Civil.Rail.GeometryModel\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n\r" +
                        "    <Reference Include=\"Bentley.CifNET.GeometryModel.SDK.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.LinearGeometry.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.Geometry.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.SDK.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.Formatting.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.TerrainModelNET\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>";
            }
            else if (bentleyApp.Contains("OpenRoads"))
            {
                return projectFileName.Contains("Form") || projectFileName.Contains("WPF")
                    ? "    <Reference Include=\"Bentley.CifNET.GeometryModel.SDK.4.0\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>"
                    : "    <Reference Include=\"Bentley.CifNET.GeometryModel.SDK.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.LinearGeometry.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.Geometry.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.SDK.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.CifNET.Formatting.4.0\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>\n" +
                        "    <Reference Include=\"Bentley.TerrainModelNET\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>";
            }
            else
                return "";

        }

        public string GetAddinAssembly
        {
            get
            {
                //if (Item != "NoKey")
                //{
                //    if (Language == "VC")
                //    {
                //        if (Item.Contains("OpenRoads"))
                //            return "Bentley::Civil::Roadway::Commands::Addin";
                //        else
                //            return "Bentley::MstnPlatformNET::AddIn";
                //    }
                //    else
                //    {
                //        if (Item.Contains("OpenRoads"))
                //            return "BC.Addin";
                //        else
                //            return "BM.AddIn";
                //    }
                //}
                //if (Language == "VC")
                //{
                //    return "Bentley::MstnPlatformNET";
                //}
                return "BM.AddIn";
            }
        }

        public string GetAdditionalUsing
        {
            get
            {
                //if (Item != "NoKey")
                //{
                //    if (Language == "CSharp")
                //    {
                //        if (Item.Contains("OpenRoads"))
                //        {
                //            return "using BC = Bentley.Civil.RoadwayDesigner.Commands;";
                //        }
                //        else
                //        {
                //            return null;
                //        }
                //    }
                //    else if (Language == "VisualBasic")
                //    {
                //        if (Item.Contains("OpenRoads"))
                //        {
                //            return "Imports BC = Bentley.Civil.RoadwayDesigner.Commands";
                //        }
                //        else
                //        {
                //            return null;
                //        }
                //    }
                //    else if (Language == "VC")
                //    {
                //        if (Item.Contains("OpenRoads"))
                //        {
                //            return "#using <Bentley.Civil.RoadwayDesigner.Commands.4.0.dll>";
                //        }
                //        else
                //        {
                //            return null;
                //        }
                //    }
                //}
                return "";
            }
        }

        // Summary:
        //     Gets the active project.
        //
        // Returns:
        //     Returns the active project.
        private Project ActiveProject { get { return (Project)ActiveSolutionProjects.GetValue(0); } }

        /// <summary>
        /// Get ActiveSolutionProjects. Read only.
        /// </summary>
        private Array ActiveSolutionProjects { get { return (Array)Dte.ActiveSolutionProjects; } }

        /// <summary>
        /// Get active project GUID. Read only.
        /// </summary>
        private Guid ActiveProjectGuid { get { return new Guid(ActiveProject.Kind); } }

        /// <summary>
        /// Get or Set BackoutWizardException String.
        /// </summary>
        private string BackoutError { get; set; }

        /// <summary>
        /// Get Bentley application. Read only.
        /// </summary>
        private string BentleyApp
        {
            get
            {
                return Dte.get_Properties("Bentley", "MicroStation CONNECT").Item("BentleyApp").Value.ToString();
            }
        }

        /// <summary>
        /// Get CSharp GUID Id. Read only.
        /// </summary>
        private Guid CSharpProjectGuid { get { return new Guid(VSLangProj.PrjKind.prjKindCSharpProject); } }

        /// <summary>
        /// Get DestinationDirectory. Read only.
        /// </summary>
        private string DestinationDirectory { get { return ReplacementsDictionary["$destinationdirectory$"]; } }

        /// <summary>
        /// Get or set Visual Studio environment DTE (EnvDTE.DTE).
        /// </summary>
        private DTE Dte { get; set; }

        /// <summary>
        /// Get or Set FunctionName.
        /// </summary>
        private string FunctionName { get; set; }

        /// <summary>
        /// Get or set boolean value to determine if Commands.xml is allowed.
        /// Allow commands.xml if true otherwise do not allow commands.xml.
        /// </summary>
        private bool IsAllowed { get; set; }

        /// <summary>
        /// Get boolean value if project is CSharp or Visual Basic.
        /// Returns true if CSharp or Visual Basic otherwise returns false.
        /// Read only.
        /// </summary>
        private bool IsCSOrvbProject { get { return ActiveProjectGuid == CSharpProjectGuid || ActiveProjectGuid == VBProjectGuid; } }

        /// <summary>
        /// Get boolean value to determine if project is a CSharp project.
        /// True if project is a CSharp project otherwise false.
        /// </summary>
        private bool IsCSProject { get { return ActiveProjectGuid == CSharpProjectGuid; } }

        /// <summary>
        /// Get boolean value to determine if project is a CPP project.
        /// True if project is a CPP project otherwise false.
        /// </summary>
        private bool IsCPPProject
        {
            get
            {
                if (Language == "CPP")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Get boolean value to determine if project is a VC Managed project.
        /// True if project is a VC Managed project otherwise false.
        /// </summary>
        private bool IsVCProject
        {
            get
            {
                if (Language == "VC")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Get or set boolean value to determine if project item is added to keyins.
        /// If true, project item is added to keyins otherwise do not add project items to keyins.
        /// </summary>
        private bool IsItemAddedToKeyins { get; set; }

        /// <summary>
        /// Get or set boolean value if the wizard run is inherited from a project item.
        /// If true, it is a project item, otherwise it is a project.
        /// </summary>
        private bool IsNewItem { get; set; }

        /// <summary>
        /// Get boolean value to determine if project is a Visual Basic project.
        /// True if project is a Visual Basic project otherwise false.
        /// </summary>
        private bool IsVBProject { get { return ActiveProjectGuid == VBProjectGuid; } }

        /// <summary>
        /// Get platformtoolset. Read only.
        /// </summary>
        private string PlatFormToolSet
        {
            get
            {
                string platformtool = "";
                double.TryParse(Version, out double version);
                if (version == 11)
                    platformtool = "v110";
                else if (version == 12)
                    platformtool = "v120";
                else if (version == 14)
                    platformtool = "v140";
                else if (version == 15)
                    platformtool = "v141";
                else
                    platformtool = "v120";

                return platformtool;
            }
        }

        /// <summary>
        /// Get or set registry key.
        /// </summary>
        private static RegistryKey Key { get; set; }

        /// <summary>
        /// Get or set ReplacementsDictionary.
        /// </summary>
        public Dictionary<string, string> ReplacementsDictionary { get; set; }

        /// <summary>
        /// Get RootNamespace. Read Only.
        /// </summary>
        public string RootNamespace { get { return ActiveProject.Properties.Item("DefaultNamespace").Value.ToString(); } }

        /// <summary>
        /// Get SolutionDirectory. Read Only.
        /// </summary>
        private string SolutionDirectory { get { return ReplacementsDictionary["$solutiondirectory$"]; } }

        /// <summary>
        /// Get Visual Basic GUID Id. Read only.
        /// </summary>
        private Guid VBProjectGuid { get { return new Guid(VSLangProj.PrjKind.prjKindVBProject); } }

        /// <summary>
        /// Get Version. Read only.
        /// </summary>
        private string Version
        {
            get
            {
                return Dte.Version.ToString();
            }
        }

        #endregion
    }

    public static class GetRegistry
    {
        public static string GetRegistryValue(string keyName, string valueName)
        {
            return (string)Microsoft.Win32.Registry.GetValue(keyName, valueName, null);
        }

        #region Properties
        //private static Dictionary<object, object> BentleyProducts { get; set; }
        private static RegistryKey LocalMachineRegistry { get; set; }
        public static RegistryKey CurrentUser { get; set; }
        private static RegistryKey Key { get; set; }
        private static RegistryKey PropertyKey { get; set; }
        private static string MostRecentVersionInstallLocation { get; set; }

        public static void StoreEnvironmentValue(string subKey, string keyValue, string value)
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(subKey, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                try
                {
                    key.SetValue(keyValue, value, Microsoft.Win32.RegistryValueKind.String);
                    string test = (string)key.GetValue(keyValue, "");
                }
                catch (Exception)
                {

                }
            }
        }
        #endregion
    }

    internal class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Int32 GetShortPathName(String path, StringBuilder shortPath, Int32 shortPathLength);
    }

























}