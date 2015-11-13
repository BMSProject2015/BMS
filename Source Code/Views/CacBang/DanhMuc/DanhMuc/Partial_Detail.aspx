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
    string urlSua = Url.Action("Edit", "DanhMuc", new { MaDanhMuc = data[TruongKhoa], MaLoaiDanhMuc = data["iID_MaLoaiDanhMuc"] });
    
    %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0px 0 5px;">
    <tr>
        <td>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="21%" valign="bottom"><h3><%= MyHtmlHelper.Label(data, "sTen", sDanhSachTruongCam)%></h3></td>
                <td width="79%" valign="bottom">
                    <span class="color_two">
                    (Ngày tạo: <%= MyHtmlHelper.Label(data, "dNgayTao", sDanhSachTruongCam)%>
                    ,ngày sửa cuối: <%= MyHtmlHelper.Label(data, "dNgaySua", sDanhSachTruongCam)%>,
                    số lần sửa: <%= MyHtmlHelper.Label(data, "iSoLanSua", sDanhSachTruongCam)%>)
                    </span>
                </td>
              </tr>
            </table>
        </td>
        <td width="60px"><a href="<%=urlSua %>" class="button4"><%=NgonNgu.LayXau("Sửa") %></a></td>
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
              <td class="td_form2_td1" style="padding-top:10px;"><div><%=NgonNgu.LayXau("Loại danh mục") %></div></td>
            <td class="td_form2_td2" style="padding-top:10px;"><div>
                <%= MyHtmlHelper.Label(data["LoaiDanhMuc"], "iID_MaLoaiDanhMuc", sDanhSachTruongCam)%>
            </div></td>
        </tr>
        <tr>
            <td class="td_form2_td1" style="padding-top:10px;"><div><%=NgonNgu.LayXau("Tên") %></div></td>
            <td class="td_form2_td2" style="padding-top:10px;"><div>
                <%= MyHtmlHelper.Label(data, "sTen", sDanhSachTruongCam)%>
            </div></td>
        </tr>  
        <tr>
            <td class="td_form2_td1" style="padding-top:10px;"><div><%=NgonNgu.LayXau("Tên khóa") %></div></td>
            <td class="td_form2_td2" style="padding-top:10px;"><div>
                <%= MyHtmlHelper.Label(data, "sTenKhoa", sDanhSachTruongCam)%>
            </div></td>
        </tr>    
      <tr>
              <td class="td_form2_td1" style="padding-top:10px;"><div><%=NgonNgu.LayXau("Ghi chú") %></div></td>
             <td class="td_form2_td2" style="padding-top:10px;"><div>
                <%= MyHtmlHelper.Label(data, "sGhichu", sDanhSachTruongCam)%>
            </div></td>
        </tr>
        <tr>
              <td class="td_form2_td1" style="padding-top:10px;"><div><%=NgonNgu.LayXau("Hoạt động") %></div></td>
            <td class="td_form2_td2" style="padding-top:10px;"><div>
                <%= MyHtmlHelper.CheckBox(ParentID, data, "bHoatDong", sDanhSachTruongCam, "disabled=\"disabled\"")%>
            </div></td>
        </tr>
    </table>
    </div>
  </div>
</div>

<% %>
