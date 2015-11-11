<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string path = string.Empty;
        string sTenKhoa = "TMTL";
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
            cmd = new SqlCommand("SELECT * FROM TL_TaiLieu WHERE iID_MaTaiLieu=@iID_MaTaiLieu");
            cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                Ten = Convert.ToString(dt.Rows[0]["sTen"]);
                MaDonVi = Convert.ToString(dt.Rows[0]["sMaDonVi"]);
                MaTaiLieuLienQuan = Convert.ToString(dt.Rows[0]["sMaTaiLieuLienQuan"]);
                MaKieuTaiLieu = Convert.ToInt32(dt.Rows[0]["iID_MaKieuTaiLieu"]);
                URL = Convert.ToString(dt.Rows[0]["sURL"]);
                dt.Dispose();
            }
        }

        string urlEdit = Url.Action("Edit", "TuLieu_DanhMuc", new { MaTaiLieu = MaTaiLieu });

        using (Html.BeginForm("EditSubmit", "TuLieu_TaiLieu", new { ParentID = ParentID, MaTaiLieu = MaTaiLieu }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiLieu", MaTaiLieu)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "ChangeDirectoryV2", new {  contronllerName = "TuLieu_TaiLieu" }), "Thay đổi thu mục lưu file")%>
                </div>
                <%--<div style="float: right; margin-right: 5px; margin-top: 3px;" onclick="OnInit_CT(1000, 'Thay đổi thư mục lưu file');">
                    <%= Ajax.ActionLink("Thay đổi thu mục lưu file", "Index", "NhapNhanh", new { id = "TL_CDIR", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT",  }, new AjaxOptions { }, new { @class = "button_title" })%>
                </div>--%>
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
        function checkAllRelatedDocument(aspnetForm) {
            if (aspnetForm.Edit_chb_RelatedDocument.checked == true) {
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
    
    </script>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Nhập thông tin tài liệu</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="80%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                <%
                                    if (String.IsNullOrEmpty(MaTaiLieu))
                                    {
                                %>
                                <tr>
                                    <td class="td_form2_td1"  style="width: 25%">
                                        <div>
                                            <%=NgonNgu.LayXau("File tài liệu")%></div>
                                    </td>
                                    <td class="td_form2_td5"  style="width: 75%">
                                        <div>
                                            <% =MyHtmlHelper.UploadFile("uploadFile", path, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"))%>
                                            <%= Html.Hidden(ParentID + "_sFileName")%>
                                            <br />
                                            <%= MyHtmlHelper.TextBox(ParentID, Url, "sURL", "", "readonly style=\"width:10px; display:none;\"")%>
                                            <script type="text/javascript">
                                                uploadFile.addListener(uploadFile.UPLOAD_COMPLETE, uploadFile_OnComplete);
                                                function uploadFile_OnComplete(FileName, url) {
                                                    document.getElementById("<%=ParentID%>_sFileName").value = FileName;
                                                    document.getElementById("<%=ParentID%>_sURL").value = uploadFile.serverPath + "/" + url;
                                                }
                                            </script>
                                        </div>
                                    </td>
                                </tr>
                                <%
                                    }%>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Tên tài liệu") %></div>
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
                                            <%=NgonNgu.LayXau("Tài liệu liên quan") %></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <table>
                                                <tr>
                                                    <td style="width: 2%" align="left">
                                                        <%= MyHtmlHelper.CheckBox(ParentID, "", "chb_RelatedDocument", "", "onclick=\"JavaScript:checkAllRelatedDocument(this.form);\"")%>
                                                    </td>
                                                    <td align="left">
                                                        Chọn tất cả
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="overflow: auto; max-height: 400px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                
                                                <tr>
                                                    <td colspan="2" style="height: 5px; font-size: 2px;">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <%
                                                    cmd = new SqlCommand("SELECT iID_MaTaiLieu, sTen FROM TL_TaiLieu where iTrangThai=1 ORDER BY sTen");
                                                dt = Connection.GetDataTable(cmd);
                                                String chk;
                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                {
                                                    chk = "";
                                                    if (MaTaiLieuLienQuan.IndexOf(String.Format(",{0},", dt.Rows[i]["iID_MaTaiLieu"])) >= 0)
                                                    {
                                                        chk = "on";
                                                    }
                                                %>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <%= MyHtmlHelper.CheckBox(ParentID, chk, "sMaTaiLieuLienQuan", "", String.Format("value='{0}'", Convert.ToString(dt.Rows[i]["iID_MaTaiLieu"])))%>
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
                                        </div>
                                        <div>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTuLieu_DanhMuc")%>
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
                                        <div style="overflow: auto; max-height: 400px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">                                               
                                                <tr>
                                                    <td colspan="2" style="height: 5px; font-size: 2px;">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <%
cmd = new SqlCommand("SELECT iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ORDER BY iID_MaDonVi");
cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
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
            if (document.getElementById("<%= ParentID%>_sFileName").value == "") {
                alert('<%=NgonNgu.LayXau("Không tìm thấy tài liệu cần upload!")%>');
                return false;
            }
            else {
                javascript: location.href = '<%=urlEdit%>';
                return true;
            }
        }
    </script>
    <%
        };
    %>
    <script type="text/javascript">

        function location_reload() {
            location.reload();
        }

        function CallSuccess_CT() {
            Bang_ShowCloseDialog();
            location_reload();
            return false;
        }

        function OnInit_CT(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizeable: false,
                draggable: true,
                width: value,
                modal: true
            });
        }

        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }

        $(document).ready(function () {


        });
    </script>
    <div id="idDialog" style="display: none;">
    </div>
    <div id="dvText" class="popup_block">
        <img src="../../KeToanChiTiet/../../Content/ajax-loader.gif" /><br />
        <p>
            Hệ thống đang thực hiện yêu cầu...</p>
    </div>
</asp:Content>
