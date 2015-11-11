<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<script src="<%= Url.Content("~/Scripts/jquery.tinyscrollbar.min.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.treeview.js") %>" type="text/javascript"></script>
<link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/style.css") %>" />
<%
    int i;
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);

    NameValueCollection data = DuToan_ChungTuModels.LayThongTin(iID_MaChungTu);
    String sLNS = Convert.ToString(ViewData["sLNS"]);
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    if (String.IsNullOrEmpty(sLNS)) { sLNS = ""; }
    if (String.IsNullOrEmpty(iID_MaDonVi)) { iID_MaDonVi = ""; }
    DataTable dt = DuToan_ChungTuChiTietModels.Get_dtCayChiTiet(iID_MaChungTu);
    String sLNS_Cu = "";
%>
<script type="text/javascript">
    $(document).ready(function () {
        $('#scrollbar1').tinyscrollbar();
    });
</script>
<script type="text/javascript">
    $(function () {
        $("#tree").treeview({
            collapsed: false,
            animated: "medium",
            control: "#sidetreecontrol",
            persist: "location"
        });
    })
</script>
<div id="scrollbar1">
    <div class="scrollbar">
        <div class="track">
            <div class="thumb">
                <div class="end">
                </div>
            </div>
        </div>
    </div>
    <div class="viewport" style="min-height: 600px;">
        <div class="overview">
            <ul id="tree">
                <li><a href="#"><strong>
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu }).ToString(), "Đợt ngày: " +  String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayChungTu"])))%>
                </strong></a>
                    <ul>
                        <%
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow R = dt.Rows[i];
                                String tgLNS = Convert.ToString(R["sLNS"]);
                                String tgMaDonVi = Convert.ToString(R["iID_MaDonVi"]);
                                if (tgLNS != sLNS_Cu)
                                {
                                    if (sLNS_Cu != "")
                                    {
                        %>
                    </ul>
                </li>
                <%
}
                %>
                <li>
                    <%
                        if (sLNS == tgLNS && String.IsNullOrEmpty(iID_MaDonVi))
                        {
                    %>
                    <strong>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu, sLNS = R["sLNS"] }).ToString(), Convert.ToString(R["sLNS"]) + "-" + Convert.ToString(R["TenLoaiNganSach"]))%></strong>
                    <%
                        }
                                    else
                                    {
                    %>
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu, sLNS = R["sLNS"] }).ToString(), Convert.ToString(R["sLNS"]) + "-" + Convert.ToString(R["TenLoaiNganSach"]))%>
                    <%
                        }
                    %>
                    <ul>
                        <%
}
                        %>
                        <li>
                            <%
                                if (sLNS == tgLNS && iID_MaDonVi == tgMaDonVi)
                                {
                            %>
                            <strong>
                                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu, sLNS = R["sLNS"], iID_MaDonVi = R["iID_MaDonVi"] }).ToString(), Convert.ToString(R["iID_MaDonVi"]) + "-" + Convert.ToString(R["TenDonVi"]))%></strong>
                            <%
                                }
                                            else
                                            {
                            %>
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu, sLNS = R["sLNS"], iID_MaDonVi = R["iID_MaDonVi"] }).ToString(), Convert.ToString(R["iID_MaDonVi"]) + "-" + Convert.ToString(R["TenDonVi"]))%>
                            <%
                                }
                            %>
                        </li>
                        <%
                            sLNS_Cu = tgLNS;
                                        }
                                        if (sLNS_Cu != "")
                                        {
                        %>
                    </ul>
                </li>
                <%
                    }
                %>
            </ul>
            </li> </ul>
        </div>
    </div>
</div>
<%
    dt.Dispose();
%>
