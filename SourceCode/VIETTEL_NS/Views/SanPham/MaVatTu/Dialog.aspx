<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%></title>
</head>
<body>
    <%
    String MaDivTen = Request.QueryString["idDiv"];
    String MaDivMaNhom = Request.QueryString["idDivMaNhom"];
    String OnSuccess = "";
    OnSuccess = Request.QueryString["OnSuccess"];
    String ParentID = "Edit";
    String MaDanhMuc = Request.QueryString["iID_MaDanhMuc"];
    String LoaiDanhMuc = Request.QueryString["LoaiDanhMuc"];
    SqlCommand cmd;
 
    String MaLoaiDanhMuc = "";
    cmd = new SqlCommand("SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang=@sTenBang");
    cmd.Parameters.AddWithValue("@sTenBang", ViewData["LoaiDanhMuc"]);
    MaLoaiDanhMuc = Connection.GetValueString(cmd, "");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                   "FROM DC_LoaiDanhMuc " +
                                                                   "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
    DataTable dt = Connection.GetDataTable(cmd);
    DataRow R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomLoaiVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    String iDM_MaNhomLoaiVatTu = Request.QueryString["MaNhomLoaiVatTu"];
    String iDM_MaNhomChinh = Request.QueryString["MaNhomChinh"];
    String iDM_MaNhomPhu = Request.QueryString["MaNhomPhu"];

    String MaChiTietVatTuGoiY = "";
    String sTenKhoa = "";
    String sTen = "";
    String sGhiChu = "";
    String bHoatDong = "true";
    String iID_MaDanhMucCha = "";
    if (!String.IsNullOrEmpty(MaDanhMuc))
    {
        cmd = new SqlCommand("SELECT * FROM DC_DanhMuc WHERE iID_MaDanhMuc=@iID_MaDanhMuc");
        cmd.Parameters.AddWithValue("@iID_MaDanhMuc", MaDanhMuc);
        dt = Connection.GetDataTable(cmd);
        cmd.Dispose();
        if (dt.Rows.Count > 0)
        {
            MaDanhMuc = Convert.ToString(dt.Rows[0]["iID_MaDanhMuc"]);
            sTenKhoa = Convert.ToString(dt.Rows[0]["sTenKhoa"]);
            sTen = Convert.ToString(dt.Rows[0]["sTen"]);
            sGhiChu = Convert.ToString(dt.Rows[0]["sGhiChu"]);
            bHoatDong = Convert.ToString(dt.Rows[0]["bHoatDong"]);
            iID_MaDanhMucCha = Convert.ToString(dt.Rows[0]["iID_MaDanhMucCha"]);

            switch (LoaiDanhMuc)
            {
                case "NhomChinh":
                    iDM_MaNhomLoaiVatTu = iID_MaDanhMucCha;
                    break;

                case "NhomPhu":
                    iDM_MaNhomChinh = iID_MaDanhMucCha;

                    cmd = new SqlCommand("SELECT iID_MaDanhMucCha FROM DC_DanhMuc WHERE iID_MaDanhMuc=@iID_MaDanhMuc");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iDM_MaNhomChinh);
                    iDM_MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    break;

                case "ChiTietVatTu":
                    iDM_MaNhomPhu = iID_MaDanhMucCha;

                    cmd = new SqlCommand("SELECT iID_MaDanhMucCha FROM DC_DanhMuc WHERE iID_MaDanhMuc=@iID_MaDanhMuc");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iDM_MaNhomPhu);
                    iDM_MaNhomChinh = Connection.GetValueString(cmd, "");
                    cmd.Dispose();

                    cmd = new SqlCommand("SELECT iID_MaDanhMucCha FROM DC_DanhMuc WHERE iID_MaDanhMuc=@iID_MaDanhMuc");
                    cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iDM_MaNhomChinh);
                    iDM_MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
                    cmd.Dispose();
                    break;
            }
        }
    }
    String Loi = "0";
    if (Convert.ToString(ViewData["Loi"]) == "1")
        Loi = "1";

    using (Ajax.BeginForm("Edit_Fast_Submit", "MaVatTu", new { ParentID = ParentID, OnSuccess = OnSuccess, MaDanhMuc = MaDanhMuc, LoaiDanhMuc = LoaiDanhMuc, MaDiv = MaDivTen, MaDivDate = MaDivMaNhom }, new AjaxOptions { }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaLoaiDanhMuc", MaLoaiDanhMuc)%>
<div style="background-color: #f0f9fe; background-repeat: repeat; border: solid 0px #ec3237">
    <div style="padding: 10px; text-align: center;">
        <h3>Nhập thông tin chi tiết vật tư</h3>
    </div>
    <div style="padding: 10px;">
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="100%">
                <%if (Convert.ToString(LoaiDanhMuc) != "DonViTinh")
                  {
                      if (LoaiDanhMuc == "NhomChinh" || LoaiDanhMuc == "NhomPhu" || LoaiDanhMuc == "ChiTietVatTu")
                      {%>
                     <tr>
                        <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu, iDM_MaNhomLoaiVatTu, "iDM_MaNhomLoaiVatTu", "", "onchange=\"ChonNhomLoaiVatTuDialog(this.value)\" style=\"width: 100%;\"")%></div>
                            <span id="error_Edit_iDM_MaNhomLoaiVatTu" style="color: Red;"></span>
                            <script type="text/javascript">
                                function ChonNhomLoaiVatTuDialog(iDM_MaNhomLoaiVatTu) {
                                    var url = '<%= Url.Action("get_dtNhomChinh?ParentID=#0&iDM_MaNhomLoaiVatTu=#1&iDM_MaNhomChinh=#2", "SanPham_DanhMuc") %>';
                                    url = url.replace("#0", "<%= ParentID %>");
                                    url = url.replace("#1", iDM_MaNhomLoaiVatTu);
                                    url = url.replace("#2", "<%= iDM_MaNhomChinh %>");
                                    $.getJSON(url, function (data) {
                                        document.getElementById("<%= ParentID %>_tdNhomChinh").innerHTML = data.ddlNhomChinh;
                                        ChonNhomChinhDialog(data.iDM_MaNhomChinh);
                                    });
                                }
                            </script>
                        </td>
                    </tr>
                    <%} if (LoaiDanhMuc == "NhomPhu" || LoaiDanhMuc == "ChiTietVatTu")
                    {%>
                    <tr>
                        <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                        <td class="td_form2_td5">
                             <div id="<%= ParentID %>_tdNhomChinh">
                                <% SanPham_DanhMucController.NhomChinh _NhomChinh = SanPham_DanhMucController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh);%>
                                <%=_NhomChinh.ddlNhomChinh %>
                            </div>
                            <span id="error_Edit_iDM_MaNhomChinh" style="color: Red;"></span>
                            <script type="text/javascript">
                                function ChonNhomChinhDialog(iDM_MaNhomChinh) {
                                    var url = '<%= Url.Action("get_dtNhomPhu?ParentID=#0&iDM_MaNhomChinh=#1&iDM_MaNhomPhu=#2", "SanPham_DanhMuc") %>';
                                    url = url.replace("#0", "<%= ParentID %>");
                                    url = url.replace("#1", iDM_MaNhomChinh);
                                    url = url.replace("#2", "<%= iDM_MaNhomPhu %>");
                                    $.getJSON(url, function (data) {
                                        document.getElementById("<%= ParentID %>_tdNhomPhu").innerHTML = data;
                                        MaChiTietVatTuGoiY()
                                    });
                                }
                            </script>
                        </td>
                    </tr>
                    <%} %>
                    <% if (LoaiDanhMuc == "ChiTietVatTu")
                    {%>
                    <tr>
                        <td class="td_form2_td1"><div><b>Nhóm phụ</b></div></td>
                        <td class="td_form2_td5">
                            <div id="<%= ParentID %>_tdNhomPhu"> 
                                <%= SanPham_DanhMucController.get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu)%>
                            </div>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td class="td_form2_td1"><div><b>Mã</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextBox(ParentID, sTenKhoa, "sTenKhoa", "", "style=\"width:50%;\" onkeypress=\"javascript:return AllowOnlyIntegers(event);\"")%>
                            <%if (Convert.ToString(LoaiDanhMuc) == "ChiTietVatTu" && String.IsNullOrEmpty(MaDanhMuc))
                            {%>&nbsp;*Mã gợi ý:&nbsp;<%=MyHtmlHelper.TextBox(ParentID, MaChiTietVatTuGoiY, "MaChiTietVatTuGoiY", "", "style=\"width:20%;\" readonly=\"readonly\"")%>
                            <script type="text/javascript">
                                MaChiTietVatTuGoiY()
                                function MaChiTietVatTuGoiY() {
                                    var MachiTietGoiYValue = document.getElementById("<%=ParentID %>_MaChiTietVatTuGoiY");
                                    var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
                                    var url = '<%= Url.Action("get_dtMaChiTietVatTu?ParentID=#0&iDM_MaNhomPhu=#1", "SanPham_DanhMuc") %>';

                                    url = url.replace("#0", "<%= ParentID %>");
                                    if (MaNhomPhu.length != 0)
                                        url = url.replace("#1", MaNhomPhu.options[MaNhomPhu.selectedIndex].value);
                                    else
                                        url = url.replace("#1", "");
                                    $.getJSON(url, function (data) {
                                        MachiTietGoiYValue.value = data;
                                    });
                                } 
                            </script>
                            <%}%>
                            <%--<%= Html.ValidationMessage(ParentID + "_" + "err_sTenKhoa")%>--%>
                            </div>
                            <span id="error_Edit_sTenKhoa" style="color: Red;"></span>
                            <script language="javascript" type="text/javascript">
                                function AllowOnlyIntegers(evt) {
                                    var value = document.getElementById('<%=ParentID %>_sTenKhoa').value;
                                    var charCode = (evt.which) ? evt.which : event.keyCode
                                    var LoaiDanhMuc = "<%= LoaiDanhMuc %>";
                                    if (charCode == 8) return true;
                                    if (LoaiDanhMuc.toString() == "ChiTietVatTu") {
                                        if (value.toString().length >= 5) return false;
                                    }
                                    else if (LoaiDanhMuc.toString() == "XuatXu") {
                                        if (value.toString().length >= 1) return false;
                                    }
                                    else {
                                        if (value.toString().length >= 2) return false;
                                    }
                                    if (charCode > 31 && (charCode < 48 || charCode > 57))
                                        return false;
                                    return true;
                                }
                            </script>
                        </td>
                    </tr>
                    <%}%>
                    <tr>
                        <td class="td_form2_td1"><div><b>Tên</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width: 98%;\"")%></div>
                            <span id="error_Edit_sTen" style="color: Red;"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"><div><b>Ghi chú</b></div></td>
                        <td class="td_form2_td5">
                            <div><%= MyHtmlHelper.TextArea(ParentID, sGhiChu, "sGhiChu", "", "style=\"width:98%;font:12px/20px Tahoma;height:50px;\"")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"><div><b>Hoạt động</b></div></td>
                        <td class="td_form2_td5">
                            <div><%= MyHtmlHelper.CheckBox(ParentID, bHoatDong, "bHoatDong","")%></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
	    <tr>
		    <td width="70%">&nbsp;</td>
		    <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
        	        <tr>
            	        <td>
            	            <input id="Register" type="submit" class="button4" value="Lưu"/>
            	        </td>
                        <td width="5px"></td>
                        <td>
                            <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
		    </td>
	    </tr>
    </table>
    <script type="text/javascript">
        ThongBao();
        function ThongBao() {
            if ('<%=Loi %>' == '1') {
                alert("Lỗi: Trùng dữ liệu!");
            }
        }
    </script>
</div>
<%
    } if (dt != null) { dt.Dispose(); };
%>
</body>
</html>
