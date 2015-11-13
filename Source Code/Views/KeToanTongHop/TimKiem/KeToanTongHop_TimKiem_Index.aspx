<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        int i;
        String ParentID = "KeToanTongHop_ChungTu";
        String MaND = User.Identity.Name;
        String iSoChungTu = Request.QueryString["iSoChungTu"];
        String iNgayCT = Request.QueryString["iNgayCT"];
        String iThangCT = Request.QueryString["iThangCT"];
        String iNgay = Request.QueryString["iNgay"];
        String iThang = Request.QueryString["iThang"];


        String iDenNgayCT = Request.QueryString["iDenNgayCT"];
        String iDenThangCT = Request.QueryString["iDenThangCT"];
        String iDenNgay = Request.QueryString["iDenNgay"];
        String iDenThang = Request.QueryString["iDenThang"];


        String sSoTienTu = Request.QueryString["sSoTienTu"];
        String sSoTienDen = Request.QueryString["sSoTienDen"];
        String sTaiKhoanNo = Request.QueryString["sTaiKhoanNo"];
        String sTaiKhoanCo = Request.QueryString["sTaiKhoanCo"];
        String sDonViNo = Request.QueryString["sDonViNo"];
        String sDonViCo = Request.QueryString["sDonViCo"];
        String sNoiDung = Request.QueryString["sNoiDung"];
        String sNguoiTao = Request.QueryString["sNguoiTao"];
        String sChiTietCo = Request.QueryString["sChiTietCo"];
        String sChiTietNo = Request.QueryString["sChiTietNo"];
        String sBNo = Request.QueryString["sBNo"];
        String sBCo = Request.QueryString["sBCo"];
        String page = Request.QueryString["page"];
        if (String.IsNullOrEmpty(sNguoiTao) && !LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
        {
            sNguoiTao = MaND;

        }
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        DataTable dtNgay = DanhMucModels.DT_Ngay();
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        dtNgay.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        DataTable dt = KeToanTongHop_ChungTuChiTietModels.Get_DanhSachChungTuChiTiet(MaND, iSoChungTu, iNgayCT, iThangCT,
                                                                                     iNgay, iThang, sSoTienTu,
                                                                                     sSoTienDen, sTaiKhoanNo,
                                                                                     sTaiKhoanCo, sDonViNo, sDonViCo,
                                                                                     sNoiDung, iDenNgayCT, iDenThangCT,
                                                                                     iDenNgay, iDenThang, sNguoiTao,
                                                                                     sChiTietCo, sChiTietNo,
                                                                                     sBNo, sBCo, CurrentPage,
                                                                                     Globals.PageSize);

        double nums = KeToanTongHop_ChungTuChiTietModels.Get_DanhSachChungTuChiTiet_Count(MaND, iSoChungTu, iNgayCT,
                                                                                          iThangCT,
                                                                                          iNgay, iThang, sSoTienTu,
                                                                                          sSoTienDen, sTaiKhoanNo,
                                                                                          sTaiKhoanCo, sDonViNo,
                                                                                          sDonViCo, sNoiDung, iDenNgayCT,
                                                                                          iDenThangCT,
                                                                                          iDenNgay, iDenThang, sNguoiTao,
                                                                                          sChiTietCo, sChiTietNo,
                                                                                          sBNo, sBCo);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new
        {
            MaND = MaND,
            iSoChungTu = iSoChungTu,
            iNgayCT = iNgayCT,
            iThangCT = iThangCT,
            iNgay = iNgay,
            iThang = iThang,
            sSoTienTu = sSoTienTu,
            sSoTienDen = sSoTienDen,
            sTaiKhoanNo = sTaiKhoanNo,
            sTaiKhoanCo = sTaiKhoanCo,
            sDonViNo = sDonViNo,
            sDonViCo = sDonViCo,
            sNoiDung = sNoiDung,
            iDenNgayCT = iDenNgayCT,
            iDenThangCT = iDenThangCT,
            iDenNgay = iDenNgay,
            iDenThang = iDenThang,
            sNguoiTao = sNguoiTao,
            sChiTietCo = sChiTietCo,
            sChiTietNo = sChiTietNo,
            sBNo = sBNo,
            sBCo = sBCo,
            page = x
        }));

        using (Html.BeginForm("SearchSubmit", "KeToanTongHop_TimKiem", new { ParentID = ParentID }))
        {
    %>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop_ChungTu"), "Danh sách chứng từ ghi sổ")%>
                </div>
            </td>
             <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông tin tìm kiếm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Số chứng từ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, iSoChungTu, "iSoChungTu", "", "class=\"input1_2\"",2)%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Nội dung chứng từ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextArea(ParentID,sNoiDung,"sNoiDung","","class=\"input1_2\" style=\"height:50px;resize:none;\" ") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Số tiền từ / đến</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sSoTienTu, "iSoTienTu", "", "class=\"input1_2\" style=\"width:40%;\"",1)%>
                                            /
                                            <%=MyHtmlHelper.TextBox(ParentID, sSoTienDen, "iSoTienDen", "", "class=\"input1_2\" style=\"width:40%;\"", 1)%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Người lập chứng từ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sNguoiTao, "sNguoiTao", "", "class=\"input1_2\" style=\"width:40%;\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Thuộc BQL nợ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sBNo, "sBNo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Thuộc BQL có</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sBCo, "sBCo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày/Tháng chứng từ ghi sổ từ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNgay, Convert.ToString(iNgayCT) , "iNgayCT", "", "class=\"input1_2\" style=\"width:20%;\"")%>/
                                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, Convert.ToString(iThangCT), "iThangCT", "", "class=\"input1_2\" style=\"width:20%;\" onchange=\"ChonThang(this.value,3)\"")%>
                                            <b>đến</b>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNgay, Convert.ToString(iDenNgayCT), "iDenNgayCT", "", "class=\"input1_2\" style=\"width:20%;\"")%>/
                                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, Convert.ToString(iDenThangCT), "iDenThangCT", "", "class=\"input1_2\" style=\"width:20%;\" onchange=\"ChonThang(this.value,4)\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày/Tháng chứng từ phát sinh từ/ đến</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNgay, Convert.ToString(iNgay), "iNgay", "", "class=\"input1_2\" style=\"width:20%;\"")%>/
                                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, Convert.ToString(iThang), "iThang", "", "class=\"input1_2\" style=\"width:20%;\" onchange=\"ChonThang(this.value,1)\"")%>
                                            <b>đến</b>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNgay, Convert.ToString(iDenNgay), "iDenNgay", "", "class=\"input1_2\" style=\"width:20%;\"")%>/
                                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, Convert.ToString(iDenThang), "iDenThang", "", "class=\"input1_2\" style=\"width:20%;\" onchange=\"ChonThang(this.value,2)\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tài khoản nợ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTaiKhoanNo, "sTaiKhoanNo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tài khoản có</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTaiKhoanCo, "sTaiKhoanCo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Thuộc đơn vị nợ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sDonViNo, "sDonViNo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Thuộc đơn vị có</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sDonViCo, "sDonViCo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Chi tiết TK nợ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sChiTietNo, "sChiTietNo", "", "class=\"input1_2\" style=\"width:35%;\"")%>
                                            <b>Chi tiết TK có</b>
                                            <%=MyHtmlHelper.TextBox(ParentID, sChiTietCo, "sChiTietCo", "", "class=\"input1_2\" style=\"width:35%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" class="td_form2_td1" style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Tìm kiếm" />
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="Hủy" onclick="history.go(-1);" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%  } %>
    <br />
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách chứng từ &nbsp;(<%=nums%>
                            chứng từ)</span>
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 3%;" align="center">
                    Số GS
                </th>
                <th style="width: 3%;" align="center">
                    Ngày
                </th>
                <th style="width: 3%;" align="center">
                    Tháng
                </th>
                <th style="width: 5%;" align="center">
                    Số CT
                </th>
                <th style="width: 25%;" align="center">
                    Nội dung
                </th>
                <th style="width: 20px;" align="center">
                    Số tiền
                </th>
                <th style="width: 4%;" align="center">
                    ĐK Có
                </th>
                <th align="center">
                    Ch.tiết có
                </th>
                <th style="width: 4%;" align="center">
                    B có
                </th>
                <th style="width: 4%;" align="center">
                    Đ.vị Có
                </th>
                <th style="width: 4%;" align="center">
                    ĐK Nợ
                </th>
                <th align="center">
                    Ch.tiết nợ
                </th>
                <th style="width: 4%;" align="center">
                    B.Nợ
                </th>
                <th style="width: 4%;" align="center">
                    Đ.vị Nợ
                </th>
                <th align="center">
                    Ghi chú
                </th>
                <th align="center">
                    Người tạo
                </th>
                <th style="width: 10px;" align="center">
                    Sửa
                </th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String sClasstr = "", sSoGhiSo = "";
                    int STT = i + 1;

                    NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(Convert.ToString(R["iID_MaChungTu"]));
                    String sTienToChungTu = data["sTienToChungTu"];
                    String sSoChungTuGS = data["iSoChungTu"];
                    sSoGhiSo = sTienToChungTu + sSoChungTuGS;
                    sSoGhiSo = data["sSoChungTu"];
                    String urlDetail = Url.Action("Index", "KeToanTongHop", new { iID_MaChungTu = R["iID_MaChungTu"], iThang = R["iThang"], iID_MaChungTuChiTiet = R["iID_MaChungTuChiTiet"] });
                    if (i % 2 == 0) sClasstr = "alt";
                    string rSoTien = "";
                    double SoTien = double.Parse(HamChung.ConvertToString(R["rSoTien"].ToString()));
                    if (SoTien<0)
                    {
                        rSoTien = "-" + CommonFunction.DinhDangSo(R["rSoTien"]);
                    }
                    else
                    {
                        rSoTien = CommonFunction.DinhDangSo(R["rSoTien"]);
                    }
            %>
            <tr class='<%=sClasstr %>'>
                <td style="padding: 3px 2px;" align="center">
                    <%=STT%>
                </td>
                <td style="padding: 3px 2px;" align="center">
                    <%=MyHtmlHelper.ActionLink(urlDetail,HttpUtility.HtmlEncode( sSoGhiSo))%>
                </td>
                <td style="padding: 3px 2px;" align="center">
                    <%=R["iNgay"]%>
                </td>
                <td style="padding: 3px 2px;" align="center">
                    <%=R["iThang"]%>
                </td>
                <td style="padding: 3px 2px;" align="left">
                    <%=HttpUtility.HtmlEncode(R["sSoChungTuChiTiet"])%>
                </td>
                <td style="padding: 3px 2px;" align="left">
                    <%=HttpUtility.HtmlEncode(R["sNoiDung"])%>
                </td>
                <td style="padding: 3px 2px;" align="right">
                    <%=rSoTien%>
                </td>
                <td style="padding: 3px 2px;" align="center">
                    <%=HttpUtility.HtmlEncode(R["iID_MaTaiKhoan_Co"])%>
                </td>
                <td style="padding: 3px 2px;" align="left">
                    <%=HttpUtility.HtmlEncode(R["sTenTaiKhoanGiaiThich_Co"])%>
                </td>
                  <td style="padding: 3px 2px;" align="center">
                    <%=HttpUtility.HtmlEncode(R["iID_MaPhongBan_Co"])%>
                </td>
                <td style="padding: 3px 2px;" align="center">
                    <%=HttpUtility.HtmlEncode(R["iID_MaDonVi_Co"])%>
                </td>
              
                <td style="padding: 3px 2px;" align="center">
                    <%=HttpUtility.HtmlEncode(R["iID_MaTaiKhoan_No"])%>
                </td>
                <td style="padding: 3px 2px;" align="left">
                    <%=HttpUtility.HtmlEncode(R["sTenTaiKhoanGiaiThich_No"])%>
                </td>
                 <td style="padding: 3px 2px;" align="center">
                    <%=HttpUtility.HtmlEncode(R["iID_MaPhongBan_No"])%>
                </td>
                <td style="padding: 3px 2px;" align="center">
                    <%=HttpUtility.HtmlEncode(R["iID_MaDonVi_No"])%>
                </td>
               
                <td style="padding: 3px 2px;" align="left">
                    <%=HttpUtility.HtmlEncode(R["sGhiChu"])%>
                </td>
                <td style="padding: 3px 2px;" align="left">
                    <%=HttpUtility.HtmlEncode(HamChung.ConvertToString( R["sID_MaNguoiDungTao"]))%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(urlDetail, "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="18" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
    <%  
        dt.Dispose();
    %>
      <script type="text/javascript">
          function ChonThang(iThang, loai) {

              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iThang=#2&iNgay=#3","KeToanTongHop_TimKiem") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", "<%= MaND %>"));
              url = unescape(url.replace("#2", iThang));
              url = unescape(url.replace("#3", "<%= iNgay %>"));
              $.getJSON(url, function(data) {
                  if (loai == 1) {
                      document.getElementById("<%= ParentID%>_iNgay").innerHTML = data;
                  }
                  else if (loai == 2) {
                      document.getElementById("<%= ParentID%>_iDenNgay").innerHTML = data;
                  }
                  else if (loai == 3) {
                      document.getElementById("<%= ParentID%>_iNgayCT").innerHTML = data;
                  }
                  else if (loai == 4) {
                      document.getElementById("<%= ParentID%>_iDenNgayCT").innerHTML = data;
                  }
              });

          }                                            
    </script>
</asp:Content>
