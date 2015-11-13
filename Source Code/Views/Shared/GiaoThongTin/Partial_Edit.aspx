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

    SqlCommand cmd;
    String MaNhomNguoiDung_DuocGiao_Tin = Convert.ToString(ViewData["MaNhomNguoiDung_DuocGiao_Tin"]);
    String MaNhomNguoiDung_DuocGiao = Convert.ToString(ViewData["MaNhomNguoiDung_DuocGiao"]);
    String MaNguoiDung_DuocGiao = Convert.ToString(ViewData["MaNguoiDung_DuocGiao"]);


    //String SQL = String.Format("SELECT * FROM QT_NhomNguoiDung WHERE iID_MaNhomNguoiDung+'-'=LEFT('{0}',Len(iID_MaNhomNguoiDung)+1) ORDER BY iID_MaNhomNguoiDung", MaNhomNguoiDung_Tin);
    String SQL = String.Format("SELECT * FROM QT_NhomNguoiDung ORDER BY iID_MaNhomNguoiDung");

    DataTable dt = Connection.GetDataTable(SQL);
    int i,j,itg1 = 0, itg2;
    string strTG = "", strDoanTrang = "", MaNhomNguoiDung;
    for (i = 0; i < dt.Rows.Count; i++)
    {
        DataRow Row = dt.Rows[i];
        string urlXemNhom = Url.Action("Index", "GiaoThongTin", new { TenBang = TenBang, TenTruongKhoa = TenTruongKhoa, GiaTriKhoa = GiaTriKhoa, MaNhomNguoiDung_DuocGiao = Row["iID_MaNhomNguoiDung"], returnUrl = Request.QueryString["returnUrl"] });
        MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"].ToString();

        strDoanTrang = "";
        itg2 = CString.DemKyTu(Row["iID_MaNhomNguoiDung"].ToString(), '-');
        for (j = itg1 + 1; j <= itg2; j++)
        {
            strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        String strGiaTri = strDoanTrang;
        if (MaNhomNguoiDung_DuocGiao == MaNhomNguoiDung)
        {
            strGiaTri += String.Format("<b>{0}</b>", Row["sTen"].ToString());
            cmd = new SqlCommand();
            cmd.CommandText = String.Format("SELECT sID_MaNguoiDung, sID_MaNguoiDung as Ten FROM QT_NguoiDung WHERE iID_MaNhomNguoiDung='{0}'", MaNhomNguoiDung);
            DataTable dtNguoiDung = Connection.GetDataTable(cmd);
            cmd.Dispose();
            strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            DataRow RowNguoiDung=dtNguoiDung.NewRow();
            RowNguoiDung[0] = "";
            RowNguoiDung[1] = "Cả nhóm";
            dtNguoiDung.Rows.InsertAt(RowNguoiDung, 0);
            
            String MaNguoiDung;
            string urlGiaoThongTin;
            for (j = 0; j < dtNguoiDung.Rows.Count; j++)
            {
                MaNguoiDung = dtNguoiDung.Rows[j]["sID_MaNguoiDung"].ToString();
                urlGiaoThongTin = Url.Action("Edit", "GiaoThongTin", new { TenBang = TenBang, TenTruongKhoa = TenTruongKhoa, GiaTriKhoa = GiaTriKhoa, MaNhomNguoiDung_DuocGiao = MaNhomNguoiDung, MaNguoiDung_DuocGiao = MaNguoiDung, returnUrl = Request.QueryString["returnUrl"] });
                strGiaTri += "<br>";
                if (MaNguoiDung == MaNguoiDung_DuocGiao && MaNhomNguoiDung_DuocGiao_Tin == MaNhomNguoiDung)
                {
                    strGiaTri += String.Format("{0}- <b>{1}</b>", strDoanTrang, dtNguoiDung.Rows[j]["Ten"].ToString());
                }
                else
                {
                    strGiaTri += String.Format("{0}- {1}", strDoanTrang, MyHtmlHelper.ActionLink(urlGiaoThongTin, dtNguoiDung.Rows[j]["Ten"].ToString(), "Edit", sDanhSachChucNangCam));
                }
            }
            dtNguoiDung.Dispose();
        }
        else
        {
            strGiaTri += MyHtmlHelper.ActionLink(urlXemNhom, Row["sTen"].ToString(), "Edit", sDanhSachChucNangCam);
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