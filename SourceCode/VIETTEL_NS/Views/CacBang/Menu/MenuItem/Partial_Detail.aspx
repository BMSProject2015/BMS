<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

    <%       
        PartialModel dlChuyen = (PartialModel)Model;
        String ParentID = dlChuyen.ControlID;
        Dictionary<string, object> dicData = dlChuyen.dicData;
        string TenBang = (string)dicData["TenBang"];
        string TruongKhoa = (string)dicData["TruongKhoa"];
        NameValueCollection data = (NameValueCollection)dicData["data"];
        string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(User.Identity.Name, TenBang);
        string urlSua = Url.Action("Edit", "MenuItem", new { MaMenuItem = data[TruongKhoa] });
    %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">
			<h3><%= MyHtmlHelper.Label(data, "sTen", sDanhSachTruongCam)%>&nbsp; &nbsp;
			<span class="color_two">
			    (Ngày tạo: <%= MyHtmlHelper.Label(data, "dNgayTao", sDanhSachTruongCam)%>,
			    ngày sửa cuối: <%= MyHtmlHelper.Label(data, "dNgaySua", sDanhSachTruongCam)%>,
                số lần sửa: <%= MyHtmlHelper.Label(data, "iSoLanSua", sDanhSachTruongCam)%>)
            </span></h3>
		</td>
		<td  width="30%" align="right">
			<div style="padding-bottom:5px;"><a href="<%=urlSua%>" class="button4"><%=NgonNgu.LayXau("Sửa")%></a></div>
		</td>
	</tr>
</table>
<div class="box_tong">
	<div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXauChuHoa("Loại danh mục")%></span>
                </td>
            </tr>
        </table>
	</div>
	<div class="nhapform">		
	    <div class="form2">
            <table  cellpadding="0"  cellspacing=""="0" border="0" class="table_form2">
            <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên") %></div></td>
                <td class="td_form2_td2"><div>
                    <%= MyHtmlHelper.Label(data,"sTen", sDanhSachTruongCam)%>
                </div></td>                
            </tr>
             <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Menu cha") %></div></td>
                <td class="td_form2_td2"><div> 
                    <%= MyHtmlHelper.Label(data["MenuCha"],"iDM_MaMenuCha", sDanhSachTruongCam)%>
                </div></td>
            </tr>
              <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Menu chứa") %></div></td>
                <td class="td_form2_td2"><div>
                    <%= MyHtmlHelper.CheckBox(ParentID,data,"bMenuChua", sDanhSachTruongCam,"diabled=\"disabled\"")%>
                </div></td>
            </tr>    
            <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("URL") %></div></td>
                <td class="td_form2_td2"><div>
                    <%= MyHtmlHelper.Label(data,"sURL", sDanhSachTruongCam)%>
                </div></td>
            </tr>
             <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Hoạt động") %></div></td>
                <td class="td_form2_td2"><div>
                    <%= MyHtmlHelper.CheckBox(ParentID,data,"bHoatDong", sDanhSachTruongCam,"diabled=\"disabled\"")%>
                </div></td>
            </tr>                                        
        </table>  
      </div>
    </div>        
</div>    
       
    <% %>

