/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

namespace $rootnamespace$
{
    partial class $safeitemrootname$
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

/// <summary>
/// Clean up any resources being used.
/// </summary>
/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
protected override void Dispose(bool disposing)
{
    if (disposing && (components != null))
    {
        components.Dispose();
    }
    base.Dispose(disposing);
}

#region Windows Form Designer generated code

/// <summary>
/// Required method for Designer support - do not modify
/// the contents of this method with the code editor.
/// </summary>
private void InitializeComponent()
{
    this.SuspendLayout();
    //
    //$safeitemrootname$
    //
    this.components = new System.ComponentModel.Container();
    this.ClientSize = new System.Drawing.Size(284, 261);
    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    this.Name = "$safeitemrootname$";
    this.Text = "Form";
    this.SizeChanged += new System.EventHandler($safeitemrootname$_SizeChanged);
    this.ResumeLayout(false);
}
#endregion

#region MicroStation
public static $safeitemrootname$ $safeitemrootname$Form { get; set; }
private Bentley.Windowing.WindowContent m_windowContent { get; set; }

/// <summary>
/// Show the form if it is not already displayed
/// </summary>
internal void ShowForm(string unparsed = "")
{
    if (null != $safeitemrootname$Form)
    {
        $safeitemrootname$Form.Focus();
        return;
    }

    $safeitemrootname$Form = new $safeitemrootname$();
    $safeitemrootname$Form.AttachAsTopLevelForm(Program.Addin, true);

    $safeitemrootname$Form.AutoOpen = true;
    $safeitemrootname$Form.AutoOpenKeyin = "mdl load $safeitemrootname$";

    $safeitemrootname$Form.NETDockable = true;
    Bentley.Windowing.WindowManager windowManager =
                Bentley.Windowing.WindowManager.GetForMicroStation();
    $safeitemrootname$Form.m_windowContent =
        windowManager.DockPanel($safeitemrootname$Form, $safeitemrootname$Form.Name, $safeitemrootname$Form.Name,
        Bentley.Windowing.DockLocation.Floating);

    $safeitemrootname$Form.m_windowContent.CanDockHorizontally = false; // limit to left and right docking
    $safeitemrootname$Form.m_windowContent.ContentCloseQuery += OnClose;
}

/// <summary>
/// Override Form OnClosed method.
/// </summary>
/// <param name="e"></param>
protected override void OnClosed(System.EventArgs e)
{
    $safeitemrootname$Form.m_windowContent.Close();
    base.OnClosed(e);
}

/// <summary>
/// Close and dispose the form.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private static void OnClose(object sender, Bentley.Windowing.ContentCloseEventArgs e)
{
    e.CloseAction = Bentley.Windowing.ContentCloseAction.Dispose;
    $safeitemrootname$Form.m_windowContent.Hide();
    if (null != $safeitemrootname$Form)
    {
        $safeitemrootname$Form.DetachFromMicroStation();
        $safeitemrootname$Form.Dispose();
        $safeitemrootname$Form = null;
    }
}

/// <summary>
/// Adjust to controls when the form changes size
/// </summary>
private void $safeitemrootname$_SizeChanged(object sender, System.EventArgs e)
{
    if (this.DesignMode)
    {
        System.Diagnostics.Debug.Assert(!this.DesignMode, "Do not use SetFormSizes in design mode.");
        return;
    }
}
#endregion
}
}