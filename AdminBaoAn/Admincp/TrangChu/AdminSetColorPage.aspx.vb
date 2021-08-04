Imports Telerik.Web.UI

Public Class AdminSetColorPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PanelColor.Visible = True
        CType(Master, MasterAdminSite).TieuDeChucNang = "Thay đổi màu Website"
        Page.Title = "Thay đổi màu Website"
    End Sub

    Public Sub getTableColor()
        Dim dt As DataTable = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable("Select * from MAUWEBSITE")

        txtNenChinh.Text = dt.Rows(0)("NenChinh")
        txtTopMXH.Text = dt.Rows(0)("TopMXH")
        txtTopMXH_hover.Text = dt.Rows(0)("TopMXH_hover")
        txtChu_Top_GioiThieu.Text = dt.Rows(0)("Chu_Top_GioiThieu")
        txtNen_Top_GioiThieu.Text = dt.Rows(0)("Nen_Top_GioiThieu")
        txtChu_Top_GioiThieu_Hover.Text = dt.Rows(0)("Chu_Top_GioiThieu_Hover")
        txtNen_Top_GioiThieu_Hover.Text = dt.Rows(0)("Nen_Top_GioiThieu_Hover")
        txtClick_GioiThieu.Text = dt.Rows(0)("Click_GioiThieu")
        txtChu_TrangChu.Text = dt.Rows(0)("Chu_TrangChu")
        txtNen_TrangChu.Text = dt.Rows(0)("Nen_TrangChu")
        txtChu_TrangChu_Hover.Text = dt.Rows(0)("Chu_TrangChu_Hover")
        txtNen_TrangChu_Hover.Text = dt.Rows(0)("Nen_TrangChu_Hover")
        txtbottom_DangDuocQuanTam.Text = dt.Rows(0)("bottom_DangDuocQuanTam")
        txtbottom_DuAnMoi.Text = dt.Rows(0)("bottom_DuAnMoi")
    End Sub
    Protected Sub btnGetColor_Click(sender As Object, e As EventArgs) Handles buttonGetColor.Click
        getTableColor()
    End Sub
    Protected Sub btnSaveColor_Click(sender As Object, e As EventArgs) Handles btnSaveColor.Click
        Dim query As String
        query = " Update MAUWEBSITE set"
        query &= " NenChinh = '" + txtNenChinh.Text.ToString + "',TopMXH='" + txtTopMXH.Text.ToString + "',"
        query &= "    TopMXH_hover='" + txtTopMXH_hover.Text.ToString + "',Chu_Top_GioiThieu='" + txtChu_Top_GioiThieu.Text.ToString + "',"
        query &= "    Nen_Top_GioiThieu='" + txtNen_Top_GioiThieu.Text.ToString + "', Chu_Top_GioiThieu_Hover= '" + txtChu_Top_GioiThieu_Hover.Text.ToString + "', Nen_Top_GioiThieu_Hover= '" + txtNen_Top_GioiThieu_Hover.Text.ToString + "', "
        query &= "    Click_GioiThieu='" + txtClick_GioiThieu.Text.ToString + "', Chu_TrangChu= '" + txtChu_TrangChu.Text.ToString + "', "
        query &= "    Nen_TrangChu='" + txtNen_TrangChu.Text.ToString + "', Chu_TrangChu_Hover= '" + txtChu_TrangChu_Hover.Text.ToString + "', "
        query &= "    Nen_TrangChu_Hover='" + txtNen_TrangChu_Hover.Text.ToString + "', bottom_DangDuocQuanTam= '" + txtbottom_DangDuocQuanTam.Text.ToString + "', "
        query &= "    bottom_DuAnMoi='" + txtbottom_DuAnMoi.Text.ToString + "' "

        query = New BaoAnLib.DbBaoAn().ExecuteSQLNonQuery(query)
    End Sub

End Class