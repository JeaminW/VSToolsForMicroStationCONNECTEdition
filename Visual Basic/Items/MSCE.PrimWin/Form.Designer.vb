'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class $safeitemrootname$
        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        '$safeitemrootname$
        '
        components = New System.ComponentModel.Container
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "$safeitemrootname$"
        Me.Text = "Tool Settings"
    End Sub
End Class