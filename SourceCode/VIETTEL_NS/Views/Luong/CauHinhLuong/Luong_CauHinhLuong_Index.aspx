<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        int i;
        String ParentID = "DuToan";        
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        String iThangHienTai = Convert.ToString(dNgayHienTai.Month);
        int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
        int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
        DataTable dtNam = new DataTable();
        dtNam.Columns.Add("MaNam", typeof(String));
        dtNam.Columns.Add("TenNam", typeof(String));
        DataRow Row;
        for (i = NamMin; i < NamMax; i++)
        {
            Row = dtNam.NewRow();
            dtNam.Rows.Add(Row);
            Row[0] = Convert.ToString(i);
            Row[1] = Convert.ToString(i);
        }
        String iNamLamViec =Convert.ToString(CauHinhLuongModels.LayNamLamViec(User.Identity.Name));
        if (iNamLamViec == "0") iNamLamViec = NamHienTai;
        String iThangLamViec = Convert.ToString(CauHinhLuongModels.LayThangLamViec(User.Identity.Name));
        if (iThangLamViec == "0") iThangLamViec = iThangHienTai;
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        
        DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "iID_MaNamNganSach", "sTen");
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        
        using (Html.BeginForm("EditSubmit", "Luong_CauHinhLuong", new { ParentID = ParentID }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID,"","LoaiNhap","") %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Cấu hình tháng làm việc</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                <tr>
                    <td width="50%">
                        <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><b>Chọn năm làm việc</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 20%\"")%>
                                         <%= Html.ValidationMessage(ParentID + "_" + "err_iNamLamViec")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div><b>Tháng làm việc</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThangLamViec, "iThangLamViec", "", "class=\"input1_2\" style=\"width:20%;\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iThangLamViec")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%">
                        <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">                   
                            <tr>
                                <td><input type="submit" class="button8" id="Submit1" onclick=ThayDoi(1) value="<%=NgonNgu.LayXau("Nhập lương")%>" /></td>
                                <td width="5px"></td>                                    
                            </tr>
                            <tr style="display:none">
                                <td><input type="submit" class="button8" id="Submit2" onclick=ThayDoi(2) value="<%=NgonNgu.LayXau("Thuế thu nhập")%>" /></td>
                                <td width="5px"></td>                                    
                            </tr>
                            <tr style="display:none">
                                <td><input type="submit" class="button8" id="Submit3" onclick=ThayDoi(3) value="<%=NgonNgu.LayXau("Khấu trừ thuế")%>" /></td>
                                <td width="5px"></td>                                    
                            </tr>
                            <tr>
                                    
                                <td><input class="button8" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" /></td>
                                <td width="5px"></td>
                            </tr>                   
                        </table>
                    </td>
                </tr>
            </table>
                
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ThayDoi(v) {
            document.getElementById('<%=ParentID%>_LoaiNhap').value = v;
        }
    </script>
    <%}%>
</asp:Content>
