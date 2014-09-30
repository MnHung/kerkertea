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
            //if (Request.Params.AllKeys.Contains("photo"))

            byte[] imageBytes = Request.BinaryRead(Request.TotalBytes);
            {
                string _pageID = "837621709595360";
                string accessToken = "CAAKagaljuDIBAOyEnKelOTgJFIDFT3U6Eg9aa8Q7uhlRHh0MQg0Hz3ZANSBB7T1q4ZAucq0apDUNXewIP91kk546UYYmr2Iz49ZCjko1r6e9o9H2dVbmWvaDQxCvt5rWpOooRc8dTrbeMroFCJUu6hGUmwtqqqaEzo8YAGQLvS6guzGk25MRIU3F0tBxB8ZD";

                FacebookClient fb = new FacebookClient(accessToken);

                // 上傳照片
                //string imageUrl = Request["photo"].ToString();
                //imageBytes = Convert.FromBase64String(imageUrl.Substring("data:image/jpeg;base64,".Length));
                //imageBytes = Convert.FromBase64String(imageUrl.Substring("data:image/png;base64,".Length));

                FacebookMediaObject media = new FacebookMediaObject();
                
                media.ContentType = "image/jpeg";
                media.FileName = "image.jpg";

                media.SetValue(imageBytes);

                Dictionary<string, object> upload = new Dictionary<string, object>();
                upload.Add("name", "今天我來到六堆客家文化園區，參觀了新茶特展，還體驗了品茶的奇幻之地！你趕快也ㄧ起來看看吧！ <center></center> <center></center> https://www.facebook.com/liuduihakkatea");
                upload.Add("message", "");
                upload.Add("access_token", accessToken);
                upload.Add("@file.jpg", media);

                try
                {
                    fb.Post(_pageID + "/photos", upload);
                }
                catch (Exception ex)
                {
                    Response.Write("false, exception: " + ex.Message);
                }
            }
            Response.Write("true");
        }
    }
}