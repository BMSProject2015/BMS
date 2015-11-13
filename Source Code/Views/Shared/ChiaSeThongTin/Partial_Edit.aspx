<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>


            

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>


<%
    String TenBang = Request.QueryString["TenBang"];
    String TenTruongKhoa = Request.QueryString["TenTruongKhoa"];
    String GiaTriKhoa = Request.QueryString["GiaTriKhoa"];
    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, TenBang);
    
    SqlCommand cmd = new SqlCommand();
    cmd.CommandText = String.Format("SELECT iID_MaNhomNguoiDung_DuocGiao, bPublic, iID_MaNhomNguoiDung_Public FROM {0} WHERE {1}=@{1}", TenBang, TenTruongKhoa);
    cmd.Parameters.AddWithValue("@" + TenTruongKhoa, GiaTriKhoa);
    DataTable dt = Connection.GetDataTable(cmd);
    cmd.Dispose();
    Boolean bPublic=Convert.ToBoolean(dt.Rows[0]["bPublic"]);
    String MaNhomNguoiDung_Public = Convert.ToString(dt.Rows[0]["iID_MaNhomNguoiDung_Public"]);
    String MaNhomNguoiDung_DuocGiao = Convert.ToString(dt.Rows[0]["iID_MaNhomNguoiDung_DuocGiao"]);
    dt.Dispose();


    //String SQL = String.Format("SELECT * FROM QT_NhomNguoiDung WHERE iID_MaNhomNguoiDung+'-'=LEFT('{0}',Len(iID_MaNhomNguoiDung)+1) ORDER BY iID_MaNhomNguoiDung", MaNhomNguoiDung_Tin);
    String SQL = String.Format("SELECT * FROM QT_NhomNguoiDung ORDER BY iID_MaNhomNguoiDung");

    dt = Connection.GetDataTable(SQL);
    int i,j;
    DataRow Row;
    int itg1 = 0, itg2;
    string strTG="",strDoanTrang = "",MaNhomNguoiDung;
    
    for (i = 0; i < dt.Rows.Count; i++)
    {
        Row = dt.Rows[i];
        string urlChiaSe = Url.Action("Edit", "ChiaSeThongTin", new { TenBang = TenBang, TenTruongKhoa = TenTruongKhoa, GiaTriKhoa = GiaTriKhoa, MaNhomNguoiDung_Public = Row["iID_MaNhomNguoiDung"], returnUrl = Request.QueryString["returnUrl"] });
        MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"].ToString();

        strDoanTrang = "";
        itg2 = CString.DemKyTu(MaNhomNguoiDung, '-');
        for (j = itg1 + 1; j <= itg2; j++)
        {
            strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        String strGiaTri = strDoanTrang;
        if (bPublic && MaNhomNguoiDung_Public == MaNhomNguoiDung)
        {
            strGiaTri += String.Format("<b>{0}</b>", Row["sTen"].ToString());
        }
        else
        {
            strGiaTri += MyHtmlHelper.ActionLink(urlChiaSe, Row["sTen"].ToString(), "Edit", sDanhSachChucNangCam);
        }
        strTG += string.Format("<tr>");

        if (i % 2 == 0)
        {
            strTG += string.Format("<td style=\"background-color:#dff0fb;\">{0}</td>", strGiaTri);
        }
        else
        {
            strTG += string.Format("<td>{0}</td>", strGiaTri);
        }
        strTG += string.Format("</tr>");
    }
    dt.Dispose();
%>

<table cellpadding="0"  cellspacing="0" border="0" class="table_form3" >
    <tr class="tr_form3">
        <td align="center"><b><%=NgonNgu.LayXau("Nhóm người dùng") %></b></td>
    </tr>
    <%=strTG %>
</table>