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
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        if (String.IsNullOrEmpty(iID_MaChungTu))
            iID_MaChungTu = "-1";
        String MaND = User.Identity.Name;
        DataTable dt = ThuNopModels.getdtLoaiHinh();

        String sTen="", sLNS = "";
        DataTable dtBaoCao = ThuNopModels.getThongTinBaoCao(iLoai, iID_MaChungTu);
        if(dtBaoCao.Rows.Count>0)
        {
            sTen = Convert.ToString(dtBaoCao.Rows[0]["sTen"]);
            sLNS = Convert.ToString(dtBaoCao.Rows[0]["sLNS"]);
        }
        String[] arrDeAn = sLNS.Split(',');
        using (Html.BeginForm("EditSubmit", "ThuNopCauHinhBaoCao", new { ParentID = ParentID, iID_MaChungTu = iID_MaChungTu, iLoai = iLoai }))
        {
%>
   
  
  
    <div class="box_tong">
        <div class="title_tong">
              <span>Cấu hình báo cáo thu nộp</span>
        </div>
       <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Cột:</b>&nbsp;<span style="color: Red;"></span></div>
                        </td>
                        <td class="td_form2_td5">
                           <%= MyHtmlHelper.Label(ParentID, iID_MaChungTu, "iID_MaChungTu", "", "class=\"input1_2\"") %>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>
                                    <%= NgonNgu.LayXau("Tên cột") %></b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%= MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"") %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                         <td class="td_form2_td1"><div>
                                <b>
                                    <%= NgonNgu.LayXau("Loại hình") %></b></div></td>
                         <td class="td_form2_td5">
                               <div style="width: 100%; overflow: scroll; border: 1px solid black;">
                                <table class="mGrid">
                                     <tr>
                               <td><input type="checkbox" id="checkAll" onclick="Chonall(this.checked)"></td>
                                <td> Chọn tất cả  </td>
                                </tr>
                                    <%
                                        String TenDeAn = "";
                                        String DeAn = "";
                                        String _Checked = "checked=\"checked\"";
                                        for (i = 0; i < dt.Rows.Count; i++)
                                        {
                                            _Checked = "";
                                            TenDeAn = Convert.ToString(dt.Rows[i]["sMoTa"]);
                                            DeAn = Convert.ToString(dt.Rows[i]["sNG"]);
                                            for (int j = 0; j < arrDeAn.Length; j++)
                                            {
                                                if (DeAn == arrDeAn[j])
                                                {
                                                    _Checked = "checked=\"checked\"";
                                                    break;
                                                }
                                            }
%>
                                    <tr>
                                        <td style="width: 15%;">
                                            <input type="checkbox" value="<%= DeAn %>" <%= _Checked %> check-group="sDeAn" id="sDeAn" 
                                                name="sDeAn" />
                                        </td>
                                        <td>
                                            <%= TenDeAn %>
                                        </td>
                                    </tr>
                                  <% }%>
                                </table>
                            </div>
                         </td>
                    </tr>
                  
                </table>
                 <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="30%" align="center">
                <table cellpadding="0" cellspacing="0" border="0" align="center">
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
            </div>
        </div>
    </div>
    <% } %>
        <script type="text/javascript">
            function Chonall(value) {
                $("input:checkbox[check-group='sDeAn']").each(function (i) {
                    this.checked = value;
                });
            }                                            
     </script>
</asp:Content>
