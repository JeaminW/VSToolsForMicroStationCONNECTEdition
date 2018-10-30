//	std_collection_typedefs.h : provides a set of typedefs
//	for standard library collections and iterators

#if !defined(LA_SOLUTIONS_STD_TYPEDEFS_INCLUDED_H_)
#define LA_SOLUTIONS_STD_TYPEDEFS_INCLUDED_H_

//////////////////////////////////////////////////////////////////////
//	Standard Library
#include <string>
#include <set>
#include <vector>

#if defined (BENTLEY)
#	pragma message( "std_collection_typedefs header (BENTLEY defined)" )
//	MDL
#	include <Mstn/MdlApi/dbdefs.h>
#	include <Mstn/MdlApi/MdlApi.h>
#	include <DgnPlatform/DgnPlatformBaseType.r.h>
#	include <DgnPlatform/DgnPlatform.h>
#	include <DgnPlatform/DgnFileIO/ElementRefBase.h>
#	include <DgnPlatform/DgnFileIO/DgnElements.h>
#	include <Mstn/MdlApi/elementref.h>

//	Note that some collections are typedef'd for CONNECT in DgnPlatform.h and elsewhere.
//	For example,
//	typedef bmap<ElementId, ElementId> T_StdElementIDMap;
//	typedef bmap<ModelId,   ModelId>   T_StdModelIDMap;
//	typedef bvector<ElementId>         T_StdElementIdVector;

#else
#	pragma message( "std_collection_typedefs header (BENTLEY not defined)" )
#endif	//	BENTLEY
//////////////////////////////////////////////////////////////////////
#include "tstring_typedef.h"		//	There's another tstring.h somewhere

//	Iterate the characters in a <basic_string>
using CharIterator						= std::string::iterator;
using TCharIterator						= tstring::iterator;
using WideCharIterator					= std::wstring::iterator;

using CharConstIterator					= std::string::const_iterator;
using TCharConstIterator				= tstring::const_iterator;
using WideCharConstIterator				= std::wstring::const_iterator;

//	Collections of <basic_string> and their iterators
using StringCollection					= std::vector<std::string>;
using StringIterator					= StringCollection::iterator;
using StringConstIterator				= StringCollection::const_iterator;
using StringReverseIterator				= StringCollection::reverse_iterator;
using StringConstReverseIterator= StringCollection::const_reverse_iterator;

using TStringCollection					= std::vector<tstring>;
using TStringIterator					= TStringCollection::iterator;
using TStringConstIterator				= TStringCollection::const_iterator;
using TStringReverseIterator			= TStringCollection::reverse_iterator;
using TStringConstReverseIterator		= TStringCollection::const_reverse_iterator;

using WideStringCollection				= std::vector<std::wstring>;
using WideStringIterator				= WideStringCollection::iterator;
using WideStringConstIterator			= WideStringCollection::const_iterator;
using WideStringReverseIterator			= WideStringCollection::reverse_iterator;
using WideStringConstReverseIterator	= WideStringCollection::const_reverse_iterator;

using StringSet = std::set<std::string>;
using StringSetIterator = StringSet::iterator;
using StringSetConstIterator = StringSet::const_iterator;

using TStringSet				= std::set<tstring>;
using TStringSetIterator		= TStringSet::iterator;
using TStringSetConstIterator	= TStringSet::const_iterator;

using WStringSet				= std::set<std::wstring>;
using WStringSetIterator		= WStringSet::iterator;
using WStringSetConstIterator	= WStringSet::const_iterator;

using DoubleCollection			= std::vector<double>;
using DoubleIterator			= DoubleCollection::iterator;
using DoubleConstIterator		= DoubleCollection::const_iterator;

#if defined (BENTLEY)
	///	<summary>Collections using std::vector.
	///	<remarks>Note: bvector is also available as a templatised container compatible with the MicroStationAPI.</remarks></summary>
	using Int32Collection				= std::vector<Int32>;
	using Int32Iterator					= Int32Collection::iterator;
	using Int32ConstIterator			= Int32Collection::const_iterator;

	using UInt32Collection				= std::vector<UInt32>;
	using UInt32Iterator				= UInt32Collection::iterator;
	using UInt32ConstIterator			= UInt32Collection::const_iterator;

	///	<summary>	MicroStation database entity nos.</summary>
	using EntityNumCollection			= std::vector<UShort>;
	using EntityNumIterator				= EntityNumCollection::iterator;
	using EntityNumConstIterator		= EntityNumCollection::const_iterator;

	using Point2dCollection				= std::vector <Point2d>;
	using Point2dIterator				= Point2dCollection::iterator;
	using Point2dConstIterator			= Point2dCollection::const_iterator;
	using Point2dReverseIterator		= Point2dCollection::reverse_iterator	;
	using Point2dConstReverseIterator	= Point2dCollection::const_reverse_iterator;

	using Point3dCollection				= std::vector <Point3d>;
	using Point3dIterator				= Point3dCollection::iterator;
	using Point3dConstIterator			= Point3dCollection::const_iterator;
	using Point3dReverseIterator		= Point3dCollection::reverse_iterator;
	using Point3dConstReverseIterator	= Point3dCollection::const_reverse_iterator;

	using DPoint2dCollection			= std::vector <DPoint2d>;
	using DPoint2dIterator				= DPoint2dCollection::iterator;
	using DPoint2dConstIterator			= DPoint2dCollection::const_iterator;
	using DPoint2dReverseIterator		= DPoint2dCollection::reverse_iterator;
	using DPoint2dConstReverseIterator	= DPoint2dCollection::const_reverse_iterator;

	using DPoint3dCollection			= std::vector <DPoint3d>;
	using DPoint3dIterator				= DPoint3dCollection::iterator;
	using DPoint3dConstIterator			= DPoint3dCollection::const_iterator;
	using DPoint3dReverseIterator		= DPoint3dCollection::reverse_iterator;
	using DPoint3dConstReverseIterator	= DPoint3dCollection::const_reverse_iterator;

	using BentleyVectorDPoint3d			= bvector<DPoint3d>;
	using BentleyVectorDPoint3dR		= bvector<DPoint3d>&;
	using BentleyVectorDPoint3dCR		= bvector<DPoint3d> const&;
	using BentleyPoint3dIter			= BentleyVectorDPoint3d::iterator;
	using BentleyPoint3dIterC			= BentleyVectorDPoint3d::const_iterator;

	///	<summary>	MicroStation Element IDs are _int64</summary>
	using ElementIdCollection			= std::vector<DgnPlatform::ElementId>;
	using ElementIdIterator				= ElementIdCollection::iterator;
	using ElementIdConstIterator		= ElementIdCollection::const_iterator;

	//	Bentley RGB Color
	using RGBColorDefCollection			= std::vector<RgbColorDef>;
	using RGBColorDefIterator			= RGBColorDefCollection::iterator;
	using RGBColorDefConstIterator		= RGBColorDefCollection::const_iterator;

	using StrokedArrays					= std::vector<DPoint3dCollection>;
	using StrokedArrayIterator			= StrokedArrays::iterator;
	using StrokedArrayConstIterator		= StrokedArrays::const_iterator;

	////////////////////////////////////////////////		//////////////////////
	using DatabaseLinkCollection		= std::vector<DatabaseLink>;
	using DatabaseLinkIterator			= DatabaseLinkCollection::iterator;
	using DatabaseLinkConstIterator		= DatabaseLinkCollection::const_iterator;

#endif //	defined BENTLEY

#endif // !defined(LA_SOLUTIONS_STD_TYPEDEFS_INCLUDED_H_)
