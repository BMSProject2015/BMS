<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers.KeToanTongHop" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID = ControlID + "_Search";
    String iID_MaChungTu = "";
    String iID_MaChungTuChiTiet = "";
    //String iID_MaChungTu = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
    //String iID_MaChungTuChiTiet = Request.QueryString["iID_MaChungTuChiTiet"];
    //if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(props["iID_MaChungTu"].GetValue(Model)); ;
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;
    //int iThang = Convert.ToInt32(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iThangLamViec"));

    //Cập nhập các thông tin tìm kiếm
    String DSTruong = "TrangThai";
    String[] arrDSTruong = DSTruong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
    String iNam = Convert.ToString(ViewData["iNam"]);
    String iQuy = Convert.ToString(ViewData["iQuy"]);
    String iLoai = Convert.ToString(ViewData["iLoai"]);
    if (String.IsNullOrEmpty(iNam)) iNam = Request.QueryString["iNam"];
    if (String.IsNullOrEmpty(iQuy)) iQuy = Request.QueryString["iQuy"];
    int iThang = Convert.ToInt32(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iThangLamViec"));
    String iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
    if (String.IsNullOrEmpty(iQuy))
    {
        iQuy = Convert.ToString(iThang);
        //if (iThang > 0 && iThang <= 3)
        //{
        //    iQuy = "1";
        //}
        //else if (iThang > 3 && iThang <= 6)
        //{
        //    iQuy = "2";
        //}
        //else if (iThang > 6 && iThang <= 9)
        //{
        //    iQuy = "3";
        //}
        //else if (iThang > 9 && iThang <= 12)
        //{
        //    iQuy = "4";
        //}
        //else
        //{
        //    iQuy = "1";
        //}
    }

    if (String.IsNullOrEmpty(iLoai)) iLoai = Request.QueryString["iLoai"];
    if (String.IsNullOrEmpty(iNam)) iNam = iNamLamViec;
    DataTable dtNam = DanhMucModels.DT_Nam(false);
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    dtNam.Dispose();


    DataTable dtLoaiThuChi = DanhMucModels.getLoaiThuChi();
    SelectOptionList slLoaiThuChi = new SelectOptionList(dtLoaiThuChi, "iIDLoai", "sTen");
    dtLoaiThuChi.Dispose();

    DataTable dtQuy = DanhMucModels.DT_Thang(false);
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaThang", "TenThang");
    dtQuy.Dispose();
    KeToanTongHop_CanDoiThuChiTaiChinh_BangDuLieu bang = new KeToanTongHop_CanDoiThuChiTaiChinh_BangDuLieu(iNam, iQuy,
                                                                                                           iLoai,
                                                                                                           arrGiaTriTimKiem,
                                                                                                           MaND, IPSua);

    String BangID = "BangDuLieu";
    int Bang_Height = 570;
    int Bang_FixedRow_Height = 50;
    int csH = 0;
    String UrlGiaiThich = Url.Action("Index", "KeToanTongHop_CanDoiThuChiTaiChinh");
    String URL = Url.Action("SoDoLuong", "KeToanTongHop");
    String strIn = Url.Action("Index", "rptKeToanTongHop_CanDoiThuChiTaiChinh");
    String strTaoCT = Url.Action("Edit", "KeToanTongHop_CanDoiThuChiTaiChinh");

    int Count = KeToanTongHop_CanDoiThuChiTaiChinhController.KiemTra_TonTai_ThuChi(Convert.ToInt32(iQuy),
                                                                                   Convert.ToInt32(iNam));
%>
<%--<form action="<%=Url.Action("SearchSubmit", "KeToanTongHop_CanDoiThuChiTaiChinh", new{ParentID = ParentID, iNam = iNam})%>"
method="post">--%>
<div class="box_tong">
    <div class="title_tong">
        <table style="background: #DFF0FB;" width="100%" border="0">
            <tr>
                <td>
                    <span>Cân đối thu chi tài chính</span>
                </td>
                <td width="30%" align="right">
                    <b>Loại thu/chi: &nbsp;</b>
                </td>
                <td width="100px">
                    <%=MyHtmlHelper.DropDownList(ParentID, slLoaiThuChi, Convert.ToString(iLoai), "iLoai", "", "onchange=\"ChonThangNam(this.value)\" style=\"width:80px;\"")%>
                </td>
                <td width="60px" align="right">
                    <b>Tháng: &nbsp;</b>
                </td>
                <td width="80px">
                    <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Convert.ToString(iQuy), "iQuy", "", "onchange=\"ChonThangNam(this.value)\" style=\"width:80px;\"")%>
                </td>
                <td width="60px" align="right">
                    <b>Năm: &nbsp;</b>
                </td>
                <td width="100px">
                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, Convert.ToString(iNam), "iNam", "", "onchange=\"ChonThangNam(this.value)\" style=\"width:80px;\"")%>
                </td>
                <%
                    if (Count == 0)
                    {
                %>
                <td align="right" width="90px">
                    <input type="button" id="Button2" class="button" value="<%=NgonNgu.LayXau("Tạo T/C")%>"
                        onclick="javascript:location.href='<%=strTaoCT%>'" />
                </td>
                <%
                      }
                %>
                <td align="right" width="100px">
                    <input type="button" id="Button1" class="button" value="<%=NgonNgu.LayXau("In báo cáo")%>"
                        onclick="javascript:location.href='<%=strIn %>'" />
                </td>
                <%
                    if (bang.ChiDoc == false)
                    {
                %>
                <td align="right" width="90px">
                    <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                        value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left" width="100px">
                    <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />
                </td>
                <%
                    }
                %>
                <%-- <td>
                    <input type="submit" class="button" id="<%=ParentID%>_btnTimKiem" <%=bang.DuocSuaChiTiet? "":"tab-index='-1'" %>
                        value="<%=NgonNgu.LayXau("Tìm kiếm")%>" style="font-size: 11px; padding: 0px 3px;" />
                </td>--%>
            </tr>
        </table>
    </div>
</div>
<div class="box_tong">
    <div id="nhapform">
        <div id="form2">
            <%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>
            <div style="display: none;">
                <input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
                <input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
                <input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>" />
                <input type="hidden" id="idBangChiDoc" value="<%=HttpUtility.HtmlEncode(bang.strChiDoc)%>" />
                <input type="hidden" id="idXauEdit" value="<%=HttpUtility.HtmlEncode(bang.strEdit)%>" />
                <input type="hidden" id="idViewport_N" value="<%=HttpUtility.HtmlEncode(bang.Viewport_N)%>" />
                <input type="hidden" id="idNC_Fixed" value="<%=HttpUtility.HtmlEncode(bang.nC_Fixed)%>" />
                <input type="hidden" id="idNC_Slide" value="<%=HttpUtility.HtmlEncode(bang.nC_Slide)%>" />
                <input type="hidden" id="idCoCotTongSo" value="0" />
                <%  
                    if (bang.ChiDoc == false)
                    {
                %>
                <form action="<%=Url.Action("DetailSubmit", "KeToanTongHop_CanDoiThuChiTaiChinh", new{ParentID = ParentID, iNam = iNam,iQuy=iQuy,iLoai=iLoai})%>"
                method="post">
                <%
                    } %>
                <input type="hidden" id="iID_MaChungTu" name="iID_MaChungTu" value="<%=iID_MaChungTu%>" />
                <input type="hidden" id="sSoChungTu" name="sSoChungTu" value="" />
                <input type="hidden" id="iNgay" name="iNgay" value="" />
                <input type="hidden" id="iThang" name="iThang" value="" />
                <input type="hidden" id="iTapSo" name="iTapSo" value="" />
                <input type="hidden" id="sDonVi" name="sDonVi" value="" />
                <input type="hidden" id="sNoiDung" name="sNoiDung" value="" />
                <input type="hidden" id="idAction" name="idAction" value="0" />
                <input type="hidden" id="idXauDuLieuThayDoi" name="idXauDuLieuThayDoi" value="<%=HttpUtility.HtmlEncode(bang.strThayDoi)%>" />
                <input type="hidden" id="idXauLaHangCha" name="idXauLaHangCha" value="<%=HttpUtility.HtmlEncode(bang.strLaHangCha)%>" />
                <input type="hidden" id="idXauMaCacHang" name="idXauMaCacHang" value="<%=HttpUtility.HtmlEncode(bang.strDSMaHang)%>" />
                <input type="hidden" id="idXauMaCacCot" name="idXauMaCacCot" value="<%=HttpUtility.HtmlEncode(bang.strDSMaCot)%>" />
                <input type="hidden" id="idXauGiaTriChiTiet" name="idXauGiaTriChiTiet" value="<%=HttpUtility.HtmlEncode(bang.strDuLieu)%>" />
                <input type="submit" id="btnXacNhanGhi" value="XN" />
                <input type="hidden" id="idXauCacHangDaXoa" name="idXauCacHangDaXoa" value="" />
                <%
                    if (bang.ChiDoc == false)
                    {
                %>
                </form>
                <%
                    }
                %>
            </div>
            <script type="text/javascript">
    $(document).ready(function() {
//          $('#dvText').show();
//            $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
//            $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
//            $(window).resize(function () {
//                $('.popup_block').css({
//                    position: 'absolute',
//                    left: ($(window).width() - $('.popup_block').outerWidth()) / 2,
//                    top: ($(window).height() - $('.popup_block').outerHeight()) / 2
//                });
//            });
//            // To initially run the function:
//            $(window).resize();
//        $('#dvText').hide();
            //Fade in Background
//        parent.ddl_reset('<%=HttpUtility.HtmlEncode(bang.strDSMaND)%>');
        Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
        BangDuLieu_iID_MaChungTu = '<%=iID_MaChungTu%>';
        BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%> ;
        BangDuLieu_sMauSac_TuChoi = '<%=bang.sMauSac_TuChoi %>';
        BangDuLieu_sMauSac_DongY = '<%=bang.sMauSac_DongY %>';
        Bang_keys.fnSetFocus( <%=csH %> , 0);
        jsKeToan_url_Tao_CTGS = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_TAOCTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" })%>';
    });
    
    function ChonThangNam(value) {
        var iNam = document.getElementById('<%=ParentID %>_iNam').value;
        var iQuy = document.getElementById('<%=ParentID %>_iQuy').value;
        var iLoai = document.getElementById('<%=ParentID %>_iLoai').value;
        var URL = '<%=UrlGiaiThich %>';
        URL += "?iNam=" + iNam + "&iQuy=" + iQuy + "&iLoai=" + iLoai;
        window.location.href = URL;
    }
      function Huy() {
            window.location.href = '<%=URL %>';
        }
      
       function OnInit_CT(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizeable: false,
                draggable: true,
                width: value,
                modal: true,
                open: function (event, ui) {
                    $(event.target).parent().css('position', 'fixed');
                    $(event.target).parent().css('top', '50px');
                    // $(event.target).parent().css('left', '10px');
                }
            });
        }
            </script>
        </div>
        <div id="idDialog" style="display: none;">
        </div>
        <div id="dvText" class="popup_block">
            <img src="../../../Content/ajax-loader.gif" /><br />
            <p>
                Hệ thống đang thực hiện yêu cầu...</p>
        </div>
    </div>
</div>
