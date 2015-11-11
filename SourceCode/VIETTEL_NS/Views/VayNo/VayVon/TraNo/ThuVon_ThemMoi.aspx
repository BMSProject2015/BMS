<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers.DuToan" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        int i;
        string iID_VayChiTiet = Convert.ToString(ViewData["iID_VayChiTiet"]);
        string key = Convert.ToString(ViewData["iID_ThuVonChiTiet"]);
        string iID_ThuVonChiTiet = "";
        if (key != null || key != "") iID_ThuVonChiTiet = key;
        String ParentID = "ThuVon";
        String page = Request.QueryString["page"];
        //Lấy thông tin vay vốn
        DataTable dtVayVon = new DataTable();
        dtVayVon = VayNoModels.getDetailChungTuChitiet(iID_VayChiTiet);
        String sLoaiDuAn = "", MaDonVi = "", TenDonVi = "", NoiDungVay = "", LoaiTinDung = "", NgayVay = "", LaiXuat = "", MienLai = "", DuVonCu = "", DuLaiCu = "", VayTrongThang = "", HanPhaiTra = "", ThoiGianThuHoi = "", ThuVon = "", ThuLai = "", GhiChu = "", TrangThai = "";
        if (dtVayVon.Rows.Count > 0)
        {
            DataRow dr = dtVayVon.Rows[0];
            //NgayChungTu = HamChung.getStringNull(HamChung.ConvertDateTime(dr["dNgayChungTu"]).ToString("dd/MM/yyyy"));

            MaDonVi = Convert.ToString(dr["iID_MaDonVi"]);
            TenDonVi = Convert.ToString(dr["sTen"]);
            NoiDungVay = Convert.ToString(dr["sTenNoiDung"]);
            LoaiTinDung = Convert.ToString(dr["iID_Loai"]);
            NgayVay = HamChung.getStringNull(HamChung.ConvertDateTime(dr["dNgayVay"]).ToString("dd/MM/yyyy"));
            LaiXuat = Convert.ToString(dr["rLaiSuat"]) + " %";
            MienLai = Convert.ToString(dr["bMienLai"]) + " %";
            DuVonCu = CommonFunction.DinhDangSo(Convert.ToString(dr["rDuVonCu"]));
            DuLaiCu = CommonFunction.DinhDangSo(Convert.ToString(dr["rDuLaiCu"]));
            VayTrongThang = CommonFunction.DinhDangSo(Convert.ToString(dr["rVayTrongThang"]));
            HanPhaiTra = HamChung.getStringNull(HamChung.ConvertDateTime(dr["dHanPhaiTra"]).ToString("dd/MM/yyyy"));
            ThoiGianThuHoi = Convert.ToString(dr["rThoiGianThuVon"]) + " tháng";
            ThuVon = CommonFunction.DinhDangSo(Convert.ToString(dr["rThuVon"]));
            ThuLai = CommonFunction.DinhDangSo(Convert.ToString(dr["rThuLai"]));
            GhiChu = Convert.ToString(dr["sGhiChu"]);
            sLoaiDuAn = Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(dr["iID_Loai"])).Rows[0]["sTen"]);
        }

        String sNguoiTra = "", sCMNDNguoiTra = "", rTraVon = "0", rTraLai = "0", sMoTa = "";
        sNguoiTra = Convert.ToString(ViewData["sNguoiTra"]);
        sCMNDNguoiTra = Convert.ToString(ViewData["sCMNDNguoiTra"]);
        rTraVon = Convert.ToString(ViewData["rTraVon"]);
        rTraLai = Convert.ToString(ViewData["rTraLai"]);
        sMoTa = Convert.ToString(ViewData["sMoTa"]);
        String dNgayTra = DateTime.Now.ToString("dd/MM/yyyy");

        DataTable tblDetail = ThuVonModels.getDetail(iID_ThuVonChiTiet);
        if (tblDetail.Rows.Count > 0)
        {
            DataRow DR = tblDetail.Rows[0];
            sNguoiTra = Convert.ToString(DR["sNguoiTra"]);
            sCMNDNguoiTra = Convert.ToString(DR["sCMNDNguoiTra"]);
            rTraVon = Convert.ToString(DR["rThuVon"]);
            rTraLai = Convert.ToString(DR["rThuLai"]);
            sMoTa = Convert.ToString(DR["sMoTa"]);
            dNgayTra = HamChung.ConvertDateTime(DR["dNgayTra"]).ToString("dd/MM/yyyy");
        }
        if (tblDetail != null) tblDetail.Dispose();
        
        
        
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = ThuVonModels.getList(iID_VayChiTiet, CurrentPage, Globals.PageSize);

        double nums = ThuVonModels.getList_Count(iID_VayChiTiet);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { iID_VayChiTiet = iID_VayChiTiet, page = x }));
        String strThemMoi = Url.Action("Edit", "VayVon");
       
    %>
    <% using (Html.BeginForm("ThuVonSummit", "TraNo", new { ParentID = ParentID, iID_VayChiTiet = iID_VayChiTiet }))
       {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_VayChiTiet", iID_VayChiTiet)%>
    <%= Html.Hidden(ParentID + "_iID_ThuVonChiTiet", iID_ThuVonChiTiet)%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>trả vốn vay</span>
                    </td>
                </tr>
            </table>
        </div>

         <div id="nhapform">
        <div id="form2">
        <script type="text/javascript">
            $(document).ready(function () {
                $("#tabs").tabs();
            });    
                </script>
                <div id="tabs">
                    <ul>
                        <li><a href="#tabs-1">Chi tiết trả vốn</a></li>
                     
                        <li><a href="#tabs-2">Thông tin chứng từ vay vốn</a></li>
                    </ul>
                    <div id="tabs-1">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="70%">
                <tr>
                    <td class="td_form2_td1"><div>Ngày trả&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div> 

                        <%=MyHtmlHelper.DatePicker(ParentID, dNgayTra, "dNgayTra", "", "class=\"input1_2\"")%>
                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayTra")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Người trả</div></td>
                    <td class="td_form2_td5">
                        <div> <%=MyHtmlHelper.TextBox(ParentID, sNguoiTra, "sNguoiTra", "", "class=\"input1_2\" tab-index='-1'")%>
                  
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Số CMND người trả</div></td>
                    <td class="td_form2_td5">
                        <div> <%=MyHtmlHelper.TextBox(ParentID, sCMNDNguoiTra, "sCMNDNguoiTra", "", "class=\"input1_2\"")%>

                    
                    
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Trả vốn&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div> <%=MyHtmlHelper.TextBox(ParentID, rTraVon, "rThuVon", "", "class=\"input1_2\"", 1)%>
                          <%= Html.ValidationMessage(ParentID + "_" + "err_rTraVon")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Trả lãi&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, rTraLai, "rThuLai", "", "class=\"input1_2\"", 1)%>
                        </div>
                    </td>
                </tr>
                 <tr>
                    <td class="td_form2_td1"><div>Ghi chú</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "class=\"input1_2\" style=\"resize:none;\"")%>
                        </div>
                    </td>
                </tr>
                 <tr>
                        <td ></td>
                        <td class="td_form2_td5" align="center">
                       
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="Lưu" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
            </table></div>
            <div id="tabs-2" >
              <table cellpadding="5" cellspacing="5" width="100%"  class="table_form3" border="1" style="display: none;">
                    <tr>
                        <td class="td_form2_td1" style="width:25%;">
                            <div>
                                <b>Đơn vị</b>
                            </div>
                        </td>
                        <td class="td_form2_td5" >
                            <div>
                                <%=  MaDonVi + " - " + TenDonVi %>
                            </div>
                        </td>
                       
                    </tr>
                    <tr>
                     <td class="td_form2_td1" >
                            <div>
                                <b>Nội dung vay</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= NoiDungVay%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Loại tín dụng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= sLoaiDuAn%>
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                    <td class="td_form2_td1">
                            <div>
                                <b>Ngày vay</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= NgayVay %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Lãi xuất</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= LaiXuat %>
                            </div>
                        </td>
                       
                    </tr>
                    <tr>
                     <td class="td_form2_td1">
                            <div>
                                <b>Miễn lãi</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= MienLai %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Dư vốn cũ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= DuVonCu %>
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                    <td class="td_form2_td1">
                            <div>
                                <b>Dư lãi cũ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= DuLaiCu %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Vay trong tháng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= VayTrongThang %>
                            </div>
                        </td>
                       
                    </tr>
                    <tr>
                     <td class="td_form2_td1">
                            <div>
                                <b>Hạn phải trả</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= HanPhaiTra %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Thời gian thu hồi vốn</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= ThoiGianThuHoi %>
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                    <td class="td_form2_td1">
                            <div>
                                <b>Thu vốn</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                               <b>  <%= ThuVon %></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Thu lãi</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                              <b>  <%= ThuLai %></b>
                            </div>
                        </td>
                       
                    </tr>
                    <tr>
                     <td class="td_form2_td1">
                            <div>
                                <b>Ghi chú</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= GhiChu %>
                            </div>
                        </td>
                    </tr>
                 
                    <tr>
                        <td colspan="2" style="text-align: center;">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

        
    </div>
       <% } %>
    <br />
<%--    <table>
    <tr>
    <td><b>Hiển thị chi tiết thông tin vay vốn &nbsp;</b></td>
    <td>
     <div><%=MyHtmlHelper.CheckBox(ParentID, "", "iThemMoi", "", "onclick=\"CheckThemMoi(this.checked)\"")%></div></td>
    </tr>
    </table>
        <div class="box_tong" id="tb_DotNganSach">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>
                            <%=NgonNgu.LayXau("chi tiết vay vốn")%>
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
           
              
               
        </div>
    </div>--%>
    <br />
    <div class="box_tong">
                    <div class="title_tong">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <span>Danh sách trả vốn</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table class="mGrid">
                        <tr>
                            <th style="width: 3%;" align="center">
                                STT
                            </th>
                            <th style="width: 10%;" align="center">
                                Ngày trả
                            </th>
                            <th  align="center">
                                Người trả
                            </th>
                            <th style="width: 20%;" align="center">
                                Số CMND người trả
                            </th>
                            <th style="width: 15%;" align="center">
                                Trả vốn
                            </th>
                            <th style="width: 15%;" align="center">
                                Trả lãi
                            </th>
                            <th style="width: 5%;" align="center">
                                Sửa
                            </th>
                            <th style="width: 5%;" align="center">
                                Xóa
                            </th>
                        </tr>
                       <%
                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];                 
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    string strEdit = string.Empty;
                    string strDelete = string.Empty;
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Detail_Edit", "TraNo", new { iID_VayChiTiet = R["iID_VayChiTiet"], iID_ThuVonChiTiet = R["iID_ThuVonChiTiet"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "EditNoiDung", "");
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Deleted", "TraNo", new { iID_VayChiTiet = R["iID_ThuVonChiTiet"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "DeleteNoiDung", "");
                   
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
            
                <td align="center">
                    <%= HamChung.ConvertDateTime( R["dNgayTra"]).ToString("dd/MM/yyyy")%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(R["sNguoiTra"])%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(R["sCMNDNguoiTra"])%>
                </td>
                  <td align="right">
                  <b>
                    <%=CommonFunction.DinhDangSo( HttpUtility.HtmlEncode(R["rThuVon"]))%></b>
                </td>
                <td align="right">
                   <b>   <%=CommonFunction.DinhDangSo(HttpUtility.HtmlEncode(R["rThuLai"]))%></b>
                </td>



                <td align="center">
                    <%=strEdit%>
                </td>
                <td align="center">
                    <%=strDelete%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="10" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
                        
                        </table></div>
 
<%--    <script type="text/javascript">
        CheckThemMoi(false);
        function CheckThemMoi(value) {
            if (value == true) {
                document.getElementById('tb_DotNganSach').style.display = ''
            } else {
                document.getElementById('tb_DotNganSach').style.display = 'none'
            }
        }        

    </script>--%>
</asp:Content>
