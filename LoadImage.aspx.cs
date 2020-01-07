using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using MaxPRM.MemberFramework;
using MaxPRM.Framework;
using System.IO;

public partial class LoadImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //HttpCachePolicy c = Response.Cache;
        //c.SetCacheability(HttpCacheability.Public);
        //c.SetMaxAge(new TimeSpan(1, 0, 0)); // Cache is 1 hour
        //Response.Clear();
        string[] strurl = HttpContext.Current.Request.Url.ToString().Split('?');
        try
        {
            if (strurl.Length > 1 && (strurl[1].Contains("/") || strurl[1].Contains("\\")))
            { UnauthorizedCall(); }
        }
        catch { }
        string outImg = this.GetImage(this.ImageName);
        if (!String.IsNullOrWhiteSpace(outImg))
        {
            Response.ContentType = this.GetMIME(outImg);
            Response.BinaryWrite(File.ReadAllBytes(outImg));
        }
    }



    private void UnauthorizedCall()
    {
        //divTarget.Visible = false;
        Response.ClearHeaders();
        Response.ClearContent();
        Response.Status = "403 Forbidden";
        Response.StatusCode = 403;
        Response.Write("Unauthorized User");
        Response.StatusDescription = "Unauthorized User";
        Response.Flush();
    }


    private string FolderPath
    {
        get
        {
            try
            {
                return Page.Request.QueryString["fp"].ToString();
            }
            catch { return "uk"; } // unknown
        }
    }

    private string ImageName
    {
        get
        {
            try
            {
                return Page.Request.QueryString["img"].ToString();
            }
            catch { return "NA.JPG"; } // unknown
        }
    }

    private string GetImage(string imageName)
    {
        string fullFilePath = Path.Combine(AppSettings.FolderInfo.Uploads, FolderPath, imageName);

        if (!File.Exists(fullFilePath)) return string.Empty;
        return fullFilePath;

    }

    private string GetMIME(string imageName)
    {

        switch (Path.GetExtension(imageName).ToLower())
        {
            case ".png":
            case "png":
                return "image/png";
            case ".jpg":
            case ".jpeg":
            case "jpg":
            case "jpeg":
                return "image/jpeg";
            case ".gif":
            case "gif":
                return "image/gif";

        }
        return string.Empty;
    }


}