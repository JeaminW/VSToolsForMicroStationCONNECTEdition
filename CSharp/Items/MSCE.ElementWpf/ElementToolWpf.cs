/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region Bentley Namespaces
using BDPN = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;
using BM = Bentley.MstnPlatformNET;
using BMW = Bentley.MstnPlatformNET.WPF;
#endregion

namespace $rootnamespace$
{
    class $safeitemname$ : BDPN.DgnElementSetTool
    {
    //Use $safeitemname$UserControl.Property, $safeitemname$UserControl.Method(), or $safeitemname$UserControl.Field
    //to access public properties, methods, or fields in $safeitemname$.xaml.cs.

       #region ToolSettings
        public static $safeitemname$ $safeitemname$Command;
private static BMW.ToolSettingsHost $safeitemname$ToolSettingsHost { get; set; }
private static $safeitemname$UC $safeitemname$UserControl { get; set; }

class MyToolSettings : BMW.ToolSettingsHost
{
    private $safeitemname$ m_command;

    public MyToolSettings($safeitemname$ command)
    {
        m_command = command;
        $safeitemname$UserControl = new $safeitemname$UC();
        this.Content = $safeitemname$UserControl;
        this.Title = "$safeitemname$ Tool Settings";
    }
};
#endregion

public $safeitemname$(int toolID, int toolName) : base()
        {
            $safeitemname$ToolSettingsHost = new MyToolSettings(this);
}

#region DgnElementSetTool Members
protected override void OnRestartTool()
{
    InstallNewInstance();
}

protected override void OnCleanup()
{
    if (null != $safeitemname$Command)
    {
        $safeitemname$ToolSettingsHost.Detach();
        $safeitemname$ToolSettingsHost.Dispose();
        $safeitemname$ToolSettingsHost = null;
        $safeitemname$Command = null;
    }
    base.OnCleanup();
}

public override BDPN.StatusInt OnElementModify(BDPN.Elements.Element element)
{

    return BDPN.StatusInt.Success;
}

protected override bool OnResetButton(BDPN.DgnButtonEvent ev)
{
    ExitTool();
    return true;
}

protected override void ExitTool()
{

    base.ExitTool();
}

protected override void OnDynamicFrame(BDPN.DgnButtonEvent ev)
{

}

protected override bool OnInstall()
{

    return true;
}

protected override void OnPostInstall()
{
    base.OnPostInstall();
}

public void InstallNewInstance(string unparsed = "")
{
    if (null == $safeitemname$Command)
    {
        $safeitemname$Command = new $safeitemname$(0, 0);
        $safeitemname$Command.InstallTool();
        $safeitemname$ToolSettingsHost.Attach(Program.Addin);
    }
    else
        $safeitemname$ToolSettingsHost.Focus();
}
        #endregion
    }
}