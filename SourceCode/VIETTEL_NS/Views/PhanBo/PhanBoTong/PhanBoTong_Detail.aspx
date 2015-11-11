<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PhanBoTong_Detail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ControlID = "PhanBoTong";
    String ParentID = ControlID + "_Search";
    String iID_MaPhanBo = Convert.ToString(ViewData["iID_MaPhanBo"]);

    //Cập nhập các thông tin tìm kiếm
    String strDSTruong = MucLucNganSachModels.strDSTruong;
    String strDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong;
    String[] arrDSTruong = strDSTruong.Split(',');
    String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');
    
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
    NameValueCollection data = PhanBo_PhanBoModels.LayThongTin(iID_MaPhanBo);
%>
   <div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Thông tin chứng từ</span>
                </td>
                <td align="right"><span>F2 - Thêm</span></td>
                <td align="right" style="width:100px;"><span>Delete - Xóa</span></td>                        
                <td align="left"><span>F10 - Lưu</span></td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                <tr>
                    <td class="td_form2_td1" style="width: 15%">
                        <div><b>Đợt chỉ tiêu</b></div>
                    </td>
                    <td class="td_form2_td5" style="width: 20%"><div>
                        <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayDotPhanBo"]))%></div>
                    </td>
                    <td class="td_form2_td1" style="width: 15%">
                        <div><b>Ngày chứng từ</b></div>
                    </td>
                    <td class="td_form2_td5" style="width: 20%"><div>
                        <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" style="width: 15%"><div><b>Số phân bổ</b></div></td>
                    <td class="td_form2_td5" style="width: 20%"><div><%=data["sTienToChungTu"]%><%=data["iSoChungTu"]%></div></td>                            
                    <td class="td_form2_td1" style="width: 15%"><div><b>Nội dung</b></div></td>
                    <td class="td_form2_td5" style="width: 20%"><div><%=data["sNoiDung"]%></div></td>
                </tr>
            </table>
        <div id="form2">
            <iframe id="ifrChiTietChungTu" width="100%" height="530px" src="<%= Url.Action("PhanTongChiTiet_Frame", "PhanBo_Tong", new {iID_MaPhanBo=iID_MaPhanBo})%>"></iframe>
        </div>
    </div>
</div>

</asp:Content>
