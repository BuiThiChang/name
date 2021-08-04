
Imports System.IO
Imports System.IO.Path
Imports Telerik.Web.UI
Imports System.Drawing

Public Class AdminSanPham_old
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then


            GanCmbLocNhomSP()
            GanCmbLocHangSX()
            GanCmbLocNguoiDang()
            If CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan.ToLower <> "admin" Then
                If Not CType(Session("AdminCP"), BaoAnLib.TaiKhoan).coQuyen("Thêm sản phẩm") Then btnTemSP.Enabled = False
            End If
            Dim t As DateTime = DateTime.UtcNow.AddHours(7)
            txtTuNgay.SelectedDate = New DateTime(t.Year, t.Month, 1)
            txtDenNgay.SelectedDate = New DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month))



        End If

        CType(Master, MasterAdminSite).TieuDeChucNang = "Danh mục Serie, Model sản phẩm"
        Page.Title = "Danh mục Serie, Model sản phẩm"

    End Sub

    Private Sub GanCmbLocNhomSP()
        Dim sql As String
        sql = "SELECT * FROM "
        sql &= "("
        sql &= "	SELECT NULL Id,N'-- Tất cả--' TenNhom,-1 SoTT "
        sql &= "UNION "
        sql &= "SELECT Id,TenNhom_VN as TenNhom,SoTT FROM NHOMSANPHAM "
        sql &= ")tbl "
        sql &= "ORDER BY SoTT,TenNhom "
        cmbLocNhomSP.DataSource = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable(sql)
        cmbLocNhomSP.DataTextField = "TenNhom"
        cmbLocNhomSP.DataValueField = "Id"
        cmbLocNhomSP.DataBind()
        cmbLocNhomSP.SelectedIndex = 0
    End Sub

    Private Sub GanCmbLocHangSX()
        Dim sql As String
        sql = "SELECT * FROM "
        sql &= "("
        sql &= "	SELECT NULL MaHang,N'-- Tất cả--' TenHang,-1 SoTT "
        sql &= "UNION "
        sql &= "SELECT MaHang,TenHang,SoTT FROM HANGSANXUAT "
        sql &= ")tbl "
        sql &= "ORDER BY SoTT,TenHang "
        cmbLocHangSX.DataSource = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable(sql)
        cmbLocHangSX.DataTextField = "TenHang"
        cmbLocHangSX.DataValueField = "MaHang"
        cmbLocHangSX.DataBind()
        cmbLocHangSX.SelectedIndex = 0
    End Sub

    Private Sub GanCmbLocNguoiDang()
        Dim sql As String
        sql = "SELECT * FROM "
        sql &= "("
        sql &= "	SELECT NULL TaiKhoan,N'-- Tất cả--' HoTen "
        sql &= "UNION "
        sql &= "SELECT TaiKhoan,HoTen FROM TAIKHOAN "
        sql &= ")tbl "
        sql &= "ORDER BY HoTen "
        cmbLocNguoiDang.DataSource = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable(sql)
        cmbLocNguoiDang.DataTextField = "HoTen"
        cmbLocNguoiDang.DataValueField = "TaiKhoan"
        cmbLocNguoiDang.DataBind()
        cmbLocNguoiDang.SelectedIndex = 0

        'cmbLocNguoiDang.SelectedValue = CType(Session("AdminCP"), TaiKhoan).TaiKhoan
    End Sub

    Private Sub gdvSerial_DeleteCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles gdvSerial.DeleteCommand

        Try

            Dim editItem = (DirectCast(e.Item, GridEditableItem))
            Dim values As New Hashtable()
            editItem.ExtractValues(values)

            If Convert.ToBoolean(values("TrangThai")) Then
                ShowThongBao(values("Bạn không thể xóa serie đã được duyệt !"))
                Exit Sub
            End If

            Dim objTK As BaoAnLib.TaiKhoan = CType(Session("AdminCP"), BaoAnLib.TaiKhoan)

            If objTK.TaiKhoan.ToLower <> "admin" Then
                If Not objTK.coQuyen("Xóa sản phẩm") Then
                    If values("NguoiDang").ToString.ToLower <> objTK.TaiKhoan.ToLower Then
                        ShowBaoLoi("Bạn không có quyền xóa serie của người khác đã đăng !")
                        Exit Sub
                    End If
                End If
            End If

            Dim obj As New BaoAnLib.DbBaoAn

            obj.AddParameter("@IdCha", values("Id"))
            Dim dt As DataTable = obj.ExecuteSQLDataTable("select count(id) from sanpham where idCha = @IdCha")
            If dt Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
            If dt.Rows(0)(0) > 0 Then Throw New Exception("Sản phẩm này vẫn còn " & dt.Rows(0)(0) & " model chưa xóa !")

            obj.AddParameter("@Id", values("Id"))
            dt = obj.ExecuteSQLDataTable("select HinhAnh,TaiLieu from sanpham where Id = @Id")
            If dt Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
            If dt.Rows.Count = 0 Then Throw New Exception("Không tìm thấy dữ liệu id = " & values("Id") & " !")
            Dim arrHinhAnh() As String = dt.Rows(0)("HinhAnh").ToString.Split(";")
            For Each _str As String In arrHinhAnh
                tryDelete(Server.MapPath("~/Images/SanPham/HinhTo/" & _str))
                tryDelete(Server.MapPath("~/Images/SanPham/HinhBe/" & _str))
            Next
            Dim arrTaiLieu() As String = dt.Rows(0)("TaiLieu").ToString.Split(";")
            For Each _str As String In arrTaiLieu
                tryDelete(Server.MapPath("~/Images/TaiLieu/" & _str))
            Next

            obj.AddParameterWhere("@dkID", values("Id"))
            If obj.doDelete("SANPHAM", "Id=@dkID") Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)

            Dim strThongBao As String = "Đã xóa thàng công sản phẩm !"
            ShowThongBao(strThongBao)

        Catch ex As Exception
            Dim str As String = ex.Message
            ShowBaoLoi(str)
        End Try

    End Sub


    Private Sub gdvModels_DeleteCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles gdvModels.DeleteCommand
        Try
            Dim editItem = (DirectCast(e.Item, GridEditableItem))
            Dim values As New Hashtable()
            editItem.ExtractValues(values)

            If Convert.ToBoolean(values("TrangThai")) Then
                ShowThongBao(values("Bạn không thể xóa model đã được duyệt !"))
                Exit Sub
            End If

            If CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan.ToLower <> "admin" Then
                If values("NguoiDang").ToString.ToLower <> CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan.ToLower Then
                    ShowBaoLoi("Bạn không có quyền xóa serie của người khác đã đăng !")
                    Exit Sub
                End If
            End If
            Dim obj As New BaoAnLib.DbBaoAn
            obj.AddParameterWhere("@dkID", values("Id"))
            If obj.doDelete("SANPHAM", "Id=@dkID") Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
            Dim strThongBao As String = "Đã xóa thàng công model !"
            ShowThongBao(strThongBao)
        Catch ex As Exception
            Dim str As String = ex.Message
            ShowBaoLoi(str)
        End Try
    End Sub


    Private Sub tryDelete(str As String)
        Try
            File.Delete(str)
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btSuaSerie_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try



            CType(CType(sender, Telerik.Web.UI.RadButton).NamingContainer, GridItem).Selected = True
            Dim index As Object = CType(CType(sender, Telerik.Web.UI.RadButton).NamingContainer, GridItem).ItemIndexHierarchical
            Dim dataItem = (DirectCast(gdvSerial.SelectedItems(index), GridDataItem))
            Dim values As New Hashtable()
            dataItem.ExtractValues(values)

            'If Convert.ToBoolean(values("TrangThai")) Then
            '    ShowBaoLoi("Serie này đã được duyệt nên không thể sửa !")
            '    Exit Sub
            'End If

            Dim obj As New BaoAnLib.DbBaoAn()

            obj.AddParameter("@dkID", values("Id"))
            lblThoiGianCapNhat.InnerHtml = ""

            Dim dt As DataTable = obj.ExecuteSQLDataTable("SELECT * FROM SANPHAM WHERE Id=@dkID")
            If dt Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
            If dt.Rows.Count = 0 Then Throw New Exception("Không tìm thấy dữ liệu id = " & values("Id"))
            Dim row As DataRow = dt.Rows(0)

            radTabSp.Visible = False
            radMutilPage.Visible = False
            panelThemSP.Visible = True
            btnTemSP.Enabled = False
            ID_SanPham.Value = values("Id")
            ID_Serie.Value = ""
            ClearDuLieu()
            InitDuLieuChung()

            lblTieuDeTop.InnerText = "Cập nhật nội dung sản phẩm"

            txtTenVN.Text = row("Ten_VN").ToString
            txtTenEN.Text = row("Ten_EN").ToString
            cmbHangSX.SelectedValue = row("HangSX")
            cmbXuatXu.SelectedValue = row("XuatXu")
            cmbNhomSP.SelectedValue = row("NhomSanPham")

            txtHoTroH.Entries.Clear()
            Dim arrHtKH() As String = row("HoTroKH").ToString.Split(";")
            For Each sStr As String In arrHtKH
                If sStr.Trim = "" Then Continue For
                txtHoTroH.Entries.Add(getNV(sStr))
            Next

            lstFileTaiLieu.Items.Clear()
            Dim arrFile() As String = row("TaiLieu").ToString.Split(";")
            For Each sStr As String In arrFile
                If sStr.Trim = "" Then Continue For
                lstFileTaiLieu.Items.Add(New RadListBoxItem(sStr, sStr))
            Next
            LST_Files.Value = row("TaiLieu")




            cmbTinhTrang.SelectedValue = row("TinhTrang")
            txtGiaBan.Text = row("GiaBan")

            txtMoTaVN.Text = row("MoTa_VN")
            txtMoTaEN.Text = row("MoTa_EN")

            txtNoiDungVN.NoiDung = row("NoiDung_VN")
            txtNoiDungEN.NoiDung = row("NoiDung_EN")

            txtTuKhoaSEO.Text = row("TuKhoaSEO")
            txtMoTaSEO.Text = row("MoTaSEO")
            chkIsSEO.Checked = CType(row("isSEO"), Boolean)

        Catch ex As Exception
            ShowBaoLoi(ex.Message)
        End Try


    End Sub

    Protected Sub btSuaModel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CType(CType(sender, Telerik.Web.UI.RadButton).NamingContainer, GridItem).Selected = True
            Dim index As Object = CType(CType(sender, Telerik.Web.UI.RadButton).NamingContainer, GridItem).ItemIndexHierarchical
            Dim dataItem = (DirectCast(gdvModels.SelectedItems(index), GridDataItem))
            Dim values As New Hashtable()
            dataItem.ExtractValues(values)

            'If Convert.ToBoolean(values("TrangThai")) Then
            '    ShowBaoLoi("Model này đã được duyệt nên không thể sửa !")
            '    Exit Sub
            'End If

            radTabSp.Visible = False
            radMutilPage.Visible = False
            panelThemSP.Visible = True
            btnTemSP.Enabled = False
            ID_SanPham.Value = values("Id")
            ClearDuLieu()
            InitDuLieuChung()
            lblTieuDeTop.InnerText = "Cập nhật model " & values("Ten")

            Dim obj As New BaoAnLib.DbBaoAn

            Dim dt As DataTable = obj.ExecuteSQLDataTable("SELECT * FROM SANPHAM WHERE Id = " & ID_Serie.Value)
            If dt.Rows.Count = 0 Then Throw New Exception("Không tìm thấy dữ liệu id = " & ID_Serie.Value)
            Dim row As DataRow = dt.Rows(0)


            txtTenVN.Text = row("Ten_VN").ToString
            txtTenEN.Text = row("Ten_EN").ToString
            cmbHangSX.SelectedValue = row("HangSX")
            cmbXuatXu.SelectedValue = row("XuatXu")
            cmbNhomSP.SelectedValue = row("NhomSanPham")
            txtMoTaVN.Text = row("MoTa_VN")
            txtMoTaEN.Text = row("MoTa_EN")

            txtHoTroH.Entries.Clear()
            Dim arrHtKH() As String = row("HoTroKH").ToString.Split(";")
            For Each sStr As String In arrHtKH
                If sStr.Trim = "" Then Continue For
                txtHoTroH.Entries.Add(getNV(sStr))
            Next

            lstFileTaiLieu.Items.Clear()
            Dim arrFile() As String = row("TaiLieu").ToString.Split(";")
            For Each sStr As String In arrFile
                If sStr.Trim = "" Then Continue For
                lstFileTaiLieu.Items.Add(New RadListBoxItem(sStr, sStr))
            Next
            LST_Files.Value = row("TaiLieu")




            dt = obj.ExecuteSQLDataTable("SELECT * FROM SANPHAM WHERE Id=" & values("Id"))
            If dt Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
            If dt.Rows.Count = 0 Then Throw New Exception("Không tìm thấy dữ liệu id = " & values("Id"))
            row = dt.Rows(0)

            txtModel.Text = row("Model")
            cmbTinhTrang.SelectedValue = row("TinhTrang")
            txtGiaBan.Text = row("GiaBan")


            txtNoiDungVN.NoiDung = row("NoiDung_VN")
            txtNoiDungEN.NoiDung = row("NoiDung_EN")

            txtTuKhoaSEO.Text = row("TuKhoaSEO")
            txtMoTaSEO.Text = row("MoTaSEO")
            chkIsSEO.Checked = CType(row("isSEO"), Boolean)


        Catch ex As Exception
            ShowBaoLoi(ex.Message)
        End Try
    End Sub

    Public Function toVisiableButton(taikhoan As String) As Boolean
        Dim objTK As BaoAnLib.TaiKhoan = CType(Session("AdminCP"), BaoAnLib.TaiKhoan)
        If objTK.TaiKhoan.ToLower = "admin" Then Return True
        If objTK.coQuyen("Sửa sản phẩm") Then Return True
        If objTK.TaiKhoan.ToLower.Equals(taikhoan.ToLower) Then Return True
        Dim obj As New BaoAnLib.DbBaoAn
        obj.AddParameter("@TaiKhoan", taikhoan)
        Dim dt As DataTable = obj.ExecuteSQLDataTable("select TrangThai from taikhoan where taikhoan = @TaiKhoan")
        If dt Is Nothing Then Return False
        If dt.Rows.Count = 0 Then Return False
        Return Not Convert.ToBoolean(dt.Rows(0)(0))
        Return False
    End Function

    Private Sub gdvSerial_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles gdvSerial.NeedDataSource



        Dim sql As String = "SET DATEFORMAT DMY "
        Dim obj As New BaoAnLib.DbBaoAn
        sql &= "declare @tblDL table(Id bigint,IdCha bigint) "
        sql &= "insert into @tblDL select id,IdCha from sanpham where "
        obj.AddParameter("@TuNgay1", txtTuNgay.SelectedDate)
        obj.AddParameter("@DenNgay1", txtDenNgay.SelectedDate)

        sql &= " convert(nvarchar," & cmbSapXepThoiGian.SelectedValue & ",103) >= @TuNgay1 "
        sql &= " and convert(nvarchar," & cmbSapXepThoiGian.SelectedValue & ",103) <= @DenNgay1 "

        If cmbLocNguoiDang.SelectedIndex > 0 Then
            sql &= " and NguoiDang = N'" & cmbLocNguoiDang.SelectedValue & "' "
        End If
        If cmbTrangThai.SelectedIndex = 1 Then
            sql &= " and TrangThai =  1 "
        ElseIf cmbTrangThai.SelectedIndex = 2 Then
            sql &= " and TrangThai =  0 "
        End If
        If cmbLocNhomSP.SelectedIndex > 0 Then sql &= " and NhomSanPham = " & cmbLocNhomSP.SelectedValue & " "
        If cmbLocHangSX.SelectedIndex > 0 Then sql &= " and HangSX = " & cmbLocHangSX.SelectedValue & " "
        If txtTimKiem.Text.Trim <> "" Then sql &= " and (Ten_VN Like N'%" & txtTimKiem.Text & "%' OR Model Like N'%" & txtTimKiem.Text & "%')  "

        sql &= " declare @tblIdSerie table(Id bigint) "
        sql &= " insert into @tblIdSerie select Id from @tblDL where IdCha is null "
        sql &= " insert into @tblIdSerie select distinct IdCha from @tblDL where IdCha is not null and idCha not in (select Id from @tblIdSerie) "

        sql &= " select Id,Ten_VN as Ten,TenNhomSP,TenNguoiDang,TenNguoiSua,NgayDang,NgaySua,NgayDuyet,TrangThai,NguoiDang,LuotXem, "
        sql &= " isnull((select max(" & cmbSapXepThoiGian.SelectedValue & ") from sanpham where IdCha = view_sanpham.Id)," & cmbSapXepThoiGian.SelectedValue & ")NgayModel, "
        sql &= " (select count(Id) from @tblDL where IdCha = view_sanpham.Id )ModelTongSo from view_sanpham where Id in (select Id from @tblIdSerie) "


        sql &= " ORDER BY NgayModel DESC "




        Dim dt As DataTable = obj.ExecuteSQLDataTable(sql)

        gdvSerial.DataSource = dt

        If dt Is Nothing Then
            ShowBaoLoi("Series: " & obj.LoiNgoaiLe)
            Exit Sub
        End If



        radTabSp.Tabs(0).Text = "Series (" & dt.Rows.Count & ")"


        'Response.Write(sql)
        'gdvSerial.DataBind()
    End Sub



    Private Sub gdvModels_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles gdvModels.NeedDataSource

        Try
            Dim sql As String = "SET DATEFORMAT DMY SELECT Id,Model as Ten,NguoiDang, "
            sql &= "TenNguoiDang,NgayDang,TenNguoiSua,NgaySua,TrangThai,LuotXem "
            sql &= "FROM view_SanPham tblSP WHERE IdCha = " & ID_Serie.Value & " "
            If cmbTrangThai.SelectedIndex = 1 Then
                sql &= " AND TrangThai =  1 "
            ElseIf cmbTrangThai.SelectedIndex = 2 Then
                sql &= " AND TrangThai =  0 "
            End If

            If cmbLocNguoiDang.SelectedIndex > 0 Then
                sql &= " AND ( NguoiDang = N'" & cmbLocNguoiDang.SelectedValue & "' "
                sql &= " OR (select count(id) from SANPHAM where IdCha = tblSP.Id AND NguoiDang = N'" & cmbLocNguoiDang.SelectedValue & "' ) > 0 ) "
            End If


            If txtTimKiem.Text.Trim <> "" Then
                sql &= " AND Model Like N'%" & txtTimKiem.Text & "%' "
            End If

            Dim obj As New BaoAnLib.DbBaoAn
            obj.AddParameter("@TuNgay", txtTuNgay.SelectedDate)
            obj.AddParameter("@DenNgay", txtDenNgay.SelectedDate)
            If cmbSapXepThoiGian.SelectedIndex = 0 Then
                sql &= " AND convert(nvarchar,NgaySua,103) >= @TuNgay AND convert(nvarchar,NgaySua,103) <= @DenNgay "
            ElseIf cmbSapXepThoiGian.SelectedIndex = 1 Then
                sql &= " AND convert(nvarchar,NgayDang,103) >= @TuNgay AND convert(nvarchar,NgayDang,103) <= @DenNgay "
            ElseIf cmbSapXepThoiGian.SelectedIndex = 2 Then
                sql &= " AND convert(nvarchar,NgayDuyet,103) >= @TuNgay AND convert(nvarchar,NgayDuyet,103) <= @DenNgay "
            End If


            sql &= " ORDER BY TrangThai, "

            If cmbSapXepThoiGian.SelectedIndex = 0 Then
                sql &= " NgaySua DESC "
            ElseIf cmbSapXepThoiGian.SelectedIndex = 1 Then
                sql &= " NgayDang DESC "
            ElseIf cmbSapXepThoiGian.SelectedIndex = 2 Then
                sql &= " NgayDuyet DESC "
            End If

            Dim dt As DataTable = obj.ExecuteSQLDataTable(sql)
            gdvModels.DataSource = dt
            radTabSp.Tabs(1).Text = Ten_Serie.Value & " (" & dt.Rows.Count & ")"
        Catch ex As Exception
            'ShowBaoLoi(LoiNgoaiLe)
        End Try

    End Sub


    Public Sub ShowModel(ByVal sender As Object, ByVal e As System.EventArgs)
        CType(CType(sender, Telerik.Web.UI.RadButton).NamingContainer, GridItem).Selected = True
        Dim index As Object = CType(CType(sender, Telerik.Web.UI.RadButton).NamingContainer, GridItem).ItemIndexHierarchical
        Dim dataItem = (DirectCast(gdvSerial.SelectedItems(index), GridDataItem))
        Dim values As New Hashtable()
        dataItem.ExtractValues(values)


        radTabSp.Tabs(1).Visible = True
        radTabSp.SelectedIndex = 1
        radMutilPage.SelectedIndex = 1

        ID_Serie.Value = values("Id")
        Ten_Serie.Value = values("Ten")
        ID_SanPham.Value = ""
        radTabSp.Tabs(1).Text = values("Ten")

        gdvModels.Rebind()

    End Sub

    Public Function ShowImgTrangThai(st As Boolean) As String
        If st Then
            Return "/Admincp/Content/Imgs/icon-16-default.png"
        Else
            Return "/Admincp/Content/Imgs/icon-16-notdefault.png"
        End If
    End Function


    Private Sub cmbLocNhomSP_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbLocNhomSP.SelectedIndexChanged, cmbLocHangSX.SelectedIndexChanged, cmbLocNguoiDang.SelectedIndexChanged, cmbTrangThai.SelectedIndexChanged
        gdvSerial.Rebind()
        radTabSp.Tabs(1).Visible = False
        radTabSp.SelectedIndex = 0
        radMutilPage.SelectedIndex = 0
    End Sub

    Private Sub cmbTrangThai_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbTrangThai.SelectedIndexChanged
        If cmbTrangThai.SelectedIndex = 1 Then
            cmbSapXepThoiGian.SelectedIndex = 2
            cmbSapXepThoiGian.Enabled = False
        Else
            cmbSapXepThoiGian.SelectedIndex = 0
            cmbSapXepThoiGian.Enabled = True
        End If

        gdvSerial.Rebind()
        radTabSp.Tabs(1).Visible = False
        radTabSp.SelectedIndex = 0
        radMutilPage.SelectedIndex = 0
    End Sub


    Private Sub txtTimKiem_TextChanged(sender As Object, e As System.EventArgs) Handles txtTimKiem.TextChanged
        gdvSerial.Rebind()
        radTabSp.Tabs(1).Visible = False
        radTabSp.SelectedIndex = 0
        radMutilPage.SelectedIndex = 0
    End Sub


    Private Sub btnTemSP_Click(sender As Object, e As System.EventArgs) Handles btnTemSP.Click

        radTabSp.Visible = False
        radMutilPage.Visible = False
        panelThemSP.Visible = True
        btnTemSP.Enabled = False
        lblThoiGianCapNhat.InnerHtml = ""



        ID_SanPham.Value = ""

        ClearDuLieu()

        If radTabSp.SelectedIndex = 0 Then ID_Serie.Value = ""


        InitDuLieuChung()

        cmbNhomSP.SelectedValue = cmbLocNhomSP.SelectedValue
        cmbHangSX.SelectedValue = cmbLocHangSX.SelectedValue


        Dim obj As New BaoAnLib.DbBaoAn

        txtHoTroH.Entries.Add(New Telerik.Web.UI.AutoCompleteBoxEntry(CType(Session("AdminCP"), BaoAnLib.TaiKhoan).HoTen & "<" & CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan & ">", CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan))

        If radTabSp.SelectedIndex = 0 Then
            lblTieuDeTop.InnerText = "Thêm mới serie sản phẩm"
        ElseIf radTabSp.SelectedIndex = 1 Then
            lblTieuDeTop.InnerText = Ten_Serie.Value
            Dim dt As DataTable = obj.ExecuteSQLDataTable("SELECT * FROM SANPHAM WHERE Id = " & ID_Serie.Value)
            If dt Is Nothing Then
                ShowBaoLoi(obj.LoiNgoaiLe & " - ")
                Exit Sub
            ElseIf dt.Rows.Count = 0 Then
                ShowBaoLoi("Không tìm thấy sản phẩm, vui lòng tải lại trang để thử lại !")
                Exit Sub
            End If
            Try
                Dim row As DataRow = dt.Rows(0)
                txtTenVN.Text = row("Ten_VN").ToString
                txtTenEN.Text = row("Ten_EN").ToString
                cmbHangSX.SelectedValue = row("HangSX")
                cmbXuatXu.SelectedValue = row("XuatXu")
                cmbNhomSP.SelectedValue = row("NhomSanPham")

                txtMoTaVN.Text = row("MoTa_VN").ToString
                txtMoTaEN.Text = row("MoTa_EN").ToString

                txtHoTroH.Entries.Clear()
                Dim arrHtKH() As String = row("HoTroKH").ToString.Split(";")
                For Each sStr As String In arrHtKH
                    If sStr.Trim = "" Then Continue For
                    txtHoTroH.Entries.Add(getNV(sStr))
                Next

                lstFileTaiLieu.Items.Clear()
                Dim arrFile() As String = row("TaiLieu").ToString.Split(";")
                For Each sStr As String In arrFile
                    If sStr.Trim = "" Then Continue For
                    lstFileTaiLieu.Items.Add(New RadListBoxItem(sStr, sStr))
                Next
                LST_Files.Value = row("TaiLieu")
            Catch ex As Exception
                ShowBaoLoi(ex.Message & " - vui lòng tải lại trang để thử lại !")
            End Try


        End If


    End Sub
    Public Function getNV(maNV As String) As AutoCompleteBoxEntry
        Dim obj As New BaoAnLib.DbBaoAn
        obj.AddParameter("@TaiKhoan", maNV)
        Dim dt As DataTable = obj.ExecuteSQLDataTable("SELECT HoTen,TaiKhoan FROM TAIKHOAN WHERE TaiKhoan = @TaiKhoan")
        Return New AutoCompleteBoxEntry(dt.Rows(0)("HoTen") & "<" & dt.Rows(0)("TaiKhoan") & ">", dt.Rows(0)("TaiKhoan"))
    End Function


    Private Sub btHuyTop_Click(sender As Object, e As System.EventArgs) Handles btHuyTop.Click, btnHuyBotom.Click
        radTabSp.Visible = True
        radMutilPage.Visible = True
        panelThemSP.Visible = False
        btnTemSP.Enabled = True
        If CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan.ToLower <> "admin" Then
            If Not CType(Session("AdminCP"), BaoAnLib.TaiKhoan).coQuyen("Thêm sản phẩm") Then btnTemSP.Enabled = False
        End If
        ID_SanPham.Value = ""
        ClearDuLieu()
    End Sub

    Private Sub ClearDuLieu()
        txtTenVN.Text = ""
        txtTenEN.Text = ""
        txtModel.Text = ""
        txtGiaBan.Text = "Liên hệ"
        txtHoTroH.Entries.Clear()
        txtMoTaVN.Text = ""
        txtMoTaEN.Text = ""
        txtNoiDungVN.NoiDung = ""
        txtNoiDungEN.NoiDung = ""
        txtTuKhoaSEO.Text = ""
        txtMoTaSEO.Text = ""
        lstFileTaiLieu.Items.Clear()
        LST_HinhAnh.Value = ""
        LST_Files.Value = ""
        'cmbXuatXu.SelectedIndex = -1
        'cmbHangSX.SelectedIndex = -1


    End Sub

    Private Sub InitDuLieuChung()

        'Itit khoi tao
        GanComboHangSX()
        GanComboXuatXu()
        GanComboNhomSP()
        GanComboTinhTrang()

        lstHinhAnh.Rebind()
        lstFileTaiLieu.Items.Clear()

        If radTabSp.SelectedIndex = 1 Then 'vo hieu hoa nut khi them mdel
            txtTenVN.Enabled = False
            txtTenEN.Enabled = False
            cmbHangSX.Enabled = False
            cmbXuatXu.Enabled = False
            cmbNhomSP.Enabled = False
            'lstHinhAnh.Enabled = False
            'btnUploadHinhAnh.Enabled = False
            'btnAnhChinh.Enabled = False
            'btnXoaAnh.Enabled = False
            btnUploadFile.Enabled = False
            btnXoaFile.Enabled = False
            txtHoTroH.Enabled = False
            txtMoTaVN.ReadOnly = True
            txtMoTaEN.ReadOnly = True
            txtModel.Enabled = True
        Else 'cho phep nhap lieu toan bo form khi them serie
            txtTenVN.Enabled = True
            txtTenEN.Enabled = True
            cmbHangSX.Enabled = True
            cmbXuatXu.Enabled = True
            cmbNhomSP.Enabled = True
            'lstHinhAnh.Enabled = True
            'btnUploadHinhAnh.Enabled = True
            'btnAnhChinh.Enabled = True
            'btnXoaAnh.Enabled = True
            btnUploadFile.Enabled = True
            btnXoaFile.Enabled = True
            txtHoTroH.Enabled = True
            txtMoTaVN.ReadOnly = False
            txtMoTaEN.ReadOnly = False
            txtModel.Enabled = False
        End If

        rTabNoiDung.SelectedIndex = 0
        rMultiPage.SelectedIndex = 0


        chkIsSEO.Checked = True













        'dtHinhAnh = New DataTable
        'dtHinhAnh.Columns.Add("HinhAnh")

        'For i As Integer = 1 To 4
        '    Dim row As DataRow
        '    row = dtHinhAnh.NewRow
        '    row("HinhAnh") = i & ".jpg"
        '    dtHinhAnh.Rows.InsertAt(row, dtHinhAnh.Rows.Count)
        'Next


    End Sub


    Private Sub lstHinhAnh_NeedDataSource(sender As Object, e As Telerik.Web.UI.RadListViewNeedDataSourceEventArgs) Handles lstHinhAnh.NeedDataSource

        Dim dtHinhAnh As New DataTable
        dtHinhAnh.Columns.Add("HinhAnh")

        If ID_SanPham.Value <> "" Or ID_Serie.Value <> "" Then

            'Dim idHinh As String = ID_Serie.Value.ToString
            'If idHinh = "" Then idHinh = ID_SanPham.Value.Trim

            Dim idHinh As String = ID_SanPham.Value.Trim

            Dim dt As DataTable = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable("select HinhAnh from sanpham where Id = " & idHinh)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim arrHinhAnh() As String = dt.Rows(0)("HinhAnh").ToString.Split(";")
                LST_HinhAnh.Value = dt.Rows(0)("HinhAnh").ToString
                For Each str As String In arrHinhAnh
                    Dim row As DataRow = dtHinhAnh.NewRow
                    row("HinhAnh") = str
                    dtHinhAnh.Rows.InsertAt(row, dtHinhAnh.Rows.Count)
                Next
            End If
        End If

        lstHinhAnh.DataSource = dtHinhAnh


    End Sub

    Private Sub GanComboHangSX()
        Dim sql As String = ""
        sql &= "SELECT MaHang,TenHang FROM HANGSANXUAT ORDER BY SoTT "
        cmbHangSX.DataSource = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable(sql)
        cmbHangSX.DataTextField = "TenHang"
        cmbHangSX.DataValueField = "MaHang"
        cmbHangSX.DataBind()
        cmbHangSX.Text = ""
        cmbHangSX.SelectedIndex = -1
    End Sub

    Private Sub GanComboXuatXu()
        Dim sql As String = ""
        sql &= "SELECT Id,Ten_VN FROM XUATXU ORDER BY SoTT "
        cmbXuatXu.DataSource = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable(sql)
        cmbXuatXu.DataTextField = "Ten_VN"
        cmbXuatXu.DataValueField = "Id"
        cmbXuatXu.DataBind()
        If cmbXuatXu.Items.Count > 0 Then cmbXuatXu.SelectedIndex = 0
    End Sub

    Private Sub GanComboNhomSP()
        Dim sql As String = ""
        sql &= "SELECT Id,TenNhom_VN FROM NHOMSANPHAM WHERE (SELECT count(id) FROM NHOMSANPHAM tbl WHERE tbl.IdCha = NHOMSANPHAM.Id) = 0 ORDER BY SoTT,TenNhom_VN "
        cmbNhomSP.DataSource = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable(sql)
        cmbNhomSP.DataTextField = "TenNhom_VN"
        cmbNhomSP.DataValueField = "Id"
        cmbNhomSP.DataBind()
        cmbNhomSP.SelectedIndex = -1
        cmbNhomSP.Text = ""
        cmbNhomSP.SelectedValue = Nothing
    End Sub

    Private Sub GanComboTinhTrang()
        Dim sql As String = ""
        sql &= "SELECT Id,Ten FROM TINHTRANGHANG ORDER BY SoTT "
        cmbTinhTrang.DataSource = New BaoAnLib.DbBaoAn().ExecuteSQLDataTable(sql)
        cmbTinhTrang.DataTextField = "Ten"
        cmbTinhTrang.DataValueField = "Id"
        cmbTinhTrang.DataBind()
        If cmbTinhTrang.Items.Count > 0 Then cmbTinhTrang.SelectedIndex = 0
    End Sub




    Private Sub btLuuTop_Click(sender As Object, e As System.EventArgs) Handles btLuuTop.Click, btnLuuBottom.Click


        Try


            If Not CheckHopLe() Then Throw New Exception()

            'Upload hinh anh
            'If ID_Serie.Value = "" Then
            Dim strHinhAnh As String = ""
            If btnUploadHinhAnh.UploadedFiles.Count > 0 Then

                For i As Integer = 0 To btnUploadHinhAnh.UploadedFiles.Count - 1
                    'btnUploadHinhAnh.UploadedFiles(i).SaveAs("~/Admincp/Content/tmp/img_", True)
                    Dim bm As Bitmap = Bitmap.FromStream(btnUploadHinhAnh.UploadedFiles(i).InputStream)
                    If bm.Width <> bm.Height Or (bm.Width <> 500 And bm.Width <> 600 And bm.Width <> 700) Then
                        Dim strLoi As String = "Hình ảnh sản phẩm <b>""" & btnUploadHinhAnh.UploadedFiles(i).GetNameWithoutExtension & """ (" & bm.Width & "x" & bm.Height & ")</b> không hợp lệ !"
                        bm.Dispose()
                        Throw New Exception(strLoi)
                    End If
                    bm.Dispose()
                Next

                For i As Integer = 0 To btnUploadHinhAnh.UploadedFiles.Count - 1
                    Dim tg As String = DateTime.UtcNow.AddHours(7).ToString("ddMMyyyyhhmmss")
                    Dim fileName As String = GetFileNameWithoutExtension(btnUploadHinhAnh.UploadedFiles(i).FileName) & " " & tg & GetExtension(btnUploadHinhAnh.UploadedFiles(i).FileName)
                    Dim bm As Bitmap = Bitmap.FromStream(btnUploadHinhAnh.UploadedFiles(i).InputStream)
                    bm.Save(Server.MapPath("~/Images/SanPham/HinhTo/" & fileName))
                    bm.Dispose()
                    'btnUploadHinhAnh.UploadedFiles(i).SaveAs(Server.MapPath("~/Images/SanPham/HinhTo/" & fileName))
                    Dim imgHinhBe As Image = createThumbnailSP(Server.MapPath("~/Images/SanPham/HinhTo/" & fileName))
                    imgHinhBe.Save(Server.MapPath("~/Images/SanPham/HinhBe/" & fileName))
                    strHinhAnh &= fileName
                    If i < btnUploadHinhAnh.UploadedFiles.Count - 1 Then strHinhAnh &= ";"
                Next


            End If

            If LST_HinhAnh.Value <> "" And strHinhAnh <> "" Then
                strHinhAnh = LST_HinhAnh.Value & ";" & strHinhAnh
            ElseIf LST_HinhAnh.Value <> "" And strHinhAnh = "" Then
                strHinhAnh = LST_HinhAnh.Value
            End If
            LST_HinhAnh.Value = strHinhAnh
            Dim obj As New BaoAnLib.DbBaoAn
            obj.AddParameter("@HinhAnh", strHinhAnh)
            'End If

            'Upload file dinh kem
            If ID_Serie.Value = "" Then
                Dim strFile As String = ""
                If btnUploadFile.UploadedFiles.Count > 0 Then
                    For i As Integer = 0 To btnUploadFile.UploadedFiles.Count - 1
                        Dim tg As String = DateTime.UtcNow.AddHours(7).ToString("ddMMyyyyhhmmss")
                        Dim fileName As String = GetFileNameWithoutExtension(btnUploadFile.UploadedFiles(i).FileName) & " " & tg & GetExtension(btnUploadFile.UploadedFiles(i).FileName)
                        btnUploadFile.UploadedFiles(i).SaveAs(Server.MapPath("~/TaiLieu/" & fileName))
                        strFile &= fileName
                        If i < btnUploadFile.UploadedFiles.Count - 1 Then strFile &= ";"
                    Next
                End If
                If LST_Files.Value <> "" And strFile <> "" Then
                    strFile = LST_Files.Value & ";" & strFile
                ElseIf LST_Files.Value <> "" And strFile = "" Then
                    strFile = LST_Files.Value
                End If
                LST_Files.Value = strFile
                obj.AddParameter("@TaiLieu", strFile)
            End If


            obj.AddParameter("@Ten_VN", txtTenVN.Text)
            obj.AddParameter("@Ten_EN", txtTenEN.Text)
            obj.AddParameter("@Model", txtModel.Text)
            obj.AddParameter("@HangSX", cmbHangSX.SelectedValue)
            obj.AddParameter("@XuatXu", cmbXuatXu.SelectedValue)
            obj.AddParameter("@TinhTrang", cmbTinhTrang.SelectedValue)
            obj.AddParameter("@GiaBan", txtGiaBan.Text)

            If ID_Serie.Value = "" Then
                Dim strHoTroKH As String = ""
                For i As Integer = 0 To txtHoTroH.Entries.Count - 1
                    strHoTroKH &= txtHoTroH.Entries(i).Value
                    If i < txtHoTroH.Entries.Count - 1 Then strHoTroKH &= ";"
                Next
                obj.AddParameter("@HoTroKH", ";" & strHoTroKH & ";")
            End If

            If ID_Serie.Value = "" Then
                obj.AddParameter("@MoTa_VN", txtMoTaVN.Text)
                obj.AddParameter("@MoTa_EN", txtMoTaEN.Text)
            End If

            obj.AddParameter("@NhomSanPham", cmbNhomSP.SelectedValue)

            If ID_Serie.Value <> "" Then
                obj.AddParameter("@IdCha", ID_Serie.Value)
            Else
                obj.AddParameter("@IdCha", DBNull.Value)
            End If

            obj.AddParameter("@NoiDung_VN", txtNoiDungVN.NoiDung)
            obj.AddParameter("@NoiDung_EN", txtNoiDungEN.NoiDung)

            obj.AddParameter("@TuKhoaSEO", txtTuKhoaSEO.Text)
            obj.AddParameter("@MoTaSEO", txtMoTaSEO.Text)
            obj.AddParameter("@isSEO", chkIsSEO.Checked)

            If ID_SanPham.Value = "" Then 'truong hop addnew
                obj.AddParameter("@NguoiDang", CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan)
                obj.AddParameter("@NgayDang", DateTime.UtcNow.AddHours(7))
            End If

            obj.AddParameter("@NguoiSua", CType(Session("AdminCP"), BaoAnLib.TaiKhoan).TaiKhoan)
            obj.AddParameter("@NgaySua", DateTime.UtcNow.AddHours(7))

            Dim strThongBao As String = ""

            If ID_SanPham.Value = "" Then
                Dim idSP As Object = obj.doInsert("SANPHAM")
                If idSP Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
                ID_SanPham.Value = idSP

                'If ID_Serie.Value = "" Then ID_Serie.Value = idSP

                strThongBao = "Thêm mới dữ liệu thành công !"
            Else
                obj.AddParameterWhere("@dk_ID", ID_SanPham.Value)
                If obj.doUpdate("SANPHAM", "Id = @dk_ID") Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
                strThongBao = "Cập nhật dữ liệu thành công !"
            End If

            If radTabSp.SelectedIndex = 0 Then
                gdvSerial.Rebind()
            ElseIf radTabSp.SelectedIndex = 1 Then
                gdvModels.Rebind()
            End If


            'rebin lai list hinh anh
            lstHinhAnh.Rebind()
            'rebin lai list file
            lstFileTaiLieu.Items.Clear()
            Dim arrFile() As String = LST_Files.Value.Split(";")
            For Each str As String In arrFile
                lstFileTaiLieu.Items.Add(New RadListBoxItem(str))
            Next

            ShowThongBao(strThongBao)
            lblThoiGianCapNhat.InnerHtml = " - <img src='/Admincp/Content/Imgs/confirm.png'> đã cập nhật lúc " & DateTime.UtcNow.AddHours(7).ToString("HH:mm:ss") & " - <a target='_blank' style='text-decoration: none;' href='/vn/sanpham-" & ID_SanPham.Value & "/NhomSP/HangSX/TenSP.aspx'>xem thử</a>"


        Catch ex As Exception
            lblThoiGianCapNhat.InnerHtml = " - <img src='/Admincp/Content/Imgs/icon-16-alert.png'> có lỗi lúc " & DateTime.UtcNow.AddHours(7).ToString("HH:mm:ss")
            ShowBaoLoi(ex.Message)
        End Try



    End Sub

    Private Function CheckHopLe() As Boolean

        If txtTenVN.Text.Trim = "" Or txtTenEN.Text.Trim = "" Then
            Throw New Exception("Chưa nhập tên sản phẩm !")
            Return False
        End If
        If ID_Serie.Value <> "" And txtModel.Text = "" Then
            Throw New Exception("Chưa nhập tên model sản phẩm !")
            Return False
        End If
        If cmbHangSX.SelectedIndex < 0 Then
            Throw New Exception("Chưa chọn hãng sản xuất !")
            Return False
        End If
        If cmbXuatXu.SelectedIndex < 0 Then
            Throw New Exception("Chưa chọn xuất xứ !")
            Return False
        End If
        If cmbNhomSP.SelectedIndex < 0 Then
            Throw New Exception("Chưa chọn nhóm sản phẩm !")
            Return False
        End If
        If txtHoTroH.Entries.Count = 0 Then
            Throw New Exception("Chưa chọn người hỗ trợ khách hàng !")
            Return False
        End If


        Dim obj As New BaoAnLib.DbBaoAn()
        Dim sql As String = ""

        If ID_Serie.Value <> "" Then 'San pham la Model
            sql = "SELECT Id FROM SANPHAM WHERE Model = N'" & txtModel.Text & "' AND IdCha is not null "
        Else 'San pham la Series
            sql = "SELECT Id FROM SANPHAM WHERE (Ten_VN = N'" & txtTenVN.Text & "' OR Ten_EN = N'" & txtTenEN.Text & "') AND IdCha is null "
        End If

        Dim dt As DataTable = obj.ExecuteSQLDataTable(sql)
        If dt Is Nothing Then
            Throw New Exception(obj.LoiNgoaiLe)
            Return False
        End If


        If ID_SanPham.Value = "" Then 'Truong hop add new
            If dt.Rows.Count > 0 Then
                Throw New Exception("Tên sản phẩm hoặc model đã tồn tại, vui lòng kiểm tra lại !")
                Return False
            End If
        Else
            If dt.Rows.Count > 0 AndAlso dt.Rows(0)("Id") <> ID_SanPham.Value Then
                Throw New Exception("Tên sản phẩm hoặc model đã tồn tại, vui lòng kiểm tra lại !")
                Return False
            End If
        End If



        'If txtMoTaVN.Text.Trim = "" Or txtMoTaEN.Text.Trim = "" Then
        '    ShowBaoLoi("Chưa nhập nội dung mô tả !")
        '    Return False
        'End If
        'If txtNoiDungVN.NoiDung.Trim = "" Or txtNoiDungEN.NoiDung.Trim = "" Then
        '    ShowBaoLoi("Chưa nhập nội dung !")
        '    Return False
        'End If

        Return True
    End Function

    Private Function createThumbnailSP(ByVal imgUrl As String) As System.Drawing.Image
        Dim newWidth As Integer = 160
        Dim newHeight As Integer = 160
        Dim oImg As System.Drawing.Image = System.Drawing.Image.FromFile(imgUrl)
        Dim newImage As System.Drawing.Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.DrawImage(oImg, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function


    Private Sub ShowBaoLoi(str As String)
        rThongBao.Text = str
        rThongBao.Title = "Lỗi"
        rThongBao.ContentIcon = "~/admincp/content/imgs/icon_24_Error.png"
        rThongBao.Show()
    End Sub

    Private Sub ShowThongBao(str As String)
        rThongBao.Text = str
        rThongBao.Title = "Chú ý"
        rThongBao.ContentIcon = "~/admincp/content/imgs/icon_24_info.png"
        rThongBao.Show()
    End Sub


    Private Sub btnTaiLai_Click(sender As Object, e As System.EventArgs) Handles btnTaiLai.Click
        If radTabSp.SelectedIndex = 0 Then
            gdvSerial.Rebind()
        ElseIf radTabSp.SelectedIndex = 1 Then
            gdvModels.Rebind()
        End If
    End Sub

    Private Sub btnAnhChinh_Click(sender As Object, e As System.EventArgs) Handles btnAnhChinh.Click
        If ID_SanPham.Value = "" Then Throw New Exception("Không có hình để thay đổi !")

        Try
            Dim arrHinhAnh() As String = LST_HinhAnh.Value.ToString.Split(";")
            Dim img As String = arrHinhAnh(lstHinhAnh.CurrentPageIndex)
            Dim strHinhNew As String = ""
            For Each _str As String In arrHinhAnh
                If _str = img Then Continue For
                If strHinhNew <> "" Then strHinhNew &= ";"
                strHinhNew &= _str
            Next

            Dim strHinhUpdate As String = ""
            If strHinhNew <> "" Then
                strHinhUpdate = img & ";" & strHinhNew
            Else
                strHinhUpdate = img
            End If
            Dim obj As New BaoAnLib.DbBaoAn
            obj.AddParameter("@HinhAnh", strHinhUpdate)
            obj.AddParameterWhere("@dk_Id", ID_SanPham.Value)
            If obj.doUpdate("SANPHAM", "Id = @dk_Id") Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)

            lstHinhAnh.Rebind()
            ShowThongBao("Thay đổi ảnh chính thành công !")
        Catch ex As Exception
            ShowBaoLoi(ex.Message)
        End Try




    End Sub

    Private Sub btnXoaAnh_Click(sender As Object, e As System.EventArgs) Handles btnXoaAnh.Click

        Dim obj As New BaoAnLib.DbBaoAn

        Try

            obj.BeginTransaction()

            If ID_SanPham.Value = "" Then Throw New Exception("Không có hình để xóa !")

            Dim arrHinhAnh() As String = LST_HinhAnh.Value.ToString.Split(";")
            Dim img As String = arrHinhAnh(lstHinhAnh.CurrentPageIndex)
            Dim strHinhNew As String = ""
            For Each _str As String In arrHinhAnh
                If _str = img Then Continue For
                If strHinhNew <> "" Then strHinhNew &= ";"
                strHinhNew &= _str
            Next

            obj.AddParameter("@HinhAnh", strHinhNew)
            obj.AddParameterWhere("@dk_Id", ID_SanPham.Value)
            If obj.doUpdate("SANPHAM", "Id = @dk_Id") Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)


            tryDelete(Server.MapPath("~/Images/SanPham/HinhTo/" & img))
            tryDelete(Server.MapPath("~/Images/SanPham/HinhBe/" & img))


            obj.ComitTransaction()
            ShowThongBao("Xóa hình thành công !")
            lstHinhAnh.Rebind()


        Catch ex As Exception
            obj.RollBackTransaction()
            ShowBaoLoi(ex.Message)
        End Try




    End Sub


    Private Sub btnXoaFile_Click(sender As Object, e As System.EventArgs) Handles btnXoaFile.Click

        If lstFileTaiLieu.SelectedItems.Count = 0 Then
            ShowBaoLoi("Chưa chọn file cần xóa !")
            Exit Sub
        End If

        If ID_SanPham.Value = "" Then Throw New Exception("Không có file để xóa !")

        Try
            Dim arrFile() As String = LST_Files.Value.ToString.Split(";")
            Dim file As String = lstFileTaiLieu.SelectedItems(0).Value
            lstFileTaiLieu.Items.Remove(lstFileTaiLieu.SelectedItems(0))
            Dim strFileNew As String = ""
            For Each _str As String In arrFile
                If _str = file Then Continue For
                If strFileNew <> "" Then strFileNew &= ";"
                strFileNew &= _str
            Next
            Dim obj As New BaoAnLib.DbBaoAn
            obj.AddParameter("@TaiLieu", strFileNew)
            obj.AddParameterWhere("@dk_Id", ID_SanPham.Value)
            If obj.doUpdate("SANPHAM", "Id = @dk_Id") Is Nothing Then Throw New Exception(obj.LoiNgoaiLe)
            tryDelete(Server.MapPath("~/TaiLieu/" & file))
            LST_Files.Value = strFileNew
            ShowThongBao("Đã xóa file tài liệu thành công !")
        Catch ex As Exception
            ShowBaoLoi(ex.Message)
        End Try


    End Sub



    Private Sub btnTaiFile_Click(sender As Object, e As System.EventArgs) Handles btnTaiFile.Click
        If lstFileTaiLieu.SelectedItems.Count = 0 Then
            ShowBaoLoi("Chưa chọn file từ danh sách !")
            Exit Sub
        End If
        Dim url As String = "/TaiLieu/" & lstFileTaiLieu.SelectedItems(0).Text

        Response.Redirect(url)

        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "window.open('2.aspx','_newtab');", True)

        'Dim script As String = "<script type=""text/javascript"">window.open('" & url.ToString & "');</script>"
        'ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)


    End Sub


    Private Sub cmbSapXepThoiGian_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbSapXepThoiGian.SelectedIndexChanged
        gdvSerial.Rebind()
        radTabSp.Tabs(1).Visible = False
        radTabSp.SelectedIndex = 0
        radMutilPage.SelectedIndex = 0
    End Sub


    Private Sub txtTuNgay_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles txtTuNgay.SelectedDateChanged, txtDenNgay.SelectedDateChanged
        gdvSerial.Rebind()
        radTabSp.Tabs(1).Visible = False
        radTabSp.SelectedIndex = 0
        radMutilPage.SelectedIndex = 0
    End Sub


    Private Sub btnUploadHinhAnh_FileUploaded(sender As Object, e As Telerik.Web.UI.FileUploadedEventArgs) Handles btnUploadHinhAnh.FileUploaded
        'e.IsValid = False
    End Sub


End Class

