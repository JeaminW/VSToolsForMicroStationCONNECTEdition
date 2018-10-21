#if !defined(LA_SOLUTIONS_TSTRING_INCLUDED_H_)
#define LA_SOLUTIONS_TSTRING_INCLUDED_H_

//////////////////////////////////////////////////////////////////////
//	There's another tstring.h somewhere
//////////////////////////////////////////////////////////////////////
#include <string>
#if defined (_UNICODE)
	using tstring = std::wstring;
#else
	using tstring = std::string;
#endif
#endif	//	LA_SOLUTIONS_TSTRING_INCLUDED_H_
