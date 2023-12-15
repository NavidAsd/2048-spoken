//npm install --save toastr
function KeyboardInputManager() {


    this.events = {};

    if (window.navigator.msPointerEnabled) {
        //Internet Explorer 10 style
        this.eventTouchstart = "MSPointerDown";
        this.eventTouchmove = "MSPointerMove";
        this.eventTouchend = "MSPointerUp";
    } else {
        this.eventTouchstart = "touchstart";
        this.eventTouchmove = "touchmove";
        this.eventTouchend = "touchend";
    }

    this.listen();
}

KeyboardInputManager.prototype.on = function (event, callback) {
    if (!this.events[event]) {
        this.events[event] = [];
    }
    this.events[event].push(callback);
};

KeyboardInputManager.prototype.emit = function (event, data) {
    var callbacks = this.events[event];
    if (callbacks) {
        callbacks.forEach(function (callback) {
            callback(data);
        });
    }
};

KeyboardInputManager.prototype.listen = function () {
    var self = this;

    var map = {
        "بالا": 0, // Up
        "سمت بالا": 0, // Up
        "به سمت بالا": 0, // Up
        "برو بالا": 0, // Up
        "بیا بالا": 0, // Up
        "بالا بالا": 0, // Up
        "بالا بالا بالا": 0, // Up

        "راست": 1, // Right
        "سمت راست": 1, // Right
        "به سمت راست": 1, // Right
        "برو راست": 1, // Right
        "بیا راست": 1, // Right
        "راست راست": 1, // Right
        "راست راست راست": 1, // Right

        "پایین": 2, // Down
        "برو پایین": 2, // Down
        "بیا پایین": 2, // Down
        "سمت پایین": 2, // Down
        "به سمت پایین": 2, // Down
        "پایین پایین": 2, // Down
        "پایین پایین پایین": 2, // Down

        "چپ": 3, // Left
        "سمت چپ": 3, // Left
        "به سمت چپ": 3, // Left
        "برو چپ": 3, // Left
        "بیا چپ": 3, // Left
        "چپ چپ": 3, // Left
        "چپ چپ چپ": 3, // Left
    };

    // Listen to voice commands
    //const io = require('socket.io-client');
    const token = document.getElementById("token").innerHTML;
    const socket = io("https://ent.persianspeech.com", { extraHeaders: { token: token, platform: "mobile-app" } });

    socket.on("connect", () => {
        console.log("connected");
    });

    socket.on("disconnect", () => {
        console.log("disconnected");
    });


    socket.on("result", (data) => {
        if ("result" in data) {
            var command = data.text;
            var mapped = map[command];
            if (data.text == "بازی جدید" || data.text == "از اول") {
                toastr.success(data.text);
                self.restart.call(self, event);
            }
            else if (data.text in map) {
                toastr.success(data.text);
            }
            else if (data.text.length <= 12) {
                toastr.error(data.text);
            }
            else {
                toastr.error("کارکتر بیش از حد مجاز! از جدول راهنما استفاده کن");
            }
            if (mapped !== undefined) {
                self.emit("move", mapped);

            }

        }
    });

    socket.on("start-microphone", async (data) => {
        if (data.lockChecked) {
            try {
                const stream = await navigator.mediaDevices.getUserMedia({ audio: true })
                micStream = stream

                recorder = new MediaRecorder(stream, { mimeType: "audio/webm;codecs=opus" });
                recorder.ondataavailable = ({ data: blob }) => {
                    socket.emit('microphone-blob', blob);
                }
                recorder.start(100);
            } catch (error) {
                //console.log("device not found (mic)");
                toastr.error("ورودی میکروفن یافت نشد");
            }
        }
    });
    let recording = false;

    if (!recording) {
        socket.emit("start-microphone")
        toastr.info("بازی شروع شد");
        recording = true
    }



    // Respond to direction keys
    document.addEventListener("keydown", function (event) {
        var modifiers = event.altKey || event.ctrlKey || event.metaKey ||
            event.shiftKey;
        var mapped = map[event.key];

        if (!modifiers) {
            if (mapped !== undefined) {
                event.preventDefault();
                self.emit("move", mapped);
            }
        }

        // R key restarts the game
        if (!modifiers && event.key.toLowerCase() === "r") {
            self.restart.call(self, event);
        }
    });

    // Respond to button presses
    this.bindButtonPress(".retry-button", this.restart);
    this.bindButtonPress(".restart-button", this.restart);
    this.bindButtonPress(".keep-playing-button", this.keepPlaying);

    // Respond to swipe events
    var touchStartClientX, touchStartClientY;
    var gameContainer = document.getElementsByClassName("game-container")[0];

    gameContainer.addEventListener(this.eventTouchstart, function (event) {
        if ((!window.navigator.msPointerEnabled && event.touches.length > 1) ||
            event.targetTouches.length > 1) {
            return; // Ignore if touching with more than 1 finger
        }

        if (window.navigator.msPointerEnabled) {
            touchStartClientX = event.pageX;
            touchStartClientY = event.pageY;
        } else {
            touchStartClientX = event.touches[0].clientX;
            touchStartClientY = event.touches[0].clientY;
        }

        event.preventDefault();
    });

    gameContainer.addEventListener(this.eventTouchmove, function (event) {
        event.preventDefault();
    });

    gameContainer.addEventListener(this.eventTouchend, function (event) {
        if ((!window.navigator.msPointerEnabled && event.touches.length > 0) ||
            event.targetTouches.length > 0) {
            return; // Ignore if still touching with one or more fingers
        }

        var touchEndClientX, touchEndClientY;

        if (window.navigator.msPointerEnabled) {
            touchEndClientX = event.pageX;
            touchEndClientY = event.pageY;
        } else {
            touchEndClientX = event.changedTouches[0].clientX;
            touchEndClientY = event.changedTouches[0].clientY;
        }

        var dx = touchEndClientX - touchStartClientX;
        var absDx = Math.abs(dx);

        var dy = touchEndClientY - touchStartClientY;
        var absDy = Math.abs(dy);

        if (Math.max(absDx, absDy) > 10) {
            // (right : left) : (down : up)
            self.emit("move", absDx > absDy ? (dx > 0 ? 1 : 3) : (dy > 0 ? 2 : 0));
        }
    });
};

KeyboardInputManager.prototype.restart = function (event) {
    event.preventDefault();
    this.emit("restart");
};

KeyboardInputManager.prototype.keepPlaying = function (event) {
    event.preventDefault();
    this.emit("keepPlaying");
};

KeyboardInputManager.prototype.bindButtonPress = function (selector, fn) {
    var button = document.querySelector(selector);
    button.addEventListener("click", fn.bind(this));
    button.addEventListener(this.eventTouchend, fn.bind(this));
};
