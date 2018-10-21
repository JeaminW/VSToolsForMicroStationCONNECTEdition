/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region System Namespaces
using System;
using System.Windows.Controls;
#endregion

#region Bentley Namespaces
using BDPN = Bentley.DgnPlatformNET;
using BMW = Bentley.MstnPlatformNET.WPF;
using BW = Bentley.Windowing;
#endregion

namespace $rootnamespace$
{
public partial class $safeitemrootname$ : UserControl
{
#region Bentley DockableWindow
private static BMW.DockableWindow $safeitemrootname$DockableWindow { get; set; }

internal void ShowWindow(string unparsed = "")
{
    if (null != $safeitemrootname$DockableWindow)
    {
        $safeitemrootname$DockableWindow.Focus();
        return;
    }

    $safeitemrootname$DockableWindow = new BMW.DockableWindow();
    $safeitemrootname$DockableWindow.Content = new $safeitemrootname$();
    $safeitemrootname$DockableWindow.Attach(Program.Addin, "control", new System.Drawing.Size(Convert.ToInt32($safeitemrootname$DockableWindow.MinWidth),
                Convert.ToInt32($safeitemrootname$DockableWindow.MinHeight)));
    $safeitemrootname$DockableWindow.WindowContent.CanDockVertically = false;
    $safeitemrootname$DockableWindow.WindowContent.ContentCloseQuery += new BW.ContentCloseEventHandler(OnClose);
}

/// <summary>
/// Close and dispose the usercontrol.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private static void OnClose(object sender, BW.ContentCloseEventArgs e)
{
    e.CloseAction = BW.ContentCloseAction.Dispose;
    if (null != $safeitemrootname$DockableWindow)
    {
        $safeitemrootname$DockableWindow.Detach();
        $safeitemrootname$DockableWindow.Dispose();
        $safeitemrootname$DockableWindow = null;
    }
}
#endregion
}
}