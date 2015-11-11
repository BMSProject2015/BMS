<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_TuLieu_Default.Master" Inherits="System.Web.Mvc.ViewPage" %>

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
        Dictionary<string, object> dicData = (Dictionary<string, object>)ViewData["TL_VanBan_dicdata"];
        NameValueCollection data = (NameValueCollection)dicData["data"];
        data.Remove("dNgayBanHanh");
        data.Add("dNgayBanHanh", Convert.ToString(data[ParentID + "_vidNgayBanHanh"]));
        data.Add(ParentID + "_dNgayBanHanh", Convert.ToString(data[ParentID + "_vidNgayBanHanh"]));
        data.Add("sFileName1", Convert.ToString(data["sFileName"]));
        if (data["sFileName1"] == null)
        {
            data.Remove("sFileName1");
            data.Add("sFileName1", Convert.ToString(data[ParentID + "_sFileName"]));
        }
        DataTable dt;
        SqlCommand cmd;
        String sSoHieu = "", Ten = "", MaDonVi = "", URL = "", MaTaiLieuLienQuan = "", iDM_LoaiVanBan = Guid.Empty.ToString(), iDM_NoiBanHanh = Guid.Empty.ToString(), sNguoiKy = "", sFileName = "", sURL = "", sNguoiLap = "", sHieuLuc = "" ;
        String iID_KieuTaiLieu = Convert.ToString(ViewData["iID_MaKieuTaiLieu"]);
        if (String.IsNullOrEmpty(iID_KieuTaiLieu)) iID_KieuTaiLieu = Request.QueryString["MaKieuTaiLieu"];
        Int32 MaKieuTaiLieu = 0;
        if (!String.IsNullOrEmpty(iID_KieuTaiLieu))
            MaKieuTaiLieu = Convert.ToInt32(iID_KieuTaiLieu);
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
                iDM_NoiBanHanh = Convert.ToString(dt.Rows[0]["iDM_NoiBanHanh"]);
                sFileName = Convert.ToString(dt.Rows[0]["sFileName"]);
                sURL = Convert.ToString(dt.Rows[0]["sURL"]);
                sNguoiLap = HamChung.LayTenTaiKhoan(Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]));
                
                String dNgayHetHan = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["dNgayHetHan"]);
                sHieuLuc = "Còn hiệu lực";
                if (!String.IsNullOrEmpty(dNgayHetHan))
                {
                    int KQ = DateTime.Compare(Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayHetHan)), Convert.ToDateTime(DateTime.Today));
                    if (KQ < 0)
                        sHieuLuc = "Hết hiệu lực";
                }
                dt.Dispose();

            }
        }

        //dtLoaiVanban
        DataTable dtLoaiVanBan = CommonFunction.Lay_dtDanhMuc("LoaiVanBan");
        SelectOptionList slLoaiVanBan = new SelectOptionList(dtLoaiVanBan, "iID_MaDanhMuc", "sTen");
        dtLoaiVanBan.Dispose();

        //dtNoiBanHanh
        DataTable dtNoiBanHanh = CommonFunction.Lay_dtDanhMuc("TLLS_NoiBanHanh");
        SelectOptionList slNoiBanHanh = new SelectOptionList(dtNoiBanHanh, "iID_MaDanhMuc", "sTen");
        dtNoiBanHanh.Dispose();
        string urlEdit = Url.Action("Edit", "TuLieu_VanBan", new { MaTaiLieu = MaTaiLieu, iID_MaKieuTaiLieu = MaKieuTaiLieu });
        string urlIndex = Url.Action("Index", "TuLieu_VanBan", new { MaKieuTaiLieu = MaKieuTaiLieu });
        String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, "TL_VanBan");
        string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(User.Identity.Name, "TL_VanBan");
        String strTaiVe = Url.Action("Download", "TuLieu_VanBan", new { MaTaiLieu = MaTaiLieu });
        using (Html.BeginForm("EditSubmit", "TuLieu_VanBan", new { ParentID = ParentID, MaTaiLieu = MaTaiLieu, MaKieuTaiLieu = MaKieuTaiLieu }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiLieu", MaTaiLieu)%>
 
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
                        <span>Thông tin chi tiết văn bản</span>
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
                                            <%=NgonNgu.LayXau("Số văn bản")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.Label(data, "sSoHieu", "", "style=\"width:100%;\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sSoHieu")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Trích yếu")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.Label(data, "sTen", "", "style=\"width:100%;\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Loại tài liệu")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slLoaiVanBan, data, "iDM_LoaiVanBan", sDanhSachTruongCam, "style=\"width:100%;\" disabled=\"disabled\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Nơi ban hành")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slNoiBanHanh, data, "iDM_NoiBanHanh", sDanhSachTruongCam, "style=\"width:100%;\" disabled=\"disabled\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Lĩnh vực")%></div>
                                    </td>
                                    <td class="td_form2_td2">
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
                                            <%=NgonNgu.LayXau("Người ký")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.Label(data, "sNguoiKy", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Từ khóa liên quan ")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.Label(data, "sTuKhoaLQ", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày ban hành ")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.Label( data, "dNgayBanHanh", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày hiệu lực ")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.Label( data, "dNgayHieuLuc", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Ngày hết hiệu lực ")%></div>
                                    </td>
                                    <td class="td_form2_td2">
                                        <div>
                                            <%= MyHtmlHelper.Label( data, "dNgayHetHan", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayHetHan")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 25%">
                                        <div>
                                            <%=NgonNgu.LayXau("File tài liệu")%></div>
                                    </td>
                                    <td class="td_form2_td2" style="width: 75%">
                                        <div id="sTenFile123">
                                            <%=sFileName%>
                                        </div>
                                        <%--  <div>
                                            <% =MyHtmlHelper.UploadFile("uploadFile", path, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"))%>
                                           
                                          <%=MyHtmlHelper.Hidden(ParentID, sFileName, "sFileName","")%>
                                            <br />
                                            <%= MyHtmlHelper.TextBox(ParentID, sURL, "sURL", "", "readonly style=\"width:10px; display:none;\"")%>
                                            <script type="text/javascript">
                                                uploadFile.addListener(uploadFile.UPLOAD_COMPLETE, uploadFile_OnComplete);
                                                function uploadFile_OnComplete(FileName, url) {
                                                    document.getElementById("sTenFile123").innerHTML = "";
                                                    document.getElementById("<%=ParentID%>_sFileName").value = FileName;
                                                    document.getElementById("<%=ParentID%>_sURL").value = uploadFile.serverPath + "/" + url;
                                                }
                                            </script>
                                        </div>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Người lập: ")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.Label( sNguoiLap, "", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
                                        </div>
                                    </td>
                                </tr>
                                  </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <%=NgonNgu.LayXau("Hiệu lực: ")%></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.Label( sHieuLuc, "", sDanhSachTruongCam, "style=\"width:100%;\"")%><br />
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
                              <input type="button" class="button" value="Tải văn bản" onclick="javascript:location.href='<%=strTaiVe %>'" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
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
                javascript: location.href = '<%=urlEdit%>';
                return false;
            }
            else {
                javascript: location.href = '<%=urlIndex%>';
                return true;
            }
        }
    </script>
    <%
        };
    %>
</asp:Content>
