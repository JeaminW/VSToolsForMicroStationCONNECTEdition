/*--------------------------------------------------------------------------------------+
|   $safeitemname$.cs
|
+--------------------------------------------------------------------------------------*/

#region System Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

#region Bentley Namespaces
using BDPN = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;
using BM = Bentley.MstnPlatformNET;
using BMW = Bentley.MstnPlatformNET.WinForms;
using BW = Bentley.Windowing;
#endregion

namespace $rootnamespace$
{
    //Select Design in Configuration to easily switch to System.Windows.Form designer
#if DESIGN
    [System.ComponentModel.DesignerCategory("designer")]
    public partial class $safeitemrootname$ : Form
#else
    [System.ComponentModel.DesignerCategory("code")]
public partial class $safeitemrootname$ : BMW.Adapter
#endif
    {
       internal $safeitemrootname$()
       {
            InitializeComponent();
}
    }
}