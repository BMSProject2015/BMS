<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
        <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
 <style type="text/css">
     div.login1 {
            text-align : center;    
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 12px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            -webkit-border-radius:2px;
            border-radius:2px;
        }
        div.login1 a:hover
        {
            text-decoration:underline;
            color:#471083;
        }    
        div.login1 a.active {
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
     .style5
     {
         width: 252px;
     }
     .style7
     {
         width: 102px;
     }
     .style8
     {
         width: 168px;
     }
 </style>
</head>
<body>
    
     <%
        
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopChiTieu";

        String MaND = User.Identity.Name;
        //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHePhanBo);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();
         
        String iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        }
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        if (String.IsNullOrEmpty(sLNS))
        {
            sLNS = Convert.ToString(ViewData["sLNS"]);
        }
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
        int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
        DataTable dtNam = new DataTable();
        dtNam.Columns.Add("MaNam", typeof(String));
        dtNam.Columns.Add("TenNam", typeof(String));
        DataRow R;
        for (int i = NamMin; i < NamMax; i++)
        {
            R = dtNam.NewRow();
            dtNam.Rows.Add(R);
            R[0] = Convert.ToString(i);
            R[1] = Convert.ToString(i);
        }
        dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
        dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");

        //Loai ngan sach

        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
        SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "sLNS");
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLoaiNganSach.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLoaiNganSach.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        String[] arrLNS = sLNS.Split(',');

        DataTable dtDotPhanBo = rptTongHopNganSachController.dtDotPhanBo(MaND, sLNS, iID_MaTrangThaiDuyet);
        SelectOptionList slTenDonVi = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            if (dtDotPhanBo.Rows.Count > 0)
            {
                iID_MaDotPhanBo = Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"]);
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
        }
        dtDotPhanBo.Dispose();
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptTongHopNganSach", new { MaND=MaND, sLNS = sLNS, iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        String BackURL = Url.Action("Index", "PhanBo_Report");
        using (Html.BeginForm("EditSubmit", "rptTongHopNganSach", new { ParentID = ParentID}))
        {%>
  <div class="box_tong" style="background-color:#F0F9FE;">
     <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp ngân sách</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
           <div id="Div1">
            <div id="rptMain" style="padding-bottom:5px;padding-top:5px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="29">&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td width="500" rowspan="21" valign="top"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;width:500px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
     <legend><b><%=NgonNgu.LayXau("Chọn Loại ngân sách ") %></b></legend>
     <div style="width: 100%; height: 500px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="checkAll" onclick="Chonall(this.checked)" onchange="ChonLNS()"></td>
                                <td> Chọn tất cả LNS </td>
                                </tr>                                                                                 
                                    <%
                                        String strLNS = "", TenLNS = "";
                                        String _Checked = "checked=\"checked\"";
                                        for (int i = 0; i < dtLoaiNganSach.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        strLNS = Convert.ToString(dtLoaiNganSach.Rows[i]["sLNS"]);
                                        TenLNS = Convert.ToString(dtLoaiNganSach.Rows[i]["TenHT"]);
                                        for (int j = 0; j < arrLNS.Length; j++)
                                        {
                                            if (strLNS == arrLNS[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=strLNS %>"<%=_Checked %> check-group="sLNS" id="sLNS" name="sLNS" onchange="ChonLNS()" />                                      </td>
                                        <td>
                                            <%=TenLNS%>                                       </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div> </fieldset></td>
    <td class="style5">&nbsp;</td>
    <td colspan="2" rowspan="2"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;width:290px;height:50px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
    <legend><b><%=NgonNgu.LayXau("Trạng thái ") %></b></legend>
    <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"size='2' tabindex='-1' onchange=ChonLNS()")%>
    </fieldset></td>
    <td width="20">&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td align="left" class="style7"><b>Đợt phân bổ : </b></td>
    <td id="<%= ParentID %>_tdDotPhanBo" align="left"><%rptTongHopNganSachController rptTB1 = new rptTongHopNganSachController();%>                                    
                                <%=rptTB1.obj_DSDotPhanBo(ParentID,MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet)%> </td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td class="style5">&nbsp;</td>
    <td class="style7">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td colspan="6" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 10px;">
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
                            </table></td>
  </tr>
</table>

</div>
</div>
</div>
<script type="text/javascript">
    $(function () {
        $("div#rptMain").hide();
        $('div.login1 a').click(function () {
            $('div#rptMain').slideToggle('fast');
            $(this).toggleClass('active');
            return false;
        });
    });
    </script>
        <script type="text/javascript">
            function ChonLNS() {
                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var sLNS = "";
                $("input:checkbox[check-group='sLNS']").each(function (i) {
                    if (this.checked) {
                        if (sLNS != "") sLNS += ",";
                        sLNS += this.value;
                    }
                });

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DotPhanBo?ParentID=#0&MaND=#1&sLNS=#2&iID_MaDotPhanBo=#3&iID_MaTrangThaiDuyet=#4","rptTongHopNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%= MaND %>"));
                url = unescape(url.replace("#2", sLNS));
                url = unescape(url.replace("#3", "<%= iID_MaDotPhanBo %>"));
                url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDotPhanBo").innerHTML = data;

                });
            }                                    
    </script>
     <script type="text/javascript">
         function Chonall(sLNS) {
             $("input:checkbox[check-group='sLNS']").each(function (i) {
                 if (sLNS) {
                     this.checked = true;
                 }
                 else {
                     this.checked = false;
                 }

             });
             ChonLNS();
         }                                            
      </script>  
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
  </script>
    <%}%>
   
       <iframe src="<%=URLView%>" height="600px" width="100%">
    </iframe>
</body>
</html>
