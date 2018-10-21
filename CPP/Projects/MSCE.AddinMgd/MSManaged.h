/*--------------------------------------------------------------------------------------+
|
|      $safeprojectname$.h
|
+--------------------------------------------------------------------------------------*/
#pragma once
#include "StdAfx.h"

#using <mscorlib.dll>
#using <System.dll>
#using <System.Drawing.dll>
#using <System.Windows.Forms.dll>
#using <Bentley.DgnDisplayNet.dll>
#using <Bentley.DgnPlatformNET.dll>
#using <Bentley.General.1.0.dll>
#using <Bentley.GeometryNET.dll>
#using <Bentley.MicroStation.Interfaces.1.0.dll>
#using <Bentley.MicroStation.dll>
#using <Bentley.MicroStation.WinForms.Docking.dll>
#using <Bentley.MicroStation.WPF.dll>
#using <Bentley.Windowing.dll>
#using <ustation.dll>

using namespace System::Runtime::InteropServices;
namespace SRI = System::Runtime::InteropServices;

namespace $safeprojectname$
{
	/*--------------------------------------------------------------------------------------+
	|   This is the class that derives from the AddIn base class.  This will allow the
	|	application to load through the MDL loader.
	+--------------------------------------------------------------------------------------*/

	$AddinAttribute$
	public ref class Program : public Bentley::MstnPlatformNET::AddIn {

	public:static Bentley::MstnPlatformNET::AddIn^ MSAddin;
		   /*--------------------------------------------------------------------------------------+
		   |   The constructor for the AddIn subclass. This is where applications need to load any
		   |	resources that will be used in the code such as commandtables.
		   +--------------------------------------------------------------------------------------*/
	public: Program(System::IntPtr mdlDesc) : Bentley::MstnPlatformNET::AddIn(mdlDesc) {
		MSAddin = this;
	}

			/*--------------------------------------------------------------------------------------+
			|   This is the public entry point for the underlying application.
			+--------------------------------------------------------------------------------------*/
	public:virtual int Run(array<System::String^>^ commandLine) override {
		Program::MSAddin = this;
		return 0;
	}

		   /*--------------------------------------------------------------------------------------+
		   |   A static method used for getting a reference to the host addin.
		   +--------------------------------------------------------------------------------------*/
		   static Bentley::MstnPlatformNET::AddIn^ GetApp() {
			   return MSAddin;
		   }

	};
};

