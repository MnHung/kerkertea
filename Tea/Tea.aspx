<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tea.aspx.cs" Inherits="Tea.Tea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上傳照片至粉絲專頁並設為封面照片</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    <h1>上傳照片至粉絲專頁並設為封面照片</h1>
    <hr />

        <h2>01.取得 FB 授權允許</h2>
        <asp:Button ID="btnFBConnect" runat="server" Text="取得 FB 授權允許" 
            onclick="btnFBConnect_Click" />

        <asp:Panel ID="PanelUpload" runat="server" Visible="false">
        <h2>02.上傳照片並設為封面</h2>
        <ul>
            <li>粉絲專頁ID: 
                <asp:DropDownList ID="ddlPageID" runat="server">
                </asp:DropDownList>
            </li>
            <li>照片檔名: <asp:TextBox ID="tbFileName" runat="server" Text="Koala.jpg"></asp:TextBox></li>
            <li><asp:Button ID="btnUpload" runat="server" Text="上傳照片到粉絲專頁，並設為封面照片" 
                    onclick="btnUpload_Click" /></li>
        </ul>
        </asp:Panel>
        

    </div>
    </form>
</body>
</html>
