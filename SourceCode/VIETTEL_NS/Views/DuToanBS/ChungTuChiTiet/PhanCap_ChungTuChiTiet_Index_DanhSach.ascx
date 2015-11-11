<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    string ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
   
    bool CoTabIndex = (props["CoTabIndex"]==null) ? false:Convert.ToBoolean(props["CoTabIndex"].GetValue(Model));
    string ParentID = ControlID + "_Search";
    string MaND = Convert.ToString(props["MaND"].GetValue(Model));
    string IPSua = Request.UserHostAddress;
    string iID_MaChungTu = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
    string sLNS = "";
    if (String.IsNullOrEmpty(sLNS))
    {
         sLNS = Convert.ToString(Request.QueryString["sLNS"]);
    }
    
    string iID_MaDonVi = Convert.ToString(Request.QueryString["iID_MaDonVi"]);
    bool ChucNangCapNhap = (props["ChucNangCapNhap"] == null) ? false : Convert.ToBoolean(props["ChucNangCapNhap"].GetValue(Model));

    //Cập nhập các thông tin tìm kiếm
    string strDSTruong = MucLucNganSachModels.strDSTruong + ",iID_MaDonVi,iID_MaPhongBanDich";
    string strDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong + ",200,30";  
    
    string[] arrDSTruong = strDSTruong.Split(',');
    string[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');
    Dictionary<String,String> dicGiaTriTimKiem = new Dictionary<string,string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        dicGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
%>
<div class="box_tong">
    <div id="nhapform">
        <div id="form2">
            <table class="mGrid1">
            <tr>
                <%
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if(i==arrDSTruong.Length-4)
                        continue;
                    int iColWidth = Convert.ToInt32(arrDSTruongDoRong[i]) + 4;
                    if (i == 0) iColWidth = iColWidth + 3;
                    string strAttr = String.Format("class='input1_4' onkeypress='jsDuToan_Search_onkeypress(event)' search-control='1' search-field='{1}' style='width:{0}px;height:22px;'", iColWidth - 2, arrDSTruong[i]);
                    if (CoTabIndex)
                    {
                        strAttr += String.Format(" tab-index='-1'");
                    }
                    %>
                    <td style="text-align:left;width:<%=iColWidth%>px;">
                        <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = dicGiaTriTimKiem[arrDSTruong[i]], TenTruong = arrDSTruong[i], LoaiTextBox = "2", Attributes = strAttr })%>
                    </td>
                    <%
                }
                %>
            </tr>
            </table>
            <iframe id="ifrChiTietChungTu" width="100%" height="538px" src="<%= Url.Action("ChungTuChiTiet_Frame", "DuToanBS_PhanCapChungTuChiTiet", new {iID_MaChungTu=iID_MaChungTu, iID_MaDonVi=iID_MaDonVi, sLNS=sLNS})%>"></iframe>
        </div>
    </div>
</div>