var websocket;
var joyContainer = document.getElementById('joyContainer');
var joyThumb = document.getElementById('joyThumb');
var buttonContainer = document.getElementById('buttonContainer');
var sideMargin = 50;
var joyContainerTop = 0;
var joyContainerSize = 1;
var enableSockets = true;

function getLeftTouch(touches) {
    var centerX = document.body.clientWidth/2;
    for(var i=0; i<touches.length; i++) {
        var touch = touches[i];
        if(touch.clientX < centerX)
            return touch;
    }
    return null;
}

function init() {
    if(enableSockets) {
        websocket = new WebSocket("ws://" + document.location.hostname + ":8082/");
        websocket.onopen = function (evt) { onOpen(evt) };
        websocket.onclose = function (evt) { onClose(evt) };
        websocket.onmessage = function (evt) { onMessage(evt) };
        websocket.onerror = function (evt) { onError(evt) };
    }

    document.body.addEventListener('touchmove', touchMove);
    document.body.addEventListener('touchstart', touchMove);
    document.body.addEventListener('touchend', touchMove);
    for (var i = 0; i < 4; i++)
        registerButton(i);
    updateSize();
    window.addEventListener('resize', updateSize);
}
function touchMove(e) {
    var touch = getLeftTouch(e.touches);
    if (touch) {
        var radius = joyContainerSize / 2;
        var dirX = (touch.clientX - (sideMargin + radius))/radius;
        var dirY = ((joyContainerTop + radius) - touch.clientY)/radius;
        var dirLen = Math.sqrt(dirX * dirX + dirY * dirY);
        if(dirLen > 1) {
            dirX /= dirLen;
            dirY /= dirLen;
        }
        if(websocket)
            websocket.send(`^|${dirX}|${dirY}`);
        // console.log(dirX, dirY);
        var top = radius - dirY * radius;
        var left = radius + dirX * radius;

        joyThumb.style.top = top + 'px';
        joyThumb.style.left = left + 'px';
    }
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

function registerButton(i) {
    var button = document.getElementById('button' + i);
    button.addEventListener('click', (e) => {
        if(websocket)
            websocket.send(`!|${i}`);
    });
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
