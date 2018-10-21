////////////////////////////////////////////////////////////////
// 1998 Microsoft Systems Journal
//
// If this code works, it was written by Paul DiLascia.
// If not, I don't know who wrote it.
//
//  Special thanks to Jon Summers, Bentley MVP, Director, LA Solutions,
//  http://www.la-solutions.co.uk/ for his contribution of the MicroStation
//	Utilities.
////////////////////////////////////////////////////////////////
#ifndef MODULE_VERSION_H_INCLUDED_
#define MODULE_VERSION_H_INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

//#include <AFX_OR_ATL_HEADER.h>
#include <tchar.h>					//	Microsoft TCHAR macros
#include <string>
#include <shlwapi.h>

// tell linker to link with version.lib for VerQueryValue, etc.
#pragma comment(linker, "/defaultlib:version.lib")

// following is from shlwapi.h, in November 1997 release of the Windows SDK
#if !defined (_INC_SHLWAPI) && !defined (DLLVERSIONINFO)

/*
typedef struct _DllVersionInfo
{
DWORD cbSize;
DWORD dwMajorVersion;                   // Major version
DWORD dwMinorVersion;                   // Minor version
DWORD dwBuildNumber;                    // Build number
DWORD dwPlatformID;                     // DLLVER_PLATFORM_*
} DLLVERSIONINFO;

// Platform IDs for DLLVERSIONINFO
#define DLLVER_PLATFORM_WINDOWS         0x00000001      // Windows 95
#define DLLVER_PLATFORM_NT              0x00000002      // Windows NT
*/
#endif // _INC_SHLWAPI && DLLVERSIONINFO

//////////////////
// CModuleVersion version info about a module.
// To use:
//
// CModuleVersion ver
// if (ver.GetFileVersionInfo(_T("mymodule"))) {
//		// info is in ver, you can call GetValue to get variable info like
//		CString s = ver.GetValue(_T("CompanyName"));
// }
//
// You can also call the static fn DllGetVersion to get DLLVERSIONINFO.
//
class CModuleVersion : public VS_FIXEDFILEINFO {
protected:
	BYTE * pVersionInfo_;		// all version info

	struct TRANSLATION {
		WORD langID;			// language ID
		WORD charset;			// character set (code page)
	} m_translation;

public:
	CModuleVersion();
	virtual ~CModuleVersion();

	BOOL			GetFileVersionInfo(wchar_t const* modulename);
	std::wstring	GetValue(wchar_t const* lpKeyName);

	static BOOL 	DllGetVersion(wchar_t const* modulename, DLLVERSIONINFO& dvi);
};

#endif	//	MODULE_VERSION_H_INCLUDED_
