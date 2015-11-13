
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<%
    String ParentID = "BaoCao";
    String MaND = User.Identity.Name;
    String DSMaTaiKhoan = Convert.ToString(ViewData["DSMaTaiKhoan"]);
    if (String.IsNullOrEmpty(DSMaTaiKhoan))
        DSMaTaiKhoan = "-1,";
    String[] arrMaTaiKhoan = DSMaTaiKhoan.Split(',');
    int ChiSo = Convert.ToInt16(ViewData["ChiSo"]);
    String iID_MaTaiKhoan = arrMaTaiKhoan[ChiSo];
    if (ChiSo + 1 < arrMaTaiKhoan.Length)
    {
        iID_MaTaiKhoan = arrMaTaiKhoan[ChiSo];
        ChiSo = ChiSo + 1;
    }
    else
    {
        ChiSo = 0;
    }
    String sLoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
    String iNgay1 = Convert.ToString(ViewData["iNgay1"]);
    String iNgay2 = Convert.ToString(ViewData["iNgay2"]);      
    String iThang1 = Convert.ToString(ViewData["iThang1"]);
    String iThang2 = Convert.ToString(ViewData["iThang2"]);
    String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);    
    String BackURL = Url.Action("Index", "rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc");
    using (Html.BeginForm("EditSubmit", "rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc", new { ParentID = ParentID, iNamLamViec = iNamLamViec, iNgay1 = iNgay1, iNgay2 = iNgay2, iThang1 = iThang1, iThang2 = iThang2, ChiSo = ChiSo }))
      { 
%>
  <%=MyHtmlHelper.Hidden(ParentID, DSMaTaiKhoan, "iID_MaTaiKhoan", "")%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1">
        <div id="Div2">
            <table  width="100%" cellpadding="0" cellspacing="0" border="0">
              <tr>
                              
                    <td align="right">
                    <div>
                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                        
                        </div>
                    </td>
                    <td width="5px">&nbsp;</td>
                    <td align="left"><div> <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" /></div></td>
                </tr>
            </table>
        </div>
    </div>
</div>
        <script type="text/javascript">
            function CheckAll(value) {
                $("input:checkbox[check-group='iID_MaTaiKhoan']").each(function (i) {
                    this.checked = value;
                });
            }                                            
    </script>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=BackURL %>';
         }
 </script>
<%} %>
<%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc", new { iNamLamViec = iNamLamViec, iNgay1 = iNgay1, iNgay2 = iNgay2, iThang1 = iThang1, iThang2 = iThang2, LoaiBaoCao = sLoaiBaoCao, iID_MaTaiKhoan = iID_MaTaiKhoan, UserName = MaND }), "Export To Excel")%>
<div>
    <iframe src="<%=Url.Action("ViewPDF","rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc",new{iNamLamViec = iNamLamViec, iNgay1 = iNgay1, iNgay2 = iNgay2,iThang1=iThang1,iThang2=iThang2,LoaiBaoCao=sLoaiBaoCao,iID_MaTaiKhoan=iID_MaTaiKhoan,UserName=MaND})%>"
        height="600px" width="100%"></iframe>
</div>

