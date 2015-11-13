<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    string ParentID = (String)(props["ParentID"].GetValue(Model));
    NameValueCollection data = (NameValueCollection)(props["data"].GetValue(Model));
    String[] arrDSTruong = MucLucNganSachModels.arrDSTruong;
    String[] arrDSTruongDoRong = MucLucNganSachModels.arrDSTruongDoRong;
    string strDSTruong = MucLucNganSachModels.strDSTruong + ",iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,sXauNoiMa,bLaHangCha";
%>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <%
        for (int i = 0; i < arrDSTruong.Length - 1; i++)
        {
            String strTruong = arrDSTruong[i];
            String strTruongText = strTruong + "1";
            String strAttr = String.Format("class='input1_4' tab-index='-1' style='width:{1}px;height:25px; padding-right:2px;' onfocus=\"LSN_MaCotNganSach='{0}';\" onblur=\"func_Auto_Complete_onblur(this.id,'{0}');\"", strTruong, Convert.ToInt32(arrDSTruongDoRong[i]));
            String strTerm = "DSGiaTri:funcGhepMa, Truong:'iID_MaMucLucNganSach', GiaTri:request.term";

            String GiaTri = data[arrDSTruong[i]];
            %>
            <td <%=(i==0)?"class='"+ParentID+"_FirstCol'":""%> style="padding:0px;">
                <%=MyHtmlHelper.Autocomplete(ParentID, GiaTri, GiaTri, strTruong, strTruongText, "", strAttr)%>
                <%=MyHtmlHelper.AutoComplete_Initialize(ParentID + "_" + strTruongText, ParentID + "_" + strTruong, Url.Action("get_DanhSach", "Public"), strTerm, "func_Auto_Complete", new { delay = 100, minLength = 1 })%>
            </td>
            <%
        }
        %>
        <td style="padding:0px;">
            <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = data["sMoTa"], TenTruong = "sMoTa", Attributes = "khong-nhap='1' class='input1_3' style='width:400px;height:25px; padding-right:2px;'" })%>
        </td>
    </tr>
</table>

<script src="<%= Url.Content("~/Scripts/jsChonMucLucNganSach.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>

<div style="display:none">
    <%=MyHtmlHelper.Hidden(ParentID, data, "iID_MaMucLucNganSach")%>
    <%=MyHtmlHelper.Hidden(ParentID, data, "iID_MaMucLucNganSach_Cha")%>
    <%=MyHtmlHelper.Hidden(ParentID, data, "sXauNoiMa")%>
    <%=MyHtmlHelper.Hidden(ParentID, data, "bLaHangCha")%>
</div>
<script type="text/javascript">
    var LSN_MaCotNganSach = '';
    var LSN_MaCotNganSach_Dien = '';
    var strParentID = '<%=ParentID%>';
    var strDSTruong = '<%=strDSTruong%>';
    var arrGiaTriCu = new Array();
    var Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
    var Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
    $(document).ready(function () {
        LuuGiaTriHienTai();
    });
</script>