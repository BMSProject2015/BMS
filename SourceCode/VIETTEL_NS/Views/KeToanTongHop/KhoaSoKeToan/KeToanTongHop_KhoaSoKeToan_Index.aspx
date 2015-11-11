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
    String ParentID = "KeToanTongHop_KhoaSoKeToan";
    String iThang = Request.QueryString["iThang"];
    String sCheck = Request.QueryString["sCheck"];
    String MaND = User.Identity.Name;
    String iNamLamViec = DateTime.Now.Year.ToString();
    if(iThang == null || iThang == "") iThang = "0";

    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
 
    if (dtCauHinh.Rows.Count > 0 && dtCauHinh != null)
    {
        DataRow R = dtCauHinh.Rows[0];
        iNamLamViec = Convert.ToString(R["iNamLamViec"]);
       // iThang = Convert.ToString(R["iThangLamViec"]);
    }
    if (dtCauHinh != null) dtCauHinh.Dispose();

    DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();

    String strThang0 = "", strThang1 = "", strThang2 = "", strThang3 = "", strThang4 = "", strThang5 = "";
    String  strThang6 = "", strThang7 = "", strThang8 = "", strThang9 = "", strThang10 = "", strThang11 = "", strThang12 = "";
    DataTable dtThangDaCo = KeToanTongHop_KhoaSoKeToanModels.Get_DanhSach_ThangTrongNam(iNamLamViec);
    for (i = 0; i < dtThangDaCo.Rows.Count; i++)
    {
        switch (Convert.ToInt32(dtThangDaCo.Rows[i]["iThang"])) { 
            case 0:
                strThang0 = "checked=\"checked\"";
                break;
            case 1:
                strThang1 = "checked=\"checked\"";
                break;
            case 2:
                strThang2 = "checked=\"checked\"";
                break;
            case 3:
                strThang3 = "checked=\"checked\"";
                break;
            case 4:
                strThang4 = "checked=\"checked\"";
                break;
            case 5:
                strThang5 = "checked=\"checked\"";
                break;
            case 6:
                strThang6 = "checked=\"checked\"";
                break;
            case 7:
                strThang7 = "checked=\"checked\"";
                break;
            case 8:
                strThang8 = "checked=\"checked\"";
                break;
            case 9:
                strThang9 = "checked=\"checked\"";
                break;
            case 10:
                strThang10 = "checked=\"checked\"";
                break;
            case 11:
                strThang11 = "checked=\"checked\"";
                break;
            case 12:
                strThang12 = "checked=\"checked\"";
                break;
        }
    }

    DataTable dt = KeToanTongHop_KhoaSoKeToanModels.Get_DanhSach_All(iNamLamViec, iThang);

    DataTable dtNB = KeToanTongHop_KhoaSoKeToanModels.Get_DanhSach_All_NgoaiBang(iNamLamViec, iThang);
    
    using (Html.BeginForm("DetailSubmit", "KeToanTongHop_KhoaSoKeToan", new { ParentID = ParentID, iNam = iNamLamViec }))
    {
%>
<script type="text/javascript">
<%if (sCheck != null && sCheck != "0")
  { %>
  alert("Tháng <%=iThang %> có chứng từ ghi sổ chưa duyệt!"); 
<%} %>
</script>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Danh sách chứng từ ghi sổ")%>
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
                	<span>Danh sách các tháng đã khóa sổ trong năm <%= iNamLamViec%> &nbsp;(Mã số 35 áp dụng cho khóa sổ tháng 12)</span> 
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 50%; padding: 10px;">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="width: 3%"></td>
                                <td style="width: 17%"></td>
                                <td style="width: 80%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang1" id="<%= ParentID %>_chkThang1" value="1" <%=strThang1 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 1</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang2" id="<%= ParentID %>_chkThang2" value="2" <%=strThang2 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 2</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang3" id="<%= ParentID %>_chkThang3" value="3" <%=strThang3 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 3</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang4" id="<%= ParentID %>_chkThang4" value="4" <%=strThang4 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 4</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang5" id="<%= ParentID %>_chkThang5" value="5" <%=strThang5 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 5</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang6" id="<%= ParentID %>_chkThang6" value="6" <%=strThang6 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 6</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td><input type="checkbox" name="<%= ParentID %>_chkThang0" id="<%= ParentID %>_chkThang0" value="0" <%=strThang0 %> disabled="disabled"/></td>
                                <td>Đầu năm</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 3%"></td>
                                <td style="width: 17%"></td>
                                <td style="width: 80%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang7" id="<%= ParentID %>_chkThang7" value="7" <%=strThang7 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 7</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang8" id="<%= ParentID %>_chkThang8" value="8" <%=strThang8 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 8</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang9" id="<%= ParentID %>_chkThang9" value="9" <%=strThang9 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 9</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang10" id="<%= ParentID %>_chkThang10" value="10" <%=strThang10 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 10</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang11" id="<%= ParentID %>_chkThang11" value="11" <%=strThang11 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 11</td>
                                            <td style="width: 3%"><input type="checkbox" name="<%= ParentID %>_chkThang12" id="<%= ParentID %>_chkThang12" value="12" <%=strThang12 %> disabled="disabled"/></td>
                                            <td style="width: 13%" align="left">Tháng 12</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 50%">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 15px; width: 40%">&nbsp;</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="padding-left: 100px;"><b>Khóa sổ kế toán cho tháng</b></td>
                                <td align="left">
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThangKhoaSo", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr><td align="left" colspan="2" class="td_form2_td1" style="height: 10px;"></td></tr>
                <tr>
                    <td align="right" colspan="2" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <input type="submit" class="button" value="Thực hiện"/>
                                </td>
                                <td style="width: 10px;"></td>
                                <td>
                                    <input type="button" class="button" value="Hủy" onclick="history.go(-1);"/>
                                </td>
                                <td style="width: 10px;"></td>
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
                    <span>Bảng khóa sổ kế toán chi tiết tháng <%=iThang %> năm <%=iNamLamViec %></span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>         
            <th style="width: 5%;" align="center">Tài khoản</th>
            <th style="width: 23%;" align="center">Diễn giải</th>
            <th style="width: 10%;" align="center">Phát sinh nợ</th>
            <th style="width: 10%;" align="center">Phát sinh có</th>
            <th style="width: 10%;" align="center">Lũy kế nợ</th>
            <th style="width: 10%;" align="center">Lũy kế có</th>
            <th style="width: 10%;" align="center">Dư cuối kỳ nợ</th>
            <th style="width: 10%;" align="center">Dư cuối kỳ có</th>
        </tr>
        <%
        String bold = "style = \"padding: 3px 3px;\"";
            
        for (i = 0; i < dtNB.Rows.Count; i++)
        {
            DataRow R1 = dtNB.Rows[i];
            String sTKNo=Convert.ToString(R1["sTKNo"]).Trim();
            if (sTKNo.Length == 3)
            {
                bold = "style = \"padding: 3px 3px; font-weight:bold;\"";
            }
            else
            {
                bold = "style = \"padding: 3px 3px;\"";
            }
            String strDoanTrang = "";
            switch (sTKNo.Length)
            {
                case 4:
                    strDoanTrang = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    break;
                case 5:
                    strDoanTrang = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    break;
            }
            
            String strClass = "";
            if (i % 2 == 0) strClass = "alt";       
            %>
            <tr class="<%=strClass %>">          
                <td align="left" <%=bold %>  onclick="OnInit_CT();">
                   <%=strDoanTrang%> <%= Ajax.ActionLink(sTKNo, "Index", "NhapNhanh", new { id = "KTTH_CHITIETCHUNGTUCHOSOTAIKHOAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iNam = iNamLamViec, iThang = iThang, iID_MaTaiKhoan = R1["sTKNo"] }, new AjaxOptions { }, "")%>
                </td>
                <td align="left"  <%=bold %>><%=R1["sTen"]%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rPS_No"],false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rPS_Co"],false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rLK_No"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rLK_Co"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rCK_No"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rCK_Co"], false)%></td>
            </tr>          
        <%} dtNB.Dispose(); %>
        <%
        Double T_rPS_No = 0, T_rPS_Co = 0, T_rLK_No = 0, T_rLK_Co = 0, T_rCK_No = 0, T_rCK_Co = 0;
        bold = "style = \"padding: 3px 3px;\"";
            
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R1 = dt.Rows[i];
            String sTKNo=Convert.ToString(R1["sTKNo"]).Trim();
            if (sTKNo.Length == 3)
            {
                bold = "style = \"padding: 3px 3px; font-weight:bold;\"";
            }
            else
            {
                bold = "style = \"padding: 3px 3px;\"";
            }
            String strDoanTrang = "";
            switch (sTKNo.Length)
            {
                case 4:
                    strDoanTrang = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    break;
                case 5:
                    strDoanTrang = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    break;
            }
            
            
            String strClass = "";
            if (i % 2 == 0) strClass = "alt"; 
            if(String.IsNullOrEmpty(Convert.ToString(R1["TK_Cha"])))
            {
                if(R1["rPS_No"]!=DBNull.Value)
                    T_rPS_No += Convert.ToDouble(R1["rPS_No"]);
                if (R1["rPS_Co"] != DBNull.Value)
                    T_rPS_Co += Convert.ToDouble(R1["rPS_Co"]);

                if (R1["rLK_No"] != DBNull.Value)
                    T_rLK_No += Convert.ToDouble(R1["rLK_No"]);
                if (R1["rLK_Co"] != DBNull.Value)
                    T_rLK_Co += Convert.ToDouble(R1["rLK_Co"]);
                if (R1["rCK_No"] != DBNull.Value)
                    T_rCK_No += Convert.ToDouble(R1["rCK_No"]);
                if (R1["rCK_Co"] != DBNull.Value)
                    T_rCK_Co += Convert.ToDouble(R1["rCK_Co"]);
            }
            //String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"]}).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
                       
            %>
            <tr class="<%=strClass %>">          
                <td align="left" <%=bold %>  onclick="OnInit_CT();">
                   <%=strDoanTrang%> <%= Ajax.ActionLink(sTKNo, "Index", "NhapNhanh", new { id = "KTTH_CHITIETCHUNGTUCHOSOTAIKHOAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iNam = iNamLamViec, iThang = iThang, iID_MaTaiKhoan = R1["sTKNo"] }, new AjaxOptions { }, "")%>
                </td>
                <td align="left"  <%=bold %>><%=R1["sTen"]%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rPS_No"],false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rPS_Co"],false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rLK_No"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rLK_Co"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rCK_No"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rCK_Co"], false)%></td>
            </tr>          
        <%} %>
            <tr>
                <td colspan="2" align="right"><b>Tổng cộng tài khoản trong bảng: &nbsp;</b></td>
                <td align="right" style="padding: 3px 3px;"><b><%=CommonFunction.DinhDangSo(T_rPS_No, false)%></b></td>
                <td align="right" style="padding: 3px 3px;"><b><%=CommonFunction.DinhDangSo(T_rPS_Co, false)%></b></td>
                <td align="right" style="padding: 3px 3px;"><b><%=CommonFunction.DinhDangSo(T_rLK_No, false)%></b></td>
                <td align="right" style="padding: 3px 3px;"><b><%=CommonFunction.DinhDangSo(T_rLK_Co, false)%></b></td>
                <td align="right" style="padding: 3px 3px;"><b><%=CommonFunction.DinhDangSo(T_rCK_No, false)%></b></td>
                <td align="right" style="padding: 3px 3px;"><b><%=CommonFunction.DinhDangSo(T_rCK_Co, false)%></b></td>
            </tr>
    </table>
</div>
<script type="text/javascript">
    function CallSuccess_CT() {
        $('#dvText').show();
        $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
        $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
        location.reload();
        return false;
    }
    function OnInit_CT() {
        $("#idDialog").dialog("destroy");
        document.getElementById("idDialog").title = '<%=NgonNgu.LayXauChuHoa("Chi tiết tài khoản")%>';
        document.getElementById("idDialog").innerHTML = "";
        $("#idDialog").dialog({
            resizeable: false,
            width: 900,
            modal: true
        });
    }
    function OnLoad_CT(v) {
        document.getElementById("idDialog").innerHTML = v;
    }

    $(document).ready(function () {
        //Hide the div tag when page is loading
        $('#dvText').hide();

        //For Show the div or any HTML element
        $("#btnLuu").click(function () {
            $('#dvText').show();
            $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
            $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
            alert(0);
        });

        //For hide the div or any HTML element
        $("#aHide").click(function () {
            $('#dvText').hide();
        });

        $(window).resize(function () {
            $('.popup_block').css({
                position: 'absolute',
                left: ($(window).width() - $('.popup_block').outerWidth()) / 2,
                top: ($(window).height() - $('.popup_block').outerHeight()) / 2
            });
        });
        // To initially run the function:
        $(window).resize();
        //Fade in Background
    });      
    
    <%if(T_rPS_No != T_rPS_Co) {%>
        alert("Phát sinh có không bằng phát sinh nợ!");
    <%} %>
    <%if(T_rLK_No != T_rLK_Co) {%>
        alert("Lũy kế có không bằng lũy kế nợ!");
    <%} %>
    <%if(T_rCK_No != T_rCK_Co) {%>
        alert("Dư cuối kỳ có không bằng dư cuối kỳ nợ!");
    <%} %>                    
</script>
<div id="idDialog" style="display: none;"></div>
<div id="dvText" class="popup_block">
    <img src="../../../Content/ajax-loader.gif"/><br />
    <p>Hệ thống đang thực hiện yêu cầu...</p>
</div>
</asp:Content>





