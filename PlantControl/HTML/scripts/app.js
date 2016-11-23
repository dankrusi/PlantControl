
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) == str;
    };
}

if (typeof String.prototype.endsWith != 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) == str;
    };
}

if (typeof String.prototype.contains != 'function') {
    String.prototype.contains = function (str) {
        return this.indexOf(str) > -1;
    };
}

String.prototype.padLeft = function (length, character) {
    return new Array(length - this.length + 1).join(character || '0') + this;
}

String.prototype.replaceAll = function (replace, withThis) {
    return this.replace(new RegExp(replace, 'g'), withThis);
}

String.prototype.capitalize = function () {
    return this[0].toUpperCase() + this.substring(1);
}

String.prototype.replaceNonAscii = function (withThis) {
    var ret = this;
    ret = ret.replaceAll("\\.", withThis);
    ret = ret.replaceAll("-", withThis);
    ret = ret.replaceAll(" ", withThis);
    return ret;
}




var PlantControl = {

	DEBUG_ENABLED: true,

	log: function log(str) {
		if(PlantControl.DEBUG_ENABLED == true) console.log(str);
	},

	init: function init() {
		// Load all app data element
		$("[app-data]").each(function() {
			PlantControl.loadAppDataForElement($(this));
		});
	},

	getQuery: function getQuery(name, url) {
	    if (!url) {
	      url = window.location.href;
	    }
	    name = name.replace(/[\[\]]/g, "\\$&");
	    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
	        results = regex.exec(url);
	    if (!results) return null;
	    if (!results[2]) return '';
	    return decodeURIComponent(results[2].replace(/\+/g, " "));
	},

	loadAppDataForElement: function loadAppDataForElement(elem) {
		// Save template and remove it from DOM
		var template = elem.html();
		elem.children().remove();
		// Get data
		var params = null;
		if(elem.attr("auto-param") == "true") {
			var match,
		        pl     = /\+/g,  // Regex for replacing addition symbol with a space
		        search = /([^&=]+)=?([^&]*)/g,
		        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
		        query  = window.location.search.substring(1);

		    params = {};
		    while (match = search.exec(query)) {
		    	params[decode(match[1])] = decode(match[2]);
		    }
		}
		// Make API call
		PlantControl.apiCall(elem.attr("app-data"),params,function(success){
			// Loop array of obj in success
			$(success).each(function(){
				var newTemplate = template;
				// Loop all keys
				var keys = Object.keys(this);
				for (var i = 0; i < keys.length; i++) {
					newTemplate = newTemplate.replaceAll("{"+keys[i].toLowerCase()+"}",this[keys[i]]);
				}
				elem.append($(newTemplate));
			});
			// Show
			elem.show();
		});
	},

	showAlert: function showAlert(okay,cancel,message,onClose,onOkay,onCancel) {
		var buttonSet = {};
        if(okay) buttonSet[okay] = function(){$(this).dialog("close");if(onOkay)onOkay();if(onClose)onClose(okay);};
        if(cancel) buttonSet[cancel] = function(){$(this).dialog("close");if(onCancel)onCancel();if(onClose)onClose(cancel);};

	    $( "<p>"+message+"</p>" ).dialog({
	      resizable: false,
	      height: "auto",
	      width: 400,
	      modal: true,
	      buttons: buttonSet
	    });
	},

	showMessage: function showAlert(message) {
		PlantControl.showAlert("Okay",null,message);
	},

	createCookie: function createCookie(name,value,days) {
	    if (days) {
	        var date = new Date();
	        date.setTime(date.getTime()+(days*24*60*60*1000));
	        var expires = "; expires="+date.toGMTString();
	    }
	    else var expires = "";
	    document.cookie = name+"="+value+expires+"; path=/";
	},

	readCookie: function readCookie(name) {
	    var nameEQ = name + "=";
	    var ca = document.cookie.split(';');
	    for(var i=0;i < ca.length;i++) {
	        var c = ca[i];
	        while (c.charAt(0)==' ') c = c.substring(1,c.length);
	        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	    }
	    return null;
	},

	eraseCookie: function eraseCookie(name) {
	    PlantControl.createCookie(name,"",-1);
	},

	apiCall: function apiCall(call, params, onSuccess, onError) {
		PlantControl.log("API "+call);

		var url = "/"+call;
		var dataType = "json";
		var method = "GET";

		// Send off request
	    $.ajax({
	        type: method,
	        url: url,
	        data: params,
	        success: function (data) {
	            PlantControl.log("API success");
	            PlantControl.log(data);
	            if(onSuccess) onSuccess(data);
	        },
	        error: function (xhr, status, errorThrown) {
	            PlantControl.log("API error: " + errorThrown);
	            // Server error
	            var message = JSON.parse(xhr.responseText);
	            PlantControl.log(message);
	            PlantControl.showAlert("Okay",null,message.Description,function(button){
	            	if(message.ErrorCode == "NotAuthenticatedException") {
	            		window.location = "/login";
	            	}
	            },null);

	        },
	        dataType: dataType
	    });
	}


};



$(document).ready(function(){
	PlantControl.init();
});