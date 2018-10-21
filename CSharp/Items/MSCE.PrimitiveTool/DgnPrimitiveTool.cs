/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region Bentley Namespaces
using BDPN = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;
using BM = Bentley.MstnPlatformNET;
#endregion

namespace $rootnamespace$
{
    class $safeitemname$ : BDPN.DgnPrimitiveTool
    {
    public $safeitemname$(int toolName, int toolPrompt) : base(toolName, toolPrompt) { }

#region DgnPrimitiveTool Members
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
            $safeitemname$ tool = new $safeitemname$(0, 0);
            tool.InstallTool();
        }
        #endregion
    }
}
