//动态添加select
function AddOption(Text, Value, Element) {
    var Text, Value, TemObj = document.getElementById(Element); //获取列表对象
    var TemOption = new Option(Text, Value); //定义一个选项对象
    TemObj[TemObj.length] = TemOption; //添加到列表
}
//替换特殊字符
var strReplace = function (str) {
    return str.replace(/(^\s*)|(\s*$)/g, "").replace(/,/g, "，").replace(/\'/g, "‘").replace(/\"/g, "”").replace(/\\/g, "、").replace(/\//g, "、");
}
//反 替换特殊字符
var outReplace = function (str) {
    return str.replace(/(^\s*)|(\s*$)/g, "").replace(/，/g, ",").replace(/‘/g, "'").replace(/“/g, "\"").replace(/、/g, "\\").replace(/、/g, "\/");
}
//给select控件赋值(ID,NAME)
function setSelect(json, name) {
    var num = json.num;
    $(("#" + name)).empty();
    AddOption("---请选择---", "", name);
    if (num > 0) {
        for (var i = 0; i < num; i++) {
            AddOption(json.Table[i].NAME, json.Table[i].ID, name);
        }
    }
}
//用文本 赋值Option 
var setTextOption = function (obj, text) {
    var count = $(obj).find("option").length;
    for (var i = 0; i < count; i++) {
        if ($(obj).get(0).options[i].text == text) {
            $(obj).get(0).options[i].selected = true;
            break;
        }
    }
}

//检查字数
function CheckWords(obj, num, info) {
    var reg = /(^\s*)|(\s*$)/g;
    var vals = $(obj).val().replace(/(^\s*)|(\s*$)/g, "");
    var len = vals.length;
    if (len > num) {
        $(obj).val(vals.slice(0, num));
    }
    else {
        var o = "#" + info;
        $(o).html("还可输入" + (num - len) + "个字");
    }
}

//url 值
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
$(document).keydown(function (e) {
    var target = e.target;
    var tag = e.target.tagName.toUpperCase();
    if (e.keyCode == 8) {
        if ((tag == 'INPUT' && !$(target).attr("readonly")) || (tag == 'TEXTAREA' && !$(target).attr("readonly"))) {
            if ((target.type.toUpperCase() == "RADIO") || (target.type.toUpperCase() == "CHECKBOX")) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
});
//提交Loading 层
function createLoading() {
    if ($("#Loading").length > 0) {
        $("#Loading").remove()
    }
    $("<div id='Loading'><img id='imgLoading' alt='' style='width:400px;' src='../images/loading120.gif' /><div>").css({
        "position": "absolute",
        "left": "0",
        "top": "0",
        "width": "100%",
        "height": "100%",
        "display": " block",
        "z-index": "9999",
        "background-color": "#ccc",
        "filter": "alpha(opacity=50)",
        "-moz-opacity": ".4",
        "opacity": "0.4"
    }).appendTo("body").show();
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();
    var scrollTop = $(window).scrollTop();
    var scrollLeft = $(window).scrollLeft();
    $("#imgLoading").css({
        "position": "absolute",
        "left": parseInt((windowWidth - $("#imgLoading").width()) / 2).toString() + "px",
        "top": parseInt((windowHeight - $("#imgLoading").height()) / 2).toString() + "px"
    });
}
function closeLoading() {
    $("#Loading").hide();
    $("#Loading").remove();
}

//读取cookies 
function getCookie(name, key) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg)) {
        var reg1 = new RegExp("(^|&)" + key + "=([^&]*)(&|$)", "i");
        var r1 = unescape(arr[2]).substr(1).match(reg1)
        if (r1 != null) return unescape(r1[2]); return null;
    }
    else {
        return null;
    }
}