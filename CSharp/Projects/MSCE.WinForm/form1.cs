using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
$if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
$endif$using System.Windows.Forms;
using BM = Bentley.MstnPlatformNET;
using BDPN = Bentley.DgnPlatformNET;

namespace $safeprojectname$
{
    public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }
}
}
