<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    Boolean CoTabIndex = (props["CoTabIndex"] == null) ? false : Convert.ToBoolean(props["CoTabIndex"].GetValue(Model));
   // String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID ="_Search";
    String[] arrDSTruong = QLDA_DanhMucDuAnModels.arrDSTruong;
    String[] arrDSTruongDoRong = QLDA_DanhMucDuAnModels.arrDSTruongDoRong;
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
%>
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
             <div id="form2">
            <div id="nhapform">
                <table class="mGrid1">
                <tr>
                    <%
                        for (int j = 0; j < arrDSTruong.Length - 4; j++)
                        {
                            int iColWidth = Convert.ToInt32(arrDSTruongDoRong[j]) + 4;
                            if (j == 0) iColWidth = iColWidth + 3;
                            String strAttr = String.Format("class='input1_4' onkeypress='jsDanhMuc_Search_onkeypress(event)' search-control='1' search-field='{1}' style='width:{0}px;height:22px;'", iColWidth - 2, arrDSTruong[j]);
                            if (CoTabIndex)
                            {
                                strAttr += " tab-index='-1'";
                            }
                    %>
                    <td style="text-align: left; width: <%=iColWidth%>px;">
                        <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[arrDSTruong[j]], TenTruong = arrDSTruong[j], LoaiTextBox = "2", Attributes = strAttr })%>
                    </td>
                    <%
                        }
                    %>
                </tr>
            </table>
            </div>
               
                        <iframe  id="ifrChiTietChungTu" width="100%" height="530px"  src="<%= Url.Action("QLDA_DanhMucDuAn_Frame", "QLDA_DanhMucDuAn", new {ControlID = "DanhMucDuAn"})%>"> 
                        
                </iframe>
               <%-- <%Html.RenderPartial("~/Views/QLDA/DanhMuc/DanhMucDuAn/QLDA_DanhMucDuAn_Index_DanhSach.ascx", new { ControlID = "DanhMucDuAn", MaND = User.Identity.Name }); %>--%>
                 </div>
            
        </div>
    </div>
</div>    
 <script type="text/javascript">
     $(document).ready(function () {
         jsCapPhat_Url_Frame = '<%=Url.Action("QLDA_DanhMucDuAn_Frame", "QLDA_DanhMucDuAn", new { ControlID = "DanhMucDuAn" })%>';
     });
    </script>

