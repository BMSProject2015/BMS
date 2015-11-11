<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String ParentID = "Loai";
        String MaND = User.Identity.Name;
        String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
        String iID_Ma = Request.QueryString["iID_Ma"];
        String iID_MaGiaiThich = Request.QueryString["iID_MaGiaiThich"];
        String page = Request.QueryString["page"];
        String sGiaiThich = "";
        if (String.IsNullOrEmpty(iID_MaGiaiThich) == false && iID_MaGiaiThich != "")
        {
            var tblChitiet = KeToanTongHop_GiaiThichTaiKhoanModels.getDetail(iID_MaGiaiThich);

            if (tblChitiet.Rows.Count > 0)
            {
                DataRow DR = tblChitiet.Rows[0];
                iID_MaTaiKhoan = HamChung.ConvertToString(DR["iID_MaTaiKhoan"]);
                sGiaiThich = HamChung.ConvertToString(DR["sGiaiThich"]);
            }
            if (tblChitiet != null) tblChitiet.Dispose();
        }
        if (String.IsNullOrEmpty(iID_Ma) || iID_Ma == "")
        {
            iID_Ma = "-1";
        }
        //đoạn lệnh nhảy đến phần thêm mới
        String strThemMoi = Url.Action("Edit", "LoaiThongTri");
        //đổ dữ liệu vào Combobox tài khoản
        var tbl = TaiKhoanModels.DT_DSTaiKhoanCha(true, "--Chọn tất cả--", User.Identity.Name, false);
        SelectOptionList slTaiKhoan = new SelectOptionList(tbl, "iID_MaTaiKhoan", "sTen");
        if (tbl != null) tbl.Dispose();

         tbl = TaiKhoanModels.DT_DSTaiKhoan_Exits_KT_TaiKhoanGiaiThich(true, "--Chọn tất cả--", User.Identity.Name, false);
         SelectOptionList slTaiKhoan1 = new SelectOptionList(tbl, "iID_MaTaiKhoan", "sTen");
        if (tbl != null) tbl.Dispose();
       
        //
        String TenTK = "";
        if (String.IsNullOrEmpty(iID_MaTaiKhoan) == false && iID_MaTaiKhoan != "")
            TenTK = TaiKhoanModels.getTenTK(iID_MaTaiKhoan);
        //Danh sách tài khoản
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = TaiKhoanDanhMucChiTietModels.Get_DanhSachTaiKhoanDanhMucChiTiet(); //KeToanTongHop_GiaiThichTaiKhoanModels.getList(iID_MaTaiKhoan, CurrentPage, Globals.PageSize);
        DataTable dtGiaiThich = KeToanTongHop_GiaiThichTaiKhoanModels.Get_DSGiaiThich(iID_MaTaiKhoan);
        String UrlGiaiThich = Url.Action("Index", "KeToanTongHop_GiaiThichTaiKhoan");
        using (Html.BeginForm("EditSubmit", "KeToanTongHop_GiaiThichTaiKhoan", new { ParentID = ParentID }))
        {     
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaGiaiThich", iID_MaGiaiThich)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>&nbsp;| &nbsp;
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "LoaiTaiKhoan"), "Danh mục tài khoản")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>giải thích
                            <% if (String.IsNullOrEmpty(TenTK) == false && TenTK != "")
                               { %>
                            (Tài khoản
                            <%=TenTK%>)
                            <%}%>
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="100%" border="0">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <b>Tài khoản</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 70%;">
                            <div style="float: left;">
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", " onchange=\"ChonTaiKhoan(this.value)\" class=\"input1_2\" tab-index='-1' style=\"width:400px;\"")%>
                                <%--  </div>
                             <div>--%>
                                <b>Sao chép giải thích</b>
                                <%=MyHtmlHelper.CheckBox(ParentID, "0", "iDisplay", "", "onclick=\"CheckDisplay(this.checked)\"")%>
                            </div>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" align="right">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Lưu" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                                    </td>
                                    <td width="10px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tb_CanBo1">
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <b>Sao chép giải thích từ tài khoản</b>
                            </div>
                        </td>
                        <td class="td_form2_td5" style="width: 400px;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan1, "", "iID_MaTaiKhoan_Copy", "", "class=\"input1_2\" style=\"width:400px;\"")%>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" class="td_form2_td1" style="height: 10px;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tb_CanBo0">
                        <td class="td_form2_td1" valign="top">
                            <div>
                                <b>Chọn nội dung giải thích cho tài khoản</b></div>
                        </td>
                        <td class="td_form2_td5" colspan="2">
                            <%--       <div>
                                <table class="mGrid">
                                    <tr>
                                        <th style="width: 5%;" align="center">
                                            Chọn
                                        </th>
                                        <th style="width: 5%;" align="center">
                                            STT
                                        </th>
                                        <th style="width: 5%;" align="center">
                                            Ký hiệu
                                        </th>
                                        <th align="center">
                                            Nội dung giải thích
                                        </th>
                                    </tr>
                                    <%
            for (i = 0; i < dt.Rows.Count; i++)
            {
                DataRow R = dt.Rows[i];
                String iID_MaTaiKhoanDanhMucChiTiet = Convert.ToString(R["iID_MaTaiKhoanDanhMucChiTiet"]);
                String sKyHieu = Convert.ToString(R["sKyHieu"]);
                String check = "";
                for (int j = 0; j < dtGiaiThich.Rows.Count; j++)
                {
                    check = "";
                    String sKyHieu1 = Convert.ToString(dtGiaiThich.Rows[j]["sKyHieu"]);
                    if (sKyHieu.Equals(sKyHieu1))
                    {
                        check = "checked=\"checked\"";
                        break;
                    }
                }
                String classtr = "";
                int STT = i + 1;                   
               
                       
                                    %>
                                    <tr <%=classtr %>>
                                        <td align="center">
                                            <input type="checkbox" id="<%=ParentID %>_iID_MaTaiKhoanDanhMucChiTiet" name="<%=ParentID %>_iID_MaTaiKhoanDanhMucChiTiet"
                                                value="<%=iID_MaTaiKhoanDanhMucChiTiet %>" <%=check %> />
                                            <%=MyHtmlHelper.Hidden("sKyHieu", R["sKyHieu"], iID_MaTaiKhoanDanhMucChiTiet, "")%>
                                            <%=MyHtmlHelper.Hidden("sTen",R["sTen"],iID_MaTaiKhoanDanhMucChiTiet,"") %>
                                        </td>
                                        <td align="center">
                                            <%=STT%>
                                        </td>
                                        <td align="left">
                                            <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sKyHieu"]))%>
                                        </td>
                                        <td align="left">
                                            <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sTen"]))%>
                                        </td>
                                    </tr>
                                    <%} %>
                                </table>
                            </div>--%>
                            <div>
                                <table class="mGrid">
                                    <tr>
                                        <th style="width: 5%;" align="center">
                                            <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                        </th>
                                        <th style="width: 5%;" align="center">
                                            STT
                                        </th>
                                        <th style="width: 20%;" align="center">
                                            Ký hiệu
                                        </th>
                                        <th style="width: 40%;" align="center">
                                            Nội dung giải thích
                                        </th>
                                        <th style="width: 15%;" align="center">
                                            Tài khoản giải thích
                                        </th>
                                    </tr>
                                    <%
       
                               int ThuTu = 0;
                               String XauHanhDong = "";
                               String XauSapXep = "";
         
                                    %>
                                    <%--<div id="td_TaiKhoan" style="max-height: 600px; height: 600px; min-height: 600px;
                                        overflow: scroll; margin-bottom: 2px;">--%>
                                    <%=TaiKhoanDanhMucChiTietModels.LayXauTaiKhoanDanhMucChiTiet_TaiKhoan(Url.Action("", ""), "", "", "", 0, ref ThuTu, dtGiaiThich, Convert.ToInt32(iID_Ma))%>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right" class="td_form2_td1" style="height: 10px;">
                            <table cellpadding="0" cellspacing="0" border="0" align="right">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Lưu" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                                    </td>
                                    <td width="10px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <script type="text/javascript">
                    function ChonTaiKhoan(value) {
                        var url = '<%=UrlGiaiThich %>';
                        url = url + "?iID_MaTaiKhoan=" + value;
                        location.href = url;
                    }
                    function ChonDieuKien(value) {
                        var url = '<%=UrlGiaiThich %>';
                        var MaTaiKhoan = '<%=iID_MaTaiKhoan %>';
                        url = url + "?iID_MaTaiKhoan=" + MaTaiKhoan + "&iID_Ma=" + value;
                        location.href = url;
                    }
                    function CheckAll(value) {
                        $("input:checkbox[check-group='iID_MaTaiKhoan']").each(function (i) {
                            this.checked = value;
                        });
                    }

                    CheckDisplay('False');
                    function CheckDisplay(value) {

                        if (value == true || value == 'True' || value == '1') {
                            document.getElementById('tb_CanBo0').style.display = 'none';
                            document.getElementById('tb_CanBo1').style.display = '';
                        } else {

                            document.getElementById('tb_CanBo0').style.display = '';
                            document.getElementById('tb_CanBo1').style.display = 'none';
                        }

                    }
                </script>
            </div>
        </div>
    </div>
    <%
        } 
    %>
</asp:Content>
