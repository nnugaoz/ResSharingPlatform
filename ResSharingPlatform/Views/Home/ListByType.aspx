<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="RecLink" runat="server">
<script type="text/javascript">
    var TreeID = "";

    $(function () {
        $("#menu1").children().addClass("cur");

        var rethtml = '<%=Html.ActionLink("首页", "Index", "Home")%>';
        rethtml += $("#txtLink").val(); // '<%=ViewData["retStr"]%>';
        $("#hLink").html(rethtml);

        var list = '<%=ViewData["typeTree"]%>';
        var treeList = eval(list);
        //加载树
        $.fn.zTree.init($("#tdTree"), setting, treeList);

        //模糊条件回车
        $('#like').bind('keypress', function (event) {
            if (event.keyCode == "13") {
                btnSearch_Click();
                return false;
            }
        });

        if ($("#typeId").val() != "") {
            selectNode($("#typeId").val());
        }
        GetInitList();
    });

    /**动态选择树节点**/
    function selectNode(id) {
        var zTree = $.fn.zTree.getZTreeObj("tdTree");
        zTree.selectNode(zTree.getNodeByParam("id", id));
    }

    /**选择树节点**/
    function zTreeOnClick(event, treeId, treeNode) {
        TreeID = treeId;
        //每次只能选择一个
        var treeObj = $.fn.zTree.getZTreeObj(TreeID);
        treeObj.selectNode(treeNode);
        //var nodes = treeObj.getSelectedNodes();
        //alert(nodes.length + "###" + nodes[0].id + ", " + nodes[0].name)
        reSearch();
    }

    /**获取Node的ID**/
    function GetNodeID() {
        var ret = "";
        if (TreeID != "") {
            var treeObj = $.fn.zTree.getZTreeObj(TreeID);
            var nodes = treeObj.getSelectedNodes();
            ret = nodes[0].id;
        }
        return ret;
    }

    function GetNodePID() {
        var ret = "";
        if (TreeID != "") {
            var treeObj = $.fn.zTree.getZTreeObj(TreeID);
            var nodes = treeObj.getSelectedNodes();
            ret = nodes[0].pId;
        }
        return ret;
    }

    function reSearch() {
        $("#typeId").val(GetNodeID());
        $.get("../../Home/GetLink?t=" + new Date().getTime(), { typeId: $("#typeId").val() }, function (obj) {
            try {
                var rethtml = '<%=Html.ActionLink("首页", "Index", "Home")%>';
                rethtml += obj;
                $("#hLink").html(rethtml);
            }
            catch (e) { }
        });
        GetInitList();
    }

    function doSearch2(id, belong) {
        window.location.href = "../../Home/ListByType?typeId=" + id + "&belong=" + belong;
    }

    function GetInitList() {
        $('#ActionForm').attr("action", "../../Home/FileList");

        $('#ActionForm').ajaxSubmit(
            function (data) {
                $("#div_content").html(data);
            }
        );
    }

    function toSequence(type) {
        $("#sequence").val(type);
        $("#pagecurrent").val("1");

        GetInitList();

        //链接样式
        for (var i = 1; i < 6; i++) {
            $("#" + i).parent().removeClass();
        }

        $("#" + type).parent().addClass("curalink");
    }

    function btnSearch_Click() {
        GetInitList();
    }
</script>

    <%--树--%>
    <script type="text/javascript">
        var setting = {
            callback: {
                onClick: zTreeOnClick
            },
            check: {
                enable: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            }
        };

        function setCheck() {
            var zTree = $.fn.zTree.getZTreeObj("tdTree"),
			py = "p",
			sy = "s",
			pn = "p",
			sn = "s",
			type = { "Y": py + sy, "N": pn + sn };
            zTree.setting.check.chkboxType = type;
        }
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%:ViewData["TypeName_2"]%>_资料大全
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="txtLink" value="<%: ViewData["retStr"]%>"/>
    <div class="list main-box mt10" style="margin-top:0px;">
        <h1 id="hLink">
            <%: Html.ActionLink("首页", "Index", "Home")%> 
        </h1>
        <div>
            <div class="left-box0" style="height:1010px;">
                <div class="left" style="height:500px;height:990px;" id="TreeDiv">
                    <ul id="tdTree" class="ztree" style="width: 100%; height: 100%; overflow-y: scroll;">
                    </ul>
                </div>
            </div>

            <div style="width:770px;float:left;padding-left:10px">
                <form id="ActionForm" action="" class="form-horizontal ui-formwizard" method="post">
                    <input type="hidden" id="typeId" name="typeId" value="<%:ViewData["typeId"] %>"/>
                    <input type="hidden" id="sequence" name="sequence" value=""/>
                    <div class="tabtitle mt10" style="font-size:14px;">
                        排序 &nbsp;&nbsp;
                        <a class="curalink" href="javascript:toSequence('5')"><font id = "5">最新上传</font></a>&nbsp;&nbsp;
                        <a href="javascript:toSequence('1')"><font id = "1">评价次数</font></a>&nbsp;&nbsp; 
                        <a href="javascript:toSequence('2')"><font id = "2">最受好评</font></a>&nbsp;&nbsp;
                        <a href="javascript:toSequence('3')"><font id = "3">最多浏览</font></a>&nbsp;&nbsp; 
                        <a href="javascript:toSequence('4')"><font id = "4">最多下载</font></a>&nbsp;&nbsp; 
                        <input id="like" name="like" type="text" style="height:24px; line-height:20px; border:1px solid #cdcdcd; width:200px;margin-left:100px;" />
                        <input type="button" id="btnSearch" class="btnSeach" value="搜索" onclick="btnSearch_Click()" />
                    </div>
                    <div id="div_content">
                    </div>
                </form>
            </div>
        </div>
    </div>
</asp:Content>