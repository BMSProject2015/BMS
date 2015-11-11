<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
        // String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        DataTable dtSo = rptKTTienGui_UyNhiemChiController.Lay_SoChungTu(iNamLamViec, iThang, iID_MaChungTu);
 
        String[] arriID_MaChungTu = iID_MaChungTu.Split(',');
        dtSo.Dispose();
        String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
        if (String.IsNullOrEmpty(LoaiBaoCao))
        {
            LoaiBaoCao = "KB";
        }  
        String inmuc = Convert.ToString(ViewData["inmuc"]);
        if (String.IsNullOrEmpty(inmuc))
        {
            inmuc = "1";
        }        
        using (Html.BeginForm("EditSubmit", "rptKTTienGui_UyNhiemChi", new { ParentID = ParentID,iID_MaChungTu=iID_MaChungTu, iNamLamViec = iNamLamViec, iThang = iThang, ChiSo = 0, LoaiBaoCao = LoaiBaoCao, inmuc = inmuc }))
        {
    %>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong">
                <div id="nhapform">
                    <div id="form2">
                      <div id="Div1">
                        <div id="Div2">
                             <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
                                <tr style="vertical-align:top;">
                                    <td rowspan="3" width = "49%">
                                       
                                       
                                       <fieldset style="border-top-right-radius:5px; border-top-left-radius:5px; -moz-border-radius-topright:5px;-moz-border-radius-topleft:5px;border:1px solid #dedede;margin-left:5px;margin-right:5px;">
                                                    <legend style="font-size:14px; font-family:Tahoma Arial; padding-left:10px;">Số UNC có trên CTGS</legend>
                                                    <div style="padding: 10px;">
                                                        <table class="mGrid">
                                                            <tr>
                                                                <td class="td_form2_td1" style="width: 40px; text-align: center">
                                                                    <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                                                </td>
                                                                <td  class="td_form2_td1" style="text-align:center; font-weight:bold;">   
                                                                    <div><%=NgonNgu.LayXau("Chọn đơn vị")%></div>                                                 
                                                                </td>
                                                            </tr>
                                                            <%for (int i = 0; i < dtSo.Rows.Count; i++)
                                                            {
                                                                String sSoChungTuChiTiet = Convert.ToString(dtSo.Rows[i]["sSoChungTuChiTiet"]);
                                                                String sTenDonVi_Nhan = Convert.ToString(dtSo.Rows[i]["sTenDonVi_Nhan"]);
                                                            %>
                                                            <tr>
                                                                 <td style="text-align:center">
                                                                    <input type="checkbox" value="<%=sSoChungTuChiTiet%>"  check-group="sSoChungTuChiTiet" id="sSoChungTuChiTiet" name="sSoChungTuChiTiet" />
                                                                </td>
                                                                <td style="text-align:left;"><%=sTenDonVi_Nhan%></td>
                                                            </tr>
                                                            <%} %>
                                                        </table>
                                                    </div>
                                                </fieldset>  
                                   </td>
                                   <td width = "50%">
                                     <fieldset style="border-top-right-radius:5px; border-top-left-radius:5px; -moz-border-radius-topright:5px;-moz-border-radius-topleft:5px;border:1px solid #dedede; margin-left:5px;margin-right:5px;">
                                                    <legend style="font-size:14px; font-family:Tahoma Arial; padding-left:10px; "> <%=NgonNgu.LayXau("Chọn loại mẫu  in ra")%> </legend>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "KB", LoaiBaoCao, "LoaiBaoCao", "")%> 
                                                        <b> 1. Mẫu kho bạc NN </b>
                                                    </p>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "NH", LoaiBaoCao, "LoaiBaoCao", "")%> 
                                                                    <b> 2. Mẫu ngân hàng - NN</b>
                                                    </p> 
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "TM", LoaiBaoCao, "LoaiBaoCao", "")%> 
                                                                    <b>3. Giấy rút tiền mặt</b>
                                                    </p>
                                                </fieldset>  
                                   <fieldset style="border-top-right-radius:5px; border-top-left-radius:5px; -moz-border-radius-topright:5px;-moz-border-radius-topleft:5px;border:1px solid #dedede; margin-left:5px;margin-right:5px;">
                                                    <legend style="font-size:14px; font-family:Tahoma Arial; padding-left:10px; "> <%=NgonNgu.LayXau("Chọn hình thức in ra")%> </legend>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "1", inmuc, "inmuc", "")%> 
                                                        <b> A. 2 liên trên trang A4</b>
                                                    </p>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "2", inmuc, "inmuc", "")%> 
                                                                    <b>B. 1 liên trên trang A4</b>
                                                    </p>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "3", inmuc, "inmuc", "")%> 
                                                                    <b>C. 2 số UNC trên trang A4</b>
                                                    </p>
                                                </fieldset>   
                                       

                                       
                                   </td>
                                </tr>
                                <tr>
                                    <td>
                                         <fieldset style="border-top-right-radius:5px; border-top-left-radius:5px; -moz-border-radius-topright:5px;-moz-border-radius-topleft:5px;border:1px solid #dedede; margin-left:5px;margin-right:5px;">
                                            <legend style="font-size:14px; font-family:Tahoma Arial; padding-left:10px; "> <%=NgonNgu.LayXau("In số liên")%> </legend>
                                            <p style="Padding:4px;">
                                                <%=MyHtmlHelper.TextBox(ParentID, "", "iSoLien","","")%> 
                                                <b> Nhập số liên</b>
                                            </p>
                                            
                                        </fieldset>  
                                    </td>
                                </tr>
                               <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
                            <tr><td  colspan="2" style="height: 10px; font-size: 5px;">&nbsp;</td></tr>
                            <tr>
                                <td style="width: 20%; text-align:center;" colspan="2">                                        
                                    <input type="submit" class="button4" value="Tiếp tục" style=" display:inline-block; margin-right:4px;" />
                                    <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" style=" display:inline-block; margin-left:4px;" />  
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
            $("input:checkbox[check-group='sSoChungTuChiTiet']").each(function (i) {
                this.checked = value;
            });
        }    
    </script>
       
</body>
</html>
