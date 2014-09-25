$(function () {
    // start here, like a "main()"
    var loginState = $("#loginState").val();
    print("#loginState: " + loginState);
    if (loginState == "connected") {
        tp.start();
        tp.startCamera();
    }
    else {
        $("#btnFBConnect").click();
    }
})

var tp = {};

tp.video = null;        // the <video> element
tp.dataUrl = null;      // photo as a string
tp.videoStream = null;  // the video stream from <video>

tp.start = function () {
    $("#start-camera").click(function (argument) {
        tp.startCamera();
    });

    $("#stop-camera").click(function (argument) {
        tp.stopCamera();
    });

    $("#take-photo").click(function () {
        if (tp.videoStream) {
            var pCanvus = document.getElementById("snap-shot");
            var pContext = pCanvus.getContext("2d");
            var video = tp.video;

            //debugger;
            video.pause();

            pCanvus.width = video.videoWidth;
            pCanvus.height = video.videoHeight;

            pContext.drawImage(video, 0, 0, pCanvus.width, pCanvus.height);

            var pDataUrl = pCanvus.toDataURL('image/webp');
            document.getElementById("photo").src = pDataUrl;
            tp.video.play();

            $.post("upload-photo.php", { photo: pDataUrl });
            //$.get("red.php", {photo: pDataUrl});
        }
        tp.stopCamera();
    });
}

tp.startCamera = function () {
    navigator.myGetMedia = (navigator.getUserMedia ||
            navigator.webkitGetUserMedia ||
            navigator.mozGetUserMedia ||
            navigator.msGetUserMedia);

    navigator.myGetMedia({ video: true }, tp.onCameraConnect, tp.onCameraConnectError);

    print(2);
}

tp.stopCamera = function (argument) {
    tp.video.pause();
    tp.videoStream.stop();
}

tp.onCameraConnect = function (stream) {
    tp.video = document.getElementById("camera");
    tp.video.src = window.URL ? window.URL.createObjectURL(stream) : stream;
    tp.video.play();

    tp.videoStream = stream;
}

tp.onCameraConnectError = function (error) {
    print("camera failed, err: " + error);
}