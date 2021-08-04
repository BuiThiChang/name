<%@ Page Title="Cập nhật nhóm sản phẩm" Language="vb" AutoEventWireup="false" MasterPageFile="~/AdminCp/MasterAdminSite.Master"
    CodeBehind="AdminNhomSanPham_old.aspx.vb" Inherits="AdminBaoAn.AdminNhomSanPham_old" %>
    <%@ Register Src="../Uctrl/BAOAN_EDITTOR.ascx" TagName="BAOAN_EDITTOR" TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="TrangChu.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Content/AdminStyle.css" rel="stylesheet" type="text/css" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="AMDSLide" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="treeNhomSP">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="treeNhomSP" LoadingPanelID="loading" />
                    <telerik:AjaxUpdatedControl ControlID="rThongBao" />
                    <telerik:AjaxUpdatedControl ControlID="PanelThem" />
                    <telerik:AjaxUpdatedControl ControlID="btThem" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btThem">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btThem" />
                    <telerik:AjaxUpdatedControl ControlID="treeNhomSP" LoadingPanelID="loading" />
                    <telerik:AjaxUpdatedControl ControlID="PanelThem" LoadingPanelID="loading" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btLuuBottom">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="treeNhomSP" />
                    <telerik:AjaxUpdatedControl ControlID="rThongBao" />
                    <telerik:AjaxUpdatedControl ControlID="btThem" />
                    <telerik:AjaxUpdatedControl ControlID="PanelThem" LoadingPanelID="loading" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btHuyBottom">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="treeNhomSP" />
                    <telerik:AjaxUpdatedControl ControlID="rThongBao" />
                    <telerik:AjaxUpdatedControl ControlID="btThem" />
                    <telerik:AjaxUpdatedControl ControlID="PanelThem" LoadingPanelID="loading" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function fileUploaded(sender, args) {
                $find('<%=AMDSLide.ClientID %>').ajaxRequest();
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel ID="loading" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div id="divAlbum" runat="server" class="NoiDung">
        <div style="padding: 5px;">
            <telerik:RadButton ID="btThem" runat="server" Text="Thêm nhóm sản phẩm" Icon-PrimaryIconUrl="~/admincp/Content/imgs/icon-16-new.png">
            </telerik:RadButton>
        </div>
        <div id="PanelThem" runat="server" visible="false" style="background-color: #f5f5f5;
            border: 3px solid #fff; box-shadow: 0 1px 2px rgba(0,0,0,0.2); padding: 5px;
            margin-top: 5px; border-radius: 10px; margin: 15px;" >
            <asp:HiddenField ID="ID_HT" runat="server" />
            <table width="100%" cellpadding="2">
                <tr>
                    <td style="width: 150px;">
                        Tên nhóm (VIE)
                    </td>
                    <td>
                        <telerik:RadTextBox ID="tbTenNhomVN" runat="server" Width="400px">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbTenNhomVN"
                            ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="KT"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        Tên nhóm (ENG)
                    </td>
                    <td>
                        <telerik:RadTextBox ID="tbTenNhomEN" runat="server" Width="400px">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbTenNhomEN"
                            ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="KT"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        Nhóm cha
                    </td>
                    <td>
                        <telerik:RadComboBox ID="cmbNhomSpCha" runat="server" DataValueField="ID" Width="400px"
                            AllowCustomText="true" Filter="Contains" DataTextField="TenNhom_VN" AppendDataBoundItems="True">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        Từ khóa
                    </td>
                    <td>
                        <telerik:RadTextBox ID="tbTuKhoa" runat="server" Width="400px">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbTuKhoa"
                            ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="KT"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        Mô tả
                    </td>
                    <td>
                        <telerik:RadTextBox ID="tbMoTa" runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        Hãng nổi bật
                    </td>
                    <td>
                        <asp:SqlDataSource ID="sqlData" runat="server" ConnectionString="<%$ ConnectionStrings:strChuoiKetNoi %>"
                            ProviderName="System.Data.SqlClient" SelectCommand="SELECT MaHang, TenHang FROM HANGSANXUAT WHERE (TrangThai = 1) ORDER BY TenHang">
                        </asp:SqlDataSource>
                        <telerik:RadAutoCompleteBox ID="tbHangNoiBat" runat="server" Width="400px" DataSourceID="sqlData"
                            DropDownWidth="400px" InputType="Token" DataTextField="TenHang" DataValueField="MaHang">
                            <TokensSettings AllowTokenEditing="true" />
                        </telerik:RadAutoCompleteBox>
                    </td>
                </tr>
                <tr>
                <td>
                    Thông tin bổ xung
                </td>
                <td>
                     <div style="padding: 5px; background-color: #F2F5FA;">
                <telerik:RadTabStrip Width="700px" ID="rTabNoiDung" runat="server" SelectedIndex="0" MultiPageID="rMultiPage">
                    <Tabs>
                        <telerik:RadTab ImageUrl="~/images/vn.png" Text="Tiếng Việt" Font-Names="Roboto Condensed"
                            Font-Size="14px" Font-Bold="false" PageViewID="RadPageView1" Selected="true">
                        </telerik:RadTab>
                        <telerik:RadTab ImageUrl="~/images/uk.png" Text="Tiếng Anh" Font-Names="Roboto Condensed"
                            Font-Size="14px" Font-Bold="false" PageViewID="RadPageView2">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="rMultiPage" runat="server" SelectedIndex="0">
                    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                        <uc1:BAOAN_EDITTOR ID="txtTiengViet" Rong="700px" Cao="150px" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView2" runat="server">
                        <uc1:BAOAN_EDITTOR ID="txtTiengAnh" Rong="700px" Cao="150px" runat="server" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
                </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                        STT
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="tbSTT" runat="server" ShowSpinButtons="true" MinValue="0"
                            NumberFormat-DecimalDigits="0">
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHienThi" Text="Hiển thị" runat="server" Checked="true" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="padding: 5px; background-color: #F2F5FA;">
                <telerik:RadButton ID="btLuuBottom" runat="server" ValidationGroup="KT" Text="Lưu lại"
                    Icon-PrimaryIconUrl="~/admincp/Content/imgs/icon-16-save.png">
                </telerik:RadButton>
                <telerik:RadButton ID="btHuyBottom" runat="server" Text="Hủy bỏ" Icon-PrimaryIconUrl="~/admincp/Content/imgs/icon-16-cancel.png">
                </telerik:RadButton>
            </div>
        </div>
        <div style="padding: 5px;">
            <telerik:RadTreeList ID="treeNhomSP" runat="server" DataKeyNames="Id" ParentDataKeyNames="IdCha"
                ItemStyle-Height="30" ItemStyle-VerticalAlign="Middle" AutoGenerateColumns="False">
                <ClientSettings>
                    <Selecting AllowItemSelection="True" />
                </ClientSettings>
                <ExportSettings>
                    <Excel PageLeftMargin="0.7in" PageRightMargin="0.7in">
                    </Excel>
                    <Pdf PageWidth="8.5in" PageHeight="11in">
                    </Pdf>
                </ExportSettings>
                <EditFormSettings EditFormType="Template">
                </EditFormSettings>
                <ValidationSettings CommandsToValidate="PerformInsert,Update"></ValidationSettings>
                <Columns>
                    <telerik:TreeListBoundColumn DataField="IdCha" Visible="false">
                    </telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="TenNhom_VN" HeaderText="Tên nhóm (VN)">
                    </telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="TenNhom_EN" HeaderText="Tên nhóm (EN)">
                    </telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="Hang" HeaderText="Hãng nổi bật">
                    </telerik:TreeListBoundColumn>
                    <telerik:TreeListBoundColumn DataField="SoTT" HeaderText="STT">
                        <HeaderStyle Width="40px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:TreeListBoundColumn>
                    <telerik:TreeListButtonColumn Text="Sửa" HeaderStyle-HorizontalAlign="Center" ButtonType="ImageButton"
                        CommandName="EditNhomSP" HeaderText="Sửa" ImageUrl="~/admincp/Content/Imgs/icon-16-Edit.png">
                        <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:TreeListButtonColumn>
                    <telerik:TreeListButtonColumn Text="Xóa" ConfirmText="Xóa ?" HeaderStyle-HorizontalAlign="Center"
                        ConfirmTitle="Chú ý" ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px"
                        HeaderText="Xóa" ConfirmDialogType="RadWindow" ImageUrl="~/admincp/Content/Imgs/icon-16-delete.png">
                        <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:TreeListButtonColumn>
                    <telerik:TreeListBoundColumn DataField="Id" HeaderText="Id">
                        <HeaderStyle Width="30px" />
                    </telerik:TreeListBoundColumn>
                </Columns>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            </telerik:RadTreeList>
        </div>
    </div>
    <telerik:RadNotification ID="rThongBao" runat="server" Position="BottomLeft" TitleIcon="">
    </telerik:RadNotification>
</asp:Content>
