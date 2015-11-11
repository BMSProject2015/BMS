<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%
        PartialModel dlChuyen = (PartialModel)Model;
        String ParentID = dlChuyen.ControlID;
        Dictionary<string, object> dicData = dlChuyen.dicData;
        string TenBang = "QT_NhomNguoiDung";
        String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, TenBang);
        %>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td><span>Nhóm người dùng</span></td>
                <td align="right" style="padding-right: 10px;">
                    <%=MyHtmlHelper.ButtonLink(Url.Action("Create", "NhomNguoiDung", new { MaNhomNguoiDungCha = 1 }), "Thêm nhóm người dùng", "Create", sDanhSachChucNangCam, "class=\"button_title\"")%>
                    <%=MyHtmlHelper.ButtonLink(Url.Action("Register", "Account"), "Đăng ký người dùng mới", null, null, "class=\"button_title\"")%>
                </td>
            </tr>
        </table>
    </div>           
    <table class="mGrid">
        <tr>
            <th align="center"><%=NgonNgu.LayXau("Nhóm người dùng") %></th>
            <th width="700px"><%=NgonNgu.LayXau("Hành động")%></th>
        </tr>
        <%
          
            String MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(User.Identity.Name);
            if (String.IsNullOrEmpty(MaNhomNguoiDung) == false)
            {
                String SQL = String.Format("SELECT * FROM QT_NhomNguoiDung WHERE iTrangThai=1 AND (iID_MaNhomNguoiDung = '{0}' OR LEFT(iID_MaNhomNguoiDung,{1})='{0}-') ORDER BY iID_MaNhomNguoiDung", MaNhomNguoiDung, MaNhomNguoiDung.Length + 1);
                //String SQL = String.Format("SELECT * FROM QT_NhomNguoiDung WHERE iDoiTuongNguoiDung>=0 ORDER BY iID_MaNhomNguoiDung", MaNhomNguoiDung, MaNhomNguoiDung.Length + 1);
                
                DataTable dt = Connection.GetDataTable(SQL);
                int i, j;
                DataRow Row;

                int itg1=0, itg2;
                //itg1 = CString.DemKyTu(MaNhomNguoiDung, '-');
                string strDoanTrang = "",vR;

                List<Boolean> arrTT = new List<Boolean>();
                List<int> arrCS = new List<int>();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    arrTT.Add(true);
                }
                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (arrTT[i])
                    {
                        CommonFunction.SapXepDanhSachNhomNguoiDung(dt, i, ref arrTT, ref arrCS);
                    }
                }

                
                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Row = dt.Rows[arrCS[i]];
                    string urlCreate = Url.Action("Create", "NhomNguoiDung", new { MaNhomNguoiDungCha = Row["iID_MaNhomNguoiDung"] });
                    string urlDetail = Url.Action("Detail", "NhomNguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    string urlEdit = Url.Action("Edit", "NhomNguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    string urlEditNguoiDung = Url.Action("Edit_NguoiDung", "NhomNguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    string urlDelete = Url.Action("Delete", "NhomNguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    string urlListNguoiDung = Url.Action("Index", "NguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    string urlListLuat = Url.Action("Edit_Luat", "NhomNguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    string urlListBangMau = Url.Action("Index", "BangMauNhomNguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    string urlSort = Url.Action("Sort", "NhomNguoiDung", new { MaNhomNguoiDung = Row["iID_MaNhomNguoiDung"] });
                    String strTG = "";
                    strTG = MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm mục con"), "Create", sDanhSachChucNangCam);
                    if (BaoMat.KiemTraNhomNguoiDungQuanTri(Row["iID_MaNhomNguoiDung"].ToString()) == false)
                    {
                        strTG += " | ";
                        strTG += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", sDanhSachChucNangCam);
                        strTG += " | ";
                        strTG += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", sDanhSachChucNangCam);
                        strTG += " | ";
                        strTG += MyHtmlHelper.ActionLink(urlEditNguoiDung, NgonNgu.LayXau("Người dùng"), "Edit", sDanhSachChucNangCam);
                        strTG += " | ";
                        strTG += MyHtmlHelper.ActionLink(urlListLuat, NgonNgu.LayXau("Luật"), "Edit", sDanhSachChucNangCam);
                    }
                    
                    strDoanTrang = "";
                    itg2 = CString.DemKyTu(Row["iID_MaNhomNguoiDung"].ToString(), '-');
                    for (j = itg1 + 1; j <= itg2; j++)
                    {
                        strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                    
                    vR = string.Format("<tr>");

                    if (i % 2 == 0)
                    {
                        vR += string.Format("<td style=\"background-color:#dff0fb;\">{0}{1}</a></td>", strDoanTrang, Row["sTen"]);
                        vR += string.Format("<td style=\"background-color:#dff0fb;\">{0}</td>", strTG);
                    }
                    else
                    {
                        vR += string.Format("<td>{0}{1}</a></td>", strDoanTrang, Row["sTen"]);
                        vR += string.Format("<td>{0}</td>", strTG);
                    }
                    vR += string.Format("</tr>");
                    %>
                        <%=vR%>
                    <%
                }
                dt.Dispose();
            }
        %>
    </table>
</div>