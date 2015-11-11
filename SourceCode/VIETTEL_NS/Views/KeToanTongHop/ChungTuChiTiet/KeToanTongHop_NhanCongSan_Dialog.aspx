<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=ConfigurationManager.AppSettings["TitleView"]%></title>
</head>
<body>
    <%
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String iThang = Request.QueryString["iThang"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        String sTaiKhoan = "008";
        
        NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);
        String iNgay = data["iNgay"];
        String iNam = data["iNamLamViec"];
        
        DataTable dtNgay = DanhMucModels.DT_Ngay();
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        dtNgay.Dispose();
        
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        DataTable dtTaiKhoan = TaiKhoanModels.DT_DSTaiKhoan(true);
        SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTen");
        dtTaiKhoan.Dispose();

        DataTable dt = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietQuyetToan_TheoThang(iNam,iThang);

        using (Ajax.BeginForm("Edit_Fast_CongSan_Submit", "KeToanTongHop_ChungTuChiTiet", new { ParentID = ParentID, OnSuccess = OnSuccess, iID_MaChungTu = iID_MaChungTu, MaDiv = MaDiv, MaDivDate = MaDivDate }, new AjaxOptions { }))
        {
    %>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Danh sách công sản quyết toán trong tháng: <%=iThang %></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td align="left" colspan="2" valign="top">
                                    <table class="mGrid">
                                        <tr>
                                            <th style="width: 25px;text-align:center;">
                                                <input type="checkbox" id="chkCheckAll" onclick="setCheckboxes();" />
                                            </th>
                                           <th style="width: 150px;text-align:center;">Số C.T.G.S đã có</th>
                                            <th style="width: 150px;text-align:center;">Số chứng từ quyết toán</th>
                                            <th style="text-align:center;">Loại ngân sách- Mô tả</th>
                                            <th style="width: 50px;text-align:center;">Tháng</th>
                                            <th style="width: 120px;text-align:center;">Số tiền</th>
                                            <th style="width: 12px;text-align:center;">&nbsp;</th>
                                        </tr>
                                    </table>
                                    <div style="width:100%;height:250px; overflow:scroll;position:relative;">
                                    <table class="mGrid">
                                        <%
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                        String strValue = Convert.ToString(dt.Rows[i]["iID_MaChungTuChiTiet"]);
                                        String strSoGhiSo = KeToanTongHop_ChungTuChiTietModels.LayChuoiChungTuGhiSoCuaChungTuChiTiet(strValue, iThang, iNam, 4);  
                                        String strClass = "";
                                        if (i % 2 == 0) strClass = "alt";
                                        %>
                                        <tr class="<%=strClass %>">
                                            <td align="center" style="width: 30px; padding: 3px 2px;">
                                                <%
                                                int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, iThang, iNam, strValue, 1);
                                                if (intDaCo == 0)
                                                {
                                                %>
                                                    <input type="checkbox" name="<%= ParentID %>_txt" id="<%= ParentID %>_txt" value="<%= strValue %>" group-index="1"/>
                                                <%
                                                }
                                                %>
                                            </td>
                                            <td align="center" style="padding: 3px 2px; width: 120px;font-weight:bold;">
                                                <%=strSoGhiSo %>
                                            </td>
                                            <td align="center" style="padding: 3px 2px; width: 150px;">
                                                <%=dt.Rows[i]["sTienToChungTu"]%> - <%=dt.Rows[i]["iSoChungTu"]%>
                                            </td>
                                            <td>
                                                <%=dt.Rows[i]["sLNS"]%><%=dt.Rows[i]["sMoTa"]%>
                                            </td>
                                            <td align="center" style="padding: 3px 2px; width: 50px;">
                                                <%=dt.Rows[i]["iThang_Quy"]%>
                                            </td>
                                            <td align="right" style="padding: 3px 2px; width: 120px;">
                                                <%=CommonFunction.DinhDangSo(dt.Rows[i]["rSoTien"])%>
                                            </td>
                                        </tr>
                                        <%} %>
                                    </table>
                                    </div>
                                </td>
                            </tr>
                            <tr><td  colspan="2" style="height: 10px; font-size: 5px;">&nbsp;</td></tr>
                            <tr>
                                <td style="width: 80%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 7%">Ngày</td>
                                            <td style="width: 10%">
                                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                                            </td>
                                            <td style="width: 7%">Tháng</td>
                                            <td style="width: 10%">
                                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThangXem", "", "class=\"input1_2\" disabled=\"disabled\" style=\"width:90%;\"")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 20%">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td align="right" style="width: 10%"><input type="submit" class="button4" value="Thêm" /></td>
                                            <td style="width: 1%">&nbsp;</td>
                                            <td align="right" style="width: 10%"><input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');"/></td>
                                            <td style="width: 1%">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%
        } if (dt != null) { dt.Dispose(); };
    %>
    <script type="text/javascript">
        function setCheckboxes() {
            $('input:checkbox[group-index="1"]').each(function (i) {
                this.checked = document.getElementById('chkCheckAll').checked;
            });
        }    
    </script>
</body>
</html>
