<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.Reflection" %>

<%
    String ParentID = "", MaNhomNguoiDung = "";
    PropertyInfo[] properties = Model.GetType().GetProperties();
    int i;
    
    for (i= 0; i < properties.Length; i++)
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

    String SQL;
    SQL = "SELECT * " +
          "FROM QT_NguoiDung  " +
          "WHERE iTrangThai=1 AND iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung " +
          "ORDER BY sID_MaNguoiDung";
    SqlCommand cmd = new SqlCommand();
    cmd.CommandText = SQL;
    cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
    DataTable dt = Connection.GetDataTable(cmd);

    cmd = new SqlCommand("SELECT * FROM QT_NhomNguoiDung WHERE iTrangThai=1 AND iID_MaNhomNguoiDung = @iID_MaNhomNguoiDung");
    cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
    DataTable dtNhomNguoiDung = Connection.GetDataTable(cmd);
    String TenNhomNguoiDung = Convert.ToString(dtNhomNguoiDung.Rows[0]["sTen"]);
    int MaDonVi = Convert.ToInt16(dtNhomNguoiDung.Rows[0]["iID_MaDonVi"]);
    cmd.Dispose();
    dtNhomNguoiDung.Dispose();
    
   
    string TenBang = "QT_NhomNguoiDung";
    string TruongKhoa = "iID_MaNhomNguoiDung";
    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, TenBang);


    //Trình độ quản lý nhà nước
    var dtDoiTuong = DanhMucModels.DT_DoiTuong(false, "--- Lựa chọn ---");
    SelectOptionList slMaDoiTuong = new SelectOptionList(dtDoiTuong, "MaDT", "TenDT");
    if (dtDoiTuong != null) dtDoiTuong.Dispose();
    
    
using (Html.BeginForm("EditNguoiDungSubmit", "NhomNguoiDung", new { ControlID = ParentID, MaNhomNguoiDung = MaNhomNguoiDung }))
{
%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Người dùng")%> của nhóm <%=TenNhomNguoiDung%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
                <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tài khoản") %></div></td>
                <td class="td_form2_td5"><div>
                    <input id="sID_MaNguoiDung" name="sID_MaNguoiDung" class="input1_2" tabindex="-1"/>
                    <span id="error" style="display:none;"></span>
                </div></td>
               
            </tr>     
            <tr>
              <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Đối tượng") %></div></td>
               <td class="td_form2_td5">
               
               <div>
                                                    <%=MyHtmlHelper.DropDownList("CauHinh", slMaDoiTuong, "", "iDoiTuongNguoiDung", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                                </div>
               </td>

            </tr>     
            <tr>
                <td class="td_form2_td1"><div>&nbsp;</div></td>
                <td class="td_form2_td5"><div>
                     <input id="AddUser" type="button" class="button6" value="<%=NgonNgu.LayXau("Cập nhật")%>"/>
                </div></td>
            </tr>          
                  
        </table>
      </div>
    </div>
</div>
<div style="display:none">
    <input id="btnOK" type="submit" value="OK"/>
    <%= Html.Hidden("DaKiemTra", "0")%>
</div>
<script type="text/javascript">
    $(function() {
        $("#AddUser").click(function() {
            $("#error").text('').hide();
            $("#DaKiemTra").val("0");
            $.get('<%=HamRiengModels.SSODomain%>/user/GetUserInfo?callback=?',
				{ username: $("#sID_MaNguoiDung").val() },
				function(ssodata) {
				    if (ssodata.Username != '') {
				        $("#DaKiemTra").val("1");
				        document.getElementById("btnOK").click();
				    } else {
				        // redirect to authentication page instead of duplicating code here
				        $("#error").text('Tài khoản này chưa được đăng ký.').show();
				    }
				    // make sure to tell jQuery this is a JSONP call
				}, 'jsonp');
        });
    });
</script>

<div class="cao5px">&nbsp;</div>

<%} %>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Danh sách người dùng")%> của nhóm <%=TenNhomNguoiDung%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
	    <table class="mGrid">
        <tr>
            <th width="20px" align="center">STT</th>
            <th align="center">Tài khoản</th>
            <th width="150px" align="center">Mật khẩu</th>
            <th width="100px" align="center">Đã kích hoạt</th>
            <th width="100px" align="center">&nbsp;</th>
        </tr>
<%
    Boolean HoatDong;
    string strTG;
    for (i = 0; i < dt.Rows.Count; i++)
    {
        %>
        <tr>
            <td><%=i+1 %></td>
            <td><%=dt.Rows[i]["sID_MaNguoiDung"]%></td>
            <td>
                <%=MyHtmlHelper.ActionLink(Url.Action("PasswordReset", "NguoiDung", new { MaNguoiDung = dt.Rows[i]["sID_MaNguoiDung"] }).ToString(), "Thiết lập lại mật khẩu", "Edit", sDanhSachChucNangCam)%>
            </td>
            <td>
                <%
                    HoatDong = Convert.ToBoolean(dt.Rows[i]["bHoatDong"]);
                    if (HoatDong)
                    {
                        strTG = MyHtmlHelper.ActionLink(Url.Action("CapNhapKichHoat", "NguoiDung", new { MaNguoiDung = dt.Rows[i]["sID_MaNguoiDung"], HoatDong = false , MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Hủy kích hoạt", "Edit", sDanhSachChucNangCam);
                    }   
                    else
                    {
                        strTG = MyHtmlHelper.ActionLink(Url.Action("CapNhapKichHoat", "NguoiDung", new { MaNguoiDung = dt.Rows[i]["sID_MaNguoiDung"], HoatDong = true, MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Kích hoạt", "Edit", sDanhSachChucNangCam);
                    }   
                %>
                <%=strTG %>
            </td>           
            <td>
                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "NguoiDung", new { MaNguoiDung = dt.Rows[i]["sID_MaNguoiDung"], MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Sửa", "Edit", sDanhSachChucNangCam)%>
                <%
                    strTG = "";
                    strTG = MyHtmlHelper.ActionLink(Url.Action("Delete", "NguoiDung", new { MaNguoiDung = dt.Rows[i]["sID_MaNguoiDung"], MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Xóa", "Delete", sDanhSachChucNangCam);
                    if (strTG != "")
                    {
                        %>
                            <br />
                            <%=strTG %>
                        <%
                    }   
                %>
                
            </td>
        </tr>
        <%
    }
    dt.Dispose();
%>
        </table>
    </div>
</div>