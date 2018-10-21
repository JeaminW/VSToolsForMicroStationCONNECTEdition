/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
|   Main entry class establishing a connection to the MicroStation host.
|
+--------------------------------------------------------------------------------------*/

using System.Resources;
using BM = Bentley.MstnPlatformNET;

namespace $safeprojectname$
{
   $AddinAttribute$
internal sealed class Program : BM.AddIn
{
    public static Program Addin = null;

    private Program(System.IntPtr mdlDesc) : base(mdlDesc)
    {
        Addin = this;
    }

    /// <summary>
    /// Initializes the AddIn. Called by the AddIn loader after
    /// it has created the instance of this AddIn class
    /// </remarks>
    /// <param name="commandLine"></param>
    /// <returns>0 on success</returns>
    protected override int Run(string[] commandLine)
    {
        // Register event handlers
        ReloadEvent += $safeprojectname$_ReloadEvent;
        UnloadedEvent += $safeprojectname$_UnloadedEvent;
        //TODO: Add application initialization code here

        return 0;
    }

    ///<summary>
    /// Handles MDL ONUNLOAD requests when the application Is being unloaded.
    /// </summary>
    /// <param name="eventArgs"></param>
    private void $safeprojectname$_ReloadEvent(BM.AddIn sender, ReloadEventArgs eventArgs)
    {
        //TODO: add specific handling for this event here

    }

    /// <summary>
    /// Handles MicroStation UNLOADED event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void $safeprojectname$_UnloadedEvent(BM.AddIn sender, UnloadedEventArgs eventArgs)
    {
        //TODO: add specific handling for this event here

    }

    /// <summary>
    /// Handles MDL ONUNLOAD requests when the application is being unloaded.
    /// </summary>
    /// <param name="eventArgs"></param>
    protected override void OnUnloading(UnloadingEventArgs eventArgs)
    {
        base.OnUnloading(eventArgs);
    }

    #region Properties
    private static ResourceManager resourceManager
    {
        get { return Properties.Resources.ResourceManager; }
    }
    #endregion
}
}