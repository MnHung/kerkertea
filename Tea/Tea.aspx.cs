﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using Facebook;

namespace Tea
{
    public partial class Tea : System.Web.UI.Page
    {
        private string _pageID = "";
        private string _fileName = "";

        private string _appID = "691947690891601";
        private string _appSecret = "2347601e744371de306e01be1edecb63";
        private string _scope = "public_profile, manage_notifications, manage_pages, publish_actions, user_photos";
        private string _redirect_url = "http://kerkertea.apphb.com/Tea.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            // 若已授權，querystring 應當取得 code，接下來要換回 access_token 再做事
            if (Request.Params.AllKeys.Contains("code"))
            {
                try
                {
                    var fb = new FacebookClient();

                    #region 取得 Access Token
                    dynamic result = fb.Get("oauth/access_token", new
                    {
                        client_id = _appID,
                        client_secret = _appSecret,
                        redirect_uri = _redirect_url,
                        code = Request["code"]
                    });

                    Session["accessToken"] = result.access_token;
                    #endregion

                    //#region 檢查 Scope 權限

                    //fb = new FacebookClient(Session["accessToken"].ToString());

                    //var query = string.Format("SELECT publish_stream, manage_pages FROM permissions WHERE uid = me()");
                    //dynamic parameters = new ExpandoObject();
                    //parameters.q = query;
                    //dynamic results = (IDictionary<string, object>)fb.Get("/fql", parameters);

                    //foreach (dynamic item in results.data)
                    //{
                    //    Dictionary<string, object>.KeyCollection keys = item.Keys;
                    //    if (!keys.Contains<string>("publish_stream") || !keys.Contains<string>("manage_pages"))
                    //    {
                    //        Response.Write("請允許 publish_stream 跟 manage_pages！");
                    //        Response.End();
                    //        return;
                    //    }
                    //}
                    //#endregion

                    #region 列出所有可管理的專頁

                    try
                    {
                        IDictionary<string, object> dic = (IDictionary<string, object>)fb.Get("/me/accounts");

                        // 列出所有我管理的粉絲專頁
                        IList<object> dicy = (IList<object>)dic["data"];
                        IDictionary<string, object> page = null;

                        ddlPageID.Items.Clear();
                        for (int i = 0; i <= dicy.Count - 1; i++)
                        {
                            page = (IDictionary<string, object>)dicy[i];
                            ddlPageID.Items.Add(new ListItem(page["name"].ToString(), page["id"].ToString()));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    PanelUpload.Visible = true;

                }
                catch (Exception ex)
                {
                    Response.Write("發生例外" + ex.Message);
                    Response.End();
                    return;
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // 選擇的粉絲專頁
            _pageID = ddlPageID.SelectedValue;
            // 照片檔名
            _fileName = tbFileName.Text;

            Facebook.FacebookClient fb = new Facebook.FacebookClient(Session["accessToken"].ToString());
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
                    fb.AccessToken = page["access_token"].ToString();
                    Session["PageAccessToken"] = page["access_token"].ToString();
                    break;
                }
            }

            if (Session["PageAccessToken"] == null || string.IsNullOrEmpty(Session["PageAccessToken"].ToString()))
            {
                Response.Write("無法管理此粉絲專頁: " + _pageID);
                Response.End();
            }

            // 上傳照片
            JsonObject result = UploadPhoto(fb, Server.MapPath("~/img/" + _fileName));

            // 設為封面
            SetCoverPhoto(fb, result["id"].ToString());

            string txt = string.Format("<p>上傳成功: <a href='https://www.facebook.com/photo.php?fbid={0}' target='_blank'>點擊這裡查看照片</a>，並<a href='https://www.facebook.com/{1}' target='_blank'>看看</a>是否已經設為封面了", result["id"].ToString(), _pageID);
            Response.Write(txt);
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
        private JsonObject UploadPhoto(Facebook.FacebookClient fbApp, string ImagePath)
        {
            Facebook.FacebookMediaObject media = new Facebook.FacebookMediaObject();
            media.ContentType = "image/jpeg";
            media.FileName = ImagePath;

            byte[] filebytes = System.IO.File.ReadAllBytes(ImagePath);
            media.SetValue(filebytes);
            Dictionary<string, object> upload = new Dictionary<string, object>();
            upload.Add("name", "照片名稱");
            upload.Add("message", "照片描述");
            upload.Add("no_story", "1"); // 是否要發佈訊息
            upload.Add("access_token", Session["PageAccessToken"]);
            upload.Add("@file.jpg", media);

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