using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using Facebook;
using System.Net;
using System.IO;

namespace Tea
{
    public partial class Tea : System.Web.UI.Page
    {
        private string _pageID = "1474519026152103";
        private string _fileName = "Koala.jpg";

        private string _appID = "___691947690891601";
        private string _appSecret = "___2347601e744371de306e01be1edecb63";
        private string _scope = "public_profile, manage_notifications, manage_pages, publish_actions, user_activities, user_photos, user_about_me";
        private string _redirect_url = "http://localhost:51601/Tea";

        public void printMessage(string s)
        {
            TextBox1.Text += s + " \n<br />";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            return;
            String accessToken = null;
            String pageAccessToken = null;
            FacebookClient fb = new FacebookClient();
            if (Session["accessToken"] != null)
            {
                accessToken = Session["accessToken"].ToString();
                //fb = new FacebookClient(accessToken);
            }
            else if (Request.Params.AllKeys.Contains("code"))
            {
                try
                {
                    #region 取得 Access Token
                    dynamic result = fb.Get("oauth/access_token", new
                    {
                        client_id = _appID,
                        client_secret = _appSecret,
                        redirect_uri = _redirect_url,
                        code = Request["code"].ToString()
                    });

                    accessToken = result.access_token;
                    Session["accessToken"] = accessToken;
                    printMessage("# access token type : " + accessToken.GetType());
                    printMessage("# access token : " + result.access_token);
                    #endregion
                    PanelUpload.Visible = true;

                }
                catch (Exception ex)
                {
                    Response.Write("發生例外" + ex.Message);
                    Response.End();
                    return;
                   // Response.Redirect(this._redirect_url);
                }
            }

            if (accessToken != null)
            {
                loginState.Value = "connected";
                #region 列出所有可管理的專頁
                return;
                try
                {
                    fb.AccessToken = accessToken;
                    IDictionary<string, object> dic = (IDictionary<string, object>)fb.Get("/me/accounts");

                    // 列出所有我管理的粉絲專頁
                    IList<object> dicy = (IList<object>)dic["data"];
                    IDictionary<string, object> page = null;

                    ddlPageID.Items.Clear();
                    printMessage("My fan page counts, " + dicy.Count);
                    for (int i = 0; i <= dicy.Count - 1; i++)
                    {
                        page = (IDictionary<string, object>)dicy[i];
                        ddlPageID.Items.Add(new ListItem(page["name"].ToString(), page["id"].ToString()));
                        printMessage(page["name"].ToString());

                        if (page["id"].ToString() == _pageID)
                        {
                            pageAccessToken = page["access_token"].ToString();
                            Session["PageAccessToken"] = page["access_token"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    printMessage("get fan page exception, " + ex.Message);
                }
                #endregion

                #region 收到照片、傳給 fb
                if (Request.Params.AllKeys.Contains("photo"))
                {
                    //string imageUrl = Request["photo"].ToString();
                    //byte[] bytes = Convert.FromBase64String(imageUrl.Substring("data:image/webp;base64,".Length));

                    //fb = PreparePhoto();

                    //_fileName = "Koala.jpg";
                    //string ImagePath = Server.MapPath("~/img/" + _fileName);
                   
                    //// 上傳照片
                    //JsonObject result = UploadPhoto(fb, ImagePath, bytes, "image/png");

                    fb = PreparePhoto();

                    //_fileName = "Koala.jpg";
                    string ImagePath = Server.MapPath("~/img/" + _fileName);
                    string imageUrl = Request["photo"].ToString();
                    // data:image/jpeg;base64,
                    byte[] imageBytes = Convert.FromBase64String(imageUrl.Substring("data:image/jpeg;base64,".Length));

                    MemoryStream ms = new MemoryStream(imageBytes, 0,
                      imageBytes.Length);

                    // Convert byte[] to Image
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                    //image.Save("haha.jpg");
                    // 上傳照片
                    JsonObject result = UploadPhoto(fb, ImagePath, imageBytes, null);
                }
                #endregion
            }
            else
            {
                loginState.Value = "";
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Facebook.FacebookClient fb = PreparePhoto();

            //_fileName = "Koala.jpg";
            string ImagePath = Server.MapPath("~/img/" + _fileName);
            byte[] filebytes = System.IO.File.ReadAllBytes(ImagePath);

            // 上傳照片
            JsonObject result = UploadPhoto(fb, ImagePath, filebytes, null);

            // 設為封面
            //SetCoverPhoto(fb, result["id"].ToString());

            //string txt = string.Format("<p>上傳成功: <a href='https://www.facebook.com/photo.php?fbid={0}' target='_blank'>點擊這裡查看照片</a>，並<a href='https://www.facebook.com/{1}' target='_blank'>看看</a>是否已經設為封面了", result["id"].ToString(), _pageID);
            //Response.Write(txt);
        }

        private FacebookClient PreparePhoto()
        {
            // 選擇的粉絲專頁
            //_pageID = ddlPageID.SelectedValue;
            //_pageID = "1474910372779635";
            // 照片檔名
            //_fileName = tbFileName.Text;
            
            printMessage("Image name :" + _fileName);

            Facebook.FacebookClient fb = new Facebook.FacebookClient(Session["accessToken"].ToString());

            printMessage("# access token again :" + Session["accessToken"].ToString());

            IDictionary<string, object> dic = (IDictionary<string, object>)fb.Get("/me/accounts");

            // 列出所有我管理的粉絲專頁
            IList<object> dicy = (IList<object>)dic["data"];
            IDictionary<string, object> page = null;

            for (int i = 0; i <= dicy.Count - 1; i++)
            {
                page = (IDictionary<string, object>)dicy[i];

                // 如果是這次要上傳的粉絲專頁 ID
                if (page["id"].ToString() == _pageID)
                {
                    // 取得粉絲專頁的 access_token，才能針對粉絲專頁做 graph api 動作
                    //CAAJ1UqBRmVEBALIU6CtOWPWcRtmcpPnY82NHMxndEAHwg72LDdgqD47u0jrsBH92fDlxSFa5VkJHZAxwWNfvyrMkPJ4docUM3Af98QnnefVYeMWu7TApray7feamYUDhk9nRadCLkIbme4IDfA6NAj8iKlvFZC2LIEujRYaZAuX12LkA2fm
                    string pageAccessToken = page["access_token"].ToString();
                    pageAccessToken = "CAAJ1UqBRmVEBALIU6CtOWPWcRtmcpPnY82NHMxndEAHwg72LDdgqD47u0jrsBH92fDlxSFa5VkJHZAxwWNfvyrMkPJ4docUM3Af98QnnefVYeMWu7TApray7feamYUDhk9nRadCLkIbme4IDfA6NAj8iKlvFZC2LIEujRYaZAuX12LkA2fm";
                    fb.AccessToken = pageAccessToken;
                    Session["PageAccessToken"] = pageAccessToken;
                    break;
                }
            }

            if (Session["PageAccessToken"] == null || string.IsNullOrEmpty(Session["PageAccessToken"].ToString()))
            {
                Response.Write("無法管理此粉絲專頁: " + _pageID);
                Response.End();
            }
            return fb;
        }

        protected void btnFBConnect_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetFacebookLoginUrl().AbsoluteUri);
        }

        /// <summary>
        /// 上傳照片方法
        /// </summary>
        /// <param name="fbApp"></param>
        /// <param name="ImagePath"></param>
        /// <returns></returns>
        private JsonObject UploadPhoto(Facebook.FacebookClient fbApp, string ImagePath, byte[] filebytes, string ContentType)
        {
            Facebook.FacebookMediaObject media = new Facebook.FacebookMediaObject();
            if (ContentType == null)
                ContentType = "image/jpeg";
            media.ContentType = ContentType;
            media.FileName = ImagePath;

            media.SetValue(filebytes);
            Dictionary<string, object> upload = new Dictionary<string, object>();
            upload.Add("name", "照片名稱");
            upload.Add("message", "照片描述");
            upload.Add("no_story", "1"); // 是否要發佈訊息
            upload.Add("access_token", Session["PageAccessToken"]);
            upload.Add("@file.jpg", media);

            return fbApp.Post(_pageID + "/photos", upload) as JsonObject;
        }

        private JsonObject UploadPhoto(FacebookClient fbApp, byte[] bytes, string pageAccessToken)
        {
            FacebookMediaObject media = new FacebookMediaObject();
            media.ContentType = "image/png";
            media.FileName = "test.png";
            media.SetValue(bytes);

            Dictionary<string, object> upload = new Dictionary<string, object>();
            upload.Add("name", "照片名稱");
            upload.Add("message", "照片描述");
            upload.Add("no_story", "1"); // 是否要發佈訊息
            upload.Add("access_token", pageAccessToken);
            upload.Add("@file.png", media);

            return fbApp.Post(_pageID + "/photos", upload) as JsonObject;
        }

        /// <summary>
        /// 設為封面照片
        /// </summary>
        /// <param name="fbApp"></param>
        /// <param name="PhotoID"></param>
        private void SetCoverPhoto(Facebook.FacebookClient fbApp, string PhotoID)
        {
            dynamic param = new ExpandoObject();
            param = new
            {
                access_token = Session["PageAccessToken"].ToString(),
                cover = PhotoID,
                no_feed_story = true
            };

            fbApp.Post(_pageID, param);
        }

        /// <summary>
        /// 取得授權登入網址
        /// </summary>
        /// <returns></returns>
        private System.Uri GetFacebookLoginUrl()
        {
            var fb = new FacebookClient();
            dynamic result = fb.GetLoginUrl(new
            {
                client_id = _appID,
                client_secret = _appSecret,
                grant_type = "client_credentials",
                scope = _scope,
                redirect_uri = _redirect_url
            });

            return result;
        }
    }
}