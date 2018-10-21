'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

#Region "Bentley Namespaces"
Imports BDPN = Bentley.DgnPlatformNET
Imports BG = Bentley.GeometryNET
Imports BM = Bentley.MstnPlatformNET
Imports BMW = Bentley.MstnPlatformNET.WPF
#End Region

Class $safeitemname$
        Inherits BDPN.DgnPrimitiveTool

    'Use $safeitemname$UserControl.Property, $safeitemname$UserControl.Method(), Or $safeitemname$UserControl.Field
    'to access public properties, methods, or fields In $safeitemname$.xaml.vb.

#Region "ToolSettings"
    Private Shared Property $safeitemname$Command As $safeitemname$
    Private Shared Property $safeitemname$ToolSettingsHost As BMW.ToolSettingsHost
    Private Shared Property $safeitemname$UserControl As $safeitemname$UC

    Class MyToolSettings
        Inherits BMW.ToolSettingsHost

        Private Property m_command As $safeitemname$

        Public Sub New(ByVal command As $safeitemname$)
            m_command = command
            $safeitemname$UserControl = New $safeitemname$UC()
            Me.Content = $safeitemname$UserControl
            Me.Title = "$safeitemname$ Tool Settings"
        End Sub
    End Class
#End Region

    Public Sub New(toolName As Integer, toolPrompt As Integer)
        MyBase.New(toolName, toolPrompt)
        $safeitemname$ToolSettingsHost = New MyToolSettings(Me)
    End Sub

#Region "DgnPrimitiveTool Members"
    Protected Overrides Function OnDataButton(ev As BDPN.DgnButtonEvent) As Boolean

        Return True
    End Function

    Protected Overrides Sub OnRestartTool()
        InstallNewInstance()
    End Sub

    Protected Overrides Sub OnCleanup()
        If $safeitemname$Command IsNot Nothing Then
            $safeitemname$ToolSettingsHost.Detach()
            $safeitemname$ToolSettingsHost.Dispose()
            $safeitemname$ToolSettingsHost = Nothing
            $safeitemname$Command = Nothing
        End If
        MyBase.OnCleanup()
    End Sub

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
            $safeitemname$ToolSettingsHost.Attach(Program.Addin)
        Else
            $safeitemname$ToolSettingsHost.Focus()
        End If
    End Sub
#End Region
End Class