<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%
    String ParentID = "", strOnSuccess = "";
    PropertyInfo[] properties = Model.GetType().GetProperties();

    for (int i = 0; i < properties.Length; i++)
    {
        switch (properties[i].Name)
        {
            case "ControlID":
                ParentID = (string)(properties[i].GetValue(Model, null));
                break;

            case "OnSuccess":
                strOnSuccess = (string)(properties[i].GetValue(Model, null));
                break;
        }
    }
    String iID_MaBangLuongChiTiet = Request.QueryString["iID_MaBangLuongChiTiet"];
    DataTable dt = LuongModels.Get_dtDanhMucPhuCap();
    DataTable dtPhuCap=LuongModels.Get_dtLuongPhuCap(iID_MaBangLuongChiTiet);
   int RCount = dt.Rows.Count;
   using (Ajax.BeginForm("Edit_LuongPhuCap_Submit", "Luong_BangLuongChiTiet", new { ParentID = ParentID, OnSuccess = strOnSuccess }, new AjaxOptions { }))
        {
     %>
     <%=MyHtmlHelper.Hidden(ParentID,iID_MaBangLuongChiTiet,"iID_MaBangLuongChiTiet","") %>
<div style="background-color: #ffffff; background-repeat: repeat">
    <div style="padding: 5px 1px 10px 1px;">

        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="mGrid">
                    <tr>
                        <th>Chọn phụ cấp</th>
                        <th>Ký hiệu</th>
                        <th>Nội dung</th>
                        <th>Số tiền</th>
                    </tr>
                        <%
                            DataRow R,R1;
                            Boolean bCongThuc = false;
                            String iID_MaPhuCap, _checked;
                            Object CongThuc,rMuc,rSoTien,sCongThuc;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                            R = dt.Rows[i];
                            iID_MaPhuCap = Convert.ToString(R["iID_MaPhuCap"]);
                            CongThuc= R["bCongThuc"];
                            rMuc = R["rMuc"];
                            rSoTien = R["rSoTien"];
                           sCongThuc=R["sCongThuc"];
                            _checked = "";
                            for (int j = 0; j < dtPhuCap.Rows.Count; j++)
                            {
                                R1 = dtPhuCap.Rows[j];   
                                if (iID_MaPhuCap == Convert.ToString(dtPhuCap.Rows[j]["iID_MaPhuCap"]))
                                {
                                    _checked = "checked=\"checked\"";
                                    CongThuc = R1["bCongThuc"];
                                    rMuc = R1["rMuc"];
                                    rSoTien = R1["rSoTien"];
                                    sCongThuc = R1["sCongThuc"];
                                    break;
                                }
                            }
                                

                                bCongThuc = Convert.ToBoolean(R["bCongThuc"]);
                            String strReadonly = "", strColor = "";
                            if (bCongThuc)
                            {
                                strReadonly = "readonly=\"readonly\"";
                                strColor = "background-color:#CFCCCC";
                            }
                            %>
                        <tr>
                            <td>
                                <input type="checkbox" <%=_checked %> id="<%=ParentID %>_iID_MaPhuCap" name="<%=ParentID %>_iID_MaPhuCap" value="<%=R["iID_MaPhuCap"]%>" />                                
                                <%=MyHtmlHelper.Hidden(ParentID + "_" + iID_MaPhuCap, R["iID_MaPhuCap"], "iID_MaPhuCap", "")%>
                                <%=MyHtmlHelper.Hidden(ParentID + "_" + iID_MaPhuCap, CongThuc, "bCongThuc", "")%>
                                <%=MyHtmlHelper.Hidden(ParentID + "_" + iID_MaPhuCap, sCongThuc, "sCongThuc", "")%>
                                <%=MyHtmlHelper.Hidden(ParentID + "_" + iID_MaPhuCap, rMuc, "rMuc", "")%>                                
                            </td>
                            <td><%=HttpUtility.HtmlEncode(R["iID_MaPhuCap"])%></td>
                            <td><%=HttpUtility.HtmlEncode(R["sNoiDung"]) %></td>
                            <td>                      
                                <%=MyHtmlHelper.TextBox(ParentID + "_" + iID_MaPhuCap, rSoTien, "rSoTien", "", String.Format("{0} style=\"width:98%;{1}\"", strReadonly, strColor))%>                              
                            </td>
                        </tr>
                        <%} %>
                </table>
            </div>
        </div>
      
    </div>
</div>
 <table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td width="65%" class="td_form2_td5">&nbsp;</td>   
        <td width="30%" align="right" class="td_form2_td5">
            <input type="submit" class="button" id="Submit1" value="Lưu" />
        </td>          
            <td width="5px">&nbsp;</td>          
        <td class="td_form2_td5">
            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
        </td>
    </tr>
</table>
<%} %>