<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    string iID_MaCanBo = Convert.ToString(props["iID_MaCanBo"].GetValue(Model));
    String ParentID = "KTCT";
    String sHoTenCha = "", iNamSinhCha = "", iID_MaNgheNghiepCha = "", sHoTenMe = "", iNamSinhMe = "", iID_MaNgheNghiepMe = "", sQueQuanGD = "", sChoOGiaDinh = "",
        sTinhHinhKinhTeGiaDinh = "", sTinhHinhChinhTriGiaDinh = "", iSoNguoiCon = "", iViTriSoMay = "", iSoConTrai = "", iSoConGai = "", sHoTenChaChong = "", iNamSinhChaChong = "",
        iID_MaNgheNghiepChaChong = "", sHoTenMeChong = "", iNamSinhMeChong = "", iID_MaNgheNghiepMeChong = "", iID_MaThanhPhanXuatThanGDChong = "",
        iID_MaThanhPhanBanThanGiaDinhChong = "", iSoNguoiConSinhDuoc = "", iSoTrai = "", iSoGai = "", iViTri = "", sHoTenVoChong = "", iNamSinhVoChong = "", iID_MaNgheNghiepVoChong = "",
        sChoOVoChong = "", sDanhSachCacCon = "", sQueQuanChong = "", sChoONhaChong = "", sTinhHinhKinhTeGiaDinhChong = "", sTinhHinhChinhTriGiaDinhChong = "";
    //Nghệ nghiệp
    var dtNgheNghiep = DanhMucModels.DT_DanhMuc("CanBo_NgheNghiep", true, "--- Lựa chọn ---");
    SelectOptionList slMaNgheNghiep = new SelectOptionList(dtNgheNghiep, "iID_MaDanhMuc", "sTen");
    if (dtNgheNghiep != null) dtNgheNghiep.Dispose();

    //Thành phần gia đình
    var dtTPGD = DanhMucModels.DT_DanhMuc("CanBo_ThanhPhanGD", true, "--- Lựa chọn ---");
    SelectOptionList slMaTPGD = new SelectOptionList(dtTPGD, "iID_MaDanhMuc", "sTen");
    if (dtTPGD != null) dtTPGD.Dispose();

    //Nghệ nghiệp
    var dtTPBT = DanhMucModels.DT_DanhMuc("CanBo_ThanhPhanBanThan", true, "--- Lựa chọn ---");
    SelectOptionList slMaTPBT = new SelectOptionList(dtTPBT, "iID_MaDanhMuc", "sTen");
    if (dtTPBT != null) dtTPBT.Dispose();
    if (String.IsNullOrEmpty(iID_MaCanBo) == false && iID_MaCanBo != "")
    {
        var dtCanBo = CanBo_HoSoNhanSuModels.GetChiTiet(iID_MaCanBo);
        if (dtCanBo.Rows.Count > 0)
        {
            DataRow DR = dtCanBo.Rows[0];
            sHoTenCha = HamChung.ConvertToString(DR["sHoTenCha"]);
            iNamSinhCha = HamChung.ConvertToString(DR["iNamSinhCha"]);
            iID_MaNgheNghiepCha = HamChung.ConvertToString(DR["iID_MaNgheNghiepCha"]);
            sHoTenMe = HamChung.ConvertToString(DR["sHoTenMe"]);
            string NamSinhMe = HamChung.ConvertToString(DR["iNamSinhMe"]);
            if (NamSinhMe != "0") iNamSinhMe = NamSinhMe;
            iID_MaNgheNghiepMe = HamChung.ConvertToString(DR["iID_MaNgheNghiepMe"]);
            sQueQuanGD = HamChung.ConvertToString(DR["sQueQuanGD"]);
            sChoOGiaDinh = HamChung.ConvertToString(DR["sChoOGiaDinh"]);
            sTinhHinhKinhTeGiaDinh = HamChung.ConvertToString(DR["sTinhHinhKinhTeGiaDinh"]);
            sTinhHinhChinhTriGiaDinh = HamChung.ConvertToString(DR["sTinhHinhChinhTriGiaDinh"]);

            string SoNguoiCon = HamChung.ConvertToString(DR["iSoNguoiCon"]);
            if (SoNguoiCon != "0") iSoNguoiCon = SoNguoiCon;
            string ViTriSoMay = HamChung.ConvertToString(DR["iViTriSoMay"]);
            if (ViTriSoMay != "0") iViTriSoMay = ViTriSoMay;

            string SoConTrai = HamChung.ConvertToString(DR["iSoConTrai"]);
            if (SoConTrai != "0") iSoConTrai = SoConTrai;


            string SoConGai = HamChung.ConvertToString(DR["iSoConGai"]);
            if (SoConGai != "0") iSoConGai = SoConGai;            
          
            sHoTenChaChong = HamChung.ConvertToString(DR["sHoTenChaChong"]);

            string NamSinhChaChong = HamChung.ConvertToString(DR["iNamSinhChaChong"]);
            if (NamSinhChaChong != "0") iNamSinhChaChong = NamSinhChaChong;             
            
     
            iID_MaNgheNghiepChaChong = HamChung.ConvertToString(DR["iID_MaNgheNghiepChaChong"]);
            sHoTenMeChong = HamChung.ConvertToString(DR["sHoTenMeChong"]);

            string NamSinhMeChong = HamChung.ConvertToString(DR["iNamSinhMeChong"]);
            if (NamSinhMeChong != "0") iNamSinhMeChong = NamSinhMeChong;  
            
            
  
            iID_MaNgheNghiepMeChong = HamChung.ConvertToString(DR["iID_MaNgheNghiepMeChong"]);
            iID_MaThanhPhanXuatThanGDChong = HamChung.ConvertToString(DR["iID_MaThanhPhanXuatThanGDChong"]);
            iID_MaThanhPhanBanThanGiaDinhChong = HamChung.ConvertToString(DR["iID_MaThanhPhanBanThanGiaDinhChong"]);

            string SoNguoiConSinhDuoc = HamChung.ConvertToString(DR["iSoNguoiConSinhDuoc"]);
            if (SoNguoiConSinhDuoc != "0") iSoNguoiConSinhDuoc = SoNguoiConSinhDuoc;


            string SoTrai = HamChung.ConvertToString(DR["iSoTrai"]);
            if (SoTrai != "0") iSoTrai = SoTrai;
            string SoGai = HamChung.ConvertToString(DR["iSoGai"]);
            if (SoGai != "0") iSoGai = SoGai;

            string ViTri = HamChung.ConvertToString(DR["iViTri"]);
            if (ViTri != "0") iViTri = ViTri; 
            

            sHoTenVoChong = HamChung.ConvertToString(DR["sHoTenVoChong"]);

            string NamSinhVoChong = HamChung.ConvertToString(DR["iNamSinhVoChong"]);
            if (NamSinhVoChong != "0") iNamSinhVoChong = NamSinhVoChong; 
            iID_MaNgheNghiepVoChong = HamChung.ConvertToString(DR["iID_MaNgheNghiepVoChong"]);
            sChoOVoChong = HamChung.ConvertToString(DR["sChoOVoChong"]);
            sDanhSachCacCon = HamChung.ConvertToString(DR["sDanhSachCacCon"]);
            sQueQuanChong = HamChung.ConvertToString(DR["sQueQuanChong"]);
            sChoONhaChong = HamChung.ConvertToString(DR["sChoONhaChong"]);
            sTinhHinhKinhTeGiaDinhChong = HamChung.ConvertToString(DR["sTinhHinhKinhTeGiaDinhChong"]);
            sTinhHinhChinhTriGiaDinhChong = HamChung.ConvertToString(DR["sTinhHinhChinhTriGiaDinhChong"]);
        }
        if (dtCanBo != null) dtCanBo.Dispose();
    }
    using (Html.BeginForm("EditTinhHinhKT", "CanBo_HoSoNhanSu", new { ParentID = ParentID, iID_MaCanBo = iID_MaCanBo }))
    {
                            
%>
  <%= Html.Hidden(ParentID + "_DuLieuMoi", 0)%>
<div style="background-color: #ffffff; background-repeat: repeat">
    <div style="padding: 5px 1px 10px 1px;">
        <div class="box_tong">
            <div id="form2" style="padding: 10px 10px">
                <table border="0" cellpadding="10" cellspacing="10" width="100%">
                    <tr>
                        <td colspan="8" style="height: 30px;">
                            <span style="color: #006400; font-size: 14px; font-weight: bold; text-decoration: underline;">
                                TÌNH HÌNH KINH TẾ CHÍNH TRỊ GIA ĐÌNH</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Họ tên cha</div>
                        </td>
                        <td colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sHoTenCha, "sHoTenCha", "", "class=\"input1_2\" tab-index='-1' maxlength='200'")%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Năm sinh</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamSinhCha, "iNamSinhCha", "", "class=\"input1_2\" maxlength='4'", 2)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Nghề nghiệp</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMaNgheNghiep, iID_MaNgheNghiepCha, "iID_MaNgheNghiepCha", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Họ tên mẹ</div>
                        </td>
                        <td colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sHoTenMe, "sHoTenMe", "", "class=\"input1_2\" maxlength='200'")%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Năm sinh</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamSinhMe, "iNamSinhMe", "", "class=\"input1_2\" maxlength='4'", 2)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Nghề nghiệp</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMaNgheNghiep, iID_MaNgheNghiepMe, "iID_MaNgheNghiepMe", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Quê quán</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sQueQuanGD, "sQueQuanGD", "", "class=\"input1_2\" maxlength='200'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Chỗ ở hiện nay</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sChoOGiaDinh, "sChoOGiaDinh", "", "class=\"input1_2\" maxlength='200'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 10%">
                            <div>
                                Số người con</div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iSoNguoiCon, "iSoNguoiCon", "", "class=\"input1_2\" maxlength='2'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10" style="width: 10%">
                            <div>
                                Số con trai</div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iSoConTrai, "iSoConTrai", "", "class=\"input1_2\" maxlength='2'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Số con gái</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iSoConGai, "iSoConGai", "", "class=\"input1_2\" maxlength='2'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Là con thứ</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iViTriSoMay, "iViTriSoMay", "", "class=\"input1_2\" maxlength='2' style=\"width:95%;\"", 1)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <span style="color: #006400; font-weight: bold;">Tình hình kinh tế chính trị gia đình</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Kinh tế</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTinhHinhKinhTeGiaDinh, "sTinhHinhKinhTeGiaDinh", "", "class=\"input1_2\" maxlength='4000'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Chính trị</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTinhHinhChinhTriGiaDinh, "sTinhHinhChinhTriGiaDinh", "", "class=\"input1_2\" maxlength='4000'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" style="height: 30px;">
                            <span style="color: #006400; font-size: 14px; font-weight: bold; text-decoration: underline;">
                                TÌNH HÌNH KINH TẾ CHÍNH TRỊ GIA ĐÌNH CHỒNG</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Họ tên cha</div>
                        </td>
                        <td colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sHoTenChaChong, "sHoTenChaChong", "", "class=\"input1_2\" tab-index='-1' maxlength='200'")%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Năm sinh</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamSinhChaChong, "iNamSinhChaChong", "", "class=\"input1_2\" maxlength='4'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Nghề nghiệp</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMaNgheNghiep, iID_MaNgheNghiepChaChong, "iID_MaNgheNghiepChaChong", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Họ tên mẹ</div>
                        </td>
                        <td colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sHoTenMeChong, "sHoTenMeChong", "", "class=\"input1_2\" maxlength='200'")%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Năm sinh</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamSinhMeChong, "iNamSinhMeChong", "", "class=\"input1_2\" maxlength='4'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Nghề nghiệp</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMaNgheNghiep, iID_MaNgheNghiepMeChong, "iID_MaNgheNghiepMeChong", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Thành phần xuất thân</div>
                        </td>
                        <td colspan="5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMaTPGD, iID_MaThanhPhanXuatThanGDChong, "iID_MaThanhPhanXuatThanGDChong", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Thành phần bản thân</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMaNgheNghiep, iID_MaThanhPhanBanThanGiaDinhChong, "iID_MaThanhPhanBanThanGiaDinhChong", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Quê quán</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sQueQuanChong, "sQueQuanChong", "", "class=\"input1_2\" maxlength='200'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Chỗ ở hiện nay</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sChoONhaChong, "sChoONhaChong", "", "class=\"input1_2\" maxlength='200'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 10%">
                            <div>
                                Số người con</div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iSoNguoiConSinhDuoc, "iSoNguoiConSinhDuoc", "", "class=\"input1_2\" maxlength='2'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10" style="width: 10%">
                            <div>
                                Số con trai</div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iSoTrai, "iSoTrai", "", "class=\"input1_2\" maxlength='2'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Số con gái</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iSoGai, "iSoGai", "", "class=\"input1_2\" maxlength='2'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Là con thứ</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iViTri, "iViTri", "", "class=\"input1_2\" maxlength='1' style=\"width:95%;\"", 1)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <span style="color: #006400; font-weight: bold;">Tình hình kinh tế chính trị gia đình</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Kinh tế</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTinhHinhKinhTeGiaDinhChong, "sTinhHinhKinhTeGiaDinhChong", "", "class=\"input1_2\" maxlength='4000'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Chính trị</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTinhHinhChinhTriGiaDinhChong, "sTinhHinhChinhTriGiaDinhChong", "", "class=\"input1_2\" maxlength='4000'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <span style="color: #006400; font-weight: bold;">Vợ (chồng), con cái</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Họ tên vợ (chồng)</div>
                        </td>
                        <td colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sHoTenVoChong, "sHoTenVoChong", "", "class=\"input1_2\" maxlength='200'")%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Năm sinh</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamSinhVoChong, "iNamSinhVoChong", "", "class=\"input1_2\" maxlength='4'", 1)%>
                            </div>
                        </td>
                        <td class="td_form2_td10">
                            <div>
                                Nghề nghiệp</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMaNgheNghiep, iID_MaNgheNghiepVoChong, "iID_MaNgheNghiepVoChong", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Chỗ ở hiện nay</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sChoOVoChong, "sChoOVoChong", "", "class=\"input1_2\" maxlength='4000'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td10" style="width: 15%">
                            <div>
                                Họ tên, năm sinh, nghề nghiệp các con</div>
                        </td>
                        <td colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sDanhSachCacCon, "sDanhSachCacCon", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" style="height: 30px;">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="8" style="background-color: Window;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="right">
                                        <input type="submit" class="button" id="Submit1" value="Lưu" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="center" width="100px">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%} %>