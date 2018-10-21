'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

Option Infer On
Option Strict Off

#Region "Bentley Namespaces"
Imports BDPN = Bentley.DgnPlatformNET
#End Region

Module ScanCriteriaExtensions
    ''' <summary>
    ''' Scan Criteria Extensions to Bentley.DgnPlatformNET.ScanCriteria.
    ''' <remarks>Published on Be Communities MicroStation Programming Forum by Robert Menger
    ''' (http://www.menger-engineering.com/en/).
    ''' <para>Additional code to ensure the Bitmask could accommodate all element types
    ''' was suggested by Robert Hook of Bentley Systems.</para></remarks>
    ''' </summary>

    ''' <summary>
    ''' Given an array of MSElementType, create a Bitmask for the scanner.
    ''' </summary>
    <Runtime.CompilerServices.Extension>
    Public Function AddElementTypes(criteria As BDPN.ScanCriteria, ParamArray types As BDPN.MSElementType()) As BDPN.ScanCriteria
        Dim bitMask As BDPN.BitMask = SetElementTypeBitMask(types)
        criteria.SetElementTypeTest(bitMask)
        Return criteria
    End Function
    ''' <summary>
    ''' Scan a DGN model and return a list of elements
    ''' </summary>
    ''' <param name="criteria">ScanCriteria object</param>
    ''' <returns>IEnumerable(Element) list of elements</returns>
    <System.Runtime.CompilerServices.Extension>
    Public Function Scan(criteria As BDPN.ScanCriteria) As IEnumerable(Of BDPN.Elements.Element)
        Dim result = New List(Of BDPN.Elements.Element)()
        criteria.Scan(Function(e, m)
                          result.Add(e)
                          Return BDPN.StatusInt.Success

                      End Function)

        Return result
    End Function
    ''' <summary>
    ''' Get the largest value from an array of numbers
    ''' </summary>
    ''' <typeparam name="T">Numeric type including enums</typeparam>
    ''' <param name="numbers"></param>
    ''' <returns>Largest T in array</returns>
    Public Function Largest(Of T)(numbers As T()) As T
        Array.Sort(numbers)
        Return numbers.Last()
    End Function
    ''' <summary>
    ''' Set bits in a BitMask of MSElementType.
    ''' <remarks>Mechanics of resizing a Bitmask shown by Robert Hook on Be Communities.</remarks>
    ''' </summary>
    ''' <param name="types">Array of MSElementType values</param>
    ''' <returns>BitMask</returns>
    Private Function SetElementTypeBitMask(types As BDPN.MSElementType()) As BDPN.BitMask
        Dim largestType As BDPN.MSElementType = Largest(types)
        Dim bmSize As UInteger = 1 + CUInt(largestType)
        bmSize = (bmSize + 7) / 8
        bmSize = (bmSize * 16) - 15
        Dim bitMask As New BDPN.BitMask(False)
        bitMask.EnsureCapacity(bmSize + 1)
        For Each type In types
            bitMask.SetBit(CUInt(type) - 1, True)
        Next
        Return bitMask
    End Function
End Module