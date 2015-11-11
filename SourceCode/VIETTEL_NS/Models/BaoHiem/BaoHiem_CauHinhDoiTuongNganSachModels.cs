using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class BaoHiem_CauHinhDoiTuongNganSachModels
    {
        public static void ThemChiTiet(String iNamLamViec, String MaND, String IPSua)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            //DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            DataTable dt = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);

            Bang bang = new Bang("BH_CauHinh_DoiTuong_NganSach");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            String sLNS = "1010000";
            DataTable dtMucLucNganSach = NganSach_HamChungModels.DT_MucLucNganSach_sLNS(sLNS);
            int iNamLamViecTruoc=Convert.ToInt16(iNamLamViec)-1;
            Dictionary<String, String> arrGiaTriTimKiem = null;
            
            DataTable dtChiTiet = LayChiTiet(Convert.ToString(iNamLamViecTruoc), arrGiaTriTimKiem);
            for (int j = 0; j < dtChiTiet.Rows.Count; j++)
            {
                //Dien thong tin cua Muc luc ngan sach
                NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtChiTiet.Rows[j], bang.CmdParams.Parameters);
                if (bang.CmdParams.Parameters.IndexOf("@sKyHieu") > 0)
                {
                    bang.CmdParams.Parameters["@sKyHieu"].Value=Convert.ToString(dtChiTiet.Rows[j]["sKyHieu"]);
                }
                else
                {
                    bang.CmdParams.Parameters.AddWithValue("@sKyHieu", Convert.ToString(dtChiTiet.Rows[j]["sKyHieu"]));
                }
                if (bang.CmdParams.Parameters.IndexOf("@sMaTX") > 0)
                {
                    bang.CmdParams.Parameters["@sMaTX"].Value = Convert.ToString(dtChiTiet.Rows[j]["sMaTX"]);
                }
                else
                {
                    bang.CmdParams.Parameters.AddWithValue("@sMaTX", Convert.ToString(dtChiTiet.Rows[j]["sMaTX"]));
                }
                bang.Save();
            }
            dtMucLucNganSach.Dispose();
        }

        public static DataTable LayChiTiet(String iNamLamViec, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);            
            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND NS.{0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }
            }

            String SQL1 = "SELECT * FROM NS_MucLucNganSach WHERE sLNS='1010000'";
            SQL = String.Format(@"SELECT NS.*,iID_MaCauHinhBaoHiem,sMaTX,sKyHieu FROM (
                                (SELECT * FROM NS_MucLucNganSach WHERE sLNS='1010000') NS
                                left JOIN  (
                                SELECT BH_CauHinh_DoiTuong_NganSach.* FROM BH_CauHinh_DoiTuong_NganSach WHERE 
                                {0}
                                ) BH
                                ON BH.iID_MaMucLucNganSach=NS.iID_MaMucLucNganSach
                                )
                                ORDER BY NS.sXauNoiMa",DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}