<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienMat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CapPhat_ThongTri_Dialog</title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <div>
        <%
       
            String srcFile = Convert.ToString(ViewData["srcFile"]);
            String ParentID = "RutDuToan";
            String UserID = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            dtCauHinh.Dispose();
            String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
          //  String MaND = Request.QueryString["UserID"];
            DataTable dtSo = rptKTTienMat_PhieuThu1Controller.Lay_SoPT(iNamLamViec, iThang, iID_MaChungTu, UserID);
            SelectOptionList slSo = new SelectOptionList(dtSo, "sSoChungTuChiTiet", "sSoChungTuChiTiet,sNoiDung");
            dtSo.Dispose();
            String LoaiBaoCao = Request.QueryString["LoaiBaoCao"];
            if (String.IsNullOrEmpty(LoaiBaoCao))
            {
                LoaiBaoCao = "PT1";
            }
            DataTable dtLoaiBaoCao = rptKTTienMat_PhieuThu1Controller.DanhSach_LoaiBaoCao();
            SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoai", "TenLoai");
            dtLoaiBaoCao.Dispose();
            using (Html.BeginForm("EditSubmit", "rptKTTienMat_PhieuThu1", new { ParentID = ParentID, UserID = UserID, iNamLamViec = iNamLamViec, iThang = iThang, ChiSo = 0 }))
            {
        %>
        <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
        <div style="background-color: #ffffff; background-repeat: repeat">
            <div style="padding: 5px 1px 10px 1px;">
                <div class="box_tong">
                    <div id="nhapform">
                        <div id="form2">
                            <div id="Div1">
                                <div id="Div2" style="min-height:450px;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">                                       
                                        <tr>
                                            <td width ="49%" style="vertical-align:top;">
                                                <fieldset style="border-top-right-radius:5px; border-top-left-radius:5px; -moz-border-radius-topright:5px;-moz-border-radius-topleft:5px;border:1px solid #dedede;margin-left:5px;margin-right:5px; min-height: 450px;">
                                                    <legend style="font-size:14px; font-family:Tahoma Arial; padding-left:10px;">Chọn phiếu</legend>
                                                    <div style="padding: 10px;">
                                                        <table class="mGrid">
                                                            <tr>
                                                                <td class="td_form2_td1" style="width: 90px; text-align: center">
                                                                    <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                                                </td>
                                                                <td class="td_form2_td1" style="width: 110px; text-align: center">
                                                                    <div><b><%=NgonNgu.LayXau("Chọn số PT")%></b></div>
                                                                </td>
                                                            </tr>
                                                            <%for (int i = 0; i < dtSo.Rows.Count; i++)
                                                              {
                                                                  String sSoChungTuChiTiet = Convert.ToString(dtSo.Rows[i]["sSoChungTuChiTiet"]);
                                                                  String sNoiDung = Convert.ToString(dtSo.Rows[i]["sNoiDung"]);
                                                            %>
                                                            <tr>
                                                                <td style="width: 200px; text-align: center">
                                                                    <input type="checkbox" value="<%=sSoChungTuChiTiet%>" check-group="MaChungTuChiTiet"
                                                                        id="<%=ParentID %>_iID_MaChungTuChiTiet" name="<%=ParentID %>_iID_MaChungTuChiTiet" />
                                                                </td>
                                                                <td style="width: 500px; text-align: left">
                                                                    <%=sSoChungTuChiTiet%>
                                                                    -
                                                                    <%=sNoiDung%>
                                                                </td>
                                                            </tr>
                                                            <%} %>
                                                        </table>
                                                    </div>
                                                </fieldset>                                                
                                            </td>
                                            <td style="width: 50px;">
                                            </td>
                                            <td width = "49%">
                                                <fieldset style="border-top-right-radius:5px; border-top-left-radius:5px; -moz-border-radius-topright:5px;-moz-border-radius-topleft:5px;border:1px solid #dedede; margin-left:5px;margin-right:5px; min-height: 450px;">
                                                    <legend style="font-size:14px; font-family:Tahoma Arial; padding-left:10px; "> <%=NgonNgu.LayXau("Chọn kiểu   in")%> </legend>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "PT1", LoaiBaoCao, "LoaiBaoCao", "")%>
                                                        <b> 1 Phiếu trên toàn khổ A4</b>
                                                    </p>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "PT2", LoaiBaoCao, "LoaiBaoCao", "")%>
                                                                    <b> 1 Phiếu trên 1/2 khổ A4</b>
                                                    </p>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "PT3", LoaiBaoCao, "LoaiBaoCao", "")%>
                                                                    <b> 2 Phiếu trên toàn khổ A4</b>
                                                    </p>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "PT4", LoaiBaoCao, "LoaiBaoCao", "")%>
                                                                    <b> In phiếu trên toàn khổ A5</b>
                                                    </p>
                                                </fieldset>                                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 10px; font-size: 5px;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" colspan="3">
                                                <p style="text-align:center;">
                                                    <input type="submit" class="button4" value="Tiếp tục" style="margin-right:5px;display:inline-block;" /><input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" style="margin-left:5px;display:inline-block;" />
                                                </p>                                                
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%
}
        %>
    </div>
    <script type="text/javascript">
        function CheckAll(value) {
            $("input:checkbox[check-group='MaChungTuChiTiet']").each(function (i) {
                this.checked = value;
            });
        }                                            
    </script>
</body>
</html>
