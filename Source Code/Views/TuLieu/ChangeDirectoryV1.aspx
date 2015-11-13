<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.IO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ChangeDirectory
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "ChangeDirectory";
        int i = 0;
        string contronllerName = Request.QueryString["contronllerName"];
        string iID_MaLoaiThuMuc = Request.QueryString["iID_MaLoaiThuMuc"];
        DataTable dt = TuLieuLichSuModels.LayDanhSachThuMuc(iID_MaLoaiThuMuc);
        DataTable dtLoaiThuMuc = DanhMucModels.DT_DanhMuc("LoaiThuMuc", true, "--- Chọn loại thư mục ---");
        SelectOptionList optLoaiThuMuc = new SelectOptionList(dtLoaiThuMuc, "iID_MaDanhMuc", "sTen");
        if (dtLoaiThuMuc != null) dtLoaiThuMuc.Dispose();
        Html.BeginForm("submit", "ChangeDirectoryV2", new { ParentID = ParentID, iID_MaLoaiThuMuc = iID_MaLoaiThuMuc, contronllerName = contronllerName });
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form3">
                <tr>
                    <td  style="width: 15%;">
                        <div>
                            Loại thư mục&nbsp;</div>
                    </td>
                    <td >
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optLoaiThuMuc, iID_MaLoaiThuMuc, "iID_MaLoaiThuMuc", "", "class=\"input1_2\" ")%>
                        </div>
                    </td>
                    <td  style="width: 15%;">
                        <div>
                            <input class="button" type="submit" value="Lọc" title="Lọc" />
                       
                        <% Html.EndForm(); %>
                        <%
                            //using (Html.BeginForm("SaveSubmit", "ChangeDirectoryV2", new { ParentID = ParentID, iID_MaLoaiThuMuc = iID_MaLoaiThuMuc, contronllerName = contronllerName }))
                            Html.BeginForm("SaveSubmit", "ChangeDirectoryV2",
                                           new
                                               {
                                                   ParentID = ParentID,
                                                   iID_MaLoaiThuMuc = iID_MaLoaiThuMuc,
                                                   contronllerName = contronllerName
                                               });
                            {%>
                        <%=MyHtmlHelper.Hidden(ParentID, "", "iID_ThuMucChon", "")%>
                        <input class="button" type="submit" value="Lưu" title="Lưu" />
                         </div>
                        <% } %>
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%; height: 100%" class="mGrid">
            <tr>
                <th style="width: 10%;" align="center">
                    STT
                </th>
                <th style="width: 70%;" align="center">
                    Thư mục
                </th>
                <th style="width: 20%;" align="center">
                    Ngày tạo
                </th>
            </tr>
            <%
                if (dt != null)
                {


                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        int sSTT = i + 1;
                        DataRow R = dt.Rows[i];
                        DateTime dtCreate = Convert.ToDateTime(R["dNgayTao"]);
                        string shortDate = dtCreate.ToString("dd/MM/yyyy");
                        string iID_MaThuMucTaiLieu = Convert.ToString(R["iID_MaThuMucTaiLieu"]);
                        string MaLoaiThuMuc = Convert.ToString(R["iID_MaLoaiThuMuc"]);
                        bool sHoatDong = Convert.ToString(R["bHoatDong"]).Equals("True") ? true : false;
                        string strChk = string.Empty;
                        if (sHoatDong)
                        {
                            strChk = "CHECKED";
                            string function = "setvalue(" + iID_MaThuMucTaiLieu + "_" + MaLoaiThuMuc+")";
                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", function, true);
                            
                        }                        
            %>
            <tr style="height: 20px">
                <td align="center" style="padding: 3px 2px;">
                    <%=sSTT%>
                </td>
                <td align="left" >
                    <input type="RADIO" name="ThuMuc" onchange="handleChange(this.form);" value='<%= iID_MaThuMucTaiLieu %>_<%=MaLoaiThuMuc %> ' <%= strChk %>> &nbsp;<%= R["sTen"] %>
                  
                </td>
                <td align="center">
                    <%=  shortDate %>
                </td>
            </tr>
            <%}
                        }%>            
        </table>
    </div>
    <%Html.EndForm();%>
    <script type="text/javascript">
        function handleChange(aspnetForm) {
            for (var i = 0; i < aspnetForm.elements.length; i++) {
                if ((aspnetForm.elements[i].checked == true)) {
                    document.getElementById("<%=ParentID%>_iID_ThuMucChon").value = aspnetForm.elements[i].value;
                }
            }

        }
        function setValue(newvalue) {
            document.getElementById("<%=ParentID%>_iID_ThuMucChon").value = newvalue;
        }
    
    </script>
</asp:Content>
