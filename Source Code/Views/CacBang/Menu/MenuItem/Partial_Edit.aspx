<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%
   PartialModel dlChuyen = (PartialModel)Model;
String ParentID = dlChuyen.ControlID;
using (Html.BeginForm("EditSubmit", "MenuItem", new { ControlID = ParentID }))
{
    Dictionary<string, object> dicData = dlChuyen.dicData;
    string TenBang = (string)dicData["TenBang"];
    string TruongKhoa = (string)dicData["TruongKhoa"];
    string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(Page.User.Identity.Name, TenBang);
    NameValueCollection data = (NameValueCollection)dicData["data"];

    %>
    <%= Html.Hidden(ParentID + "_" + TruongKhoa, data[TruongKhoa])%>
    <%= Html.Hidden(ParentID + "_iID_MaMenuItemCha", data["iID_MaMenuItemCha"])%>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", dicData["DuLieuMoi"])%>
<br />
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td style="width: 70%" align="left">
                	<span>
                        <%
                        if (dicData["DuLieuMoi"] == "1")
                        {
                            %>
                            <%=NgonNgu.LayXau("Nhập mới Menu")%>
                            <%
                        }
                        else
                        {
                            %>
                            <%=NgonNgu.LayXau("Thay đổi Menu")%>
                            <%
                        }
                        %>
                    </span>
                </td>
                <td style="padding-right: 10px;">
                    <table cellpadding="0" cellspacing="0" border="0" align="right">
    	                <tr>
        	                <td><input type="submit" type="button" class="button4" value="<%=NgonNgu.LayXau("Lưu")%>"/></td>
                            <td width="5px"></td>
                            <td><input type="button" value="<%=NgonNgu.LayXau("Hủy")%>" class="button4" onclick="javascript:history.go(-1);" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
                <td class="td_form2_td1" style="width: 20%"><div><%=NgonNgu.LayXau("Tên") %></div></td>
                <td class="td_form2_td5" style="width: 80%"><div>
                     <%= MyHtmlHelper.TextBox(ParentID, data, "sTen", sDanhSachTruongCam, "style=\"width:95%;\"")%>
                     <%= Html.ValidationMessage(ParentID+"_"+"err_sTen")%>
                </div></td>                
            </tr>
             <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("URL") %></div></td>
                <td class="td_form2_td5"><div>
                    <%= MyHtmlHelper.TextBox(ParentID, data, "sURL", sDanhSachTruongCam, "style=\"width:95%;\"")%>
                    <%= Html.ValidationMessage(ParentID + "_" + "err_sURL")%>
                </div></td>
            </tr> 
            <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Hoạt động") %></div></td>
                <td class="td_form2_td5" align="left"><div>
                    <%= MyHtmlHelper.CheckBox(ParentID, data, "bHoatDong", sDanhSachTruongCam)%>
                     <%= Html.ValidationMessage(ParentID + "_" + "err_bHoatDong")%>
                </div></td>
            </tr> 
            <tr>
                <td class="td_form2_td1"><div>&nbsp;</div></td>
                <td class="td_form2_td5" align="right"><div>
                    <table cellpadding="0" cellspacing="0" border="0" align="right">
        	            <tr>
            	            <td><input type="submit" type="button" class="button4" value="<%=NgonNgu.LayXau("Lưu")%>"/></td>
                            <td width="5px"></td>
                            <td><input type="button" value="<%=NgonNgu.LayXau("Hủy")%>" class="button4" onclick="javascript:history.go(-1);" /></td>
                        </tr>
                    </table>
                </div></td>
            </tr>                               
        </table>
      </div>
    </div>
 </div>
<%}%>



