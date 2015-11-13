<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%@ Import Namespace="System.Reflection" %>

<%
    String ParentID = "", MaNhomNguoiDung = "";
    PropertyInfo[] properties = Model.GetType().GetProperties();

    for (int i = 0; i < properties.Length; i++)
    {
        switch (properties[i].Name)
        {
            case "ControlID":
                ParentID = (string)(properties[i].GetValue(Model, null));
                break;

            case "MaNhomNguoiDung":
                MaNhomNguoiDung = (string)(properties[i].GetValue(Model, null));
                break;
        }
    }
    String TenNhomNguoiDung = (string)(CommonFunction.LayTruong("QT_NhomNguoiDung", "iID_MaNhomNguoiDung", MaNhomNguoiDung, "sTen"));
    using (Html.BeginForm("EditLuatSubmit", "NhomNguoiDung", new { ControlID = ParentID, MaNhomNguoiDung = MaNhomNguoiDung }))
{
%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Luật")%> của nhóm <%=TenNhomNguoiDung%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Luật") %></div></td>
                <td class="td_form2_td5"><div>
                    <%
                        //Tạo dữ liệu cho ddlToChuc
                        DataTable dtLuat =BaoMat.dtDanhSachLuat(MaNhomNguoiDung);
                        SelectOptionList slLuat = new SelectOptionList(dtLuat, "iID_MaLuat", "sTen");
                    %>
                    <%=MyHtmlHelper.DropDownList(ParentID, slLuat, "", "iID_MaLuat", null, "style=\"width:200px;\"")%>
                </div></td>
            </tr>          
            <tr>
                <td class="td_form2_td1"><div>&nbsp;</div></td>
                <td class="td_form2_td5"><div>
                     <input type="submit" type="button" class="button6" value="<%=NgonNgu.LayXau("Cộng thêm")%>"/>
                </div></td>
            </tr>          
                  
        </table>
      </div>
    </div>
</div>
<div class="cao5px">&nbsp;</div>
<%} %>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Danh sách luật")%> của nhóm <%=TenNhomNguoiDung%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<table class="mGrid">
            <tr class="tr_form3">
                <th><%=NgonNgu.LayXau("Tên luật") %></th>
                <th style="width:10%">Hành động</th>
            </tr>
                <%
    
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT PQ_Luat.* FROM PQ_NhomNguoiDung_Luat INNER JOIN PQ_Luat ON PQ_NhomNguoiDung_Luat.iID_MaLuat = PQ_Luat.iID_MaLuat WHERE PQ_Luat.iTrangThai=1 AND iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung ORDER BY PQ_Luat.sTen";
                cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
                DataTable dt = Connection.GetDataTable(cmd);
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow Row = dt.Rows[i];
                    string urlDetail = Url.Action("Detail", "Luat", new { MaLuat = Row["iID_MaLuat"] });
                    string urlDelete = Url.Action("Delete_Luat", "NhomNguoiDung", new { MaNhomNguoiDung = MaNhomNguoiDung, MaLuat = Row["iID_MaLuat"] });
                    %>
                    <tr>
                        <td><%= MyHtmlHelper.ActionLink(urlDetail, (string)Row["sTen"])%></td>                
                        <td>
                            <%= MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"))%>
                        </td>
                    </tr>
        <%      }
                dt.Dispose();
                %>
        </table>
    </div>
</div>