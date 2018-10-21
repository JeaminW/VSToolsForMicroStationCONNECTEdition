'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
''
'+--------------------------------------------------------------------------------------*/

Imports BM = Bentley.MstnPlatformNET

$AddinAttribute$
Friend NotInheritable Class Program
    Inherits BM.AddIn
    Public Shared Addin As Program = Nothing

    Private Sub New(mdlDesc As System.IntPtr)
        MyBase.New(mdlDesc)
        Addin = Me
    End Sub

    ''' <summary>The AddIn loader creates an instance of a class
    ''' derived from Bentley.MicroStation.AddIn and invokes Run.
    ''' </summary>
    Protected Overrides Function Run(commandLine As String()) As Integer
        ' Get the localized resources

        Return 0
    End Function

    ''' <summary>
    ''' Handles MicroStation UNLOADED event.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="eventArgs"></param>
    Private Sub $safeprojectname$_ReloadEvent(sender As BM.AddIn, eventArgs As ReloadEventArgs) Handles MyBase.ReloadEvent
        'TODO: add specific handling For this Event here

    End Sub

    ''' <summary>
    ''' Handles MicroStation UNLOADED event.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="eventArgs"></param>
    Protected Sub $safeprojectname$_UnloadedEvent(sender As BM.AddIn, eventArgs As UnloadedEventArgs) Handles MyBase.UnloadedEvent
        'TODO: add specific handling For this Event here

    End Sub

    ''' <summary>
    ''' Handles MDL ONUNLOAD requests when the application Is being unloaded.
    ''' </summary>
    ''' <param name="eventArgs"></param>
    Protected Overrides Sub OnUnloading(eventArgs As UnloadingEventArgs)
        MyBase.OnUnloading(eventArgs)
    End Sub
End Class