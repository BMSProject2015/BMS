<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "QuyetToanNganSach";
        String iID_MaChungTu = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
           
       
        String iID_MaDonVi = "";
        iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iLoai = "";
        iLoai = Convert.ToString(ViewData["iLoai"]);
        String sSoCT = "", sGhiChu="";
        sGhiChu = Convert.ToString(CommonFunction.LayTruong("TN_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sGhiChu"));
        sSoCT = Convert.ToString(ViewData["sSoCT"]);
        String iThang = Convert.ToString(ViewData["iThang"]); 

        String iNam = Convert.ToString(ViewData["iNam"]); 
       
        //String LoaiTK = Request.QueryString["LoaiTK"];
        String UserID = User.Identity.Name;
        DataTable dtPhongBan = DonViModels.DanhSach_DonVi_ThuNop(iID_MaChungTu,sSoCT);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtPhongBan.Rows.Count > 0)
                iID_MaDonVi = Convert.ToString(dtPhongBan.Rows[0]["iID_MaDonVi"].ToString());
            
        }
        String SQL = @"SELECT DISTINCT sSoCT FROM TN_ChungTuChiTiet WHERE iTrangThai=1 AND sSoCT<>'' AND  iID_MaChungTu='" + iID_MaChungTu + "'";
        DataTable dtSoCT = Connection.GetDataTable(SQL);
        
        SelectOptionList slSoCT= new SelectOptionList(dtSoCT,"sSoCT","sSoCT");
        if (String.IsNullOrEmpty(sSoCT))
        {
            if (dtSoCT.Rows.Count > 0) sSoCT = Convert.ToString(dtSoCT.Rows[0]["sSoCT"]);
        }
        dtSoCT.Dispose();

        DataTable dtLoaiTT = ThuNopModels.getDSLoaiThongTri();
        SelectOptionList slLoaiTT = new SelectOptionList(dtLoaiTT, "MaLoai", "sTen");
        dtLoaiTT.Dispose();
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaDonVi", "sTen");

        //Lay ds Loại thông tri
        DataTable dtLoaiThongTri = rptThuNopThongTriController.getDSLoaiThongTri(User.Identity.Name);
        String iID_MaThongTri = Convert.ToString(ViewData["iID_MaThongTri"]); 
        if (String.IsNullOrEmpty(iID_MaThongTri))
        {
            if (dtLoaiThongTri.Rows.Count > 0)
                iID_MaThongTri = Convert.ToString(dtLoaiThongTri.Rows[0]["iID_MaThongTri"]);
        }
        String UrlEditThongTri = Url.Action("Index", "LoaiThongTri");
        using (Html.BeginForm("EditSubmit", "rptThuNopThongTri", new { ParentID = ParentID, iThang = iThang, iNam = iNam}))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
                <div>
                    <div style="width: 40%; float: left;">
                        <fieldset style="padding: 2px; border-radius: 5px; margin-right: 3px; min-height: 50px;">
                            <legend><b style="font-size: 12px;">&nbsp;
                                <%=NgonNgu.LayXau("Chọn Số CT")%></b></legend>
                            <div style="float: left; width: 98%; margin: 3px;">
                                <%=MyHtmlHelper.DropDownList(ParentID, slSoCT, sSoCT, "sSoCT", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon() ")%>
                            </div>
                        </fieldset>
                         <fieldset style="padding: 2px; border-radius: 5px; margin-right: 3px; min-height: 50px;">
                            <legend><b style="font-size: 12px;">&nbsp;
                                <%=NgonNgu.LayXau("Chọn đơn vị cần in thông tri")%></b></legend>
                            <div style="float: left; width: 98%; margin: 3px;" id="<%= ParentID %>_tdDonVi">
                              <%--  <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"  ")%>--%>
                                 <%rptThuNopThongTriController rpt = new rptThuNopThongTriController();%> 
                                <%=rpt.obj_DonVi(ParentID, iID_MaChungTu,sSoCT,iID_MaDonVi)%>
                            </div>
                        </fieldset>
                         <fieldset style="padding: 2px; border-radius: 5px; margin-right: 3px; min-height: 50px;">
                            <legend><b style="font-size: 12px;">&nbsp;
                                <%=NgonNgu.LayXau("Chọn loại thông tri")%></b></legend>
                            <div style="float: left; width: 98%; margin: 3px;" >
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiTT, iLoai, "iLoai", "", "class=\"input1_2\" style=\"width: 100%\"  ")%>
                          
                            </div>
                        </fieldset>
                         <fieldset style="padding: 2px; border-radius: 5px; margin-right: 3px; min-height: 200px;">
                            <legend><b style="font-size: 12px;">&nbsp;
                                <%=NgonNgu.LayXau("Ghi chú")%></b></legend>
                            <div style="float: left; width: 98%; margin: 3px;">
                                <%=MyHtmlHelper.TextArea(ParentID, sGhiChu, "sGhiChu", "", "class=\"input1_2\" style=\"width: 100%;height:200px;\"  ")%>
                            </div>
                        </fieldset>
                    </div>
                    <div style="width: 59%; float: right;">
                        <fieldset style="padding: 2px; border-radius: 5px; margin-right: 3px; min-height: 250px;">
                            <legend><b>&nbsp;
                                <%=MyHtmlHelper.ActionLink(UrlEditThongTri, "Chọn nội dung in thông tri")%></b></legend>
                            <div style="float: left; width: 98%; margin: 3px; height: 250px; overflow: scroll;">
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="mGrid">
                                    <tr>
                                        <th>
                                        </th>
                                        <th>
                                            <%-- Tên--%>
                                            Tên thông tri
                                        </th>
                                        <th>
                                            <%-- Nội dung--%>
                                            Tên loại ngân sách
                                        </th>
                                    </tr>
                                    <%for (int i = 0; i < dtLoaiThongTri.Rows.Count; i++)
                                      {%>
                                    <tr>
                                        <td style="text-align: center;">
                                            <%=MyHtmlHelper.Option("ThongTri", Convert.ToString(dtLoaiThongTri.Rows[i]["iID_MaThongTri"]), iID_MaThongTri, "iID_MaThongTri", "", "")%>
                                        </td>
                                        <td>
                                            <%=MyHtmlHelper.Label(dtLoaiThongTri.Rows[i]["sLoaiThongTri"].ToString(),"")%>
                                        </td>
                                        <td>
                                            <%=MyHtmlHelper.Label(dtLoaiThongTri.Rows[i]["sTenLoaiNS"].ToString(),"")%>
                                        </td>
                                    </tr>
                                    <%} %>
                                </table>
                            </div>
                        </fieldset>
                         <fieldset style="padding: 2px; border-radius: 5px; margin-right: 3px;">
                        <legend><b style="font-size: 12px;">&nbsp;
                            <%=NgonNgu.LayXau("Thêm mới Nội dung in thông tri")%></b></legend>
                        <div style="width: 98%; margin: 2px; text-align: right;">
                            <%--<div id="nhapform">
                                <div id="form2">--%>
                            <table border="0" cellspacing="10" cellpadding="1" width="100%">
                                 <tr>
                                    <td>
                                        <b>Thêm mới</b>
                                    </td>
                                    <td>
                                        <div style="float: left; margin: 3px;">
                                            <%=MyHtmlHelper.CheckBox(ParentID, "", "chkThemMoi", "", "")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <b>Tên thông tri</b>&nbsp;<span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <div style="margin: 3px;">
                                            <%=MyHtmlHelper.TextBox(ParentID, "", "sTen", "", "class=\"input1_2\" style=\"width: 100%;height:20px\" maxlength='150'")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Tên loại ngân sách</b>&nbsp;<span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <div style="margin: 3px;">
                                            <%=MyHtmlHelper.TextBox(ParentID, "", "sNoiDung", "", "class=\"input1_2\" style=\"width: 100%; height:20px\" maxlength='150'")%></div>
                                    </td>
                                </tr>
                            
                            </table>
                            <%-- </div>
                            </div>--%>
                        </div>
                    </fieldset>
                    </div>
                </div>
            </td>
        </tr>
                           
                    
        <tr>
            <td>
                <div style="margin-top: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 40%">
                            </td>
                            <td align="right">
                                <input type="submit" class="button4" value="Tiếp tục" />
                            </td>
                            <td style="width: 1%">
                                &nbsp;
                            </td>
                            <td align="left">
                                <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                            </td>
                            <td style="width: 40%">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <%} %>
       <script type="text/javascript">
           function Chon() {
               jQuery.ajaxSetup({ cache: false });
               var sSoCT = document.getElementById("<%= ParentID %>_sSoCT").value;
               var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaChungTu=#1&sSoCT=#2&iID_MaDonVi=#3", "rptThuNopThongTri") %>');
               url = unescape(url.replace("#0", "<%= ParentID %>"));
               url = unescape(url.replace("#1", "<%= iID_MaChungTu %>"));
               url = unescape(url.replace("#2", sSoCT));
               url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
               $.getJSON(url, function (data) {
                   document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
               });
           }  
         </script>
</body>
</html>
