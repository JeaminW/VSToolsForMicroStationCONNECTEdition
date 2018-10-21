'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

#Region "Bentley Namespaces"
Imports BDPN = Bentley.DgnPlatformNET
Imports BG = Bentley.GeometryNET
Imports BM = Bentley.MstnPlatformNET
#End Region

Class $safeitemname$
        Inherits BDPN.DgnPrimitiveTool

    Public Sub New(toolName As Integer, toolPrompt As Integer)
        MyBase.New(toolName, toolPrompt)

    End Sub

#Region "DgnPrimitiveTool Members"
    Protected Overrides Function OnDataButton(ev As BDPN.DgnButtonEvent) As Boolean

        Return True
    End Function

    Protected Overrides Sub OnRestartTool()
        InstallNewInstance()
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
        Dim tool As $safeitemname$ = New $safeitemname$(0, 0)
        tool.InstallTool()
    End Sub
#End Region
End Class
