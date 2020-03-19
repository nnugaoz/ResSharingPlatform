var app =
{
    //ajax提交表单
    ajaxForm: function () {

        var sum = 0;
        var url;
        var debug;
        var callback;
        var actionfrom;
        sum = arguments.length;

        if (sum == 2) {
            url = arguments[0];
            callback = arguments[1];
            actionfrom = "actionform";
        }

        if (sum == 3) {
            url = arguments[0];
            callback = arguments[1];
            debug = arguments[2]
            actionfrom = "actionform";
            if (debug == true) {
                $("#" + actionfrom).attr("action", url);
                $("#" + actionfrom).submit();
                return false;
            }
        }

        if (sum == 4) {
            url = arguments[0];
            callback = arguments[1];
            debug = arguments[2]
            actionfrom = arguments[3];
            if (debug == true) {
                $("#" + actionfrom).attr("action", url);
                $("#" + actionfrom).submit();
                return false;
            }
        }
        //alert(url+"/"+callback+"/"+actionfrom);
        callback = callback || function () { };

        $("#" + actionfrom).ajaxSubmit({
            type: "post",  //提交方式
            dataType: "json", //数据类型
            url: url, //请求url
            success: function (data) { //提交成功的回调函数
                callback(data);
            }
        });
        return false; //不刷新页面

    },
    //提示消息
    msg: function (title) {
        //提示层
        layer.msg(title);
    },
    //跳转页面
    redirect: function (url) {
        window.location.href = url;
    },
    //跨域jsonp
    jspon: function (urls,queuecount, userid, password, name,schoolid,schoolname, callback) {
        callback = callback || function () { }; 
            jQuery.ajax({
                url: urls, //单点登录的报表服务器  
                dataType: "jsonp", //跨域采用jsonp方式  
                jsonp: "callback",
                crossDomain: true,
                data: { "userid": userid, "password": password, "name": name, "flag": queuecount, "schoolid": schoolid, "schoolname": schoolname },
                timeout: 5000, //超时时间（单位：毫秒）
                success: function (data) {
                    callback(data,name);
                },
                error: function () {
                    // 登出失败（超时或服务器其他错误）  
                    app.msg("登录失败（请求超时）");
                }
            });

        }
//    //跨域jsonp
//    jspon: function (urls, userid, password, name, callback) {
//        callback = callback || function () { };
//        var strs = new Array(); //定义一数组 
//        strs = urls.split(";"); //字符分割 
//        var queue = new Array();
//        for (i = 0; i < strs.length; i++) {
//            var strurls = new Array(); //定义一数组 
//            strurls = strs[i].split(","); //字符分割
//            var flag = strurls[1];
//            //            if (strurls[1] == "true") {
//            //                flag = true;
//            //            } 
//            
//            jQuery.ajax({
//                url: strurls[0], //单点登录的报表服务器  
//                dataType: "jsonp", //跨域采用jsonp方式  
//                jsonp: "callback",
//                crossDomain: true,
//                data: { "userid": userid, "password": password, "name": name, "flag": flag },
//                timeout: 5000, //超时时间（单位：毫秒）
//                success: function (data) {
//                    callback(data);
//                },
//                error: function () {
//                    // 登出失败（超时或服务器其他错误）  
//                }
//            });

//        }
//    }

}
		
 