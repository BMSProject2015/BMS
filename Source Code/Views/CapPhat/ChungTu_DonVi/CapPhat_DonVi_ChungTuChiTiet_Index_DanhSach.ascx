<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    Boolean CoTabIndex = (props["CoTabIndex"] == null) ? false : Convert.ToBoolean(props["CoTabIndex"].GetValue(Model));
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID = ControlID + "_Search";
    String iID_MaCapPhat = Request.QueryString["iID_MaCapPhat"];
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;

    //Cập nhập các thông tin tìm kiếm
    String DSTruong = MucLucNganSachModels.strDSTruong;
    String[] arrDSTruong = DSTruong.Split(',');
    String strDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong;
    String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }
    
    //VungNV: 2015/10/28 lấy giá trị sLoai
    DataTable dtChungTu = CapPhat_ChungTuModels.LayToanBoThongTinChungTu(iID_MaCapPhat);
    DataRow RChungTu = dtChungTu.Rows[0];
    String sLoai = Convert.ToString(RChungTu["sLoai"]);
    dtChungTu.Dispose();
    int indexLoai = 0;
    
    if(!String.IsNullOrEmpty(sLoai))
    {
        for (int i = 0; i < arrDSTruong.Length; i++ ) 
        {
            if(arrDSTruong[i] == sLoai)
            {
                indexLoai = i;
                break;
            }
        }
    }
  
%>

<div class="box_tong">
    <div id="nhapform">
        <div id="form2">

    <form action="<%=Url.Action("SearchSubmit","CapPhat_ChungTu_DonVi",new {ParentID = ParentID, iID_MaCapPhat = iID_MaCapPhat})%>" method="post">
            <!--VungNV: 2015/10/28 update search textbox  -->
         <table class="mGrid1">
                    <tr>
                        <%
                            for (int j = 0; j <= indexLoai; j++)
                            {
                                int iColWidth = Convert.ToInt32(arrDSTruongDoRong[j]) + 4;
                                if (j == 0) iColWidth = iColWidth + 3;
                                String strAttr = String.Format("class='input1_4' onkeypress='jsCapPhat_Search_onkeypress(event)' search-control='1' search-field='{1}' style='width:{0}px;height:22px;'", iColWidth - 2, arrDSTruong[j]);
                                if (CoTabIndex)
                                {
                                    strAttr += " tab-index='-1'";
                                }
                        %>
                            <td style="text-align: left; width: <%=iColWidth%>px;">
                                <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[arrDSTruong[j]], TenTruong = arrDSTruong[j], LoaiTextBox = "2", Attributes = strAttr })%>
                            </td>
                        <%
                            }
                        %>
                    </tr>
                </table>
                  <iframe id="ifrChiTietChungTu" width="100%" height="530px" src="<%= Url.Action("CapPhatChiTiet_Frame", "CapPhat_ChungTu_DonVi", new {iID_MaCapPhat=iID_MaCapPhat})%>">
                </iframe>
    </form>

        <script type="text/javascript">
            $(document).ready(function () {
                Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
                Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
                Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
            });
        </script>

        </div>
    </div>
</div>