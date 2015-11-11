<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers.DuToan" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "EditVayVon";
        string iID_VayChiTiet = Convert.ToString(ViewData["iID_VayChiTiet"]);
        string MaDonVi = string.Empty;
        string iID_Loai = string.Empty;
        string dNgayVay = string.Empty;
        string rLaiSuat = string.Empty;
        string rMienLai = string.Empty;
        string iID_MaChungTuVayVon = string.Empty;
        //string rDuVonCu = string.Empty;
        //string rDuLaiCu = string.Empty;
        string rVayTrongThang = string.Empty;
        string dHanPhaiTra = string.Empty;
        string rThoiGianThuVon = string.Empty;
        //string rThuVon = string.Empty;
        //string rThuLai = string.Empty;
        string sMoTa = string.Empty;
        string iID_MaNoiDung = string.Empty;
        DataTable dtVayVon = new DataTable();
        if (iID_VayChiTiet != null && iID_VayChiTiet != string.Empty )
        {
            dtVayVon = VayNoModels.LayThongTinVayVon(iID_VayChiTiet);
        }
        
        if (dtVayVon != null)
        {
            if (dtVayVon.Rows.Count != 0)
            {
                DataRow dr = dtVayVon.Rows[0];
                iID_MaChungTuVayVon = Convert.ToString(dr["iID_MaChungTuVayVon"]);
                MaDonVi = Convert.ToString(dr["iID_MaDonVi"]);
                iID_MaNoiDung = Convert.ToString(dr["iID_MaNoiDung"]);
                iID_Loai = Convert.ToString(dr["iID_Loai"]);
                if (dr["dNgayVay"] != DBNull.Value)
                {
                    DateTime dtNgayVay = Convert.ToDateTime(dr["dNgayVay"]);
                    dNgayVay = dtNgayVay.ToString("dd/MM/yyyy");
                }
                if (dr["dHanPhaiTra"] != DBNull.Value)
                {
                    DateTime dtHanPhaiTra = Convert.ToDateTime(dr["dHanPhaiTra"]);
                    dHanPhaiTra = dtHanPhaiTra.ToString("dd/MM/yyyy");
                }
                rLaiSuat = Convert.ToString(dr["rLaiSuat"]);
                rMienLai = Convert.ToString(dr["rMienLai"]);
                //rDuVonCu = Convert.ToString(dr["rDuVonCu"]);
                //rDuLaiCu = Convert.ToString(dr["rDuLaiCu"]);
                rVayTrongThang = Convert.ToString(dr["rVayTrongThang"]);
                rThoiGianThuVon = Convert.ToString(dr["rThoiGianThuVon"]);
                //rThuVon = Convert.ToString(dr["rThuVon"]);
                //rThuLai = Convert.ToString(dr["rThuLai"]);
                sMoTa = Convert.ToString(dr["sMoTa"]);
                
            }
        }
        DataTable dtDonVi = DanhMucModels.NS_DonVi();
        String sTenDonVi = Convert.ToString(dtDonVi.Rows[0]["sTen"]);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        DataTable dtNoiDung = VayNoModels.ListNoiDung();
        SelectOptionList slNoiDung = new SelectOptionList(dtNoiDung, "iID_MaNoiDung", "sMaTen");
       
    %>
    <% using (Html.BeginForm("EditSubmit", "VayVon", new { ParentID = ParentID }))
       {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_VayChiTiet", iID_VayChiTiet)%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>
                            <%
                                if (ViewData["DuLieuMoi"] == "1")
                                {
                            %>
                            <%=NgonNgu.LayXau("Thêm mới thông tin vay vốn")%>
                            <%
                                }
                                else
                                {
                            %>
                            <%=NgonNgu.LayXau("Sửa thông tin vay vốn")%>
                            <%
                                }
                            %>&nbsp; &nbsp; </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <div style="width: 50%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                     <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Số chứng từ</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                     <%=MyHtmlHelper.TextBox(ParentID, iID_MaChungTuVayVon, "iID_MaChungTuVayVon", "", "class=\"input1_2\"", 1)%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaChungTuVayVon")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Đơn vị</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" onchange=\"Chon_DonVi(this.value)\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr id="<%= ParentID %>_divPhongQuanLY">
                            <%=VayVonController.GetBQuanLy(ParentID, MaDonVi)%>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Nội dung vay
                                </div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slNoiDung, iID_MaNoiDung, "iID_MaNoiDung", "", "class=\"input1_2\" onchange=\"Chon_NoiDung(this.value)\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Loại
                                </div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iID_Loai, "iID_Loai", "", "class=\"input1_2\"" , 1)%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_Loai")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Ngày vay
                                </div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayVay, "dNgayVay", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayVay")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Lãi suất
                                </div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, rLaiSuat, "rLaiSuat", "", "class=\"input1_2\"",1)%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_rLaiSuat")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Miễn lãi
                                </div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, rMienLai, "rMienLai", "", "class=\"input1_2\"",1)%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_rMienLai")%>
                                </div>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Số tiền vay</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, rVayTrongThang, "rVayTrongThang", "", "class=\"input1_2\"",1)%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_rVayTrongThang")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Hạn phải trả</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dHanPhaiTra, "dHanPhaiTra", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dHanPhaiTra")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Thời gian thu vốn</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, rThoiGianThuVon, "rThoiGianThuVon", "", "class=\"input1_2\"",1)%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_rThoiGianThuVon")%>
                                </div>
                            </td>
                        </tr>
                      
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Ghi chú</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "class=\"input1_2\" style=\"height: 40px;\"")%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sMoTa")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td width="65%" class="td_form2_td5">
                                                &nbsp;
                                            </td>
                                            <td width="30%" align="right" class="td_form2_td5">
                                                <input type="submit" class="button" id="Submit1" value="Lưu" />
                                            </td>
                                            <td width="5px">
                                                &nbsp;
                                            </td>
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
    <%} %>
    <script type="text/javascript">
        CheckThemMoi(false);
        function CheckThemMoi(value) {
            if (value == true) {
                document.getElementById('tb_DotNganSach').style.display = ''
            } else {
                document.getElementById('tb_DotNganSach').style.display = 'none'
            }
        }

        function Chon_DonVi(maDonVi) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("get_objPhongQuanLy?ParentID=#0&MaDonVi=#1", "VayVon")%>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", maDonVi));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divPhongQuanLY").innerHTML = data;
            });
        }        
        
    </script>  
</asp:Content>
