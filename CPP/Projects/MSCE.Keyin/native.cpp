/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cpp
|
+--------------------------------------------------------------------------------------*/

#include	<Mstn\MdlApi\dlogitem.h>
#include    "$safeprojectname$Cmd.h"
#include    "$safeprojectname$.h"
#include    "MicroStationUtilities.h"

$AddinAttribute$

/*----------------------------------------------------------------------+
* The command entry point for user function.				            *
* @param        unparsed     unused unparsed argument to the command    *
+----------------------------------------------------------------------*/
void yourFunctionName(WCharCP unparsed)
{

}

/*----------------------------------------------------------------------+
* The command entry point for DLLEXPORT function.	                    *
* @param        unparsed     unused unparsed argument to the command    *
+----------------------------------------------------------------------*/
extern "C" DLLEXPORT void yourExportFunctionName(WCharCP unparsed)
{

	return;
}

/*----------------------------------------------------------------------------------+
* The main entry point for the native code MDL application.						    *
* @param        argc     The count of args passed to the application			    *
* @param        argv[]   The array of args passed to this application as char arrays*
* @return																		    *
+-----------------------------------------------------------------------------------*/
extern "C" DLLEXPORT  void MdlMain(int argc, WCharCP argv[])
{
	// Map key-in to function
	static MdlCommandNumber cmdNumbers[] =
	{
		{ (CmdHandler)yourFunctionName,  CMD_COMMAND_OPEN },
		0,
	};
	//  MESSAGELIST_Commands and MESSAGELIST_Prompts are your message list IDs
	MicroStation::MdlMain(cmdNumbers, MESSAGELIST_Commands, MESSAGELIST_Prompts);
}