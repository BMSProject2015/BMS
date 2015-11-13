<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<script src="<%= Url.Content("~/Scripts/jquery.tinyscrollbar.min.js") %>" type="text/javascript"></script> 
<script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script> 
<script src="<%= Url.Content("~/Scripts/jquery.treeview.js") %>" type="text/javascript"></script> 
<%
   int i, j, k;
   DataTable dtPhongBan = DanhMucModels.NS_PhongBan();
   DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSach();
   DataTable dtDonVi = DanhMucModels.NS_DonVi();
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
                        <div id="sidetreecontrol" style="text-align: right; margin-right: 5px;"><a href="?#">Close</a> | <a href="?#">Open</a></div>
                    </div>
                </td>
            </tr>
        </table>
	</div> 
    <div id="nhapform">
        <div id="form2" style="padding-left: 10px;">
            <div id="scrollbar1">
                <div class="scrollbar"><div class="track"><div class="thumb"><div class="end"></div></div></div></div>
                <div class="viewport">
                    <div class="overview">
                        <ul><li><a href="#"><strong>Cục tài chính</strong></a></li></ul>
                        <ul id="tree">
                            <%
                            for (i = 0; i < dtPhongBan.Rows.Count; i++)
                            {
                                DataRow R = dtPhongBan.Rows[i];
                                %>
                                <li><%=MyHtmlHelper.ActionLink(Url.Action("Edit", "PhongBan", new { MaPhongBan = dtPhongBan.Rows[i]["iID_MaPhongBan"] }).ToString(), dtPhongBan.Rows[i]["sTen"].ToString())%>
                                    <ul>
                                        <li><a href="#">CST-001</a>
                                            <ul>
                                                <%for (j = 0; j < dtLoaiNganSach.Rows.Count; j++) { 
                                                %>
                                                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Edit", "PhongBan", new { MaPhongBan = dtLoaiNganSach.Rows[j]["sLNS"] }).ToString(), dtLoaiNganSach.Rows[j]["TenHT"].ToString())%>
                                                        <ul>
                                                            <%for (k = 0; k < dtDonVi.Rows.Count; k++) { 
                                                            %>
                                                                <li><%=MyHtmlHelper.ActionLink(Url.Action("Edit", "PhongBan", new { MaPhongBan = dtDonVi.Rows[k]["iID_MaDonVi"] }).ToString(), dtDonVi.Rows[k]["sTen"].ToString())%></li>
                                                            <%
                                                            }
                                                            dtDonVi.Dispose();
                                                            %>
                                                        </ul>
                                                    </li>
                                                <%
                                                  } 
                                                  dtLoaiNganSach.Dispose();
                                                %>
                                            </ul>
                                        </li>
                                    </ul>
                                </li>
                                <%
                            }
                            dtPhongBan.Dispose();
                            %>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

