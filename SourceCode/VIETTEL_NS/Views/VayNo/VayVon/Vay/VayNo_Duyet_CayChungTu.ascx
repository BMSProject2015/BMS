<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<script src="<%= Url.Content("~/Scripts/jquery.tinyscrollbar.min.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.treeview.js") %>" type="text/javascript"></script>
<%
    int i;
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    int Thang = Convert.ToInt32(props["Thang"].GetValue(Model));
    int Nam = Convert.ToInt32(props["Nam"].GetValue(Model));
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    DataTable dt = VayNoModels.Get_DanhSachChungTu(MaND, Thang, Nam);
    DataTable tbl = null;
   
  
%>
<script type="text/javascript">
    $(document).ready(function () {
        $('#scrollbar1').tinyscrollbar();
    });
</script>
<script type="text/javascript">
    $(function () {
        $("#tree").treeview({
            collapsed: true,
            animated: "medium",
            control: "#sidetreecontrol",
            persist: "location"
        });
    })
</script>
<div class="box_tong" style="background-color: #fff; background-repeat: repeat;">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <div style="width: 60%; float: left;">
                        <span style="text-indent: 5px;">Cây chứng từ</span>
                    </div>
                    <div style="width: 40%; float: right;">
                        <div id="sidetreecontrol" style="text-align: right; margin-right: 5px;">
                            <%--  <a href="?#">Close</a> | <a href="?#">Open</a>--%>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2" style="padding-left: 10px;">
            <div id="scrollbar1">
                <div class="scrollbar">
                    <div class="track">
                        <div class="thumb">
                            <div class="end">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="viewport">
                    <div class="overview">
                        <ul id="tree">
                            <%
                                for (i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow R = dt.Rows[i];
                                    tbl = VayNoModels.getDonVibyChungTu(Convert.ToString(R["iID_Vay"]));
                            %>
                            <li><a href="#"><strong>
                                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = R["iID_Vay"] }).ToString(),  Convert.ToString(R["sSoChungTu"]), "Detail", null, "title=\"Xem chi tiết chứng từ\"")%>
                            </strong></a>
                                <% if (tbl.Rows.Count > 0)
                                   { %>
                                <ul>
                                    <%
                                        for (int j = 0; j < tbl.Rows.Count; j++)
                                        {
                                            DataRow R1 = tbl.Rows[j];
                                    %>
                                    <li><a href="#"><strong>
                                        <%=MyHtmlHelper.ActionLink(Url.Action("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = R["iID_Vay"], iID_MaDonVi = R1["iID_MaDonVi"] }).ToString(), Convert.ToString(R1["iID_MaDonVi"]) + "-" + Convert.ToString(R1["sTen"]), "Detail", null, "title=\"Xem chi tiết chứng từ\"")%>
                                    </strong></a></li>
                                    <%  }  %>
                                </ul>
                                <%  }  %>
                            </li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<%
    if (dt != null) dt.Dispose();
    if (tbl != null) tbl.Dispose();
%>
