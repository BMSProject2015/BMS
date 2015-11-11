<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <%
        String ParentID = "DuToan";
        String iID_MaChungTu = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
        int SoCot = 1;
        //dt don vi
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = DuToan_ReportModels.dtDonVi_ChungTu(iID_MaChungTu);

        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtDonVi.Dispose();
        String[] arrMaDonVi = iID_MaDonVi.Split(',');
        
        
        //dtKieu xem
        String sKieuXem = Convert.ToString(ViewData["sKieuXem"]);
        DataTable dtKieuXem = DuToan_ReportModels.dtKieuXem();
        SelectOptionList slKieuXem= new SelectOptionList(dtKieuXem,"iID","sTen");
        if (String.IsNullOrEmpty(sKieuXem))
        {
            if (dtKieuXem.Rows.Count > 0)
            {
                sKieuXem = Convert.ToString(dtKieuXem.Rows[0]["iID"]);
            }
            else
            {
                sKieuXem = Guid.Empty.ToString();
            }
        }
        dtKieuXem.Dispose();

        //iDonViTinh
        String iDonViTinh = Convert.ToString(ViewData["iDonViTinh"]);
        DataTable dtDonViTinh = DuToan_ReportModels.dtDonViTinh();
  SelectOptionList slDonViTinh= new SelectOptionList(dtDonViTinh,"iID","sTen");
        if (String.IsNullOrEmpty(iDonViTinh))
        {
            if (dtDonViTinh.Rows.Count > 0)
            {
                iDonViTinh = Convert.ToString(dtDonViTinh.Rows[0]["iID"]);
            }
            else
            {
                iDonViTinh = Guid.Empty.ToString();
            }
        }
        dtDonViTinh.Dispose();
        
        
        
        using (Html.BeginForm("EditSubmit", "rptDuToan_BieuKiem_1010000", FormMethod.Post, new { target = "_blank" }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td class="td_form2_td1" style="width: 15%">
                <div>
                    Chọn kiểu xem:</div>
            </td>
            <td class="td_form2_td1" style="width: 20%">
                <div>
                   <%=MyHtmlHelper.DropDownList(ParentID,slKieuXem,sKieuXem,"sKieuXem","","class=\"input1_2\" style=\"width: 100%\"") %></div>
            </td>
            <td class="td_form2_td1" style="width: 20%">
                <div>
                    Chọn đơn vị:</div>
            </td>
            <td style="width: 50%" rowspan="2">
                <div style="overflow: scroll; height: 400px">
                    <table class="mGrid" style="width: 100%">
                        <tr>
                            <th align="center" style="width: 40px;">
                                <input type="checkbox" id="abc" onclick="CheckAllDV(this.checked)" />
                            </th>
                            <%for (int c = 0; c < SoCot * 2 - 1; c++)
                              {%>
                            <th>
                            </th>
                            <%} %>
                        </tr>
                        <%
                
String strsTen = "", MaDonVi = "", strChecked = "";
for (int i = 0; i < dtDonVi.Rows.Count; i = i + SoCot)
{
                    
                    
                        %>
                        <tr>
                            <%for (int c = 0; c < SoCot; c++)
                              {
                                  if (i + c < dtDonVi.Rows.Count)
                                  {
                                      strChecked = "";
                                      strsTen = Convert.ToString(dtDonVi.Rows[i + c]["TenHT"]);
                                      MaDonVi = Convert.ToString(dtDonVi.Rows[i + c]["iID_MaDonVi"]);
                                      for (int j = 0; j < arrMaDonVi.Length; j++)
                                      {
                                          if (MaDonVi.Equals(arrMaDonVi[j]))
                                          {
                                              strChecked = "checked=\"checked\"";
                                              break;
                                          }
                                      }
                            %>
                            <td align="center" style="width: 40px;">
                                <input type="checkbox" value="<%=MaDonVi %>" <%=strChecked %> check-group="DonVi"
                                    id="iID_MaDonVi" name="iID_MaDonVi" />
                            </td>
                            <td align="left">
                                <%=strsTen%>
                            </td>
                            <%} %>
                            <%} %>
                        </tr>
                        <%}%>
                    </table>
                </div>
            </td>
            <td class="td_form2_td1">
            </td>
        </tr>
        <tr>
             <td class="td_form2_td1" >
                <div>
                    Chọn ĐVT:</div>
            </td>
            <td class="td_form2_td1" ">
                <div>
                   <%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDonViTinh, "iDonViTinh", "", "class=\"input1_2\" style=\"width: 100%\"")%></div>
            </td>
            <td style="width: 20%" class="td_form2_td1">
            </td>
            <td class="td_form2_td1" style="width: 10%">
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div style="margin-top: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 40%">
                            </td>
                            <td align="right">
                                <input type="submit" class="button" value="Tiếp tục" />
                            </td>
                            <td style="width: 1%">
                                &nbsp;
                            </td>
                            <td align="left">
                                <input type="button" class="button" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                            </td>
                            <td style="width: 40%">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <script type="text/javascript">
            function CheckAllDV(value) {
                $("input:checkbox[check-group='DonVi']").each(function (i) {
                    this.checked = value;
                });
            }                                            
        </script>
    </table>
    <%} %>
</body>
</html>
