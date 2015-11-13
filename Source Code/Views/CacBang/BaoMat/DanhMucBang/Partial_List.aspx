<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="System.Reflection" %>

<%
    String ParentID = "";
    PropertyInfo[] properties = Model.GetType().GetProperties();
    int i;
    for (i= 0; i < properties.Length; i++)
    {
        switch (properties[i].Name)
        {
            case "ControlID":
                ParentID = (string)(properties[i].GetValue(Model, null));
                break;
        }
    }
    String SQL = "SELECT * FROM PQ_DanhMucBang ORDER BY sTenBang;";
    DataTable dt = Connection.GetDataTable(SQL);
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td><span>Danh sách bảng</span></td>
                <td align="right" style="padding-right: 10px;">
                    <b class="btn_form3">
                        <%= MyHtmlHelper.ActionLink(Url.Action("Create", "DanhMucBang"), NgonNgu.LayXau("Thêm bảng"), "Create", null)%>
                    </b>
                </td>
            </tr>
        </table>
    </div>
    <table  cellpadding="0"  cellspacing=""="0" border="0" class="table_form3">
        <tr class="tr_form3">
            <td align="center" rowspan="2"><b><%=NgonNgu.LayXau("Tên") %></b></td>
            <td align="center" rowspan="2"><b><%=NgonNgu.LayXau("Tên hiển thị")%></b></td>
            <td align="center" rowspan="2"><b><%=NgonNgu.LayXau("Danh sách trường")%></b></td>
            <td align="center" colspan="6">
                <b>Các chức năng</b>
            </td>
            <td align="center" rowspan="2">
                <b>&nbsp;</b>
            </td>
        </tr>
        <tr class="tr_form3">
            <td align="center" style="width:40px">
                <b><%=NgonNgu.LayXau("Xem")%></b>
            </td>
            <td align="center" style="width:40px">
                <b><%=NgonNgu.LayXau("Thêm")%></b>
            </td>
            <td align="center" style="width:40px">
                <b><%=NgonNgu.LayXau("Xóa")%></b>
            </td>
            <td align="center" style="width:40px">
                <b><%=NgonNgu.LayXau("Sửa")%></b>
            </td>
            <td align="center" style="width:40px">
                <b><%=NgonNgu.LayXau("Chia sẻ")%></b>
            </td>
            <td align="center" style="width:90px">
                <b><%=NgonNgu.LayXau("Giao phụ trách")%></b>
            </td>
        </tr>
            <%
            int j=0;
            String strSua_TruongCam = NgonNgu.LayXau("Sửa");
            String TenBang, TenBangHT;
            String strXem = "", strThem = "", strXoa = "", strSua = "", strChiaSe = "", strGiaoPhuTrach = "";
            for (i = 0; i <= dt.Rows.Count - 1; i++)
            {
                DataRow Row = dt.Rows[i];
                TenBang = (string)Row["sTenBang"];
                TenBangHT = (string)Row["sTenBangHT"];
                strXem = "";
                strThem = "";
                strXoa = "";
                strSua = "";
                strChiaSe = "";
                strGiaoPhuTrach = "";
                if ((Boolean)Row["bXem"])
                {
                    strXem = " checked='checked'";
                }
                if ((Boolean)Row["bThem"])
                {
                    strThem = " checked='checked'";
                }
                if ((Boolean)Row["bXoa"])
                {
                    strXoa = " checked='checked'";
                }
                if ((Boolean)Row["bSua"])
                {
                    strSua = " checked='checked'";
                }
                if ((Boolean)Row["bChiaSe"])
                {
                    strChiaSe = " checked='checked'";
                }
                if ((Boolean)Row["bGiaoPhuTrach"])
                {
                    strGiaoPhuTrach = " checked='checked'";
                }
                strXem = String.Format("<input type=\"checkbox\" disabled {0}/>", strXem);
                strThem = String.Format("<input type=\"checkbox\" disabled {0}/>", strThem);
                strSua = String.Format("<input type=\"checkbox\" disabled {0}/>", strSua);
                strXoa = String.Format("<input type=\"checkbox\" disabled {0}/>", strXoa);
                strChiaSe = String.Format("<input type=\"checkbox\" disabled {0}/>", strChiaSe);
                strGiaoPhuTrach = String.Format("<input type=\"checkbox\" disabled {0}/>", strGiaoPhuTrach);
            
                string urlListTruong = Url.Action("Index", "DanhMucTruong", new { TenBang = TenBang });
                string urlEdit = Url.Action("Edit", "DanhMucBang", new { MaDanhMucBang = Row["iID_MaDanhMucBang"] });
                string urlDelete = Url.Action("Delete", "DanhMucBang", new { MaDanhMucBang = Row["iID_MaDanhMucBang"] });
                string urlSort = Url.Action("Sort", "DanhMucTruong", new { TenBang = TenBang });
                String classtr = "";
                if (i % 2 == 0)
                {
                    classtr = "class=\"alt\"";
                }
                %>
                <tr <%=classtr %>>
                    <td style="padding: 3px 2px;">
                        <%= TenBang%>
                    </td>
                    <td>
                        <%= TenBangHT %>
                    </td>
                    <td>
                        <%= MyHtmlHelper.ActionLink(urlListTruong, NgonNgu.LayXau("Danh sách trường"), "ListTruong", null)%> | 
                        <%= MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp trường"), "Sort", null)%>
                    </td>
                    <td align="center">
                        <%= strXem%>
                    </td>
                    <td align="center">
                        <%= strThem%>
                    </td>
                    <td align="center">
                        <%= strXoa%>
                    </td>
                    <td align="center">
                        <%= strSua%>
                    </td>
                    <td align="center">
                        <%= strChiaSe%>
                    </td>
                    <td align="center">
                        <%= strGiaoPhuTrach%>
                    </td>
                    <td>
                        <%= MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", null)%>
                        |
                        <%= MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", null)%>
                    </td>
                </tr>
             <%   
            }
            dt.Dispose();
            %>
    </table>
</div>