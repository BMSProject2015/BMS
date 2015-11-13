<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_PhuCap_MucLucNganSach.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String iID_MaPhuCap = Convert.ToString(ViewData["iID_MaPhuCap"]);
    String MaND = User.Identity.Name;
    String IPSua = Request.UserHostAddress;
    String ParentID = "Edit";
    NameValueCollection data = (NameValueCollection)ViewData["data"];
    DataTable dtLoaiMa = new DataTable();
    dtLoaiMa.Columns.Add("iLoaiMa",typeof(int));
    DataRow R=dtLoaiMa.NewRow();
    R["iLoaiMa"]=0;
    dtLoaiMa.Rows.Add(R);
    R = dtLoaiMa.NewRow();
    R["iLoaiMa"] = 1;
    dtLoaiMa.Rows.Add(R);
    R = dtLoaiMa.NewRow();
    R["iLoaiMa"] = 2;
    dtLoaiMa.Rows.Add(R);
    SelectOptionList slLoaiMa = new SelectOptionList(dtLoaiMa, "iLoaiMa", "iLoaiMa");
    //Cập nhập các thông tin tìm kiếm
    String DSTruong = BangLuongModels.strDSTruong;
    String[] arrDSTruong = DSTruong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }

    Luong_DanhMucPhuCap_MucLucNganSach_BangDuLieu bang = new Luong_DanhMucPhuCap_MucLucNganSach_BangDuLieu(iID_MaPhuCap, arrGiaTriTimKiem, MaND, IPSua);

    String BangID = "BangDuLieu";
    int Bang_Height = 470;
    int Bang_FixedRow_Height = 50;
    //iLoại mã= 0: Mã kiểu AB trong đó B là hệ số; 1: Mã theo kiểu AB, trong đó B*10 là hệ số; 2: Mã cố định;
    using (Html.BeginForm("EditSubmit", "Luong_DanhMucPhuCap", new { ParentID = ParentID, iID_MaPhuCap = iID_MaPhuCap }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%=Html.Hidden(ParentID +"_iID_MaDanhMucPhuCap_MucLucNganSach",data["iID_MaDanhMucPhuCap_MucLucNganSach"]) %>
<% %>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập loại công thức</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table class="mGrid">
                <tr><th colspan="6"><b>Tên trường</b></th></tr>
                <tr style="height:20px;">
                    <td>rLuongCoBan</td>
                    <td>rPhuCap_ChucVu</td>
                    <td>rPhuCap_ThamNien</td>
                    <td>rPhuCap_VuotKhung</td>
                    <td>rLuongToiThieu</td>
                    <td>rPhuCap_BaoLuu</td>                    
                </tr>
                <tr style="height:20px;">
                <td>rPhuCap_AnNinhQuocPhong</td>
                    <td>rPhuCap_DacBiet</td>
                    <td>rPhuCap_TrenHanDinh</td>
                    <td>rPhuCap_NuQuanNhan</td>
                    <td>rPhuCap_Khac</td>                    
                    <td>&nbsp;</td>
                </tr>
                <tr><td colspan="6">&nbsp;</td></tr>
            </table>
            <table cellpadding="0"  cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="50%">
                           <table cellpadding="0"  cellspacing="0" border="0" width="100%">
                              <tr>
                                    <td class="td_form2_td1" ><div>Mã phụ cấp&nbsp;<span  style="color:Red;">*</span></div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "iID_MaPhuCap", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhuCap")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" ><div>Tên phụ cấp&nbsp;<span  style="color:Red;">*</span></div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sTenPhuCap", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTenPhuCap")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" ><div>Hệ số</div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "rHeSo", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_rHeSo")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" ><div>Số tiền</div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "rSoTien", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_rSoTien")%>
                                        </div>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="td_form2_td1" ><div>Trường hệ số bảng lương</div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sMaTruongHeSo_BangLuong", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sMaTruongHeSo_BangLuong")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" ><div>Trường số tiền bảng lương&nbsp;<span  style="color:Red;">*</span></div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sMaTruongSoTien_BangLuong", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sMaTruongSoTien_BangLuong")%>
                                        </div>
                                    </td>
                                </tr>                                                                              
                            </table>
                    </td>
                    <td width="50%">
                         <table cellpadding="0"  cellspacing="0" border="0" width="100%">
                            <tr>
                                    <td class="td_form2_td1" ><div>Loại mã&nbsp;<span  style="color:Red;">*</span></div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID,slLoaiMa, data, "iLoaiMa", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iLoaiMa")%>
                                        </div>
                                    </td>
                                </tr> 
                                <tr>
                                    <td class="td_form2_td1"  colspan="2"><div>Mã kiểu AB trong đó B là hệ số -Loại mã =0: Mã theo kiểu AB, trong đó B*10 là hệ số -Loại mã =1;Mã cố định-Loại mã =2;</div></td>
                                </tr>
                                
                                <tr>
                                    <td class="td_form2_td1" ><div>Độ dài mã tối thiểu&nbsp;<span  style="color:Red;">*</span></div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "iDoDaiMaToiThieu", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iDoDaiMaToiThieu")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" ><div>Luôn có phụ cấp</div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.CheckBox(ParentID, data, "bLuonCo", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_bLuonCo")%>
                                        </div>
                                    </td>
                                </tr>  
                                
                                <tr>
                                    <td class="td_form2_td1" ><div>Có công thức</div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.CheckBox(ParentID, data, "bCongThuc", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_bCongThuc")%>
                                        </div>
                                    </td>
                                </tr> 
                                <tr>
                                    <td class="td_form2_td1" ><div>Được sửa hệ số</div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.CheckBox(ParentID, data, "bDuocSuaHeSo", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_bDuocSuaHeSo")%>
                                        </div>
                                    </td>
                                </tr> 
                                <tr>
                                    <td class="td_form2_td1" ><div>Được sửa chi tiết</div></td>
                                    <td class="td_form2_td5" >
                                       <div>
                                            <%=MyHtmlHelper.CheckBox(ParentID, data, "bDuocSuaChiTiet", "", "style=\"width:98%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_bDuocSuaChiTiet")%>
                                        </div>
                                    </td>
                                </tr> 
                         </table>
                    </td>
                </tr>            
            </table>
            <table cellpadding="0"  cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Công thức&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5" width="90%">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, data, "sCongThuc", "", "style=\"width:98%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sCongThuc")%>
                        </div>
                    </td>
                </tr>                
                <tr>
                    <td colspan="2">
                        
<div class="box_tong">
    <div id="Div1">
        <div id="Div2">
        
            
<%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>    

<div style="display:none;">
<input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
<input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
<%--<input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>" /--%>>
<input type="hidden" id="idBangChiDoc" value="<%=HttpUtility.HtmlEncode(bang.strChiDoc)%>" />
<input type="hidden" id="idXauEdit" value="<%=HttpUtility.HtmlEncode(bang.strEdit)%>" />
<input type="hidden" id="idViewport_N" value="<%=HttpUtility.HtmlEncode(bang.Viewport_N)%>" />
<input type="hidden" id="idNC_Fixed" value="<%=HttpUtility.HtmlEncode(bang.nC_Fixed)%>" />
<input type="hidden" id="idNC_Slide" value="<%=HttpUtility.HtmlEncode(bang.nC_Slide)%>" />

   
        <input type="hidden" id="idAction" name="idAction" value="0" />
        <input type="hidden" id="idXauDuLieuThayDoi" name="idXauDuLieuThayDoi" value="<%=HttpUtility.HtmlEncode(bang.strThayDoi)%>" />
        <input type="hidden" id="idXauLaHangCha" name="idXauLaHangCha" value="<%=HttpUtility.HtmlEncode(bang.strLaHangCha)%>" />
        <input type="hidden" id="idXauMaCacHang" name="idXauMaCacHang" value="<%=HttpUtility.HtmlEncode(bang.strDSMaHang)%>" />
        <input type="hidden" id="idXauMaCacCot" name="idXauMaCacCot" value="<%=HttpUtility.HtmlEncode(bang.strDSMaCot)%>" />
        <input type="hidden" id="idXauGiaTriChiTiet" name="idXauGiaTriChiTiet" value="<%=HttpUtility.HtmlEncode(bang.strDuLieu)%>" />
        <input type="submit" id="btnXacNhanGhi" value="XN" />
        <input type="hidden" id="idXauCacHangDaXoa" name="idXauCacHangDaXoa" value="" />
</div>





        </div>
    </div>
</div>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                  function func_Auto_Complete(ui, item) {
                      document.getElementById('<%=ParentID%>_sTenBacLuong').value = '';
                      document.getElementById('<%=ParentID%>_iID_MaBacLuong').value = '';
                  }
                  function func_Auto_Complete1(ui, item) {
                  }
                  function fnDSGhepMa() {
                      var vR = document.getElementById('<%=ParentID%>_iID_MaNgachLuong').value;
                      return vR;
                  }
              </script>
              
        </div>
    </div>
</div><br />
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <input type="button" id="Button1" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();" value="<%=NgonNgu.LayXau("Thực hiện")%>"/>
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<script type="text/javascript">
    $(document).ready(function () {

        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
        Bang_keys.fnSetFocus(null, null);
    });
</script>
<%
    }
%>
</asp:Content>