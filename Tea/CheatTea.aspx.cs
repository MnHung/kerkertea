using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using System.Net;
using System.IO;

namespace Tea
{
    public partial class CheatTea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params.AllKeys.Contains("photo"))
            {
                string _pageID = "1474519026152103";
                string accessToken = "CAAJ1UqBRmVEBAKMjBEkRYaPwHV7A19cTdXZCSjiDsrriZBWTSZCSVaJQBeuJ060tqZBZBpZCZA6hr09b3P4hhoR09ndftjcMxM1AuZCgpbSyJByyvGq3x9s5IF9Ut10ZAQ9KNG68zQKZB85QLZACvoasxDLSOZCRYw0xGeZBTuufL8tAg2EvZAWRZBTve7FsrH0S4ZC4iHIZD";

                FacebookClient fb = new FacebookClient(accessToken);

                // 上傳照片
                string imageUrl = Request["photo"].ToString();
                byte[] imageBytes = Convert.FromBase64String(imageUrl.Substring("data:image/jpeg;base64,".Length));

                FacebookMediaObject media = new FacebookMediaObject();
                media.ContentType = "image/jpeg";
                media.FileName = "image.jpg";// ImagePath;
                media.SetValue(imageBytes);

                Dictionary<string, object> upload = new Dictionary<string, object>();
                upload.Add("name", "照片名稱");
                upload.Add("message", "照片描述");
                //upload.Add("no_story", "1"); // 是否要發佈訊息
                upload.Add("access_token", accessToken);
                upload.Add("@file.jpg", media);

                fb.Post(_pageID + "/photos", upload);
            }
        }
    }
}