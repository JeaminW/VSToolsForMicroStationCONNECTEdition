'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

#Region "Bentley Namespaces"
Imports BDPN = Bentley.DgnPlatformNET
Imports BG = Bentley.GeometryNET
Imports BM = Bentley.MstnPlatformNET
Imports BMW = Bentley.MstnPlatformNET.WinForms
#End Region

Class $safeitemname$
		Inherits BDPN.DgnElementSetTool

    'Use $safeitemname$MSForm.Property, $safeitemname$MSForm.Method(), or $safeitemname$MSForm.Field
    'to access public properties, methods, Or fields in $safeitemname$Form.cs.

#Region "ToolSettings"
    Private Shared Property $safeitemname$Command As $safeitemname$
    Private Shared Property $safeitemname$MSForm As $safeitemname$Form
    Private Shared Property $safeitemname$Adapter As BMW.Adapter

    Class MyToolSettings
        Inherits BMW.Adapter
        Private Property m_command As $safeitemname$

        Public Sub New(command As $safeitemname$)
            m_command = command
            $safeitemname$MSForm = New $safeitemname$Form()
        End Sub
    End Class
#End Region

    Public Sub New(ToolId As Integer, toolName As Integer)
        MyBase.New()
        $safeitemname$Adapter = New MyToolSettings(Me)
    End Sub

#Region "DgnElementSetTool Members"
    Protected Overrides Sub OnRestartTool()
        InstallNewInstance()
    End Sub

    Protected Overrides Sub OnCleanup()
        If $safeitemname$Command IsNot Nothing Then
            $safeitemname$MSForm.DetachFromMicroStation()
            $safeitemname$MSForm.Dispose()
            $safeitemname$MSForm = Nothing
            $safeitemname$Command = Nothing
        End If
        MyBase.OnCleanup()
    End Sub

    Public Overrides Function OnElementModify(element As BDPN.Elements.Element) As BDPN.StatusInt

        Return BDPN.StatusInt.Success
    End Function

    Protected Overrides Function OnResetButton(ev As BDPN.DgnButtonEvent) As Boolean
        ExitTool()
        Return True
    End Function

    Protected Overrides Sub ExitTool()

        MyBase.ExitTool()
    End Sub

    Protected Overrides Sub OnDynamicFrame(ByVal ev As BDPN.DgnButtonEvent)
    End Sub

    Protected Overrides Function OnInstall() As Boolean

        Return True
    End Function

    Protected Overrides Sub OnPostInstall()
        MyBase.OnPostInstall()
    End Sub

    Public Sub InstallNewInstance(ByVal Optional unparsed As String = Nothing)
        If $safeitemname$Command Is Nothing Then
            $safeitemname$Command = New $safeitemname$(0, 0)
            $safeitemname$Command.InstallTool()
            $safeitemname$MSForm.Text = "$safeitemname$ Tool Settings"
            $safeitemname$MSForm.AttachToToolSettings(Program.Addin)
        Else
            $safeitemname$MSForm.Focus()
        End If
    End Sub
#End Region
End Class