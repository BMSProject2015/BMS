<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<script src="<%= Url.Content("~/Scripts/jsDuToan.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>"
        type="text/javascript"></script>
<script>
    var browserID = 'msie'; //ie = 'mozilla'
   
    function DetectBrowser() {
        jQuery.each(jQuery.browser, function (i, val) {
            if (val == true) {
                browserID = i.toString();
            }
        });
    }
</script>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    //String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String MaND = Convert.ToString(ViewData["MaND"]);
   // String ParentID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID="ChungTuChiTiet_Index_LichSuVaNhap";
    String iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
    NameValueCollection data = DuToan_ChungTuModels.LayThongTin(iID_MaChungTu);
        
    int iNamLamViec = Convert.ToInt32(data["iNamLamViec"]);
    String DSTruongTienTieuDe=DuToanModels.strDSTruongTienTieuDe;
    String DSTruongTien = DuToanModels.strDSTruongTien;
    String[] arrDSTruongTienTieuDe = DSTruongTienTieuDe.Split(',');
    String[] arrDSTruongTien = DSTruongTien.Split(',');

    String DSTruongTieuDe = "Đơn vị," + MucLucNganSachModels.strDSTruongTieuDe;
    String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
    String DSTruongDoRong = "100," + MucLucNganSachModels.strDSTruongDoRong;
    String[] arrDSTruongTieuDe = DSTruongTieuDe.Split(',');
    String[] arrDSTruong = DSTruong.Split(',');
    String[] arrDSTruongDoRong = DSTruongDoRong.Split(',');
        
    List<Boolean> lstHienThiTruongTien = new List<Boolean>();
    List<Boolean> lstDuocNhapTruongTien = new List<Boolean>();
    String[] arrDSDuocNhapTruongTien =  DuToanModels.strDSDuocNhapTruongTien.Split(',');
    for (int i = 0; i < arrDSDuocNhapTruongTien.Length ; i++)
    {
        lstHienThiTruongTien.Add(false);
        lstDuocNhapTruongTien.Add(false);
    }
        
    Dictionary<String, String> arrGT = new Dictionary<String, String>();
    for(int i =0;i<arrDSTruong.Length;i++)
    {
        arrGT.Add(arrDSTruong[i],"");
    }
    arrGT.Add("sTenDonVi", "");
    
    DataTable dtChungTuChiTiet = DuToan_ChungTuChiTietModels.GetChungTuChiTiet_LichSu(iID_MaChungTu);
    //<--Cập nhập thông tin hiển thị của các cột tiền
    for (int i = 0; i < dtChungTuChiTiet.Rows.Count; i++)
    {
        DataRow R = dtChungTuChiTiet.Rows[i];
        for (int j = 0; j < arrDSDuocNhapTruongTien.Length; j++)
        {
         
            string sTenDSDuocNhapTruongTien=   arrDSDuocNhapTruongTien[j];
            if (sTenDSDuocNhapTruongTien.IndexOf("_DonVi") >= 0) sTenDSDuocNhapTruongTien = sTenDSDuocNhapTruongTien.Substring(0, sTenDSDuocNhapTruongTien.IndexOf("_DonVi"));
            lstHienThiTruongTien[j] = lstHienThiTruongTien[j] || Convert.ToBoolean(R[sTenDSDuocNhapTruongTien]);
        }
    }
    //-->Cập nhập thông tin hiển thị của các cột tiền

    String iID_MaMucLucNganSach = "", sXauNoiMa = "";
    //<--Cập nhập thông tin của hàng mới thêm vào nhất
    if (dtChungTuChiTiet.Rows.Count>0)
    {
        //Lấy hàng 
        DataRow R = dtChungTuChiTiet.Rows[0];
        for (int i = 0; i < arrDSTruong.Length; i++)
        {
            arrGT[arrDSTruong[i]] = Convert.ToString(R[arrDSTruong[i]]);
        }
        arrGT["sTenDonVi"] = Convert.ToString(R["sTenDonVi"]);
        iID_MaMucLucNganSach = Convert.ToString(R["iID_MaMucLucNganSach"]);
        sXauNoiMa = Convert.ToString(R["sXauNoiMa"]);
        //<--Cập nhập thông tin được nhập của các cột tiền
        for (int j = 0; j < arrDSDuocNhapTruongTien.Length; j++)
        {
            string sTenDSDuocNhapTruongTien = arrDSDuocNhapTruongTien[j];
            if (sTenDSDuocNhapTruongTien.IndexOf("_DonVi") >= 0) sTenDSDuocNhapTruongTien = sTenDSDuocNhapTruongTien.Substring(0, sTenDSDuocNhapTruongTien.IndexOf("_DonVi"));
            lstDuocNhapTruongTien[j] = Convert.ToBoolean(R[arrDSDuocNhapTruongTien[j]]);
        }
        //-->Cập nhập thông tin được nhập của các cột tiền
    }
    //-->Cập nhập thông tin của hàng mới thêm vào nhất

    String DSHienThiTruongTien = "", DSDuocNhapTruongTien = "";
    for (int i = 0; i < lstHienThiTruongTien.Count; i++)
    {
        if (DSHienThiTruongTien != "") DSHienThiTruongTien += ",";
        DSHienThiTruongTien += lstHienThiTruongTien[i] ? "1" : "0";
        
        if (DSDuocNhapTruongTien != "") DSDuocNhapTruongTien += ",";
        DSDuocNhapTruongTien += lstDuocNhapTruongTien[i] ? "1" : "0";
    }
    int dTongSoCot = arrDSTruongTieuDe.Length + arrDSTruongTieuDe.Length + 3;
    int WidthCotTien = 120;
    int DoRongBangFix = 0;
    for (int j = 0; j < arrDSTruongDoRong.Length; j++)
    {
        DoRongBangFix += Convert.ToInt32(arrDSTruongDoRong[j]);
    }
    DoRongBangFix += 100 * 2;
    String strAttr;
    int dCot = 0;
    %>


<table cellspacing="0" cellpadding="0" border="0" style="width: 99%; background-color: #fff; margin: 0px;">
    <tr>
        <td valign="top" style="width:<%=DoRongBangFix+4%>px; border: solid 1px #525252;">
            <%--Cột fix--%>
            <div id="<%=ParentID%>_div1" style="width:<%=DoRongBangFix+4%>px;">
                <table class="gridBang">
                    <%--Hàng tiêu đề 1--%>
                    <tr>
                        <%
                        for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
                        {
                        %>
                            <th rowspan="2" style="width:<%=arrDSTruongDoRong[j]%>px;min-width:<%=arrDSTruongDoRong[j]%>px;max-width:<%=arrDSTruongDoRong[j]%>px;">
                                <span style="width:<%=arrDSTruongDoRong[j]%>px;min-width:<%=arrDSTruongDoRong[j]%>px;max-width:<%=arrDSTruongDoRong[j]%>px;display: inline-block;overflow: hidden;white-space:nowrap;">
                                    <%=arrDSTruongTieuDe[j]%>
                                </span>
                            </th>
                        <%
                        }
                        %>
                        <th colspan="2" style="border-bottom: solid 1px #999;">Năm <%=iNamLamViec-1%></th>
                    </tr>
                    <%--Hàng tiêu đề 2--%>
                    <tr id="<%=ParentID%>_OFixBang1">
                        <th style='width:73spx'><span style="width:70px;min-width:70px;max-width:70px;display: inline-block;overflow: hidden;white-space:nowrap;">Dự toán</span></th>
                        <th style='width:73px'><span style="width:70px;min-width:70px;max-width:70px;display: inline-block;overflow: hidden;white-space:nowrap;">Ước thực hiện</span></th>
                    </tr>
                    <%--Hàng lịch sử--%>
                    <%
                    for (int i = 9;i >= dtChungTuChiTiet.Rows.Count ; i--)
                    {
                        String classtr = "";
                        if (i % 2 == 0)
                        {
                            classtr = "class=\"alt\"";
                        }
                        %>
                        <tr <%=classtr %> id="idHangTieuDe<%=i%>" style="display:none;">
                            <%
                            for (int j = 0; j < arrDSTruongTieuDe.Length-1; j++)
                            {
                                if (arrDSTruong[j] == "iID_MaDonVi")
                                {
                                    %>
                                    <td <%=(j==0)?"class='"+ParentID+"_FirstCol'":""%> style="text-align:left;"><span style="width:<%=arrDSTruongDoRong[j]%>px;min-width:<%=arrDSTruongDoRong[j]%>px;max-width:<%=arrDSTruongDoRong[j]%>px;display: inline-block;overflow: hidden;white-space:nowrap;"></span></td>
                                    <%
                                }
                                else
                                {
                                    %>
                                    <td <%=(j==0)?"class='"+ParentID+"_FirstCol'":""%>></td>
                                    <%
                                }
                            }
                            %>
                            <td style="text-align:left;"></td>
                            <td></td>
                            <td></td>
                        </tr>
                    <%}
                    for (int i = dtChungTuChiTiet.Rows.Count - 1; 0 <= i; i--)
                    {
                        DataRow R = dtChungTuChiTiet.Rows[i];
                        String classtr = "";
                        if (i % 2 == 0)
                        {
                            classtr = "class=\"alt\"";
                        }
                        %>
                        <tr <%=classtr %> id="idHangTieuDe<%=i%>">
                            <%
                            for (int j = 0; j < arrDSTruongTieuDe.Length-1; j++)
                            {
                                if (arrDSTruong[j] == "iID_MaDonVi")
                                {
                                    %>
                                    <td <%=(j==0)?"class='"+ParentID+"_FirstCol'":""%> style="text-align:left;">
                                        <span style="display: inline-block;overflow: hidden;white-space:nowrap;width:<%=arrDSTruongDoRong[j]%>px;min-width:<%=arrDSTruongDoRong[j]%>px;max-width:<%=arrDSTruongDoRong[j]%>px;"><%=R["sTenDonVi"]%></span>
                                    </td>
                                    <%
                                }
                                else
                                {
                                    %>
                                    <td <%=(j==0)?"class='"+ParentID+"_FirstCol'":""%>><%=R[arrDSTruong[j]]%></td>
                                    <%
                                }
                            }
                            %>
                            <td style="text-align:left;"><span style="display: inline-block;overflow: hidden;white-space:nowrap;"><%=R["sMoTa"]%></span></td>
                            <td><%=DuToan_ChungTuChiTietModels.DinhDangTruongTien(R["rTongSoNamTruoc"])%></td>
                            <td><%=DuToan_ChungTuChiTietModels.DinhDangTruongTien(R["rUocThucHien"])%></td>
                        </tr>
                    <%} %>
                    <%--Hàng nhập dữ liệu--%>
                    <tr>
                        <%
                        for (int i = 0; i < arrDSTruong.Length - 1; i++)
                        {
                            String strTruong = arrDSTruong[i];
                            String strTruongText = strTruong + "1";
                            String GiaTri = arrGT[arrDSTruong[i]];
                            String GiaTriText = arrGT[arrDSTruong[i]];
                            
                            String strTerm = "DSGiaTri:funcGhepMa, Truong:'iID_MaMucLucNganSach', GiaTri:request.term";
                            if (strTruong == "iID_MaDonVi")
                            {
                                strAttr = String.Format("class='input1_4' chi-tiet-du-toan='1' tab-index='-1' style='text-align:left; width:{1}px; height:25px; padding-right:2px;' ten-truong='{0}' onfocus=\"LSN_MaCotNganSach='{0}';\" cot='{2}' onblur=\"func_Auto_Complete_onblur(this.id,'{0}');\"", strTruong, Convert.ToInt32(arrDSTruongDoRong[i]), dCot++);
                                strTerm = String.Format("Truong:'sTenDonViCoTen', GiaTri:request.term", strTruong);
                                GiaTriText = arrGT["sTenDonVi"];
                            }
                            else
                            {
                                strAttr = String.Format("class='input1_4' chi-tiet-du-toan='1' tab-index='-1' style='width:{1}px; height:25px; padding-right:2px;' ten-truong='{0}' onfocus=\"LSN_MaCotNganSach='{0}';\" cot='{2}' onblur=\"func_Auto_Complete_onblur(this.id,'{0}');\"", strTruong, Convert.ToInt32(arrDSTruongDoRong[i]), dCot++);
                                strAttr += " truong-ngan-sach='1'";
                            }
                            
                            %>
                            <td <%=(i==0)?"class='"+ParentID+"_FirstCol'":""%> style="padding:0px;">
                                <%=MyHtmlHelper.Autocomplete(ParentID, GiaTri, GiaTriText, strTruong, strTruongText, "", strAttr)%>
                                <%=MyHtmlHelper.AutoComplete_Initialize(ParentID + "_" + strTruongText, ParentID + "_" + strTruong, Url.Action("get_DanhSach", "Public"), strTerm, "func_Auto_Complete", new { delay = 100, minLength = 1 })%>
                            </td>
                            <%
                        }
                        %>
                        <td style="padding:0px;">
                            <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGT["sMoTa"], TenTruong = "sMoTa", Attributes = String.Format("khong-nhap='1' chi-tiet-du-toan='1' class='input1_3' style='height:25px; padding-right:2px;' cot='{0}'", dCot++) })%>
                        </td>
                        <td style="padding:0px;">
                            <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, TenTruong = "rTongSoNamTruoc", LoaiTextBox = "1", Attributes = String.Format("khong-nhap='1' chi-tiet-du-toan='1' class='input1_4' style='height:25px; padding-right:2px;' cot='{0}'", dCot++) })%>
                        </td>
                        <td style="padding:0px;">
                            <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, TenTruong = "rUocThucHien", LoaiTextBox = "1", Attributes = String.Format("tab-index='-1' chi-tiet-du-toan='1' class='input1_4' style='height:25px; padding-right:2px;' cot='{0}'", dCot++) })%>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
        <td valign="top" style="width:<%=DoRongBangFix%>px; border: solid 1px #525252;">
            <%--Cột tiền--%>
            <table id="<%=ParentID%>_tieude2" class="gridBang" style="width:100%;border-bottom: solid 0px #999;">
                <%--Hàng tiêu đề 1--%>
                <tr style="height:24px;">
                    <th colspan="<%=arrDSTruongTienTieuDe.Length + 1%>" style="border-bottom: solid 0px #999;">Kế hoạch năm <%=iNamLamViec%></th>
                </tr>
            </table>
            <div id="<%=ParentID%>_div2" style="overflow-x: scroll;overflow-y: hidden;position:relative;width:100px;" onscroll="fnScroll('<%=ParentID%>_div1','<%=ParentID%>_div2')">
                <table id="<%=ParentID%>_table2" class="gridBang" style="border-top: solid 0px #999;">
                    <%--Hàng tiêu đề 2--%>
                    <tr id="<%=ParentID%>_OFixBang2">
                        <%
                        for (int j = 0; j < arrDSTruongTienTieuDe.Length; j++)
                        {
                        %>
                            <th col-index='<%=j%>' style="width:<%=WidthCotTien%>px;border-bottom: solid 0px #999;"><%=arrDSTruongTienTieuDe[j]%></th>
                        <%
                        }
                        %>
                        <th style='width:<%=WidthCotTien%>px;border-bottom: solid 0px #999;'>Tổng số</th>
                    </tr>
                    <%--Hàng lịch sử--%>
                    <%
                    for (int i = 9;i >= dtChungTuChiTiet.Rows.Count ; i--)
                    {
                        String classtr = "";
                        if (i % 2 == 0)
                        {
                            classtr = "class=\"alt\"";
                        }
                        %>
                        <tr <%=classtr %> id="idHangTien<%=i%>" style="display:none;">
                            <%
                            for (int j = 0; j < arrDSTruongTienTieuDe.Length; j++)
                            {
                                %>
                                <td col-index='<%=j%>'></td>
                                <%
                            }
                            %>
                            <td></td>
                        </tr>
                    <%}
                    for (int i = dtChungTuChiTiet.Rows.Count - 1; 0 <= i; i--)
                    {
                        DataRow R = dtChungTuChiTiet.Rows[i];
                        String classtr = "";
                        if (i % 2 == 0)
                        {
                            classtr = "class=\"alt\"";
                        }
                        %>
                        <tr <%=classtr %> id="idHangTien<%=i %>">
                            <%
                            for (int j = 0; j < arrDSTruongTienTieuDe.Length; j++)
                            {
                                String strGT = "";
                                if (arrDSTruongTien[j].StartsWith("r"))
                                {
                                    strGT = DuToan_ChungTuChiTietModels.DinhDangTruongTien(R[arrDSTruongTien[j]]);
                                }
                                else
                                {
                                    strGT = Convert.ToString(R[arrDSTruongTien[j]]);
                                }
                                %>
                                <td col-index='<%=j%>'><%=strGT%></td>
                                <%
                            }
                            %>
                            <td><%=DuToan_ChungTuChiTietModels.DinhDangTruongTien(R["rTongSo"])%></td>
                        </tr>
                    <%} %>
                    <%--Hàng nhập dữ liệu--%>
                    <tr id="<%=ParentID%>_firstTR">
                        <%
                        for (int i = 0; i < arrDSTruongTien.Length; i++)
                        {
                            strAttr = String.Format("tab-index='-1' chi-tiet-du-toan='1' cot='{0}'", dCot++);
                            String LoaiTextBox = "1";
                            if (arrDSTruongTien[i].StartsWith("s"))
                            {
                                LoaiTextBox = "2";
                                strAttr += " class='input1_3'";
                                //strAttr += String.Format("style='width:{0}px; height:25px;margin-right: 10px;' ", WidthCotTien);
                                strAttr += String.Format("style='width:{0}px;height:25px;' ", WidthCotTien);
                                
                            }
                            else
                            {
                               strAttr += " class='input1_4'";
                                strAttr += String.Format("style='width:{0}px; height:25px;' ", WidthCotTien);
                            }
                            if (lstDuocNhapTruongTien[i]==false)
                            {
                                strAttr += " khong-nhap='1'";
                                strAttr += String.Format("style='width:{0}px;height:25px;' ", WidthCotTien);
                            }
                            //strAttr += String.Format("style='width:{0}px; height:25px;padding-right:2px; ' ", WidthCotTien);
                            if (arrDSTruongTien[i] == "sTenCongTrinh")
                            {
                                String strTruong = "sMaCongTrinh";
                                String strTruongText = "sTenCongTrinh";
                                strAttr += String.Format(" ten-truong='{0}' onblur=\"func_Auto_Complete_onblur(this.id,'{0}');\"", strTruong);
                                String strTerm = "DSGiaTri:funcLayMaDonVi, Truong:'sTenCongTrinh', GiaTri:request.term";
                                %>
                                <td col-index='<%=i%>' style="padding:0px;">
                                    <%=MyHtmlHelper.Autocomplete(ParentID, "", "", strTruong, strTruongText, "", strAttr)%>
                                    <%=MyHtmlHelper.AutoComplete_Initialize(ParentID + "_" + strTruongText, ParentID + "_" + strTruong, Url.Action("get_DanhSach", "Public"), strTerm, "func_Auto_Complete", new { delay = 100, minLength = 1 })%>
                                </td>
                                <%
                            }
                            else
                            {
                                strAttr += String.Format(" onblur=\"LayGiaTri(this.value,'{0}','{1}')\"", ParentID, arrDSTruongTien[i]);
                                %>
                                <td col-index='<%=i%>' style="padding:0px;">
                                    <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, TenTruong = arrDSTruongTien[i], LoaiTextBox = LoaiTextBox, Attributes = strAttr })%>
                                </td>
                                <%
                            }
                        }
                        %>
                        <td style="padding:0px;">
                            <%
                        strAttr =
                            String.Format(
                                "khong-nhap='1' class='input1_3' style='width:{0}px; height:25px; padding-right:2px;' cot='{1}'",
                                WidthCotTien, dCot++);
                                 %>
                            <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, TenTruong = "rTongSo", LoaiTextBox = "1", Attributes = strAttr })%>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>


<table width="100%" cellpadding="0" cellspacing="0" border="0" align="right" class="table_form2">
    <tr><td>&nbsp;</td></tr>
    <tr>
        <td align=center>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <input type="button" class="button" id="btnOK" tab-index='-1' value="<%=NgonNgu.LayXau("Thêm mới")%>" onclick="ThemMoiChiTietDuToan();"/>                        
                    </td>
                    <td>&nbsp;</td>
                    <td align="left">
                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td >&nbsp;</td></tr>
</table>
            
<%
    dtChungTuChiTiet.Dispose();
%>

<script src="<%= Url.Content("~/Scripts/jsDuToan_LichSuVaNhap.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>

<script type="text/javascript">
    var LSN_MaCotNganSach = '';
    var LSN_MaCotNganSach_Dien = '';
    var strParentID = '<%=ParentID%>';
    var strDSTruong = '<%=DSTruong%>';
    var arrDSTruongTien = '<%=DSTruongTien%>'.split(',');
    var arrDSHienThiTruongTien = '<%=DSHienThiTruongTien%>'.split(',');
    var arrDSDuocNhapTruongTien = '<%=DSDuocNhapTruongTien%>'.split(',');
    var arrGiaTriCu = new Array();
    var DoRongCotTiet = <%=WidthCotTien%>;
    var DuToan_LichSuVaNhap_iID_MaChungTu = "<%=iID_MaChungTu%>";

    $(document).ready(function () {
        jsLichSuVaNhap_UrlSave = '<%=Url.Action("CreateSubmit", "DuToan_ChungTuChiTiet", new { ParentID = ParentID, iID_MaChungTu = iID_MaChungTu })%>';
        jsLichSuVaNhap_Url_getGiaTri_public = '<%=Url.Action("get_GiaTri", "Public")%>';
        jsLichSuVaNhap_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "DuToan_ChungTuChiTiet")%>';
        LuuGiaTriHienTai();
        CapNhapHienThiCot();
        DuToan_LichSuVaNhap_fnAdjustTable(strParentID + '_FirstCol', strParentID + '_div2', strParentID + '_firstTR');
    });
</script>
<%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>