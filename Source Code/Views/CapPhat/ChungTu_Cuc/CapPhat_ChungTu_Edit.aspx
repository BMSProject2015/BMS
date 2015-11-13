
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String DonVi = Convert.ToString(ViewData["DonVi"]);
    String iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);    
    String sLNS = Convert.ToString(ViewData["sLNS"]);
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

    DataTable dtCapPhat = CapPhat_ChungTuModels.GetCapPhat(iID_MaCapPhat);
    DataRow R;
    String sLoai="sTM",iSoCapPhat = "", sTienToChungTu="", dNgayCapPhat = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "", iDM_MaLoaiCapPhat = "";
    if (dtCapPhat!=null && dtCapPhat.Rows.Count > 0)
    {
        R = dtCapPhat.Rows[0];
        iDM_MaLoaiCapPhat = Convert.ToString(R["iDM_MaLoaiCapPhat"]);
        dNgayCapPhat = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayCapPhat"]));
        sNoiDung = Convert.ToString(R["sNoiDung"]);
        iSoCapPhat = Convert.ToString(R["iSoCapPhat"]);
        sLyDo = Convert.ToString(R["sLyDo"]);
        iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);
        sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);       
    }
    else
    {
        dNgayCapPhat = CommonFunction.LayXauNgay(DateTime.Now);
    }
    if (ViewData["DuLieuMoi"] == "1")
    {
        sTienToChungTu = PhanHeModels.LayTienToChungTu(CapPhatModels.iID_MaPhanHe);
        //iSoCapPhat = Convert.ToString(CapPhat_ChungTuModels.GetMaxChungTu() + 1);
    }
     DataTable dtLoaiCapPhat=CommonFunction.Lay_dtDanhMuc("LoaiCapPhat");         
     SelectOptionList slLoaiCapPhat= new SelectOptionList(dtLoaiCapPhat,"iID_MaDanhMuc","sTen");
     using (Html.BeginForm("EditSubmit", "CapPhat_ChungTu_Cuc", new { ParentID = ParentID, iID_MaCapPhat = iID_MaCapPhat, sLNS = sLNS, DonVi = DonVi }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
<div class="box_tong">
    <div class="title_tong" >
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
    if(dtCapPhat!=null) dtCapPhat.Dispose();
    dtLoaiCapPhat.Dispose();    
%>
</asp:Content>
