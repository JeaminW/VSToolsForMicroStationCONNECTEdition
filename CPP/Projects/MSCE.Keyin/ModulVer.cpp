////////////////////////////////////////////////////////////////
// 1998 Microsoft Systems Journal
// If this code works, it was written by Paul DiLascia.
// If not, I don't know who wrote it.
//
// CModuleVersion provides an easy way to get version info
// for a module.(DLL or EXE).
//
//  Special thanks to Jon Summers, Bentley MVP, Director, LA Solutions,
//  http://www.la-solutions.co.uk/ for his contribution of the MicroStation
//	Utilities.
////////////////////////////////////////////////////////////////
#include	"ModulVer.h"

CModuleVersion::CModuleVersion()
{
	pVersionInfo_ = nullptr;				// raw version info data
}

//////////////////
// Destroy: delete version info
//
CModuleVersion::~CModuleVersion()
{
	delete[] pVersionInfo_;
}

//////////////////
// Get file version info for a given module
// Allocates storage for all info, fills "this" with
// VS_FIXEDFILEINFO, and sets codepage.
//
BOOL CModuleVersion::GetFileVersionInfo(LPCWSTR modulename)
{
	m_translation.charset = 1252;		// default = ANSI code page
	memset((VS_FIXEDFILEINFO*)this, 0, sizeof(VS_FIXEDFILEINFO));

	// get module handle
	wchar_t filename[_MAX_PATH];
	HMODULE hModule = ::GetModuleHandle(modulename);
	if (hModule == nullptr && modulename != nullptr)
		return FALSE;

	// get module file name
	DWORD len = GetModuleFileName(hModule, filename,
		sizeof(filename) / sizeof(filename[0]));
	if (len <= 0)
		return FALSE;

	// read file version info
	DWORD dwDummyHandle; // will always be set to zero
	len = GetFileVersionInfoSize(filename, &dwDummyHandle);
	if (len <= 0)
	{
		//DWORD error = ::GetLastError ();
		return FALSE;
	}

	pVersionInfo_ = new BYTE[len]; // allocate version info
	if (!::GetFileVersionInfo(filename, 0, len, pVersionInfo_))
		return FALSE;

	LPVOID lpvi;
	UINT iLen;
	if (!VerQueryValue(pVersionInfo_, L"\\", &lpvi, &iLen))
		return FALSE;

	// copy fixed info to myself, who is derived from VS_FIXEDFILEINFO
	*(VS_FIXEDFILEINFO*)this = *(VS_FIXEDFILEINFO*)lpvi;

	// Get translation info
	if (VerQueryValue(pVersionInfo_,
		L"\\VarFileInfo\\Translation", &lpvi, &iLen) && iLen >= 4)
	{
		m_translation = *(TRANSLATION*)lpvi;
		//TRACE("code page = %d\n", m_translation.charset);
	}

	return dwSignature == VS_FFI_SIGNATURE;
}
//////////////////
// Get string file info.
// Key name is something like "CompanyName".
// returns the value as a CString.
//
std::wstring CModuleVersion::GetValue(wchar_t const* lpKeyName)
{
	std::wstring	sVal;
	if (pVersionInfo_)
	{	// To get a string value must pass query in the form
		//
		//    "\StringFileInfo\<langID><codepage>\keyname"
		//
		// where <lang-codepage> is the languageID concatenated with the
		// code page, in hex. Wow.
		//
		//CString query;
		//query.Format(_T("\\StringFileInfo\\%04x%04x\\%s"),
		//	m_translation.langID,
		//	m_translation.charset,
		//	lpKeyName);
		wchar_t	query[64];
		swprintf_s(query, L"\\StringFileInfo\\%04x%04x\\%s",
			m_translation.langID,
			m_translation.charset,
			lpKeyName);

		wchar_t const* pVal;
		UINT iLenVal;
		if (VerQueryValue(pVersionInfo_, query,
			(LPVOID*)&pVal, &iLenVal)) {

			sVal = pVal;
		}
	}
	return sVal;
}
// typedef for DllGetVersion proc
typedef HRESULT(CALLBACK* DLLGETVERSIONPROC)(DLLVERSIONINFO *);

/////////////////
// Get DLL Version by calling DLL's DllGetVersion proc
//
BOOL CModuleVersion::DllGetVersion(wchar_t const* modulename, DLLVERSIONINFO& dvi)
{
	HINSTANCE hinst = LoadLibrary(modulename);
	if (!hinst)
		return FALSE;

	// Must use GetProcAddress because the DLL might not implement
	// DllGetVersion. Depending upon the DLL, the lack of implementation of the
	// function may be a version marker in itself.
	//
	DLLGETVERSIONPROC pDllGetVersion =
		(DLLGETVERSIONPROC)GetProcAddress(hinst, "DllGetVersion");

	if (!pDllGetVersion)
		return FALSE;

	memset(&dvi, 0, sizeof(dvi));			 // clear
	dvi.cbSize = sizeof(dvi);				 // set size for Windows

	return SUCCEEDED((*pDllGetVersion)(&dvi));
}
