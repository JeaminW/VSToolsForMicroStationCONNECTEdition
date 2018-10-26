using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

            IsCommandsXmlAllowed = true;
            IsItemAddedToKeyins = false;

            //Activates and ensures Solution Explorer is visible.
            Dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Activate();
            Dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Visible = true;
            if (IsNewItem)
                ProcessProjectItem();
            else
                ProcessProject();

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

        /// <summary>
        /// Process project item.
        /// </summary>
        private void ProcessProjectItem()
        {
            foreach (ProjectItem projectItem in ActiveProject.ProjectItems)
            {
                if (projectItem.Name.ToLower() == "commands.xml")
                {
                    if (ReplacementsDictionary["$safeitemname$"].ToLower().Contains("keyincommands"))
                    {
                        MessageBox.Show(Properties.Resources.CommandTableError, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IsCommandsXmlAllowed = false;
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
                        IsCommandsXmlAllowed = false;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Process project.
        /// </summary>
        private void ProcessProject()
        {
            if (ProjectFileInfo.Name == "MSCE.Keyin.vstemplate" || ProjectFileInfo.Name == "MSCE.Addin.vstemplate")
            {
                ProcessEmbeddedResource();
            }

            ReplacementsDictionary.Add("$ToolVersion$", Version);
            ReplacementsDictionary.Add("$platformtoolset$", PlatFormToolSet);
            ReplacementsDictionary.Add("$AddinAttribute$", AddAddinAttribute());
            ReplacementsDictionary.Add("$reference$", AddReferences());
        }

        /// <summary>
        /// Process embedded resources.
        /// </summary>
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

        /// <summary>
        /// Get substitution results.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
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
        /// Add locate command, placement command, toolbar, winforms, and wpf function name to
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
                                switch (GetItemTypeFromTemplate)
                                {
                                    case "MicroStation Selection Tool":
                                        keyinCommandFunctionCS = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Selection Tool Settings":
                                        keyinCommandFunctionCS = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Selection Tool Settings WPF":
                                        keyinCommandFunctionCS = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation IPrimitive Tool Settings":
                                        keyinCommandFunctionCS = AddFunctionCode(FunctionName, "tool", "Run(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode(FunctionName, "tool", "Run(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation IPrimitive Tool Settings WPF":
                                        keyinCommandFunctionCS = AddFunctionCode(FunctionName, "tool", "Run(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode(FunctionName, "tool", "Run(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Placement Tool":
                                        keyinCommandFunctionCS = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Placement Tool Settings":
                                        keyinCommandFunctionCS = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Placement Tool Settings WPF":
                                        keyinCommandFunctionCS = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode2(FunctionName, "tool", "InstallNewInstance(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Tool Settings WPF":
                                        keyinCommandFunctionCS = AddFunctionCode(FunctionName, "tool", "ShowWindow(Program.Addin, unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode(FunctionName, "tool", "ShowWindow(Program.Addin, unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Tool Settings":
                                        keyinCommandFunctionCS = AddFunctionCode(FunctionName, "tool", "ShowForm(Program.Addin, unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode(FunctionName, "tool", "ShowForm(Program.Addin, unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Toolbar WPF":
                                        keyinCommandFunctionCS = AddFunctionCode(FunctionName, "toolbar", "ShowToolbar(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode(FunctionName, "toolbar", "ShowToolbar(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation Windows Form":
                                        keyinCommandFunctionCS = AddFunctionCode(FunctionName, "form", "ShowForm(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionCPP = FunctionName + "::ShowForm(Program::MSAddin);";
                                        keyinCommandFunctionVB = AddFunctionCode(FunctionName, "form", "ShowForm(unparsed)", ProjectLanguage.VB);
                                        break;
                                    case "MicroStation UserControl (WPF)":
                                        keyinCommandFunctionCS = AddFunctionCode(FunctionName, "usercontrol", "ShowWindow(unparsed)", ProjectLanguage.CS);
                                        keyinCommandFunctionVB = AddFunctionCode(FunctionName, "usercontrol", "ShowWindow(unparsed)", ProjectLanguage.VB);
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

        /// <summary>
        /// Add function code for MicroStation CONNECT Edition Iprimitive Tool, Windows Form and WPF Tool Settings and applications.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string AddFunctionCode(string functionName, string obj, string method, ProjectLanguage lang)
        {
            switch (lang)
            {
                case ProjectLanguage.CS:
                    return functionName + " " + obj + " = new " + functionName + "();\n" + obj + "." + method + ";";
                case ProjectLanguage.VB:
                    return "Dim " + obj + " As " + functionName + " = New " + functionName + "\n" + obj + "." + method;
                default:
                    return functionName + " " + obj + " = new " + functionName + "();\n" + obj + "." + method + ";";
            }
        }

        /// <summary>
        /// Add function code for MicroStation CONNECT Edition Selection and Placement Tool.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private static string AddFunctionCode2(string functionName, string obj, string method, ProjectLanguage lang)
        {
            switch (lang)
            {
                case ProjectLanguage.CS:
                    return functionName + " " + obj + " = new " + functionName + "(0, 0);\n" + obj + "." + method + ";";
                case ProjectLanguage.VB:
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
        /// Add addin attribute for project.
        /// </summary>
        /// <returns></returns>
        private string AddAddinAttribute()
        {
            string bentleyApp = BentleyApp;
            string safeprojectname = ReplacementsDictionary["$safeprojectname$"];
            if (IsCSProject)
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
            else if (IsVBProject)
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
            else if (IsCPPProject)
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
            else if (IsVCProject)
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

        /// <summary>
        /// Add assembly references for specific Bentley product projects.
        /// </summary>
        /// <returns></returns>
        public string AddReferences()
        {
            string bentleyApp = BentleyApp;
            if (bentleyApp.Contains("AECOsim"))
            {
                if (ProjectFileInfo.Name.Contains("Form") || ProjectFileInfo.Name.Contains("WPF"))
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
                return ProjectFileInfo.Name.Contains("Form") || ProjectFileInfo.Name.Contains("WPF")
                    ? "    <Reference Include=\"Bentley.OpenPlantModeler.SDK\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>"
                    : "    <Reference Include=\"Bentley.OpenPlantModeler.SDK\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>";
            }
            else if (bentleyApp.Contains("OpenBridge"))
            {
                return ProjectFileInfo.Name.Contains("Form") || ProjectFileInfo.Name.Contains("WPF")
                    ? "    <Reference Include=\"Bentley.ObmNET.Model\">\n\r" +
                        "      <Private>True</Private>\n\r" +
                        "    </Reference>"
                    : "    <Reference Include=\"Bentley.ObmNET.Model\">\n\r" +
                        "      <Private>False</Private>\n\r" +
                        "    </Reference>";
            }
            else if (bentleyApp.Contains("OpenRail"))
            {
                return ProjectFileInfo.Name.Contains("Form") || ProjectFileInfo.Name.Contains("WPF")
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
                return ProjectFileInfo.Name.Contains("Form") || ProjectFileInfo.Name.Contains("WPF")
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

        #endregion
        #region Properties

        #endregion

        #region Properties

        /// <summary>
        /// Get language from template.
        /// </summary>
        public string GetLanguageFromTemplate
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

        /// <summary>
        /// Get item type from project item template.
        /// </summary>
        public string GetItemTypeFromTemplate
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
        /// Get or Set project file information.
        /// </summary>
        public FileInfo ProjectFileInfo { get; set; }

        /// <summary>
        /// Get the active project. (Read only)
        /// </summary>
        private Project ActiveProject => (Project)ActiveSolutionProjects.GetValue(0);

        /// <summary>
        /// Get ActiveSolutionProjects. (Read only)
        /// </summary>
        private Array ActiveSolutionProjects => (Array)Dte.ActiveSolutionProjects;

        /// <summary>
        /// Get Bentley application. (Read only)
        /// </summary>
        private string BentleyApp => Dte.get_Properties("Bentley", "MicroStation CONNECT").Item("BentleyApp").Value.ToString();

        /// <summary>
        /// Get or set Visual Studio environment DTE (EnvDTE.DTE).
        /// </summary>
        private DTE Dte { get; set; }

        /// <summary>
        /// Get or Set FunctionName.
        /// </summary>
        private string FunctionName { get; set; }

        /// <summary>
        /// Determine if Commands.xml is allowed. Allow commands.xml if true
        /// otherwise do not allow commands.xml.
        /// </summary>
        private bool IsCommandsXmlAllowed { get; set; }

        /// <summary>
        /// Determine if the project is a CSharp project. Returns true if project is a CSharp Project
        /// otherwise returns false. (Read only)
        /// </summary>
        private bool IsCSProject => GetLanguageFromTemplate == "CSharp";

        /// <summary>
        /// Determine if the project is a CPP project. Returns true if project is a CPP Project
        /// otherwise returns false. (Read only)
        /// </summary>
        private bool IsCPPProject => GetLanguageFromTemplate == "CPP";

        /// <summary>
        /// Determine if the project is a managed CPP project. Returns true if project is a managed CPP Project
        /// otherwise returns false. (Read only)
        /// </summary>
        private bool IsVCProject => GetLanguageFromTemplate == "VC";

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
        /// Determine if the project is a Visual Basic project. Returns true if project is a Visual Basic Project
        /// otherwise returns false. (Read only)
        /// </summary>
        private bool IsVBProject => GetLanguageFromTemplate == "VisualBasic";

        /// <summary>
        /// Get platformtoolset. (Read only)
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
        /// Get or set ReplacementsDictionary.
        /// </summary>
        public Dictionary<string, string> ReplacementsDictionary { get; set; }

        /// <summary>
        /// Get RootNamespace. (Read only)
        /// </summary>
        public string RootNamespace => ActiveProject.Properties.Item("DefaultNamespace").Value.ToString();

        /// <summary>
        /// Get Version. (Read only)
        /// </summary>
        private string Version => Dte.Version.ToString();

        #endregion

        #region Enumerations

        private enum ProjectLanguage
        {
            CS,
            VB,
            CPP,
            VC
        }

        #endregion
    }
}