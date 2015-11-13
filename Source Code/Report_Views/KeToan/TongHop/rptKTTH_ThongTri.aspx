<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "QuyetToanNganSach";
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        //String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String iID_MaDonVi = "";
        String iThang = Request.QueryString["iThang"];

        String iNam = Request.QueryString["iNam"];
        String LoaiTK = "0";
        //String LoaiTK = Request.QueryString["LoaiTK"];
        String UserID = User.Identity.Name;
        DataTable dtPhongBan = DonViModels.DanhSach_DonVi_ChungTu_KeToan(iID_MaChungTu);
        if (dtPhongBan.Rows.Count > 0)
            iID_MaDonVi = Convert.ToString(dtPhongBan.Rows[0]["iID_MaDonVi"].ToString());
        else
            iID_MaDonVi = Guid.Empty.ToString();
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaDonVi", "sTen");

        //Lay ds Loại thông tri
        DataTable dtLoaiThongTri = rptKTTongHop_ThongTri_PhongBanController.getDSLoaiThongTri();
        String iID_MaThongTri = Request.QueryString["iID_MaThongTri"];
        if (String.IsNullOrEmpty(iID_MaThongTri))
        {
            if (dtLoaiThongTri.Rows.Count > 0)
                iID_MaThongTri = Convert.ToString(dtLoaiThongTri.Rows[0]["iID_MaThongTri"]);
        }
        String UrlEditThongTri = Url.Action("Index", "LoaiThongTri");
        using (Html.BeginForm("EditSubmit", "rptKTTongHop_ThongTri_PhongBan", new { ParentID = ParentID, iThang = iThang, iNam = iNam, iID_MaThongTri = iID_MaThongTri }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
                <div>
                    <div style="width: 40%; float: left;">
                        <fieldset style="min-height: 320px;">
                            <legend><b>&nbsp;
                                <%=NgonNgu.LayXau("Chọn đơn vị cần in thông tri")%></b></legend>
                            <div style="float: left; width: 98%; margin: 3px;">
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\" size=22'")%>
                            </div>
                        </fieldset>
                    </div>
                    <div style="width: 59%; float: right;">
                        <fieldset style="min-height: 320px;">
                            <legend><b>&nbsp;
                                <%=MyHtmlHelper.ActionLink(UrlEditThongTri, "Chọn nội dung in thông tri")%></b></legend>
                            <div style="float: left; width: 98%; margin: 3px; height: 320px; overflow:scroll;">
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="mGrid">
                                    <tr>
                                        <th>
                                        </th>
                                        <th>
                                            <%-- Tên--%>
                                          Loại thông tri
                                        </th>
                                        <th>
                                            <%-- Nội dung--%>
                                            Tên loại ngân sách
                                        </th>
                                    </tr>
                                    <%for (int i = 0; i < dtLoaiThongTri.Rows.Count; i++)
                                      {%>
                                    <tr>
                                        <td style="text-align: center;">
                                            <%=MyHtmlHelper.Option("ThongTri", Convert.ToString(dtLoaiThongTri.Rows[i]["iID_MaThongTri"]), iID_MaThongTri, "iID_MaThongTri", "", "")%>
                                        </td>
                                        <td>
                                            <%=MyHtmlHelper.Label(dtLoaiThongTri.Rows[i]["sLoaiThongTri"].ToString(),"")%>
                                        </td>
                                        <td>
                                            <%=MyHtmlHelper.Label(dtLoaiThongTri.Rows[i]["sTenLoaiNS"].ToString(),"")%>
                                        </td>
                                    </tr>
                                    <%} %>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <fieldset>
                        <legend><b>&nbsp;
                            <%=NgonNgu.LayXau("Thêm mới Nội dung in thông tri")%></b></legend>
                        <div style="width: 98%; margin: 2px; text-align:right; ">
                            <%--<div id="nhapform">
                                <div id="form2">--%>
                                    <table border="0" cellspacing="10" cellpadding="1" width="100%" >
                                        <tr>
                                            <td  style="width: 150px;">
                                              
                                                    <b>Loại thông tri</b>&nbsp;<span style="color: Red;">*</span>
                                               
                                            </td>
                                            <td >
                                       <div style="margin: 3px;">
                                                    <%=MyHtmlHelper.TextBox(ParentID, "", "sTen", "", "class=\"input1_2\" style=\"width: 100%;height:20px\" maxlength='150'")%></div>
                                            
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Tên loại ngân sách</b>&nbsp;<span style="color: Red;">*</span>
                                            </td>
                                            <td >
                                            <div style="margin: 3px;">
                                                    <%=MyHtmlHelper.TextBox(ParentID, "", "sNoiDung", "", "class=\"input1_2\" style=\"width: 100%; height:20px\" maxlength='150'")%></div>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td >
                                               
                                                    <b>Thêm mới</b>
                                                
                                            </td>
                                            <td >
                                                <div style="float: left;margin: 3px;">
                                                    <%=MyHtmlHelper.CheckBox(ParentID, "", "chkThemMoi", "", "")%>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                               <%-- </div>
                            </div>--%>
                        </div>
                    </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td>
            <div style="margin-top: 10px;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width: 40%">
                        </td>
                        <td align="right">
                            <input type="submit" class="button4" value="Tiếp tục" />
                        </td>
                        <td style="width: 1%">
                            &nbsp;
                        </td>
                        <td align="left">
                            <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                        </td>
                        <td style="width: 40%">
                        </td>
                    </tr>
                </table></div>
            </td>
        </tr>
    </table>
    <%} %>
</body>
</html>
