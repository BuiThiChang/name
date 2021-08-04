<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admincp/MasterAdminSite.Master" CodeBehind="AdminSetColorPage.aspx.vb" Inherits="AdminBaoAn.AdminSetColorPage" %>
    <%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="TrangChu.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
             <div id="PanelColor" runat="server" visible="false" style="background-color: #f5f5f5; border: 3px solid #fff;
            box-shadow: 0 1px 2px rgba(0,0,0,0.2); padding: 5px; margin-top: 5px; border-radius: 10px;margin:15px;width:650px;margin-left:auto;margin-right:auto;">
             <div style=" margin-top: 20px; text-align:center">
                   <p style=" font-size:32px; color: #536ec3; width: 100%; font-weight: 700; text-align:center; margin: 50px 0 20px 0;">Chỉnh sửa màu trang Web</p>
                   <div style=" width: 500px; ">
                       <div style="text-align:left; margin-left:30%; width: 400px">
                        <asp:Label CssClass="maMau"  Text="Mã màu:"  runat="server" />
                        <input type="color" name="color1" id ="color1" /> <span style="margin-left:10px;" id="res_color1"></span>
                            <br/> <br/>  <br/> 

                            <asp:Label CssClass="lbl_Khung"  Text="Nền chính:"  runat="server" />
                            <asp:TextBox ID="txtNenChinh" runat="server"></asp:TextBox><br/> <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Icon TopMXH:"  runat="server" />
                            <asp:TextBox ID="txtTopMXH" runat="server"></asp:TextBox><br/> <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Hover icon TopMXH :"  runat="server" />
                             <asp:TextBox ID="txtTopMXH_hover" runat="server"></asp:TextBox><br/> <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Chữ Top_Giới thiệu:"  runat="server" />
                            <asp:TextBox  ID="txtChu_Top_GioiThieu" runat="server"></asp:TextBox>   <br/> <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Nền Top_Giới thiệu:"  runat="server" />
                            <asp:TextBox  ID="txtNen_Top_GioiThieu" runat="server"></asp:TextBox>   <br/> <br />
                             
                            <asp:Label CssClass="lbl_Khung"  Text="Hover Chữ Top_Giới thiệu:"  runat="server" />
                            <asp:TextBox  ID="txtChu_Top_GioiThieu_Hover" runat="server"></asp:TextBox>   <br/> <br />
                          
                           <asp:Label CssClass="lbl_Khung"  Text="Hover Nền Top_Giới thiệu:"  runat="server" />
                            <asp:TextBox  ID="txtNen_Top_GioiThieu_Hover" runat="server"></asp:TextBox>   <br/> <br />

                           <asp:Label CssClass="lbl_Khung"  Text="Click_Giới thiệu:"  runat="server" />
                             <asp:TextBox ID="txtClick_GioiThieu" runat="server"></asp:TextBox><br/>  <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Chữ_Trang chủ:"  runat="server" />
                            <asp:TextBox  ID="txtChu_TrangChu" runat="server"></asp:TextBox>   <br/> <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Nền_Trang chủ:"  runat="server" />
                            <asp:TextBox  ID="txtNen_TrangChu" runat="server"></asp:TextBox>   <br/> <br />
                             
                            <asp:Label CssClass="lbl_Khung"  Text="Hover chữ_Trang chủ:"  runat="server" />
                            <asp:TextBox  ID="txtChu_TrangChu_Hover" runat="server"></asp:TextBox>   <br/> <br />

                           <asp:Label CssClass="lbl_Khung"  Text="Hover Nền_Trang chủ:"  runat="server" />
                             <asp:TextBox ID="txtNen_TrangChu_Hover" runat="server"></asp:TextBox><br/>  <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Bottom SP_Đang đc QT:"  runat="server" />
                            <asp:TextBox  ID="txtbottom_DangDuocQuanTam" runat="server"></asp:TextBox>   <br /> <br />

                            <asp:Label CssClass="lbl_Khung"  Text="Bottom_Dự án mới:"  runat="server" />
                            <asp:TextBox  ID="txtbottom_DuAnMoi" runat="server"></asp:TextBox>   <br/><br/>
                           
                           
                           <asp:Button ID="buttonGetColor" CssClass="btn_KhungGetColor"  Text="LẤY MÀU" Runat="server" />&nbsp
                            <asp:Button ID="btnSaveColor" CssClass="btn_KhungSaveColor" Text="LƯU" Runat="server" /> 
                       </div>
                    </div>
             </div>
            <br />
        </div>

     <%--  tạo mã màu khi click vào ô chọn màu /////--%>
    <script type="text/javascript">
        var color1= document.getElementById("color1");
        var rescolor1= document.getElementById("res_color1");
          color1.addEventListener("input", function() {
            res_color1.innerHTML = color1.value;
          }, false); 
    </script>
</asp:Content>
