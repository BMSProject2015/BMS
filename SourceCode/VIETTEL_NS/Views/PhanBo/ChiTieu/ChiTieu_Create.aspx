<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String MaChiTieu = Convert.ToString(ViewData["MaChiTieu"]);
    String MaDotPhanBo = Convert.ToString(ViewData["MaDotPhanBo"]);
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

    DataTable dtChungTu = PhanBo_ChiTieuModels.GetChiTieu(MaChiTieu);
    DataRow R;
    String iSoChungTu = "", dNgayChungTu = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "";
    if (dtChungTu.Rows.Count > 0)
    {
        R = dtChungTu.Rows[0];
        dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
        sNoiDung = Convert.ToString(R["sNoiDung"]);
        iSoChungTu = Convert.ToString(R["iSoChungTu"]);
        sLyDo = Convert.ToString(R["sLyDo"]);
        iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);
    }
    else
    {
        dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
    }
    DataTable dtDuToan = PhanBo_ChiTieuModels.GET_DanhSachDuToan(UserID, true);
    DataTable dtChon = PhanBo_ChiTieuModels.GET_DanhSachDuToanDuocChon(MaChiTieu);
    using (Html.BeginForm("EditSubmit_DauNam", "PhanBo_ChiTieu", new { ParentID = ParentID, MaChiTieu = MaChiTieu, MaDotPhanBo = MaDotPhanBo }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_DotPhanBo"), "Đợt phân bổ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_ChiTieu", new { MaDotPhanBo = MaDotPhanBo }), "Chỉ tiêu phân bổ")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới chỉ tiêu phân bổ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa thông tin chỉ tiêu phân bổ")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 50%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>Số chứng từ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <b><%=iSoChungTu%></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Ngày chứng từ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\" onblur=isDate(this);")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Nội dung</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Chọn dự toán</div>
                        </td>
                        <td class="td_form2_td5">
                             <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid">
                  <tr>
                        <th align="center"> <input type="checkbox"  id="abc" onclick="CheckAll(this.checked)" /></th>
                        <th class="td_form2_td1" style="width: 15%;">
                            <div>Số chứng từ</div>
                        </th>
                        <th class="td_form2_td1">
                            <div>
                                <b>Ngày chứng từ</b>
                            </div>
                        </th>
                    </tr>
                <%for (int i = 0; i < dtDuToan.Rows.Count; i++)
                  {
                      String strChecked="";
                      String iID_MaDuToan = Convert.ToString(dtDuToan.Rows[i]["iID_MaChungTu"]);
                      String sdNgayChungTu = Convert.ToString(dtDuToan.Rows[i]["dNgayChungTu"]);
                      String sSoChungTu = Convert.ToString(dtDuToan.Rows[i]["sSoChungTu"]);
                      String sDuToan="";
                       for (int j = 0; j < dtChon.Rows.Count; j++)
                          {
                              sDuToan = Convert.ToString(dtChon.Rows[j]["iID_MaDuToan"]);
                              if (iID_MaDuToan.Equals(sDuToan))
                              {
                                  strChecked = "checked=\"checked\"";
                                  break;
                              }
                          }
                       %>
                    <tr>
                        <td align="center"><input type="checkbox" value="<%=sDuToan %>" <%=strChecked %> check-group="PhanBo" id="iID_MaPhanBo" name="iID_MaPhanBo" /></td>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div> <b><%=sSoChungTu%></b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                               <%=MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy}",dtDuToan.Rows[i]["dNgayChungTu"]),"dNgayChungTu") %>
                            </div>
                        </td>
                    </tr>
              <%} %>
                   
                </table>
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
                                            <input type="submit" class="button" id="Submit1" value="Lưu" />
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
<%
    }       
%>
</asp:Content>


