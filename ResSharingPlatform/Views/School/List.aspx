<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    资源平台-学校列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
    <style type="text/css">
        .tr_select
        {
            background: #0066ff;
        }
    </style>
    <%--JS初始化--%>
    <script type="text/javascript">
        var txt_SchoolName = "";

        $(function () {
            //初始化取数据
            GetInitList();
        });
    </script>
    <%--Ajax数据交互--%>
    <script type="text/javascript">
        // 初始化数据列表
        function GetInitList() {
                app.ajaxForm("../../School/GetSchoolList", schoolCallBack);
            }

            function schoolCallBack(data) {
                var html = template('SchoolList', data);
                $("#div_list").html(html);
            }

        // 删除数据
        function ajax_delete(id) {
            $.post("../../School/SaveDelete?t=" + new Date().getTime(), { id: id }, function (obj) {
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
            txt_SchoolName = $("#SchoolName").val();
            //初始化取数据
            GetInitList();
        }

        // 双击
        // 进入详细页面
        function do_detail(id) {
            select_id = id;
            location.href = "../Role/Detail?t=" + new Date().getTime() + "&id=" + id;
        }

        // 新增
        function do_add() {
            location.href = "../Role/Add?t=" + new Date().getTime();
        }

        function DeleteSchool(id, name) {
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
        function UpdateSchool(SchoolId) {
            location.href = "../School/AddSchool?t=" + new Date().getTime() + "&SchoolId=" + SchoolId;
        }
    </script>


    <!--学校列表-->
        <script id="SchoolList" type="text/html">
            <table id="resTable" border="0" cellpadding="0" cellspacing="0" class="con-tab">
            <thead>
                <tr>
                    
                    <th width="50px">
                        序号
                    </th>
                    <th width="300px">
                        学校名称
                    </th>
                    <th>
                        更新时间
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
				                <td>{{=value.schoolname}}</td>
				                <td>{{=value.createtime}}</td>
				                <td>
				                    <a style="text-decoration: none;" onclick="UpdateSchool('{{=value.recordid}}');" title="修改">修改</a>
				                    <a style="text-decoration: none;" onclick="DeleteSchool('{{=value.recordid}}','{{=value.schoolname}}');" title="删除">删除</a>
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
    <form id="actionform"  class="form-horizontal ui-formwizard" method="post">
    <div class="box">
        <div class="graup">
            学校名称 &nbsp;&nbsp;
            <input id="SchoolName" name="SchoolName" type="text" />
            &nbsp;&nbsp;
            <input type="button" value="查询" onclick="do_search()" class="btn btn-primary btn-sm" />
            &nbsp;&nbsp;
            <input type="button" value="新增" onclick="UpdateSchool('')" class="btn btn-primary btn-sm" />
        </div>
        <div id="div_list">
        </div>

    </div>
    </form>
</asp:Content>
