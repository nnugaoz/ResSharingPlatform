<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Upload/Upload.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    统计图表
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RecLink" runat="server">
    <script type="text/javascript">
        $(function () {
            chartInit("total");
            chartInit("praisePre");
            chartInit("viewNum");
            chartInit("downloadNum");
        });

        function chartInit(id) {
            var title = "";
            var ytitle = "";
            var valueSuffix = "";
            if (id == "total") {
                title = "上传资源总数";
                ytitle = "资源总数";
            } else if (id == "praisePre") {
                title = "好评率";
                ytitle = "好评率";
                valueSuffix = "%";
            } else if (id == "viewNum") {
                title = "浏览量";
                ytitle = "浏览量";
            } else if (id == "downloadNum") {
                title = "下载量";
                ytitle = "下载量";
            }

            var name = [];
            var da = [];

            $.ajax({
                type: "post",
                url: "../../Upload/ChartData",
                data: "type=" + id,
                async: false,
                dataType: "json",
                success: function (data) {
                    name = data.name;
                    da = data.data;
                }
            });

            if (name != null) {
                if (name.length > 10) {
                    $('#' + id).css("width", (name.length * 100 + 200) + 'px');
                }
            }

            $('#' + id).highcharts({
                title: {
                    text: title,
                    x: -20 //center
                },
                xAxis: {
                    categories: name
                },
                yAxis: {
                    title: {
                        text: ytitle
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: valueSuffix
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: title,
                    data: da
                }]
            });
        }

        function goBack() {
            if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {
                window.location.href = "../../Upload/Statistics";
            } else {
                ExamLogin();
            }
        }

        function packUp(obj) {
            $(obj).children(":eq(0)").attr("src", "../../Images/left_arrow1.png");
            $(obj).attr("onclick", "expansion(this); return false;");
            $(obj).parent().next().hide();
        }

        function expansion(obj) {
            $(obj).children(":eq(0)").attr("src", "../../Images/bottom_arrow1.png");
            $(obj).attr("onclick", "packUp(this); return false;");
            $(obj).parent().next().show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <div class="con-title">
        <h1 class="con-title-h1">统计图表</h1>
        <div class="con-title-btns">
            <input type="button" id="Button1" name="search" class="title-btn" value="返回" onclick="goBack()" />
        </div>
    </div>
    <div class="box">
        <div style="margin-bottom:10px">
            <div>
                <a href="#" onclick="packUp(this); return false;" class="chart">
                    <img src="../../Images/bottom_arrow1.png" style="height:16px;width:16px"/>
                    <span style="font-size:16px;width:96px;text-align:center;display:inline-block">上传资源总数</span>
                    <img src="../../Images/line.png" style="height:2px;width:80%;"/>
                </a>
            </div>
            <div style="width: 1050px; height: 420px; overflow:auto">
                <div id="total" style="width: 1050px; height: 400px; margin: 0 auto"></div>
            </div>
        </div>
        
        <div style="margin-bottom:10px">
            <div>
                <a href="#" onclick="expansion(this); return false;" class="chart">
                    <img src="../../Images/left_arrow1.png" style="height:16px;width:16px"/>
                    <span style="font-size:16px;width:96px;text-align:center;display:inline-block">好评率</span>
                    <img src="../../Images/line.png" style="height:2px;width:80%;"/>
                </a>
            </div>
            <div style="width: 1050px; height: 420px; overflow:auto;display:none">
                <div id="praisePre" style="width: 1050px; height: 400px; margin: 0 auto;"></div>
            </div>
        </div>

        <div style="margin-bottom:10px">
            <div>
                <a href="#" onclick="expansion(this); return false;" class="chart">
                    <img src="../../Images/left_arrow1.png" style="height:16px;width:16px"/>
                    <span style="font-size:16px;width:96px;text-align:center;display:inline-block">浏览量</span>
                    <img src="../../Images/line.png" style="height:2px;width:80%;"/>
                </a>
            </div>
            <div style="width: 1050px; height: 420px; overflow:auto;display:none">
                <div id="viewNum" style="width: 1050px; height: 400px; margin: 0 auto;"></div>
            </div>
        </div>
        
        <div>
            <div>
                <a href="#" onclick="expansion(this); return false;" class="chart">
                    <img src="../../Images/left_arrow1.png" style="height:16px;width:16px"/>
                    <span style="font-size:16px;width:96px;text-align:center;display:inline-block">下载量</span>
                    <img src="../../Images/line.png" style="height:2px;width:80%;"/>
                </a>
            </div>
            <div style="width: 1050px; height: 420px; overflow:auto;display:none">
                <div id="downloadNum" style="width: 1050px; height: 400px; margin: 0 auto;"></div>
            </div>
        </div>
        
    </div>
</div>
</asp:Content>
