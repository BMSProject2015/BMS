<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="System.Reflection" %>
<%
String ParentID = "", MaLuat = "";
PropertyInfo[] properties = Model.GetType().GetProperties();
int i;
for (i= 0; i < properties.Length; i++)
{
    switch (properties[i].Name)
    {
        case "ControlID":
            ParentID = (string)(properties[i].GetValue(Model, null));
            break;

        case "MaLuat":
            MaLuat = (string)(properties[i].GetValue(Model, null));
            break;
    }
}   
SqlCommand cmd = new SqlCommand();
cmd.CommandText = "SELECT * FROM PQ_Bang_ChucNangCam WHERE iID_MaLuat=@iID_MaLuat ORDER BY sTenBang";
cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
DataTable dt = Connection.GetDataTable(cmd);
//String SQL = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
String SQL = "SELECT * FROM PQ_DanhMucBang WHERE bHoatDong=1 ORDER BY iSTT, sTenBangHT;";
DataTable dtBang = Connection.GetDataTable(SQL);
%>
<div id="nhapform">	
	<table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
	        <td width="70%"> &nbsp;</td>
	        <td  width="30%" align="right" style="padding: 5px 10px 0px 0px;">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
        	        <tr>
            	        <td><input type="button" value="<%=NgonNgu.LayXau("Sửa")%>" class="button4" onclick="javascript:location.href='<%=Url.Action("Edit", "ChucNangCam", new { MaLuat = MaLuat })%>';" /></td>
                        <td width="5px"></td>
                        <td><input type="button" value="Hủy" class="button4" onclick="javascript:history.go(-1);" /></td>
                    </tr>
                </table>
	        </td>
        </tr>
    </table>	    
	<table class="mGrid">
        <tr>
            <th align="center" style="width:200px"><%=NgonNgu.LayXau("Mã") %></th>
            <th align="center"><%=NgonNgu.LayXau("Tên") %></th>
            <th align="center" style="width:100px"><%=NgonNgu.LayXau("Trường cấm")%></th>
            <th align="center" style="width:40px"><%=NgonNgu.LayXau("Xem")%></th>
            <th align="center" style="width:40px"><%=NgonNgu.LayXau("Thêm")%></th>
            <th align="center" style="width:40px"><%=NgonNgu.LayXau("Xóa")%></th>
            <th align="center" style="width:40px"><%=NgonNgu.LayXau("Sửa")%></th>
            <th align="center" style="width:40px"><%=NgonNgu.LayXau("Chia sẻ")%></th>
            <th align="center" style="width:90px"><%=NgonNgu.LayXau("Giao phụ trách")%></th>
        </tr>
            <%
            int d=-1,j=0;
            String strSua_TruongCam = NgonNgu.LayXau("Sửa");
            String TenBang, TenBangHT;
            String strXem = "", strThem = "", strXoa = "", strSua = "", strChiaSe = "", strGiaoPhuTrach= "", ChucNangCam;
            for (i = 0; i <= dtBang.Rows.Count - 1; i++)
            {
                DataRow Row = dtBang.Rows[i];
                TenBang = (string)Row["sTenBang"];
                TenBangHT = (string)Row["sTenBangHT"];

                string urlEdit_TruongCam = Url.Action("Edit", "TruongCam", new { MaLuat = MaLuat, TenBang = TenBang });
                if (TenBang.StartsWith("aspnet") == false && TenBang.StartsWith("PQ_") == false && TenBang.StartsWith("QT_") == false && TenBang != "sysdiagrams")
                {
                    d = d + 1;
                    strXem = "";
                    strThem = "";
                    strXoa = "";
                    strSua = "";
                    strChiaSe = "";
                    strGiaoPhuTrach = "";
                    for (j = 0; j <= dt.Rows.Count - 1; j++ )
                    {
                        if (TenBang == (String)(dt.Rows[j]["sTenBang"]))
                        {
                            ChucNangCam = (String)(dt.Rows[j]["sChucNang"]);
                            if (ChucNangCam.IndexOf("Detail") >= 0)
                            {
                                strXem = " checked='checked'";
                            }
                            if (ChucNangCam.IndexOf("Create") >= 0)
                            {
                                strThem = " checked='checked'";
                            }
                            if (ChucNangCam.IndexOf("Delete") >= 0)
                            {
                                strXoa = " checked='checked'";
                            }
                            if (ChucNangCam.IndexOf("Edit") >= 0)
                            {
                                strSua = " checked='checked'";
                            }
                            if (ChucNangCam.IndexOf("Share") >= 0)
                            {
                                strChiaSe = " checked='checked'";
                            }
                            if (ChucNangCam.IndexOf("Responsibility") >= 0)
                            {
                                strGiaoPhuTrach = " checked='checked'";
                            }
                            break;
                        }
                    }
                    if ((Boolean)Row["bXem"])
                    {
                        strXem = String.Format("<input type=\"checkbox\" disabled {0}/>",strXem);
                    }
                    else
                    {
                        strXem = "&nbsp;";
                    }
                    if ((Boolean)Row["bThem"])
                    {
                        strThem = String.Format("<input type=\"checkbox\" disabled {0}/>", strThem);
                    }
                    else
                    {
                        strThem = "&nbsp;";
                    }
                    if ((Boolean)Row["bSua"])
                    {
                        strSua = String.Format("<input type=\"checkbox\" disabled {0}/>", strSua);
                    }
                    else
                    {
                        strSua = "&nbsp;";
                    }
                    if ((Boolean)Row["bXoa"])
                    {
                        strXoa = String.Format("<input type=\"checkbox\" disabled {0}/>", strXoa);
                    }
                    else
                    {
                        strXoa = "&nbsp;";
                    }
                    if ((Boolean)Row["bChiaSe"])
                    {
                        strChiaSe = String.Format("<input type=\"checkbox\" disabled {0}/>", strChiaSe);
                    }
                    else
                    {
                        strChiaSe = "&nbsp;";
                    }
                    if ((Boolean)Row["bGiaoPhuTrach"])
                    {
                        strGiaoPhuTrach = String.Format("<input type=\"checkbox\" disabled {0}/>", strGiaoPhuTrach);
                    }
                    else
                    {
                        strGiaoPhuTrach = "&nbsp;";
                    }

                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                %>
                <tr <%=classtr %>>
                    <td style="padding: 3px 2px;"><%= TenBang%></td>
                    <td><%= TenBangHT%></td>
                    <td align="center"><%= MyHtmlHelper.ActionLink(urlEdit_TruongCam, strSua_TruongCam)%></td>
                    <td align="center"><%= strXem%></td>
                    <td align="center"><%= strThem%></td>
                    <td align="center"><%= strXoa%></td>
                    <td align="center"><%= strSua%></td>
                    <td align="center"><%= strChiaSe%></td>
                    <td align="center"><%= strGiaoPhuTrach%></td>
                </tr>
    <%      
        }
    }
    dt.Dispose();
    dtBang.Dispose();
    %>
    </table>
</div>