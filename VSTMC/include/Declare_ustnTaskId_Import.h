#if !defined(DECLARE_USTNTASKID_H_INCLUDED_)
#define DECLARE_USTNTASKID_H_INCLUDED_
#pragma once

#include <Bentley/Bentley.h>

///	<summary><c>ustnTaskId</c> is a public MDL variable.
///	<remarks><c>ustnTaskId</c> is used in functions such as <c>mdlInput_sendCommand ()</c>.
///	<para>Usage: <c>#include &lt;Declare_ustnTaskId_Import.h&gt;</c>.</para>
///	<para>Link with <c>mdlbltin.lib</c>.</para>
///	</remarks>
///	</summary>
BEGIN_EXTERN_C

BENTLEYDLL_EXPORT extern WCharCP      ustnTaskId;

END_EXTERN_C

#endif	// !defined(DECLARE_USTNTASKID_H_INCLUDED_)
