!function(t){var e={};function n(r){if(e[r])return e[r].exports;var o=e[r]={i:r,l:!1,exports:{}};return t[r].call(o.exports,o,o.exports,n),o.l=!0,o.exports}n.m=t,n.c=e,n.d=function(t,e,r){n.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:r})},n.r=function(t){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},n.t=function(t,e){if(1&e&&(t=n(t)),8&e)return t;if(4&e&&"object"==typeof t&&t&&t.__esModule)return t;var r=Object.create(null);if(n.r(r),Object.defineProperty(r,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var o in t)n.d(r,o,function(e){return t[e]}.bind(null,o));return r},n.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return n.d(e,"a",e),e},n.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},n.p="/dist/",n(n.s=2)}([function(t,e){t.exports=dc},function(t,e){t.exports=d3},function(t,e,n){"use strict";n.r(e);var r=n(0),o=n(1);function i(t){return(i="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(t){return typeof t}:function(t){return t&&"function"==typeof Symbol&&t.constructor===Symbol&&t!==Symbol.prototype?"symbol":typeof t})(t)}function a(t,e){for(var n=0;n<e.length;n++){var r=e[n];r.enumerable=r.enumerable||!1,r.configurable=!0,"value"in r&&(r.writable=!0),Object.defineProperty(t,r.key,r)}}function u(t,e,n){return(u="undefined"!=typeof Reflect&&Reflect.get?Reflect.get:function(t,e,n){var r=function(t,e){for(;!Object.prototype.hasOwnProperty.call(t,e)&&null!==(t=f(t)););return t}(t,e);if(r){var o=Object.getOwnPropertyDescriptor(r,e);return o.get?o.get.call(n):o.value}})(t,e,n||t)}function c(t,e){return(c=Object.setPrototypeOf||function(t,e){return t.__proto__=e,t})(t,e)}function s(t){var e=function(){if("undefined"==typeof Reflect||!Reflect.construct)return!1;if(Reflect.construct.sham)return!1;if("function"==typeof Proxy)return!0;try{return Date.prototype.toString.call(Reflect.construct(Date,[],(function(){}))),!0}catch(t){return!1}}();return function(){var n,r=f(t);if(e){var o=f(this).constructor;n=Reflect.construct(r,arguments,o)}else n=r.apply(this,arguments);return l(this,n)}}function l(t,e){return!e||"object"!==i(e)&&"function"!=typeof e?function(t){if(void 0===t)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return t}(t):e}function f(t){return(f=Object.setPrototypeOf?Object.getPrototypeOf:function(t){return t.__proto__||Object.getPrototypeOf(t)})(t)}var h=function(t){!function(t,e){if("function"!=typeof e&&null!==e)throw new TypeError("Super expression must either be null or a function");t.prototype=Object.create(e&&e.prototype,{constructor:{value:t,writable:!0,configurable:!0}}),e&&c(t,e)}(l,t);var e,n,r,i=s(l);function l(){var t;return function(t,e){if(!(t instanceof e))throw new TypeError("Cannot call a class as a function")}(this,l),(t=i.call(this))._listeners=o.dispatch("rendered"),t}return e=l,(n=[{key:"render",value:function(){u(f(l.prototype),"render",this).call(this),this._listeners.call("rendered",null,this._g)}},{key:"on",value:function(t,e){this._listeners.on(t,e)}}])&&a(e.prototype,n),r&&a(e,r),l}(r.Legend);function y(t){return(y="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(t){return typeof t}:function(t){return t&&"function"==typeof Symbol&&t.constructor===Symbol&&t!==Symbol.prototype?"symbol":typeof t})(t)}function p(t,e){return function(t){if(Array.isArray(t))return t}(t)||function(t,e){if("undefined"==typeof Symbol||!(Symbol.iterator in Object(t)))return;var n=[],r=!0,o=!1,i=void 0;try{for(var a,u=t[Symbol.iterator]();!(r=(a=u.next()).done)&&(n.push(a.value),!e||n.length!==e);r=!0);}catch(t){o=!0,i=t}finally{try{r||null==u.return||u.return()}finally{if(o)throw i}}return n}(t,e)||function(t,e){if(!t)return;if("string"==typeof t)return v(t,e);var n=Object.prototype.toString.call(t).slice(8,-1);"Object"===n&&t.constructor&&(n=t.constructor.name);if("Map"===n||"Set"===n)return Array.from(t);if("Arguments"===n||/^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))return v(t,e)}(t,e)||function(){throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")}()}function v(t,e){(null==e||e>t.length)&&(e=t.length);for(var n=0,r=new Array(e);n<e;n++)r[n]=t[n];return r}function d(t,e){for(var n=0;n<e.length;n++){var r=e[n];r.enumerable=r.enumerable||!1,r.configurable=!0,"value"in r&&(r.writable=!0),Object.defineProperty(t,r.key,r)}}function b(t,e,n){return(b="undefined"!=typeof Reflect&&Reflect.get?Reflect.get:function(t,e,n){var r=function(t,e){for(;!Object.prototype.hasOwnProperty.call(t,e)&&null!==(t=O(t)););return t}(t,e);if(r){var o=Object.getOwnPropertyDescriptor(r,e);return o.get?o.get.call(n):o.value}})(t,e,n||t)}function g(t,e){return(g=Object.setPrototypeOf||function(t,e){return t.__proto__=e,t})(t,e)}function _(t){var e=function(){if("undefined"==typeof Reflect||!Reflect.construct)return!1;if(Reflect.construct.sham)return!1;if("function"==typeof Proxy)return!0;try{return Date.prototype.toString.call(Reflect.construct(Date,[],(function(){}))),!0}catch(t){return!1}}();return function(){var n,r=O(t);if(e){var o=O(this).constructor;n=Reflect.construct(r,arguments,o)}else n=r.apply(this,arguments);return m(this,n)}}function m(t,e){return!e||"object"!==y(e)&&"function"!=typeof e?function(t){if(void 0===t)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return t}(t):e}function O(t){return(O=Object.setPrototypeOf?Object.getPrototypeOf:function(t){return t.__proto__||Object.getPrototypeOf(t)})(t)}function w(t){return(w="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(t){return typeof t}:function(t){return t&&"function"==typeof Symbol&&t.constructor===Symbol&&t!==Symbol.prototype?"symbol":typeof t})(t)}function k(t,e){var n;if("undefined"==typeof Symbol||null==t[Symbol.iterator]){if(Array.isArray(t)||(n=function(t,e){if(!t)return;if("string"==typeof t)return S(t,e);var n=Object.prototype.toString.call(t).slice(8,-1);"Object"===n&&t.constructor&&(n=t.constructor.name);if("Map"===n||"Set"===n)return Array.from(t);if("Arguments"===n||/^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))return S(t,e)}(t))||e&&t&&"number"==typeof t.length){n&&(t=n);var r=0,o=function(){};return{s:o,n:function(){return r>=t.length?{done:!0}:{done:!1,value:t[r++]}},e:function(t){throw t},f:o}}throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")}var i,a=!0,u=!1;return{s:function(){n=t[Symbol.iterator]()},n:function(){var t=n.next();return a=t.done,t},e:function(t){u=!0,i=t},f:function(){try{a||null==n.return||n.return()}finally{if(u)throw i}}}}function S(t,e){(null==e||e>t.length)&&(e=t.length);for(var n=0,r=new Array(e);n<e;n++)r[n]=t[n];return r}function x(t,e){for(var n=0;n<e.length;n++){var r=e[n];r.enumerable=r.enumerable||!1,r.configurable=!0,"value"in r&&(r.writable=!0),Object.defineProperty(t,r.key,r)}}function C(t,e,n){return(C="undefined"!=typeof Reflect&&Reflect.get?Reflect.get:function(t,e,n){var r=function(t,e){for(;!Object.prototype.hasOwnProperty.call(t,e)&&null!==(t=R(t)););return t}(t,e);if(r){var o=Object.getOwnPropertyDescriptor(r,e);return o.get?o.get.call(n):o.value}})(t,e,n||t)}function j(t,e){return(j=Object.setPrototypeOf||function(t,e){return t.__proto__=e,t})(t,e)}function D(t){var e=function(){if("undefined"==typeof Reflect||!Reflect.construct)return!1;if(Reflect.construct.sham)return!1;if("function"==typeof Proxy)return!0;try{return Date.prototype.toString.call(Reflect.construct(Date,[],(function(){}))),!0}catch(t){return!1}}();return function(){var n,r=R(t);if(e){var o=R(this).constructor;n=Reflect.construct(r,arguments,o)}else n=r.apply(this,arguments);return P(this,n)}}function P(t,e){return!e||"object"!==w(e)&&"function"!=typeof e?function(t){if(void 0===t)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return t}(t):e}function R(t){return(R=Object.setPrototypeOf?Object.getPrototypeOf:function(t){return t.__proto__||Object.getPrototypeOf(t)})(t)}var M=function(t){!function(t,e){if("function"!=typeof e&&null!==e)throw new TypeError("Super expression must either be null or a function");t.prototype=Object.create(e&&e.prototype,{constructor:{value:t,writable:!0,configurable:!0}}),e&&j(t,e)}(u,t);var e,n,i,a=D(u);function u(t,e){return function(t,e){if(!(t instanceof e))throw new TypeError("Cannot call a class as a function")}(this,u),a.call(this,t,e)}return e=u,(n=[{key:"legend",value:function(t){return t?(t.on("rendered",(function(t){return t.style("pointer-events","all")})),C(R(u.prototype),"legend",this).call(this,t)):C(R(u.prototype),"legend",this).call(this)}},{key:"plotData",value:function(){var t=this;this._calculateBarWidth(),this.data().forEach((function(e,n){return t._renderBars(n,e)})),this.renderLabel()&&this._renderLabels(this.data()),this.drawCanvas()}},{key:"legendHighlight",value:function(t){this.updateShapes((function(e){return 0===e.type?{alpha:o.color(e.color).hex()===o.color(t.color).hex()?1:.2}:e})),this.drawCanvas(0),C(R(u.prototype),"legendHighlight",this).call(this,t)}},{key:"legendReset",value:function(t){this.updateShapes((function(t){return 0===t.type?{alpha:1}:t})),this.drawCanvas(0),C(R(u.prototype),"legendHighlight",this).call(this,t)}},{key:"onClick",value:function(t){this.stack().length>1||C(R(u.prototype),"onClick",this).call(this,t)}},{key:"initializeData",value:function(t){switch(t.type){case 0:return{y:this.effectiveHeight(),height:0};case 1:return{y:this.effectiveHeight()}}}},{key:"listenForMouseEvents",value:function(){return this.isOrdinal()}},{key:"onMouseOverCanvas",value:function(t,e,n){return n.x<=t&&t<=n.x+n.width&&n.y<=e&&e<=n.y+n.height}},{key:"drawShapeOnCanvas",value:function(t,e){switch(e.type){case 0:var n=this._barXPos(e.value),r=this._barYPos(e.value),o=this._barWidth,i=this._barHeight(e.value),a=this._barColorFunction()(e.key,e.value);t.globalAlpha=e.hover?.5:e.alpha,t.fillStyle=a,t.fillRect(n,r,o,i),t.globalAlpha=1;break;case 1:var u="number"==typeof e.text?Math.round(e.text):e.text,c=(e.width-t.measureText(u).width)/2;t.font="14px Helvetica Neue",t.fillStyle="#000",t.fillText(u,e.x+c,e.y)}}},{key:"_renderBars",value:function(t,e){var n,r=k(e.values);try{for(r.s();!(n=r.n()).done;){var o=n.value,i=this.keyAccessor()(o.data);this.updateShape("".concat(t,"-").concat(i),{type:0,key:i,value:o,alpha:1},o)}}catch(t){r.e(t)}finally{r.f()}}},{key:"_barColorFunction",value:function(){var t=this;return this.isOrdinal()?this.hasFilter()?function(e,n){return t.hasFilter(n.x)?t.getColor(n):"#cccccc"}:function(e,n){return t.getColor(n)}:this.brushOn()||this.parentBrushOn()?this.filter()?function(e,n){return t.filter().isFiltered(e)?t.getColor(n):"#cccccc"}:function(e,n){return t.getColor(n)}:void 0}},{key:"_barXPos",value:function(t){return C(R(u.prototype),"_barXPos",this).call(this,t)+this.margins().left}},{key:"_barYPos",value:function(t){var e=this.y()(t.y+t.y0);return t.y<0&&(e-=this._barHeight(t)),r.utils.safeNumber(e)+this.margins().top}},{key:"_renderLabels",value:function(t){var e,n=k(t[t.length-1].values);try{for(n.s();!(e=n.n()).done;){var r=e.value,o=this.keyAccessor()(r.data),i=this.label()(r);this.updateShape("text-".concat(o),{type:1,x:this._barXPos(r),y:this._barYPos(r)-2,width:this._barWidth,text:i})}}catch(t){n.e(t)}finally{n.f()}}}])&&x(e.prototype,n),i&&x(e,i),u}(function(t){return function(t){!function(t,e){if("function"!=typeof e&&null!==e)throw new TypeError("Super expression must either be null or a function");t.prototype=Object.create(e&&e.prototype,{constructor:{value:t,writable:!0,configurable:!0}}),e&&g(t,e)}(u,t);var e,n,i,a=_(u);function u(t,e){var n;return function(t,e){if(!(t instanceof e))throw new TypeError("Cannot call a class as a function")}(this,u),(n=a.call(this,t,e))._context=null,n._timer=null,n._easing=r.Canvas.easeInQuart,n._originalData={},n._currentData={},n._nextData={},n._lastHoverKey=null,n._animating=!1,n._clientX=NaN,n._clientY=NaN,n._initialized=!1,n}return e=u,(n=[{key:"easing",value:function(t){return arguments.length?(this._easing=t,this):this._easing}},{key:"drawCanvas",value:function(t){var e=this;this._timer&&this._timer.stop(),0===t?this._drawShapes(1):(t=t||this.transitionDuration()||500,this._animating=!0,this._timer=o.timer((function(n){var r=Math.min(n/t,1);e._drawShapes(r),1===r&&(e._timer.stop(),e._animating=!1,e.listenForMouseEvents()&&e.canvas().dispatchEvent(new Event("mousemove")))}),5))}},{key:"_drawShapes",value:function(t){for(var e in this.clearCanvas(),this._nextData)this._currentData[e]=this._createInterpolatedData(e,t),this.drawShapeOnCanvas(this._context,this._currentData[e])}},{key:"_drawChart",value:function(t){this.hasContext()||this.resetCanvas(),b(O(u.prototype),"_drawChart",this).call(this,t),this.svg().style("position","relative").style("pointer-events","none")}},{key:"updateShape",value:function(t,e,n){var r=this;this._currentData[t]||(this._currentData[t]=Object.assign(this.initializeData(e),e)),this._nextData[t]={},Object.keys(e).forEach((function(n){r._nextData[t][n]=o.interpolate(r._currentData[t][n],e[n])})),void 0!==n&&(this._originalData[t]=n)}},{key:"updateShapes",value:function(t){for(var e in this._currentData){var n=t(this._currentData[e]);this.updateShape(e,n)}}},{key:"_createInterpolatedData",value:function(t,e){e=this._easing(e);var n=Object.assign({},this._currentData[t]);for(var r in this._nextData[t])n[r]=this._nextData[t][r](e);return n}},{key:"resetCanvas",value:function(){this.select("canvas").remove(),this.root().style("position","relative"),this.svg().style("position","relative").style("pointer-events","none");var t=window.devicePixelRatio||1,e=this._getMargins(),n=parseFloat(this.svg().attr("width"))-e.right,r=parseFloat(this.svg().attr("height"))-e.bottom,o=this.root().insert("canvas",":first-child").attr("x",0).attr("y",0).attr("width",n*t).attr("height",r*t).style("width","".concat(n,"px")).style("height","".concat(r,"px")).style("position","absolute").style("pointer-events","auto").node().getContext("2d");o.scale(t,t),o.rect(0,0,n,r),o.clip(),o.imageSmoothingQuality="high",this._context=o,this.listenForMouseEvents()&&(this.canvas().addEventListener("mousemove",this._canvasMouseMove.bind(this)),this.canvas().addEventListener("mouseenter",this._canvasMouseMove.bind(this)),this.canvas().addEventListener("mouseleave",this._canvasMouseLeave.bind(this)),this.canvas().addEventListener("click",this._canvasMouseClick.bind(this))),this._initialized=!0}},{key:"_resize",value:function(){var t=this._getMargins(),e=parseFloat(this.svg().attr("width"))-t.right,n=parseFloat(this.svg().attr("height"))-t.bottom;this.root().select("canvas").attr("width",e).attr("height",n).style("width","".concat(e,"px")).style("height","".concat(n,"px"))}},{key:"_getMargins",value:function(){return this.margins?this.margins():{top:0,right:0,bottom:0,left:0}}},{key:"_canvasMouseMove",value:function(t){var e=this;if(this._clientX=t.clientX||this._clientX,this._clientY=t.clientY||this._clientY,!this._animating){var n=p(this._getCoordinate(t),2),r=n[0],o=n[1];Object.keys(this._currentData).filter((function(t){return e.onMouseOverCanvas(r,o,e._currentData[t])})).forEach((function(t){e._lastHoverKey&&e.updateShape(e._lastHoverKey,{hover:!1}),e.updateShape(t,{hover:!0}),e.drawCanvas(0),e._lastHoverKey=[t]}))}}},{key:"_canvasMouseLeave",value:function(){this._clientX=NaN,this._clientY=NaN,this._lastHoverKey&&this.updateShape(this._lastHoverKey,{hover:!1}),this.drawCanvas(0)}},{key:"_canvasMouseClick",value:function(t){var e=this,n=p(this._getCoordinate(t),2),r=n[0],o=n[1];Object.keys(this._currentData).filter((function(t){return e.onMouseOverCanvas(r,o,e._currentData[t])})).forEach((function(t){return e.onClick(e._originalData[t])}))}},{key:"_getCoordinate",value:function(t){var e=t.target.getBoundingClientRect();return[t.clientX-e.left,t.clientY-e.top]}},{key:"clearCanvas",value:function(){this.context().clearRect(0,0,this.canvas().width+2,this.canvas().height+2)}},{key:"context",value:function(){return this._context}},{key:"canvas",value:function(){return this._context.canvas}},{key:"hasContext",value:function(){return!!this._context}},{key:"initializeData",value:function(){throw new Error("Not implemented")}},{key:"drawShapeOnCanvas",value:function(){throw new Error("Not implemented")}},{key:"onMouseOverCanvas",value:function(){return null}},{key:"listenForMouseEvents",value:function(){return!1}},{key:"render",value:function(){var t=b(O(u.prototype),"render",this).call(this);return this._resize(),t}}])&&d(e.prototype,n),i&&d(e,i),u}(t)}(r.BarChart)),E={linear:function(t){return t},easeInQuad:function(t){return t*t},easeOutQuad:function(t){return t*(2-t)},easeInOutQuad:function(t){return t<.5?2*t*t:(4-2*t)*t-1},easeInCubic:function(t){return t*t*t},easeOutCubic:function(t){return--t*t*t+1},easeInOutCubic:function(t){return t<.5?4*t*t*t:(t-1)*(2*t-2)*(2*t-2)+1},easeInQuart:function(t){return t*t*t*t},easeOutQuart:function(t){return 1- --t*t*t*t},easeInOutQuart:function(t){return t<.5?8*t*t*t*t:1-8*--t*t*t*t},easeInQuint:function(t){return t*t*t*t*t},easeOutQuint:function(t){return 1+--t*t*t*t*t},easeInOutQuint:function(t){return t<.5?16*t*t*t*t*t:1+16*--t*t*t*t*t},BarChart:M};r.Canvas=Object.assign({BarChart:M,barChart:function(t,e){return new r.Canvas.BarChart(t,e)}},E),r.legend=function(){return new h}}]);
//# sourceMappingURL=dc.canvas.js.map