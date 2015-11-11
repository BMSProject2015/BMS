<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String MaND = User.Identity.Name;
    String ParentID = "BaoCaoNganSachNam";
    String NamLamViec = Request.QueryString["NamLamViec"];
    String sLNS = Request.QueryString["sLNS"];//Số đầu tiên của loại ngân sách=1 là ngân sách quốc phòng=2 ngân sách nhà nước    
    DateTime dNgayHienTai = DateTime.Now;
    String NamHienTai = Convert.ToString(dNgayHienTai.Year);
    int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
    int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
    DataTable dtNam = new DataTable();
    dtNam.Columns.Add("MaNam", typeof(String));
    dtNam.Columns.Add("TenNam", typeof(String));
    DataRow R;
    for (int i = NamMin; i < NamMax; i++)
    {
        R = dtNam.NewRow();
        dtNam.Rows.Add(R);
        R[0] = Convert.ToString(i);
        R[1] = Convert.ToString(i);
    }
    dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    DataTable dt = new DataTable();
    if(String.IsNullOrEmpty(NamLamViec)) NamLamViec=NamHienTai;
    
    dt = DuToan_ReportModels.DT_DuToanChiNganSachQuocPhong(NamLamViec);
    using (Html.BeginForm("EditSubmit", "rptDuToanChiNganSachQuocPhong", new { ParentID = ParentID,sAction="Index" }))
    {
     %>
 
   
   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Lọc</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div><%=NgonNgu.LayXau("Chọn năm làm việc")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 50%\"")%>
                                 <%= Html.ValidationMessage(ParentID + "_" + "err_iNamLamViec")%>
                            </div>
                        </td>
                    </tr>
                         <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
            </div>
        </div>
    </div>
    <%} %>
 <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
               <table  width="100%" cellpadding="0"  cellspacing="0" border="0">
                   <tr>
                        <td>&nbsp;</td>
                   </tr>
                  <tr>
                        <td>&nbsp;</td>
                        <td width="30%"><b>BỘ QUỐC PHÒNG</b></td>
                        <td width="70%" align="center"><b>SỐ PHÂN BỔ DỰ TOÁN NGÂN SÁCH NĂM <%=NamLamViec %></b></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td width="30%">&nbsp;</td>
                        <td width="70%" align="center"><b>(Phần ngân sách bảo đảm)</b></td>
                     </tr>
                      <tr>
                        <td>&nbsp;</td>
                   </tr>
               </table>
               <table>
                <tr>
                    <td><b>LNS:1040100 &nbsp;&nbsp;&nbsp;Loại:460&nbsp;&nbsp;&nbsp;Khoản:468</b></td>
                </tr>
               </table>
                <table width="100%" cellpadding="0"  cellspacing="0" border="1" class="mGird">                    
                    <tr>
                        <th width="20px;">Mục</th>
                        <th width="20px">Tiểu mục</th>
                        <th width="20px">Tiết mục</th>
                        <th width="10px">Ng</th>
                        <th width="100px">Nội dung</th>
                        <th>Khối</th>
                        <th width="90px">Tổng số</th>
                        <th colspan="8">
                            <table width="100%" cellpadding="0"  cellspacing="0" border="0">
                                <tr>                                    
                                    <th colspan="2" class="bd_bottom">Trong đó</th>
                                </tr>
                                <tr>
                                    <th class="bd_Right bd_Top" width="100px">Sử dụng tồn kho</th>
                                    <th>
                                        <table width="100%" cellpadding="0"  cellspacing="0" border="0">
                                            <tr>
                                                <th colspan="3" class="bd_bottom">Chi bằng tiền</th>
                                            </tr>
                                            <tr>
                                                <th width="100px" class="bd_Right">Tổng số</th>
                                                <th width="100px">Chi bằng ngoại tệ(Quy VND)</th>
                                                <th>
                                                    <table width="100%" cellpadding="0"  cellspacing="0" border="0">
                                                        <tr>
                                                            <th colspan="4" class="bd_Left bd_bottom">Chi bằng tiền trong nước</th>
                                                        </tr>
                                                        <tr>
                                                            <th width="100px" class="bd_Left">Cộng</th>
                                                            <th>
                                                                <table width="100%" cellpadding="0"  cellspacing="0" border="0">
                                                                    <tr>
                                                                        <th colspan="2" class="bd_Left bd_Right bd_bottom">Tự chi</th>
                                                                    </tr>
                                                                    <tr>
                                                                        <th width="100px" class="bd_Left bd_Right">Gián tiếp</th>
                                                                        <th width="100px" class="bd_Left bd_Right">Hàng mua</th>
                                                                    </tr>
                                                                </table>
                                                            </th>
                                                            <th width="100px" class="bd_Top bd_Right">Số đã phân cấp</th>
                                                            <th width="100px" class="bd_Top bd_Right">Số chờ phân cấp</th>
                                                        </tr>
                                                    </table>
                                                </th>
                                            </tr>
                                        </table>
                                    </th>
                                </tr>
                            </table>
                        </th>
                    </tr>
                    <%
                        Decimal T1=0, T2=0, T3=0, T4=0, T5=0, T6=0, T7=0, T8=0, T9=0;
                      for (int i = 0; i < dt.Rows.Count; i++)
                      { 
                          DataRow Row=dt.Rows[i];
                          String Tenkhoi = CommonFunction.LayTenDanhMuc(Convert.ToString(Row["iID_MaKhoiDonVi"]));
                          T1 += Convert.ToDecimal(Row["rTongSo1"]);
                          T2 += Convert.ToDecimal(Row["rTonKho"]);
                          T3 += Convert.ToDecimal(Row["rTongSo2"]);
                          T4 += Convert.ToDecimal(Row["rHangNhap"]);
                          T5 += Convert.ToDecimal(Row["rTongSo3"]);
                          T6 += Convert.ToDecimal(Row["rTuChi"]);
                          T7 += Convert.ToDecimal(Row["rHangMua"]);
                          T8 += Convert.ToDecimal(Row["rPhanCap"]);
                          T9 += Convert.ToDecimal(Row["rDuPhong"]);
                          %>

                    <tr>
                        <td><%=Row["sM"]%></td>
                        <td><%=Row["sTM"] %></td>
                        <td><%=Row["sTTM"] %></td>
                        <td><%=Row["sNG"] %></td>
                        <td><%=Row["sMoTa"] %></td>
                        <td><%=Tenkhoi %></td>
                        <td align="center"><%=CommonFunction.DinhDangSo(Row["rTongSo1"])%></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rTonKho"]) %></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rTongSo2"])%></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rHangNhap"])%></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rTongSo3"])%></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rTuChi"])%></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rHangMua"])%></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rPhanCap"])%></td>
                        <td width="100px" align="center"><%=CommonFunction.DinhDangSo(Row["rDuPhong"])%></td>                        
                    </tr>                    
                    <%} %> 
                    <tr align="center" >
                        <td colspan="4">&nbsp;</td>
                        <td colspan="2" align="center"><b>Tổng cộng:</b></td>
                        <td><%=CommonFunction.DinhDangSo(T1) %></td>
                        <td><%=CommonFunction.DinhDangSo(T2) %></td>
                        <td><%=CommonFunction.DinhDangSo(T3) %></td>
                        <td><%=CommonFunction.DinhDangSo(T4) %></td>
                        <td><%=CommonFunction.DinhDangSo(T5) %></td>
                        <td><%=CommonFunction.DinhDangSo(T6) %></td>
                        <td><%=CommonFunction.DinhDangSo(T7) %></td>
                        <td><%=CommonFunction.DinhDangSo(T8) %></td>
                        <td><%=CommonFunction.DinhDangSo(T9) %></td>
                    </tr>
                </table>
             </div>
        </div>
   </div>
</asp:Content>
