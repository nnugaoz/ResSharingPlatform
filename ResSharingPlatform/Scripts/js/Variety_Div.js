//eval(function (p, a, c, k, e, d) { e = function (c) { return (c < a ? '' : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36)) }; if (!''.replace(/^/, String)) { while (c--) { d[e(c)] = k[c] || e(c) } k = [function (e) { return d[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) { if (k[c]) { p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]) } } return p }('a 1F=8(l,13,14,Y,12,K){a 1l=$(m).i();a 1k=$(m).9();a P=$(m).P();a U=$(m).U();a 19=(l==L?"0":$(l).k().j);a 1c=(l==L?"0":$(l).k().d);c($("#7").Q>0){$("#7").X()}y{}$("<b o=\'7\'></b>").1G("1E").5({"z-1i":"2","Z":"1D 1B #1C","J":"I","Z-1I":"E","i":12,"d":1c,"j":19,"9":"1g"}).n("B").1M("C",8(){s w});$("<b o=\'D\'></b>").5({"1b":"1L","M":"d","i":"q%","W-1J":"d","W-1A":"1P","9":"10","1z-9":"10","H-N":14,"1q-1t":"1u"}).n("#7").11(13);$("<b o=\'g\'><u r=\'../p/1d.t\' /></b>").5({"J":"I","9":"15","i":"15","1b":"1v","M":"O","O":"1K","j":"E"}).n("#D");$("#g").V("22",8(){$("#g u").T("r","../p/1h.t")}).V("x",8(){$("#g u").T("r","../p/1h.t")}).V("1Q",8(){$("#g u").T("r","../p/1d.t")});$("<b o=\'F\'></b>").5({"9":"1g","i":"q%","M":"d","H-N":"#2b"}).n("#7");$("#F").11(Y);$("#D").29(8(3){a k=$("#7").k();1f=3.18-k.d;17=3.1e-k.j;a v=w;c(f.23&&3.1a==1){v=S}y{c(3.1a==0)v=S}c(v){$(f).x(8(3){$("#7").5("d",(3.18-1f)+"16");$("#7").5("j",(3.1e-17)+"16")})}});c($("#7").5("R").1X()!="1p"){$("B").21(8(3){$(f).x(8(3){});$(f).20("x")})}c($("#h").Q>0){$("#h").X()}y{}c(K==1Y||K){$("<b o=\'h\'><b>").5({"J":"I","d":"0","j":"0","i":"q%","9":"q%","R":" 26","z-1i":"1","H-N":"#1Z","1W":"1S(G=1R)","-1T-G":".4","G":"0.4"}).n("B").1U();c($.1m.1V&&$.1m.24=="6.0"){$("#g").5({"O":"E"});$("1n").2a($("#F 1n")).5("28","27");$("#h").5("9",$(f).9()).1j(0,0.4)}y{$("#h").5("9",$(f).9()).1j(0,0.4)}}$("#7").5({"d":U+(1l-$("#7").i())/2,"j":P+(1k-$("#7").9())/2});$("#g").25(8(3){$("#7").5("R","1p");c($("#h").Q>0){$("#h").1w()}f.C=8(){}});f.C=8(3){a 3=(3==L)?m.3:3;1x{a A=3.1y;c(!((A.1o=="1r"&&A.1s.1O()=="W")||A.1o=="1N")){s w}s S}1H(e){s w}}};', 62, 136, '|||event||css||moveDiv|function|height|var|div|if|left||document|closeDiv|maskDiv|width|top|offset|obj|window|appendTo|id|Images|100|src|return|gif|img|witchButton|false|mousemove|else||the|body|onselectstart|headDiv|4px|bodyDiv|opacity|background|absolute|position|isShowMask|null|float|color|right|scrollTop|length|display|true|attr|scrollLeft|live|text|remove|content|border|29px|html|widthDiv|title|titleBGC|20px|px|y1|clientX|ot|button|cursor|ol|closefff|clientY|x1|auto|closelightf00|index|fadeTo|windowHeight|windowWidth|browser|select|tagName|none|font|INPUT|type|weight|800|pointer|hide|try|srcElement|line|indent|solid|cdcdcd|3px|slow|VarietyDiv|fadeIn|catch|radius|align|14px|move|bind|TEXTAREA|toLowerCase|10px|mouseleave|50|alpha|moz|show|msie|filter|toString|undefined|ccc|unbind|mouseup|mouseover|all|version|click|block|hidden|visibility|mousedown|not|fff'.split('|'), 0, {}))
///*********
/// 层
/// <param name="obj">this对象</param>
/// <param name="title">标题</param>
/// <param name="titleBGC">标题颜色</param>
/// <param name="content">内容</param>
/// <param name="widthDiv">宽度</param>
/// <param name="isShowMask">是否有遮罩</param>
/// 制作人：何青青（heqq）
/// 邮箱：529349150@qq.com
///********* $("#closeDiv").click();
var VarietyDiv = function (obj, title, titleBGC, content, widthDiv, isShowMask) {
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();
    var scrollTop = $(window).scrollTop();
    var scrollLeft = $(window).scrollLeft();
    var ot = (obj == null ? "0" : $(obj).offset().top);
    var ol = (obj == null ? "0" : $(obj).offset().left);
    if ($("#moveDiv").length > 0) {
        $("#moveDiv").remove()
    } else { }
    $("<div id='moveDiv'></div>").fadeIn("slow").css({
        "z-index": "10003",
        "border": "2px solid #989898",
        "position": "absolute",
        "border-radius": "4px",
        "width": widthDiv,
        "left": ol,
        "top": ot,
        "height": "auto"
    }).appendTo("body").bind("onselectstart",
    function () {
        return false
    });
    $("<div id='headDiv'></div>").css({
        "cursor": "move",
        "float": "left",
        "width": "100%",
        "text-align": "left",
        "text-indent": "10px",
        "height": "32px",
        "line-height": "32px",
        "border-bottom": "#c8dde9 solid 1px",
        //"background": "url('../../Images/bg.png')",
        "background-color": titleBGC,
        "color": "#333",
        "font-size": "14px",
        "font-weight": "800"
    }).appendTo("#moveDiv").html(title);
    $("<div id='_closeDiv'><img src='../../Images/close_fff1.png' /></div>").css({
        "position": "absolute",
        "height": "15px",
        "width": "15px",
        "cursor": "pointer",
        "float": "right",
        "right": "18px",
        "top": "3px"
    }).appendTo("#headDiv");
    $("#_closeDiv").bind("mouseover",
    function () {
        $("#_closeDiv img").attr("src", "../../Images/close_fff2.png").attr("title", "关闭")
    }).bind("mousemove",
    function () {
        $("#_closeDiv img").attr("src", "../../Images/close_fff2.png").attr("title", "关闭")
    }).bind("mouseleave",
    function () {
        $("#_closeDiv img").attr("src", "../../Images/close_fff1.png").attr("title", "关闭")
    });
    $("<div id='bodyDiv'></div>").css({
        "height": "auto",
        "width": "100%",
        "float": "left",
        "background-color": "#fff"
    }).appendTo("#moveDiv");
    $("#bodyDiv").html(content);
    $("#headDiv").mousedown(function (event) {
        var offset = $("#moveDiv").offset();
        x1 = event.clientX - offset.left;
        y1 = event.clientY - offset.top;
        var witchButton = false;
        if (document.all && event.button == 1) {
            witchButton = true
        } else {
            if (event.button == 0) witchButton = true
        }
        if (witchButton) {
            $(document).mousemove(function (event) {
                $("#moveDiv").css("left", (event.clientX - x1) + "px");
                $("#moveDiv").css("top", (event.clientY - y1) + "px")
            })
        }
    });
    if ($("#moveDiv").css("display").toString() != "none") {
        $("body").mouseup(function (event) {
            $(document).mousemove(function (event) { });
            $(document).unbind("mousemove")
        })
    }
    if ($("#maskDiv").length > 0) {
        $("#maskDiv").remove()
    } else { }
    if (isShowMask == undefined || isShowMask) {
        $("<div id='maskDiv'><div>").css({
            "position": "absolute",
            "left": "0",
            "top": "0",
            "width": "100%",
            "height": "100%",
            "display": " block",
            "z-index": "10000",
            "background-color": "#A6A6A6",
            "filter": "alpha(opacity=50)",
            "-moz-opacity": ".4",
            "opacity": "0.4"
        }).appendTo("body").show();
        if ($.browser.msie && $.browser.version == "6.0") {
            $("#_closeDiv").css({
                "right": "4px"
            });
            $("select").not($("#bodyDiv select")).css("visibility", "hidden");
            $("#maskDiv").css("height", $(document).height()).fadeTo(0, 0.4)
        } else {
            $("#maskDiv").css("height", $(document).height()).fadeTo(0, 0.4)
        }
    }
    $("#moveDiv").css({
        "left": scrollLeft + (windowWidth - $("#moveDiv").width()) / 2,
        "top": scrollTop + (windowHeight - $("#moveDiv").height()) / 2
    });
    $("#_closeDiv").click(function (event) {
        $("#moveDiv").css("display", "none");
        if ($("#maskDiv").length > 0) {
            $("#maskDiv").hide()
        }
        document.onselectstart = function () { }
    });
    document.onselectstart = function (event) {
        var event = (event == null) ? window.event : event;
        try {
            var the = event.srcElement;
            if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
                return false
            }
            return true
        } catch (e) {
            return false
        }
    }
};