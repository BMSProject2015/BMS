<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%
    if (Request.QueryString["Saved"] == "1")
    {
        %>
        <script type="text/javascript">
            $(document).ready(function () {
                parent.jsLuong_Dialog_Close(true);
            });                                 
        </script>
        <%
    }
    else
    {
        %>
        <script src="<%= Url.Content("~/Scripts/Luong/jsBang_Luong_CanBo.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
        <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
        <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
        <%
            String ParentID = "Parent";
            String iID_MaBangLuong = Request.QueryString["iID_MaBangLuong"];
            String iID_MaBangLuongChiTiet = Request.QueryString["iID_MaBangLuongChiTiet"];
            Boolean okFormThemMoi = false;
            if (String.IsNullOrEmpty(iID_MaBangLuongChiTiet))
            {
                okFormThemMoi = true;
                iID_MaBangLuongChiTiet = Guid.Empty.ToString();
            }    

            String rLuongToiThieu = "";
            
            String sPhuCap_ChucVu_CongThuc = Luong_DanhMucPhuCapModels.Get_CongThuc("rPhuCap_ChucVu");

            Luong_CanBo_BangDuLieu bang = new Luong_CanBo_BangDuLieu(iID_MaBangLuong, iID_MaBangLuongChiTiet, User.Identity.Name, Request.UserHostAddress);

            NameValueCollection data = (NameValueCollection)ViewData["data"];
            if (data == null)
            {
                if (iID_MaBangLuongChiTiet == Guid.Empty.ToString())
                {
                    data = new NameValueCollection();
                }
                else
                {
                    data = LuongModels.LayThongTinBangLuongChiTiet(iID_MaBangLuongChiTiet);       
                }
            }
            DataTable dtBangLuongChiTiet = LuongModels.Get_dtBangLuongChiTiet(iID_MaBangLuongChiTiet, null, -1);

            //<- Đồng bộ trường và trường có phần trước là Parent
            for (int i = 1; i < dtBangLuongChiTiet.Columns.IndexOf("iSTT"); i++)
            {
                if (data[ParentID + "_" + dtBangLuongChiTiet.Columns[i].ColumnName] != null)
                {
                    data[dtBangLuongChiTiet.Columns[i].ColumnName] = data[ParentID + "_" + dtBangLuongChiTiet.Columns[i].ColumnName];
                }
                if (data[dtBangLuongChiTiet.Columns[i].ColumnName] != null)
                {
                    data[ParentID + "_" + dtBangLuongChiTiet.Columns[i].ColumnName] = data[dtBangLuongChiTiet.Columns[i].ColumnName];
                }
            }
            // Đồng bộ trường và trường có phần trước là Parent ->

            string strPhaiLoadLai = "";
            if (Convert.ToString(ViewData["PhaiLoadLai"]) == "1")
            {
                strPhaiLoadLai = "true";
            }
            

            //<- Gán các giá trị thêm
            if (String.IsNullOrEmpty(data["sHieuTangGiam"]))
            {
                data["sHieuTangGiam"] = "S";
            }
            if (String.IsNullOrEmpty(data["rLuongToiThieu"]))
            {
                rLuongToiThieu = Luong_DanhMucThamSoModels.Get_ThamSo("rLuongToiThieu");
                if (rLuongToiThieu == "") rLuongToiThieu = "0";
                data["rLuongToiThieu"] = rLuongToiThieu;
            }
            else
            {
                rLuongToiThieu = data["rLuongToiThieu"];
            }
            if (String.IsNullOrEmpty(data["iTrichLuong_Loai"]))
            {
                data["iTrichLuong_Loai"] = "2";
            }
            if (String.IsNullOrEmpty(data["iSoNguoiPhuThuoc_CanBo"]))
            {
                data["iSoNguoiPhuThuoc_CanBo"] = "2";
            }
            if (String.IsNullOrEmpty(data["sHieuTangGiam"]) == false)
            {
                data["iID_MaLyDoTangGiam"] = data["sKyHieu_MucLucQuanSo_HieuTangGiam"];
                data["sTenLyDoTangGiam"] = data["sHieuTangGiam"];
                if (String.IsNullOrEmpty(data["sKyHieu_MucLucQuanSo_HieuTangGiam"]) == false)
                {
                    data["sTenLyDoTangGiam"] += " - " + data["sKyHieu_MucLucQuanSo_HieuTangGiam"];
                }
            }
            data["sHoVaTen_CanBo"] += String.Format("{0} {1}", data["sHoDem_CanBo"], data["sTen_CanBo"]).Trim();
            if (String.IsNullOrEmpty(data["iID_MaLyDoTangGiam"]) == false)
            {
                data["LyDoTangGiam"] = MucLucQuanSoModels.Get_Mota(data["iID_MaLyDoTangGiam"]);
            }
            if (String.IsNullOrEmpty(data["iID_MaNgachLuong_CanBo"]) == false)
            {
                string iID_MaNgachLuong_CanBo = data["iID_MaNgachLuong_CanBo"];
                string sTenNgachLuong_CanBo = LuongModels.Get_TenNgachLuong(iID_MaNgachLuong_CanBo);
                data["sTenNgachLuong_CanBo"] = String.Format("{0} - {1}", iID_MaNgachLuong_CanBo, sTenNgachLuong_CanBo);
            }
            if (String.IsNullOrEmpty(data["iID_MaBacLuong_CanBo"]) == false)
            {
                String iID_MaBacLuong_CanBo = data["iID_MaBacLuong_CanBo"];
                String sTenCapBac_CanBo = LuongModels.Get_TenBacLuong(iID_MaBacLuong_CanBo);
                data["sTenBacLuong_CanBo"] = String.Format("{0} - {1}", iID_MaBacLuong_CanBo, sTenCapBac_CanBo);
            }
            // Gán các giá trị thêm ->

            //<- Đồng bộ trường và trường có phần trước là Parent
            for (int i = 1; i < dtBangLuongChiTiet.Columns.IndexOf("iSTT"); i++)
            {
                String tgTruong = dtBangLuongChiTiet.Columns[i].ColumnName;
                if (tgTruong.StartsWith("r"))
                {
                    if (data[tgTruong] == null)
                    {
                        data[tgTruong] = "0";
                    }
                }
                if (data[ParentID + "_" + tgTruong] != null)
                {
                    data[tgTruong] = data[ParentID + "_" + tgTruong];
                }
                if (data[tgTruong] != null)
                {
                    data[ParentID + "_" + tgTruong] = data[tgTruong];
                }
            }
            // Đồng bộ trường và trường có phần trước là Parent ->
            
            //Xác định các trường được phép nhập hay không
            NameValueCollection arrKhongNhap = new NameValueCollection();
            Boolean ChiDoc = bang.ChiDoc;

            arrKhongNhap.Add("sTenDonVi", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("sSoSoLuong_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("sTenNgachLuong_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("sTenBacLuong_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("sHoVaTen_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("sTenLyDoTangGiam", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("sSoTaiKhoan_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("iSoNguoiPhuThuoc_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rLuongCoBan_HeSo_CanBo", (ChiDoc || data["iID_MaNgachLuong_CanBo"] == "1" || data["iID_MaNgachLuong_CanBo"] == "2" || data["iID_MaNgachLuong_CanBo"]=="4") ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("sTenLyDoTangGiam", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rPhuCap_VuotKhung_HeSo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rPhuCap_BaoLuu_HeSo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            //arrKhongNhap.Add("dNgayNhapNgu_CanBo", (!ChiDoc || data["iID_MaNgachLuong_CanBo"] == "1" || data["iID_MaNgachLuong_CanBo"] == "2" || data["iID_MaNgachLuong_CanBo"] == "4") ? "khong-nhap='0'" : "khong-nhap='1'");
            arrKhongNhap.Add("dNgayNhapNgu_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("dNgayXuatNgu_CanBo", arrKhongNhap["dNgayNhapNgu_CanBo"]);
            arrKhongNhap.Add("dNgayTaiNgu_CanBo", arrKhongNhap["dNgayNhapNgu_CanBo"]);
            arrKhongNhap.Add("rPhuCap_ChucVu_HeSo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rTienAn1Ngay", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rPhuCap_TrenHanDinh_HeSo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("iNuQuanNhan_CanBo", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rKhoanTru_Khac", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("bBaoHiem_ThatNghiep_CaNhan_CoNop", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rTrichLuong", (ChiDoc || data["iTrichLuong_Loai"] != "2") ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("bOmDaiNgay", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("iSoNgayNghiOm", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            arrKhongNhap.Add("rTroCapKhac", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            //arrKhongNhap.Add("bTruyLinh", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            //arrKhongNhap.Add("dTruyLinhLuong_TuNgay", (ChiDoc || data["bTruyLinhLuong"] == "True") ? "khong-nhap='1'" : "khong-nhap='0'");
            //arrKhongNhap.Add("dTruyLinhLuong_DenNgay", (ChiDoc || data["bTruyLinhLuong"] == "True") ? "khong-nhap='1'" : "khong-nhap='0'");
           // arrKhongNhap.Add("dTruyLinh_TuNgay", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            //arrKhongNhap.Add("dTruyLinh_DenNgay", (ChiDoc) ? "khong-nhap='1'" : "khong-nhap='0'");
            
            
            DataTable dtDanhMucLoaiCongThuc = LuongModels.Get_dtDanhMucLoaiCongThuc();
            String BangID = "BangDuLieu";
            int Bang_FixedRow_Height = 50;
    
            String sUrl_GetGiaTri = Url.Action("get_GiaTri", "Public");
            if (bang.ChiDoc==false)
            {
                if (iID_MaBangLuongChiTiet == Guid.Empty.ToString())
                {%>
                    <form action="<%=Url.Action("ThemCanBo_Submit", "Luong_BangLuongChiTiet_CanBo", new {ParentID = ParentID, iID_MaBangLuong = iID_MaBangLuong})%>" method="post">
                <%}
                else
                { %>
                    <form action="<%=Url.Action("SuaCanBo_Submit", "Luong_BangLuongChiTiet_CanBo", new {ParentID = ParentID, iID_MaBangLuong = iID_MaBangLuong, iID_MaBangLuongChiTiet = iID_MaBangLuongChiTiet})%>" method="post">
                <%}
            }%>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" >
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid" >
                          <tr style="height:52px;"><th colspan="4" style=" font-size:14pt;">CẬP NHẬT SỐ LIỆU LƯƠNG</th></tr>
                            <tr>    
                                <td class="td_label"><div>Đơn vị<span style="color:red">&nbsp;(*)</span></div></td>
                                <td colspan="7"><div>
                                    <%=MyHtmlHelper.AutocompleteTextBox(new{
                                            ParentID = ParentID,
                                            TruongText = "sTenDonVi",
                                            Attributes = "tab-index='0' style='width:99%;' " + arrKhongNhap["sTenDonVi"],
                                            TruongValue = "iID_MaDonVi",
                                            data = data,
                                            Url_LayDanhSach = Url.Action("get_DanhSachDonViTheoBangLuong", "Luong_BangLuongChiTiet_CanBo", new { iID_MaBangLuong = iID_MaBangLuong }),
                                            Url_LayGiaTri = Url.Action("get_GiaTriDonViTheoBangLuong", "Luong_BangLuongChiTiet_CanBo", new { iID_MaBangLuong = iID_MaBangLuong })
                                        })%>
                                </div></td>    
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Số sổ lương</div></td>
                                <td colspan="7"><div style="float:left; width:40%;"><%=MyHtmlHelper.TextBox(ParentID, data, "sSoSoLuong_CanBo", "", "tab-index='1' style='width:99%;' " + arrKhongNhap["sSoSoLuong_CanBo"], 0)%>
                             </div>
                               <div style="float: right; color:Red;width:60%;">  &nbsp;&nbsp;(Thêm mới không phải nhập số sổ lương)</div></td>
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Ngạch<span style="color:red">&nbsp;(*)</span></div></td>
                                <td colspan="3"><div>
                                    <%=MyHtmlHelper.AutocompleteTextBox(new{
                                            ParentID = ParentID,
                                            data = data,
                                            TruongText = "sTenNgachLuong_CanBo",
                                            TruongValue = "iID_MaNgachLuong_CanBo",
                                            Attributes = "tab-index='2' style='width:99%;' " + arrKhongNhap["sTenNgachLuong_CanBo"],
                                            Url_LayDanhSach = Url.Action("get_DanhSach", "public", new { Truong = "sTenNgachLuong" }),
                                            Url_LayGiaTri = Url.Action("get_GiaTri", "public", new { Truong = "sTenNgachLuong" }),
                                            fnComplete = "func_Auto_Complete_NgachLuong"
                                        })%>
                                    <script type="text/javascript">
                                        function func_Auto_Complete_NgachLuong(id, item) {
                                            document.getElementById('<%=ParentID%>_sTenBacLuong_CanBo').value = '';
                                            document.getElementById('<%=ParentID%>_iID_MaBacLuong_CanBo').value = '';
                                            document.getElementById('<%=ParentID%>_rLuongCoBan_HeSo_CanBo').value = '0';
                                            document.getElementById('<%=ParentID%>_rLuongCoBan_HeSo_CanBo_show').value = '';
                                            document.getElementById('<%=ParentID%>_rLuongCoBan').value = '0';
                                            document.getElementById('<%=ParentID%>_rLuongCoBan_show').value = '';
                                            if (item.value == "1" || item.value == "2" || item.value == "4") {
                                                $('#<%=ParentID%>_dNgayNhapNgu_CanBo').attr("khong-nhap", "0");
                                                $('#<%=ParentID%>_dNgayXuatNgu_CanBo').attr("khong-nhap", "0");
                                                $('#<%=ParentID%>_dNgayTaiNgu_CanBo').attr("khong-nhap", "0");
                                            }
                                            else {
                                                $('#<%=ParentID%>_dNgayNhapNgu_CanBo').attr("khong-nhap", "1");
                                                $('#<%=ParentID%>_dNgayXuatNgu_CanBo').attr("khong-nhap", "1");
                                                $('#<%=ParentID%>_dNgayTaiNgu_CanBo').attr("khong-nhap", "1");
                                                document.getElementById('<%=ParentID%>_dNgayNhapNgu_CanBo').value = '';
                                                document.getElementById('<%=ParentID%>_dNgayXuatNgu_CanBo').value = '';
                                                document.getElementById('<%=ParentID%>_dNgayTaiNgu_CanBo').value = '';
                                            }
                                        }
                                    </script>
                                </div></td>  
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Cấp bậc<span style="color:red">&nbsp;(*)</span></div></td>
                                <td colspan="3"><div>
                                    <%=MyHtmlHelper.AutocompleteTextBox(new{
                                                ParentID = ParentID,
                                                data = data,
                                                TruongText = "sTenBacLuong_CanBo",
                                                TruongValue = "iID_MaBacLuong_CanBo",
                                                Attributes = "tab-index='3' style='width:99%;' " + arrKhongNhap["sTenBacLuong_CanBo"],
                                                fnLayDSGiaTri = "sTenBacLuong_GhepGiaTriThem",
                                                Url_LayDanhSach = Url.Action("get_DanhSach", "public", new { Truong = "sTenBacLuong" }),
                                                Url_LayGiaTri = Url.Action("get_GiaTri", "public", new { Truong = "sTenBacLuong" }),
                                                fnComplete = "func_Auto_Complete_BacLuong"
                                            })%>
                                    <script type="text/javascript">
                                        function sTenBacLuong_GhepGiaTriThem() {
                                            return document.getElementById('<%=ParentID%>_iID_MaNgachLuong_CanBo').value;
                                        }
                                        function func_Auto_Complete_BacLuong(id, item) {
                                            var rHeSoLuong = parseFloat(item.ThongTinThem);
                                            var rLuongCoBan = rHeSoLuong * rLuongToiThieu;
                                            document.getElementById('<%=ParentID%>_rLuongCoBan_HeSo_CanBo').value = rHeSoLuong;
                                            document.getElementById('<%=ParentID%>_rLuongCoBan_HeSo_CanBo_show').value = FormatNumber(rHeSoLuong, 2);
                                            document.getElementById('<%=ParentID%>_rLuongCoBan').value = rLuongCoBan;
                                            document.getElementById('<%=ParentID%>_rLuongCoBan_show').value = FormatNumber(rLuongCoBan, 0);
                                            if (rHeSoLuong == 0) {
                                                $('#<%=ParentID%>_rLuongCoBan_HeSo_CanBo_show').attr("khong-nhap", "0");
                                            }
                                            else {
                                                $('#<%=ParentID%>_rLuongCoBan_HeSo_CanBo_show').attr("khong-nhap", "1");
                                            }
                                        }
                                    </script>
                                </div></td>     
                            </tr>
                            <tr>                                                
                                <td class="td_label"><div>Họ tên<span style="color:red">&nbsp;(*)</span></div></td>
                                <td colspan="3"><div><%=MyHtmlHelper.TextBox(ParentID, data, "sHoVaTen_CanBo", "", "tab-index='4' style='width:99%;' " + arrKhongNhap["sHoVaTen_CanBo"], 0)%></div></td>
                                <%=MyHtmlHelper.Hidden(ParentID, data, "sHoDem_CanBo", "")%>
                                <%=MyHtmlHelper.Hidden(ParentID, data, "sTen_CanBo", "")%>
                                <script type="text/javascript">
                                    $('#<%=ParentID%>_sHoVaTen_CanBo').focusout(function () {
                                        if ($('#<%=ParentID%>_sHoVaTen_CanBo').attr("khong-nhap") == "0") {
                                            var sHoDem = '';
                                            var sTen = '';
                                            var sHoVaTen = $('#<%=ParentID%>_sHoVaTen_CanBo').val();
                                            if (sHoVaTen != '') {
                                                sHoVaTen = ' ' + sHoVaTen.trim();
                                                var arrTG = sHoVaTen.split(' ');
                                                sTen = arrTG[arrTG.length - 1];
                                                var i;
                                                for (i = 0; i < arrTG.length - 1; i++) {
                                                    sHoDem += ' ' + arrTG[i];
                                                }
                                                sHoDem = sHoDem.trim();
                                            }
                                            $('#<%=ParentID%>_sHoDem_CanBo').val(sHoDem);
                                            $('#<%=ParentID%>_sTen_CanBo').val(sTen);
                                        }
                                    });
                                </script>
                            </tr>
                            <tr>
                                <td class="td_label"><div>Lý do T/G</div></td>
                                <td><div>
                                    <%=MyHtmlHelper.AutocompleteTextBox(new {
                                                ParentID = ParentID,
                                                data = data,
                                                TruongText = "sTenLyDoTangGiam",
                                                TruongValue = "iID_MaLyDoTangGiam",
                                                Attributes = "tab-index='5' style='width:99%;' " + arrKhongNhap["sTenLyDoTangGiam"],
                                                Url_LayDanhSach = Url.Action("get_DanhSach", "public", new { Truong = "sTenLyDoTangGiam" }),
                                                Url_LayGiaTri = Url.Action("get_GiaTri", "public", new { Truong = "sTenLyDoTangGiam" }),
                                                fnComplete = "func_Auto_Complete_LyDoTangGiam"
                                            })%>
                                    <%=MyHtmlHelper.Hidden(ParentID,data,"sHieuTangGiam","") %>
                                    <script type="text/javascript">
                                        function func_Auto_Complete_LyDoTangGiam(id, item) {
                                            document.getElementById('div_LyDoTangGiam').innerHTML = item.MoTa;
                                            document.getElementById('<%=ParentID%>_sHieuTangGiam').value = item.sHieuTangGiam;
                                        }
                                    </script>
                                </div></td>
                                <td colspan="2"><span id="div_LyDoTangGiam" style="color:red;"><%=data["LyDoTangGiam"]%></span></td>
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Số tài khoản</div></td>                                   
                                <td ><div><%=MyHtmlHelper.TextBox(ParentID, data, "sSoTaiKhoan_CanBo", "", "tab-index='6' style='width:98%;' " + arrKhongNhap["sSoTaiKhoan_CanBo"], 0)%></div></td>
                                <td class="td_label"><div>Số người phụ thuộc</div></td>
                                <td ><div><%=MyHtmlHelper.TextBox(ParentID, data, "iSoNguoiPhuThuoc_CanBo", "", "tab-index='7' style='width:50px;' " + arrKhongNhap["iSoNguoiPhuThuoc_CanBo"])%></div></td>
                            </tr>                                
                            <tr>    
                                <td class="td_label"><div>Hệ số lương<span style="color:red">&nbsp;(*)</span></div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rLuongCoBan_HeSo_CanBo", LoaiTextBox = 1, Attributes = "tab-index='9' style='width:50px;' " + arrKhongNhap["rLuongCoBan_HeSo_CanBo"], SoSauDauPhay = 2 })%></div></td>
                                <td class="td_label"><div>Lương cơ bản</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rLuongCoBan", LoaiTextBox = 1, Attributes = "khong-nhap='1' style='width:98%;'"})%></div></td>
                            </tr>                               
                                <tr>    
                                <td class="td_label"><div>Vượt khung</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rPhuCap_VuotKhung_HeSo", LoaiTextBox = 1, Attributes = "tab-index='10' style='width:50px;' maxlength='5'" + arrKhongNhap["rPhuCap_VuotKhung_HeSo"], SoSauDauPhay = 2 })%></div></td>
                                <td class="td_label"><div>HS Bảo lưu</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rPhuCap_BaoLuu_HeSo", LoaiTextBox = 1, Attributes = "tab-index='11' style='width:50px;' maxlength='5'" + arrKhongNhap["rPhuCap_BaoLuu_HeSo"], SoSauDauPhay = 2 })%></div></td>
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Nhập ngũ</div></td>                                    
                                <td colspan="3">
                                    <table class="mGrid" cellpadding="0" cellspacing="0" style="border: solid 0px #525252;">
                                        <tr>                                                    
                                            <td><div"><%=MyHtmlHelper.DateTextBox(new { ParentID = ParentID, data = data, TenTruong = "dNgayNhapNgu_CanBo", Format = "MM/yyyy", Attributes = "tab-index='12' style='width:97%;' " + arrKhongNhap["dNgayNhapNgu_CanBo"] })%></div></td>
                                            <td class="td_label"><div>Xuất ngũ</div></td>
                                            <td ><div><%=MyHtmlHelper.DateTextBox(new { ParentID = ParentID, data = data, TenTruong = "dNgayXuatNgu_CanBo", Format = "MM/yyyy", Attributes = "tab-index='13' style='width:97%;' " + arrKhongNhap["dNgayXuatNgu_CanBo"] })%></div></td>
                                            <td class="td_label"><div>Tái ngũ</div></td>
                                            <td ><div><%=MyHtmlHelper.DateTextBox(new { ParentID = ParentID, data = data, TenTruong = "dNgayTaiNgu_CanBo", Format = "MM/yyyy", Attributes = "tab-index='14' style='width:97%;' " + arrKhongNhap["dNgayTaiNgu_CanBo"] })%></div></td>
                                        </tr>
                                    </table>           
                                </td>                                    
                            </tr>
                            <tr>
                                <td class="td_label"><div>H.s Chức vụ</div></td>
                                <td><div>
                                    <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rPhuCap_ChucVu_HeSo", LoaiTextBox = 1, Attributes = "tab-index='15' style='width:50px;' maxlength='5'" + arrKhongNhap["rPhuCap_ChucVu_HeSo"], SoSauDauPhay = 2 })%>
                                    <span id="rPhuCap_ChucVu" style="color:red;"><%=CommonFunction.DinhDangSo(data["rPhuCap_ChucVu"], 0, false)%></span>
                                </div></td>
                                <td class="td_label"><div>H.s Khu vực</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rPhuCap_KhuVuc_HeSo", LoaiTextBox = 1, Attributes = "khong-nhap='0' tab-index='16' style='width:50px;' maxlength='5'" + arrKhongNhap["rPhuCap_KhuVuc_HeSo"], SoSauDauPhay = 2 })%></div></td>
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Ăn một ngày</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rTienAn1Ngay", LoaiTextBox = 1, Attributes = "khong-nhap='0' tab-index='17' style='width:98%;' maxlength='6'" + arrKhongNhap["rTienAn1Ngay"], SoSauDauPhay = 0 })%></div></td>
                                <td class="td_label"><div>HS Trên hạn định</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rPhuCap_TrenHanDinh_HeSo", LoaiTextBox = 1, Attributes = "tab-index='18' style='width:50px;' maxlength='3'" + arrKhongNhap["rPhuCap_TrenHanDinh_HeSo"], SoSauDauPhay = 2 })%></div></td>
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Nữ</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "iNuQuanNhan_CanBo", LoaiTextBox = 1, Attributes = "tab-index='19' style='width:98%;' maxlength='1'" + arrKhongNhap["iNuQuanNhan_CanBo"], SoSauDauPhay = 0 })%></div></td>
                                <td class="td_label"><div>Trừ khác</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rKhoanTru_Khac", LoaiTextBox = 1, Attributes = "tab-index='20' style='width:98%;' maxlength='9'" + arrKhongNhap["rKhoanTru_Khac"], SoSauDauPhay = 0 })%></div></td>
                            </tr>
                            <tr> 
                             <td class="td_label"><div>Trợ cấp khác</div></td>
                              <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rTroCapKhac", LoaiTextBox = 1, Attributes = "tab-index='21' style='width:98%;' maxlength='9'" + arrKhongNhap["rTroCapKhac"], SoSauDauPhay = 0 })%></div></td>
                               
                                   
                              
                                <td class="td_label"><div>Trích lương, PC</div></td>
                                <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "rTrichLuong", LoaiTextBox = 1, Attributes = "tab-index='22' style='width:98%;' maxlength='9'" + arrKhongNhap["rTrichLuong"], SoSauDauPhay = 0 })%></div></td>
                                <%=MyHtmlHelper.Hidden(ParentID, data, "rTrichLuong_SoLuong", "")%>
                                <%=MyHtmlHelper.Hidden(ParentID, data, "iTrichLuong_Loai", "")%>
                                <script type="text/javascript">
                                    $('#<%=ParentID%>_rTrichLuong_show').focusout(function () {
                                        var TruongKhongNhap = $('#<%=ParentID%>_rTrichLuong_show').attr("khong-nhap");
                                        if (TruongKhongNhap == "0") {
                                            var GiaTri = $('#<%=ParentID%>_rTrichLuong').val();
                                            $('#<%=ParentID%>_rTrichLuong_SoLuong').val(GiaTri);
                                        }
                                    });
                                </script>
                            </tr>
                            <tr>    
                                <td class="td_label"><div>Có nộp BHTN</div></td>

                                <td colspan="3">
                                   <table class="mGrid" cellpadding="0" cellspacing="0" style="border: solid 0px #525252;">
                                        <tr>                                                    
                                            <td><div><%=MyHtmlHelper.CheckBox(ParentID, data, "bBaoHiem_ThatNghiep_CaNhan_CoNop", "", "tab-index='25' " + arrKhongNhap["bBaoHiem_ThatNghiep_CaNhan_CoNop"])%></div></td>
                                            <td class="td_label"><div>Ốm dài</div></td>
                                            <td ><div><%=MyHtmlHelper.CheckBox(ParentID, data, "bOmDaiNgay", "", "tab-index='24' " + arrKhongNhap["bOmDaiNgay"])%></div></td>
                                            <td class="td_label"><div>Số ngày nghỉ ốm</div></td>
                                            <td><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, data = data, TenTruong = "iSoNgayNghiOm", LoaiTextBox = 1, Attributes = "tab-index='23' style='width:98%;' maxlength='3'" + arrKhongNhap["iSoNgayNghiOm"], SoSauDauPhay = 0 })%></div></td>
                                        </tr>
                                    </table>     
                                </td>


                             
                           
                              
                            </tr>
                            <tr style="display:none;">    
                                <td class="td_label"><div>Truy lĩnh</div></td>                                    
                                <td colspan="3">
                                    <table class="mGrid" cellpadding="0" cellspacing="0" style="border: solid 0px #525252;">
                                        <tr>                                                    
                                            <td ><div><%=MyHtmlHelper.CheckBox(ParentID, data, "bTruyLinh", "", "tab-index='25' " + arrKhongNhap["bOmDaiNgay"])%></div></td>
                                            <td class="td_label"><div>Từ ngày</div></td>
                                            <td ><div><%=MyHtmlHelper.DateTextBox(new { ParentID = ParentID, data = data, TenTruong = "dTruyLinh_TuNgay", Format = "dd/MM/yyyy", Attributes = "tab-index='26' style='width:97%;' " + arrKhongNhap["dTruyLinh_TuNgay"] })%></div></td>
                                            <td class="td_label"><div>Đến ngày</div></td>
                                            <td ><div><%=MyHtmlHelper.DateTextBox(new { ParentID = ParentID, data = data, TenTruong = "dTruyLinh_DenNgay", Format = "dd/MM/yyyy", Attributes = "tab-index='27' style='width:97%;' " + arrKhongNhap["dTruyLinh_DenNgay"] })%></div></td>
                                        </tr>
                                    </table>           
                                </td>                                    
                            </tr> 
                        </table>
                        </td>
                    <td valign="top" width="400px">
                        <%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = 435, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>    
                    </td>
                </tr>
            </table>
            <hr />
          <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align:center;">
                <tr><td colspan="4" style="color:Red;"><%= Html.ValidationSummary()%></td></tr>
                <tr>
                    
                    <td align="right" style="width: 90px"><input type="button" class="button" value="Lưu" onclick="Bang_HamTruocKhiKetThuc();"/></td>
                    <%
                        if (okFormThemMoi == false)
                        {
                            %>
                            <td style="width: 5px">&nbsp;</td>
                            <td align="right" style="width: 10%"><input type="button" class="button" value="Tăng tiếp" onclick="Bang_HamTruocKhiKetThuc(3);"/></td>
                            <td style="width: 5px">&nbsp;</td>
                            <td align="right" style="width: 10%"><input type="button" class="button" value="Thêm mới" onclick="parent.jsLuong_ThemMoiCanBo_Dialog_Open();"/></td>
                            <%
                        }
                         %>
                    <td style="width: 5px">&nbsp;</td>
                    <td  style="width: 90px"><input type="button" class="button" value="Hủy" onclick="parent.jsLuong_Dialog_Close(<%=strPhaiLoadLai%>);"/></td>
                </tr>
            </table>
            <div style="display:none;">
                <input type="hidden" id="idAction" name="idAction" value="0" />
                <input type="hidden" id="idXauDuLieuThayDoi" name="idXauDuLieuThayDoi" value="<%=HttpUtility.HtmlEncode(bang.strThayDoi)%>" />
                <input type="hidden" id="idXauLaHangCha" name="idXauLaHangCha" value="<%=HttpUtility.HtmlEncode(bang.strLaHangCha)%>" />
                <input type="hidden" id="idXauMaCacHang" name="idXauMaCacHang" value="<%=HttpUtility.HtmlEncode(bang.strDSMaHang)%>" />
                <input type="hidden" id="idXauMaCacCot" name="idXauMaCacCot" value="<%=HttpUtility.HtmlEncode(bang.strDSMaCot)%>" />
                <input type="hidden" id="idXauGiaTriChiTiet" name="idXauGiaTriChiTiet" value="<%=bang.strDuLieu%>" />
                <input type="submit" id="btnXacNhanGhi" value="XN" />
                <input type="hidden" id="idXauCacHangDaXoa" name="idXauCacHangDaXoa" value="" />
            </div>
            <div style="display:none;">
                <input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
                <input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
                <input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>" />
                <input type="hidden" id="idBangChiDoc" value="<%=HttpUtility.HtmlEncode(bang.strChiDoc)%>" />
                <input type="hidden" id="idXauEdit" value="<%=HttpUtility.HtmlEncode(bang.strEdit)%>" />
                <input type="hidden" id="idViewport_N" value="<%=HttpUtility.HtmlEncode(bang.Viewport_N)%>" />
                <input type="hidden" id="idNC_Fixed" value="<%=HttpUtility.HtmlEncode(bang.nC_Fixed)%>" />
                <input type="hidden" id="idNC_Slide" value="<%=HttpUtility.HtmlEncode(bang.nC_Slide)%>" />
                <input type="hidden" id="idCoCotTongSo" value="0" />
            </div>
            <script type="text/javascript">
                var rLuongToiThieu = <%=rLuongToiThieu %>;
                $(document).ready(function () {
                    strParentID = '<%=ParentID%>';
                    BangDuLieu_sPhuCap_ChucVu_CongThuc = '<%=sPhuCap_ChucVu_CongThuc%>';
                    Bang_Url_getGiaTri = '<%=sUrl_GetGiaTri%>';
                    Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
                    BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
                    Bang_keys.fnSetFocus(null, null);
                    $('#<%=ParentID%>_rLuongCoBan_HeSo_CanBo_show').focusout(function () {
                        if ($('#<%=ParentID%>_rLuongCoBan_HeSo_CanBo_show').attr("khong-nhap") == "0") {
                            var rHeSoLuong = parseFloat($('#<%=ParentID%>_rLuongCoBan_HeSo_CanBo').val());
                            var rLuongCoBan = rHeSoLuong * rLuongToiThieu;
                            document.getElementById('<%=ParentID%>_rLuongCoBan').value = rLuongCoBan;
                            document.getElementById('<%=ParentID%>_rLuongCoBan_show').value = FormatNumber(rLuongCoBan, 0);
                        }
                    });

                    $('#<%=ParentID%>_rPhuCap_ChucVu_HeSo_show').focusout(function () {
                        if ($('#<%=ParentID%>_rPhuCap_ChucVu_HeSo_show').attr("khong-nhap") == "0") {
                            var rPhuCap_ChucVu_HeSo = parseFloat($('#<%=ParentID%>_rPhuCap_ChucVu_HeSo').val());
                            if(isNaN(rPhuCap_ChucVu_HeSo)) rPhuCap_ChucVu_HeSo = 0;
                            BangDuLieu_arrBangLuongChiTiet['rPhuCap_ChucVu_HeSo'] = rPhuCap_ChucVu_HeSo;
                            var rPhuCap_ChucVu = BangDuLieu_ThucHienCongThuc_sPhuCap_ChucVu(rPhuCap_ChucVu_HeSo);
                            if(isNaN(rPhuCap_ChucVu)) rPhuCap_ChucVu = 0;
                            document.getElementById('rPhuCap_ChucVu').innerHTML = FormatNumber(rPhuCap_ChucVu, 0);
                        }
                    });

                    <%
                        for (int i = 0; i < dtDanhMucLoaiCongThuc.Rows.Count; i++)
                        {
                            %>BangDuLieu_arrDSTruong.push('<%=dtDanhMucLoaiCongThuc.Rows[i]["sTen"]%>');
                            BangDuLieu_arrBangLuongChiTiet['<%=dtDanhMucLoaiCongThuc.Rows[i]["sTen"]%>'] = '(<%=dtDanhMucLoaiCongThuc.Rows[i]["sCongThuc"]%>)';
                            <%  
                        }
                        for (int i = 1; i < dtBangLuongChiTiet.Columns.IndexOf("iSTT"); i++)
                        {
                            String sValue = "";
                            if(data[ParentID+"_"+dtBangLuongChiTiet.Columns[i].ColumnName]!=null)
                            {
                                sValue = data[ParentID+"_"+dtBangLuongChiTiet.Columns[i].ColumnName];
                            }
                            %>BangDuLieu_arrDSTruongBangLuongChiTiet.push('<%=dtBangLuongChiTiet.Columns[i].ColumnName%>');
                            BangDuLieu_arrDSTruong.push('[<%=dtBangLuongChiTiet.Columns[i].ColumnName%>]');
                            BangDuLieu_arrBangLuongChiTiet['[<%=dtBangLuongChiTiet.Columns[i].ColumnName%>]'] = '<%=sValue.ToLower() %>';
                            <%  
                        }
                    %>
                });

            </script>
            <%if (bang.ChiDoc==false)
            {
            %></form><%
            }
    }
%>
</asp:Content>