<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">,
<%
    String ParentID = "Edit";
    String iNamLamViec = Request.QueryString["iNamLamViec"];
    String iLoai = Request.QueryString["iLoai"];
    String iID_MaLoaiTaiSan = Request.QueryString["iID_MaLoaiTaiSan"];
    String iID_MaNhomTaiSan= Request.QueryString["iID_MaNhomTaiSan"];
    String Saved = Request.QueryString["Saved"];
    String iID_MaTaiSan = Request.QueryString["iID_MaTaiSan"];
    DataTable dlLoaiTS = KTCS_TaiSanModels.DT_NhomTaiSan();
    SelectOptionList slLoaiTS = new SelectOptionList(dlLoaiTS, "MaLoaiTS", "sTen");
    dlLoaiTS.Dispose();

    DataTable dtDanhMucLoaiTS = KTCS_TaiSanModels.ddl_DanhMucNhomTaiSan(true);
    SelectOptionList slDanhMucLoaiTaiSan = new SelectOptionList(dtDanhMucLoaiTS, "iID_MaNhomTaiSan", "sTen");
    dtDanhMucLoaiTS.Dispose();

    DataTable dtNam = DanhMucModels.DT_Nam(true);
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    dtNam.Dispose();
    
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    
    String[] arrDSTruong = "iNamLamViec,iID_MaTaiSan".Split(',');
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
    DataTable dtTaiSan = KTCS_TaiSanModels.Get_Table_TaiSan(iLoai, iID_MaNhomTaiSan, arrGiaTriTimKiem);
 %>


<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("List", "KTCS_TaiSan"), "Tài sản")%>
            </div>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#pHeader').click(function () {
            $('#dvContent').slideToggle('slow');
        });
    });
    $(document).ready(function () {      
        $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
    });            
</script>
<div id="ContainerPanel" class="ContainerPanel">
    <div id="pHeader" class="collapsePanelHeader"> 
        <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Tìm kiếm thông tin tài sản")%></span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 50%;">
                    <div id="nhapform">
                        <div id="form2">
                            <%
                            using (Html.BeginForm("SearchSubmit", "KTCS_TaiSan", new { ParentID = ParentID }))
                            {       
                            %>
                            <%=MyHtmlHelper.Hidden(ParentID,"List","List","") %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="50%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Năm</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\"")%>    
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Mã tài sản</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.TextBox(ParentID, iID_MaTaiSan, "iID_MaTaiSan", "", "class=\"input1_2\"")%>        
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Loại tài sản</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiTS, iLoai, "slLoaiTS", "", "class=\"input1_2\"")%>    
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Danh mục tài sản</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slDanhMucLoaiTaiSan, iID_MaLoaiTaiSan, "iID_MaNhomTaiSan", "", "class=\"input1_2\"")%>        
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="td_form2_td1" align="right">
            	                            <div style="text-align:right; float:right; width:100%">
                                                <input type="submit" class="button4" value="Tìm kiếm" style="float:right; margin-left:10px;"/>
            	                            </div> 
            	                        </td>
                                    </tr>
                                    <tr><td class="td_form2_td1" align="right" colspan="2"></td></tr>
                                </table>
                            <%} %>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Danh sách tài sản</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">                    
                    <table cellpadding="0" cellspacing="0" border="0" class="mGrid" width="100%">
                        <tr>
                            <th>STT</th>
                            <th>Loại</th>
                            <th>Nhóm</th>
                            <th>Mã tài sản</th>
                            <th>Tên tài sản</th>
                            <th>Số năm hao mòn</th>
                            <th>Nguyên giá</th>
                            <th>Trạng thái</th>
                            <th>Thao tác</th>
                        </tr>
                        <%for (int i = 0; i < dtTaiSan.Rows.Count; i++)
                          {
                              String MaTaiSan = Convert.ToString(dtTaiSan.Rows[i]["iID_MaTaiSan"]);
                              String rNguyenGia=Convert.ToString(dtTaiSan.Rows[i]["rNguyenGia"]);
                              String urlThanhLy = Url.Action("ThanhLy", "KTCS_TaiSan", new { iID_MaTaiSan = MaTaiSan });
                              String urlLichSuDieuChuyen = Url.Action("Index", "KTCS_TaiSan_DonVi", new { iID_MaTaiSan = MaTaiSan });
                              String TrangThai = "Chưa thanh lý";
                              switch(Convert.ToString(dtTaiSan.Rows[i]["iTrangThaiTaiSan"]))
                              {
                                  case "1":
                                  case "2":
                                      TrangThai = "Đang sử dụng";
                                     break;
                                  case "3":
                                      TrangThai = "Loại khỏi biên chế";
                                      break;
                                  case "4":
                                      TrangThai = "Điều chuyển";
                                      break;
                                      
                              }
                          %>
                              
                        <tr>
                            <td><%=i+1 %></td>
                            <td><%=dtTaiSan.Rows[i]["iLoaiTS"]%></td>
                            <td><%=dtTaiSan.Rows[i]["sTenNhomTaiSan"]%></td>
                            <td><%=dtTaiSan.Rows[i]["iID_MaTaiSan"]%></td>
                            <td><%=dtTaiSan.Rows[i]["sTenTaiSan"]%></td>
                            <td><%=dtTaiSan.Rows[i]["rSoNamKhauHao"]%></td>
                            <td align="right"><%=CommonFunction.DinhDangSo(rNguyenGia)%></td>
                            <td><%=TrangThai%></td>
                            <td>
                                <div style="float: right; margin-right: 5px;" onclick="OnInit_CT();">      
                                    <%= Ajax.ActionLink("Trạng thái", "Index", "NhapNhanh", new { id = "TrangThai_TaiSan", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaTaiSan = MaTaiSan}, new AjaxOptions { }, new { @class = "button_title" })%>                                
                                  </div>                            
                            </td>
                        </tr>
                        <%} %>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function CallSuccess_CT() {
        location.reload();
        return false;
    }
    function OnInit_CT() {
        $("#idDialog").dialog("destroy");
        document.getElementById("idDialog").title = 'Trạng thái';
        document.getElementById("idDialog").innerHTML = "";
        $("#idDialog").dialog({
            resizeable: false,
            width: 400,
            modal: true
        });
    }
    function OnLoad_CT(v) {
        document.getElementById("idDialog").innerHTML = v;
    }
</script>
  
<div id="idDialog" style="display: none;">    
</div>
</asp:Content>
