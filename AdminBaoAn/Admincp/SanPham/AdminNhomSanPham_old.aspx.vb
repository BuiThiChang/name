
Imports Telerik.Web.UI

Public Class AdminNhomSanPham_old
    Inherits System.Web.UI.Page
    Public Shared tbDSHang As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadDSHangSX()
            LoadDsNhomCha()
            treeNhomSP.ExpandAllItems()
        End If
        CType(Master, MasterAdminSite).TieuDeChucNang = "Danh mục nhóm sản phẩm"
        Page.Title = "Danh mục nhóm sản phẩm"
    End Sub

    Public Sub LoadDSHangSX()
        Dim dt As DataTable = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable("SELECT MaHang,TenHang FROM HANGSANXUAT ORDER BY TenHang ")
        tbDSHang = dt
    End Sub


    Public Sub LoadDsNhomCha()
        Dim dt As DataTable = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable("SELECT Id,TenNhom_VN FROM NhomSanPham WHERE IdCha is null order by SoTT,TenNhom_VN ")
        cmbNhomSpCha.DataSource = dt
        cmbNhomSpCha.DataBind()
    End Sub

    Protected Sub btThem_Click(sender As Object, e As EventArgs) Handles btThem.Click
        PanelThem.Visible = True
        treeNhomSP.Visible = False
        btThem.Visible = False
        ID_HT.Value = ""
        'Dim tb As DataTable = ExecuteSQLDataTable("SELECT ISNULL(MAX(SoTT),0) +1 FROM NHOMSANPHAM")
        'If Not tb Is Nothing Then
        '    tbSTT.Value = Convert.ToInt32(tb.Rows(0)(0))
        'End If
        tbSTT.Value = 0
        tbTenNhomVN.Text = ""
        tbTenNhomEN.Text = ""
        tbTuKhoa.Text = ""
        tbMoTa.Text = ""
        tbHangNoiBat.Entries.Clear()
        cmbNhomSpCha.SelectedIndex = -1
        cmbNhomSpCha.Text = ""
    End Sub

    Private Sub treeNhomSP_DeleteCommand(sender As Object, e As Telerik.Web.UI.TreeListCommandEventArgs) Handles treeNhomSP.DeleteCommand
        Try

            Dim editItem = (DirectCast(e.Item, TreeListEditableItem))
            Dim values As New Hashtable()
            editItem.ExtractValues(values)
            Dim obj As New BaoAnLib.DbBaoAn

            obj.AddParameter("@id", values("Id"))
            Dim sql As String = "select count(id) from nhomsanpham where idCha = @id"
            Dim dt As DataTable = obj.ExecuteSQLDataTable(sql)
            If dt.Rows.Count > 0 Then Throw New Exception("Nhóm sản phẩm này hiện đang còn " & dt.Rows(0)(0).ToString & " nhóm con nên không thể xóa !")


            obj.AddParameter("@id", values("Id"))
            sql = "select count(id) from sanpham where NhomSanPham = @id"
            dt = obj.ExecuteSQLDataTable(sql)
            If dt Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
            If dt.Rows.Count > 0 Then Throw New Exception("Nhóm sản phẩm này hiện đang còn " & dt.Rows(0)(0).ToString & " sản phẩm nên không thể xóa !")
            obj.AddParameterWhere("@dkID", values("Id"))
            If obj.doDelete("NHOMSANPHAM", "Id=@dkID") Is Nothing Then
                Throw New Exception(obj.LoiNgoaiLe)
            Else
                rThongBao.ContentIcon = BienToanCuc._infoImg
                rThongBao.Text = "Đã xóa !"
                rThongBao.Show()
            End If
            treeNhomSP.ExpandAllItems()
        Catch ex As Exception
            rThongBao.Text = ex.Message
            rThongBao.Title = "Lỗi"
            rThongBao.ContentIcon = BienToanCuc._errImg
            rThongBao.Show()
        End Try
    End Sub


    Private Sub UpdateNhomSP(_id As Object)
        Dim obj As New BaoAnLib.DbBaoAn
        obj.AddParameter("@dkID", _id)
        Dim tb As DataTable = obj.ExecuteSQLDataTable("SELECT Id,IdCha,TenNhom_VN,TenNhom_EN,HangNoiBat,SoTT,TuKhoa,MoTa,TrangThai,qc_vn,qc_en FROM NHOMSANPHAM WHERE Id=@dkID")
        If Not tb Is Nothing Then
            PanelThem.Visible = True
            treeNhomSP.Visible = False
            btThem.Visible = False
            ID_HT.Value = tb.Rows(0)("Id")
            tbTenNhomVN.Text = tb.Rows(0)("TenNhom_VN").ToString
            tbTenNhomEN.Text = tb.Rows(0)("TenNhom_EN").ToString
            tbTuKhoa.Text = tb.Rows(0)("TuKhoa").ToString
            tbMoTa.Text = tb.Rows(0)("MoTa").ToString
            txtTiengViet.NoiDung = tb.Rows(0)("qc_vn").ToString
            txtTiengAnh.NoiDung = tb.Rows(0)("qc_en").ToString
            tbSTT.Value = CType(tb.Rows(0)("SoTT"), Int32)
            chkHienThi.Checked = Convert.ToBoolean(tb.Rows(0)("TrangThai"))
            If Not tb.Rows(0)("IdCha") Is DBNull.Value Then
                cmbNhomSpCha.SelectedValue = tb.Rows(0)("IdCha")
            Else
                cmbNhomSpCha.SelectedIndex = -1
                cmbNhomSpCha.Text = ""
            End If
            tbHangNoiBat.Entries.Clear()
            If tb.Rows(0)("HangNoiBat").ToString.Trim <> "" Then
                Dim tbDSH As DataTable = AdminBaoAn.ConvertDataStr.DataSourceDSFile(tb.Rows(0)("HangNoiBat").ToString.Trim, "MaHang", ",")
                Dim tbData As DataTable = CType(tbHangNoiBat.DataSource, DataTable)
                For i As Integer = 0 To tbDSH.Rows.Count - 1
                    Dim row() As DataRow = tbDSHang.Select("MaHang='" & tbDSH.Rows(i)("MaHang") & "'")
                    If row.Length > 0 Then
                        tbHangNoiBat.Entries.Add(New Telerik.Web.UI.AutoCompleteBoxEntry(row(0)("TenHang"), row(0)("MaHang")))
                    End If

                Next
            End If


        Else
            rThongBao.Text = obj.LoiNgoaiLe
            rThongBao.Title = "Lỗi"
            rThongBao.ContentIcon = "~/admincp/Images/icon_24_Error.png"
            rThongBao.Show()
        End If

        treeNhomSP.ExpandAllItems()

    End Sub

    Private Sub treeNhomSP_ItemCommand(sender As Object, e As Telerik.Web.UI.TreeListCommandEventArgs) Handles treeNhomSP.ItemCommand
        If e.CommandName = "EditNhomSP" Then
            Dim editItem = (DirectCast(e.Item, TreeListEditableItem))
            Dim values As New Hashtable()
            editItem.ExtractValues(values)

            UpdateNhomSP(values("Id"))
        End If
    End Sub


    Protected Sub btHuyTop_Click(sender As Object, e As EventArgs) Handles btHuyBottom.Click
        btThem.Visible = True
        PanelThem.Visible = False
        treeNhomSP.Visible = True
        tbSTT.Value = 0
        ID_HT.Value = ""
        tbTenNhomVN.Text = ""
        tbTenNhomEN.Text = ""
        tbTuKhoa.Text = ""
        tbMoTa.Text = ""
        cmbNhomSpCha.SelectedIndex = -1
        cmbNhomSpCha.Text = ""
        tbHangNoiBat.Entries.Clear()

    End Sub


    Protected Sub btLuuTop_Click(sender As Object, e As EventArgs) Handles btLuuBottom.Click
        LuuLai()
    End Sub

    Protected Sub LuuLai()
        Dim strIDHang As String = ""
        For i As Integer = 0 To tbHangNoiBat.Entries.Count - 1
            strIDHang &= tbHangNoiBat.Entries(i).Value
            If i < tbHangNoiBat.Entries.Count - 1 Then
                strIDHang &= ","
            End If
        Next

        Dim obj As New BaoAnLib.DbBaoAn
        obj.AddParameter("@TenNhom_VN", tbTenNhomVN.Text.Trim)
        obj.AddParameter("@TenNhom_EN", tbTenNhomEN.Text.Trim)
        obj.AddParameter("@MoTa", tbMoTa.Text.Trim)
        obj.AddParameter("@TuKhoa", tbTuKhoa.Text.Trim)
        obj.AddParameter("@SoTT", tbSTT.Value)
        obj.AddParameter("@HangNoiBat", strIDHang)
        obj.AddParameter("@TrangThai", chkHienThi.Checked)
        obj.AddParameter("@qc_vn", txtTiengViet.NoiDung)
        obj.AddParameter("@qc_en", txtTiengAnh.NoiDung)

        If cmbNhomSpCha.Text <> "" Then
            obj.AddParameter("@IdCha", cmbNhomSpCha.SelectedValue)
        Else
            obj.AddParameter("@IdCha", DBNull.Value)
        End If


        If ID_HT.Value = "" Then

            If obj.doInsert("NHOMSANPHAM") Is Nothing Then
                rThongBao.Text = obj.LoiNgoaiLe
                rThongBao.Title = "Lỗi"
                rThongBao.ContentIcon = BienToanCuc._errImg
                rThongBao.Show()
            Else
                treeNhomSP.Rebind()
                'resetForm()
                PanelThem.Visible = False
                treeNhomSP.Visible = True
                btThem.Visible = True

                rThongBao.Text = "Đã thêm nhóm sản phẩm !"
                rThongBao.ContentIcon = BienToanCuc._infoImg
                rThongBao.Show()
            End If
        Else
            obj.AddParameterWhere("@IDH", ID_HT.Value)
            If obj.doUpdate("NHOMSANPHAM", "Id=@IDH") = Nothing Then
                rThongBao.Text = obj.LoiNgoaiLe
                rThongBao.Title = "Lỗi"
                rThongBao.ContentIcon = BienToanCuc._errImg
                rThongBao.Show()
            Else
                treeNhomSP.Rebind()
                ' resetForm()
                PanelThem.Visible = False
                treeNhomSP.Visible = True
                btThem.Visible = True

                rThongBao.Text = "Đã sửa thông tin nhóm sản phẩm!"
                rThongBao.ContentIcon = BienToanCuc._infoImg
                rThongBao.Show()
            End If
        End If

        treeNhomSP.ExpandAllItems()
    End Sub


    'Private Sub grv_RowDrop(sender As Object, e As Telerik.Web.UI.GridDragDropEventArgs) Handles grv.RowDrop
    '    'rThongBao.Text = e.DraggedItems(0).Item("Id").Text

    '    'rThongBao.Text = e.DestDataItem("Id").Text
    '    'rThongBao.ContentIcon = BienToanCuc._infoImg
    '    'rThongBao.Show()


    'End Sub


    Private Sub treeNhomSP_NeedDataSource(sender As Object, e As Telerik.Web.UI.TreeListNeedDataSourceEventArgs) Handles treeNhomSP.NeedDataSource
        Dim dt As DataTable = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable("SELECT Id,IdCha,TenNhom_VN,TenNhom_EN,HangNoiBat,N''Hang,SoTT,TrangThai FROM NHOMSANPHAM ORDER BY SoTT,TenNhom_VN ")
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim tbdata As DataTable = AdminBaoAn.ConvertDataStr.DataSourceDSFile(dt.Rows(i)("HangNoiBat").ToString, "MaHang", ",")
            For j As Integer = 0 To tbdata.Rows.Count - 1
                Dim row() As DataRow = tbDSHang.Select("MaHang='" & tbdata.Rows(j)("MaHang") & "'")
                If row.Length > 0 Then
                    dt.Rows(i)("Hang") &= row(0)("TenHang")
                End If
                If j < tbdata.Rows.Count - 1 Then
                    dt.Rows(i)("Hang") &= ","
                End If
            Next
        Next
        treeNhomSP.DataSource = dt
    End Sub


End Class