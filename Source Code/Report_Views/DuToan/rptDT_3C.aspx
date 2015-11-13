<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
     <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rptDT_3CController.tbTrangThai();
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
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

         
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="0"});

    using (Html.BeginForm("EditSubmit", "rptDT_3C", new { ParentID = ParentID, MaND = MaND,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>BÁO CÁO DỰ TOÁN NGÂN SÁCH QUỐC PHÒNG ( Phần Ngân Sách Cơ Bản )</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                 <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <%--<td class="td_form2_td1" style="text-align:center;40%">
                            <div><b>Chọn năm làm việc :</b>   <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%>
                                 <%= Html.ValidationMessage(ParentID + "_" + "err_iNamLamViec")%>
                            </div>
                        </td>--%>
                        <td class="td_form2_td1" style="text-align:center;width:60%">
                            <div><b>Chọn trạng thái: </b>   <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%>    
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="margin:0 auto;" colspan="2">
                            <table cellpadding="0" cellspacing="0" border="0" style="margin-left:500px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
            </div>
        </div>
    </div>
    <%}
        dtNam.Dispose();    
         %>
 
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=BackURL%>';
          }
    </script> 
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDT_3C", new { MaND = MaND,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet }), "Export to Excel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptDT_3C",new{MaND = MaND,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
</body>
</html>
