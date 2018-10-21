/*--------------------------------------------------------------------------------------+
|   $safeitemname$.r
|
+--------------------------------------------------------------------------------------*/

#include <Mstn\MdlApi\rscdefs.r.h>

/*----------------------------------------------------------------------+
| Required resource for a native-code-only application.                 |
-----------------------------------------------------------------------*/
#define DLLAPP_$safeprojectname$  1

DllMdlApp DLLAPP_$safeprojectname$ =
    {
		L"$safeprojectname$", L"$safeprojectname$" // taskid, dllName
    }
