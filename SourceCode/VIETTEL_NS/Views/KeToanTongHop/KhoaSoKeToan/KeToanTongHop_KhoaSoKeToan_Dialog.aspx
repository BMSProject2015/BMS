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
<head id="Head1" runat="server">
    <title><%=ConfigurationManager.AppSettings["TitleView"]%></title>
</head>
<body>
    <%
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
        String iThang = Request.QueryString["iThang"];
        String iNam = Request.QueryString["iNam"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        DataTable dtLuyKe = KeToanTongHop_KhoaSoKeToanModels.Get_LuyKeKyTruocChoMotTaiKhoan(iNam, iThang, iID_MaTaiKhoan);
        DataTable dt = KeToanTongHop_KhoaSoKeToanModels.Get_DanhSach_ChiTietTaiKhoanPhatSinhTrongKy(iID_MaTaiKhoan, iNam, iThang);
    %>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Danh sách chứng từ phát sinh trong tháng: <%=iThang %> của tài khoản <%=iID_MaTaiKhoan %></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table class="mGrid">
                            <tr>
                                <th rowspan="2" style="width: 25px">STT</th>
                                <th colspan="2" style="width: 150px; border-bottom: solid 1px #999;"">Chứng từ</th>
                                <th colspan="2" rowspan="2">Diễn giải chi tiết</th>
                                <th rowspan="2" style="width: 100px">TK đối ứng</th>
                                <th colspan="2" style="width: 200px; border-bottom: solid 1px #999;"">Số phát sinh</th>
                                <th rowspan="2" style="width: 12px;">&nbsp;</th>
                            </tr>
                            <tr>
                                <th style="width: 50px">Ngày</th>
                                <th style="width: 100px">Số chứng từ</th>
                                <th style="width: 100px">Nợ</th>
                                <th style="width: 100px">Có</th>
                            </tr>
                        </table>
                        <div style="width:100%;height:300px;overflow:scroll;position:relative;">
                        <table class="mGrid">
                            <tr>
                                <th style="width: 25px">0</th>
                                <th style="width: 50px"></th>
                                <th style="width: 100px"></th>
                                <th>Tồn đầu kỳ</th>
                                <th style="width: 100px"></th>
                                <th style="width: 100px" align="right"><%=CommonFunction.DinhDangSo(dtLuyKe.Rows[0]["rCK_No"])%></th>
                                <th style="width: 100px" align="right"><%=CommonFunction.DinhDangSo(dtLuyKe.Rows[0]["rCK_Co"])%></th>
                            </tr>
                            <%
                            DataRow R;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int sSTT = i + 1;
                                R = dt.Rows[i];
                                String strClass = "";
                                if (i % 2 == 0) strClass = "alt";
                                %>
                                <tr class="<%=strClass %>">
                                    <td align="center" style="padding: 3px 3px;">
                                        <%=sSTT %>
                                    </td>
                                    <td align="center" style="padding: 3px 3px;">
                                        <%=R["iNgay"] %>
                                    </td>
                                    <td align="center" style="padding: 3px 3px;">
                                        <%=R["sSoChungTuChiTiet"]%>
                                    </td>
                                    <td align="left" style="padding: 3px 3px;">
                                        <%=R["sNoiDung"]%>
                                    </td>
                                    <td align="center" style="padding: 3px 3px;">
                                        <%=R["iID_MaTaiKhoan_DoiUng"]%>
                                    </td>
                                    <td align="right" style="padding: 3px 3px;">
                                        <%=CommonFunction.DinhDangSo(R["rSoPhatSinhNo"])%>
                                    </td>
                                    <td align="right" style="padding: 3px 3px;">
                                        <%=CommonFunction.DinhDangSo(R["rSoPhatSinhCo"])%>
                                    </td>
                                </tr>
                            <%} %>
                        </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%
    if (dt != null) { dt.Dispose(); };
    %>
</body>
</html>