using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using Image = System.Drawing.Image;

namespace VIETTEL.Report_Controllers.NhanSu
{
    public class rptNhanSu_SoYeuLyLichController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/NhanSu/rptNhanSu_SoYeuLyLich.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/NhanSu/rptNhanSu_SoYeuLyLich.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            String iID_MaCanBo = Convert.ToString(Request.Form[ParentID + "_iID_MaCanBo"]);
            return RedirectToAction("Index", new { iID_MaCanBo = iID_MaCanBo });
        }
        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaCanBo)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String KhenThuong = "";
            DataTable dtKhenThuong = NhanSu_KhenThuong(iID_MaCanBo, "0");
            for (int i = 0; i < dtKhenThuong.Rows.Count;i++ )
            {
                KhenThuong += "+" + dtKhenThuong.Rows[i]["CapPhongTang"].ToString() + "," + dtKhenThuong.Rows[i]["HinhThuc"].ToString() + "(" + dtKhenThuong.Rows[i]["iThang"].ToString() + "--" + dtKhenThuong.Rows[i]["iNam"].ToString() + ")," + dtKhenThuong.Rows[i]["sLyDo"].ToString() + ", ";
            }
            String DanhHieu = "";
            DataTable dtDanhHieu = NhanSu_KhenThuong(iID_MaCanBo, "1");
            for (int i = 0; i < dtDanhHieu.Rows.Count; i++)
            {
                DanhHieu += "+" + dtDanhHieu.Rows[i]["CapPhongTang"].ToString() + "," + dtDanhHieu.Rows[i]["HinhThuc"].ToString() + "(" + dtDanhHieu.Rows[i]["iThang"].ToString() + "--" + dtDanhHieu.Rows[i]["iNam"].ToString() + ")," + dtDanhHieu.Rows[i]["sLyDo"].ToString() + ", ";
            }
            String KyLuat = "";
            DataTable dtKyLuat = NhanSu_KyLuat(iID_MaCanBo);
            for (int i = 0; i < dtKyLuat.Rows.Count; i++)
            {
                KyLuat += "+" + dtKyLuat.Rows[i]["CapKyLuat"].ToString() + "," + dtKyLuat.Rows[i]["HinhThuc"].ToString() + "(" + dtKyLuat.Rows[i]["iThang"].ToString() + "--" + dtKyLuat.Rows[i]["iNam"].ToString() + ")," + dtKyLuat.Rows[i]["sLydoDi"].ToString() + ", ";
            }
            if (KyLuat == "")
            {
                KyLuat = "Không";
            }
            String DiNuocNgoai = "";
            DataTable dtNuocNgoai = NhanSu_DiNuocNgoai(iID_MaCanBo);
            for (int i = 0; i < dtNuocNgoai.Rows.Count; i++)
            {
                DiNuocNgoai += "+"+dtNuocNgoai.Rows[i]["dTuNgay"].ToString() + "--" + dtNuocNgoai.Rows[i]["dDenNgay"].ToString() + ":" + dtNuocNgoai.Rows[i]["TenNuoc"].ToString() + "," + dtNuocNgoai.Rows[i]["sLyDoDi"].ToString() + ", ";
            }

          
            using (FlexCelReport fr = new FlexCelReport())
            {
                String sPathImg =  LoadData(fr, iID_MaCanBo);
                fr.SetValue("KhenThuong", KhenThuong);
                fr.SetValue("DanhHieu", DanhHieu);
                fr.SetValue("KyLuat", KyLuat);
                fr.SetValue("DiNuocNgoai", DiNuocNgoai);
                fr.Run(Result);
                try
                {
                    // lay thong tin anh
                    if (!sPathImg.Equals(""))
                    {
                        int indexPath = sPathImg.LastIndexOf("../");
                        if (indexPath >= 0)
                        {
                            sPathImg = "~" + sPathImg.Substring(indexPath+2);
                        }
                        sPathImg = Server.MapPath(sPathImg);
                        Image imgAnh = GetImage(sPathImg, 118, 133);
                        if (imgAnh != null)
                        {
                            Result.AddImage(1, 10, imgAnh);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
                return Result;
            }
        }

        /// <summary>
        /// ham resize anh
        /// </summary>
        /// <param name="sPathImg">duong dan anh</param>
        /// <param name="iHeight">Chieu cao</param>
        /// <param name="iWidth">chieu dai</param>
        /// <returns></returns>
        public Image GetImage(String sPathImg, int iHeight, int iWidth)
        {
            Image img = Image.FromFile(sPathImg);
            //create a new Bitmap the size of the new image
            //create a new graphic from the Bitmap
            Bitmap bmp = new Bitmap(iWidth, iHeight);

            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, iWidth, iHeight);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iID_MaCanBo"></param>
        private string LoadData(FlexCelReport fr, String iID_MaCanBo)
        {
            string sPathImg = "";
            DataTable data = NhanSu_SoYeuLyLich(iID_MaCanBo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            if(data != null && data.Rows.Count >0)
            {
                try
                {
                    sPathImg = Convert.ToString(data.Rows[0]["sAnh"]);
                }
                catch (Exception)
                {
                    
                }
            }
            DataTable DaoTao = NhanSu_DaoTao(iID_MaCanBo);
            DaoTao.TableName = "DaoTao";
            fr.AddTable("DaoTao", DaoTao);
           
            DataTable CongTac = NhanSu_CongTac(iID_MaCanBo);
            CongTac.TableName = "CongTac";
            fr.AddTable("CongTac", CongTac);
          
            data.Dispose();
            DaoTao.Dispose();           
            CongTac.Dispose();
            return sPathImg;
        }

        /// <summary>
        /// ghet image
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        private byte[] GetByteArrayImg(String strFileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.Open);
            // initialise the binary reader from file streamobject
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            // define the byte array of filelength

            byte[] imgbyte = new byte[fs.Length + 1];
            // read the bytes from the binary reader

            imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
            // add the image in bytearray

            br.Close();
            // close the binary reader

            fs.Close();
            // close the file stream
            return imgbyte;
        }


        /// <summary>
        /// Xuất ra file PDF
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String iID_MaCanBo)
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaCanBo);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }
        /// <summary>
        /// Hàm xem file PDF
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaCanBo)
        {
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaCanBo);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "BaoCao");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }
            }
            return null;
        }
        /// <summary>
        /// Xuat ra file Excel
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String iID_MaCanBo )
        {
            String DuongDanFile = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaCanBo);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptNhanSu_SoYeuLyLich.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        /// <summary>
        /// Sơ yếu lý lích cán bộ
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable NhanSu_SoYeuLyLich(String iID_MaCanBo)
        {
            String SQL = String.Format(@"SELECT sAnh,sSoHieuCBCC,iID_MaCanBo,sHoTenThuongDung,sHoTenKhaiSinh,sTen,sTenGoiKhac,sBiDanh,
sTenBacLuong,dNgayHuongNgach,NgaySinh,ThangSinh,NamSinh,TenTonGiao,TenDanToc,sTenTinh,sTenHuyen,sTenXaPhuong,
sChoOHienNay,dNgayThamGiaCM,dNgayTuyenDung,dNgayNhapNgu,dNgayXuatNgu,dNgayTaiNgu,dNgayVaoDang,
dNgayVaoDangCT,dNgayVaoDoan,TrinhDo,HocHam,HocVi,iNamPhongHocVi,iNamPhongHocHam,TrinhDoCHQS,TrinhDoLLCT,TrinhDoCM,sNgoaiNgu,sTiengDanToc,
sChienTruongDaQua,sTinhTrangSucKhoe,sBenhChinh,sLaThuongBinhHang,sCongViecTruocCM,sTinhHinhNhaO,

sHoTenCha,iNamSinhCha,NgheNghiepCha,sHoTenMe,iNamSinhMe,NgheNghiepMe,sQueQuanGD,sChoOGiaDinh,ThanhPhanBanThan,ThanhPhanXuatThan,
iSoNguoiCon,iSoConGai,iSoConTrai,iViTriSoMay,sTinhHinhKinhTeGiaDinh,sTinhHinhChinhTriGiaDinh,
sHoTenChaChong,iNamSinhChaChong,NgheNghiepChaChong,sHoTenMeChong,iNamSinhMeChong,NgheNghiepMeChong,ThanhPhanBanThanGDChong,ThanhPhanXuatThanGDChong,
sTinhHinhKinhTeGiaDinhChong,sTinhHinhChinhTriGiaDinhChong,sQueQuanChong,sChoONhaChong,iSoNguoiConSinhDuoc,iSoTrai,iSoGai,iViTri,

sHoTenVoChong,iNamSinhVoChong,NgheNghiepVoChong,sChoOVoChong,sDanhSachCacCon
 FROM(
SELECT
--Phan 1, 
sAnh,sSoHieuCBCC,iID_MaCanBo,sHoTenThuongDung,sTen,sHoDem+ ' '+sTen as sHoTenKhaiSinh, sTenGoiKhac,sBiDanh,
iID_MaBacLuong,iID_MaNgachLuong,
SUBSTRING( CONVERT(varchar(10),dNgayHuongNgach,105),4,7) as dNgayHuongNgach,
--Phan 2
NgaySinh=SUBSTRING(CONVERT(varchar(10),dNgaySinh,105),1,2),
ThangSinh=SUBSTRING(CONVERT(varchar(10),dNgaySinh,105),4,2),
NamSinh=SUBSTRING(CONVERT(varchar(10),dNgaySinh,105),7,4),

iID_MaTonGiao,iID_MaDanToc,iID_MaTinh,iID_MaHuyen,iID_MaXaPhuong,sDiaChi as sChoOHienNay,
CONVERT(varchar(10),dNgayThamGiaCM,103)as dNgayThamGiaCM,CONVERT(varchar(10),dNgayTuyenDung,103) as dNgayTuyenDung,
CONVERT(varchar(10),dNgayNhapNgu,103) as dNgayNhapNgu,CONVERT(varchar(10),dNgayXuatNgu,103) as dNgayXuatNgu,
CONVERT(varchar(10),dNgayTaiNgu,103) as dNgayTaiNgu,CONVERT(varchar(10),dNgayVaoDang,103) as dNgayVaoDang,
CONVERT(varchar(10),dNgayChinhThuc,103) as dNgayVaoDangCT,CONVERT(varchar(10),dNgayVaoDoan,103) as dNgayVaoDoan,
--Trinh do van hoa
iID_MaTrinhDoVanHoa,iID_MaHocHam,iID_MaHocVi,iNamPhongHocVi,iNamPhongHocHam,sID_MaTrinhDoChuyenMonCaoNhat,iID_MaTrinhDoLyLuanChinhTri,iID_MaTrinhDoChiHuyQuanSu,sNgoaiNgu,sTiengDanToc,
sChienTruongDaQua,sTinhTrangSucKhoe,sBenhChinh,sLaThuongBinhHang,sCongViecTruocCM,sTinhHinhNhaO,
--II-Tinh hinh kinh te chinh tri gia dinh
sHoTenCha,iNamSinhCha,iID_MaNgheNghiepCha,sHoTenMe,iNamSinhMe,iID_MaNgheNghiepMe,sQueQuanGD,sChoOGiaDinh,
iID_MaThanhPhanBanThan,iID_MaThanhPhanGD,
iSoNguoiCon,iSoConGai,iSoConTrai,iViTriSoMay,sTinhHinhKinhTeGiaDinh,sTinhHinhChinhTriGiaDinh,

sHoTenChaChong,iNamSinhChaChong,iID_MaNgheNghiepChaChong,sHoTenMeChong,iNamSinhMeChong,iID_MaNgheNghiepMeChong,
iID_MaThanhPhanBanThanGiaDinhChong,iID_MaThanhPhanXuatThanGDChong,sTinhHinhKinhTeGiaDinhChong,sTinhHinhChinhTriGiaDinhChong,
sQueQuanChong, sChoONhaChong,iSoNguoiConSinhDuoc,iSoTrai,iSoGai,iViTri,
sHoTenVoChong,iNamSinhVoChong,iID_MaNgheNghiepVoChong,sChoOVoChong,sDanhSachCacCon

FROM CB_CanBo
WHERE 1=1 AND iID_MaCanBo=@iID_MaCanBo AND iTrangThai=1) as CB_CanBo
LEFT JOIN (SELECT * FROM L_DanhMucBacLuong) L_DanhMucBacLuong
ON L_DanhMucBacLuong.iID_MaBacLuong=CB_CanBo.iID_MaBacLuong AND L_DanhMucBacLuong.iID_MaNgachLuong=CB_CanBo.iID_MaNgachLuong
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as TenTonGiao FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_TonGiao')) As TonGiao
ON CB_CanBo.iID_MaTonGiao = TonGiao.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as TenDanToc FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_DanToc')) As DanToc
ON CB_CanBo.iID_MaDanToc = DanToc.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaTinh,sTenTinh  FROM CB_DM_Tinh) As Tinh
ON CB_CanBo.iID_MaTinh = Tinh.iID_MaTinh
LEFT JOIN (SELECT iID_MaTinh,iID_MaHuyen,sTenHuyen  FROM CB_DM_Huyen) As Huyen
ON CB_CanBo.iID_MaTinh = Huyen.iID_MaTinh AND CB_CanBo.iID_MaHuyen=Huyen.iID_MaHuyen
LEFT JOIN (SELECT  iID_MaHuyen,iID_MaXaPhuong,sTenXaPhuong FROM CB_DM_XaPhuong) As XaPhuong
ON CB_CanBo.iID_MaXaPhuong = XaPhuong.iID_MaXaPhuong AND CB_CanBo.iID_MaHuyen=XaPhuong.iID_MaHuyen
LEFT JOIN (SELECT sID_MaTrinhDo,sTen as TrinhDo  FROM CB_TrinhDo) As TrinhDo
ON CB_CanBo.iID_MaTrinhDoVanHoa = TrinhDo.sID_MaTrinhDo
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as HocHam FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HocHam')) As HocHam
ON CB_CanBo.iID_MaHocHam = HocHam.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as HocVi FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HocVi')) As HocVi
ON CB_CanBo.iID_MaHocVi = HocVi.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as TrinhDoCHQS FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_TrinhDoChiHuyQuanSu')) As TrinhDoCHQS
ON CB_CanBo.iID_MaTrinhDoChiHuyQuanSu = TrinhDoCHQS.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as TrinhDoLLCT FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_TrinhDoLyLuanChinhTri')) As TrinhDoLLCT
ON CB_CanBo.iID_MaTrinhDoLyLuanChinhTri = TrinhDoLLCT.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as TrinhDoCM FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_TrinhDoChuyenMon')) As TrinhDoCM
ON CB_CanBo.sID_MaTrinhDoChuyenMonCaoNhat = TrinhDoCM.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as NgheNghiepCha FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_NgheNghiep')) As NgheNghiepCha
ON CB_CanBo.iID_MaNgheNghiepCha = NgheNghiepCha.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as NgheNghiepMe FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_NgheNghiep')) As NgheNghiepMe
ON CB_CanBo.iID_MaNgheNghiepMe = NgheNghiepMe.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as ThanhPhanBanThan FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_ThanhPhanBanThan')) As ThanhPhanBanThan
ON CB_CanBo.iID_MaThanhPhanBanThan = ThanhPhanBanThan.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as ThanhPhanXuatThan FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_ThanhPhanBanThan')) As ThanhPhanXuatThan
ON CB_CanBo.iID_MaThanhPhanGD = ThanhPhanXuatThan.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as NgheNghiepChaChong FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_NgheNghiep')) As NgheNghiepChaChong
ON CB_CanBo.iID_MaNgheNghiepChaChong = NgheNghiepChaChong.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as NgheNghiepMeChong FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_NgheNghiep')) As NgheNghiepMeChong
ON CB_CanBo.iID_MaNgheNghiepMeChong = NgheNghiepChaChong.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as ThanhPhanBanThanGDChong FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_ThanhPhanBanThan')) As ThanhPhanBanThanGDChong
ON CB_CanBo.iID_MaThanhPhanBanThanGiaDinhChong = ThanhPhanBanThanGDChong.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as ThanhPhanXuatThanGDChong FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_ThanhPhanBanThan')) As ThanhPhanXuatThanGDChong
ON CB_CanBo.iID_MaThanhPhanXuatThanGDChong = ThanhPhanXuatThanGDChong.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as NgheNghiepVoChong FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_NgheNghiep')) As NgheNghiepVoChong
ON CB_CanBo.iID_MaNgheNghiepVoChong = NgheNghiepVoChong.iID_MaDanhMuc
");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Quá trình đào tạo của cán bộ
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public DataTable NhanSu_DaoTao(String iID_MaCanBo)
        {
            String SQL = String.Format(@"SELECT iID_MaCanBo,sNoiDaoTao,ChuyenNganh,HinhThucDaoTao,dTuNgay,dDenNgay,VanBang,KetQuaDaoTao,NoiDungDaoTao 
FROM(
SELECT Top 10  iID_MaCanBo,sNoiDaoTao,iID_MaChuyenNganh,iID_MaHinhThucDaoTao,YEAR(dTuNgay) AS dTuNgay,YEAR(dDenNgay) as dDenNgay,iID_MaVanBang,iID_MaKetQuaDaoTao,iID_MaNoiDungDaoTao
FROM CB_QuaTrinhDaoTao
WHERE 1=1 AND iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo ORDER BY dDenNgay DESC)  AS CB_QuaTrinhDaoTao
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as ChuyenNganh FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_ChuyenNganh')) As ChuyenNganh
ON CB_QuaTrinhDaoTao.iID_MaChuyenNganh = ChuyenNganh.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as HinhThucDaoTao FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HinhThucDaoTao')) As HinhThucDaoTao
ON CB_QuaTrinhDaoTao.iID_MaHinhThucDaoTao = HinhThucDaoTao.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as VanBang FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HinhThucDaoTao')) As VanBang
ON CB_QuaTrinhDaoTao.iID_MaVanBang = VanBang.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as KetQuaDaoTao FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HinhThucDaoTao')) As KetQuaDaoTao
ON CB_QuaTrinhDaoTao.iID_MaKetQuaDaoTao = KetQuaDaoTao.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as NoiDungDaoTao FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HinhThucDaoTao')) As NoiDungDaoTao
ON CB_QuaTrinhDaoTao.iID_MaNoiDungDaoTao = NoiDungDaoTao.iID_MaDanhMuc");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int a = dt.Rows.Count;
            if (a < 5)
            {
                for (int i = 0; i < (5 - a);i++)
                {
                    DataRow r=dt.NewRow();
                    dt.Rows.InsertAt(r, a + 1);
                }
            }
            return dt;
        }
        /// <summary>
        /// Quá trình đi nước ngoài
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public DataTable NhanSu_DiNuocNgoai(String iID_MaCanBo)
        {
            String SQL = String.Format(@"SELECT iID_MaCanBo,dTungay,dDenNgay,sLyDoDi,TenNuoc FROM(
                                        SELECT iID_MaCanBo,YEAR(dTuNgay) as dTungay, YEAR(dDenNgay) as dDenNgay,sLyDoDi,iID_MaNuoc
                                        FROM CB_DiNuocNgoai
                                        WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo) CB_DiNuocNgoai
                                        LEFT JOIN (SELECT iID_MaDanhMuc,sTen as TenNuoc FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_QuocGia')) As TenNuoc
                                        ON CB_DiNuocNgoai.iID_MaNuoc = TenNuoc.iID_MaDanhMuc");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public DataTable NhanSu_CongTac(String iID_MaCanBo)
        {
            String SQL = String.Format(@"SELECT   sTenBacLuong,sDonViCongTac,DoanThe,SUBSTRING (CONVERT(varchar(10),dTuNgay,103),4,7) as dTuNgay,SUBSTRING (CONVERT(varchar(10),dDenNgay,103),4,7) as dDenNgay
FROM(
SELECT TOP 10 * FROM CB_QuaTrinhCongTac WHERE iID_MaCanbo=@iID_MaCanBo ORDER BY dDenNgay DESC) CB_QuaTrinhCongTac
LEFT JOIN (SELECT * FROM L_DanhMucBacLuong) L_DanhMucBacLuong
ON L_DanhMucBacLuong.iID_MaBacLuong=CB_QuaTrinhCongTac.iID_MaBacLuong AND L_DanhMucBacLuong.iID_MaNgachLuong=CB_QuaTrinhCongTac.iID_MaNgachLuong
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as DoanThe FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_ChucVu')) As DoanThe
ON CB_QuaTrinhCongTac.iID_MaChucVuDoanThe = DoanThe.iID_MaDanhMuc");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int a = dt.Rows.Count;
            if (a < 13)
            {
                for (int i = 0; i < (13 - a); i++)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, a + 1);
                }
            }
            return dt;
        }
        /// <summary>
        /// danh sách khen thưởng
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public DataTable NhanSu_KhenThuong(String iID_MaCanBo,String Loai)
        {
            String SQL = String.Format(@"SELECT CapPhongTang,HinhThuc,sLyDo,iThang,iNam FROM (SELECT  TOP 5 iID_MaCapPhongTang,iID_MaHinhThuc,sLyDo,iThang,iNam FROM CB_KhenThuong WHERE iID_MaCanBo=@iID_MaCanBo AND iLoai=@Loai ORDER BY iThang DESC,iNam DESC) as CB_KhenThuong
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as CapPhongTang FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_CapKhenThuongKyLuat')) As CapPhongTang
                                        ON CB_KhenThuong.iID_MaCapPhongTang = CapPhongTang.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as HinhThuc FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HinhThucKhenThuongKyLuat')) As HinhThuc
                                        ON CB_KhenThuong.iID_MaHinhThuc = HinhThuc.iID_MaDanhMuc");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.Parameters.AddWithValue("@Loai", Loai);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách kỷ luật
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public DataTable NhanSu_KyLuat(String iID_MaCanBo)
        {
            String SQL = String.Format(@"SELECT CapKyLuat,HinhThuc,sLyDo,iThang,iNam FROM (SELECT TOP 5 iID_MaCapKyLuat,iID_MaHinhThuc,sLyDo,iThang,iNam FROM CB_KyLuat WHERE iID_MaCanBo=@iID_MaCanBo ORDER BY iThang DESC,iNam DESC) as CB_KyLuat

LEFT JOIN (SELECT iID_MaDanhMuc,sTen as CapKyLuat FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_CapKhenThuongKyLuat')) As CapKyLuat
                                        ON CB_KyLuat.iID_MaCapKyLuat = CapKyLuat.iID_MaDanhMuc
LEFT JOIN (SELECT iID_MaDanhMuc,sTen as HinhThuc FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang='CanBo_HinhThucKhenThuongKyLuat')) As HinhThuc
                                        ON CB_KyLuat.iID_MaHinhThuc = HinhThuc.iID_MaDanhMuc");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}
