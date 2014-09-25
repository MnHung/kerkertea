<%@ Page Title="註冊外部登入" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterExternalLogin.aspx.cs" Inherits="Tea.Account.RegisterExternalLogin" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<h3>以您的 <%: ProviderName %> 帳戶登入</h3>

    <asp:PlaceHolder runat="server">
        <div class="form-horizontal">
            <h4>關聯表單</h4>
            <hr />
            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
            <p class="text-info">
                您已透過 <strong><%: ProviderName %></strong>. 進行驗證。請在下方輸入目前網站的使用者名稱
                並按一下 [記錄] 按鈕。
            </p>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="userName" CssClass="col-md-2 control-label">使用者名稱</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="userName" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="userName"
                        Display="Dynamic" CssClass="text-danger" ErrorMessage="需要使用者名稱" />
                    <asp:ModelErrorMessage runat="server" ModelStateKey="UserName" CssClass="text-error" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" Text="登入" CssClass="btn btn-default" OnClick="LogIn_Click" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
