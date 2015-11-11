<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID = ControlID + "_Search";
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
    String MaLoai = Convert.ToString(ViewData["MaLoai"]);
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;
    String urlBaoHiem = Url.Action("Index_BH", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
    String urlGiaiThichBangLoi = Url.Action("Index_GTBL", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
    String urlGiaiThichSoTien = Url.Action("Index_GTST", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
    
    //Cập nhập các thông tin tìm kiếm
    String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
    String[] arrDSTruong = DSTruong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length-1; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }

    //QuyetToan_ChungTu_BangDuLieu bang = new QuyetToan_ChungTu_BangDuLieu(iID_MaChungTu, arrGiaTriTimKiem, MaND, IPSua,MaLoai);
    //String BangID = "BangDuLieu";
    //int Bang_Height = 470;
    //int Bang_FixedRow_Height = 50;

    String[] arrDSMaCot = MucLucNganSachModels.arrDSTruong;
    String[] arrWidth = MucLucNganSachModels.arrDSTruongDoRong;
    String strLaySoLieuTuLuong = Url.Action("LoadDataLuong", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
    
    int iID_MaTrangThaiDuyet_TuChoi = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
    int iID_MaTrangThaiDuyet_TrinhDuyet = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
%>

<div class="box_tong">
    <div id="nhapform">
        <div id="form2">
    <table class="mGrid1">
    <tr>
        <%
            for (int j = 0; j < arrDSMaCot.Length-2; j++)
        {
            int iColWidth = Convert.ToInt16(arrWidth[j]) +4;
            if (j == 0) iColWidth = iColWidth + 1;
            String strAttr = String.Format("class='input1_4' onkeypress='jsQuyetToan_Search_onkeypress(event)' search-control='1' search-field='{1}' style='width:{0}px;height:22px;'", iColWidth - 2, arrDSMaCot[j]);
            %>
            <td style="text-align:left;width:<%=iColWidth%>px;">
                <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[arrDSMaCot[j]], TenTruong = arrDSMaCot[j], LoaiTextBox = "2", Attributes = strAttr })%>
            </td>
            <%
        }
        %>
       
    </tr>
    </table>
    <iframe id="ifrChiTietChungTu" width="100%" height="538px" src="<%= Url.Action("ChungTuChiTiet_Frame", "QuyetToan_ChungTuChiTiet", new {iID_MaChungTu=iID_MaChungTu,MaLoai=MaLoai})%>"></iframe>
        </div>
    </div>
</div>
            
