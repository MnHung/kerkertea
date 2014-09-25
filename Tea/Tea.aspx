<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tea.aspx.cs" Inherits="Tea.Tea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上傳照片至粉絲專頁並設為封面照片</title>
</head>
<body>
    <div id="fb-div">
        <form id="form1" runat="server">
            <div>

                <h1>上傳照片至粉絲專頁並設為封面照片</h1>
                <hr />
                <asp:HiddenField ID="loginState" runat="server" Value="not_authorized" />
                <asp:Label ID="TextBox1" runat="server"></asp:Label>
                <h2>01.取得 FB 授權允許</h2>
                <asp:Button ID="btnFBConnect" runat="server" Text="取得 FB 授權允許"
                    OnClick="btnFBConnect_Click" />

                <asp:Panel ID="PanelUpload" runat="server">
                    <h2>02.上傳照片並設為封面</h2>
                    <ul>
                        <li>粉絲專頁ID: 
                <asp:DropDownList ID="ddlPageID" runat="server">
                </asp:DropDownList>
                        </li>
                        <li>照片檔名:
                    <asp:TextBox ID="tbFileName" runat="server" Text="Koala.jpg"></asp:TextBox></li>
                        <li>
                            <asp:Button ID="btnUpload" runat="server" Text="上傳照片到粉絲專頁，並設為封面照片"
                                OnClick="btnUpload_Click" /></li>
                    </ul>
                </asp:Panel>


            </div>
        </form>
    </div>

    <div id="photo-div">
        <div>

            <video id="camera" class="photo"></video>
            <br />

            <canvas id="snap-shot" class="photo"></canvas>

        </div>
        <div>
            <button id="take-photo">take photo</button>
            <button id="start-camera">start camera</button>
            <button id="stop-camera">stop camera</button>
        </div>

        <img id="photo" src="" class="photo">
    </div>



    <script src="../Scripts/js/jquery-2.1.1.js"></script>
    <script src="../Scripts/js/common.js"></script>
    <script src="../Scripts/js/take-photo.js"></script>
</body>
</html>
