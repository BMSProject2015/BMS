using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanChiNganSachQuocPhong_PhanCapNganSachDamBaoController : Controller
    {
        //
        // GET: /rptDuToanChiNganSachQuocPhong_PhanCapNganSachDamBao/

        public ActionResult Index()
        {
            return View();
        }
        public DataTable DT_ChiNSQL_PhanCapNganSachDamBao(String NamLamViec, String NG)
        {
            DataTable dt = null;
            String SQL = String.Format(@"SELECT NS_DonVi.sTen,sM,sTM,sTTM,sNG,rTongSo,ChiTiet.sMoTa
                                       FROM ((SELECT iID_MaDonVi,sM,sTM,sTTM,sNG,rTongSo,sMoTa FROM DT_ChungTuChiTiet 
                                       WHERE sLNS=@sLNS AND sL=@sL AND sK=@sK AND iNamLamViec=@NamLamViec) ChiTiet
                                       INNER JOIN NS_DonVi ON NS_DonVi.iID_MaDonVi=ChiTiet.iID_MaDonVi )
                                        ORDER BY NS_DonVi.sTen");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", "1020800");
            cmd.Parameters.AddWithValue("@sL", "460");
            cmd.Parameters.AddWithValue("@sK", "468");
            cmd.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
