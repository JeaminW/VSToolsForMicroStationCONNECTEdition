'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*

#Region "System Namespaces"
Imports System
Imports System.Drawing
#End Region

#Region "Bentley Namespaces"
Imports BMW = Bentley.MstnPlatformNET.WPF
Imports BMG = Bentley.MstnPlatformNET.GUI
#End Region

Class $safeitemname$
    Inherits BMW.DockableToolbar
    Implements BMG.IGuiDockable
    Private Shared Property $safeitemname$Toolbar As $safeitemname$
        Public Sub New()
        Dim $safeitemname$UserControl As $safeitemname$UC = New $safeitemname$UC()
        $safeitemname$UserControl.VerticalContentAlignment = System.Windows.VerticalAlignment.Center
        Me.Content = $safeitemname$UserControl

        Me.Title = "Dockable Toolbar"
        Me.Attach(Program.Addin, "$safeitemname$Toolbar")

        ' Setup AutoOpen after calling Attach()
        Me.AutoOpen = True
        'TODO: Change the following AutoOpenKeyin, If necessary.
        Me.AutoOpenKeyin = "mdl silentload $rootnamespace$,,DEFAULTDOMAIN;$rootnamespace$ $safeitemname$Keyin"
    End Sub

    ''' <summary>
    ''' React to the Window closed
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnClosed(e As EventArgs)
        MyBase.OnClosed(e)

        Me.Detach()
        Me.Dispose()
        $safeitemname$Toolbar = Nothing
    End Sub


    ''' <summary>
    ''' Creates And opens the tool bar.
    ''' </summary>
    ''' <param name="unparsed"></param>
    Public Sub ShowToolbar(ByVal Optional unparsed As String = Nothing)
        If $safeitemname$Toolbar IsNot Nothing Then
            $safeitemname$Toolbar.Focus()
                Return
        End If

        $safeitemname$Toolbar = New $safeitemname$()
        $safeitemname$Toolbar.Show()
    End Sub

    ''' <summary>
    ''' Closes the Form by closing the WindowContent.
    ''' </summary>
    Public Shared Sub CloseWindow()
        If $safeitemname$Toolbar IsNot Nothing Then
                $safeitemname$Toolbar.Close()
        End If
    End Sub

    Private Sub $safeitemname$_AttachingToHost(sender As Object, e As BMG.AttachingToHostEventArgs) Handles MyBase.AttachingToHost
        e.AttachPoint = New Point(0, 0)
        System.Diagnostics.Debug.WriteLine("$safeitemname$_AttachingToHost")
    End Sub

    Private Sub $safeitemname$_DetachingFromHost(sender As Object, e As EventArgs) Handles MyBase.DetachingFromHost
        System.Diagnostics.Debug.WriteLine("$safeitemname$_DetachingFromHost")
    End Sub

#Region "IGuiDockable Members"

    Private m_rejectedSize As Size = Size.Empty

    Public Function GetDockedExtent(dockPosition As BMG.GuiDockPosition, ByRef extentFlag As BMG.GuiDockExtent, ByRef dockExtent As Size) As Boolean Implements BMG.IGuiDockable.GetDockedExtent
        dockExtent.Height = Me.CommonDockSize.Height
        If dockPosition = BMG.GuiDockPosition.Top OrElse dockPosition = BMG.GuiDockPosition.Bottom Then
            dockExtent.Width = CInt(Me.ActualWidth)
            extentFlag = BMG.GuiDockExtent.Specified
        ElseIf dockPosition = BMG.GuiDockPosition.NotDocked Then
            extentFlag = BMG.GuiDockExtent.Specified
        Else
            extentFlag = BMG.GuiDockExtent.InvalidRegion
        End If
        Return True
    End Function

    Public Function WindowMoving(corners As BMG.WindowMovingCorner, ByRef newSize As Size) As Boolean Implements BMG.IGuiDockable.WindowMoving
        newSize.Height = CommonDockSize.Height

        If corners <> BMG.WindowMovingCorner.LowerRight OrElse m_rejectedSize.Equals(newSize) Then
            m_rejectedSize = newSize
            newSize.Width = CInt(Me.ActualWidth)
        End If
        Return True
    End Function

#End Region
End Class