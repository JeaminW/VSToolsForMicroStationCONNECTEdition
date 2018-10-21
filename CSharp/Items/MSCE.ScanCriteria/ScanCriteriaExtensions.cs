/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region System Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

#region Bentley Namespaces
using BDPN = Bentley.DgnPlatformNET;
#endregion

namespace $rootnamespace$
{
    /// <summary>
    /// Scan Criteria Extensions to Bentley.DgnPlatformNET.ScanCriteria.
    /// <remarks>Published on Be Communities MicroStation Programming Forum by Robert Menger
    /// (http://www.menger-engineering.com/en/).
    /// <para>Additional code to ensure the Bitmask could accommodate all element types
    /// was suggested by Robert Hook of Bentley Systems.</para></remarks>
    /// </summary>
    public static class $safeitemname$
    {
        /// <summary>
        /// Given an array of MSElementType, create a Bitmask for the scanner.
        /// </summary>
        public static BDPN.ScanCriteria AddElementTypes(this BDPN.ScanCriteria criteria, params BDPN.MSElementType[] types)
{
    BDPN.BitMask bitMask = SetElementTypeBitMask(types);
    criteria.SetElementTypeTest(bitMask);
    return criteria;
}
/// <summary>
/// Scan a DGN model and return a list of elements
/// </summary>
/// <param name="criteria">ScanCriteria object</param>
/// <returns>IEnumerable(Element) list of elements</returns>
public static IEnumerable<BDPN.Elements.Element> Scan(this BDPN.ScanCriteria criteria)
{
    var result = new List<BDPN.Elements.Element>();
    criteria.Scan((e, m) =>
    {
        result.Add(e);
        return BDPN.StatusInt.Success;
    });

    return result;
}
/// <summary>
/// Get the largest value from an array of numbers
/// </summary>
/// <typeparam name="T">Numeric type including enums</typeparam>
/// <param name="numbers"></param>
/// <returns>Largest T in array</returns>
public static T Largest<T>(T[] numbers)
{
    Array.Sort(numbers);
    return numbers.Last();
}
/// <summary>
/// Set bits in a BitMask of MSElementType.
/// <remarks>Mechanics of resizing a Bitmask shown by Robert Hook on Be Communities.</remarks>
/// </summary>
/// <param name="types">Array of MSElementType values</param>
/// <returns>BitMask</returns>
private static BDPN.BitMask SetElementTypeBitMask(BDPN.MSElementType[] types)
{
    BDPN.MSElementType largestType = Largest(types);
    uint bmSize = 1 + (uint)largestType;
    bmSize = (bmSize + 7) / 8;
    bmSize = (bmSize * 16) - 15;
    BDPN.BitMask bitMask = new BDPN.BitMask(false);
    bitMask.EnsureCapacity(bmSize + 1);
    foreach (var type in types)
    {
        bitMask.SetBit((uint)type - 1, true);
    }
    return bitMask;
}
}
}
