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
    String MaND = User.Identity.Name;
    DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeDuToan);
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = "0";
    }
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    String dsKhoi="bb052603-be5d-494d-a4bf-7651894d5a0a,2ea265f1-6db9-42d8-8fb3-067356f31bc9,687860da-8dc1-4bca-b810-5cc7d6634846";
    String[] arrKhoi=dsKhoi.Split(',');
    DataTable dt = DuToan_ReportModels.DT_DuToanChiNganSachSuDung(dsKhoi,NamLamViec,MaND);
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
                            <b>SỐ PHÂN BỔ DỰ TOÁN NGÂN SÁCH NĂM
                                <%=NamLamViec %>
                                CÁC ĐƠN VỊ</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td colspan="4" align="center">
                            <b>(Phần thường xuyên)</b>
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
                    <td><b>LNS:1040100 &nbsp;&nbsp;&nbsp;Loại:460&nbsp;&nbsp;&nbsp;Khoản:468</b></td>
                </tr>
               </table>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="mGrid">
                    <tr>
                        <th width="30px;">STT</th>
                        <th>Mục</th>
                        <th>Tiểu mục</th>
                        <th>Tiết mục</th>
                        <th>Ng</th>
                        <th>Nội dung</th>
                        <th>Khối doanh nghiệp</th>
                        <th>Khối quân khu,Quân đoàn H.viên, nhà trường</th>
                        <th>Khối BTTM và các tổng cục</th>
                        <th>Tổng cộng</th>
                    </tr>
                    <%
                        Decimal[] TK = new Decimal[arrKhoi.Length];
                        Decimal Tong = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                          DataRow Row = dt.Rows[i];
                          if (Row["rT"]!= DBNull.Value)
                          {
                              Tong += Convert.ToDecimal(Row["rT"]);
                          }  
                        for(int j=0;j<arrKhoi.Length;j++)
                        {
                            if (Row["rK" + j] != DBNull.Value)
                             {
                                 TK[j] =Convert.ToDecimal(TK[j]) + Convert.ToDecimal(Row["rK" + j]);
                             }  
                        }                                               
                     %>
                     <tr>
                        <td><%=i+1 %></td>
                        <td><%=Row["sM"] %></td>
                        <td><%=Row["sTM"] %></td>
                        <td><%=Row["sTTM"] %></td>
                        <td><%=Row["sNG"] %></td>
                        <td><%=Row["sMoTa"] %></td>
                       <%for (int k = 0; k < arrKhoi.Length; k++)
                         {                   
                         %>
                            <td><%=CommonFunction.DinhDangSo(Row["rK"+k]) %></td>    
                        <%} %>
                        <td><%=CommonFunction.DinhDangSo(Row["rT"]) %></td>    
                     </tr>
                    <%} %>
                    <tr>
                        <td colspan="5">&nbsp;</td>
                        <td>Tổng cộng:</td>
                        <%for (int k = 0; k < TK.Length; k++)
                          { %>
                            <td><%=CommonFunction.DinhDangSo(TK[k]) %></td>
                        <%} %>
                        <td><%=CommonFunction.DinhDangSo(Tong) %></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
