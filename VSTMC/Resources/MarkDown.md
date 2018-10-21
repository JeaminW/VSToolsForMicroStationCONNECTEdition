## Seamlessly Integrations of Visual Studio and MicroStation - A more reliable, productive, robust and a simply more enjoyable development experience.

### **MicroStation CONNECT Edition** and **OpenRoads Designer SDK** C# and VB .NET snippets are available upon request to anyone who asks...[martyrobbins@innovocad.com](mailto:martyrobbinsinnovocad.com)  

#### Supported Bentley Products
+ **[Bentley AECOSim Building Designer CONNECT Edition](https://www.bentley.com/en/products/brands/aecosim/)**
+ **[Bentley Descartes CONNECT Edition](https://www.bentley.com/en/products/brands/descartes/)**
+ **[Bentley Map CONNECT Edition](https://www.bentley.com/en/products/brands/map/)**
+ **[Bentley MicroStation CONNECT Edition](https://www.bentley.com/en/products/brands/microstation/)**
+ **Bentley MicroStation PowerDraft CONNECT Edition**
+ **[Bentley OpenBridge Modeler CONNECT Edition](https://www.bentley.com/en/products/brands/openbridge-modeler/)**
+ **[Bentley OpenPlant CONNECT Edition](https://www.bentley.com/en/products/brands/openplant/)**
+ **[Bentley OpenRail CONNECT Edition](https://www.bentley.com/en/products/brands/openrail/)**
+ **[Bentley OpenRoads Designer CONNECT Edition](https://www.bentley.com/en/products/brands/openroads/)**
+ **Bentley Substation CONNECT Edition**

#### Features
+ [**C# and VB .Net Project Templates**](#CSharp-and-VB-NET-Project-Templates) 
+ [**C++ Project Templates**](#CPP-Project-Templates) 
+ [**C# and VB .Net Item Templates**](#CSharp-and-VB-NET-Item-Templates) 
+ [**Snippets**](#Snippets)
+ [**Import Native C++ applications to Visual Studio**](#Import-Native-CPP) 
+ [**Search Bentley forums easily within Visual Studio**](#Search-Bentley-Forums) 
+ [**Keyin Command Table (Command.xml) Intellisense**](#Keyin-Command-Table-Intellisense)
+ [**Easily Get Needed Bentley Assembly References**](#Easily-Get-Needed-Bentley-Assembly-References)
+ [**Reference Assemblies** ***Copy Local*** **property set to** ***False*** **by default for MicroStation hosted add-ins**](#Copy-Local-Property) 
+ [**Open Bentley Application Folder and MDLAPPS Folder in Solution Explorer**](#Open-Bentley-Application-Folder-and-MDLAPPS-Folder-in-Solution-Explorer)

---
**Connect with us on...** 

[![YouTube](https://www.innovocad.com/images/VSMarketplace/YouTube.png)](https://www.youtube.com/channel/UC9jL56FG4IN4uBpO1Lyedfg)
[![LinkedIn](https://www.innovocad.com/images/VSMarketplace/LinkedIn2.png)](https://www.linkedin.com/company/innovocad)
[![Facebook](https://www.innovocad.com/images/VSMarketplace/Facebook2.png)](https://www.facebook.com/innovocad)
[![Facebook](https://www.innovocad.com/images/VSMarketplace/Twitter.png)](https://www.twitter.com/innovocad)

**For additional assistance, please contact [Support](mailto:support@innovocad.com "Need help please click me").**  

---
Please visit **[Bentley Systems](https://www.bentley.com)** for additional information on Bentley products.
**Bentley Systems, Inc. is not associated with innovoCAD Solutions or Visual Studio Tools for MicroStation.**

---
##### Templates are available only if a supported Bentley product is installed.
---
#### **CSharp and VB NET Project Templates**
The .NET Framework and Visual Studio requirements is dependent on the supported Bentley product and version. The following table shows supported
Bentley products .NET Framework requirements. The following table may not be exhaustive. Please contact [info@innofocad.com](mailto:info@innovocad.com)
for corrections, errors, or omissions.

| Product | Start Version | To Version | .NET Framework |
| ------ | ----------- |-------------|-------------|-------------|
| **AECOSim Building Designer** | 10.00.00.92 | 10.00.00.118 | 4.5.2
| **AECOSim Building Designer** | 10.00.00.154 | 10.03.00.40 | 4.6.1
| **Descartes CONNECT Edition** | 10.01.00.33 |--|4.5.2
| **Map CONNECT Edition** | 10.00.00.030 |--|4.6.1
| **MicroStation CONNECT Edition** | -- | 10.04.00.046 | 4.5.2 |
| **MicroStation CONNECT Edition** | 10.05.00.40 | 10.06.00.38 | 4.6.1
| **MicroStation CONNECT Edition** | 10.07.00.39 | 10.09.01.001 | 4.6.2
| **MicroStation PowerDraft CONNECT Edition** | -- | 10.04.00.49 | 4.5.2
| **MicroStation PowerDraft CONNECT Edition** | 10.05.00.43 | 10.05.00.43 | 4.6.1
| **MicroStation PowerDraft CONNECT Edition** | 10.07.00.39 | 10.08.00.041 | 4.6.2
| **OpenBridge Modeler CONNECT Edition** | 10.03.00.003 |--|4.6.1
| **OpenPlant CONNECT Edition** | 10.00.00.119 |--|4.6.1
| **OpenRail CONNECT Edition** | 10.03.00.043 |--|4.6.1
| **OpenRoads Designer CONNECT Edition** | 10.01.00.15 | 10.03.00.043| 4.6.1
| **Substation CONNECT Edition** | 10.00.00.232 |--|4.6.1

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCAddNewProject.png)

+ **MicroStation Add-in**
    + MicroStation CONNECT Edition Add-in without key-ins. Can be used for a non-interactive application. Use **Add | New Item** and select project item **Key-In Command Table** to add a command table.
+ **MicroStation Add-in Command**
    + MicroStation CONNECT Edition Add-in with key-in command table.
+ **MicroStation Window Form Application**
    + A project for creating an external (.exe) application with a Windows Forms with MicroStation CONNECT Edition.
+ **MicroStation WPF Application**
    + A project for creating an external (.exe) application with a Windows Presentation Foundation (WPF) UserControl with MicroStation CONNECT Edition.

See **[Tool Options](#tool-options)** for additional information.

[**Go to Features**](#Features)

---
#### **CSharp and VB NET Item Templates**
+ **Class**
    - MicroStation empty class definition.
+ **IPrimitive Command (Wpf)**
    - A project item to implement the **IPrimitiveCommandEvents** interface that implements a new element-creation command with a custom Wpf UserControl Tool Settings.
    - By default use ***ItemName*****UserControl**.***Property***, ***ItemName*****UserControl**.***Method()***, ***ItemName*****UserControl**.***Field*** to access public properties, methods, or fields in ***ItemName***.xaml.cs.
+ **IPrimitive Command Windows Form**
    - A project item to implement the **IPrimitiveCommandEvents** interface that implements a new element-creation command with a custom Windows Form Tool Settings.
    - By default use ***ItemName*****MSForm**.***Property***, ***ItemName*****MSForm**.***Method()***, ***ItemName*****MSForm**.***Field*** to access public properties, methods, or fields in ***ItemName*****Form**.cs.
+ **Key-In Command Table**
    - A project item to create a **Commands.xml** file and **KeyinCommands.cs** or **KeyinCommands.vb** mapped to the command.xml. One command table is allowed per project.
+ **Placement Tool**
    - A project item that inherits from the **DgnPrimitiveTool** class to create and place elements.
+ **Placement Tool (Wpf)**
    - A project item that inherits from the **DgnPrimitiveTool** class to create and place elements with a custom Wpf UserControl Form Tool Settings.
    - By default use ***ItemName*****UserControl**.***Property***, ***ItemName*****UserControl**.***Method()***, ***ItemName*****UserControl**.***Field*** to access public properties, methods, or fields in ***ItemName***.xaml.cs.
+ **Placement Tool Windows Form**
    - A project item that inherits from the **DgnPrimitiveTool** class to create and place elements with a custom Windows Form Tool Settings.
    - By default use ***ItemName*****MSForm**.***Property***, ***ItemName*****MSForm**.***Method()***, ***ItemName*****MSForm**.***Field*** to access public properties, methods, or fields in ***ItemName*****Form**.cs.
+ **Scan Criteria Extensions**
    - A project item to add the Scan Criteria Extension. One scan criteria extension is necessary per project.
+ **Selection Tool**
    - A project item that inherits from a **DgnElementSetTool** class that modifies an element.
+ **Selection Tool Settings (Wpf)**
    - A project item that inherits from a **DgnElementSetTool** class that modifies an element with a custom Wpf UserControl Tool Settings.
    - By default use ***ItemName*****UserControl**.***Property***, ***ItemName*****UserControl**.***Method()***, ***ItemName*****UserControl**.***Field*** to access public properties, methods, or fields in ***ItemName***.xaml.cs.
+ **Selection Tool Windows Form**
    - A project item that inherits from a **DgnElementSetTool** class that modifies an element with a custom Windows Form Tool Settings.
    - By default use ***ItemName*****MSForm**.***Property***, ***ItemName*****MSForm**.***Method()***, ***ItemName*****MSForm**.***Field*** to access public properties, methods, or fields in ***ItemName*****Form**.cs.
+ **Toolbar (Wpf)**
    - A project item to create a MicroStation tool bar using a Wpf UserControl.
+ **UserControl (Wpf)**
    - A project item to create a Wpf UserControl.
+ **Window Form**
    - A project item to create a Windows Form.

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCItemTemplates2.png)

---
For Classic Window Forms... Select **DESIGN** in the **Solution Configuration** to easily switch to System.Windows.Form designer to 
edit the Form in the designer. Close form when complete and select **Debug** or **Release** in **Solution Configuration**. App will not
successfully build int **DESIGN** mode.

[**Go to Features**](#Features)

---
#### **CPP Project Templates**
Visual Studio requirement is dependent on the supported Bentley product and version.
+ **MicroStation Managed Add-in**
    -  MicroStation CONNECT Edition Managed/CLR without Key-ins.
+ **MicroStation Managed Command**
    - MicroStation CONNECT Edition Managed/CLR with Key-ins.
+ **MicroStation Native**
    -  MicroStation CONNECT Edition Native without key-ins.
+ **MicroStation Native Command**
    -  MicroStation CONNECT Edition Native with key-ins.

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCCPPAddNewProject.png)

**Visual Studio Tools for MicroStation CONNECT Edition** uses a Visual Studio build process to build the dll application using a **Dynamic Library (.dll)** configuration Type.
A batch file is used to create the necessary **.ma** file using the **Bentley Systems Make Utility**. The default make batch file that is deployed with this tool is located at 
%SystemDrive%/ProgramData/innovoCAD/Bentley/VisualStudioTools/CONNECTBuildV1Tool.bat. This batch file is intended to be used by **Visual Studio Tools for MicroStation CONNECT Edition** using C++ Makefile project settings with the file path and parameters 
set by the **[Tool Options](#tool-options)**. With that in mind, this file can be modified. The default path and file can be changed in the **Tools Options**. However, if this file is
deleted it will be regenerated when Visual Studio opens. The default make batch file is shown below...

```
echo off
set MS=%~fs1
set MSMDE=%~fs2
set MDLBIN=%MSMDE%bin\
set PATH=%PATH%;%MDLBIN%
set BMAKE_OPT=-I%MSMDE%mki
set DEFAULT_TARGET_PROCESSOR_ARCHITECTURE=x64
set DLM_NO_SIGN=1
set MSMDE_OUTPUT=%~fs3
pushd %~4
Bmake %~5
```
The following successful build example is using Visual Studio 2015 and the default make batch file used to create the **.ma** file.
```
1>------ Build started: Project: MSCENative1, Configuration: Debug x64 ------
1>  ModulVer.cpp
1>  MicroStationUtilities.cpp
1>  std_collection_typedefs header (BENTLEY defined)
1>  MSCENative1.cpp
1>  std_collection_typedefs header (BENTLEY defined)
1>  Generating Code...
1>     Creating library x64\Debug\objects\MSCENative1.lib and object x64\Debug\objects\MSCENative1.exp
1>  MSCENative1.vcxproj -> F:\Program Files\Bentley\MicroStation CONNECT Edition\MicroStation\mdlapps\MSCENative1.dll
1>  MSCENative1.vcxproj -> F:\Program Files\Bentley\MicroStation CONNECT Edition\MicroStation\mdlapps\MSCENative1.pdb (Full PDB)
1>  Bentley Systems Make Utility. Version 10.00.00.22, May 15 2017
1>  Mon Jan 01 19:15:14 2018
1>
1>
1>  C:\Users\MARTYR~1\AppData\Local\Temp\getucrtversion14.bat
1>
1>
1>
1>
1>  [== Building f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\objects\MSCENative1cmd.h, (f:\Documents\VISUAL~1\Projects\MSCENative1\MSCENative1\MSCENative1cmd.r) ==]
1>  rcomp @f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\objects\make.opt
1>  MicroStation Resource Compiler 03.12.02
1>     Generating header file (f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\objects\MSCENative1cmd.h) ... done.
1>
1>  [== Building f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\rscobj\MSCENative1cmd.rsc, (f:\Documents\VISUAL~1\Projects\MSCENative1\MSCENative1\MSCENative1cmd.r) ==]
1>  rcomp @f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\objects\make.opt
1>  MicroStation Resource Compiler 03.12.02
1>
1>  [== Building f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\rscobj\MSCENative1.rsc, (f:\Documents\VISUAL~1\Projects\MSCENative1\MSCENative1\MSCENative1.r) ==]
1>  rcomp @f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\objects\make.opt
1>  MicroStation Resource Compiler 03.12.02
1>
1>  [== Building f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\reqdobjs\MSCENative1.mi, (f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\rscobj\MSCENative1.rsc) ==]
1>  rlib @f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\objects\make.opt
1>  MicroStation Resource Librarian 03.12.02
1>
1>  [== Building F:\PROGRA~2\Bentley\MICROS~2\MICROS~1\mdlapps\MSCENative1.ma, (f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\reqdobjs\MSCENative1.mi) ==]
1>  rlib @f:\Documents\VISUAL~1\Projects\MSCENA~1\MSCENA~1\x64\Debug\rscobj\make.opt
1>  MicroStation Resource Librarian 03.12.02
1>  Mon Jan 01 19:15:22 2018, elapsed time: 0:08
========== Build: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped ==========
```

[**Go to Features**](#Features)

---
#### Tool Options  
You can access the tool options from **Tools | Options...** and select the **Bentley** category. You will have various option pages

+ **Reset Button**  
    - Click the reset button to reset the options to default settings
+ **Folder Open Button** ![File](https://www.innovocad.com/images/VSMarketplace/FolderOpen.png)  
    - Click the folder open button to open the file explorer to the location specified.  
+ **Browse Button**
    - Click the Browse button to manually change the specified location path or file path, if necessary.

##### MicroStation CONNECT Edition

+ **Bentley Product**  
    - The Bentley product drop down list contains the supported Bentley products installed on your machine.
    - The Bentley product location contains the location of the selected Bentley product.
+ **MicroStation CONNECT Edition SDK Path**  
    - The location of the MicroStation CONNECT Edition SDK Path, if installed.  
+ **Debug Arguments**
    + The default debug arguments.
+ **MDLAPPSPATH Build Path**
    + The default location of build output path.

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCMSProjectOptions.png)

[**Go to Features**](#Features)

---
#### Snippets
You can view the available snippets in the **Tools | Code Snippets Manager...**. MicroStation CONNECT Edition Shortcuts are prefixed with **MSCE**
If you have an issue with the snippets that are installed as part of the tool, you may remove those snippets and download the snippets
separately. Please click **[here](https://www.innovocad.com/downloads/MSCESnippets.zip)** to download the MicroStation CONNECT Edition snippets and then add them in the **Code Snippets Manager** 

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCSnippets.png)

---
##### Import Native CPP
Use **Tools | MS CONNECT Native Import** to import native c++ applications to Visual Studio using the .mke file.

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCImport.png)

---
##### Search Bentley Forums
Search Bentley forums by selecting text and using the right-click context menu and select **Search Bentley Forums**

![File](https://www.innovocad.com/images/VSMarketplace/VSTMSearch2.png)

---
#### Keyin Command Table Intellisense    
Get **Command.xml** intellisense using the **KeyinTree.xsd** XML Schema Definition.

![File](https://www.innovocad.com/images/VSMarketplace/CommandIntellisense.png)

---
#### **Easily Get Needed Bentley Assembly References**
Right-click project **References** in Solution Explorer and select **Add Reference**. Select the **Assemblies**
tab and then the **Extensions** tab and add your needed references.

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCAddREferences.png)

---
#### ***Copy Local Property***
Referenced assemblies ***Copy Local*** property set to ***False*** for MicroStation hosted add-ins. In General, 
we want our Bentley referenced assemblies ***Copy Local*** property to be set to false. By Visual Studio 
normal behavior default, the ***Copy Local*** property is set to ***True***. For **Bentley** 
referenced assemblies, you would have to manually change these values to ***False***. If you build the
application with the **Bentley** referenced ***Copy Local*** property set to ***True***, those assemblies
will be copied to your **MDLAPPS** (bin) folder.

[**Go to Features**](#Features)

---
#### **Open Bentley Application Folder and MDLAPPS Folder in Solution Explorer**
Right-click your **Bentley** project in the Solution Explorer and select **Open Bentley App in File Explorer** 
or **Open MDLAPPS Folder in File Explorer**.

![File](https://www.innovocad.com/images/VSMarketplace/VSTMC/VSTMCProjectContext.png)

---
![File](https://www.innovocad.com/images/BentleyTechnologyPartner_LogoWeb.png)

Please email comments to [martyrobbins@innovocad.com](mailto:martyrobbinsinnovocad.com)  
Please contact [Support](mailto:support@innovocad.com "Need help please click me"), if you need additional assistance.  
For additional information please contact [info@innovocad.com](mailto:info@innovocad.com)

---
[![YouTube](https://www.innovocad.com/images/VSMarketplace/YouTube.png)](https://www.youtube.com/channel/UC9jL56FG4IN4uBpO1Lyedfg)
[![Facebook](https://www.innovocad.com/images/VSMarketplace/Facebook2.png)](https://www.facebook.com/innovocad)
[![Facebook](https://www.innovocad.com/images/VSMarketplace/Twitter.png)](https://www.twitter.com/innovocad)
