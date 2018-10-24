using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace VSTMC
{
    public static class BentleyDataCollector
    {
        #region  Method

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string MSCEPath()
        {
            RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = localMachineRegistry.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products");
            string displayName = null;
            string installLocation = null;
            //string version = null;
            SortedDictionary<string, string> bentleyProducts = new SortedDictionary<string, string>();
            if (null == key)
            {
                bentleyProducts.Add("Not Installed", "Not Installed");
            }
            else
            {
                foreach (var v in key.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey propertyKey = localMachineRegistry.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products\"
                        + v + "\\InstallProperties");
                        displayName = propertyKey.GetValue("DisplayName", "") as string;
                        installLocation = propertyKey.GetValue("InstallLocation") as string;
                        string version = propertyKey.GetValue("DisplayVersion", "") as string;
                        if (!displayName.ToLower().Contains("sdk") && !displayName.ToLower().Contains("documentation"))
                        {
                            if (displayName.ToLower().Contains("microstation connect edition") ||
                                displayName.ToLower().Contains("openroads designer connect edition") ||
                                displayName.ToLower().Contains("microstation powerdraft connect edition") ||
                                displayName.ToLower().Contains("aecosim building designer connect edition") ||
                                displayName.ToLower().Contains("bentley descartes connect edition") ||
                                displayName.ToLower().Contains("bentley map connect edition") ||
                                displayName.ToLower().Contains("openbridge modeler connect edition") ||
                                displayName.ToLower().Contains("openplant modeler connect edition") ||
                                displayName.ToLower().Contains("openrail designer connect edition") ||
                                displayName.ToLower().Contains("bentley substation connect edition"))
                            {
                                try
                                {
                                    if (displayName.ToLower().Contains("microstation connect"))
                                        installLocation += "MicroStation\\";
                                    else if (displayName.ToLower().Contains("openroads"))
                                        installLocation += "OpenRoadsDesigner\\";
                                    else if (displayName.ToLower().Contains("powerdraft connect"))
                                        installLocation += "PowerDraft\\";
                                    else if (displayName.ToLower().Contains("aecosim building designer connect"))
                                        installLocation += "AECOsimBuildingDesigner\\";
                                    else if (displayName.ToLower().Contains("bentley descartes connect edition"))
                                        installLocation += "DescartesStandAlone\\";
                                    else if (displayName.ToLower().Contains("bentley map connect edition"))
                                        installLocation += "BentleyMap\\";
                                    else if (displayName.ToLower().Contains("openbridge modeler connect edition"))
                                        installLocation += "OpenBridgeModeler\\";
                                    else if (displayName.ToLower().Contains("openplant modeler connect edition"))
                                        installLocation += "OpenPlantModeler\\";
                                    else if (displayName.ToLower().Contains("openrail designer connect edition"))
                                        installLocation += "OpenRailDesigner\\";
                                    else if (displayName.ToLower().Contains("bentley substation connect edition"))
                                        installLocation += "Substation\\";
                                    bentleyProducts.Add(displayName + " - v" + version, installLocation);
                                }
                                catch (Exception)
                                {
                                    //do nothing
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //do nothing
                    }
                }
            }
            if (0 == bentleyProducts.Count)
                bentleyProducts.Add("Not Installed", "Not Installed");

            BentleyProducts = bentleyProducts;
            //var bentleyProductsSorted = bentleyProducts.Cast<DictionaryEntry>()
            //           .OrderBy(r => r.Value)
            //           .ToDictionary(c => c.Key, d => d.Value);
            //BentleyProducts = bentleyProductsSorted;
            IDictionaryEnumerator myEnumerator = BentleyProducts.GetEnumerator();
            myEnumerator.Reset();
            myEnumerator.MoveNext();
            return (string)myEnumerator.Value;
        }

        /// <summary>
        /// Get the Bentley product SDK Path.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SDKPath(string value)
        {
            RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = localMachineRegistry.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products");
            string displayName = null;
            string installLocation = null;
            Version version = new Version("0.0.0.0");
            if (null == key)
            {
                return "Not Installed";
            }
            else
            {
                foreach (var v in key.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey propertyKey = localMachineRegistry.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products\"
                        + v + "\\InstallProperties");
                        displayName = propertyKey.GetValue("DisplayName", "") as string;
                        installLocation = propertyKey.GetValue("InstallLocation") as string;
                        if (displayName.ToLower().Contains("sdk"))
                        {
                            if (displayName.ToLower().Contains(value))
                            {
                                return installLocation;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //do nothing
                    }
                }
            }
            return "Not Installed";
        }

        /// <summary>
        ///  Get the Bentley product SDK path.
        /// </summary>
        /// <param name="mscepath"></param>
        /// <returns></returns>
        public static string GetSDKPath(string mscepath)
        {
            if (mscepath.Contains("AECOsim"))
                return SDKPath("aecosim building designer connect edition");
            else if (mscepath.Contains("OpenRoads") || mscepath.Contains("OpenRail"))
                return SDKPath("openroads designer connect edition");
            else if (mscepath.Contains("Map"))
                return SDKPath("bentley map connect edition");
            else
                return SDKPath("microstation connect edition");
        }

        /// <summary>
        /// Get the Bentley product executable application.
        /// </summary>
        /// <param name="mscepath"></param>
        /// <returns></returns>
        public static string GetBentleyApp(string mscepath)
        {
            if (mscepath.Contains("MicroStation"))
                return "MicroStation.exe";
            else if (mscepath.Contains("OpenRoads"))
                return "OpenRoadsDesigner.exe";
            else if (mscepath.Contains("PowerDraft"))
                return "Draft.exe";
            else if (mscepath.Contains("AECOsim"))
                return "AECOsimBuildingDesigner.exe";
            else if (mscepath.Contains("DescartesStandAlone"))
                return "DescartesStandAlone.exe";
            else if (mscepath.Contains("Map"))
                return "BentleyMap.exe";
            else if (mscepath.Contains("OpenBridge"))
                return "OpenBridgeModeler.exe";
            else if (mscepath.Contains("OpenPlant"))
                return "OpenPlantModeler.exe";
            else if (mscepath.Contains("OpenRail"))
                return "OpenRailDesigner.exe";
            else if (mscepath.Contains("Substation"))
                return "Substation.exe";
            else
                return "MicroStation.exe";

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="microStationCONNECTPath"></param>
        /// <returns></returns>
        public static string GetMdlappsPath(string microStationCONNECTPath)
        {
            if (microStationCONNECTPath == "Not Installed")
                return "Not Installed";

            return microStationCONNECTPath + @"mdlapps\";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="BentleyApp"></param>
        /// <returns></returns>
        public static string BentleyBuildBatchFilePath(string BentleyApp)
        {
            if (BentleyApp.Contains("AECOsimBuildingDesigner"))
                return Environment.GetEnvironmentVariable("ProgramData") + @"\innovoCAD\Bentley\VisualStudioTools\AECOsimBuild.bat";
            else if (BentleyApp.Contains("OpenRoadsDesigner") || BentleyApp.Contains("OpenRailDesigner"))
                return Environment.GetEnvironmentVariable("ProgramData") + @"\innovoCAD\Bentley\VisualStudioTools\OrdBuild.bat";
            else if (BentleyApp.Contains("BentleyMap"))
                return Environment.GetEnvironmentVariable("ProgramData") + @"\innovoCAD\Bentley\VisualStudioTools\MapBuild.bat";
            else
                return Environment.GetEnvironmentVariable("ProgramData") + @"\innovoCAD\Bentley\VisualStudioTools\MSCEBuild.bat";
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public static SortedDictionary<string, string> BentleyProducts { get; set; }

        #endregion
    }
}