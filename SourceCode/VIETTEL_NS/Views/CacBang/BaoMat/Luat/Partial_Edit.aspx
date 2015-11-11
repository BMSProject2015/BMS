<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>


<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%
PartialModel dlChuyen = (PartialModel)Model;
String ParentID = dlChuyen.ControlID;
using (Html.BeginForm("EditSubmit", "Luat", new { ControlID = ParentID }))
{
Dictionary<string, object> dicData = dlChuyen.dicData;
string TenBang = (string)dicData["TenBang"];
string TruongKhoa = (string)dicData["TruongKhoa"];
string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(Page.User.Identity.Name, TenBang);
NameValueCollection data = (NameValueCollection)dicData["data"];
    

%>
<%= Html.Hidden(ParentID + "_" + TruongKhoa, data[TruongKhoa])%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", dicData["DuLieuMoi"])%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>
                    <%
                    if (dicData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Nhập mới luật")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Thay đổi luật")%>
                        <%
                    }
                    %>
                    </span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên luật") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.TextBox(ParentID, data, "sTen", sDanhSachTruongCam, "style=\"width: 50%;\"")%>
                     <%= Html.ValidationMessage(ParentID+"_"+"err_sTen")%>
                </div></td>                
            </tr>
            <%
                if (dicData["DuLieuMoi"] == "1")
                {
                    SelectOptionList slLuat = (SelectOptionList)dicData["slLuat"];
            %>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Dựa vào luật")%></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.DropDownList(ParentID, slLuat, null, "iID_MaLuat_Base")%> 
                </div></td>                
            </tr>
              <%}%>
        </table>
      </div>
    </div>
</div>
<div class="cao5px">&nbsp;</div>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
	    <td width="70%">&nbsp;
	    </td>
	    <td  width="30%" align="right">						
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td><input type="submit" type="button" class="button4" value="<%=NgonNgu.LayXau("Lưu")%>"/></td>
                    <td width="5px"></td>
                    <td><input type="button" value="Hủy" class="button4" onclick="javascript:history.go(-1);" /></td>
                </tr>
            </table>         
	    </td>
    </tr>
</table>
<div class="cao5px">&nbsp;</div>
<%}%>