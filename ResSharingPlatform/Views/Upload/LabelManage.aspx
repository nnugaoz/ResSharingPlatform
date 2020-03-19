<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    标签管理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RecLink" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        var html = '<div style="padding-left:0px;"><p class="arrow"></p><a style="font-size:14px;" >标签分类</a></div>';
        $(function () {
            var list = '<%=ViewData["typeTree"]%>';
            var treeList = eval(list);
            GetJsonData(treeList, 1);
            html += '<div style="padding-left:30px;"><p class="addarrow" onclick="addTagType(event);"></p></div>';
            $("#TreeDiv").html(html);

            if ($("#parentID").val() != "") {
                selectType($("#parentName").val(), $("#parentID").val());
            }

            $("#addTag").click(function () {
                if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                    if ($('#addTagName').val() == "") {
                        $('#addTagName').focus();
                        return;
                    }
                    $('#submitForm').attr("action", "../../Upload/AddTag");
                    $('#submitForm').submit();
                }
                else {
                    ExamLogin();
                }
            });
        });

        //标签分类
        function GetJsonData(tree, n) {
            for (var i = 0; i < tree.length; i++) {
                html += '<div style="padding:4px 0px;padding-left:' + n * 15 + 'px;"><p class="parrow" style="padding-right:5px;"></p><a style="font-size:14px;color:#333;" onclick="selectType(\'' + tree[i].NAME + '\',\'' + tree[i].ID + '\')" ondblclick="delTag(event,\'' + tree[i].ID + '\',0)">' + fucking(tree[i].NAME) + '</a><image src="../../Images/delete.png" onclick="delTag(event,\'' + tree[i].ID + '\',0)" style="cursor:pointer;margin-left:5px"></image></div>';
            }
        }

        //字符串太长截取
        function fucking(str) {
            if (str.length > 10)
                return str.substring(0, 10) + "...";
            else
                return str;
        }

        function selectType(name, id) {
            $('#TagTypeName').html(name);
            $("#parentID").val(id);$("#parentName").val(name);
            $.ajax({
                type: "post",
                url: "../../Upload/GetLabel",
                data: "id=" + id + "&keyword=",
                async: false,
                dataType: "json",
                success: function (data) {
                    var html = "";
                    if (data != null) {
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            html += "<div class='label-queue-item'><div class='labelName'>" + item.NAME + "<image src='../../Images/delete.png' onclick='delTag(event,\"" + item.ID + "\",1)' style='cursor:pointer;margin-left:5px'></image></div></div>";
                            //html += "<input type='button' value='" + item.NAME + "' class='inputTag' ondblclick='delTag(event,\"" + item.ID + "\",1)'/>";
                        }
                    }
                    //html += "<image alt=\"添加标签\" src=\"../../Images/add24.png\" class=\"addImg\" onclick=\"addTagsByType(event,'" + id + "');\"></image>";
                    $("#tagsDiv").html(html);
                }
            });
        }

        function addTagType(e) {
            showAddTag(e,"");
        }

        function addTagsByType(e, pid) {
            pid = $("#parentID").val();
            showAddTag(e,pid);
        }

        function selectTag(name, id) {

        }


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
            var o = document.getElementById('addTagDiv');
            o.style.display = "none";
            EX.removeEvent.call(document, 'click', hide);
        }

        var getCoordInDocument = function (e) {
            e = e || window.event;
            var x = e.pageX || (e.clientX +
          (document.documentElement.scrollLeft
          || document.body.scrollLeft));
            var y = e.pageY || (e.clientY +
          (document.documentElement.scrollTop
          || document.body.scrollTop));
            return { 'x': x, 'y': y };
        };

        function showAddTag(e, pid) {
            $('#tagTypeID').val(pid);
            var pointer = getCoordInDocument(e);
            var X = pointer.x;
            var Y = pointer.y;
            $('#addTagDiv').css("display", "block");
            $('#addTagDiv').css("z-index", "9999");
            $('#addTagDiv').css("left", X + "px");
            $('#addTagDiv').css("top", Y + "px");
            document.getElementById('addTagDiv').onclick = EX.stop;
            setTimeout(function () { EX.addEvent.call(document, 'click', hide); });
        }

        function delTag(e, id, i) {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                $("#delTagID").val(id);
                if (i == 0) { $("#parentID").val(""); $("#parentName").val(""); }
                var lyr = layer.confirm("您确定要删除标签？", function () {
                    layer.close(lyr);
                    $('#submitForm').attr("action", "../../Upload/DelTag");
                    $('#submitForm').submit();
                });
            }
            else {
                ExamLogin();
            }
        }
    </script>
    <div>
    <form id="submitForm" class="form-horizontal ui-formwizard" onsubmit="return true;" action="" method="post">
        <input id="delTagID" name="delTagID" type="hidden" />
        <div class="con-title">
            <h1 class="con-title-h1">标签管理</h1>
        </div>
        <div class="box">
        <div class="left-box0" style="width:250px;">
            <div class="left" style="width:100%;height:500px;overflow:auto;" id="TreeDiv">
            </div>
        </div>
        <div class="rightBox" style="margin-left:10px; width:500px;height:500px;overflow:auto;">
            <div id="tagDiv" style="margin-top:15px;margin-left:10px;">
                <p id="TagTypeName" style="color:Blue;margin-bottom:5px">标签</p>
                <image alt="添加标签" src="../../Images/add24.png" class="addImg" onclick="addTagsByType(event,'<%:ViewData["ParentID"]%>');" style="margin-bottom:5px"></image>
                <input id="parentID" name="parentID" type="hidden" value="<%:ViewData["ParentID"]%>""/>
                <input id="parentName" name="parentName" type="hidden" value="<%:ViewData["ParentName"] %>" />
                <div id="tagsDiv" style="width:470px;"></div>
            </div>
        </div>
        </div>
        <div id="addTagDiv" style="position:absolute;width:300px;border:1px solid #cdcdcd;background:#fff;display:none;">
            <table cellpadding="0" cellspacing="0" class="tab">
                <tr>
                    <td>名称</td>
                    <td><input id="addTagName" name="addTagName" type="text" class="inputText"/><input id="tagTypeID" name="tagTypeID" type="hidden" /></td>
                </tr>
            </table>
            <div style="height:40px;">
                <input id="addTag" type="button" class="seach-btn" style="margin-left:110px;" value="添加标签" />
            </div>
        </div>
    </form>
    </div>
</asp:Content>
