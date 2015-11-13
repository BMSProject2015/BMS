<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<%
    String Path = Url.Action("","");
    String MaLuat = "";
 %>
<%
    if (Request.IsAuthenticated) 
    {
        String SQL = "SELECT iID_MaLuat "+ 
                        "FROM QT_NguoiDung INNER JOIN PQ_NhomNguoiDung_Luat ON QT_NguoiDung.iID_MaNhomNguoiDung = PQ_NhomNguoiDung_Luat.iID_MaNhomNguoiDung "+
                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung";
        SqlCommand cmd = new SqlCommand(SQL);
        cmd.Parameters.AddWithValue("@sID_MaNguoiDung", Page.User.Identity.Name);
        MaLuat = Connection.GetValueString(cmd, "");
        cmd.Dispose();
        String MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(Page.User.Identity.Name);
        int MaxLength = 0;
        if (String.IsNullOrEmpty(MaLuat) == false || BaoMat.KiemTraNhomNguoiDungQuanTri(MaNhomNguoiDung))
        {
%>
        <%=CommonFunction.LayXauMenu(Path, MaLuat, 0, 0)%>
<%
        }
    }
%> 