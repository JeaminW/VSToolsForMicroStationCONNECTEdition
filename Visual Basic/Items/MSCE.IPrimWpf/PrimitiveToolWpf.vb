'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

#Region "Bentley Namespaces"
Imports BCOM = Bentley.Interop.MicroStationDGN
Imports BMI = Bentley.MstnPlatformNET.InteropServices
Imports BMW = Bentley.MstnPlatformNET.WPF
#End Region

Class $safeitemname$
    Implements BCOM.IPrimitiveCommandEvents

    'Use $safeitemname$UserControl.Property, $safeitemname$UserControl.Method(), Or $safeitemname$UserControl.Field
    'to access public properties, methods, or fields in $safeitemname$.xaml.vb.

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

#Region "IPrimitiveCommandEvents Members"

    Public Sub New()
        $safeitemname$ToolSettingsHost = New MyToolSettings(Me)
    End Sub

    Public Sub Start() Implements BCOM.IPrimitiveCommandEvents.Start
        $safeitemname$ToolSettingsHost.Attach(Program.Addin)
    End Sub

    Public Sub Cleanup() Implements BCOM.IPrimitiveCommandEvents.Cleanup
        If $safeitemname$Command IsNot Nothing Then
            $safeitemname$ToolSettingsHost.Detach()
            $safeitemname$ToolSettingsHost.Dispose()
            $safeitemname$ToolSettingsHost = Nothing
            $safeitemname$Command = Nothing
        End If
    End Sub

    Public Sub DataPoint(ByRef Point As BCOM.Point3d, ByVal View As BCOM.View) Implements BCOM.IPrimitiveCommandEvents.DataPoint
    End Sub

    Public Sub Dynamics(ByRef Point As BCOM.Point3d, ByVal View As BCOM.View, ByVal DrawMode As BCOM.MsdDrawingMode) Implements BCOM.IPrimitiveCommandEvents.Dynamics
    End Sub

    Public Sub Keyin(ByVal Keyin As String) Implements BCOM.IPrimitiveCommandEvents.Keyin
    End Sub

    Public Sub Reset() Implements BCOM.IPrimitiveCommandEvents.Reset

        BMI.Utilities.ComApp.CommandState.StartDefaultCommand()
    End Sub
#End Region

    Public Sub Run(ByVal Optional unparsed As String = "")
        If $safeitemname$Command Is Nothing Then
            $safeitemname$Command = New $safeitemname$()
            BMI.Utilities.ComApp.CommandState.StartPrimitive($safeitemname$Command, False)
        Else
            $safeitemname$ToolSettingsHost.Focus()
        End If
    End Sub
End Class