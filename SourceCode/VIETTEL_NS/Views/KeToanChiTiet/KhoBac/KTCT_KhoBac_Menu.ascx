<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="DomainModel" %>
<script type="text/javascript">

    ddaccordion.init({
        headerclass: "expandable", //Shared CSS class name of headers group that are expandable
        contentclass: "categoryitems", //Shared CSS class name of contents group
        revealtype: "click", //Reveal content when user clicks or onmouseover the header? Valid value: "click", "clickgo", or "mouseover"
        mouseoverdelay: 200, //if revealtype="mouseover", set delay in milliseconds before header expands onMouseover
        collapseprev: true, //Collapse previous content (so only one open at any time)? true/false 
        defaultexpanded: [0], //index of content(s) open by default [index1, index2, etc]. [] denotes no content
        onemustopen: false, //Specify whether at least one header should be open always (so never all headers closed)
        animatedefault: false, //Should contents open by default be animated into view?
        persiststate: true, //persist state of opened contents within browser session?
        toggleclass: ["", "openheader"], //Two CSS classes to be applied to the header when it's collapsed and expanded, respectively ["class1", "class2"]
        togglehtml: ["prefix", "", ""], //Additional HTML added to the header when it's collapsed and expanded, respectively  ["position", "html1", "html2"] (see docs)
        animatespeed: "fast", //speed of animation: integer in milliseconds (ie: 200), or keywords "fast", "normal", or "slow"
        oninit: function (headers, expandedindices) { //custom code to run when headers have initalized
            //do nothing
        },
        onopenclose: function (header, index, state, isuseractivated) { //custom code to run whenever a header is opened or closed
            //do nothing
        }
    })
</script>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="left" valign="top">
            <div class="arrowlistmenu">
                <h3 class="menuheader expandable">
                    <img src="../../../Content/Themes/images/icoKyHopDong.gif" alt="" border="0" />&nbsp;&nbsp;Quản lý danh mục</h3>
                <ul class="categoryitems">
                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Index", "KhoaHoc").ToString(), "Ngoại tệ")%></li>
                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Index", "LopHoc").ToString(), "Tỷ giá")%></li>
                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_KhoBac_ChuongTrinhMucTieu").ToString(), "Chương trình mục tiêu")%></li>
                </ul>
                <h3 class="menuheader expandable">
                    <img src="../../../Content/Themes/images/icoKyHopDong.gif" alt="" border="0" />&nbsp;&nbsp;Rút dự toán</h3>
                <ul class="categoryitems">
                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanChiTietKhoBac", new { iLoai = 1 }).ToString(), "Rút dự toán")%></li>
                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanChiTietKhoBac", new { iLoai = 2 }).ToString(), "Nhập số duyệt tạm ứng")%></li>
                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanChiTietKhoBac", new { iLoai = 3 }).ToString(), "Khôi phục dự toán")%></li>
                    <li><%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanChiTietKhoBac", new { iLoai = 4 }).ToString(), "Hủy dự toán")%></li>
                </ul>
                <h3 class="menuheader expandable">
                    <img src="../../../Content/Themes/images/icoKyHopDong.gif" alt="" border="0" />&nbsp;&nbsp;Báo cáo</h3>
                <ul class="categoryitems">
                </ul>
            </div>
        </td>
    </tr>
</table>