<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CapPhat_ThongTri_Dialog</title>
</head>
<body>
    <div>
    <%
        String ParentID = "CapPhat_ThongTri";
        String iID_MaCapPhat = Request.QueryString["iID_MaCapPhat"];
        DataTable dt = CapPhat_ChungTuModels.dt_DonViCapPhat(iID_MaCapPhat);
        String sLoaiThongTri=Request.QueryString["sLoaiThongTri"];
        if (String.IsNullOrEmpty(sLoaiThongTri)) sLoaiThongTri = "sNG";

        using (Html.BeginForm("EditSubmit", "rptCapPhat_ThongTri_78", new { ParentID = ParentID, iID_MaCapPhat = iID_MaCapPhat }))
        {
    %>
    <div style="background-color: #ffffff; background-repeat: repeat">
        <div style="padding: 5px 1px 10px 1px;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Chọn dơn vị:</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                      <div id="Div1">
                        <div id="Div2">
                             <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td align="left" colspan="2" valign="top" style="width: 40%">                                   
                                    <div style="width:100%;height:250px; overflow:scroll;position:relative;">
                                        <%Html.RenderPartial("~/Views/DungChung/DonVi/DonVi_DanhSach.ascx", new { ControlID = "DonVi", MaND = User.Identity.Name, iSoCot = 2, sMaDonVi = "", dtDonVi=dt }); %>
                                    </div>
                                </td>
                                <td style="width: 3%">&nbsp;</td>
                                <td style="width: 40%">
                                    <table class="mGrid">
                                        <tr>                                            
                                            <th style="width: 120px;" colspan="2">Chọn loại thông tri</th>                                                                                       
                                        </tr>
                                    </table>
                                    <div style="width:100%;height:250px; overflow:scroll;position:relative;">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mGrid">
                                        <tr>
                                            <td><%=MyHtmlHelper.Option(ParentID, "sNG", "sNG", "sLoaiThongTri", "")%></td>   
                                            <td><b>Ngành</b></td>                                        
                                        </tr>
                                        <tr>
                                            <td><%=MyHtmlHelper.Option(ParentID, "sTM", "sTM", "sLoaiThongTri", "")%></td>                                           
                                            <td><b>Tiểu mục</b></td>                                        
                                        </tr>
                                        <tr>
                                            <td><%=MyHtmlHelper.Option(ParentID, "sM", "sM", "sLoaiThongTri", "")%></td>                                           
                                            <td><b>Mục</b></td>                                        
                                        </tr>
                                        <tr>
                                            <td><%=MyHtmlHelper.Option(ParentID, "sLNS", "sLNS", "sLoaiThongTri", "")%></td>                                           
                                            <td><b>Loại ngân sách</b></td>                                        
                                        </tr>
                                    </table>
                                    </div>
                                </td>
                            </tr>
                            <tr><td  colspan="2" style="height: 10px; font-size: 5px;">&nbsp;</td></tr>
                            <tr>
                                
                                <td style="width: 20%">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td align="right" style="width: 10%"><input type="submit" class="button4" value="Tiếp tục" /></td>
                                            <td style="width: 1%">&nbsp;</td>
                                            <td align="right" style="width: 10%"><input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');"/></td>
                                            <td style="width: 1%">&nbsp;</td>
                                        </tr>
                                    </table>
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
            function setCheckboxes() {
                $('input:checkbox[group-index="1"]').each(function (i) {
                    this.checked = document.getElementById('chkCheckAll').checked;
                });
            }    
    </script>
</body>
</html>
