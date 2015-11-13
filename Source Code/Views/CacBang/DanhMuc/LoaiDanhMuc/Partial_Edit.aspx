<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%
PartialModel dlChuyen = (PartialModel)Model;
String ParentID = dlChuyen.ControlID;
using (Html.BeginForm("EditSubmit", "LoaiDanhMuc", new { ControlID = ParentID }))
{
    Dictionary<string, object> dicData = dlChuyen.dicData;
    string TenBang = (string)dicData["TenBang"];
    string TruongKhoa = (string)dicData["TruongKhoa"];
    string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(Page.User.Identity.Name, TenBang);
    NameValueCollection data = (NameValueCollection)dicData["data"];
  
 
    %>
    <%= Html.Hidden(ParentID + "_" + TruongKhoa, data[TruongKhoa])%>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", dicData["DuLieuMoi"])%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">
			<h3><%
                    if (dicData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Nhập mới loại danh mục")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Thay đổi loại danh mục")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                    <span class="color_two">
                        (Ngày tạo: <%= MyHtmlHelper.Label(data, "dNgayTao", sDanhSachTruongCam)%>
                        ,ngày sửa cuối: <%= MyHtmlHelper.Label(data, "dNgaySua", sDanhSachTruongCam)%>,
                        số lần sửa: <%= MyHtmlHelper.Label(data, "iSoLanSua", sDanhSachTruongCam)%>)
                    </span>
            </h3>
		</td>
		<td width="30%" align="right">
		    <div style="display:none;">
                <input id="<%=ParentID%>_btnLuu" type="submit" value="Lưu" />
                <script type="text/javascript">
                    function <%=ParentID%>_btnLuu_click()
                    {
                        document.getElementById('<%=ParentID%>_btnLuu').click();
                        return false;
                    }
                </script>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	            <input type="submit" class="button" id="Submit1" value="Lưu" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>

<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Loại danh mục")%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
            <table cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
                <tr>
                    <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên") %></div></td>
                    <td class="td_form2_td5"><div>
                        <%= MyHtmlHelper.TextBox(ParentID, data, "sTen", sDanhSachTruongCam, "class=\"input1_2\"")%>
                        <%= Html.ValidationMessage(ParentID+"_"+"err_sTen")%>
                    </div></td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><%=NgonNgu.LayXau("Tên Bảng") %></td>
                    <td class="td_form2_td5"><div>
                        <%= MyHtmlHelper.TextBox(ParentID, data, "sTenBang", sDanhSachTruongCam, "class=\"input1_2\"")%>
                        <%= Html.ValidationMessage(ParentID + "_" + "err_sTenBang")%>
                    </div></td>
                </tr>
            </table>
          </div>
        </div>
</div>        
   
<%}%>   