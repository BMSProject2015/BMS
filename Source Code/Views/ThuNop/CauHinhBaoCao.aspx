<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        int i;
        String ParentID = "ThuNop_ChungTu";
        String iLoai = Request.QueryString["iLoai"];
        if (String.IsNullOrEmpty(iLoai))
            iLoai = "1";
        String MaND = User.Identity.Name;
        DataTable dtBaoCao = ThuNopModels.getThongTinCotBaoCao(iLoai);
%>
   
  
  
    <div class="box_tong">
        <div class="title_tong">
            <%if (iLoai == "1")
              { %>
              <span>Cấu hình báo cáo thu nộp THU02</span>
              <% }
              else
              { %>
               <span>Cấu hình báo cáo thu nộp biểu giao ban</span>
              <% } %>
        </div>
         <table class="mGrid">
            <tr>
                <th style="width: 5%;" align="center">
                    STT
                </th>
                <th style="width: 40%;" align="center">
                    Tên cột
                </th>
                <th style="width: 30%;" align="center">
                   Loại hình
                </th>
                <th style="width: 10%;" align="center">
                  Thao tác
                </th>
            </tr>
             <%
                 for (i = 0; i < dtBaoCao.Rows.Count; i++)
                {
                    DataRow R = dtBaoCao.Rows[i];
                    String strEdit = "";
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "ThuNopCauHinhBaoCao", new { iID_MaChungTu = R["iID_MaCot"], iLoai = iLoai }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
            %>
            <tr>
                <td align="center">
                    <%=R["iID_MaCot"]%>
                </td>
                <td align="left">
                    <%=R["sTen"]%>
                </td>
               <td align="left">
                    <%=R["sLNS"]%>
                </td>
              <td align="center">
                    <%=strEdit%>
                </td>
            </tr>
            <%} %>
        </table>
      
    </div>
</asp:Content>
