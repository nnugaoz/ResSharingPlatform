<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html>
<html lang="zh">
<head>
<meta charset="utf-8" />
<title>平台引导页面</title>
    <link href="../../Content/comit/font/iconfont.css" rel="stylesheet" type="text/css" />
     <script src="../../Scripts/js/jquery-1.8.2.min.js" type="text/javascript"></script> 
    <script src="../../Scripts/js/OS.js" type="text/javascript"></script>
<style type="text/css">
	* {
		margin: 0;
		padding: 0;
	}
	body { 
		font-size: 12px;
		font-family: '微软雅黑';
		min-width: 1100px;
		background-color: #3385b6;
		background-image: url(../../Content/comit/images/bg.png);
		background-repeat: no-repeat;
		background-position: center top; 
		overflow: hidden;
	}
	ul {
		display: block;
		margin: 0;
		padding: 0;
		list-style: none;
	}
	li {
		display: block;
		margin: 0;
		padding: 0;
		list-style: none;
	}
	img {
		border: 0;
	}
	a, a:focus {
		text-decoration: none;
		color: #000;
		outline: none;
	}
	a:hover {
		color: #00a4ac;
		text-decoration: none;
	}
	.top {
		height: 47px;
		position: absolute;
		top: 0;
		background: url(../../Content/comit/images/top-bg.png) repeat-x;
		z-index: 100;
		width: 100%;
		font-size: 13px;
	}
	.top span {
		color: #def0f6;
		line-height: 47px;
		padding-left: 50px;
		color: #afc5d2;
		float: left;
	}
	.list {
		display: block;
		position: absolute;
		width: 1100px;
		height: 160px;
		top: 50%;
		left: 50%;
		margin-top: -80px;
		margin-left: -550px;
	}
	.list li {
		float: left;
		padding: 0px 20px;
	}
	.list li a {
		display: block;
		width: 140px;
		height: 120px;
		padding: 20px;
		text-align: center;
		cursor: pointer;
		border-radius: 4px;
		-webkit-box-shadow:0 10px 30px rgba(0, 0, 0, .2);  
  		-moz-box-shadow:0 10px 30px rgba(0, 0, 0, .2);  
 		box-shadow:0 10px 30px rgba(0, 0, 0, .2); 
	}
	.list li.one a{ background-color:rgba(186,52,46,.9);}
	.list li.two a{ background-color:rgba(77,110,183,.9);}
	.list li.three a{ background-color:rgba(244,130,62,.9);}
	.list li.four a{ background-color:rgba(127,73,95,.9);}
	.list li.five a{ background-color:rgba(71,185,133,.9);}
	.list li.one:hover a{ background-color:rgba(186,52,46,1);}
	.list li.two:hover a{ background-color:rgba(77,110,183,1);}
	.list li.three:hover a{ background-color:rgba(244,130,62,1);}
	.list li.four:hover a{ background-color:rgba(127,73,95,1);}
	.list li.five:hover a{ background-color:rgba(71,185,133,1);}
	.list li a span.iconfont {
		display: inline-block;
		width: 100%;
		height: 80px;
		text-align: center;
		line-height: 80px;
		color: #fff;
		font-size: 50px;
	}
	.list li a p {
		display: inline-block;
		font-size: 14px;
		color: #fff;
		line-height: 40px;
	}
</style>

<script>
    function openResource(unloginurl, url) {

        
        if (getCookie("gm_userinfo", "guid") != null && getCookie("gm_userinfo", "Account") != null) {

            $("#urllink").attr("href", url);
            window.location.href = url;
        } else {
            window.location.href = unloginurl;
        }
        
         
    }
 
</script>

</head>

<body>
    <div class="top"><span></span></div>
    <ul class="list">
      <li class="one">
      	<a href="<%=ViewData["ResoureUrl"] %>" onclick="openResource('<%=ViewData["ResoureNoLoginUrl"] %>','<%=ViewData["ResoureUrl"] %>')">
        	<span class="iconfont">&#xe608;</span>
        	<p>教师信息及资源共享</p>
        </a>
      </li>
       <li class="two">
      	<a href="<%=ViewData["HomeWorkUrl"] %>"  target="_blank">
        	<span class="iconfont">&#xe603;</span>
        	<p>作业互动平台</p>
        </a>
      </li>
       <li class="three">
      	<a href="<%=ViewData["SafeMonitorUrl"] %>"  target="_blank">
        	<span class="iconfont">&#xe605;</span>
        	<p>安全监控平台</p>
        </a>
      </li>
       <li class="four">
      	<a href="<%=ViewData["CmsUrl"] %>?type=cms"  target="_blank">
        	<span class="iconfont">&#xe601;</span>
        	<p>CMS站群后台</p>
        </a>
      </li>
       <li class="five">
      	<a href="<%=ViewData["BbsUrl"] %>" target="_blank">
        	<span class="iconfont">&#xe607;</span>
        	<p>BBS</p>
        </a>
      </li>
    </ul>
</body>
</html>

