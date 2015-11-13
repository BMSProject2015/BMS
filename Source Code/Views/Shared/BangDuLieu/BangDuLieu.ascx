<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    BangDuLieu bang = (BangDuLieu)(props["bang"].GetValue(Model));
    String BangID = Convert.ToString(props["BangID"].GetValue(Model));
    int Bang_Height = Convert.ToInt16(props["Bang_Height"].GetValue(Model));
    int Bang_FixedRow_Height = Convert.ToInt16(props["Bang_FixedRow_Height"].GetValue(Model));
    int Bang_ChiTiet_Height = Bang_Height - Bang_FixedRow_Height;

    String HienThiPhan_Fixed = "";
    if (bang.nC_Fixed==0)
    {
        HienThiPhan_Fixed = "display:none;";
    }
    String HienThiPhan_Slide = "";
    if (bang.nC_Slide == 0)
    {
        HienThiPhan_Slide = "display:none;";
    }
    String Rowspan_Fixed = "";
    if (bang.CoNhomCot_Fixed)
    {
        Rowspan_Fixed = "rowspan='2'";
    }
    String Rowspan_Slide = "";
    if (bang.CoNhomCot_Slide)
    {
        Rowspan_Slide = "rowspan='2'";
    }

    int Bang_Width_Fixed = bang.WidthFixed;
    int Bang_Width_Slide = bang.WidthSlide;
    
%>

<script type="text/javascript">
    $(document).ready(function () {
        Bang_sMauSac_ChuaDuyet = "<%=bang.sMauSac_ChuaDuyet %>";
        Bang_sMauSac_TuChoi = "<%=bang.sMauSac_TuChoi %>";
        Bang_sMauSac_DongY = "<%=bang.sMauSac_DongY %>";
    });
</script>
<input type="hidden" id="idXauDSCotDuocPhepNhap" value="<%=bang.strDSCotDuocPhepNhap%>" />
<input type="hidden" id="idXauDinhDangDuLieu" value="<%=bang.strFormat%>" />
<div id="<%=BangID%>_div" style="width: 100%; height:<%=Bang_Height%>px; padding:0px;">
    <table id="<%=BangID%>" cellspacing="0" cellpadding="0" border="0" style="width: 100%; background-color: #fff; margin: 0px;">
        <tr style="background-color: #006666; background-repeat: repeat;">
            <td valign="top" align="left" style="<%=HienThiPhan_Fixed%>width:<%=Bang_Width_Fixed%>px; border: solid 1px #525252;">
                <%--Ô tiêu đề trên trái--%>
                <table id="<%=BangID%>_TB00" class="gridBang" style="width:<%=Bang_Width_Fixed%>px; height:<%=Bang_FixedRow_Height%>px;">
                    <tr>
                        <%
                            for (int j = 0; j < bang.nC_Fixed; j = j + bang.arrSoCotCungNhom[j])
                            {
                                if (bang.arrSoCotCungNhom[j] <= 1)
                                {
                                    //Hàng tiêu đề chi tiết
                                    String strAttr = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                    String strAttrSpan = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                    if (bang.arrHienThiCot[j] == false)
                                    {
                                        strAttr += "display:none;";
                                    }
                                    %>
                                    <th id='<%=BangID%>_th<%=j%>' <%=Rowspan_Fixed%> style='<%=strAttr%>'><span style="<%=strAttrSpan%>"><%=bang.arrTieuDe[j]%></span></th>
                                    <%
                                }
                                else
                                {
                                    //Hàng tiêu đề nhóm
                                    %>
                                    <th colspan='<%=bang.arrSoCotCungNhom[j]%>'><span><%=bang.arrTieuDeNhomCot[j]%></span></th>
                                    <%
                                }
                            }
                        %>
                    </tr>
                    <%
                        if (bang.CoNhomCot_Fixed)
                        {
                            %>
                            <tr>
                                <%
                                    for (int j = 0; j < bang.nC_Fixed; j++)
                                    {
                                        if (bang.arrSoCotCungNhom[j] > 1)
                                        {
                                            String strAttr = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                            String strAttrSpan = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                            if (bang.arrHienThiCot[j] == false)
                                            {
                                                strAttr += "display:none;";
                                            }
                                        %>
                                        <th id='<%=BangID%>_th<%=j%>' style='<%=strAttr%>'><span style="<%=strAttrSpan%>"><%=bang.arrTieuDe[j]%></span></th>
                                        <%
                                        }
                                    }
                                %>
                            </tr>
                            <%
                        }
                    %>
                </table>
            </td>
            <td valign="top" align="left" style="<%=HienThiPhan_Slide%>border: solid 1px #525252;">
                <%--Ô tiêu đề trên phải--%>
                <div id="<%=BangID%>_TB01_div" style="overflow:hidden;position:relative;width:100px;">
                    <table id="<%=BangID%>_TB01" class="gridBang" style="width:<%=Bang_Width_Slide%>px;height:<%=Bang_FixedRow_Height%>px;">
                        <tr>
                        <%
                            for (int j = bang.nC_Fixed; j < bang.nC_Fixed + bang.nC_Slide; j = j + bang.arrSoCotCungNhom[j])
                            {
                                if (bang.arrSoCotCungNhom[j] <= 1)
                                {
                                    //Hàng tiêu đề chi tiết
                                    String strAttr = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                    String strAttrSpan = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                    if (bang.arrHienThiCot[j] == false)
                                    {
                                        strAttr += "display:none;";
                                    }
                                    %>
                                    <th id='<%=BangID%>_th<%=j%>' <%=Rowspan_Slide%> style='<%=strAttr%>'><span style="<%=strAttrSpan%>"><%=bang.arrTieuDe[j]%></span></th>
                                    <%
                                }
                                else
                                {
                                    //Hàng tiêu đề nhóm
                                    int ColSpan = 0;
                                    for (int i = j; i < j + bang.arrSoCotCungNhom[j]; i++)
                                        if (bang.arrHienThiCot[i])
                                            ColSpan++;
                                    if (ColSpan > 0)
                                    {                        
                                        %>
                                        <th colspan='<%=ColSpan%>'> <span><%=bang.arrTieuDeNhomCot[j]%></span></th>
                                        <%
                                     }
                                }
                            }
                        %>
                        </tr>
                        <%
                        if (bang.CoNhomCot_Slide)
                        {
                            %>
                            <tr>
                                <%
                                    for (int j = bang.nC_Fixed; j < bang.nC_Fixed + bang.nC_Slide; j++)
                                    {
                                        if (bang.arrSoCotCungNhom[j] > 1 && bang.arrHienThiCot[j])
                                        {
                                            String strAttr = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                            String strAttrSpan = String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                        %>
                                        <th id='<%=BangID%>_th<%=j%>' style='<%=strAttr%>'><span style="<%=strAttrSpan%>"><%=bang.arrTieuDe[j]%></span></th>
                                        <%
                                        }
                                    }
                                %>
                            </tr>
                            <%
                        }
                    %>
                    </table>
                </div>
            </td>
            <td valign="top" align="left" style="min-width:17px; max-width:17px; width:17px; border: solid 1px #525252;">
                <%--Ô tiêu đề cho scrollBar--%>
                <table class="gridBang" style="width:100%; height:<%=Bang_FixedRow_Height%>px;">
                    <tr>
                        <th></th>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="<%=BangID%>_TR_DuLieu">
            <td valign="top" align="left" style="<%=HienThiPhan_Fixed%>border: solid 1px #525252;">
                <%--Ô cột dữ liệu Fixed--%>
                <div id="<%=BangID%>_TB10_div" style="overflow:hidden;position:relative;width:100%; height:100px;">
                    <div style="width:<%=bang.WidthFixed%>px;height:<%=bang.Height+1%>px;">
                        <table id="<%=BangID%>_TB10" class="gridBang" style="width:100%;"><%
                            for (int i = 0; i < bang.Viewport_N; i++)
                            {
                                String strAttrRow="";
                                if (i >= bang.arrDSMaHang.Count)
                                {
                                    strAttrRow += "display:none;";
                                }
                                if (strAttrRow != "")
                                {
                                    strAttrRow = String.Format(" style='{0}'", strAttrRow);
                                }
                                %><tr<%=strAttrRow%>><%
                                for (int j = 0; j < bang.nC_Fixed; j++)
                                {
                                    String strAttr = "";
                                    if (i == 0)
                                    {
                                        strAttr += String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                    }
                                    if (bang.arrAlign[j] != "right")
                                    {
                                        strAttr += String.Format("text-align:{0};", bang.arrAlign[j]);
                                    }
                                    if (bang.arrHienThiCot[j] == false)
                                    {
                                        strAttr += "display:none;";
                                    }
                                    if (strAttr != "")
                                    {
                                        strAttr = String.Format(" style='{0}'", strAttr);
                                    }
                                    %><td<%=strAttr%>></td><%
                                }
                                %></tr><%
                            }%>
                        </table>
                    </div>
                </div>
            </td>
            <td colspan="2" valign="top" align="left" style="<%=HienThiPhan_Slide%>border: solid 1px #525252;">
                <%--Ô cột dữ liệu--%>
                <div id="<%=BangID%>_TB11_div" style="overflow:scroll;position:relative;width:100px;height:100px;" onscroll="Bang_fnScroll()">
                    <div style="width:<%=Bang_Width_Slide%>px;height:<%=bang.Height+1%>px;">
                        <table id="<%=BangID%>_TB11" class="gridBang"><%
                            for (int i = 0; i < bang.Viewport_N; i++)
                            {
                                String strAttrRow="";
                                if (i >= bang.arrDSMaHang.Count)
                                {
                                    strAttrRow += "display:none;";
                                }
                                if (strAttrRow != "")
                                {
                                    strAttrRow = String.Format(" style='{0}'", strAttrRow);
                                }
                                %><tr<%=strAttrRow%>><%
                                for (int j = bang.nC_Fixed; j < bang.nC_Fixed + bang.nC_Slide; j++)
                                {
                                    String strAttr = "";
                                    if (i == 0)
                                    {
                                        strAttr += String.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", bang.arrWidth[j]);
                                    }
                                    if (bang.arrAlign[j] != "right")
                                    {
                                        strAttr += String.Format("text-align:{0};", bang.arrAlign[j]);
                                    }
                                    if (bang.arrHienThiCot[j] == false)
                                    {
                                        strAttr += "display:none;";
                                    }
                                    if (strAttr != "")
                                    {
                                        strAttr = String.Format(" style='{0}'", strAttr);
                                    }
                                    %><td<%=strAttr%>></td><%
                                }
                                %></tr><%
                            }%>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>