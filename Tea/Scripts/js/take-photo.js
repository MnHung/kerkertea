var Debug = true;
var loginState;

$(function () {    
    if (!Debug) {
        $("#fb-div").hide();
        $("button").hide();
    }

    // start here, like a "main()"
    loginState = $("#loginState").val();
    print("#loginState: " + loginState);
    if (loginState == "connected") {
        tp.start();

        // safe delay
        setTimeout(function () {
            tp.startCamera();
        }, 1000);
    }
    else {
        tp.start();
        tp.startCamera();
        // safe delay
        //setTimeout(function () {
        //    $("#btnFBConnect").click();
        //    print("login.........");
        //}, 500);
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
        tp.takePhoto();
    });

    $(document).keydown(function (key) {
        if (key.keyCode == 87) {    // "w"
            print("w");

            ///
            tp.takePhoto();
        }
    });
}

tp.takePhoto = function () {
    if (tp.videoStream) {
        var pCanvus = document.getElementById("snap-shot");
        var pContext = pCanvus.getContext("2d");
        var video = tp.video;

        //debugger;
        video.pause();

        pCanvus.width = video.videoWidth;
        pCanvus.height = video.videoHeight;

        pContext.drawImage(video, 0, 0, pCanvus.width, pCanvus.height);

        var pDataUrl = pCanvus.toDataURL('image/jpeg');
        document.getElementById("photo").src = pDataUrl;
        tp.video.play();

        $.post("./CheatTea", { photo: pDataUrl });
    }
    //tp.stopCamera();
}


tp.startCamera = function () {
    navigator.myGetMedia = (navigator.getUserMedia ||
            navigator.webkitGetUserMedia ||
            navigator.mozGetUserMedia ||
            navigator.msGetUserMedia);

    navigator.myGetMedia({ video: true }, tp.onCameraConnect, tp.onCameraConnectError);
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