using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;


public class CImage
{
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

    public static void SaveImage(System.Drawing.Image originalImg,
                                 String Path, String FileName,
                                 int Width, int Height)
    {
        CreateDirectory(Path);
        SaveFile(originalImg, Path + "/" + FileName, Width, Height);
    }

    private static void SaveFile(System.Drawing.Image originalImg, String FileName, int Lx, int Ly)
    {
        int newWidth, newHeight, x0, y0;

        if (Lx == -1) Lx = originalImg.Width;
        if (Ly == -1) Ly = originalImg.Height;
        double sX = (double)(originalImg.Width) / Lx ;
        double sY = (double)(originalImg.Height) / Ly;
        if (sX > sY)
        {
            newWidth = Lx;
            newHeight = (int)(originalImg.Height / sX);
            x0 = 0;
            y0 = (Ly - newHeight) / 2;
        }
        else
        {
            newHeight = Ly;
            newWidth = (int)(originalImg.Width / sY);
            x0 = (Lx - newWidth) / 2;
            y0 = 0;
        }

        Bitmap img = new Bitmap(Lx, Ly);

        //Create a graphics object
        Graphics gr_dest = Graphics.FromImage(img);

        // just in case it's a transparent Gif force the bg to white
        SolidBrush sb = new SolidBrush(System.Drawing.Color.White);
        gr_dest.FillRectangle(sb, 0, 0, Lx, Ly);

        //Re-draw the image to the specified height and width
        gr_dest.DrawImage(originalImg, x0, y0, newWidth, newHeight);

        img.Save(FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
    }
}

public class UploadServer : System.Web.UI.Page, IHttpHandler
{
    override public void ProcessRequest(HttpContext context)
    {
        if (context.Request.Files.Count > 0)
        {
            String ImageServerPath = context.Request.PhysicalApplicationPath;
            object Path = context.Request.QueryString["Path"];

            if (Path != null && Path.ToString().IndexOf(":") < 0)
            {
                Path = ImageServerPath + Path;
            }

            object TypeFile = context.Request.QueryString["Type"];
            object ThumsPath = context.Request.QueryString["imageThumsPath"];
            object Width = context.Request.QueryString["imageWidth"];
            object Height = context.Request.QueryString["imageHeight"];
            object ThumsWidth = context.Request.QueryString["imageThumsWidth"];
            object ThumsHeight = context.Request.QueryString["imageThumsHeight"];


            String FileName;

            int i;
            //lặp các file upload
            for (i = 0; i <= context.Request.Files.Count - 1; i++)
            {
                //lấy file hiện tại thứ j
                HttpPostedFile uploadFile = context.Request.Files[i];
                //nếu có 1 file đã được up lên server
                if (uploadFile.ContentLength > 0)
                {
                    if (TypeFile == null)
                    {
                        CImage.CreateDirectory((string)Path);
                        FileName = context.Request.Files.AllKeys[i];
                        uploadFile.SaveAs(Path + "/" + FileName);
                    }
                    else
                    {
                        if (ThumsPath != null && ThumsPath.ToString().IndexOf(":") < 0)
                        {
                            ThumsPath = ImageServerPath + ThumsPath;
                        }
                        if (Width == null) Width = -1;
                        if (Height == null) Height = -1;
                        if (ThumsWidth == null) ThumsWidth = -1;
                        if (ThumsHeight == null) ThumsHeight = -1;
                        System.Drawing.Image originalImg = System.Drawing.Image.FromStream(uploadFile.InputStream);
                        FileName = context.Request.Files.AllKeys[i];
                        if (Path != null)
                        {
                            CImage.SaveImage(originalImg, (string)Path, (string)FileName, int.Parse(Width.ToString()), int.Parse(Height.ToString()));
                        }
                        if (ThumsPath != null)
                        {
                            CImage.SaveImage(originalImg, (string)ThumsPath, (string)FileName, int.Parse(ThumsWidth.ToString()), int.Parse(ThumsHeight.ToString()));
                        }
                    }
                }
            }
        }
    }
}