<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.Reflection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script src="<%= Url.Content("~/Scripts/jsLuong_Dialog.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>

<%  
    String ParentID = "BangLuongChiTiet";
    
    String iID_MaBangLuong = Request.QueryString["iID_MaBangLuong"];
    
    String MaND = User.Identity.Name;

    DataTable dtDonVi = LuongModels.Get_DSDonViCuaBangLuong(iID_MaBangLuong);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();
    
    DataTable dtNgachLuong = LuongModels.Get_dtDanhMucNgachLuong("");
    SelectOptionList slNgachLuong = new SelectOptionList(dtNgachLuong, "iID_MaNgachLuong", "sTen");
    dtNgachLuong.Dispose();
%>

<%=MyHtmlHelper.Hidden(ParentID, "","sTen","") %>
<%=MyHtmlHelper.Hidden(ParentID, "","sHoDem","") %>
<%=MyHtmlHelper.Hidden(ParentID, "","rLuongToiThieu","") %>

<div style="background-color: #f0f9fe; background-repeat: repeat; border: solid 1px #ec3237">
    <div style="padding: 10px;">
        <div id="nhapform">
            <div id="form2">                    
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form2">
                    <tr>
                        <td width="50%">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form2">
                                <tr>    
                                    <td class="td_form2_td1"><div>Đơn vị</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Số sổ lương</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "sSoSoLuong_CanBo", "", "khong-nhap='1'")%></div></td>
                                </tr>
                                    <tr>    
                                    <td class="td_form2_td1"><div>Ngạch</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "iID_MaNgachLuong_CanBo", "", "khong-nhap='1'")%></div></td>
                                </tr>
                                <tr>                                                
                                    <td class="td_form2_td1"><div>Họ tên</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "CanBo_HoTenDayDu", "", "khong-nhap='1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Số tài khoản</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "sSoTaiKhoan_CanBo", "", "khong-nhap='1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Cấp bậc</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, TenTruong = "iID_MaBacLuong_CanBo", Attributes = "khong-nhap='1'" })%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Hệ số lương</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, TenTruong = "rLuongCoBan_HeSo_CanBo", Attributes = "tab-index='-1'" })%></div></td>
                                </tr>                                
                                <tr>    
                                    <td class="td_form2_td1"><div>Vượt khung</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(new { ParentID = ParentID, TenTruong = "rVuotKhung", Attributes = "tab-index='-1'" })%></div></td>
                                </tr>                                
                                <tr>    
                                    <td class="td_form2_td1"><div>Nhập ngũ</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.DatePicker(ParentID, "", "sNXTNgu", "", "tab-index='-1'")%></div></td>
                                </tr>                                                                
                                <tr>    
                                    <td class="td_form2_td1"><div>Hệ số chức vụ</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "rHeSoChucVu", "", "tab-index='-1'")%></div></td>
                                </tr>                                                                
                                <tr>    
                                    <td class="td_form2_td1"><div>Hệ số khu vực</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "rHeSoKhuVuc", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Có nộp BHTN</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.CheckBox(ParentID, "", "bCoNop_BHTN", "", "tab-index='-1'")%></div></td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form2">   
                                <tr>    
                                    <td class="td_form2_td1"><div>Ký hiệu tăng giảm</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "iID_MaTinhTrangCanBo", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Số người phụ thuộc</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "iSoNguoiPhuThuoc_CanBo", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Bảo lưu</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "rBaoLuu", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Xuất ngũ</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayXuatNgu_CanBo", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Tái ngũ</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayTaiNgu_CanBo", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Trên hạn định</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "rTrenHanDinh", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Tiền ăn một ngày</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "rTienAnMotNgay", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>Tiền ăn một ngày</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "iNuQuanNhan_CanBo", "", "tab-index='-1'")%></div></td>
                                </tr>  
                                <tr>    
                                    <td class="td_form2_td1"><div>Trừ khác</div></td>
                                    <td class="td_form2_td5"><div><%=MyHtmlHelper.TextBox(ParentID, "", "rTruKhac", "", "tab-index='-1'")%></div></td>
                                </tr>
                                <tr>    
                                    <td class="td_form2_td1"><div>&nbsp;</div></td>
                                    <td class="td_form2_td5"><div>&nbsp;</div></td>
                                </tr>  
                                </table>
                        </td>
                    </tr>                        
                </table>
            </div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td width="65%" class="td_form2_td5">
                        &nbsp;
                    </td>
                    <td width="30%" align="right" class="td_form2_td5">
                        <button class="button" tab-index='-1' onclick="Luong_Dialog_btnOK_Click();">OK</button>
                    </td>
                    <td width="5px">
                        &nbsp;
                    </td>
                    <td class="td_form2_td5">
                        <button class="button" tab-index='-1' onclick="Luong_Dialog_btnCancel_Click();">Hủy</button>
                    </td>
                </tr>
            </table>
        </div>        
    </div>
</div>

<script type="text/javascript">
    //onblue_txt('<%=ParentID %>_CanBo_sTen', 'Chọn cán bộ');
    Luong_Dialog_id = '<%=ParentID%>';
</script>
</asp:Content>