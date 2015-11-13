<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        .style1
        {
            width: 304px;
        }
        .style2
        {
            width: 147px;
        }
        .style3
        {
            width: 185px;
        }
        .style4
        {
            width: 244px;
        }
    </style>
</head>
<body>
    <%
        String ParentID = "QuyetToan";

        String BackURL = Url.Action("Index", "QuyetToan_QuanSo_Report");
        String URLView = "";
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        String iTuThang = Convert.ToString(ViewData["iTuThang"]);
        String iDenThang = Convert.ToString(ViewData["iDenThang"]);
        Boolean bBq = Convert.ToBoolean(ViewData["bBq"]);
        DataTable dtThang = new DataTable();
        dtThang.Columns.Add("Id", typeof(string));
        dtThang.Columns.Add("Text", typeof(string));
        for (int i = 0; i <= 12; i++)
        {
            dtThang.Rows.Add(i.ToString(), i.ToString());
        }
        SelectOptionList slThang = new SelectOptionList(dtThang, "Id", "Text");
        dtThang.Dispose();

        int SoCot = 1;
        //dt Loại ngân sách
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        //DataTable dtDonVi = DuToan_ReportModels.dtDonVi(MaND, "1010000");

        //if (String.IsNullOrEmpty(iID_MaDonVi))
        //{
        //    if (dtDonVi.Rows.Count > 0)
        //    {
        //        iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
        //    }
        //    else
        //    {
        //        iID_MaDonVi = Guid.Empty.ToString();
        //    }
        //}
        //dtDonVi.Dispose();

        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan_QuanSo(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();

        String[] arrMaDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrMaDonVi.Length];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(iID_MaDonVi) && bBq==false) PageLoad = "0";
        if (PageLoad == "1")
        {
            if (bBq)
            {
                URLView = Url.Action("ViewPDF", "rptQTQS_Thang",
                                     new
                                         {
                                             MaND = MaND,
                                             iID_MaPhongBan = iID_MaPhongBan,
                                             bBq = bBq,
                                             iTuThang = iTuThang,
                                             iDenThang = iDenThang,
                                             iID_MaDonVi = iID_MaDonVi
                                         });
            }
            else
            {
                for (int i = 0; i < arrMaDonVi.Length; i++)
                {
                    arrView[i] =
                        String.Format(
                            @"/rptQTQS_Thang/viewpdf?iID_MaDonVi={0}&MaND={1}&iTuThang={2}&iDenThang={3}&bBq={4}&iID_MaPhongBan={5}",
                            arrMaDonVi[i], MaND, iTuThang, iDenThang, bBq, iID_MaPhongBan);
                    Chuoi += arrView[i];
                    if (i < arrMaDonVi.Length - 1)
                        Chuoi += "+";
                }
            }
        }
        using (Html.BeginForm("EditSubmit", "rptQTQS_Thang", new { ParentID = ParentID }))
        {
    %>
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo tổng hợp quân số năm
                        <%=iNamLamViec %>
                    </span>
                </td>
            </tr>
        </table>
    </div>
    <table style="width: 100%;">
        <tr>
            <td class="style3">
                <div>
                    <b>Từ Tháng: </b>
                </div>
            </td>
            <td class="style4">
                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iTuThang, "iTuThang", "", "")%>
            </td>
            <td class="style2">
                &nbsp; &nbsp; &nbsp; Chọn đơn vị
            </td>
            <td rowspan="11">
                &nbsp; &nbsp; &nbsp;
                <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 205px; width: 271px;">
                </div>
            </td>
        </tr>
        <tr>
            <td class="style3">
                <div style="width: 194px">
                    <b>Đến Tháng: </b>
                </div>
            </td>
            <td class="style4">
                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iDenThang, "iDenThang", "", "")%>
            </td>
            <td class="style2">
            </td>
        </tr>
        <tr>
            <td class="style3">
                <div style="width: 193px">
                    <b>Chọn phòng ban: </b>
                </div>
            </td>
            <td class="style4">
                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\" onchange=Chon()")%>
            </td>
            <td class="style2" rowspan="9">
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                <input id="chkBinhQuan" type="checkbox" name="chkBinhQuan" />Tổng hợp quân số bình
                quân
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="td_form2_td1" style="margin: 0 auto;" colspan="2">
                <table cellpadding="0" cellspacing="0" border="0" style="margin-left: 45%;">
                    <tr>
                        <td>
                            <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTongHop_QuanSo_QuyetToan", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi }), "Export to Excel")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
<script type="text/javascript">
            Chon();
            $(document).ready(function () {
                   $('.title_tong a').click(function () {
                    $('div#rptMain').slideToggle('normal');
                    $(this).toggleClass('active');
                    return false;
                });

               var bBinhQuan = '<%=bBq %>';
               if(bBinhQuan.toLowerCase()==='true') {
               console.log($("input#chkBinhQuan"));
               $("input#chkBinhQuan").attr('checked','on');
               }

              
            });
            var count = <%=arrView.Length%>;
                var Chuoi = '<%=Chuoi%>';
                var Mang=Chuoi.split("+");
                   var pageLoad = <%=PageLoad %>;
                   var bBinhQuan = '<%=bBq %>';
                   if(pageLoad=="1" &&bBinhQuan.toLowerCase()==='false') {
                var siteArray = new Array(count);
                for (var i = 0; i < count; i++) {
                    siteArray[i] = Mang[i];
                }
                    for (var i = 0; i < count; i++) {
                        window.open(siteArray[i], '_blank');
                    }
                } 
            
            function Chon() {
               var Phongban = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;
               var TuThang=document.getElementById("<%= ParentID %>_iTuThang").value;
                var DenThang=document.getElementById("<%= ParentID %>_iDenThang").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&PhongBan=#1&TuThang=#2&DenThang=#3&iID_MaDonVi=#4", "rptQTQS_Thang") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", Phongban));
                url = unescape(url.replace("#2", TuThang));
                url = unescape(url.replace("#3", DenThang));
                url = unescape(url.replace("#4","<%= iID_MaDonVi %>"));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });

                
            }
            
             function CheckAll(value) {
                  $("input:checkbox[check-group='DonVi']").each(function (i) {
                      this.checked = value;
                  });
              }
                      
                  $(document).ready(function(){
                   var maDonVi='<%=iID_MaDonVi %>';
                var arrchk_donvi=maDonVi.split(",");
                $("input:checkbox[check-group='DonVi']").each(function (i) {
                 if (arrchk_donvi.indexOf($(this).val())>=0) {
               $(this).attr('checked','on');
                }
                
             });
                  });                                   
</script>
