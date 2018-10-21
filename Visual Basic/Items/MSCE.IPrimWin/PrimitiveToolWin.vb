'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

#Region "Bentley Namespaces"
Imports BCOM = Bentley.Interop.MicroStationDGN
Imports BMI = Bentley.MstnPlatformNET.InteropServices
Imports BMW = Bentley.MstnPlatformNET.WinForms
#End Region

Class $safeitemname$
        Implements BCOM.IPrimitiveCommandEvents

    'Use $safeitemname$MSForm.Property, $safeitemname$MSForm.Method(), or $safeitemname$MSForm.Field
    'to access public properties, methods, or fields in $safeitemname$Form.cs.

#Region "ToolSettings"
    Private Shared Property $safeitemname$Command As $safeitemname$
    Private Shared Property $safeitemname$MSForm As $safeitemname$Form
    Private Shared Property $safeitemname$Adapter As BMW.Adapter

    Class MyToolSettings
        Inherits BMW.Adapter

        Private Property m_command As $safeitemname$

        Public Sub New(ByVal command As $safeitemname$)
            m_command = command
            $safeitemname$MSForm = New $safeitemname$Form()
        End Sub
    End Class
#End Region

    Public Sub New()
        $safeitemname$Adapter = New MyToolSettings(Me)
    End Sub

#Region "PrimitiveTool Members"
    Public Sub Keyin(Keyin As String) Implements BCOM.IPrimitiveCommandEvents.Keyin

    End Sub

    Public Sub DataPoint(ByRef Point As BCOM.Point3d, View As BCOM.View) Implements BCOM.IPrimitiveCommandEvents.DataPoint

    End Sub

    Public Sub Reset() Implements BCOM.IPrimitiveCommandEvents.Reset

        BMI.Utilities.ComApp.CommandState.StartDefaultCommand()
    End Sub

    Public Sub Cleanup() Implements BCOM.IPrimitiveCommandEvents.Cleanup
        If $safeitemname$Command IsNot Nothing Then
            $safeitemname$MSForm.DetachFromMicroStation()
            $safeitemname$MSForm.Dispose()
            $safeitemname$MSForm = Nothing
            $safeitemname$Command = Nothing
        End If
    End Sub

    Public Sub Dynamics(ByRef Point As BCOM.Point3d, View As BCOM.View, DrawMode As BCOM.MsdDrawingMode) Implements BCOM.IPrimitiveCommandEvents.Dynamics

    End Sub

    Public Sub Start() Implements BCOM.IPrimitiveCommandEvents.Start
        $safeitemname$MSForm.AttachToToolSettings(Program.Addin)
        $safeitemname$MSForm.Text = "$safeitemname$ Tool Settings"
    End Sub

    Public Sub Run(ByVal Optional unparsed As String = Nothing)
        If $safeitemname$Command Is Nothing Then
            $safeitemname$Command = New $safeitemname$()
            BMI.Utilities.ComApp.CommandState.StartPrimitive($safeitemname$Command, False)
        Else
            $safeitemname$MSForm.AttachToToolSettings(Program.Addin)
            $safeitemname$MSForm.Focus()
        End If
    End Sub
#End Region
End Class