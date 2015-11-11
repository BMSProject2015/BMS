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
        String ParentID = "Edit";
        Dictionary<string, object> dicData = (Dictionary<string, object>)ViewData["TL_VanBan_dicdata"];
        NameValueCollection data = (NameValueCollection)dicData["data"];
        data.Remove("dNgayBanHanh");
        data.Add("dNgayBanHanh", Convert.ToString(data[ParentID + "_vidNgayBanHanh"]));
        data.Add(ParentID + "_dNgayBanHanh", Convert.ToString(data[ParentID + "_vidNgayBanHanh"]));
        data.Remove("dNgayHieuLuc");
        data.Add("dNgayHieuLuc", Convert.ToString(data[ParentID + "_vidNgayHieuLuc"]));
        data.Add(ParentID + "_dNgayHieuLuc", Convert.ToString(data[ParentID + "_vidNgayHieuLuc"]));
        data.Remove("dNgayHetHan");
        data.Add("dNgayHetHan", Convert.ToString(data[ParentID + "_vidNgayHetHan"]));
        data.Add(ParentID + "_dNgayHetHan", Convert.ToString(data[ParentID + "_vidNgayHetHan"]));
        data.Add("sFileName1", Convert.ToString(data["sFileName"]));
        if (data["sFileName1"] == null)
        {
            data.Remove("sFileName1");
            data.Add("sFileName1", Convert.ToString(data[ParentID + "_sFileName"]));
        }
        String MaTaiLieu = Convert.ToString(ViewData["iID_MaTaiLieu"]);

        DataTable dt;
        SqlCommand cmd;
        String sSoHieu = "", Ten = "", MaDonVi = "", URL = "", MaTaiLieuLienQuan = "", iDM_LoaiVanBan = Guid.Empty.ToString(), iDM_NoiBanHanh = Guid.Empty.ToString(), sNguoiKy = "", sFileName = "", sURL = "", sTuKhoa = "";
        String iID_KieuTaiLieu = Convert.ToString(ViewData["iID_MaKieuTaiLieu"]);
        if (String.IsNullOrEmpty(iID_KieuTaiLieu)) iID_KieuTaiLieu = Request.QueryString["MaKieuTaiLieu"];
        Int32 MaKieuTaiLieu = 0;
        if (!String.IsNullOrEmpty(iID_KieuTaiLieu))
            MaKieuTaiLieu = Convert.ToInt32(iID_KieuTaiLieu);
        sFileName = Convert.ToString(ViewData["sFileName"]);
        sURL = Convert.ToString(ViewData["url"]);

        if (String.IsNullOrEmpty(MaTaiLieu) == false)
        {
            cmd = new SqlCommand("SELECT * FROM TL_VanBan WHERE iID_MaTaiLieu=@iID_MaTaiLieu");
            cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                sSoHieu = Convert.ToString(dt.Rows[0]["sSoHieu"]);
                Ten = Convert.ToString(dt.Rows[0]["sTen"]);
                MaKieuTaiLieu = Convert.ToInt32(dt.Rows[0]["iID_MaKieuTaiLieu"]);
                URL = Convert.ToString(dt.Rows[0]["sURL"]);
                iDM_LoaiVanBan = Convert.ToString(dt.Rows[0]["iDM_LoaiVanBan"]);
                sNguoiKy = Convert.ToString(dt.Rows[0]["sNguoiKy"]);
                // sTuKhoa = Convert.ToString(dt.Rows[0]["sTuKhoa"]);
                iDM_NoiBanHanh = Convert.ToString(dt.Rows[0]["iDM_NoiBanHanh"]);
                if (String.IsNullOrEmpty(sFileName))
                     sFileName = Convert.ToString(dt.Rows[0]["sFileName"]);
                if (String.IsNullOrEmpty(sURL))
                     sURL = Convert.ToString(dt.Rows[0]["sURL"]);
                dt.Dispose();

            }
        }

        //dtLoaiVanban
        DataTable dtLoaiVanBan = CommonFunction.Lay_dtDanhMuc("LoaiVanBan");
        DataView dv = dtLoaiVanBan.DefaultView;
        dv.Sort = "sTen";
        dtLoaiVanBan = dv.ToTable();
        SelectOptionList slLoaiVanBan = new SelectOptionList(dtLoaiVanBan, "iID_MaDanhMuc", "sTen");
        dtLoaiVanBan.Dispose();

        //dtNoiBanHanh
        DataTable dtNoiBanHanh = CommonFunction.Lay_dtDanhMuc("TLLS_NoiBanHanh");
        dv = dtNoiBanHanh.DefaultView;
        dv.Sort = "sTen";
        dtNoiBanHanh = dv.ToTable();
        SelectOptionList slNoiBanHanh = new SelectOptionList(dtNoiBanHanh, "iID_MaDanhMuc", "sTen");
        dtNoiBanHanh.Dispose();
        string urlEdit = Url.Action("Edit", "TuLieu_VanBan", new { MaTaiLieu = MaTaiLieu, iID_MaKieuTaiLieu = MaKieuTaiLieu });
        string urlIndex = Url.Action("Index", "TuLieu_VanBan", new { MaKieuTaiLieu = MaKieuTaiLieu });
        String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, "TL_VanBan");
        string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(User.Identity.Name, "TL_VanBan");
        DataTable dtHieuLuc = new DataTable();
        dtHieuLuc.Columns.Add("sMa", typeof(String));
        dtHieuLuc.Columns.Add("sTen", typeof(String));

        DataRow dr;
        dr = dtHieuLuc.NewRow();
        dr[0] = "0";
        dr[1] = "--Chọn--";
        dtHieuLuc.Rows.Add(dr);
        dr = dtHieuLuc.NewRow();
        dr[0] = "1";
        dr[1] = "Còn hiệu lực";
        dtHieuLuc.Rows.Add(dr);

        dr = dtHieuLuc.NewRow();
        dr[0] = "2";
        dr[1] = "Hết hiệu lực";
        dtHieuLuc.Rows.Add(dr);

        dr = dtHieuLuc.NewRow();
        dr[0] = "3";
        dr[1] = "Còn hiệu lực một phần";
        dtHieuLuc.Rows.Add(dr);
        SelectOptionList slHieuLuc = new SelectOptionList(dtHieuLuc, "sMa", "sTen");
        using (Html.BeginForm("EditSubmit", "TuLieu_VanBan", new { ParentID = ParentID, MaTaiLieu = MaTaiLieu, MaKieuTaiLieu = MaKieuTaiLieu, path = path }, FormMethod.Post, new { enctype = "multipart/form-data" }))
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
                        <span>Nhập thông tin văn bản</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="100%">
                            <table width="60%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Số văn bản ")%><b style="color: Red">(*)</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextBox(ParentID, data, "sSoHieu", "", "style=\"width:100%;\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sSoHieu")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Trích yếu ")%><b style="color: Red">(*)</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextArea(ParentID, data, "sTen", "", "style=\"width:100%;\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Loại tài liệu")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slLoaiVanBan, data, "iDM_LoaiVanBan", "", "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Nơi ban hành")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slNoiBanHanh, data, "iDM_NoiBanHanh", "", "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Lĩnh vực")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=TuLieuLichSuModels.wrDanhMucTaiLieu_DDL(ParentID, MaKieuTaiLieu)%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTuLieu_DanhMuc")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Người ký ")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextBox(ParentID, data, "sNguoiKy", "", "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Từ khóa liên quan ")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextBox(ParentID, data, "sTuKhoaLQ", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày ban hành ")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DatePicker(ParentID, data, "dNgayBanHanh", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày hiệu lực ")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DatePicker(ParentID, data, "dNgayHieuLuc", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày hết hiệu lực ")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DatePicker(ParentID, data, "dNgayHetHan", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayHetHan")%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Hiệu lực:
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slHieuLuc, data, "iHieuLuc", "", "style=\"width:100%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Ghi chú:
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.TextArea(ParentID, data, "sGhiChu", "", "style=\"width:100%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 25%">
                                        <div>
                                            <%=NgonNgu.LayXau("File tài liệu ")%>
                                            <b style="color: Red">(*)</b></div>
                                    </td>
                                    <td class="td_form2_td5" style="width: 75%">
                                        <div>
                                            <%=MyHtmlHelper.Label(sFileName, "sFileName1", "")%>
                                            <%=MyHtmlHelper.Hidden(ParentID, sFileName, "sFileName","")%>
                                            <%=MyHtmlHelper.Hidden(ParentID, sURL, "sURL","")%>
                                        </div>
                                        <div>
                                            <%--<% =MyHtmlHelper.UploadFile("uploadFile", path, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"))%>--%>
                                            <input type="file" name="uploadFile" id="uploadFile" style="width: 95%" onchange="checkFileExtension();" /><br />
                                             <%-- <%=MyHtmlHelper.Hidden(ParentID, data, "sFileName","")%>
                                            <br />
                                            <%= MyHtmlHelper.TextBox(ParentID, data, "sURL", "", "readonly style=\"width:10px; display:none;\"")%>--%>
                                          <%--  <script type="text/javascript">
                                                uploadFile.addListener(uploadFile.UPLOAD_COMPLETE, uploadFile_OnComplete);
                                                function uploadFile_OnComplete(FileName, url) {
                                                    document.getElementById("<%=ParentID%>_sFileName").value = FileName;
                                                    document.getElementById("<%=ParentID%>_sURL").value = uploadFile.serverPath + "/" + url;
                                                }
                                            </script>--%>
                                             <%= Html.ValidationMessage(ParentID + "_" + "err_sFileName")%>
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
            //            if (document.getElementById("<%= ParentID%>_sFileName").value == "") {
            //                alert('<%=NgonNgu.LayXau("Không tìm thấy tài liệu cần upload!")%>');
            //                javascript: location.href = '<%=urlEdit%>';
            //                return false;
            //            }
            //            else {
            //                javascript: location.href = '<%=urlIndex%>';
            //                return true;
            //            }
        }
    </script>
    <%
        };
    %>
</asp:Content>
