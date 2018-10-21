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
    class $safeitemname$ : BDPN.DgnElementSetTool
    {
    public $safeitemname$(int toolID, int toolName) : base() { }

#region DgnElementSetTool Members
protected override void OnRestartTool()
        {
            InstallNewInstance();
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

protected override bool OnInstall()
{

    return true;
}

protected override void OnPostInstall()
{

    base.OnPostInstall();
}

protected override bool OnDataButton(BDPN.DgnButtonEvent ev)
{

    return true;
}

protected override void OnDynamicFrame(BDPN.DgnButtonEvent ev)
{

}


public void InstallNewInstance(string unparsed = "")
        {
            $safeitemname$ tool = new $safeitemname$(0, 0);
            tool.InstallTool();
        }
        #endregion
    }
}
