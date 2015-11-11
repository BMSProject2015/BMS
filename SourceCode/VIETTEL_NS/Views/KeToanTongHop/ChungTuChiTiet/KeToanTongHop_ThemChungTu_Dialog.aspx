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
        String UserID = User.Identity.Name;
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        String iThang = Request.QueryString["iThang"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";

        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();

        DataTable dtNgay = DanhMucModels.DT_Ngay();
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        dtNgay.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        using (Ajax.BeginForm("Edit_Fast_ChungTu_Submit", "KeToanTongHop", new { ParentID = ParentID, OnSuccess = OnSuccess, MaDiv = MaDiv, MaDivDate = MaDivDate }, new AjaxOptions { }))
        {
    %>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Thêm chứng từ ghi sổ</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td class="td_form2_td1">
                                    <div><b>Ngày/Tháng chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slNgay, "" , "iNgay", "", "class=\"input1_2\" style=\"width:17%;\"")%>/
                                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, "", "iThang", "", "class=\"input1_2\" style=\"width:17%;\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div><b>Tập số</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextBox(ParentID, "", "sTapSo", "", "class=\"input1_2\"")%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div><b>Đơn vị</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextBox(ParentID, "", "sDonVi", "", "class=\"input1_2\"")%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div><b>Nội dung</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextArea(ParentID, "", "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                                <td width="30%" align="right" class="td_form2_td5">
                                                    <input type="submit" class="button" id="btnLuu" value="Lưu" />
                                                </td>          
                                                    <td width="5px">&nbsp;</td>          
                                                <td class="td_form2_td5">
                                                    <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%
        } 
    %>
</body>
</html>
