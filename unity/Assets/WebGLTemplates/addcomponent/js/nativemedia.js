const ID_UNITY = "canvas"; // unity container
const ID_CANVAS = "canvasContainer"; // unity canvas container
const ID_VIDEO = "video"; // video container
const ID_STREAM = "stream"; // actual video stream element
const ID_CANVAS_IMAGE = "imageCanvas"; // canvas used to draw image
const ID_INPUT = "input"; // input field used for image selection

const ID_BUTTON_PAUSE = "buttonPause"; // pause the video stream
const ID_BUTTON_RESUME = "buttonResume"; // resumes the video stream
const ID_BUTTON_SAVE = "buttonSave"; // saves the current video frame
const ID_BUTTON_CLOSE = "buttonClose"; // close the camera view

var max_size = -1;
var unity_gameObject;
var unity_callback;
var unity_log_callback;
var localStream;

var containerUnity = document.getElementById(ID_UNITY);
var containerVideo = document.getElementById(ID_VIDEO);
var video = document.getElementById(ID_STREAM);
var buttonPause = document.getElementById(ID_BUTTON_PAUSE);
var buttonResume = document.getElementById(ID_BUTTON_RESUME);
var buttonSave = document.getElementById(ID_BUTTON_SAVE);
var buttonClose = document.getElementById(ID_BUTTON_CLOSE);

function enableCamera(audioEnabled, maxSize, gameObject, callback) {
    // SendMessage(gameObject, callback, "enable camera " + audioEnabled + ", " + maxSize);

    // set max size
    max_size = maxSize;
    unity_gameObject = gameObject;
    unity_callback = callback;

    // Prefer camera resolution nearest to 1280x720.
    var constraints = {
        audio: false,
        video: {
            width: 1280,
            height: 720
        }
    };

    // Older browsers might not implement mediaDevices at all, so we set an empty object first
    if (navigator.mediaDevices === undefined) {
        navigator.mediaDevices = {};
    }

    // Some browsers partially implement mediaDevices. We can't just assign an object
    // with getUserMedia as it would overwrite existing properties.
    // Here, we will just add the getUserMedia property if it's missing.
    if (navigator.mediaDevices.getUserMedia === undefined) {
        navigator.mediaDevices.getUserMedia = function(constraints) {

            // First get ahold of the legacy getUserMedia, if present
            var getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia;

            // Some browsers just don't implement it - return a rejected promise with an error
            // to keep a consistent interface
            if (!getUserMedia) {
                return null;
            } else {
                // Otherwise, wrap the call to the old navigator.getUserMedia with a Promise
                return new Promise(function(resolve, reject) {
                    getUserMedia.call(navigator, constraints, resolve, reject);
                });
            }
        }
    }

    var userMedia = navigator.mediaDevices.getUserMedia({
        audio: false,
        video: true
    });

    if (userMedia === null) {

        var callback = {
            "success": false,
            "message": "Camera is not supported"
        };

        SendMessage(unity_gameObject, unity_callback, JSON.stringify(callback));

    } else {
        userMedia.then(function(stream) {
                // keep a reference
                localStream = stream;

                // Older browsers may not have srcObject
                if ("srcObject" in video) {
                    video.srcObject = stream;
                } else {
                    // Avoid using this in new browsers, as it is going away.
                    video.src = window.URL.createObjectURL(stream);
                }
                video.onloadedmetadata = function(e) {
                    video.play();
                    video.style.objectFit = "cover";

                    containerUnity.style.display = "none";
                    containerVideo.style.display = "";

                    buttonPause.style.display = "";
                    buttonResume.style.display = "none";
                    buttonSave.style.display = "none";

                    buttonClose.addEventListener("click", closeCamera);
                    buttonPause.addEventListener("click", pauseCamera);
                    buttonResume.addEventListener("click", resumeCamera);
                    buttonSave.addEventListener("click", saveCameraImage);
                };
            })
            .catch(function(err) {
                var callback = {
                    "success": false,
                    "message": err.name + ": " + err.message
                };

                SendMessage(unity_gameObject, unity_callback, JSON.stringify(callback));
            });
    }
}

function closeCamera(e) {
    e.preventDefault();
    var callback = {
        "success": true,
        "image": null
    };

    SendMessage(unity_gameObject, unity_callback, JSON.stringify(callback));

    disableCamera();

    containerUnity.style.display = "";
    containerVideo.style.display = "none";
}

function pauseCamera(e) {
    e.preventDefault();

    buttonPause.style.display = "none";
    buttonResume.style.display = "";
    buttonSave.style.display = "";

    // Pause video playback of stream.
    video.pause();
}

function resumeCamera(e) {
    e.preventDefault();

    buttonPause.style.display = "";
    buttonResume.style.display = "none";
    buttonSave.style.display = "none";

    // resume video playback of stream.
    video.play();
}

function saveCameraImage(e) {
    e.preventDefault();

    var data = getImage(video, video.videoWidth, video.videoHeight);
    var callback = {
        "success": true,
        "image": data
    };

    SendMessage(unity_gameObject, unity_callback, JSON.stringify(callback));

    disableCamera();

    containerUnity.style.display = "";
    containerVideo.style.display = "none";
}

function disableCamera() {

    // SendMessage(unity_gameObject, unity_callback, "disable camera");

    buttonClose.removeEventListener("click", closeCamera);
    buttonPause.removeEventListener("click", pauseCamera);
    buttonResume.removeEventListener("click", resumeCamera);
    buttonSave.removeEventListener("click", saveCameraImage);

    var canvas = document.getElementById(ID_CANVAS_IMAGE);
    var context = canvas.getContext('2d');
    context.clearRect(0, 0, 0, 0);
    context.beginPath();

    localStream.getTracks().forEach(function(track) {
        track.stop()
    });
}

function selectImage(maxSize, gameObject, callback) {
    // SendMessage(gameObject, callback, "selectImage " + maxSize);

    // set max size
    max_size = maxSize;
    unity_gameObject = gameObject;
    unity_callback = callback;

    var input = document.getElementById(ID_INPUT);
    var canvas = document.getElementById(ID_UNITY);

    var OpenFileDialog = function() {
        input.click();
        canvas.removeEventListener('click', OpenFileDialog);
    };

    // detect mouse down on canvas container
    canvas.addEventListener('click', OpenFileDialog, false);

    input.onclick = function(event) {
        this.value = null;
    }

    input.onchange = function(event) {
        // select the first file
        var file = event.target.files[0];

        var type = /^image\//;
        if (type.test(file.type)) {

            var reader = new FileReader();
            reader.onload = function(event) {

                var image = new Image();
                image.onload = function(event) {

                    var data = getImage(image, image.width, image.height);
                    var callback = {
                        "success": true,
                        "image": data
                    };

                    SendMessage(unity_gameObject, unity_callback, JSON.stringify(callback));
                }
                image.src = event.target.result;
            }
            reader.readAsDataURL(file);
        } else {
            var callback = {
                "success": false,
                "message": "Not a valid image file"
            };

            SendMessage(unity_gameObject, unity_callback, JSON.stringify(callback));
        }
    }
}

function getImage(element, width, height) {
    var canvas = document.getElementById(ID_CANVAS_IMAGE);
    var context = canvas.getContext('2d');

    var targetWidth = width;
    var targetHeight = height;

    // SendMessage(unity_gameObject, unity_callback, "Image Loaded " + width + "x" + height + "px");

    if (targetWidth && targetHeight) {

        // resize image if necessary
        if (max_size !== -1 && max_size !== "-1") {
            if (targetWidth > targetHeight) {
                if (targetWidth > max_size) {
                    targetHeight *= max_size / targetWidth;
                    targetWidth = max_size;
                }
            } else {
                if (targetHeight > max_size) {
                    targetWidth *= max_size / targetHeight;
                    targetHeight = max_size;
                }
            }
            // SendMessage(unity_gameObject, unity_callback, "Image Resized " + targetWidth + "x" + targetHeight + "px");

        } else {
            // SendMessage(unity_gameObject, unity_callback, "No Resize");
        }

        // Setup a canvas with the same dimensions as the video.
        canvas.width = targetWidth;
        canvas.height = targetHeight;

        // Make a copy of the current frame in the video on the canvas.
        context.drawImage(element, 0, 0, targetWidth, targetHeight);

        // Turn the canvas image into a dataURL that can be used as a src for our photo.
        return canvas.toDataURL('image/png');
    }
}
