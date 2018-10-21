/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region Bentley Namespaces
using BDPN = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;
using BM = Bentley.MstnPlatformNET;
using BMW = Bentley.MstnPlatformNET.WinForms;
#endregion

namespace $rootnamespace$
{
    class $safeitemname$ : BDPN.DgnElementSetTool
    {
    //Use $safeitemname$MSForm.Property, $safeitemname$MSForm.Method(), or $safeitemname$MSForm.Field
    //to access public properties, methods, or fields in $safeitemname$Form.cs.

     #region ToolSettings
        private static $safeitemname$ $safeitemname$Command { get; set; }
private static $safeitemname$Form $safeitemname$MSForm { get; set; }
private static BMW.Adapter $safeitemname$Adapter { get; set; }

class MyToolSettings : BMW.Adapter
{
    private $safeitemname$ m_command;

    public MyToolSettings($safeitemname$ command)
    {
        m_command = command;
        $safeitemname$MSForm = new $safeitemname$Form();
    }
};
#endregion

public $safeitemname$(int toolID, int toolName) : base()
        {
            $safeitemname$Adapter = new MyToolSettings(this);
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
        $safeitemname$MSForm.DetachFromMicroStation();
        $safeitemname$MSForm.Dispose();
        $safeitemname$MSForm = null;
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
        $safeitemname$MSForm.AttachToToolSettings(Program.Addin);
        $safeitemname$MSForm.Text = "$safeitemname$ Tool Settings";
    }
    else
        $safeitemname$MSForm.Focus();
}
        #endregion
    }
}