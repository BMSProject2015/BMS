using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;


public class CImages
{
    public static int FixWidthThums = 200;
    public static int FixHeightThums = 200;
    public static Boolean CreateDirectory(String Dir)
    {
        if (Directory.Exists(Dir) == false)
        {
            try
            {
                Directory.CreateDirectory(Dir);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return false;
    }

    public static void SaveImage(System.Drawing.Image originalImg, String ImageServerPath,
                                String Path, String PathThums, String PathRealThums,
                                ref String FileName, ref String FileNameThums, ref String FileNameRealThums,
                                ref int Width, ref int Height,
                                int WidthThums, int HeightThums,
                                ref int WidthRealThums, ref int HeightRealThums)
    {
        DateTime TG = DateTime.Now;
        String subPath = TG.ToString("yyyy/MM/dd");
        String subName = TG.ToString("HHmmssfff") + ".jpg";
        String newPath = String.Format("{0}/{1}", Path, subPath);
        String newPathThums = String.Format("{0}/{1}", PathThums, subPath);
        String newRealPathThums = String.Format("{0}/{1}", PathRealThums, subPath);
        String newFN = String.Format("{0}/{1}", newPath, subName);
        String newFNThums = String.Format("{0}/{1}", newPathThums, subName);
        String newFNRealThums = String.Format("{0}/{1}", newRealPathThums, subName);
        CreateDirectory(ImageServerPath + newPath);
        CreateDirectory(ImageServerPath + newPathThums);
        CreateDirectory(ImageServerPath + newRealPathThums);
        Width = -1;
        Height = -1;
        if (WidthThums == -1)
        {
            WidthThums = FixWidthThums;
            HeightThums = FixHeightThums;
        }
        if (WidthRealThums == -1)
        {
            WidthRealThums = FixWidthThums;
            HeightRealThums = FixHeightThums;
        }
        SaveFile(originalImg, ImageServerPath, newFN, ref Width, ref Height);
        SaveFileThums(originalImg, ImageServerPath, newFNThums, WidthThums, HeightThums);
        SaveFile(originalImg, ImageServerPath, newFNRealThums, ref WidthRealThums, ref HeightRealThums);
        FileName = newFN;
        FileNameThums = newFNThums;
        FileNameRealThums = newFNRealThums;
    }

    public static double GetScale(int Width, int Height, int realWidth, int realHeight)
    {
        if (Width > 0 && Height > 0)
        {
            double sX = (double)(realWidth) / Width;
            double sY = (double)(realHeight) / Height;
            return (sX > sY) ? sX : sY;
        }
        return -1;
    }

    private static void SaveFile(System.Drawing.Image originalImg, String ImageServerPath, String FileName, ref int Width, ref int Height)
    {
        double Scale = GetScale(Width, Height, originalImg.Width, originalImg.Height);
        int newWidth, newHeight, x0 = 0, y0 = 0;

        newHeight = originalImg.Height;
        newWidth = originalImg.Width;
        if (Scale > 0)
        {
            newHeight = (int)(originalImg.Height / Scale);
            newWidth = (int)(originalImg.Width / Scale);
        }

        Bitmap img = new Bitmap(newWidth, newHeight);

        //Create a graphics object
        Graphics gr_dest = Graphics.FromImage(img);

        //Re-draw the image to the specified height and width
        gr_dest.DrawImage(originalImg, x0, y0, newWidth, newHeight);

        Width = newWidth;
        Height = newHeight;
        img.Save(ImageServerPath + FileName, ImageFormat.Jpeg);
    }

    public static double GetScale1(int Width, int Height, int realWidth, int realHeight)
    {
        if (Width > 0 && Height > 0)
        {
            double sX = (double)(realWidth) / Width;
            double sY = (double)(realHeight) / Height;
            return (sX < sY) ? sX : sY;
        }
        return -1;
    }

    private static void SaveFileThums(System.Drawing.Image originalImg, String ImageServerPath, String FileName, int Width, int Height)
    {
        double Scale = GetScale1(Width, Height, originalImg.Width, originalImg.Height);
        int newWidth, newHeight, x0 = 0, y0 = 0;

        newHeight = originalImg.Height;
        newWidth = originalImg.Width;
        if (Scale > 0)
        {
            newHeight = (int)(originalImg.Height / Scale);
            newWidth = (int)(originalImg.Width / Scale);
        }

        Bitmap img = new Bitmap(Width, Height);

        x0 = (Width - newWidth) / 2;
        y0 = (Height - newHeight) / 2;
        //Create a graphics object
        Graphics gr_dest = Graphics.FromImage(img);

        //Re-draw the image to the specified height and width
        gr_dest.DrawImage(originalImg, x0, y0, newWidth, newHeight);

        img.Save(ImageServerPath + FileName);
    }
}


//Vidu
//[HttpPost]
//public ActionResult UploadSubmit()
//{
//    //Xác định tài khoản đăng nhập
//    String sUserID;
//    sUserID = Request.Form["Upload_sUserID"];
//    if (String.IsNullOrEmpty(sUserID))
//    {
//        sUserID = HomeModels.GetUserID(User.Identity.Name);
//    }
//    if (sUserID == "")
//    {
//        String sUrl = Url.Action("Index", "Upload");
//        return RedirectToAction("New", "Photographer", new { url = sUrl });
//    }
//    String ImageServerPath = Server.MapPath("~/");
//    Double rLat = Convert.ToDouble(Request.Form["Upload_rLat"]);
//    Double rLng = Convert.ToDouble(Request.Form["Upload_rLng"]);
//    String iID_MaTinh = Request.Form["Upload_iID_MaTinh"];
//    String iID_MaQuan = Request.Form["Upload_iID_MaQuan"];
//    Double iID_MaChuDe = Convert.ToDouble(Request.Form["Upload_iID_MaChuDe"]);
//    Double iID_MaTour = Convert.ToDouble(Request.Form["Upload_iID_MaTour"]);
//    if (iID_MaTour == 0) {
//        iID_MaChuDe = 0;
//    }
//    Boolean bAnhDaiDienQuan, bAnhDaiDienTinh;
//    String strV;
//    strV = Request.Form["Upload_bAnhDaiDienQuan"];
//    if (strV == null || Convert.ToString(strV).ToUpper() == "OFF" || Convert.ToString(strV) == "0" || Convert.ToString(strV).ToUpper() == "FALSE")
//    {
//        bAnhDaiDienQuan = false;
//    }
//    else
//    {
//        bAnhDaiDienQuan = true;
//    }
//    strV = Request.Form["Upload_bAnhDaiDienTinh"];
//    if (strV == null || Convert.ToString(strV).ToUpper() == "OFF" || Convert.ToString(strV) == "0" || Convert.ToString(strV).ToUpper() == "FALSE")
//    {
//        bAnhDaiDienTinh = false;
//    }
//    else
//    {
//        bAnhDaiDienTinh = true;
//    }
//    String Path = "Libraries/Images";
//    String PathThums = "ThumsLibraries/Images";
//    String RealPathThums = "RealThumsLibraries/Images";
//    SqlCommand cmd;
//    for (int i = 0; i < 5; i++)
//    {
//        String ctlFNName = "Upload_File"+i;
//        HttpPostedFileBase hpf = Request.Files[ctlFNName] as HttpPostedFileBase;
//        if (hpf.ContentLength > 0)
//        {
//            String FileName = hpf.FileName;
//            String FileNameThums = "";
//            String FileNameRealThums = "";
//            System.Drawing.Image originalImg = System.Drawing.Image.FromStream(hpf.InputStream);
//            int WidthRealThums = -1, HeightRealThums = -1, WidthThums = CImage.FixWidthThums, HeightThums = CImage.FixHeightThums, Width = -1, Height = -1;
//            CImage.SaveImage(originalImg, ImageServerPath, Path, PathThums, RealPathThums, ref FileName, ref FileNameThums, ref FileNameRealThums, ref Width, ref Height, WidthThums, HeightThums, ref WidthRealThums, ref HeightRealThums);
//            String sTieuDe = Request.Form["Upload_sTieuDe"+i];
//            String iID_MaSlide = Request.Form["Upload_iID_MaSlide"+i];

//            cmd = new SqlCommand();
//            cmd.Parameters.AddWithValue("@iID_MaSlide", iID_MaSlide);
//            cmd.Parameters.AddWithValue("@sAnhDaiDien_Thums", FileNameThums);
//            cmd.Parameters.AddWithValue("@iAnhDaiDien_Thums_Width", WidthThums);
//            cmd.Parameters.AddWithValue("@iAnhDaiDien_Thums_Height", HeightThums);
//            cmd.Parameters.AddWithValue("@sAnhDaiDien_RealThums", FileNameRealThums);
//            cmd.Parameters.AddWithValue("@iAnhDaiDien_RealThums_Width", WidthRealThums);
//            cmd.Parameters.AddWithValue("@iAnhDaiDien_RealThums_Height", HeightRealThums);
//            cmd.Parameters.AddWithValue("@sTieuDe", sTieuDe);
//            cmd.Parameters.AddWithValue("@rLat", rLat);
//            cmd.Parameters.AddWithValue("@rLng", rLng);
//            cmd.Parameters.AddWithValue("@iID_MaTour", iID_MaTour);
//            cmd.Parameters.AddWithValue("@iID_MaChuDe", iID_MaChuDe);
//            cmd.Parameters.AddWithValue("@iID_MaTinh", iID_MaTinh);
//            cmd.Parameters.AddWithValue("@iID_MaQuan", iID_MaQuan);
//            cmd.Parameters.AddWithValue("@sFileAnh", FileName);
//            cmd.Parameters.AddWithValue("@iWidth", Width);
//            cmd.Parameters.AddWithValue("@iHeight", Height);
//            if (HomeModels.KiemTraAdmin(User.Identity.Name))
//            {
//                cmd.Parameters.AddWithValue("@bAnhDaiDienTinh", bAnhDaiDienTinh);
//                cmd.Parameters.AddWithValue("@bAnhDaiDienQuan", bAnhDaiDienQuan);
//            }
//            cmd.Parameters.AddWithValue("@sUserID", sUserID);
//            Connection.InsertRecord("tb_Slide", cmd);
//            cmd.Dispose();
//        }
//    }
//    return RedirectToAction("Index", "Upload");
//}