/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region Bentley Namespaces
using BCOM = Bentley.Interop.MicroStationDGN;
using BMI = Bentley.MstnPlatformNET.InteropServices;
using BMW = Bentley.MstnPlatformNET.WPF;
#endregion

namespace $rootnamespace$
{
    class $safeitemname$ : BCOM.IPrimitiveCommandEvents
    {
    //Use $safeitemname$UserControl.Property, $safeitemname$UserControl.Method(), or $safeitemname$UserControl.Field
    //to access public properties, methods, or fields in $safeitemname$.xaml.cs.

    #region ToolSettings
    private static $safeitemname$ $safeitemname$Command { get; set; }
private static BMW.ToolSettingsHost $safeitemname$ToolSettingsHost { get; set; }
private static $safeitemname$UC $safeitemname$UserControl { get; set; }

class MyToolSettings : BMW.ToolSettingsHost
{
    private $safeitemname$ m_command ;

    public MyToolSettings($safeitemname$ command)
    {
        m_command = command;

        $safeitemname$UserControl = new $safeitemname$UC();
        this.Content = $safeitemname$UserControl;
        this.Title = "$safeitemname$ Tool Settings";
    }
};
#endregion

public $safeitemname$()
{
    $safeitemname$ToolSettingsHost = new MyToolSettings(this);
}

#region IPrimitiveCommandEvents Members

public void Start()
{
    $safeitemname$ToolSettingsHost.Attach(Program.Addin);
}

public void Cleanup()
{
    if (null != $safeitemname$Command)
    {
        $safeitemname$ToolSettingsHost.Detach();
        $safeitemname$ToolSettingsHost.Dispose();
        $safeitemname$ToolSettingsHost = null;
        $safeitemname$Command = null;
    }
}

public void DataPoint(ref BCOM.Point3d Point, BCOM.View View)
{
}

public void Dynamics(ref BCOM.Point3d Point, BCOM.View View, BCOM.MsdDrawingMode DrawMode)
{
}

public void Keyin(string Keyin)
{
}

public void Reset()
{

    BMI.Utilities.ComApp.CommandState.StartDefaultCommand();
}

#endregion

/// <summary>
/// Open $safeitemname$ WPF user control.
/// </summary>
/// <param name="unparsed"></param>
public void Run(string unparsed = "")
{
    if (null == $safeitemname$Command)
    {
        $safeitemname$Command = new $safeitemname$();
        BMI.Utilities.ComApp.CommandState.StartPrimitive($safeitemname$Command, false);
    }
    else
        $safeitemname$ToolSettingsHost.Focus();
}
}
    }