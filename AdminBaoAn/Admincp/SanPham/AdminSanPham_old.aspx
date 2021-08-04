<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admincp/MasterAdminSite.Master"
    CodeBehind="AdminSanPham_old.aspx.vb" Inherits="AdminBaoAn.AdminSanPham_old" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../Uctrl/BAOAN_EDITTOR.ascx" TagName="BAOAN_EDITTOR" TagPrefix="baoan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  
<script type="text/javascript">
    function RadConfirmXoaAnh(sender, args) {
        var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
            if (shouldSubmit) {
                this.click();
            }
        });

        var text = "Bạn có chắc muốn xóa hình này không?";
        radconfirm(text, callBackFunction, 320, 120, null, "Chú ý");
        args.set_cancel(true);
    }

    function RadConfirmXoaFile(sender, args) {
        var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
            if (shouldSubmit) {
                this.click();
            }
        });

        var text = "Bạn có chắc muốn xóa file này không?";
        radconfirm(text, callBackFunction, 320, 120, null, "Chú ý");
        args.set_cancel(true);
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="panelAax">
            <UpdatedControls >
                <telerik:AjaxUpdatedControl ControlID="panelAax" LoadingPanelID="loading" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="loading" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div id="panelAax" runat="server">

    <table width="100%" cellpadding="5" style="background: #f5f5f5; border-bottom: 1px solid #bbbbbb;">
        <tr>
            <td style="width: 30px;">
                Nhóm
            </td>
            <td style="width: 220px;">
                <telerik:RadComboBox Width="220" ID="cmbLocNhomSP" runat="server" AutoPostBack="true"
                    MaxHeight="350" AllowCustomText="true" Filter="Contains">
                </telerik:RadComboBox>
            </td>
            <td style="width: 30px;">
                Hãng
            </td>
            <td style="width: 143px;">
                <telerik:RadComboBox Width="143px" ID="cmbLocHangSX" runat="server" AutoPostBack="true" MaxHeight="350"
                    AllowCustomText="true" Filter="Contains">
                </telerik:RadComboBox>
            </td>
            <td style="width: 65px;">
                Người đăng
            </td>
            <td style="width: 185px;">
                <telerik:RadComboBox Width="185px" ID="cmbLocNguoiDang" runat="server" AutoPostBack="true" MaxHeight="350"
                    AllowCustomText="true" Filter="Contains">
                </telerik:RadComboBox>
            </td>
             <td style="width: 55px;">
                Trạng thái
            </td>
            <td style="width: 100px;">
                <telerik:RadComboBox Width="95px" ID="cmbTrangThai" Filter="Contains" runat="server" AutoPostBack="true">
                    <Items>
                        <telerik:RadComboBoxItem Text="Tất Cả" />
                        <telerik:RadComboBoxItem Text="Đã Duyệt" />
                        <telerik:RadComboBoxItem Text="Chưa Duyệt" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" cellpadding="5" style="background: #f5f5f5; border-bottom: 1px solid #bbbbbb;">
        <tr>
        <td style="width: 33px;">
                    Từ
                </td>
                <td style="width: 100px;">
                    <telerik:RadDatePicker Width="100" ID="txtTuNgay" AutoPostBack="true" runat="server">
                    </telerik:RadDatePicker>
                </td>
                <td style="width: 15px;">
                    Đến
                </td>
                <td style="width: 100px;">
                    <telerik:RadDatePicker Width="100" AutoPostBack="true" ID="txtDenNgay" runat="server">
                    </telerik:RadDatePicker>
                </td>

                 <td style="width: 45px;">
                Sắp xếp
            </td>
            <td style="width: 100px;">
                <telerik:RadComboBox Width="110px" ID="cmbSapXepThoiGian" runat="server" Filter="Contains" AutoPostBack="true">
                    <Items>
                        <telerik:RadComboBoxItem Text="Ngày sửa" Selected="true" Value="NgaySua" />
                        <telerik:RadComboBoxItem Text="Ngày đăng" Value="NgayDang" />
                        <telerik:RadComboBoxItem Text="Ngày duyệt" Value="NgayDuyet" />
                    </Items>
                </telerik:RadComboBox>
            </td>

            <td style="width: 60px;">
                Tìm kiếm
            </td>
            <td style="width: 250px;">
                <telerik:RadTextBox ID="txtTimKiem" EmptyMessage="tìm kiếm serial hoặc model sản phẩm ..."
                    Width="250" runat="server" AutoPostBack="true">
                </telerik:RadTextBox>
            </td>
            <td>
                <telerik:RadButton ID="btnTemSP" runat="server" Text="Thêm mới" Icon-PrimaryIconUrl="~/admincp/Content/Imgs/icon-16-new.png">
                </telerik:RadButton>
            </td>
            <td align="right">
                <telerik:RadButton ID="btnTaiLai" runat="server" Text="Tải lại" Icon-PrimaryIconUrl="~/admincp/Content/Imgs/icon-16-clear.png">
                </telerik:RadButton>
            </td>
        </tr>
    </table>
    <telerik:RadTabStrip ID="radTabSp" runat="server" SelectedIndex="0" MultiPageID="radMutilPage">
        <Tabs>
            <telerik:RadTab Text="Series" PageViewID="pageSerial" Font-Bold="true" ForeColor="red"
                Font-Names="Roboto Condensed" Font-Size="14px">
            </telerik:RadTab>
            <telerik:RadTab Text="Models" PageViewID="pageModel" Visible="false" ForeColor="Blue"
                Font-Names="Roboto Condensed" Font-Size="14px" Font-Bold="true">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="radMutilPage" runat="server" SelectedIndex="0">
        <telerik:RadPageView ID="pageSerial" runat="server" >
            <telerik:RadGrid ID="gdvSerial" runat="server" AllowPaging="True" AutoGenerateColumns="true"
                CellSpacing="0" Culture="vi-VN" GridLines="None" ShowStatusBar="True" AutoGenerateHierarchy="True"
                AllowMultiRowSelection="false" Font-Names="Roboto Condensed">
                <ClientSettings>
                    <Selecting CellSelectionMode="None" AllowRowSelect="false"></Selecting>
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" CommandItemDisplay="None" CommandItemSettings-RefreshText="Tải lại"
                    CommandItemSettings-AddNewRecordText="Thêm bài viêt" NoMasterRecordsText="Không có dữ liệu !">
                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <HeaderStyle Font-Bold="true" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tên series">
                            <ItemStyle />
                            <ItemTemplate>
                                <a href='/KiemDuyet/KiemDuyetSanPham.aspx?lang=vn&id=<%# Eval("Id")%>' target="_blank" class="LinkTieuDe" style="color: #333;">
                                    <%# Eval("Ten")%></a>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Models">
                            <ItemStyle />
                            <ItemTemplate>
                                <center>
                                    <telerik:RadButton Skin="Office2010Silver" Font-Names="Roboto Condensed" OnClick="ShowModel"
                                        ID="radViewListModels" Value='<%# Eval("Id")%>' runat="server" Text='<%# Eval("ModelTongSo")%>'>
                                    </telerik:RadButton>
                                </center>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </telerik:GridTemplateColumn>
                         <telerik:GridBoundColumn DataField="LuotXem" FilterControlAltText="Filter LuotXem column"
                            HeaderText="Xem" UniqueName="LuotXem">
                            <HeaderStyle Width="60px" />
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenNhomSP" FilterControlAltText="Filter TenNhomSP column"
                            HeaderText="Nhóm sản phẩm" UniqueName="TenNhomSP">
                            <HeaderStyle Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Ten" UniqueName="Ten" Display="false">
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn DataField="NguoiDang" UniqueName="NguoiDang" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenNguoiDang" FilterControlAltText="Filter TenNguoiDang column"
                            HeaderText="Người đăng" UniqueName="TenNguoiDang">
                            <HeaderStyle Width="120px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NgayDang" FilterControlAltText="Filter NgayDang column"
                      DataFormatString="{0:HH:mm ddd dd/MM/yyyy}" DataType="System.DateTime"
                            HeaderText="Ngày đăng" UniqueName="NgayDang">
                            <HeaderStyle Width="80px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="TenNguoiSua" FilterControlAltText="Filter TenNguoiSua column"
                            HeaderText="Người sửa" UniqueName="TenNguoiSua">
                            <HeaderStyle Width="120px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="NgaySua" FilterControlAltText="Filter NgaySua column"
                      DataFormatString="{0:HH:mm ddd dd/MM/yyyy}" DataType="System.DateTime"
                            HeaderText="Ngày sửa" UniqueName="NgaySua">
                            <HeaderStyle Width="80px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="Duyệt">
                            <ItemStyle VerticalAlign="Middle" Width="25px" />
                            <ItemTemplate>
                                <center>
                                    <img src='<%# ShowImgTrangThai( Eval("TrangThai") ) %>' />
                                </center>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Sửa">
                            <ItemTemplate>
                                <telerik:RadButton Visible='<%# toVisiableButton(Eval("NguoiDang")) %>' ID="btSuaSerie"  OnClick="btSuaSerie_Click" runat="server" BorderStyle="None" Width="16px" ToolTip="Sửa serie"
                                    Height="16px" Image-ImageUrl="~/admincp/Content/Imgs/icon-16-edit.png">
                                </telerik:RadButton>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </telerik:GridTemplateColumn>



                        <telerik:GridButtonColumn  Text="Xóa" ConfirmText="Xóa sản phẩm này ?" HeaderStyle-HorizontalAlign="Center"
                            ConfirmTitle="Chú ý" ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px"
                            HeaderText="Xóa" ConfirmDialogType="RadWindow" ImageUrl="~/admincp/Content/Imgs/icon-16-delete.png">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridButtonColumn>


                         <telerik:GridBoundColumn DataField="TrangThai" Display="false">
                         </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="Id" FilterControlAltText="Filter Id column" HeaderText="Id"
                            UniqueName="Id">
                            <HeaderStyle Width="10px" />
                        </telerik:GridBoundColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template" InsertCaption="Thêm bài viêt">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <HeaderStyle HorizontalAlign="Center" ForeColor="#003366"></HeaderStyle>
                </MasterTableView>
                <PagerStyle FirstPageToolTip="Trang đầu" LastPageToolTip="Trang cuối" NextPagesToolTip="Trang tiếp "
                    NextPageToolTip="Trang tiếp" PagerTextFormat="Đổi trang: {4} &amp;nbsp;Trang &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, từ &lt;strong&gt;{2}&lt;/strong&gt; đến &lt;strong&gt;{3}&lt;/strong&gt; trong tổng số &lt;strong&gt;{5}&lt;/strong&gt; series."
                    PageSizeLabelText="Cỡ trang:" PrevPagesToolTip="Trang sau" PrevPageToolTip="Trang sau" />
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="pageModel" runat="server">
            <telerik:RadGrid ID="gdvModels" runat="server" AllowPaging="True" AutoGenerateColumns="true"
                CellSpacing="0" Culture="vi-VN" GridLines="None" ShowStatusBar="True" AutoGenerateHierarchy="True"
                AllowMultiRowSelection="false">
                <ClientSettings>
                    <Selecting CellSelectionMode="None" AllowRowSelect="false"></Selecting>
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" CommandItemDisplay="None" CommandItemSettings-RefreshText="Tải lại"
                    CommandItemSettings-AddNewRecordText="Thêm bài viêt" NoMasterRecordsText="Không có dữ liệu !">
                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <HeaderStyle Font-Bold="true" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tên model">
                            <ItemStyle />
                            <ItemTemplate>
                                <a href='/KiemDuyet/KiemDuyetSanPham.aspx?lang=vn&id=<%# Eval("Id")%>' target="_blank" class="LinkTieuDe" style="color: #333;">
                                    <%# Eval("Ten")%></a>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="LuotXem" FilterControlAltText="Filter LuotXem column"
                            HeaderText="Xem" UniqueName="LuotXem">
                            <HeaderStyle Width="60px" />
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenNguoiDang" FilterControlAltText="Filter TenNguoiDang column"
                            HeaderText="Người đăng" UniqueName="TenNguoiDang">
                            <HeaderStyle Width="120px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NgayDang" FilterControlAltText="Filter NgayDang column"
                        DataType="System.DateTime" DataFormatString="{0:HH:mm -ddd dd/MM/yyyy}"
                            HeaderText="Ngày đăng" UniqueName="NgayDang">
                            <HeaderStyle Width="80px" />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="TenNguoiSua" FilterControlAltText="Filter TenNguoiSua column"
                            HeaderText="Người sửa" UniqueName="TenNguoiSua">
                            <HeaderStyle Width="120px" />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="NgaySua" FilterControlAltText="Filter NgaySua column"
                        DataType="System.DateTime" DataFormatString="{0:HH:mm -ddd dd/MM/yyyy}"
                            HeaderText="Ngày sửa" UniqueName="NgaySua">
                            <HeaderStyle Width="80px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Duyệt">
                            <ItemStyle VerticalAlign="Middle" Width="25px" />
                            <ItemTemplate>
                                <center>
                                    <img src='<%# ShowImgTrangThai( Eval("TrangThai") ) %>' />
                                </center>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridBoundColumn DataField="NguoiDang" UniqueName="NguoiDang" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="Sửa">
                            <ItemTemplate>
                                <telerik:RadButton Visible='<%# toVisiableButton(Eval("NguoiDang")) %>' ID="btSuaModel" OnClick="btSuaModel_Click" runat="server" BorderStyle="None" Width="16px" ToolTip="Sửa bài viêt"
                                  Height="16px" Image-ImageUrl="~/admincp/Content/Imgs/icon-16-edit.png">
                                </telerik:RadButton>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn Text="Xóa" ConfirmText="Xóa model này ?" HeaderStyle-HorizontalAlign="Center"
                            ConfirmTitle="Chú ý" ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px"
                            HeaderText="Xóa" ConfirmDialogType="RadWindow" ImageUrl="~/admincp/Content/Imgs/icon-16-delete.png">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridButtonColumn>
                        <telerik:GridBoundColumn DataField="Id" FilterControlAltText="Filter Id column" HeaderText="Id"
                            UniqueName="Id">
                            <HeaderStyle Width="10px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TrangThai" Display="false">
                         </telerik:GridBoundColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template" InsertCaption="Thêm bài viêt">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <HeaderStyle HorizontalAlign="Center" ForeColor="#003366"></HeaderStyle>
                </MasterTableView>
                <PagerStyle FirstPageToolTip="Trang đầu" LastPageToolTip="Trang cuối" NextPagesToolTip="Trang tiếp "
                    NextPageToolTip="Trang tiếp" PagerTextFormat="Đổi trang: {4} &amp;nbsp;Trang &lt;strong&gt;{0}&lt;/strong&gt; / &lt;strong&gt;{1}&lt;/strong&gt;, từ &lt;strong&gt;{2}&lt;/strong&gt; đến &lt;strong&gt;{3}&lt;/strong&gt; trong tổng số &lt;strong&gt;{5}&lt;/strong&gt; models."
                    PageSizeLabelText="Cỡ trang:" PrevPagesToolTip="Trang sau" PrevPageToolTip="Trang sau" />
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    <div id="panelThemSP" runat="server" visible="false" style="background-color: #f5f5f5;
        padding: 10px; border: 1px solid #d5d5d5; box-shadow: 0 1px 2px rgba(0,0,0,0.2);
        color: #333; font-family: 'Roboto Condensed' , arial, serif;">
        <div id="fixedButton" style="padding: 5px; background-color: #F2F5FA; padding-left: 5px;">
            <table border="0">
                <tr>
                    <td>
                        <telerik:RadButton ID="btLuuTop" runat="server" ValidationGroup="KT" Text="Lưu lại"
                            Icon-PrimaryIconUrl="~/admincp/Content/Imgs/icon-16-save.png">
                        </telerik:RadButton>
                    </td>
                    <td style="padding:5px;">
                        <telerik:RadButton ID="btHuyTop" runat="server" Text="Quay lại" Icon-PrimaryIconUrl="~/admincp/Content/Imgs/back.png">
                        </telerik:RadButton>
                    </td>
                    <td>
                        <span style="color: #666; height: 35px; font-size: 16px; font-weight: normal; padding-left: 10px;"
                            runat="server" id="lblTieuDeTop">Thêm mới serie sản phẩm</span>
                    </td>
                    <td style="margin-left:5px;color:#4C607A;" id="lblThoiGianCapNhat" runat="server"></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="ID_Serie" runat="server" Value="" />
        <asp:HiddenField ID="Ten_Serie" runat="server" Value="" />
        <asp:HiddenField ID="ID_SanPham" runat="server" Value="" />
        <asp:HiddenField ID="LST_HinhAnh" runat="server" Value="" />
        <asp:HiddenField ID="LST_Files" runat="server" Value="" />
        <table border="0" width="100%" style="background-color: #f5f5f5; border: 1px solid #fff;
            box-shadow: 0 1px 2px rgba(0,0,0,0.2); padding: 5px; margin-top: 5px; border-radius: 10px;">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width: 65px;">
                                Tên VN
                            </td>
                            <td>
                                <telerik:RadTextBox EmptyMessage="Tên sản phẩm tiếng việt ..." Width="420" ID="txtTenVN"
                                    runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 65px; padding-left: 15px;">
                                Tên EN
                            </td>
                            <td>
                                <telerik:RadTextBox EmptyMessage="Tên sản phẩm tiếng anh ..." Width="420" ID="txtTenEN"
                                    runat="server">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0">
                        <tr>
                            <td style="width: 65px;">
                                Model
                            </td>
                            <td>
                                <telerik:RadTextBox Width="165" ID="txtModel" runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 65px; padding-left: 15px;">
                                Hãng SX
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbHangSX" runat="server" Width="170" MaxHeight="350" AllowCustomText="true"
                                    Filter="Contains">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 65px; padding-left: 15px;">
                                Xuất xứ
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbXuatXu" runat="server" Width="130" MaxHeight="350" AllowCustomText="true"
                                    Filter="Contains">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 65px; padding-left: 15px;">
                                Nhóm SP
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbNhomSP" runat="server" Width="200" MaxHeight="350" AllowCustomText="true"
                                    Filter="Contains">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0">
                        <tr>
                            <td style="width: 65px;">
                                Tình trạng
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbTinhTrang" runat="server" Width="165" MaxHeight="350"
                                    AllowCustomText="true" Filter="Contains">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 65px; padding-left: 15px;">
                                Giá bán
                            </td>
                            <td>
                                <telerik:RadTextBox Text="Liên hệ" Width="165" ID="txtGiaBan" runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 65px; padding-left: 20px;">
                                Hỗ trợ KH
                            </td>
                            <td>
                                <asp:SqlDataSource ID="sqlData" runat="server" ConnectionString="<%$ ConnectionStrings:strChuoiKetNoi %>"
                                    ProviderName="System.Data.SqlClient" SelectCommand="SELECT TaiKhoan, (HoTen + N' <' + TaiKhoan + N'>')HoTen FROM TAIKHOAN WHERE (TrangThai = 1)">
                                </asp:SqlDataSource>
                                <telerik:RadAutoCompleteBox ID="txtHoTroH" runat="server" Width="420px" DataSourceID="sqlData"
                                    EmptyMessage="Điền tên nhân viên hỗ trợ sản phẩm này ..." DropDownWidth="400px"
                                    InputType="Token" DataTextField="HoTen" DataValueField="TaiKhoan">
                                    <TokensSettings AllowTokenEditing="true" />
                                </telerik:RadAutoCompleteBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0">
                        <tr>
                            <td style="width: 65px;">
                                Mô tả VN
                            </td>
                            <td style="width: 165px;">
                                <telerik:RadTextBox MaxLength="255" Width="165" Height="200" TextMode="MultiLine" ID="txtMoTaVN"
                                    runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 65px; padding-left: 15px;">
                                Mô tả EN
                            </td>
                            <td style="width: 165px;">
                                <telerik:RadTextBox MaxLength="255" Width="165" Height="200" TextMode="MultiLine" ID="txtMoTaEN"
                                    runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 70px; padding-left: 15px;">
                                Hình ảnh <br /><br />
                                <i>
                                (500 x 500)<br />
                                (600 x 600)<br />
                                (700 x 700)</i>

                            </td>
                            <td>
                                <div style="overflow: hidden; height: 200px; width: 150px; border: 1px solid #B8CBDE;
                                    background-color: #fff;">
                                    <telerik:RadListView runat="server" ID="lstHinhAnh" PageSize="1" ItemPlaceholderID="HinhAnhContainer"
                                    DataKeyNames="HinhAnh" AllowMultiItemSelection = "false" 
                                      DataMember="HinhAnh"  AllowPaging="true">
                                        <LayoutTemplate>
                                            <div style="height: 150px; background-image: url('/admincp/Content/Imgs/khongcohinh.png')">
                                                <asp:PlaceHolder ID="HinhAnhContainer" runat="server"></asp:PlaceHolder>
                                            </div>
                                            <center>
                                                <div style="text-align: center; margin-top: 5px; height: 46px; vertical-align: middle;
                                                    display: table-cell;">
                                                    <asp:Button runat="server" ID="btnPrev" CommandName="Page" CommandArgument="Prev"
                                                        Text="Trước" Enabled="<%#Container.CurrentPageIndex > 0 %>"></asp:Button>
                                                    <span style="vertical-align: middle; line-height: 22px; display: inline-block;">
                                                        <%#Container.CurrentPageIndex + 1 %>
                                                        /
                                                        <%#Container.PageCount %></span>
                                                    <asp:Button runat="server" ID="btnNext" CommandName="Page" CommandArgument="Next"
                                                        Text="Sau" Enabled="<%#Container.CurrentPageIndex + 1 < Container.PageCount %>">
                                                    </asp:Button>
                                                </div>
                                            </center>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <img width="150" height="150" src='/Images/SanPham/HinhBe/<%# Eval("HinhAnh") %>' />
                                        </ItemTemplate>
                                    </telerik:RadListView>
                                    <telerik:RadDataPager ID="RadDataPager1" runat="server">
                                    </telerik:RadDataPager>
                                </div>
                            </td>
                            <td valign="top">
                                <telerik:RadAsyncUpload Width="250" runat="server" ID="btnUploadHinhAnh" MaxFileInputsCount="25"
                                  MultipleFileSelection="Automatic"  AllowedFileExtensions="jpeg,jpg,gif,png">
                                    <Localization Select="Thêm ảnh" Cancel="Hủy" Remove="Xóa" />
                                </telerik:RadAsyncUpload>
                                <span style="margin-left: 75px; display: block; margin-top: -29px;">
                                    <telerik:RadButton ID="btnAnhChinh" runat="server" Text="Ảnh chính">
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="btnXoaAnh" OnClientClicking="RadConfirmXoaAnh" runat="server" Text="Xóa ảnh">
                                    </telerik:RadButton>
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="height: 1px; color: #d5d5d5; background-color: #d5d5d5; border: 0px;width:1000px;margin-bottom:10px;margin-top:5px;" ></div>
                    <table border="0">
                        <tr>
                            <td style="width: 65px;">
                                Nội dung
                            </td>
                            <td>
                                <telerik:RadTabStrip  ID="rTabNoiDung" runat="server" SelectedIndex="0" MultiPageID="rMultiPage">
                                    <Tabs>
                                        <telerik:RadTab ImageUrl="~/images/vn.png" Text="Nội dung tiếng Việt" Font-Names="Roboto Condensed" Font-Size="14px" Font-Bold="false" PageViewID="RadPageView1" Selected="true">
                                        </telerik:RadTab>
                                        <telerik:RadTab ImageUrl="~/images/uk.png" Text="Nội dung tiếng Anh" Font-Names="Roboto Condensed" Font-Size="14px" Font-Bold="false" PageViewID="RadPageView2">
                                        </telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage ID="rMultiPage" runat="server">
                                    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                                        <baoan:BAOAN_EDITTOR Cao="600px" Rong="1120px" ID="txtNoiDungVN" runat="server" />
                                    </telerik:RadPageView>
                                    <telerik:RadPageView  ID="RadPageView2" runat="server">
                                        <baoan:BAOAN_EDITTOR  Cao="600px" Rong="1120px" ID="txtNoiDungEN" runat="server" />
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
               
                 <div style="height: 1px; color: #d5d5d5; background-color: #d5d5d5; border: 0px;width:1000px;margin-bottom:10px;margin-top:5px;" ></div>

                    <table>

                        <tr>

                            <td style="width: 65px;">
                                Từ khóa SEO
                            </td>
                            <td>
                                <telerik:RadTextBox EmptyMessage="Đặt từ khóa SEO ON PAGE, ngăn cách dấu phẩy ..."
                                    Width="420" ID="txtTuKhoaSEO" runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 65px; padding-left: 15px;">
                                Ghi chú
                            </td>
                            <td>
                                <telerik:RadTextBox EmptyMessage="Thông tin khác ..."
                                    Width="150" ID="txtMoTaSEO" runat="server">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsSEO" runat="server" Checked="true" Text="SEO URL theo chuẩn mới" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0">
                        <tr>
                            <td style="width: 65px;">
                                Tài liệu
                            </td>
                            <td>
                                <div style="height: 200px; width: 420px; border: 1px solid #B8CBDE; background-color: #f5f5f5;">
                                    <telerik:RadListBox Height="200px" Width="420px" ID="lstFileTaiLieu" runat="server">
                                    </telerik:RadListBox>
                                </div>
                            </td>
                            <td valign="top" style="padding: 5px;">
                                <telerik:RadAsyncUpload runat="server" ID="btnUploadFile" MaxFileInputsCount="25"
                                MultipleFileSelection="Automatic"
                                    AllowedFileExtensions="doc,xls,pdf,zip">
                                    <Localization Select="Thêm file" Cancel="Hủy" Remove="Xóa" />
                                </telerik:RadAsyncUpload>
                                <span style="margin-left: 75px; display: block; margin-top: -29px;">
                                    <telerik:RadButton ID="btnTaiFile" runat="server" Text="Tải file">
                                    </telerik:RadButton>
                                    <telerik:RadButton ID="btnXoaFile" OnClientClicking="RadConfirmXoaFile" runat="server" Text="Xóa file">
                                    </telerik:RadButton>
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="padding: 5px; background-color: #F2F5FA; padding-left: 66px;">
                        <table border="0">
                            <tr>
                                <td>
                                    <telerik:RadButton ID="btnLuuBottom" runat="server" ValidationGroup="KT" Text="Lưu lại"
                                        Icon-PrimaryIconUrl="~/admincp/Content/Imgs/icon-16-save.png">
                                    </telerik:RadButton>
                                </td>
                                <td>
                                    <telerik:RadButton ID="btnHuyBotom" runat="server" Text="Quay lại" Icon-PrimaryIconUrl="~/admincp/Content/Imgs/back.png">
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <telerik:RadNotification ID="rThongBao" runat="server" Position="BottomLeft" TitleIcon="">
    </telerik:RadNotification>

        <telerik:RadWindow ID="RadWindow1" runat="server">
        </telerik:RadWindow>

</div>

</asp:Content>
