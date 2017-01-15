
"use strict";

var commonUI = window.sampleUI.commonUI;
var input = window.sampleUI.input;
var misc = window.sampleUI.misc;
var mobileHacks = window.sampleUI.mobileHacks;
var strings = window.sampleUI.strings;
var touch = window.sampleUI.touch;
var inputElem = document.getElementById("inputarea");

var $ = document.getElementById.bind(document);
var globals = {
  debug: false,
  // orientation: "landscape-primary",
  provideOrientation: false,
  provideMotion: false,
  provideRotationRate: false,
};
misc.applyUrlSettings(globals);
mobileHacks.disableContextMenu();
mobileHacks.fixHeightHack();
mobileHacks.adjustCSSBasedOnPhone([
  {
    test: mobileHacks.isIOS8OrNewerAndiPhone4OrIPhone5,
    styles: {
      ".button": {
        bottom: "40%",
      },
    },
  },
]);

var client = new window.hft.GameClient();

function to255(v) {
  return v * 255 | 0;
}
function handleColor(data) {
  // the color arrives in data.color.
  // we use chroma.js to darken the color
  // then we get our style from a template in controller.html
  // sub in our colors, remove extra whitespace and attach to body.
  var color = "rgb(" + to255(data.color.r) + "," + to255(data.color.g) + "," + to255(data.color.b) + ")";
  var subs = {
    light: color,
    dark: chroma(color).darken().hex(),
  };
  var style = $("background-style").text;
  style = strings.replaceParams(style, subs).replace(/[\n ]+/g, ' ').trim();
  $("hft-content").style.background = style;
}

function handlePlay() {
  commonUI.setOrientation("portrait", true);
}

client.addEventListener('color', handleColor);
client.addEventListener('play', handlePlay);

commonUI.setupStandardControllerUI(client, globals);

var maxIndex = 10;
var usedIndices = [];
var pointerIdToIndex = {};
function getPointerIndex(e, start) {
  var index = pointerIdToIndex[e.pointerId];
  if (index === undefined) {
    for (var ii = 0; ii < maxIndex; ++ii) {
      if (!usedIndices[ii]) {
        usedIndices[ii] = start;
        index = ii;
        break;
      }
    }
    if (index === undefined) {
      throw "what";
    }
    pointerIdToIndex[e.pointerId] = index;
  }
  if (!start) {
    delete pointerIdToIndex[e.pointerId];
    usedIndices[index] = undefined;
  }
  return index;
}

function getAngle(event){
	var target = event.target;
	var p = input.getRelativeCoordinates(target, event);
	  
	var x = (p.x / target.clientWidth) - 0.5;
	var y = (p.y / target.clientHeight) - 0.5;
	var a = Math.atan2(-y,x) * 180 / Math.PI;
	if(a < 0){
		a = a + 360;
	}
	a = a / 11.25;	// 360 / 11.25 = 32 different intervals
	return a | 0; 	// "| 0" floors result
}

// Setup the touch area
var lastA = 0;
inputElem.addEventListener('pointermove', function(event){
	var a = getAngle(event);
	if(a != lastA){	// only resend if its different from last time
		client.sendCmd('touchDir', { angle : a });  
		lastA = a;
	}
  
	event.preventDefault();
});

function handleTouchDown(e) {
	lastA = getAngle(e);
	client.sendCmd('touch', { touching:true , angle: lastA });
	
}

function handleTouchUp(e) {
	client.sendCmd('touch', { touching:false , angle: 0 });
}

inputElem.addEventListener('pointerdown', handleTouchDown);
inputElem.addEventListener('pointerup', handleTouchUp);

//$("touch").addEventListener('pointerdown', handleTouchDown);
//$("touch").addEventListener('pointerup', handleTouchUp);


