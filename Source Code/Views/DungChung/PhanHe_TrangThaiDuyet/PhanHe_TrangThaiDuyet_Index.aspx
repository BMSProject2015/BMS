<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        int i;
        String ParentID = "PhanHe_TrangThaiDuyet";
        SqlCommand cmd;
        String MaPhanHe = Request.QueryString["MaPhanHe"];
        DataTable dtPhanHe = PhanHe_TrangThaiDuyetModel.DT_PhanHe(false, "---Chọn phân hệ---");
        if (String.IsNullOrEmpty(MaPhanHe)) MaPhanHe = Convert.ToString(dtPhanHe.Rows[0]["iID_MaPhanHe"]);
       
        DataTable dtLoaiTrangThaiDuyet = PhanHe_TrangThaiDuyetModel.DT_LoaiTrangThaiDuyet(true, "------");
        SelectOptionList slPhanHe = new SelectOptionList(dtPhanHe, "iID_MaPhanHe", "sTen");

        DataTable dtTuChoi = PhanHe_TrangThaiDuyetModel.DT_TrangThaiDuyet(true, "------", MaPhanHe);
        DataTable dtTrinhDuyet = PhanHe_TrangThaiDuyetModel.DT_TrangThaiDuyet(true, "------", MaPhanHe);
        SelectOptionList optTuChoi = new SelectOptionList(dtTuChoi, "iID_MaTrangThaiDuyet", "sTen");
        SelectOptionList optTrinhDuyet = new SelectOptionList(dtTrinhDuyet, "iID_MaTrangThaiDuyet", "sTen");
        
        DataTable dtNhomNguoiDung = PhanHe_TrangThaiDuyetModel.DT_NguoiDung(false, "--- Chọn nhóm người dùng ---");
        SelectOptionList optNhomNguoiDung = new SelectOptionList(dtNhomNguoiDung, "iID_MaNhomNguoiDung", "sTen");

        DataTable dt = LuongCongViecModel.Get_dtDSTrangThaiDuyet(Convert.ToInt32(MaPhanHe));

        String strThemMoi = Url.Action("Edit", "PhanHe_TrangThaiDuyet", new { MaPhanHe = MaPhanHe });
        String strSort = Url.Action("Sort", "PhanHe_TrangThaiDuyet",new{MaPhanHe=MaPhanHe});
        using (Html.BeginForm("Loc", "PhanHe_TrangThaiDuyet", new { ParentID = ParentID }))
        {
       
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông tin tìm kiếm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                 <tr>
                    <td class="td_form2_td1">
                        <div>
                            Phân hệ</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slPhanHe, MaPhanHe, "iID_MaPhanHe", null, "style=\"width:20%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhanHe")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" colspan="2">
                        <div style="float:right;">
                            <input id="Button2" type="submit" class="button" value="Lọc"/>
                        </div>
                    </td>
                </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <br />
    <div class="box_tong">
        <div class="title_tong">
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách phân hệ trạng thái duyệt</span>
                    </td>
                    <td align="right" style="width: 25%; padding-right: 10px;">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                                </td>
                                <td style="width: 10px;"></td>
                                <td>
                                    <input id="Button3" type="button" class="button_title" value="Sắp xếp" onclick="javascript:location.href='<%=strSort %>'" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
      <%using (Html.BeginForm("SubmitEdit", "PhanHe_TrangThaiDuyet", new { ParentID = ParentID }))
        { %>      
        <%=MyHtmlHelper.Hidden(ParentID,MaPhanHe,"iID_MaPhanHe","") %>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">STT</th>
                <th style="width: 5%;" align="center">Mã trạng thái</th>
                <th style="width: 15%;" align="center">Tên trạng thái duyệt</th>
                <th style="width: 7%;" align="center">Loại trạng thái duyệt</th>
                <th style="width: 15%;" align="center">Nhóm người dùng sửa</th>               
                <th style="width: 20%;" align="center">Trạng thái duyệt từ chối</th>                
                <th style="width: 20%;" align="center">Trạng thái duyệt trình duyệt</th>   
                <th style="width: 5%;" align="center">Sửa</th>
                <th style="width: 5%;" align="center">Xóa</th>
            </tr>
            <%  
          for (i = 0; i < dt.Rows.Count; i++)
          {
              DataRow R = dt.Rows[i];
              String TenPhanHe = Convert.ToString(CommonFunction.LayTruong("NS_PhanHe", "iID_MaPhanHe", dt.Rows[i]["iID_MaPhanHe"].ToString(), "sTen"));
              String TenNguoiDung = Convert.ToString(CommonFunction.LayTruong("QT_NhomNguoiDung", "iID_MaNhomNguoiDung", dt.Rows[i]["iID_MaNhomNguoiDung"].ToString(), "sTen"));
              String TenLoaiTrangThai = "";
              for (int j = 0; j < dtLoaiTrangThaiDuyet.Rows.Count; j++)
              {
                  if (R["iLoaiTrangThaiDuyet"].Equals(dtLoaiTrangThaiDuyet.Rows[j]["iLoaiTrangThaiDuyet"]))
                  {
                      TenLoaiTrangThai = Convert.ToString(dtLoaiTrangThaiDuyet.Rows[j]["sTen"]);
                      break;
                  }
              }
              String classtr = "";
              int STT = i + 1;
              if (i % 2 == 0)
              {
                  classtr = "class=\"alt\"";
              }
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                    <%=MyHtmlHelper.Hidden(ParentID,Convert.ToString(dt.Rows[i]["iID_MaTrangThaiDuyet"]),"iID_MaTrangThaiDuyet","") %>
                </td>
                <td align="center">
                    <%=dt.Rows[i]["iID_MaTrangThaiDuyet"]%>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["sTen"]%>
                </td>
                <td align="center">
                    <%=TenLoaiTrangThai%>
                </td>
                <td align="left">
                    <%=MyHtmlHelper.DropDownList(Convert.ToString(R["iID_MaTrangThaiDuyet"]), optNhomNguoiDung, Convert.ToString(R["iID_MaNhomNguoiDung"]), "iID_MaNhomNguoiDung", null, "style=\"width:98%;\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(Convert.ToString(R["iID_MaTrangThaiDuyet"]), optTuChoi, Convert.ToString(R["iID_MaTrangThaiDuyet_TuChoi"]), "iID_MaTrangThaiDuyet_TuChoi", null, "style=\"width: 98%;\"")%>
                </td>
                <td>
                    <%=MyHtmlHelper.DropDownList(Convert.ToString(R["iID_MaTrangThaiDuyet"]), optTrinhDuyet, Convert.ToString(R["iID_MaTrangThaiDuyet_TrinhDuyet"]), "iID_MaTrangThaiDuyet_TrinhDuyet", null, "style=\"width: 98%;\"")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "PhanHe_TrangThaiDuyet", new { MaTrangThaiDuyet = R["iID_MaTrangThaiDuyet"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "PhanHe_TrangThaiDuyet", new { MaTrangThaiDuyet = R["iID_MaTrangThaiDuyet"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                </td>   
            </tr>
            <%} %>
        </table>
        <div style="float:right; padding: 5px 5px;">
            <input type="submit" value="Lưu" class="button" />
        </div>
        
        <%}
            dt.Dispose();
            dtLoaiTrangThaiDuyet.Dispose();
            dtNhomNguoiDung.Dispose();
            dtPhanHe.Dispose();
            dtTuChoi.Dispose();
            dtTrinhDuyet.Dispose();
            %>
    </div>
</asp:Content>
