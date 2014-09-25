using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tea.Models;

namespace Tea.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        private bool HasPassword(UserManager manager)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            return (user != null && user.PasswordHash != null);
        }

        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                // 決定要呈現的區段
                UserManager manager = new UserManager();
                if (HasPassword(manager))
                {
                    changePasswordHolder.Visible = true;
                }
                else
                {
                    setPassword.Visible = true;
                    changePasswordHolder.Visible = false;
                }
                CanRemoveExternalLogins = manager.GetLogins(User.Identity.GetUserId()).Count() > 1;

                // 呈現成功訊息
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // 使查詢字串脫離動作
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "您的密碼已變更。"
                        : message == "SetPwdSuccess" ? "您的密碼已設定。"
                        : message == "RemoveLoginSuccess" ? "帳戶已移除。"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                UserManager manager = new UserManager();
                IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), CurrentPassword.Text, NewPassword.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                }
                else
                {
                    AddErrors(result);
                }
            }
        }

        protected void SetPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // 建立本機登入資訊並將本機帳戶連結到使用者
                UserManager manager = new UserManager();
                IdentityResult result = manager.AddPassword(User.Identity.GetUserId(), password.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
                }
                else
                {
                    AddErrors(result);
                }
            }
        }

        public IEnumerable<UserLoginInfo> GetLogins()
        {
            UserManager manager = new UserManager();
            var accounts = manager.GetLogins(User.Identity.GetUserId());
            CanRemoveExternalLogins = accounts.Count() > 1 || HasPassword(manager);
            return accounts;
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            UserManager manager = new UserManager();
            var result = manager.RemoveLogin(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            var msg = result.Succeeded
                ? "?m=RemoveLoginSuccess"
                : String.Empty;
            Response.Redirect("~/Account/Manage" + msg);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}