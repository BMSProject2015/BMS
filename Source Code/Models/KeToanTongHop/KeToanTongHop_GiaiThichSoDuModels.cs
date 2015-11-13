using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
namespace VIETTEL.Models
{
    public class KeToanTongHop_GiaiThichSoDuModels
    {
        public static DataTable Get_dtDanhSachGiaiThichSoDu(int iThang, int iNam, int iLoai, Dictionary<String, String> arrGiaTriTimKiem = null)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iNamLamViec = @iNam AND iThangLamViec = @iThang AND iLoai=@iLoai";
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iLoai", iLoai);

            if (arrGiaTriTimKiem != null && arrGiaTriTimKiem.Count > 0 && String.IsNullOrEmpty(arrGiaTriTimKiem["sLyDo"]) == false)
            {
                DK += String.Format(" AND sLyDo LIKE @sLyDo");
                cmd.Parameters.AddWithValue("@sLyDo", '%' + arrGiaTriTimKiem["sLyDo"] + '%');
            }

            String SQL = String.Format("SELECT * FROM KT_GiaiThichSoDu WHERE {0} ORDER BY dNgayTao ", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_dtCuoiKy(String iThang, String iNam, String iTrangThai)
        {
//            String SQL = @"
//SELECT iID_MaTaiKhoan_No,SUBSTRING(sTenTaiKhoan_No,(CHARINDEX('-',sTenTaiKhoan_No))+1,Len(sTenTaiKhoan_No)) as sTen,
//iID_MaDonVi_No,
//SUBSTRING(sTenDonVi_No,(CHARINDEX('-',sTenDonVi_No))+1,Len(sTenDonVi_No)) as sTenDonVi,
//SUM(rSoTien_No) as rSoTien_No,SUM(rSoTien_Co) as rSoTien_Co,
//rSoTien=CASE WHEN (SUM(rSoTien_No)-SUM(rSoTien_Co)>0) THEN SUM(rSoTien_No)-SUM(rSoTien_Co) ELSE (SUM(rSoTien_No)-SUM(rSoTien_Co))*-1 END
//,iLoai=CASE SubString(iID_MaTaiKhoan_No,1,3)
//      WHEN '311' THEN 1 
//      WHEN '312' THEN 2
//      WHEN '331' THEN 3
//      END 
//FROM(
//SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No, rSoTien_No=SUM(rSoTien),rSoTien_Co=0
//       FROM KT_ChungTuChiTiet
//       WHERE iTrangThai=1  
//             AND (iID_MaTaiKhoan_No LIKE '311%' OR iID_MaTaiKhoan_No LIKE '312%' OR iID_MaTaiKhoan_No LIKE '331%')
//             AND iID_MaDonVi_No <>''
//             AND iID_MaDonVi_No IS NOT NULL
//             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
//             AND iNamLamViec=@iNamLamViec
//             AND iThangCT=@iThang
//       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No
//       
//       UNION 
//       
//        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co, rSoTien_No=0,rSoTien_Co=SUM(rSoTien)
//       FROM KT_ChungTuChiTiet
//       WHERE iTrangThai=1  AND (iID_MaTaiKhoan_Co LIKE '311%' OR iID_MaTaiKhoan_Co LIKE '312%' OR iID_MaTaiKhoan_Co LIKE '331%')
//			 AND iID_MaDonVi_Co <>''
//             AND iID_MaDonVi_Co IS NOT NULL
//             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
//             AND iNamLamViec=@iNamLamViec
//             AND iThangCT=@iThang
//       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co) as a
//       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No";
           
            if (iTrangThai == "0")// da duyet
            {
                String SQL =
                    @"SELECT * FROM (SELECT iID_MaTaiKhoan_No,SUBSTRING(sTenTaiKhoan_No,(CHARINDEX('-',sTenTaiKhoan_No))+1,Len(sTenTaiKhoan_No)) as sTen,
iID_MaDonVi_No,
SUBSTRING(sTenDonVi_No,(CHARINDEX('-',sTenDonVi_No))+1,Len(sTenDonVi_No)) as sTenDonVi,
SUM(rSoTien_No) as rSoTien_No,SUM(rSoTien_Co) as rSoTien_Co,SUM(rSoTien_DuNo) as rSoTien_DuNo, SUM(rSoTien_DuCo) as rSoTien_DuCo,
rSoTien=CASE WHEN (SUM(rSoTien_DuNo)-SUM(rSoTien_DuCo)>=0) THEN SUM(rSoTien_DuNo)-SUM(rSoTien_DuCo) + SUM(rSoTien_No)-SUM(rSoTien_Co) ELSE (SUM(rSoTien_DuNo)-SUM(rSoTien_DuCo) + SUM(rSoTien_No)-SUM(rSoTien_Co))*-1 END
,iLoai=CASE SubString(iID_MaTaiKhoan_No,1,3)
      WHEN '311' THEN 1 
      WHEN '312' THEN 2
      WHEN '331' THEN 3
      END 
FROM(
SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No, rSoTien_No=SUM(rSoTien),rSoTien_Co=0, rSoTien_DuNo=0,rSoTien_DuCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
             AND (iID_MaTaiKhoan_No LIKE '311%' OR iID_MaTaiKhoan_No LIKE '312%' OR iID_MaTaiKhoan_No LIKE '331%')
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT=@iThang
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No
       
       UNION 
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co, rSoTien_No=0,rSoTien_Co=SUM(rSoTien), rSoTien_DuNo=0,rSoTien_DuCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  AND (iID_MaTaiKhoan_Co LIKE '311%' OR iID_MaTaiKhoan_Co LIKE '312%' OR iID_MaTaiKhoan_Co LIKE '331%')
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT=@iThang
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co
       
       Union
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co, rSoTien_No=0,rSoTien_Co=0, rSoTien_DuNo=0,rSoTien_DuCo=SUM(rSoTien)
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  AND (iID_MaTaiKhoan_Co LIKE '311%' OR iID_MaTaiKhoan_Co LIKE '312%' OR iID_MaTaiKhoan_Co LIKE '331%')
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<@iThang
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co
             
       union
       
       SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No, rSoTien_No=0,rSoTien_Co=0, rSoTien_DuNo=SUM(rSoTien),rSoTien_DuCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
             AND (iID_MaTaiKhoan_No LIKE '311%' OR iID_MaTaiKhoan_No LIKE '312%' OR iID_MaTaiKhoan_No LIKE '331%')
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<@iThang
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No
       ) as a
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No) b
        GROUP BY iID_MaTaiKhoan_No,sTen,iID_MaDonVi_No,sTenDonVi,rSoTien_No,rSoTien_Co,rSoTien_DuNo,rSoTien_DuCo,rSoTien,iLoai HAVING rSoTien>0";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                                            LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(
                                                PhanHeModels.iID_MaPhanHeKeToanTongHop));
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
            else
            {
                String SQL =
                    @"SELECT * FROM (SELECT iID_MaTaiKhoan_No,SUBSTRING(sTenTaiKhoan_No,(CHARINDEX('-',sTenTaiKhoan_No))+1,Len(sTenTaiKhoan_No)) as sTen,
iID_MaDonVi_No,
SUBSTRING(sTenDonVi_No,(CHARINDEX('-',sTenDonVi_No))+1,Len(sTenDonVi_No)) as sTenDonVi,
SUM(rSoTien_No) as rSoTien_No,SUM(rSoTien_Co) as rSoTien_Co,SUM(rSoTien_DuNo) as rSoTien_DuNo, SUM(rSoTien_DuCo) as rSoTien_DuCo,
rSoTien=CASE WHEN (SUM(rSoTien_DuNo)-SUM(rSoTien_DuCo)>=0) THEN SUM(rSoTien_DuNo)-SUM(rSoTien_DuCo) + SUM(rSoTien_No)-SUM(rSoTien_Co) ELSE (SUM(rSoTien_DuNo)-SUM(rSoTien_DuCo) + SUM(rSoTien_No)-SUM(rSoTien_Co))*-1 END
,iLoai=CASE SubString(iID_MaTaiKhoan_No,1,3)
      WHEN '311' THEN 1 
      WHEN '312' THEN 2
      WHEN '331' THEN 3
      END 
FROM(
SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No, rSoTien_No=SUM(rSoTien),rSoTien_Co=0, rSoTien_DuNo=0,rSoTien_DuCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
             AND (iID_MaTaiKhoan_No LIKE '311%' OR iID_MaTaiKhoan_No LIKE '312%' OR iID_MaTaiKhoan_No LIKE '331%')
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL            
             AND iNamLamViec=@iNamLamViec
             AND iThangCT=@iThang
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No
       
       UNION 
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co, rSoTien_No=0,rSoTien_Co=SUM(rSoTien), rSoTien_DuNo=0,rSoTien_DuCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  AND (iID_MaTaiKhoan_Co LIKE '311%' OR iID_MaTaiKhoan_Co LIKE '312%' OR iID_MaTaiKhoan_Co LIKE '331%')
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL           
             AND iNamLamViec=@iNamLamViec
             AND iThangCT=@iThang
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co
       
       Union
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co, rSoTien_No=0,rSoTien_Co=0, rSoTien_DuNo=0,rSoTien_DuCo=SUM(rSoTien)
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  AND (iID_MaTaiKhoan_Co LIKE '311%' OR iID_MaTaiKhoan_Co LIKE '312%' OR iID_MaTaiKhoan_Co LIKE '331%')
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL           
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<@iThang
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co
             
       union
       
       SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No, rSoTien_No=0,rSoTien_Co=0, rSoTien_DuNo=SUM(rSoTien),rSoTien_DuCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
             AND (iID_MaTaiKhoan_No LIKE '311%' OR iID_MaTaiKhoan_No LIKE '312%' OR iID_MaTaiKhoan_No LIKE '331%')
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL           
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<@iThang
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No
       ) as a
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No) b
        GROUP BY iID_MaTaiKhoan_No,sTen,iID_MaDonVi_No,sTenDonVi,rSoTien_No,rSoTien_Co,rSoTien_DuNo,rSoTien_DuCo,rSoTien,iLoai HAVING rSoTien>0";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iThang", iThang);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
        }
    }
}