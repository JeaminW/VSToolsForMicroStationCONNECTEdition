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

#Region "Windows Form Designer generated code"

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
        Me.Text = "$safeitemrootname$"
        Me.ResumeLayout(False)
    End Sub
#End Region

#Region "MicroStation"
    Private Shared Property $safeitemrootname$Form As $safeitemrootname$
    ''' <summary>
    ''' Show the form if it is not already displayed
    ''' </summary>
    Friend Sub ShowForm(ByVal Optional unparsed As String = Nothing)
        If $safeitemrootname$Form IsNot Nothing Then
            $safeitemrootname$Form.Focus()
            Return
        End If

        $safeitemrootname$Form = New $safeitemrootname$()
        $safeitemrootname$Form.AttachAsTopLevelForm(Program.Addin, True)

        $safeitemrootname$Form.AutoOpen = True
        $safeitemrootname$Form.AutoOpenKeyin = "mdl load $safeitemrootname$"

        $safeitemrootname$Form.NETDockable = True
        Dim windowManager As Bentley.Windowing.WindowManager = Bentley.Windowing.WindowManager.GetForMicroStation()
        $safeitemrootname$Form.m_windowContent = windowManager.DockPanel($safeitemrootname$Form, $safeitemrootname$Form.Name, $safeitemrootname$Form.Name, Bentley.Windowing.DockLocation.Floating)

        $safeitemrootname$Form.m_windowContent.CanDockHorizontally = False
    End Sub

    ''' <summary>
    ''' Override Form OnClosed method.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnClosed(e As EventArgs)
        $safeitemrootname$Form.m_windowContent.Close()
        MyBase.OnClosed(e)
    End Sub

    ''' <summary>
    ''' Close and dispose the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OnClose(sender As Object, e As Bentley.Windowing.ContentCloseEventArgs) Handles m_windowContent.ContentCloseQuery
        e.CloseAction = Bentley.Windowing.ContentCloseAction.Dispose
        $safeitemrootname$Form.m_windowContent.Hide()
        If $safeitemrootname$Form IsNot Nothing Then
            DetachFromMicroStation()
            $safeitemrootname$Form.Dispose(True)
            $safeitemrootname$Form = Nothing
        End If
    End Sub
#End Region
End Class