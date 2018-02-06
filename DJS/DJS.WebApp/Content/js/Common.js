
function htmlEncode(str) {
    var ele = document.createElement('span');
    ele.appendChild(document.createTextNode(str));
    return ele.innerHTML;
}
function htmlDecode(str) {
    var ele = document.createElement('span');
    ele.innerHTML = str;
    return ele.textContent;
}

function actionIndex() {
    parent.window.location.href = "/Home/Index";
}
//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

//获取url中文参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}
//复制文本
function copyToClipboard(txt) {
    if (window.clipboardData) {
        window.clipboardData.clearData();
        clipboardData.setData("Text", txt);
        alert("复制成功！");

    } else if (navigator.userAgent.indexOf("Opera") != -1) {
        window.location = txt;
    } else if (window.netscape) {
        try {
            netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
        } catch (e) {
            alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将 'signed.applets.codebase_principal_support'设置为'true'");
        }
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
        if (!clip)
            return;
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
        if (!trans)
            return;
        trans.addDataFlavor("text/unicode");
        var str = new Object();
        var len = new Object();
        var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
        var copytext = txt;
        str.data = copytext;
        trans.setTransferData("text/unicode", str, copytext.length * 2);
        var clipid = Components.interfaces.nsIClipboard;
        if (!clip)
            return false;
        clip.setData(trans, null, clipid.kGlobalClipboard);
        alert("复制成功！");
    } else {
        alert("复制失败，浏览器不提供此功能！");
    }
}


//上传文件
function uploadFile(fileId, uploadtype, dirname) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    formData.append("fileicon", $("#" + fileId + "")[0].files[0]);
    formData.append("uploadtype", uploadtype);
    formData.append("dirname", dirname);
    $.ajax({
        type: "POST",
        url: "/Common/UploadFile",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                $.modalMsgError(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传多个文件
function uploadFiles(fileId, uploadtype, dirname) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    var upfiles = $("#" + fileId + "")[0].files;
    if (upfiles != null) {
        for (var i = 0; i < upfiles.length; i++) {
            formData.append("files" + i, upfiles[i]);
        }
        formData.append("uploadtype", uploadtype);
        formData.append("dirname", dirname);
    }
    $.ajax({
        type: "POST",
        url: "/Common/UploadFiles",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                $.modalMsgError(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

var WEBSOCKETURL = "ws://127.0.0.1:10020";

var wsrunjobnums;
function initWSRunJobNums(ele)
{
    //wsindex = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/Api/WSChat");
    wsrunjobnums = new WebSocket(WEBSOCKETURL);
    wsrunjobnums.onopen = function () {
        $.modalMsgSuccess("消息监听链接成功");
        wsrunjobnums.send("RUNJOBNUMS");
    };
    wsrunjobnums.onmessage = function (result) {
        //$.modalMsgSuccess(result.data);
        var data = eval(result.data);
        if (data.key == "RUNJOBNUMS")
        {
            $("#" + ele)[0].innerText = data.value;
        }
    };
    wsrunjobnums.onerror = function (error) {
        $.modalMsgWarning("消息监听链接失败");
    };
    wsrunjobnums.onclose = function () {
        $.modalMsgWarning("消息监听链接关闭");
    };
}
function closeWSRunJobNums()
{
    wsrunjobnums.close();
}
var wsjobpool;
function initWSJobPool() {
    //wsindex = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/Api/WSChat");
    wsjobpool = new WebSocket(WEBSOCKETURL);
    wsjobpool.onopen = function () {
        //$.modalMsgSuccess("消息监听链接成功");
        wsjobpool.send("JobPool");
    };
    wsjobpool.onmessage = function (result) {
        $.modalMsgSuccess(result.data);
    };
    wsjobpool.onerror = function (error) {
        $.modalMsgWarning("消息监听链接失败");
    };
    wsjobpool.onclose = function () {
        $.modalMsgWarning("消息监听链接关闭");
    };
}
function closeWSJobPool() {
    wsjobpool.close();
}