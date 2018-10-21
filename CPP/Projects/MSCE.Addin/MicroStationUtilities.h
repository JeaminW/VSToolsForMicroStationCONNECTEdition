//////////////////////////////////////////////////////////////////////
//
///	<summary>MicroStationUtilities.h: interface for the MicroStationUtilities class.</summary>
//  Special thanks to Jon Summers, Bentley MVP, Director, LA Solutions,
//  http://www.la-solutions.co.uk/ for his contribution of the MicroStation
//	Utilities.
//
//////////////////////////////////////////////////////////////////////

#if !defined(MICROSTATION_UTILITIES_H_INCLUDED_)
#define MICROSTATION_UTILITIES_H_INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#if !defined (winNT)
#define winNT
#endif

#if !defined (BENTLEY)
#	define BENTLEY
#endif

#define NO_BOOLEAN_TYPE

#if !defined (NORECTANGLE)
#	define NORECTANGLE
#endif

//	Standard Library
#include <vector>

#pragma region	MicroStationAPI
#include <Mstn/MdlApi/MdlApi.h>
#include <DgnPlatform/DgnPlatform.h>
#include <DgnPlatform/DgnPlatform.r.h>
#include <DgnPlatform/DgnFileIO/ElementRefBase.h>
#include <DgnPlatform/DgnFileIO/DgnElements.h>
#include <DgnPlatform/DgnFile.h>
#include <Mstn/MdlApi/elementref.h>
#pragma endregion
//	Win32
///	<summary>Include path must have C:/Program Files (x86)/Microsoft SDKs/Windows/v7.1A/Include.</summary>
//	for 2015 build
//	OLE Colour functions moved to OleColours.h
//	Last Modified functions moved to LastModified.h
///	<summary>Annoying Win32 macros no longer required since Bentley created typedef MSDialog.</summary>
#ifdef DialogBox
#undef DialogBox
#endif

//	LA_INCLUDE
//#include <la.h>
#include <std_collection_typedefs.h>


//////////////////////////////////////////////////////////////////////
///	<summary>MicroStation namespace contains a number of utilities.</summary>
///	<remarks>Version 10.05.</remarks>
namespace MicroStation
{
	///	<summary>Number of vertices in specified polygons.</summary>
	///	<remarks>A MicroStation closed shape has coincident start and end coordinates.
	///	</remarks>
	enum ShapeControl
	{
		TriangleVertexCount = 3 + 1,
		RectangleVertexCount = 4 + 1,
	};

	enum FillMode
	{   //  See mdlShape_create
		FillModeActive = -1,
		FillModeNone = 0,
		FillModeFilled = 1,
	};

	enum	Misc
	{
		MaxLineWeight = 31,
	};
	/// <summary>	Cap a line weight to the maximum value. </summary>
	UInt32	CapLineWeight(UInt32					weight);

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Window Cursor lets us specify a cursor to be shown in MicroStation. </summary>
	/// <remarks>	Introduced with v8.11.07.
	/// 			RAII idom applied to the MicroStation system cursor.<para>
	/// 			The destructor returns the cursor to its initial type.</para>
	///				<para>Cursor IDs from enum tagSYSTEMCURSOR in dlogbox.h</para>
	/// 			</remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	struct WindowCursor
	{
		WindowCursor(SYSTEMCURSOR  cursor);
		~WindowCursor();
	};
	//////////////////////////////////////////////////////////////////////
	/// <summary>	Unload the current MDL task </summary>
	void			UnloadCurrentTask();
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Busy Cursor lets us change the cursor to be shown in MicroStation. </summary>
	///
	/// <remarks>	The destructor returns the cursor to its initial type</remarks>
	struct BusyCursor
	{
		BusyCursor();
		~BusyCursor();
	};
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Busy Bar lets us pop a progress to be shown in MicroStation. </summary>
	///
	/// <remarks>	The destructor removes the Busy Bar.</remarks>
	struct BusyBar
	{
		void	UpdateMessage(wchar_t const*			msg);
		BusyBar(wchar_t const*			title,
			wchar_t const*			msg);
		~BusyBar();
	};

	/// <summary>   Queue a command to unload the owner MDL task </summary>
	void						QueueMdlUnload();
	/// <summary>
	///     <para>Generate a MicroStation version number from our resource.
	/// mdlSystem_setMdlAppVersionNumber () fills in a VersionNumber
	/// struct to set  an MDL version number from a native-code DLL:</para>
	///<c>basetype.h
	/// typedef struct versionnumber
	///     {
	///     UInt16   release:8;
	///     UInt16   major:8;
	///     UInt16   minor:8;
	///     UInt16   subMinor:8;
	///     } VersionNumber;</c>
	/// <para>mdlSystem_getTaskStatistics () fills in a ProgramVersionNumber
	/// struct to obtain an MDL version number:</para>
	///<c>system.h
	/// typedef struct programVersionNumber
	///     {
	///     unsigned short   release:8;
	///     unsigned short   major:8;
	///     unsigned short   minor:16;
	///     } ProgramVersionNumber;</c>
	/// <para>When I set a VersionNumber using mdlSystem_setMdlAppVersionNumber (),
	/// it becomes mangled by the MDL->Detail dialog box. The difference is
	/// either due to misunderstanding on my part, or the fact that
	/// VersionNumber is a four-byte struct, whereas ProgramVersionNumber
	/// has three parts.</para>
	/// <para>Trouble Report TR # 97786 has been filed against this.
	/// A workaround is in this snippet:</para>
	/// <c>VersionNumber vn;
	/// vn.release = 5;     // Range: 0 - 255
	/// vn.major = 7;       // Range: 0 - 255
	/// vn.minor = 1;       // Range: 0       for 05.06.00 or lower,
	///                               0 - 255 for 05.07.00.00 or higher
	/// vn.subMinor = 3;    // Range: 0 - 255</c>
	/// <para>Only set the minor field of this structure greater than zero
	/// if your application is specifying a release of 05.07.00 or higher.
	/// Using values lower than this will default to an internal format of
	/// "rel.maj.sub", not "rel.maj.min.sub" as would be expected.</para>
	/// <para>Set our .dll/.ma version number as seen in the "Details" dialog box
	/// mdlSystem_setMdlAppVersionNumber(NULL, &amp;vn);</para>
	/// </summary>
	///
	/// <param name="dllName">  Name of the dll. </param>
	bool						SetVersion(Bentley::WCharCP		dllName);
	///	<summary>VersionString gets a DLL version no. as a string</summary>
	Bentley::WString			VersionString(Bentley::WCharCP		dllName);
	/// <summary>Code that always belongs in your Application.MdlMain but is
	/// not dependent on your application headers
	/// </summary>
	/// <remarks>This code will queue an MDL UNLOAD command if a fatal error occurs.
	///	<para>Viz Studio may use a period or comma in a version string: 1.2.3.4 or 1,2,3,4</para></remarks>
	RscFileHandle				MdlMain(MdlCommandNumber		cmdNumbers[],
		long					commands,
		long					prompts);
	/// <summary>   Determines whether the default model in a DGN file is 3D </summary>
	///
	/// <remarks>   Accepts file path as a C-style multibyte string </remarks>
	///
	/// <param name="filePath"> Full pathname of the file. </param>
	///
	/// <returns>   true if 3D, false otherwise. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Determines whether the default model in a DGN file is 3D </summary>
	///
	/// <remarks>   Accepts file path as a wchar_t string </remarks>
	///
	/// <param name="filePath"> Full pathname of the file. </param>
	///
	/// <returns>   true if 3D, false otherwise. </returns>
	bool						DefaultModelIs3D(Bentley::WCharCP 		filePath);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	File format DGN8, DGN7, DWG, etc. </summary>
	/// <param name="filePath">	Full pathname of the file as a <c>wchar_t*</c> </param>
	/// <param name="is3D">		[in,out] If non-null, the value is TRUE if the file default model is 3D</param>
	/// <returns>	DgnFileFormatType <c>enum</c> value </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	Bentley::DgnPlatform::DgnFileFormatType			FileFormat(Bentley::WCharCP		filePath,
		bool*					is3D = NULL);

#if defined (IMODEL_API)
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Query if the active design file is an i-Model. </summary>
	/// <remarks>   V8i SS3 and later</remarks>
	/// <returns>	true if i-Model. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						IsIModel();
#endif
	/// <summary>Describe a CAD file format.</summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <remarks>	#include &lt;msdefs.h&gt; </remarks>
	/// <param name="format">	Describes the format to use. </param>
	/// <returns>	<c>tstring</c> description </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				DescribeFileFormat(Bentley::DgnPlatform::DgnFileFormatType		format);
	std::wstring				DescribeElement(UInt32					filePos,
		DgnModelRefP			modelRef);
	std::wstring				DescribeElement(const ElementRefP		elemRef,
		DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Queries if we can open 'filePath' with MicroStation </summary>
	/// <remarks>	<c>mdlFile_validDesignFile</c> occasionally returns valid for non-CAD files</remarks>
	/// <param name="filePath">	Full pathname of the file as a <c>char*</c> </param>
	/// <returns>	true if the file can be opened by MicroStation, false otherwise. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						CanOpen(Bentley::WCharCP		filePath);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Publish a complex variable to make it visible to
	///             MicroStation's Dialog Manager </summary>
	///
	/// <remarks>   V 8i, 09/11/2010. </remarks>
	///
	/// <param name="data">         data </param>
	/// <param name="tagName">      Name of the <c>struct</c>'s tag </param>
	/// <param name="variableName"> Name of the variable. </param>
	///
	/// <returns>   true if it succeeds, false if it fails. </returns>
	bool						PublishComplexVariable(const void* 			data,
		char const* 			tagName,
		char const* 			variableName);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Reference model name. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	wstring containing the model name </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				RefModelName(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Reference model description. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	wstring containing the model description </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				RefDescription(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Reference model logical name. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	wstring containing the model  logical name </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				RefLogicalName(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Reference model attachment name. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	wstring containing the model  attachment name </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				RefAttachName(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Fully resolved reference file name. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	wstring containing the fully-resolved reference file name </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				RefFileName(DgnModelRefP			modelRef);

	UInt32						ElementCount(DgnModelRefP			modelRef);


	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get the master unit name of a specified model. </summary>
	/// <remarks>	V 8i, 04/12/2011. </remarks>
	/// <param name="modelRef">	The model reference. </param>
	/// <returns>	The unit name e.g. 'm'. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				MasterUnitName(DgnModelRefP			modelRef = ACTIVEMODEL);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get the sub unit name of a specified model. </summary>
	/// <remarks>	V 8i, 04/12/2011. </remarks>
	/// <param name="modelRef">	The model reference. </param>
	/// <returns>	The unit name e.g. 'mm'. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				SubUnitName(DgnModelRefP			modelRef = ACTIVEMODEL);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get the number of models in the DGN file that hosts the specified model. </summary>
	/// <remarks>	V 8i, 04/12/2011. </remarks>
	/// <param name="modelRef">	The model reference. </param>
	/// <returns>	Count of all models, including the specified model. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	UInt32						ModelCount(DgnModelRefP			modelRef = ACTIVEMODEL);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Returns a level description from a level ID </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <param name="levelId">	Identifier for the level. </param>
	/// <returns>	String containing the level description </returns>
	std::wstring				LevelDescr(DgnModelRefP			modelRef,
		UInt32					levelId);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Returns a level name from an ElemHandle </summary>
	/// <remarks>	You probably need to check that the element is graphical before calling this function. </remarks>
	///	<see>IsGraphicalElement</see>
	/// <param name="eh">	Element handle. </param>
	///
	/// <returns>	String containing the level name </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				LevelName(ElementHandleCR			eh);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Returns a level name from a level ID </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <param name="levelId">	Identifier for the level. </param>
	///
	/// <returns>	String containing the level name </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				LevelName(DgnModelRefP			modelRef,
		UInt32					levelId);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Returns a model's file name as a <c>std::wstring</c> </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	<c>String</c> containing the file name </returns>
	std::wstring				ModelFileName(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Returns a model's name as a <c>std::wstring</c> </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">	The model reference. </param>
	/// <returns>	<c>std::wstring</c> containing the model name </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				ModelName(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Query if 'modelRef' is cell library. </summary>
	/// <remarks>	V 8i, 29/11/2011. </remarks>
	/// <param name="modelRef">	The model reference. </param>
	/// <returns>	true if cell library, false if not. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						IsCellLibrary(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Query if <c>modelRef</c> is same model as another named model. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">		A model reference. </param>
	/// <param name="modelName">	Name of another model. </param>
	/// <returns>	true if same model, false if not. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						IsSameModel(DgnModelRefP			modelRef,
		Bentley::WCharCP		modelName);
	bool						IsSameModel(DgnModelRefP			modelRef,
		const std::wstring&		modelName);
	bool						IsSameModel(DgnModelRefP			modelRef1,
		DgnModelRefP			modelRef2);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Creates a new design file.
	/// 			Wrapper around mdlSystem_newDesignFile that takes care of version-
	///	dependent issues for read-only files
	///	</summary>
	///
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	///
	/// <param name="filePath">		Full pathname of the file as a <c>wchar_t*</c> </param>
	/// <param name="modelName">	Name of the model as a <c>wchar_t*</c>  </param>
	/// <param name="readOnly">		true to read only. </param>
	///
	/// <returns>	Status code returned by <c>mdlSystem_newDesignFile</c>, typically SUCCESS </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						NewDesignFile(const wchar_t*			filePath,
		const wchar_t*			modelName = NULL,
		bool					readOnly = false);
	bool						NewDesignFile(const std::wstring&	filePath,
		const std::wstring&		modelName = L"",
		bool					readOnly = false);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Trace list model. </summary>
	///
	/// <remarks>   For debugging. </remarks>
	///
	/// <param name="pListModel">   A list model. </param>
	void						TraceListModel(ListModel const*		pListModel);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Extracts a  wide text string from a text element </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="text">			[out] The text as a <c>std::wstring</c> </param>
	/// <param name="origin">		[out] The text element's origin. </param>
	/// <param name="textParam">	[in,out] If non-null, the <c>TextParamWide</c> data. </param>
	/// <param name="el">			The el. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	//////////////////////////////////////////////////////////////////
	/*
	bool						ExtractTextWide			(Bentley::WString&		text,
	DPoint3d&				origin,
	TextParamWide*			textParam,	//	NULL OK
	MSElement const*		el);
	bool						ExtractTextWide			(std::wstring&			text,
	DPoint3d&				origin,
	TextParamWide*			textParam,	//	NULL OK
	MSElement const*		el);
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Create an new text element ready to replace an existing text element </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="text">		The replacement text. </param>
	/// <param name="old">		The old text element. </param>
	/// <param name="buffer">	[in,out] A buffer to store the new text element. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	//////////////////////////////////////////////////////////////////
	/*
	bool						RecreateText			(LPCWTSTR				text,
	MSElementCP				old,
	MSElementP				buffer);
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Create a text element using Unicode </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="location">	The text origin. </param>
	/// <param name="text">		A Unicode text string. </param>
	/// <param name="buffer">	[in,out] A buffer to store the element. </param>
	/// <param name="size">		<c>TextSizeParam</c> data. </param>
	/// <param name="param">	<c>TextParamWide</c> data. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	bool						CreateTextWide			(DPoint3d const&		location,
	std::wstring const&		text,
	MSElement*				buffer,
	TextSizeParam const*	size,
	TextParamWide const*	param);
	bool						CreateTextWide			(DPoint3d const&		location,
	Bentley::WString const&	text,
	MSElement*				buffer,
	TextSizeParam const*	size,
	TextParamWide const*	param);
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Create a text element using Unicode </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="location">	The text origin. </param>
	/// <param name="text">		A multibyte text string. </param>
	/// <param name="buffer">	[in,out] A buffer to store the element. </param>
	/// <param name="size">		<c>TextSizeParam</c> data. </param>
	/// <param name="param">	<c>TextParamWide</c> data. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	bool						CreateTextWide			(DPoint3d const&		location,
	std::string const&		text,
	MSElement*				buffer,
	TextSizeParam const*	size,
	TextParamWide const*	param);
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Create text element and either add to a model or make a transient </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="location">		The text origin. </param>
	/// <param name="text">			The  multibyte text string. </param>
	/// <param name="buffer">		[in,out] A buffer to store the new text element. </param>
	/// <param name="size">			<c>TextSizeParam</c> data. </param>
	/// <param name="colour">		The colour. </param>
	/// <param name="permanent">	Determines whether to create a permanent or a transient element. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	bool						CreateText				(DPoint3d const&		location,
	LPCTSTR					text,
	MSElement*				marker,
	TextSizeParam const*	size,
	const UInt32&			colour,
	bool					permanent);
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Create a named cell header </summary>
	/// <remarks>	V 8i, 22/02/2012. </remarks>
	/// <param name="header">		Element Descriptor to be allocated. </param>
	/// <param name="name">			The  Unicode cell name. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						CreateCellHeader(MSElementDescrP*		header,
		const wchar_t*			name);
	bool						CreateCellHeader(MSElementDescrP*		header,
		const std::wstring&		name);
	bool						CreateCellHeader(MSElementDescrP*		header,
		const Bentley::WString&	name);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Create circle element and either add to a model or make a transient </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="location">		The text origin. </param>
	/// <param name="buffer">		[in,out] A buffer to store the new marker element. </param>
	/// <param name="size">			The size. </param>
	/// <param name="colour">		The colour. </param>
	/// <param name="permanent">	true to make permanent, otherwise make transient. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						CreateMarker(DPoint3d const&		location,
		MSElementP				marker,
		double					size,
		const UInt32&			colour,
		bool					permanent);

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	IsGraphicalElement. </summary>
	/// <remarks>	Determines whether an element is graphical. </remarks>
	/// <param name="eh">	The Element Handle. </param>
	/// <returns>	true if the element is graphical. </returns>
	bool 											IsGraphicalElement(ElementHandleCR			eh);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Gets an element ID. </summary>
	/// <remarks>	Missing API for Element Handle. </remarks>
	/// <param name="eh">	The Element Handle. </param>
	/// <returns>	The element ID. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	Bentley::DgnPlatform::ElementId					GetElementID(ElementHandleCR			eh);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get the level ID of a named level in a specified model reference. </summary>
	/// <remarks>	V 8i, 04/12/2011. </remarks>
	/// <param name="name">		The level name. </param>
	/// <param name="modelRef">	The model reference. </param>
	/// <returns>	Level ID. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	UInt32						LevelId(Bentley::WCharCP		name,
		DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get the level ID of a specified reference attachment. </summary>
	/// <remarks>	V 8i, 04/12/2011. </remarks>
	/// <param name="modelRef">	The model reference. </param>
	/// <returns>	Level ID. </returns>
	//////////////////////////////////////////////////////////////////
	UInt32						LevelId(DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Query if 'pElm' is a cell component. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="pElm">	An element descriptor </param>
	/// <returns>	true if cell component, false if not. </returns>
	bool						IsCellComponent(MSElementDescrCP	pElm);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Apply a colour to components of a complex element. </summary>
	///
	/// <remarks>	This is a wrapper for const-correctness. </remarks>
	///
	/// <param name="el">		[in,out] If non-null, the element. </param>
	/// <param name="colour">	The colour. </param>
	void						SetColour(MSElementDescrP		el,
		const UInt32&			colour);
	void						SetColour(MSElementP				el,
		const UInt32&			colour);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Set a Color Book's named colour as MicroStation's Active Colour </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="book">		A Color Book name. </param>
	/// <param name="colour">	A colour. </param>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	void						ActiveColour(wchar_t const*			book,
		wchar_t const*			colour);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Convert a string representing a Hexadecimal format number to an MDL color definition </summary>
	/// <remarks>	V 8i, 12/02/2012. </remarks>
	/// <param name="hex">		A hex string. </param>
	/// <returns>	Bentley::RgbColorDef struct. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	Bentley::RgbColorDef					ConvertString2ColorDef(Bentley::WCharCP			hex);
	Bentley::RgbColorDef					ConvertString2ColorDef(const std::wstring&		hex);

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Obtain angle between two linear elements at a point of intersection </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="rotation">		[in,out] The rotation. </param>
	/// <param name="line1">		[in,out] The first line. </param>
	/// <param name="line2">		[in,out] The second line. </param>
	/// <param name="point1">		The first point. </param>
	/// <param name="point2">		The second point. </param>
	/// <param name="tolerance">	The tolerance. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	bool						AngleBetweenLines		(RotMatrix&				rotation,
	CElementDescr&			line1,
	CElementDescr&			line2,
	DPoint3d const*			point1,
	DPoint3d const*			point2,
	double					tolerance);
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Convert UORs to master units. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="out_master_units">	[out] Master units out. </param>
	/// <param name="in_uors">			The UORs. </param>
	/// <param name="modelRef">			A model reference. </param>
	void						Cnv2MasterUnits(DPoint3d&				out_master_units,
		const DPoint3d&			in_uors,
		DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Convert master units to UORs. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="in_master_units">	[in] Master units. </param>
	/// <param name="out_uors">			[out] UORs. </param>
	/// <param name="modelRef">			A model reference. </param>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	void						Cnv2UORs(DPoint3d&				out_uors,
		const DPoint3d&			in_master_units,
		DgnModelRefP			modelRef);
	bool						HasArea(MSElementCP			el);
	bool						HasArea(MSElementDescrCP		pElm);
	bool						HasArea(ElementRefP			elemRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Determine if an element is a point (zero length line). </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="pElm">	The elm. </param>
	/// <returns>	true if zero length line, false if not. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						IsZeroLengthLine(MSElementDescrCP	el);
	bool						IsZeroLengthLine(MSElementCP		el,
		DgnModelRefP		modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Determine if 'shape' is a rectangle. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="shape">	A shape element. </param>
	/// <returns>	true if rectangle, false if not. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						IsRectangle(MSElementDescrCP	shape);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	///	<summary> Initialise a point to negative infinity</summary>
	void						InitDPoint3dMin(DPoint3d& pt);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	///	<summary> Initialise a point to positive infinity</summary>
	void						InitDPoint3dMax(DPoint3d& pt);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	///	<summary> Initialise a point to zero</summary>
	///	<remarks>Same as mdlVec_zero but type-safe for DPoint3d.
	///	<para>Note that DVec3d and DPoint3d are structs in the MicroStationAPI and have their
	///	own methods for initialising and setting values.</para></remarks>
	void						InitDPoint3dZero(DPoint3d& pt);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Add element descriptor to a model if status is OK </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="pElm">		[in,out] If non-null, the element descriptor. </param>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	<c>UInt32</c> the file position of the newly-added element</returns>
	UInt32						AddToModel(MSElementDescrP		pDescr,
		DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Add or display an element.
	///	<remarks>An idiom used often in primitive commands</remarks>
	///	</summary>
	/// <returns>	<c>UInt32</c> the file position of the newly-added element, or zero if it is not added</returns>
	UInt32						AddOrDisplay(MSElementP				el,
		Bentley::DgnPlatform::DgnDrawMode	drawMode,
		DgnModelRefP			modelRef = ACTIVEMODEL);
	UInt32						AddOrDisplay(MSElementDescrP		pDescr,
		Bentley::DgnPlatform::DgnDrawMode	drawMode,
		DgnModelRefP			modelRef = ACTIVEMODEL);

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Provide comment on result of hatch/crosshatch/pattern function </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="status">		The status returned by a hatching function. </param>
	/// <param name="patternName">	Name of the pattern. </param>
	/// <param name="id">			The Element ID. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	bool						HatchStatus(int					status,
		const std::wstring&		patternName,
		const Bentley::DgnPlatform::ElementId&		id);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Sends a keyin to MicroStation. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="keyin">	The keyin string. </param>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	void						SendKeyin(Bentley::WCharCP		keyin);
	/// <summary>   Returns true if at least one active view has its fill setting enabled. </summary>
	///
	/// <remarks>   V 8i, 09/11/2010. </remarks>
	///
	/// <returns>   true if one or more views has fill setting enabled. </returns>
	bool						ViewSettingsFill();
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Remove fill pattern from specified element. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="id">		The element ID. </param>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	true if it succeeds, false if it fails. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						RemoveElementPattern(const Bentley::DgnPlatform::ElementId&		id,
		DgnModelRefP			modelRef);
	/// <summary>   Converts DgnModel precision to number
	///             of decimal digits. </summary>
	///
	/// <returns>   int precision. </returns>
	Bentley::PrecisionFormat	DecimalPrecision(DgnModelRefP			modelRef);

	///	<summary>Get rotation matrix from view</summary>
	///	<remarks>This is the inverse rotation that you can apply to an element
	///	for the user to place it naturally</remarks>
	void						GetViewRotation(RotMatrix&				r,
		int						viewNum);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Query if this object  model is writable. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="modelRef">			A model reference. </param>
	/// <param name="message_list_ID">	Identifier for the message list. </param>
	/// <param name="message_ID">		Identifier for the message. </param>
	/// <param name="alert">			true to alert. </param>
	/// <returns>	true if model writable, false if read-only. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	bool						IsModelWritable(DgnModelRefP			modelRef,
		UInt32					message_list_ID,
		UInt32					message_ID,
		bool					alert = true);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Attempt to attach a named cell library.  Library name may be naked,
	///	as we search for files having a .cel extension in the paths
	///	indicated by the configuration variable that defaults to <c>MS_CELL</c>.
	/// </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="library">	The library name. </param>
	/// <param name="cfgVar">	A configuration variable. </param>
	/// <returns>	A <c>DgnFileP</c> if the library is found, NULL otherwise. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	DgnFileP					AttachCellLibrary(const std::wstring&	library,
		Bentley::WCharCP		cfgVar = L"MS_CELL" /* CFGVAR_MS_CELL */);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get string length from an element descriptor that contains a text element. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="pText">	A text element. </param>
	/// <returns>	<c>UInt32</c> the length of the string in characters. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	UInt32						StringLength(MSElementDescrCP	pText);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get a text string from an element descriptor. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="pText">	A text element descriptor. </param>
	/// <returns>	The text as a Unicode string. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	Bentley::WString			GetText(MSElementDescrCP	pText);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get a cell name from a cell element descriptor. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="pCell">	A cell descriptor. </param>
	/// <returns>	The Unicode cell name. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	std::wstring				GetCellName(MSElementDescrCP	pCell);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Get a vector of Unicode (<c>MSWChar</c>) cell names from the named cell library. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="cellNames">	[in,out] List of names of the cells. </param>
	/// <param name="oLibrary">		The cell library. </param>
	/// <returns>	The no. of cell names. </returns>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	UInt32						GetCellNames(WideStringCollection&	cellNames,
		DgnFileP				oLibObj);
	std::string					DgnFileObjectPath(DgnFileP				o);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Obtain the colour defined by a Level. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="levelId">	Identifier for the level. </param>
	/// <param name="modelRef">	A model reference. </param>
	/// <returns>	<c>UInt32</c> colour index. </returns>
	//////////////////////////////////////////////////////////////////////
	UInt32						LevelColour(UInt32					levelId,
		DgnModelRefP			modelRef);

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Convert an RGB colour to MicroStation colour index. </summary>
	///
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	///
	/// <param name="red">		red component. </param>
	/// <param name="green">	green component. </param>
	/// <param name="blue">		blue component. </param>
	/// <param name="modelRef">	A model reference. </param>
	///
	/// <returns>	. </returns>
	UInt32						ColourRGB(int					red,
		int						green,
		int						blue,
		DgnModelRefP			modelRef);
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Gets the active MicroStation colour. </summary>
	///
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	///
	/// <returns>	<c>UInt32</c> colour index. </returns>
	UInt32						ActiveColour();

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Examine the active parameter return status and warn if there was a problem. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="status">		The status value returned by <c>mdlParams_setActive</c>. </param>
	/// <param name="paramName">	Name of the parameter. </param>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	void						ActiveParamStatus(int					status,
		std::wstring const&		paramName);

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Lock MicroStation's Associative Pattern setting. </summary>
	/// <remarks>	V 8i, 09/11/2010. </remarks>
	/// <param name="b">	true to set associative pattern lock. </param>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	void						AssociativePatternLock(bool					b);

	class AreaStroker
	{
	public:
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	AreaStroker Constructor. </summary>
		/// <remarks>	V 8i, 09/11/2010. </remarks>
		/// <param name="pArea">		An element that contains an area. </param>
		/// <param name="tolerance">	tolerance. </param>
		AreaStroker(MSElementDescrCP		pArea,
			double					tolerance);
		~AreaStroker();
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Stroke a closed element descriptor into an array of <c>DPoint3d</c>. </summary>
		/// <remarks>	V 8i, 09/11/2010. </remarks>
		/// <param name="points">	[in,out] The points. </param>
		/// <param name="shape">	A shape element. </param>
		/// <returns>	<c>UInt32</c> the number of points. </returns>
		UInt32						Stroke(DPoint3dCollection&		points,
			MSElementDescrCP		shape);
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Stroke a closed element descriptor into an array of DPoint3d.  If
		///	the area is a Grouped Hole, then create an additional array for each hole. </summary>
		/// <remarks>	V 8i, 09/11/2010. </remarks>
		/// <returns>	Count of shapes in area. </returns>
		UInt32						Stroke();
		const DPoint3dCollection&	Array(UInt32						index = 0)	const;
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Test whether a point lies inside a polygon. </summary>
		///
		/// <remarks>	V 8i, 09/11/2010. </remarks>
		///
		/// <param name="test">		The point to test. </param>
		/// <param name="polygon">	The polygon as a point collection. </param>
		///
		/// <returns>	true if it succeeds, false if it fails. </returns>
		bool						PointInside(DPoint3d const*			test)			const;
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Test whether a point lies inside our boundary polygon and not
		///	inside any interior polygon in a Grouped Hole. </summary>
		/// <remarks>	V 8i, 09/11/2010. </remarks>
		/// <param name="test">	The point to test. </param>
		/// <returns>	true if it succeeds, false if it fails. </returns>
		bool						PointInside(DPoint3d const*			test,
			const DPoint3dCollection&	polygon)		const;
		UInt32						ShapeCount()	const
		{
			return static_cast<UInt32>(polygons_.size());
		}
	private:
		MSElementDescrCP	pArea_;
		double					tolerance_;
		StrokedArrays			polygons_;
	};

	//////////////////////////////////////////////////////////////////////
	///	<summary>
	///	Load an MDL task in constructor and unload in destructor.
	///	</summary>
	class MdlTaskController
	{
		bool			alreadyLoaded_;
		const wchar_t* taskName_;

	public:
		MdlTaskController(const wchar_t*	taskName);
		~MdlTaskController();

	private:
		//	No copying or assignment
		MdlTaskController(MdlTaskController const&);
		MdlTaskController& operator=(MdlTaskController const&);
	};
	//////////////////////////////////////////////////////////////////
	//	Extend Symbology definition from <mstypes.h> to include
	//	transparency and priority
	//	typedef struct symbology
	//	{
	//	Int32           style;
	//	UInt32          weight;
	//	UInt32          color;
	//	} Symbology;
	//
	/// <summary>   SymbologyExt extends Symbology definition from mstypes.h to include
	//	transparency and priority </summary>
	///
	/// <remarks>   Extended symbology adds transparency and priority to
	///             MicroStation's Symbology struct
	/// </remarks>
	///
	/// <param name="colour">       colour </param>
	/// <param name="transparency"> transparency </param>
	/// <param name="weight">       weight </param>
	/// <param name="style">        style </param>
	/// <param name="priority">     priority </param>
	struct SymbologyExt
	{
		/// <summary>   SymbologyExt Constructor. </summary>
		SymbologyExt(const Bentley::RgbColorDef&		lineColour,
			double					lineTransparency = 0.0,
			UInt32					lineThickness = 1,
			Int32					lineStyle = 0,
			Int32					priority = 0);
		SymbologyExt(const Bentley::RgbColorDef&		fillColour = ConvertString2ColorDef(L"FFFFFF"),
			double					fillTransparency = 0.0,
			const Bentley::RgbColorDef&		lineColour = ConvertString2ColorDef(L"000000"),
			double					lineTransparency = 0.0,
			UInt32					lineThickness = 1,
			Int32					lineStyle = 0,
			Int32					priority = 0);

		Bentley::RgbColorDef			fillColour_;
		double				fillTransparency_;
		Bentley::RgbColorDef			lineColour_;
		double				lineTransparency_;
		UInt32				lineThickness_;
		Int32				lineStyle_;
		mutable Int32		priority_;		//	Priority is set at run-time

	};

};	//	namespace MicroStation
	//////////////////////////////////////////////////////////////////////
#endif