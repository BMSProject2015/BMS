<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .div-floatleft
        {
            float:left;    
            max-height:110px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding-top:2px;
            min-width:100px;     
        }
        .div-txt
        {
            padding-top:5px;
            width:250px;            
        }    
        .p
        {
            height:20px;
            line-height:20px;
            padding:2px 4px 2px 2px;     
            text-align:right;                       
        }
    </style>       
</head>
<body>
    <%        
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "ThuNop";
        String Nam = Request.QueryString["Nam"];

        if (String.IsNullOrEmpty(Nam))
        {
            Nam = DateTime.Now.Year.ToString();
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
        DataTable dtDonVi = rptThuNop_BaoCao_QuyetToan_KhoanChi_03Controller.GetDonVi(Nam);
        String maDV=Convert.ToString(Request.QueryString["MaDV"]);
        if (String.IsNullOrEmpty(maDV))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                maDV = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                maDV = Guid.Empty.ToString();
            }
        }
        String[] arrMaDV = maDV.Split(',');
             
        dtDonVi.Dispose();
        String BackURL = Url.Action("Index", "ThuNop_Report");
        using (Html.BeginForm("EditSubmit", "rptThuNop_BaoCao_QuyetToan_KhoanChi_03", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo quyết toán các khoản chi có từ nguồn thu được để lại theo chế độ</span>
                    </td>
                </tr>
            </table>
        </div>     
        <div id="table_form2" class="table_form2">
            <div id="" style="width:600px; margin:0px auto; min-height:10px;">                
                <div class="div-floatleft div-label" style="max-width:160px; text-align:left; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn năm")%></p>                      
                </div>
                <div class="div-floatleft div-txt" style="width:160px;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "iNam", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNamLamViec()\"")%></p>                    
                </div>
                <div class="div-floatleft div-label" style="max-width:160px; text-align:left; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn đơn vị")%></p>                      
                </div>
                <div class="div-floatleft div-txt" style="width:200px; margin:auto">
                    <div style="width: 100%; clear:both; min-height: 10px; max-height:80px; overflow: visible;">
                        <table class="mGrid" style="background-color:#ededed;">
                            <tr style="height: 20px; font-size: 12px;">
                                <td style="width: 20px; text-align:center; height:auto; line-height:7px;">
                                    <input type="checkbox" id="Checkbox1" onclick="ChonDonVi(this.checked)" style="cursor:pointer;" />
                                </td>
                                <td>
                                    Tất cả
                                </td>
                            </tr>
                        </table>                        
                    </div>
                    <div style="width: 100%; clear:both; min-height: 20px; max-height:80px; overflow: auto;" id="<%=ParentID %>_divDonVi"> 
                        <%rptThuNop_BaoCao_QuyetToan_KhoanChi_03Controller rptTB1 = new rptThuNop_BaoCao_QuyetToan_KhoanChi_03Controller();%>                                    
                        <%=rptTB1.obj_DSDonVi(Nam,arrMaDV)%>
                    </div>               
                </div>
            </div>
            <div style="height:5px; clear:both;"></div>
            <div id="both" style="clear:both; height:30px; line-height:30px;">
                <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 5px auto; width:200px;">
                    <tr>
                        <td><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                        <td width="5px"></td>
                        <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                    </tr>
               </table>   
            </div>
        </div>   
    </div>    
         <script type="text/javascript">
             function ChonNamLamViec() {
                 var NamLamViec = document.getElementById("<%=ParentID %>_iNam").value

                 jQuery.ajaxSetup({ cache: false });
                 var url = unescape('<%= Url.Action("ds_DonVi?NamLamViec=#0&arrDV=#1", "rptThuNop_BaoCao_QuyetToan_KhoanChi_03") %>');
                 url = unescape(url.replace("#0", NamLamViec));
                 url = unescape(url.replace("#1", "<%= arrMaDV %>"));
                 $.getJSON(url, function (data) {
                     document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
                 });
             }  
    </script> 
    <script type="text/javascript">
        function ChonDonVi(DonVi) {
            $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
                if (DonVi) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                }
            });
        }                                                   
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                $("#iID_MaDonVi input:checkbox").click(function () {
                    if ($("#Checkbox1").attr('checked') == true && this.checked == false) {
                        $("#Checkbox1").attr('checked', false);
                    }
                });
            });
        </script>
        <%} %>  
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
    </script>     
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThuNop_BaoCao_QuyetToan_KhoanChi_03", new { Nam = Nam, MaDV = maDV }), "ExportToExcel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptThuNop_BaoCao_QuyetToan_KhoanChi_03",new{ Nam=Nam,MaDV=maDV})%>" height="600px" width="100%"></iframe>
    </body>
</html>
