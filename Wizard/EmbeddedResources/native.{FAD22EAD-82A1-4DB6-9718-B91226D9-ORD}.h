/*--------------------------------------------------------------------------------------+
|   $safeitemname$.h
|
+--------------------------------------------------------------------------------------*/

#ifndef     __$safeitemname$H__
#define     __$safeitemname$H__

#pragma once

#include <Geom/GeomApi.h>
#include <DgnPlatform/DgnPlatformAPI.h>
#include <Mstn/MstnPlatformAPI.h>
#include <Mstn/MdlApi/MdlApi.h>
#include <Cif/SDK/CIFGeometryModelSDK.h>
#include <Cif/Bentley.Cif.h>
#include <Cif/LinearGeometry/Bentley.Cif.LinearGeometry.h>
#include <wchar.h>


/*----------------------------------------------------------------------+
|                                                                       |
|   Message List ID's                                                   |
|                                                                       |
+----------------------------------------------------------------------*/
enum
{
	MESSAGELIST_Commands = 1,
};

enum
{
	MESSAGELIST_Prompts = 100,
};

#endif
