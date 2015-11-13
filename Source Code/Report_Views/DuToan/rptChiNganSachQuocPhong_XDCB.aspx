<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
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
    dt = DuToan_ReportModels.DT_DuToanChiNganSachQuocPhong_XDCB(NamLamViec);
    using (Html.BeginForm("EditSubmit", "rptDuToanChiNganSachQuocPhong", new { ParentID = ParentID,sAction="XDCB" }))
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
                        <td width="70%" align="center"><b>(Phần ngân sách Xây dựng cơ bản)</b></td>
                     </tr>
                      <tr>
                        <td>&nbsp;</td>
                   </tr>
               </table>
               <table>
                <tr>
                    <td><b>LNS:1030100 &nbsp;&nbsp;&nbsp;Loại:460&nbsp;&nbsp;&nbsp;Khoản:468</b></td>
                </tr>
               </table>
                <table width="100%" cellpadding="0"  cellspacing="0" border="1" class="mGird">
                    
                    <tr>
                        <th width="5%"><b>Mục</b></th>
                        <th width="5%"><b>Tiểu mục</b></th>
                        <th width="5%"><b>Tiết mục</b></th>
                        <th width="5%"><b>Ng</b></th>
                        <th><b>Nội dung</b></th>
                        <th><b>Khối</b></th>
                        <th width="10%"><b>Tổng số</b></th>
                        <th width="40%" colspan="2">
                            <table width="100%" cellpadding="0"  cellspacing="0" border="0">
                                <tr>                                    
                                    <th colspan="2" class="bd_bottom">Trong đó</th>
                                </tr>
                                <tr>
                                    <th class="bd_Right">Đã phân cấp</th>
                                    <th>Chờ phân cấp</th>
                                </tr>
                            </table>
                        </th>
                    </tr>
                    <%
                        Decimal Tong = 0,T1=0,T2=0,T3=0;
                        for (int i = 0; i < dt.Rows.Count;i++)
                      {
                          DataRow Row = dt.Rows[i];
                          String Tenkhoi = CommonFunction.LayTenDanhMuc(Convert.ToString(Row["iID_MaKhoiDonVi"]));
                          if (Row["rPhanCap"] != DBNull.Value)
                          {
                              Tong += Convert.ToDecimal(Row["rPhanCap"]);
                              T2 += Convert.ToDecimal(Row["rPhanCap"]);
                          }
                          if (Row["rDuPhong"] != DBNull.Value)
                          {
                              Tong += Convert.ToDecimal(Row["rDuPhong"]);
                              T3 += Convert.ToDecimal(Row["rDuPhong"]);
                          }
                          T1 += Tong;
                           %>
                    <tr>
                        <td><%=Row["sM"] %></td>
                        <td><%=Row["sTM"] %></td>
                        <td><%=Row["sTTM"] %></td>
                        <td><%=Row["sNG"] %></td>
                        <td><%=Row["sMoTa"] %></td>
                        <td><%=Tenkhoi %></td>
                        <td><%=CommonFunction.DinhDangSo(Tong) %></td>
                        <td><%=CommonFunction.DinhDangSo(Row["rPhanCap"])%></td>
                        <td><%=CommonFunction.DinhDangSo(Row["rDuPhong"])%></td>
                    </tr>
                    <%} %>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>Tổng cộng:</td>
                        <td><%=CommonFunction.DinhDangSo(T1) %></td>
                        <td><%=CommonFunction.DinhDangSo(T2) %></td>
                        <td><%=CommonFunction.DinhDangSo(T3) %></td>
                    </tr>
                </table>
             </div>
        </div>
   </div>
</asp:Content>
