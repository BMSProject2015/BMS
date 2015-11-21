<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    Boolean CoTabIndex = (props["CoTabIndex"] == null) ? false : Convert.ToBoolean(props["CoTabIndex"].GetValue(Model));
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID = ControlID + "_Search";
    String iID_MaCapPhat = Request.QueryString["iID_MaCapPhat"];
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;
    String ChiNganSach = Request.QueryString["ChiNganSach"];

    //HungPX: lấy giá trị loại chi tiết đến của chứng từ
    DataTable dtCapPhat = CapPhat_ChungTuModels.LayChungTuCapPhat(iID_MaCapPhat);
    String sLoai = Convert.ToString(dtCapPhat.Rows[0]["sLoai"]);
    int indexChiTietDen = CapPhat_BangDuLieu.getIndex(sLoai);
    String[] arrDSTruong = MucLucNganSachModels.arrDSTruong;
    String[] arrDSTruongDoRong = MucLucNganSachModels.arrDSTruongDoRong;
    
    //Hungpx lấy tên và size các cột có mặt trong vùng fix
    string dsTruong = "", dsTruongDoRong = "";
    for (int i = 0; i <= indexChiTietDen ; i++)
    {
        dsTruong += arrDSTruong[i] + ",";
        dsTruongDoRong+= arrDSTruongDoRong[i]+",";
    }
    dsTruong += "sMoTa";
    dsTruongDoRong += "250";
    
    //Cập nhập các thông tin tìm kiếm
    string _dsTruong = dsTruong;
    string _dsTruongDoRong = "60,30,30,40,40,30,30,250";

    String DSTruong = _dsTruong;
    arrDSTruong = DSTruong.Split(',');
    String strDSTruongDoRong = _dsTruongDoRong;
    arrDSTruongDoRong = strDSTruongDoRong.Split(',');
    
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
    
%>
<div class="box_tong">
    <div id="nhapform">
        <div id="form2">
            <form action="<%=Url.Action("SearchSubmit","CapPhat_ChungTuChiTiet",new {ParentID = ParentID, iID_MaCapPhat = iID_MaCapPhat})%>"
            method="post">
            <%-- <table class="mGrid1">
    <tr>
        <%
        for (int j = 0; j <= bang.arrDSMaCot.IndexOf("sTNG"); j++)
        {
            int iColWidth = bang.arrWidth[j] +4;
            if (j == 0) iColWidth = iColWidth + 1;
            String strAttr = String.Format("class='input1_4' style='width:{0}px;height:22px;'", iColWidth - 2);
            if (bang.DuocSuaChiTiet == false) strAttr += " tab-index='-1'";
            %>
            <td style="text-align:left;width:<%=iColWidth%>px;">
                <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[bang.arrDSMaCot[j]], TenTruong = bang.arrDSMaCot[j], LoaiTextBox = "2", Attributes = strAttr })%>
            </td>
            <%
        }
        %>
        <td style="padding: 1px 5px; text-align: left;">
            <input type="submit" id="<%=ParentID%>_btnTimKiem" <%=bang.DuocSuaChiTiet? "":"tab-index='-1'" %> value="<%=NgonNgu.LayXau("Tìm kiếm")%>" style="font-size: 11px; padding: 0px 3px;"/> 
        </td>
    </tr>
    </table>--%>
            <table class="mGrid1">
                <tr>
                    <%
                        for (int j = 0; j < arrDSTruong.Length - 1; j++)
                        {
                            int iColWidth = Convert.ToInt32(arrDSTruongDoRong[j]) + 4;
                            if (j == 0) iColWidth = iColWidth + 3;
                            String strAttr = String.Format("class='input1_4' onkeypress='jsCapPhat_Search_onkeypress(event)' search-control='1' search-field='{1}' style='width:{0}px;height:22px;'", iColWidth - 2, arrDSTruong[j]);
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
            <iframe id="ifrChiTietChungTu" width="100%" height="530px" src="<%= Url.Action("CapPhatChiTiet_Frame", "CapPhat_ChungTuChiTiet", new {iID_MaCapPhat=iID_MaCapPhat})%>">
            </iframe>
            </form>
        </div>
    </div>
</div>
