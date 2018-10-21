/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region System Namespaces
using System;
using System.Drawing;
#endregion

#region Bentley Namespaces
using BMW = Bentley.MstnPlatformNET.WPF;
using BMG = Bentley.MstnPlatformNET.GUI;
#endregion

namespace $rootnamespace$
{
    class $safeitemname$ : BMW.DockableToolbar, BMG.IGuiDockable
    {
        private static $safeitemname$ $safeitemname$Toolbar { get; set; }

public $safeitemname$(string unparsed = "")
        {
            $safeitemname$UC $safeitemname$UserControl = new $safeitemname$UC();
            $safeitemname$UserControl.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            this.Content = $safeitemname$UserControl;

            this.Title = "Dockable Toolbar";
            this.AttachingToHost += new BMG.AttachingToHostEventHandler ($safeitemname$_AttachingToHost);
            this.DetachingFromHost += new EventHandler($safeitemname$_DetachingFromHost);

            this.Attach (Program.Addin, "$safeitemname$Toolbar");

            // Setup AutoOpen after calling Attach()
            this.AutoOpen = true;
            //TODO: Change the following AutoOpenKeyin, if necessary.
            this.AutoOpenKeyin = "mdl silentload $rootnamespace$,,DEFAULTDOMAIN;$rootnamespace$ $safeitemname$Keyin";
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>React to the Window closed</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.Detach();
            this.Dispose();
            $safeitemname$Toolbar = null;
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Creates and opens the test Form</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public void ShowToolbar(string unparsed = "")
        {
            if (null != $safeitemname$Toolbar)
            {
                $safeitemname$Toolbar.Focus();
                return;
    }

            $safeitemname$Toolbar = new $safeitemname$();
            $safeitemname$Toolbar.Show();
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Closes the Form by closing the WindowContent</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public static void CloseWindow()
        {
            if (null != $safeitemname$Toolbar)
            {
                $safeitemname$Toolbar.Close();
            }
        }

        void $safeitemname$_AttachingToHost(object sender, BMG.AttachingToHostEventArgs e)
        {
            e.AttachPoint = new Point(0, 0);
            System.Diagnostics.Debug.WriteLine("$safeitemname$_AttachingToHost");
        }

        void $safeitemname$_DetachingFromHost(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("$safeitemname$_DetachingFromHost");
        }

#region IGuiDockable Members

private Size m_rejectedSize = Size.Empty;

public bool GetDockedExtent(BMG.GuiDockPosition dockPosition, ref BMG.GuiDockExtent extentFlag, ref Size dockExtent)
{
    dockExtent.Height = this.CommonDockSize.Height;

    if (dockPosition == BMG.GuiDockPosition.Top ||
        dockPosition == BMG.GuiDockPosition.Bottom)
    {
        dockExtent.Width = (int)this.ActualWidth;
        extentFlag = BMG.GuiDockExtent.Specified;
    }
    else if (dockPosition == BMG.GuiDockPosition.NotDocked)
        extentFlag = BMG.GuiDockExtent.Specified;
    else
        extentFlag = BMG.GuiDockExtent.InvalidRegion;
    return true;
}

public bool WindowMoving(BMG.WindowMovingCorner corners, ref Size newSize)
{
    newSize.Height = CommonDockSize.Height;

    if (corners != BMG.WindowMovingCorner.LowerRight || m_rejectedSize.Equals(newSize))
    {
        m_rejectedSize = newSize;
        newSize.Width = (int)this.ActualWidth;
    }
    return true;
}

#endregion
}
}
