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
    if (String.IsNullOrEmpty(NamLamViec)) NamLamViec = NamHienTai;
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
    String dsKhoi="bb052603-be5d-494d-a4bf-7651894d5a0a,2ea265f1-6db9-42d8-8fb3-067356f31bc9,687860da-8dc1-4bca-b810-5cc7d6634846";
    String[] arrKhoi=dsKhoi.Split(',');
    DataTable dt = DuToan_ReportModels.DT_DuToanChiNganSachQuocPhongNamDoanhNghiep(NamLamViec);
     %>
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
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>BỘ QUỐC PHÒNG</b>
                        </td>
                        <td colspan="4" align="center">
                            <b>DỰ TOÁN CHI NGÂN SÁCH QUỐC PHÒNG NĂM <%=NamLamViec %></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td colspan="4" align="center">
                            <b>(Phần chi cho Doanh nghiệp)</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>                
               <table>
                <tr>
                    <td><b>LNS:1050000 &nbsp;&nbsp;&nbsp;Loại:460&nbsp;&nbsp;&nbsp;Khoản:468</b></td>
                </tr>
               </table>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="mGrid">
                    <tr>
                        <th width="30px;">STT</th>
                        <th>Nội dung ngân sách <br/> Đơn vị</th>
                        <th>Tổng cộng</th>
                        <th>Column 1</th>
                        <th>Column 2</th>
                        <th>Column 3</th>
                        <th>Column 4</th>
                        <th>Column 5</th>
                        <th>Column 6</th>
                        
                    </tr>
                    <%
                       
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow Row = dt.Rows[i];
                                                            
                     %>
                     <tr>
                        <td><%=i+1 %></td>
                        <td><%=Row["DonVi"] %></td>
                        <td><%=Row["TongCong"] %></td>                        
                       <%for (int k = 2; k < Row.ItemArray.Count(); k++)
{
    string columnName = "Column" + (k - 2);              
                         %>
                            <td><%=CommonFunction.DinhDangSo(Row[columnName]) %></td>    
                        <%} %>
                       
                     </tr>
                    <%} %>
                   <%-- <tr>
                        <td colspan="5">&nbsp;</td>
                        <td>Tổng cộng:</td>
                        <%for (int k = 0; k < TK.Length; k++)
                          { %>
                            <td><%=CommonFunction.DinhDangSo(TK[k]) %></td>
                        <%} %>
                        <td><%=CommonFunction.DinhDangSo(Tong) %></td>
                    </tr>--%>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
