<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    string iID_MaCanBo = Convert.ToString(props["iID_MaCanBo"].GetValue(Model));
    String ParentID = "KTCT";
    String sDacDiemLichSu = "", sDacDiemBanThan = "", sThamGiaToChucNuocNgoai = "", sThanNhanNuocNgoai = "",
            sChienTruongDaQua = "", sCongViecTruocCM = "", sTinhHinhNhaO = "";
    if (String.IsNullOrEmpty(iID_MaCanBo) == false && iID_MaCanBo != "")
    {
        var dtCanBo = CanBo_HoSoNhanSuModels.GetChiTiet(iID_MaCanBo);
        if (dtCanBo.Rows.Count > 0)
        {
            DataRow DR = dtCanBo.Rows[0];

            sDacDiemLichSu = HamChung.ConvertToString(DR["sDacDiemLichSu"]);
            sDacDiemBanThan = HamChung.ConvertToString(DR["sDacDiemBanThan"]);
            sThamGiaToChucNuocNgoai = HamChung.ConvertToString(DR["sThamGiaToChucNuocNgoai"]);
            sThanNhanNuocNgoai = HamChung.ConvertToString(DR["sThanNhanNuocNgoai"]);
            sChienTruongDaQua = HamChung.ConvertToString(DR["sChienTruongDaQua"]);
            sCongViecTruocCM = HamChung.ConvertToString(DR["sCongViecTruocCM"]);
            sTinhHinhNhaO = HamChung.ConvertToString(DR["sTinhHinhNhaO"]);
        }
    }
    using (Html.BeginForm("EditTinhHinhKT", "CanBo_HoSoNhanSu", new { ParentID = ParentID, iID_MaCanBo = iID_MaCanBo }))
    {
%>
  <%= Html.Hidden(ParentID + "_DuLieuMoi", 0)%>
<table border="0" cellpadding="10" cellspacing="10" width="100%">
    <tr>
        <td>
            <span style="color: #006400; font-size: 14px; font-weight: bold;">ĐẶC ĐIỂM LỊCH SỬ BẢN
                THÂN</span>
        </td>
    </tr>
    <tr>
        <td>
            Khai rõ: bị bắt, bị tù (từ ngày tháng năm nào đến ngày tháng năm nào, ở đâu), đã
            khai báo cho ai, những vấn đề gì?
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <%=MyHtmlHelper.TextArea(ParentID, sDacDiemLichSu, "sDacDiemLichSu", "", "class=\"input1_2\" tab-index='-1' style=\"width:99%;resize:none;\"")%>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            Bản thân có làm việc trong chế độ cũ (cơ quan, đơn vị nào, địa điểm, chức danh,
            chức vụ, thời gian làm việc, ...)
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <%=MyHtmlHelper.TextArea(ParentID, sDacDiemBanThan, "sDacDiemBanThan", "", "class=\"input1_2\"  style=\"width:99%;resize:none;\"")%>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            Chiến trường đã qua (thời gian) chiến đấu, phục vụ chiến đấu (tính chất, đối tượng,
            cương vị, đơn vị)
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <%=MyHtmlHelper.TextArea(ParentID, sChienTruongDaQua, "sChienTruongDaQua", "", "class=\"input1_2\"  style=\"width:99%;resize:none;\"")%>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            Trước khi tham gia CM làm gì? ở đâu? quan hệ CT XH:
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <%=MyHtmlHelper.TextArea(ParentID, sCongViecTruocCM, "sCongViecTruocCM", "", "class=\"input1_2\"  style=\"width:99%;resize:none;\"")%>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            Tình hình nhà ở (hình thức sở hữu, loại nhà, diện tích):
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <%=MyHtmlHelper.TextArea(ParentID, sTinhHinhNhaO, "sTinhHinhNhaO", "", "class=\"input1_2\"  style=\"width:99%;resize:none;\"")%>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <span style="color: #006400; font-size: 14px; font-weight: bold;">QUAN HỆ VỚI NGƯỜI
                NƯỚC NGOÀI</span>
        </td>
    </tr>
    <tr>
        <td>
            Tham gia hoặc có quan hệ với các tổ chức chính trị, kinh tế, xã hội nào ở nước ngoài
            (làm gì, tổ chức nào, đặt trụ sở ở đâu ..?
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <%=MyHtmlHelper.TextArea(ParentID, sThamGiaToChucNuocNgoai, "sThamGiaToChucNuocNgoai", "", "class=\"input1_2\" tab-index='-1' style=\"width:99%;resize:none;\"")%>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            Có thân nhân (bố, mẹ, vợ, chồng, con, anh chị em ruột) ở nước ngoài (làm gì, địa
            chỉ ..)?
        </td>
    </tr>
    <tr>
        <td>
            <div>
                <%=MyHtmlHelper.TextArea(ParentID, sThanNhanNuocNgoai, "sThanNhanNuocNgoai", "", "class=\"input1_2\"  style=\"width:99%;resize:none;\"")%>
            </div>
        </td>
    </tr>
    <tr>
        <td style="height: 30px;">
        </td>
    </tr>
    <tr>
        <td align="center" colspan="6" style="background-color: Window;">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <input type="submit" class="button" id="Submit1" value="Lưu" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td align="center" width="100px">
                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<%} %>