<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<script src="<%= Url.Content("~/Scripts/jquery.tinyscrollbar.min.js") %>" type="text/javascript"></script> 
<script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script> 
<script src="<%= Url.Content("~/Scripts/jquery.treeview.js") %>" type="text/javascript"></script> 
<%
    int i;
    String sLNS = Convert.ToString(ViewData["sLNS"]);   
    if (String.IsNullOrEmpty(sLNS)) { sLNS = ""; }
    String sL = Convert.ToString(ViewData["sL"]);
    if (String.IsNullOrEmpty(sL)) { sL = ""; }
    String sK = Convert.ToString(ViewData["sK"]);
    if (String.IsNullOrEmpty(sK)) { sK = ""; }

    sLNS = "1";
    
    String sLNS_Cu = "";
    String sL_Cu = "";
    String sK_Cu = "";

    DataTable dt = MucLucNganSachModels.Get_dtDanhSachMucLucNganSach_Nhom();
%>

<script type="text/javascript">
    $(document).ready(function () {
        $('#scrollbar1').tinyscrollbar();
    });
</script>
<script type="text/javascript">
    var myWidth; var myHeight;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE      
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    }
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {      //IE 6+ in 'standards compliant mode'      
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    }
    else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible      
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
    }
    myWidth = myWidth - 15;
    $(function () {
        $("#tree").treeview({
            collapsed: false,
            animated: "medium",
            control: "#sidetreecontrol",
            persist: "location"
        });
    })
    //    $(document).ready(function () {
    //        alert(myWidth);
    //        $("#btnClose").click(function () {
    //            $("#divTree").animate({ width: 50 }, "slow");
    //            $("#divChungTuChiTietHT").animate({ width: myWidth - 50 }, "slow");
    //        });
    //    });
    //    $(document).ready(function () {
    //        $("#btnOpen").click(function () {
    //            $("#divTree").animate({ width: 250}, "slow");
    //            $("#divChungTuChiTietHT").animate({ width: myWidth - 250 }, "slow");
    //        });
    //    });
</script>
<div style="background-color: #fff; background-repeat: repeat;">
    <div class="title_tong" style="border-left: solid 1px #006666;">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <div style="width: 60%; float: left;">
                        <span style="text-indent: 5px;">Cây chứng từ</span>
                    </div>
                    <div style="width: 40%; float: right;">
                        <div id="sidetreecontrol" style="text-align: right; margin-right: 5px;"><a id="btnClose" href="?#">Close</a> | <a id="btnOpen" href="?#">Open</a></div>
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
                        <ul id="tree">
                            <li>
                                <a href="#"><strong>
                                   Mục lục ngân sách
                                </strong></a>
                                <ul>
                                <%for (i = 0; i < dt.Rows.Count; i++)
                                  {
                                      String sNguon, LNS, sNguon_Cu, LNS_Cu;
                                      LNS = Convert.ToString(dt.Rows[i]["sLNS"]);
                                      sNguon=
                                       %>
                                     <li>
                                        <a href="#"><strong>1</strong></a>
                                        <ul>
                                            <li>
                                                <a href="#"><strong>101</strong></a>
                                                <ul>
                                                    <li>
                                                        <a href="#"><strong>10010000</strong></a>    
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                      </li>
                                      <li>
                                        <a href="#"><strong>2</strong></a>
                                        <ul>
                                            <li>
                                                <a href="#"><strong>201</strong></a>
                                                <ul>
                                                    <li>
                                                        <a href="#"><strong>20010000</strong></a>    
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </li>
                                    <%} %>
                                </ul>
                            </li>
                        </ul>                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<%
    dt.Dispose();
%>



