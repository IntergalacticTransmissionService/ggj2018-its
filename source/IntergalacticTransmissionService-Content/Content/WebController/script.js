var websocket;
var joyContainer = document.getElementById('joyContainer');
var joyThumb = document.getElementById('joyThumb');
var buttonContainer = document.getElementById('buttonContainer');
var sideMargin = 50;
var joyContainerTop = 0;
var joyContainerSize = 1;
var buttonsState = [0, 0, 0, 0];

function enterFullscreen() {
    var element = document.body;
    if (element.requestFullscreen) {
        element.requestFullscreen();
    } else if (element.mozRequestFullScreen) {
        element.mozRequestFullScreen();
    } else if (element.msRequestFullscreen) {
        element.msRequestFullscreen();
    } else if (element.webkitRequestFullscreen) {
        element.webkitRequestFullscreen();
    }
}
function exitFullscreen() {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
    }
}
function toggleFullscreen() {
    if (document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen)
        exitFullscreen();
    else
        enterFullscreen();
}
function getLeftTouch(touches) {
    var centerX = document.body.clientWidth * 0.4;
    for (var i = 0; i < touches.length; i++) {
        var touch = touches[i];
        if (touch.clientX < centerX)
            return touch;
    }
    return null;
}

function updateButtonsState(touches) {
    var newState = [0, 0, 0, 0];
    for (var i = 0; i < touches.length; i++) {
        var touch = touches[i];
        var touchTarget = document.elementFromPoint(touch.clientX, touch.clientY);
        if (touchTarget && touchTarget.id && touchTarget.id.startsWith("button")) {
            var index = parseInt(touchTarget.id.substr(6));
            newState[index] = 1;
        }
    }

    for (var i = 0; i < 4; i++) {
        if (buttonsState[i] != newState[i]) {
            buttonsState = newState;
            if (websocket)
                websocket.send(`!|${buttonsState.join('')}`);
            return;
        }
    }
}

function init() {
    try {
        websocket = new WebSocket("ws://" + document.location.hostname + ":8082/");
        websocket.onopen = function (evt) { onOpen(evt) };
        websocket.onclose = function (evt) { onClose(evt) };
        websocket.onmessage = function (evt) { onMessage(evt) };
        websocket.onerror = function (evt) { onError(evt) };
    } catch (e) {
        console.error('Websocket creation failed', e);
        websocket = null;
    }

    document.body.addEventListener('touchmove', touchMove);
    document.body.addEventListener('touchstart', touchMove);
    document.body.addEventListener('touchend', touchMove);
    updateSize();
    window.addEventListener('resize', updateSize);
    document.getElementById('fullScreenToggle').addEventListener('click', toggleFullscreen, false);
}
function touchMove(e) {
    updateButtonsState(e.touches);
    var touch = getLeftTouch(e.touches);
    var radius = joyContainerSize / 2;
    var dirX = 0, dirY = 0;
    if (touch) {
        dirX = (touch.clientX - (sideMargin + radius)) / radius;
        dirY = ((joyContainerTop + radius) - touch.clientY) / radius;
    }
    var dirLen = Math.sqrt(dirX * dirX + dirY * dirY);
    if (dirLen > 1) {
        dirX /= dirLen;
        dirY /= dirLen;
    }
    if (websocket)
        websocket.send(`^|${dirX}|${dirY}`);
    // console.log(dirX, dirY);
    var top = radius - dirY * radius;
    var left = radius + dirX * radius;

    joyThumb.style.top = top + 'px';
    joyThumb.style.left = left + 'px';
}

function updateSize() {
    var w = document.body.clientWidth;
    var h = document.body.clientHeight;
    var hw = w / 2;
    joyContainerSize = (hw > h ? h : hw) * 0.5;
    var sizePx = joyContainerSize + 'px';
    joyContainerTop = (joyContainerSize / 2);
    var topPx = joyContainerTop + 'px';
    joyContainer.style.height = sizePx;
    joyContainer.style.width = sizePx;
    joyContainer.style.top = topPx;
    joyContainer.style.left = sideMargin + 'px';
    buttonContainer.style.height = sizePx;
    buttonContainer.style.width = sizePx;
    buttonContainer.style.top = topPx;
    buttonContainer.style.right = sideMargin + 'px';
}

function onOpen(evt) {
    console.log("CONNECTED");
}

function onClose(evt) {
    console.log("DISCONNECTED");
}

function onMessage(evt) {
    console.log(evt.data);
}

function onError(evt) {
    console.error(evt.data);
}

window.addEventListener("load", init, false);
