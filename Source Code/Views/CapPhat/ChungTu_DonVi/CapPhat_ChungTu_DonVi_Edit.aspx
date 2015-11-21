<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.CapPhat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);    
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    dtCauHinh.Dispose();
    
    String Loai = Convert.ToString(Request.QueryString["Loai"]);

    if(String.IsNullOrEmpty(Loai)){
        Loai = Convert.ToString(ViewData["Loai"]);
    }
    
     //Danh sách đơn vị
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();

    //Danh sách loại cấp phát 
    DataTable dtLoaiCapPhat = CommonFunction.Lay_dtDanhMuc("LoaiCapPhat");
    SelectOptionList slLoaiCapPhat = new SelectOptionList(dtLoaiCapPhat, "iID_MaDanhMuc", "sTen");
    dtLoaiCapPhat.Dispose();
     
    //Danh sách tính chất cấp thu
    DataTable dtTinhChatCapThu = TinhChatCapThuModels.Get_dtTinhChatCapThu();
    SelectOptionList slTinhChatCapThu = new SelectOptionList(dtTinhChatCapThu, "iID_MaTinhChatCapThu", "sTen");
    DataRow R2 = dtTinhChatCapThu.NewRow();
    R2["iID_MaTinhChatCapThu"] = "-1";
    R2["sTen"] = "--- Danh sách tính chất cấp thu ---";
    dtTinhChatCapThu.Rows.InsertAt(R2, 0);
    dtTinhChatCapThu.Dispose();

    //Danh sách loại ngân sách
    DataTable dtLNS = DanhMucModels.NS_LoaiNganSachQuocPhong(MaPhongBanNguoiDung);
    SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
    dtLNS.Dispose();

    //Danh sách chi tiết đến
    DataTable dtLoai = CapPhat_ChungTuModels.LayLoaiNganSachCon();
    SelectOptionList slLoai = new SelectOptionList(dtLoai, "iID_Loai", "TenHT");
    dtLoai.Dispose();
     
    //Khai báo biến
    String iID_MaTinhChatCapThu = "";
    String iID_MaDonVi = "";
    String iSoCapPhat = "";
    String sTienToChungTu = "";
    String dNgayCapPhat = "";
    String sNoiDung = "";
    String sLNS = "";
    String sLoai = "";
    String iDM_MaLoaiCapPhat = "";
    String ReadOnly = "";
    //Nếu là thêm mới chứng từ
     if (ViewData["DuLieuMoi"] == "1")
     {
         sLoai = "sNG";
         sTienToChungTu = PhanHeModels.LayTienToChungTu(CapPhatModels.iID_MaPhanHe);
         iSoCapPhat = Convert.ToString(CapPhat_ChungTuModels.GetMaxSoCapPhat() + 1);
         dNgayCapPhat = CommonFunction.LayXauNgay(DateTime.Now);
     }
     //Nếu là update chứng từ
     else {
         //Lấy thông tin chứng từ cấp phát. 
         DataTable dtCapPhat = CapPhat_ChungTuModels.LayChungTuCapPhat(iID_MaCapPhat);
         if (dtCapPhat != null && dtCapPhat.Rows.Count > 0)
         {
             DataRow R = dtCapPhat.Rows[0];

             iSoCapPhat = Convert.ToString(R["iSoCapPhat"]);
             sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);
             iDM_MaLoaiCapPhat = Convert.ToString(R["iDM_MaLoaiCapPhat"]);
             iID_MaTinhChatCapThu = Convert.ToString(R["iID_MaTinhChatCapThu"]);
             iID_MaDonVi = Convert.ToString(R["iID_MaDonVi"]);
             sLNS = Convert.ToString(R["sDSLNS"]);
             sLoai = Convert.ToString(R["sLoai"]);
             dNgayCapPhat = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayCapPhat"]));
             sNoiDung = Convert.ToString(R["sNoiDung"]);
             ReadOnly = "disabled=\"disabled\"";
             
             dtCapPhat.Dispose();
         }
          
     }

     using (Html.BeginForm("LuuChungTu", "CapPhat_ChungTu_DonVi", new { ParentID = ParentID, iID_MaCapPhat = iID_MaCapPhat, Loai = Loai }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới chứng từ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa thông tin chứng từ")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 50%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>Số chứng từ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=sTienToChungTu %><%=iSoCapPhat%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Loại cấp phát</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slLoaiCapPhat, iDM_MaLoaiCapPhat, "iDM_MaLoaiCapPhat", "")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iDM_MaLoaiCapPhat")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Tính chất cấp thu</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTinhChatCapThu, iID_MaTinhChatCapThu, "iID_MaTinhChatCapThu", "")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTinhChatCapThu")%>
                            </div>
                        </td>
                    </tr>
                 
                    <tr>
                        <td class="td_form2_td1">
                            <div>Đơn vị</div>
                        </td>
                        <td class="td_form2_td5">
                            <div id="<%= ParentID %>_divDonVi">
                              
                                <%= CapPhat_ChungTu_DonViController.LayDoiTuongDonVi_PhongBan(ParentID, UserID, iID_MaDonVi, iNamLamViec)%>
                            </div>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                        </td>
                    </tr>
                    <!--VungNV: 2015/10/20 Nếu là loại NSQP thì có thêm dropdown list Chọn LNS:-->
                    <%if (Loai == "1")
                      { %>
                     <tr>
                            <td class="td_form2_td1">
                                <div>Loại ngân sách</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%= MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "class=\"input1_2\" style=\"height: 100px;\"", ReadOnly)%><br />
                                </div>
                            </td>
                      </tr>
                      <% } %>

                     <tr>
                        <td class="td_form2_td1">
                            <div>Chi tiết đến:</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slLoai, sLoai, "iID_Loai", "class=\"input1_2\" style=\"height: 100px;\"", ReadOnly)%></div>
                        </td>
                    </tr>

                    <tr>
                        <td class="td_form2_td1">
                            <div>Ngày chứng từ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayCapPhat, "dNgayCapPhat", "", "class=\"input1_2\" onblur=isDate(this);")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayCapPhat")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Nội dung</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="Lưu" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%
    }
%>
</asp:Content>

