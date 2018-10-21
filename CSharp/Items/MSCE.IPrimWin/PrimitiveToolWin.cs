/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region Bentley Namespaces
using BCOM = Bentley.Interop.MicroStationDGN;
using BMI = Bentley.MstnPlatformNET.InteropServices;
using BMW = Bentley.MstnPlatformNET.WinForms;
#endregion

namespace $rootnamespace$
{
    class $safeitemname$ : BCOM.IPrimitiveCommandEvents
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

public $safeitemname$()
{
    $safeitemname$Adapter = new MyToolSettings(this);
}

#region IPrimitiveCommandEvents Members

public void Start()
{
    $safeitemname$MSForm.AttachToToolSettings(Program.Addin);
    $safeitemname$MSForm.Text = "$safeitemname$ Tool Settings";
}

public void Cleanup()
{
    if (null != $safeitemname$Command)
    {
        $safeitemname$MSForm.DetachFromMicroStation();
        $safeitemname$MSForm.Dispose();
        $safeitemname$MSForm = null;
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
/// Open $safeitemname$ Form.
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
        $safeitemname$MSForm.Focus();
}
}
    }