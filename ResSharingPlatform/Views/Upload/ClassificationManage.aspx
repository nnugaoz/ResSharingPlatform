<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    分类管理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RecLink" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var html = '<div style="padding-left:0px;"><p class="arrow"></p><a style="font-size:14px;" onclick="selectType(\'资源分类\',\'\')" >资源分类</a></div>';
        $(function () {
            var list = '<%=ViewData["typeTree"]%>';
            var treeList = eval(list);
            GetJsonData(treeList, 1);
            $("#TreeDiv").html(html);

            $("#addType").click(function () {
                if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                    if ($('#addTypeName').val() == "") {
                        layer.alert("请添加分类节点！", 5);
                        $('#addTypeName').focus();
                        return;
                    }

                    $.get("../../Upload/IsExsit", { selTypeID: $('#selTypeID').val(), addTypeName: $('#addTypeName').val(), r: Math.random() }, function (data) {
                        eval("ret=" + data);
                        if (ret) {
                            layer.alert("同节点下存在此分类名，不能再添加！", 5);
                            return;
                        }
                        else {
                            $('#submitForm').attr("action", "../../Upload/AddType");
                            $('#submitForm').submit();
                        }
                    });
                }
                else {
                    ExamLogin();
                }
            });

            $("#delType").click(function () {
                if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                    if ($('#selTypeID').val() == "") {
                        layer.alert("请选择要删除的分类！", 5)
                        return;
                    }

                    /**被使用了的不能删除**/
                    $.get("../../Upload/IsUsedType", { typeId: $('#selTypeID').val(), r: Math.random() }, function (data) {
                        eval("ret=" + data);
                        if (ret) {
                            layer.alert("已被使用的分类，不能被删除！", 5);
                            return;
                        }
                        else {
                            var lyr = layer.confirm("你确定要删除吗？", function () {
                                layer.close(lyr);
                                $('#submitForm').attr("action", "../../Upload/DelType");
                                $('#submitForm').submit();
                            });
                        }
                    });

                }
                else {
                    ExamLogin();
                }
            });
        });

        //递归json tree
        function GetJsonData(tree, n) {
            for (var i = 0; i < tree.length; i++) {
                if (tree[i].children == "") {
                    html += '<div style="padding:4px 0px;padding-left:' + n * 15 + 'px;"><p class="carrow" style="padding-right:5px;"></p><a style="font-size:14px;color:green;" onclick="selectType(\'' + tree[i].text + '\',\'' + tree[i].id + '\')" >' + fucking(tree[i].text) + '</a></div>';
                } else {
                    html += '<div style="padding:4px 0px;padding-left:' + n * 15 + 'px;"><p class="parrow" style="padding-right:5px;"></p><a style="font-size:14px;color:#333;" onclick="selectType(\'' + tree[i].text + '\',\'' + tree[i].id + '\')" >' + fucking(tree[i].text) + '</a></div>';
                    GetJsonData(tree[i].children, n + 1);
                }
            }
        }

        //字符串太长截取
        function fucking(str) {
            if (str.length > 8)
                return str.substring(0, 8) + "...";
            else
                return str;
        }

        function selectType(name, id) {
            $("#selTypeName").val(name);
            $("#selTypeID").val(id);
        }
    </script>
    <div>
        <form id="submitForm" class="form-horizontal ui-formwizard" onsubmit="return true;"
        action="" method="post">
        <div class="con-title">
            <h1 class="con-title-h1">
                分类管理</h1>
        </div>
        <div class="box">
            <div class="left-box0" style="width: 250px;">
                <div class="left" style="width: 100%; height: 500px; overflow: auto;" id="TreeDiv">
                    <div style="padding-left: 0px;">
                        <p class="arrow">
                        </p>
                        <a style="font-size: 14px;" onclick="selectType('','')">资源分类</a></div>
                </div>
            </div>
            <div class="rightBox" style="margin-left: 10px; width: 500px; height: 500px;">
                <table cellpadding="0" cellspacing="0" class="tab">
                    <tr>
                        <td>
                            选择分类节点
                        </td>
                        <td>
                            <input id="selTypeName" type="text" class="inputText" readonly="readonly" /><input
                                id="selTypeID" name="selTypeID" type="hidden" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            添加分类节点
                        </td>
                        <td>
                            <input id="addTypeName" name="addTypeName" type="text" class="inputText" />
                        </td>
                    </tr>
                </table>
                <div>
                    <input id="addType" type="button" class="seach-btn" style="margin-left: 50px;" value="添加分类" />
                    <input id="delType" type="button" class="seach-btn" style="margin-left: 50px;" value="删除分类" />
                </div>
            </div>
        </div>
        </form>
    </div>
</asp:Content>
