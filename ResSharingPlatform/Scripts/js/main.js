function goBack() {
    window.location.href = "../../Upload/Index";
}

//在线预览附件
function viewOnline(fileName) {
    window.open("../../Upload/ViewOnline?fileName=" + fileName);
}

function openPop(e) {
    var x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
    var y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
    $("#xingZQYTree").attr("style", "display:display;width:auto;z-index:9999");
    $("#xingZQYTree").attr("style", "top:" + y + "px;left:" + x + "px");
}

function closePop(id) {
    $("#" + id).attr("style", "display:none");
    if (id == "xingZQYTree") {
        $("#resType").val("");
    }
}

//function seachLabel(e) {
//    var x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
//    var y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
//    $("#searchLabel").css("display", "block");
//    $("#searchLabel").css("z-index", "9999");
//    $("#searchLabel").css("top", y + "px");
//    $("#searchLabel").css("left", x + "px");

//    getLabel();
//}

//function closeDiv(e,id) {
//    if ($("#" + id).css("display") != "none") {
//        var width = $("#" + id).width();
//        var height = $("#" + id).height();
//        var top = $("#" + id).offset().top;
//        var left = $("#" + id).offset().left;
//        var toright = left + width;
//        var tobottom = top + height;
//        if ((e.pageX < left || e.pageX > toright) || (e.pageY < top || e.pageY > tobottom)) {
//            $("#" + id).attr("style", "display:none");
//        }
//    }
//}

var EX = {
    addEvent: function (k, v) {
        var me = this;
        if (me.addEventListener)
            me.addEventListener(k, v, false);
        else if (me.attachEvent)
            me.attachEvent("on" + k, v);
        else
            me["on" + k] = v;
    },
    removeEvent: function (k, v) {
        var me = this;
        if (me.removeEventListener)
            me.removeEventListener(k, v, false);
        else if (me.detachEvent)
            me.detachEvent("on" + k, v);
        else
            me["on" + k] = null;
    },
    stop: function (evt) {
        evt = evt || window.event;
        evt.stopPropagation ? evt.stopPropagation() : evt.cancelBubble = true;
    }
};


function hide() {
    var o = document.getElementById('TagDiv');
    o.style.display = "none";
    EX.removeEvent.call(document, 'click', hide);
}

function addTags() {
    $('#TagDiv').css("display", "block");
    $('#TagDiv').css("z-index", "9999");
    $('#TagDiv').css("top", ($('#imgDiv').offset().top + $('#imgDiv').height()) + "px");
    $('#TagDiv').css("left", $('#imgDiv').offset().left + "px");
    setTimeout(function () { EX.addEvent.call(document, 'click', hide); });

    document.getElementById('TagDiv').onclick = EX.stop;

    getLabel();
}

function saveType() {
    if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
        var resType = $("#resType").val();
        if (resType == null || resType == "") {
            layer.alert("请输入分类名称！", 5);
            $("#resType").focus();
            return;
        }

        $.ajax({
            type: "post",
            url: "../../Upload/SaveResType",
            data: "resType=" + $("#resType").val() + "&father=" + $('#typeId').combotree('getValue'),
            dataType: "html",
            async: false,
            success: function (data) {
                closePop("xingZQYTree");
                if (data == "true") {
                    $('#typeId').combotree('reload');
                } else {
                    layer.alert("新建失败！", 8);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert(textStatus);
                layer.alert(errorThrown);
            }
        });
    } else {
        ExamLogin();
    }
}

//日期格式化
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
				(this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
							RegExp.$1.length == 1 ? o[k] :
								("00" + o[k]).substr(("" + o[k]).length));
    return format;
};