<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String iID_KeHoachVon_QuyetDinh = Convert.ToString(ViewData["iID_KeHoachVon_QuyetDinh"]);
        String ParentID = "Edit";
        String sSoQuyetDinh = "", dNgayQD = "", sNoiDung = "", dNgayKeHoachVon = "", iLoaiKeHoachVon = "";
        DataTable dtLoaiKeHoachVon = QLDA_KeHoachVonModels.DT_LoaiKeHoachVon();
        SelectOptionList slLoaiKeHoachVon = new SelectOptionList(dtLoaiKeHoachVon, "iID_MaLoaiKeHoachVon", "sTen");
        dtLoaiKeHoachVon.Dispose();
        DataTable dt = QLDA_KeHoachVonModels.Get_Row_KHV_ChungTu(iID_KeHoachVon_QuyetDinh);
        if (dt.Rows.Count > 0 && iID_KeHoachVon_QuyetDinh != null && iID_KeHoachVon_QuyetDinh != "")
        {
            DataRow R = dt.Rows[0];
            sSoQuyetDinh = HamChung.ConvertToString(R["sSoQuyetDinh"]);
            iLoaiKeHoachVon = HamChung.ConvertToString(R["iLoaiKeHoachVon"]);
            dNgayQD = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayQD"]));
            dNgayKeHoachVon = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayKeHoachVon"]));
            sNoiDung = Convert.ToString(R["sNoiDung"]);
        }
        else
        {
            //DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            //String NamLamViec = DateTime.Now.Year.ToString();
            //int iDotMoi = 0;
            //if (dtCauHinh != null && dtCauHinh.Rows.Count > 0)
            //{
            //    NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            //    dtCauHinh.Dispose();
            //    if (QLDA_DuToan_NamModels.Get_Max_Dot(NamLamViec) != "")
            //    {
            //        iDotMoi = Convert.ToInt32(QLDA_DuToan_NamModels.Get_Max_Dot(NamLamViec)) + 1;
            //    }
            //    else
            //    {
            //        iDotMoi = 1;
            //    }
            //}
            //sSoQuyetDinh = iDotMoi.ToString();

        }
        if (dt != null) { dt.Dispose(); };



        using (Html.BeginForm("EditSubmit", "QLDA_KeHoachVon", new { ParentID = ParentID, iID_KeHoachVon_QuyetDinh = iID_KeHoachVon_QuyetDinh }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_KeHoachVon"), "Danh sách Kế hoạch vốn")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <% if (ViewData["DuLieuMoi"] == "1")
                           {
                        %>
                        <span>Thêm Kế hoạch vốn </span>
                        <% 
                           }
                           else
                           { %>
                        <span>Sửa Kế hoạch vốn</span>
                        <% } %>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <b>
                                    <%=NgonNgu.LayXau("Số quyết định")%></b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sSoQuyetDinh, "sSoQuyetDinh", "", "class=\"input1_2\"")%>
                                   <%= Html.ValidationMessage(ParentID + "_" + "err_sSoQuyetDinh")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%">
                            <div>
                                <b>Ngày quyết định</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, dNgayQD, "dNgayQD", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayQD")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%">
                            <div>
                                <b>Ngày lập</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, dNgayKeHoachVon, "dNgayKeHoachVon", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayKeHoachVon")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%">
                            <div>
                                <b>Loại kế hoạch vốn</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiKeHoachVon, iLoaiKeHoachVon, "iLoaiKeHoachVon", "", " class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iLoaiKeHoachVon")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <b>
                                    <%=NgonNgu.LayXau("Nội dung")%></b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height:60px\"")%>
                            </div>
                        </td>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } 
    %>
</asp:Content>
