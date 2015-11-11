<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.IO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string path = string.Empty;
        string sTenKhoa = "TMVD";
        path = TuLieuLichSuModels.ThuMucLuu(sTenKhoa);
        String MaTaiLieu = Convert.ToString(ViewData["iID_MaTaiLieu"]);
        String ParentID = "Edit";
        DataTable dt;
        SqlCommand cmd;
        String Ten = "", MaDonVi = "", URL = "", MaTaiLieuLienQuan = "";
        String iID_KieuTaiLieu = Convert.ToString(ViewData["iID_MaKieuTaiLieu"]);
        Int32 MaKieuTaiLieu = 0;
        if (!String.IsNullOrEmpty(iID_KieuTaiLieu))
            MaKieuTaiLieu = Convert.ToInt32(iID_KieuTaiLieu);
        if (String.IsNullOrEmpty(MaTaiLieu) == false)
        {
            cmd = new SqlCommand("SELECT * FROM TL_Video WHERE iID_MaTaiLieu_Video=@iID_MaTaiLieu");
            cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                Ten = Convert.ToString(dt.Rows[0]["sTen"]);
                MaDonVi = Convert.ToString(dt.Rows[0]["sMaDonVi"]);
                MaKieuTaiLieu = Convert.ToInt32(dt.Rows[0]["iID_MaKieuTaiLieu"]);
                URL = Convert.ToString(dt.Rows[0]["sURL"]);
                dt.Dispose();
            }
        }
        string urlEdit = Url.Action("Edit", "TuLieu_DanhMuc", new { MaTaiLieu = MaTaiLieu });
        using (Html.BeginForm("EditSubmit", "TuLieu_Video", new { ParentID = ParentID, MaTaiLieu = MaTaiLieu, path = path }, FormMethod.Post, new { enctype = "multipart/form-data" }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiLieu_Video", MaTaiLieu)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TuLieu_DanhMuc"), "Danh mục tài liệu")%>
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "ChangeDirectoryV2", new { contronllerName = "TuLieu_Video" }), "Thay đổi thu mục lưu file")%>
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function checkDeleteAll(aspnetForm) {
            if (aspnetForm.Edit_chb_DeleteAll.checked == true) {
                for (var i = 0; i < aspnetForm.elements.length; i++) {
                    if ((aspnetForm.elements[i].type == "checkbox") && (aspnetForm.elements[i].name == "Edit_sMaDonVi")) {
                        aspnetForm.elements[i].checked = true;
                    }
                }
            }
            else {
                for (var i = 0; i < aspnetForm.elements.length; i++) {
                    if ((aspnetForm.elements[i].type == "checkbox") && (aspnetForm.elements[i].name == "Edit_sMaDonVi")) {
                        aspnetForm.elements[i].checked = false;
                    }
                }
            }
        }
        function checkAllRelatedVideos(aspnetForm) {
            if (aspnetForm.Edit_chb_RelatedVideos.checked == true) {
                for (var i = 0; i < aspnetForm.elements.length; i++) {
                    if ((aspnetForm.elements[i].type == "checkbox") && (aspnetForm.elements[i].name == "Edit_sMaTaiLieuLienQuan")) {
                        aspnetForm.elements[i].checked = true;
                    }
                }
            }
            else {
                for (var i = 0; i < aspnetForm.elements.length; i++) {
                    if ((aspnetForm.elements[i].type == "checkbox") && (aspnetForm.elements[i].name == "Edit_sMaTaiLieuLienQuan")) {
                        aspnetForm.elements[i].checked = false;
                    }
                }
            }
        }

        function checkFileExtension() {
            var filePath = document.getElementById("uploadFile").value;
            if (filePath.indexOf('.') == -1)
                return false;
            var validExtensions = new Array();
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
            validExtensions[0] = '3gp';
            validExtensions[1] = 'avi';
            validExtensions[2] = 'flv';
            validExtensions[3] = 'mp4';
            validExtensions[4] = 'mpg';
            validExtensions[5] = 'mpeg';
            for (var i = 0; i < validExtensions.length; i++) {
                if (ext == validExtensions[i])
                    return true;
            }
            document.getElementById("uploadFile").value = "";
            alert('File dữ liệu video không phù hợp!');
            return false;
        }
    </script>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Nhập thông tin tư liệu</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="50%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                <%
                                    if (String.IsNullOrEmpty(MaTaiLieu))
                                    {
                                %>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("File tư liệu")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%--<% =MyHtmlHelper.UploadFile("uploadFile", "Libraries/Videos", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"))%>--%>
                                            <input type="file" name="uploadFile" id="uploadFile" style="width: 95%" accept="video/*"
                                                onchange="checkFileExtension();" /><br />
                                            <%= Html.Hidden(ParentID + "_sFileName")%>
                                            <br />
                                            <%= MyHtmlHelper.TextBox(ParentID, Url, "sURL", "", "readonly style=\"width:10px; display:none;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <%
                                    }%>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Tên tư liệu")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextBox(ParentID, Ten, "sTen", "", "style=\"width:95%;\"")%><br />
                                            <%= MyHtmlHelper.Hidden(ParentID, Ten, "iID_MaTrangThaiDuyet", "")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Tư liệu liên quan") %></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <table>
                                                <tr>
                                                    <td style="width: 2%" align="left">
                                                        <%= MyHtmlHelper.CheckBox(ParentID, "", "chb_RelatedVideos", "", "onclick=\"JavaScript:checkAllRelatedVideos(this.form);\"")%>
                                                    </td>
                                                    <td align="left">
                                                        Chọn tất cả
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="overflow: auto;max-height: 400px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                
                                                <tr>
                                                    <td colspan="2" style="height: 5px; font-size: 2px;">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <%
                                                    cmd = new SqlCommand("SELECT iID_MaTaiLieu_Video, sTen FROM TL_Video  where iTrangThai=1 ORDER BY sTen");
                                                dt = Connection.GetDataTable(cmd);
                                                String chk;
                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                {
                                                    chk = "";
                                                    if (MaTaiLieuLienQuan.IndexOf(String.Format(",{0},", dt.Rows[i]["iID_MaTaiLieu_Video"])) >= 0)
                                                    {
                                                        chk = "on";
                                                    }
                                                %>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <%= MyHtmlHelper.CheckBox(ParentID, chk, "sMaTaiLieuLienQuan", "", String.Format("value='{0}'", Convert.ToString(dt.Rows[i]["iID_MaTaiLieu_Video"])))%>
                                                    &nbsp;&nbsp;
                                                        <%= dt.Rows[i]["sTen"]%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 5px; font-size: 2px;">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <%
                                                }
                                                dt.Dispose();
                                                %>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Danh mục tài liệu") %></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=TuLieuLichSuModels.wrDanhMucTaiLieu_DDL(ParentID, MaKieuTaiLieu)%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaKieuTaiLieu")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Chia sẻ cho đơn vị") %></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <table>
                                                  <tr>
                                                    <td style="width: 2%" align="left">
                                                        <%= MyHtmlHelper.CheckBox(ParentID, "", "chb_DeleteAll", "", "onclick=\"JavaScript:checkDeleteAll(this.form);\"")%>
                                                    </td>
                                                    <td align="left">
                                                        Chọn tất cả
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="overflow: auto;max-height: 400px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                              
                                                <tr>
                                                    <td colspan="2" style="height: 5px; font-size: 2px;">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <%
                                                cmd = new SqlCommand("SELECT iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ORDER BY iID_MaDonVi");
                                                cmd.Parameters.AddWithValue("@iNamLamViec",NguoiDungCauHinhModels.iNamLamViec);
                                                dt = Connection.GetDataTable(cmd);
                                                String tg;
                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                {
                                                    tg = "";
                                                    if (MaDonVi.IndexOf(String.Format(",{0},", dt.Rows[i]["iID_MaDonVi"])) >= 0)
                                                    {
                                                        tg = "on";
                                                    }
                                                %>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <%= MyHtmlHelper.CheckBox(ParentID, tg, "sMaDonVi", "", String.Format("value='{0}'", Convert.ToString(dt.Rows[i]["iID_MaDonVi"])))%>
                                                    &nbsp;&nbsp;
                                                        <%= dt.Rows[i]["sTen"]%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 5px; font-size: 2px;">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <%
                                                }
                                                dt.Dispose();
                                                %>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button4" value="Lưu" onclick="return checkChoise();" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function checkChoise() {
            if ('<%=String.IsNullOrEmpty(MaTaiLieu) %>' == "False") {
                return true;
            }
            if (document.getElementById("uploadFile").value == "") {
                alert('<%=NgonNgu.LayXau("Không tìm thấy file tư liệu cần upload!")%>');
                return false;
            }
            if (checkFileExtension() == false) return false;
            else {
                javascript: location.href = '<%=urlEdit%>';
                return true;
            }
        }
    </script>
    <%
        };
    %>
</asp:Content>
