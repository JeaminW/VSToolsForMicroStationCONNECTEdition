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
    class $safeitemname$ : BDPN.DgnPrimitiveTool
    {
    //Use $safeitemname$UserControl.Property, $safeitemname$UserControl.Method(), or $safeitemname$UserControl.Field
    //to access public properties, methods, or fields in $safeitemname$.xaml.cs.

     #region ToolSettings
        private static $safeitemname$ $safeitemname$Command { get; set; }
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

#region DgnPrimitiveTool members
public $safeitemname$(int toolName, int toolPrompt) : base(toolName, toolPrompt)
    {
        $safeitemname$ToolSettingsHost = new MyToolSettings(this);
    }

        protected override bool OnDataButton(BDPN.DgnButtonEvent ev)
{

    return true;
}

protected override void OnRestartTool()
{
    InstallNewInstance();
}

protected override bool OnResetButton(BDPN.DgnButtonEvent ev)
{
    ExitTool();
    return true;
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