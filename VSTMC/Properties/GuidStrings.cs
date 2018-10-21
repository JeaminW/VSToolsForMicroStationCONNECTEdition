/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;

namespace VSTMC
{
    /// <summary>
    /// This class contains a list of GUIDs specific to this sample.
    /// </summary>
    public static class PackageGuids
    {
        /// <summary>
        /// VSTMCPackage GUID string.
        /// </summary>
        public const string guidPackageString = "373071be-5abe-4690-ad2b-99d591d5a9e5";
        /// <summary>
        /// Guid for the OptionsPageGeneral class.
        /// </summary>
        public const string guidPackageCmdSetString = "438120cb-7081-4b71-bac5-ccd58b950c38";

        /// <summary>
        /// Guid for the OptionsPageCustom class.
        /// </summary>
        public const string guidPageString = "D703DD42-82CE-4E03-B3F5-C16894EE3707";

        public static Guid guidPackage = new Guid(guidPackageString);
        public static Guid guidPackageCmdSet = new Guid(guidPackageCmdSetString);
        public static Guid guidPage = new Guid(guidPageString);
    }
}