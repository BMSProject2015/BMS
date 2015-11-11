<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PhanBoTong_Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <%String ParentID = "Edit";
     String UserID = User.Identity.Name;
       String iID_MaPhanBo = Convert.ToString(ViewData["iID_MaPhanBo"]);
       DataTable dt = PhanBo_PhanBoModels.Get_dtChungTuPhanBo(iID_MaPhanBo,User.Identity.Name);
       DataTable dtChon=PhanBo_PhanBoModels.DanhSachPhanBoDuocChon(iID_MaPhanBo);
       using (Html.BeginForm("EditSubmit", "PhanBo_Tong", new { ParentID = ParentID, iID_MaPhanBo = iID_MaPhanBo }))
       {
   %>
  <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
  <%= Html.Hidden(ParentID + "_iID_MaPhanBo", iID_MaPhanBo)%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới chỉ tiêu phân bổ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa thông tin chỉ tiêu phân bổ")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 100%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid">
                  <tr>
                        <th align="center"> <input type="checkbox"  id="abc" onclick="CheckAll(this.checked)" /></th>
                        <th class="td_form2_td1" style="width: 15%;">
                            <div>Số chứng từ</div>
                        </th>
                        <th class="td_form2_td1">
                            <div>
                                <b>Ngày chứng từ</b>
                            </div>
                        </th>
                    </tr>
                <%for (int i = 0; i < dt.Rows.Count; i++)
                  {
                      String strChecked="";
                      String siID_MaPhanBo = Convert.ToString(dt.Rows[i]["iID_MaPhanBo"]);
                      String dNgayChungTu = Convert.ToString(dt.Rows[i]["dNgayChungTu"]);
                      String sTienToChungTu= Convert.ToString(dt.Rows[i]["sTienToChungTu"]);
                      String iSoChungTu= Convert.ToString(dt.Rows[i]["iSoChungTu"]);
                      String sPhanBoTong="";
                       for (int j = 0; j < dtChon.Rows.Count; j++)
                          {
                              sPhanBoTong = Convert.ToString(dtChon.Rows[j]["iID_MaPhanBo"]);
                              if (siID_MaPhanBo.Equals(sPhanBoTong))
                              {
                                  strChecked = "checked=\"checked\"";
                                  break;
                              }
                          }
                       %>
                    <tr>
                        <td align="center"><input type="checkbox" value="<%=siID_MaPhanBo %>" <%=strChecked %> check-group="PhanBo" id="iID_MaPhanBo" name="iID_MaPhanBo" /></td>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div> <b><%=sTienToChungTu + iSoChungTu%></b></div>
                        </td>
                        <td class="td_form2_td1">
                            <div>
                               <%=MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy}",dt.Rows[i]["dNgayChungTu"]),"dNgayChungTu") %>
                            </div>
                        </td>
                    </tr>
              <%} %>
                   
                </table>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                     <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td1">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="Lưu" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div >
<script type="text/javascript">
          function CheckAll(value) {
              $("input:checkbox[check-group='PhanBo']").each(function (i) {
                  this.checked = value;
              });
          }                                            
 </script>

<%
    }       
%>
</asp:Content>
