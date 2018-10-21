'--------------------------------------------------------------------------------------+
'|   $safeitemname$.vb
'|
'+--------------------------------------------------------------------------------------

#Region "System Namespaces"
Imports System.Windows.Controls
#End Region

#Region "Bentley Namespaces"
Imports BDPN = Bentley.DgnPlatformNET
Imports BG = Bentley.GeometryNET
Imports BM = Bentley.MstnPlatformNET
Imports BMW = Bentley.MstnPlatformNET.WPF
Imports BW = Bentley.Windowing
#End Region

Partial Public Class $safeitemrootname$
        Inherits UserControl

#Region "Bentley DockableWindow"
    Private Shared Property dockableWindow As BMW.DockableWindow

    ''' <summary>
    ''' Show the form if it is not already displayed
    ''' </summary>
    Friend Sub ShowWindow(ByVal Optional unparsed As String = Nothing)
        If dockableWindow IsNot Nothing Then
            dockableWindow.Focus()
            Return
        End If

        dockableWindow = New Bentley.MstnPlatformNET.WPF.DockableWindow()
        dockableWindow.Content = New $safeitemrootname$()
        dockableWindow.Attach(Program.Addin, "control", New Drawing.Size(Convert.ToInt32(dockableWindow.MinWidth), Convert.ToInt32(dockableWindow.MinHeight)))
        dockableWindow.WindowContent.CanDockVertically = False
        AddHandler dockableWindow.WindowContent.ContentCloseQuery, AddressOf OnClose
    End Sub

    ''' <summary>
    ''' Close and dispose the usercontrol.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Shared Sub OnClose(sender As Object, e As BW.ContentCloseEventArgs)
        e.CloseAction = BW.ContentCloseAction.Dispose
        dockableWindow.WindowContent.Hide()
        If dockableWindow IsNot Nothing Then
            dockableWindow.Detach()
            dockableWindow.Dispose()
            dockableWindow = Nothing
        End If
    End Sub
#End Region
End Class