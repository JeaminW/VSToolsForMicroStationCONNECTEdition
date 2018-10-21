'*--------------------------------------------------------------------------------------+
'   $safeitemname$.vb
'
'+--------------------------------------------------------------------------------------*/

#Region "System Namespaces"
Imports System.Windows.Forms
#End Region

#Region "Bentley Namespaces"
Imports BDPN = Bentley.DgnPlatformNET
Imports BG = Bentley.GeometryNET
Imports BM = Bentley.MstnPlatformNET
Imports BMW = Bentley.MstnPlatformNET.WinForms
#End Region

'Select Design in Configuration to easily switch to System.Windows.Form designer
#If DESIGN Then
<System.ComponentModel.DesignerCategory("designer")>
public partial class $safeitemrootname$
    Inherits Form
#Else
<System.ComponentModel.DesignerCategory("Code")>
Partial Public Class $safeitemrootname$
    Inherits BMW.Adapter
#End If

    ''' <summary>Constructor</summary>
    Friend Sub New()
        InitializeComponent()
    End Sub

End Class