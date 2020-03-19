<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    资源平台-学校列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <script src="../../Scripts/layer/layer.min.js" type="text/javascript"></script>
    <link href="../../Content/comit/font/iconfont.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tr_select
        {
            background: #0066ff;
        }
    </style>
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
        }
        body
        {
            font-size: 12px;
            font-family: '微软雅黑';
            min-width: 1100px;
            background-color: #3385b6;
            
        }
        ul
        {
            display: block;
            margin: 0;
            padding: 0;
            list-style: none;
        }
        li
        {
            display: block;
            margin: 0;
            padding: 0;
            list-style: none;
        }
        img
        {
            border: 0;
        }
        a, a:focus
        {
            text-decoration: none;
            color: #000;
            outline: none;
        }
        a:hover
        {
            color: #000;
            text-decoration: none;
        }
        .top
        {
            height: 47px;
            position: absolute;
            top: 0;
            background: url(../../Content/comit/images/top-bg.png) repeat-x;
            z-index: 100;
            width: 100%;
            font-size: 13px;
        }
        .top span
        {
            color: #def0f6;
            line-height: 47px;
            padding-left: 50px;
            color: #afc5d2;
            float: left;
        }
        .list
        {
            display: block;
            width: 580px;
            height: 160px;
            margin-top:25px;
            margin-left:5px;
        }
        .list li
        {
            float: left;
            padding: 0px 20px;
        }
        .list li a
        {
            display: block;
            width: 140px;
            height: 120px;
            padding: 20px;
            text-align: center;
            cursor: pointer;
            border-radius: 4px;
            -webkit-box-shadow: 0 10px 30px rgba(0, 0, 0, .2);
            -moz-box-shadow: 0 10px 30px rgba(0, 0, 0, .2);
            box-shadow: 0 10px 30px rgba(0, 0, 0, .2);
        }
        .list li.one a
        {
            background-color: rgba(186,52,46,.9);
        }
        .list li.two a
        {
            background-color: rgba(77,110,183,.9);
        }
        .list li.three a
        {
            background-color: rgba(244,130,62,.9);
        }
        .list li.four a
        {
            background-color: rgba(127,73,95,.9);
        }
        .list li.five a
        {
            background-color: rgba(71,185,133,.9);
        }
        .list li.one:hover a
        {
            background-color: rgba(186,52,46,1);
        }
        .list li.two:hover a
        {
            background-color: rgba(77,110,183,1);
        }
        .list li.three:hover a
        {
            background-color: rgba(244,130,62,1);
        }
        .list li.four:hover a
        {
            background-color: rgba(127,73,95,1);
        }
        .list li.five:hover a
        {
            background-color: rgba(71,185,133,1);
        }
        .list li a span.iconfont
        {
            display: inline-block;
            width: 100%;
            height: 80px;
            text-align: center;
            line-height: 60px;
            color: #fff;
            font-size: 50px;
        }
        .list li a p
        {
            display: inline-block;
            font-size: 14px;
            color: #fff;
            line-height: 0px;
        }
    </style>
    <%--JS初始化--%>
    <script type="text/javascript">

        $(function () {
            getSchoolList()
            //初始化取数据
            GetInitList();
        });
    </script>
    <%--Ajax数据交互--%>
    <script type="text/javascript">
        // 初始化学校数据列表
        function getSchoolList() {
            $.ajax({
                type: 'post',
                url: "../../Teacher/GetSchoolList",
                dataType: 'json',
                success: function (data) {
                    var html = template('schoolList', data);
                    $("#schoolId").append(html);
                }
            });
        }
        // 初始化数据列表
        function GetInitList() {
            app.ajaxForm("../../Teacher/GetTeacherList", teacherCallBack);
        }

        function teacherCallBack(data) {
            var html = template('TeacherList', data);
            $("#div_list").html(html);
        }

        // 删除数据
        function ajax_delete(id) {
            $.post("../../Teacher/SaveDelete?t=" + new Date().getTime(), { id: id }, function (obj) {
                try {
                    eval("ret=" + obj + ";");
                    if (ret[0].Save == "true") {
                        layer.msg("提交成功", 1, 10, function result() {
                            //初始化取数据
                            GetInitList();
                        });
                    } else {
                        layer.alert("提交失败", 10);
                        return false;
                    }
                } catch (e) {
                    layer.alert("提交失败", 10);
                    return false;
                }
            });
        }
    </script>
    <%--页面按钮响应事件--%>
    <script type="text/javascript">
        // 查询
        function do_search() {
            //初始化取数据
            GetInitList();
        }

        function DeleteTeacher(id, name) {
            // 删除确认
            $.layer({
                shade: [0],
                area: ['auto', 'auto'],
                dialog: {
                    msg: '确认删除' + name + '吗？',
                    btns: 2,
                    type: 4,
                    btn: ['确认', '取消'],
                    yes: function () {
                        ajax_delete(id);
                        return false;
                    }, no: function () {
                        layer.msg('删除取消', 1, 1);
                        return false;
                    }
                }
            });
        }
        //编辑
        function UpdateTeacher(userid) {
            location.href = "../Teacher/Edit?t=" + new Date().getTime() + "&userid=" + userid;
        }
        //编辑
        function AddTeacher() {
            location.href = "../Teacher/Add?t=" + new Date().getTime();
        }





        function TeacherDetail(userid, name) {



            $.ajax({
                type: 'post',
                url: "../../Teacher/GetTeacherPoint?userid=" + userid,
                dataType: 'json',
                success: function (data) {

                    //alert(data.ResourcePoint);
                    //alert(data.BbsPoint);

                    var html = "";
                    //html = html = "<div style=\"background-color: #3385b6;\">";
                    html = html + "<ul class=\"list\">";
                    html = html + "<li class=\"one\">";
                    html = html + "<a target=\"_blank\" href=\"../Teacher/ResourceList?userid=" + userid + "\">";
                    html = html + "<span class=\"iconfont\">&#xe608;</span>";
                    html = html + "<p>资源</p>";
                    html = html + "</a>";
                    html = html + "<li class=\"two\">";
                    html = html + "<a target=\"_blank\" href=\"../Teacher/ArticleList?userid=" + userid + "\">";
                    html = html + "<span class=\"iconfont\">&#xe601;</span>";
                    html = html + "<p>文章</p>";
                    html = html + "</a>";
                    html = html + "</li>";
                    html = html + "<li class=\"three\">";
                    html = html + "<a href=\"#\">";
                    html = html + "<span class=\"iconfont\" style=\"height: 40px;\"><p style=\"margin-top:30px;\">资源积分：" + data.ResourcePoint + "</p></span>";
                    html = html + "<p style=\"margin-top:10px;\">BBS积分：" + data.BbsPoint + "</p>";
                    html = html + "</a>";
                    html = html + "</li>";
                    html = html + "</ul>";
                    //html = html = "</div>";

                    $.layer({
                        type: 1,
                        title: [name, true], //不显示默认标题栏
                        shade: [1], //显示遮罩
                        area: ["550px", "200px"],
                        shift: 'left', //从左动画弹出
                        page: { html: html }
                    });
                }
            });

        }

        function AddCms() {
            var flag = false;
            $('input[name="isactivate"]:checked').each(function () {
                flag = true;
            });

            if (flag == false) {
                layer.alert("请选择要激活的老师账户", 5);
                return false;
            }

            app.ajaxForm("../../Teacher/AddCmsList", cmsCallBack);
        }

        function cmsCallBack(data) {
            if (data.result == "success") {
                layer.alert("激活成功！", "1");
                //初始化取数据
                GetInitList();
            } else {
                layer.alert("激活失败！", 5);
            }
        }


        function AddMonitor() {
            var flag = false;
            $('input[name="isactivate"]:checked').each(function () {
                flag = true;
            });

            if (flag == false) {
                layer.alert("请选择要激活的老师账户", 5);
                return false;
            }

            app.ajaxForm("../../Teacher/AddMonitorList", monitorCallBack);
        }

        function monitorCallBack(data) {
            if (data.result == "success") {
                layer.alert("激活成功！", "1");
                //初始化取数据
                GetInitList();
            } else {
                layer.alert("激活失败！", 5);
            }
        }
    </script>
    <!--学校列表-->
    <script id="schoolList" type="text/html">
    {{each list as value}}
        <option value="{{=value.recordid}}">{{=value.schoolname}}</option>
    {{/each}}
    </script>
    <!--学校列表-->
    <!--学校列表-->
    <script id="TeacherList" type="text/html">
<table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
    <thead>
        <tr>
           
            <th width="50px">
                序号
            </th>
            <th width="50px">
                
            </th>
            <th width="300px">
                教师名
            </th>
            <th>
                所属学校
            </th>
            <th>
                CMS账户是否激活
            </th>
             <th>
                监控账户是否激活
            </th>
            <th>
                操作
            </th>
        </tr>
    </thead>
    <tbody id="tbody">
       {{each list as value}}
    <tr> 
        <td>{{=value.id0}}</td>
        <td>
            <input type="checkbox" value="{{=value.id}}" name="isactivate">
        </td>
        <td>
            <a style="text-decoration: none;" onclick="TeacherDetail('{{=value.id}}','{{=value.name}}');" title="教师详情">{{=value.name}}</a>
        </td>
        <td>{{=value.schoolname}}</td>
        <td style="text-align:center;">
        {{if value.iscmsadmin =="1"}}
            <font style="color:blue;">已激活</font>
        {{/if}}
        </td>
        <td style="text-align:center;">
        {{if value.ismonitoradmin =="1"}}
            <font style="color:blue;">已激活</font>
        {{/if}}
        </td>
        <td>
            <a style="text-decoration: none;" onclick="UpdateTeacher('{{=value.id}}');" title="修改">修改</a>
            <a style="text-decoration: none;" onclick="DeleteTeacher('{{=value.id}}','{{=value.name}}');" title="删除">删除</a>
        </td>
    </tr>
{{/each}}
    </tbody>
</table> 
<div style="text-align: right;" class="yahoo">
   {{=linkpage}}
</div>

    </script>
    <!--学校列表-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="actionform" action="" class="form-horizontal ui-formwizard" method="post">
     <div class="con-title">
               <h1 class="con-title-h1">
                教师查询</h1>
            <div class="con-title-btns">
               
            </div>
            </div>
    <div class="box">
     
        <div class="graup">
            <select style="width: 200px;" name="schoolId" id="schoolId">
                <option value="">请选择学校</option>
            </select>
            教师名称 &nbsp;&nbsp;
            <input id="TeacherName" name="TeacherName" type="text" />
            &nbsp;&nbsp;
            <input type="button" value="查询" onclick="do_search()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="新增" onclick="AddTeacher()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="激活CMS账户" onclick="AddCms()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="激活监控账户" onclick="AddMonitor()" class="btn btn-primary btn-sm" />
        </div>
        <div id="div_list">
        </div>
    </div>
    </form>
</asp:Content>
