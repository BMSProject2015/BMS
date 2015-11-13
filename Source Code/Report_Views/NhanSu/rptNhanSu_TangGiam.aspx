<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NhanSu" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <% 
        String ParentID = "NhanSu_CapBac";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        //load du lieu doi tuong
        DataTable dtDoiTuong = new DataTable();
        dtDoiTuong.Columns.Add("MaDoiTuong", typeof (String));
        dtDoiTuong.Columns.Add("TenDoiTuong", typeof(String));
        DataRow drDoiTuong = dtDoiTuong.NewRow();
        drDoiTuong["MaDoiTuong"] = "1";
        drDoiTuong["TenDoiTuong"] = "Quân số tăng";
        dtDoiTuong.Rows.Add(drDoiTuong);
        drDoiTuong = dtDoiTuong.NewRow();
        drDoiTuong["MaDoiTuong"] = "-1";
        drDoiTuong["TenDoiTuong"] = "Quân số giảm";
        dtDoiTuong.Rows.Add(drDoiTuong);
        
        String DoiTuong = Convert.ToString(ViewData["DoiTuong"]);
        if (String.IsNullOrEmpty(DoiTuong))
        {
            DoiTuong = "0";
        }
        SelectOptionList slDoiTuong = new SelectOptionList(dtDoiTuong, "MaDoiTuong", "TenDoiTuong");
        dtDoiTuong.Dispose();

        DataTable dtLoaiDonVi = DanhMucModels.DT_DanhMuc("LoaiDonVi", false, "");
        DataRow R = dtLoaiDonVi.NewRow();
        R["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000000";
        R["sTen"] = "Toàn Lực lượng";
        dtLoaiDonVi.Rows.InsertAt(R, 0);
        R = dtLoaiDonVi.NewRow();
        R["iID_MaDanhMuc"] = "00000000-0000-0000-0000-000000000001";
        R["sTen"] = "Theo đơn vị";
        dtLoaiDonVi.Rows.InsertAt(R, 1);
        String LoaiIn = Convert.ToString(ViewData["LoaiIn"]);
        if (String.IsNullOrEmpty(LoaiIn))
        {
            LoaiIn = "00000000-0000-0000-0000-000000000000";
        }
        SelectOptionList slLoaiDonVi = new SelectOptionList(dtLoaiDonVi, "iID_MaDanhMuc", "sTen");
        dtDoiTuong.Dispose();


        DataTable dtCap = DanhMucModels.NS_DonVi_DenCap(true);
        String Cap = Convert.ToString(ViewData["Cap"]);
        if (String.IsNullOrEmpty(DoiTuong))
        {
            Cap = "";
        }
        SelectOptionList slCap = new SelectOptionList(dtCap, "iCap", "sCap");
        dtDoiTuong.Dispose();

        DataTable dtNam = DanhMucModels.DT_Nam(true,"--Tất cả--");
        String Nam = Convert.ToString(ViewData["Nam"]);
        if (String.IsNullOrEmpty(Nam))
        {
            if (PageLoad == "1")
            {
                Nam = "0";
            }
            else
            {
                Nam = DateTime.Now.Year.ToString();
            }
        }
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtDoiTuong.Dispose();

        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        Boolean bBaoCaoTH = Convert.ToBoolean(ViewData["bBaoCaoTH"]);
        String URLView = "";
        if (PageLoad == "1")
        {
            //edit
            if (bBaoCaoTH)
                URLView = Url.Action("ViewPDF_TongHop", "rptNhanSu_TangGiam", new { DoiTuong = DoiTuong, LoaiIn = LoaiIn, Cap = Cap, Nam = Nam });
            else
                URLView = Url.Action("ViewPDF", "rptNhanSu_TangGiam", new { DoiTuong = DoiTuong, LoaiIn = LoaiIn, Cap = Cap, Nam = Nam });
        }
        String urlExport = Url.Action("ExportToExcel", "rptNhanSu_TangGiam", new { DoiTuong = DoiTuong, LoaiIn = LoaiIn, Cap = Cap, Nam = Nam });
        using (Html.BeginForm("EditSubmit", "rptNhanSu_TangGiam", new { ParentID = ParentID, DoiTuong = DoiTuong, LoaiIn = LoaiIn, Cap = Cap, Nam = Nam }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaDonVi, "iID_MaDonVi", "")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách cán bộ tăng, giảm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Báo cáo:")%></div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDoiTuong, DoiTuong, "DoiTuong", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("In theo:")%></div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiDonVi, LoaiIn, "LoaiIn", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Đến cấp:")%></div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slCap, Cap, "Cap", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 5%;">
                            <div>
                                <%=NgonNgu.LayXau("Năm:")%></div>
                        </td>
                        <td width="15%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "Nam", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="8">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 47%;" align="right">
                                        <input type="submit" class="button" name="submitButton" id="Submit1" value="<%=NgonNgu.LayXau("Danh sách")%>" />
                                    </td>
                                    <td width="1%">
                                    </td>
                                    <td style="width: 5%;" align="center">
                                       <input id="TongHop" name="submitButton" type="submit" class="button" value="Tổng hợp" />
                                    </td>
                                    <td width="1%">
                                    </td>
                                    <td style="width: 48%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    <%} %>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
</body>
</html>
