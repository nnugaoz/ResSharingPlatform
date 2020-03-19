using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResSharingPlatform.Lib
{
    public class Pages2
    {
        private string[] page_array;// 用来构造分页的数组  
        private string pageurl;// 最终显示 
        private string[] current_array;// 用来构造当前分页的数组
        public int subpagenums { get; set; }// subpagenums  
        public int subeachnums { get; set; }// 每页显示的条目数  
        public int subnums { get; set; }// 总条目数  
        public int subcurrentpage { get; set; }// 当前被选中的页  
        public int subeachpages { get; set; }// 每次显示的页数  
        public string subpagelink { get; set; }// 每个分页的链接
        public int subpagetype { get; set; }// 分页样式
        public string subformname { get; set; }// 表单名
        public string hidIdName { get; set; }//存放当前页的隐藏控件的id
        public string subPageId { get; set; }//跳转页数输入框控件的id
        public string functionName { get; set; }//获取列表信息的方法名
        public string functionPageName { get; set; }//翻页的方法名
        public string functionTopageName { get; set; }//跳转页面的方法名

        /// <summary>
        /// 当@subpagetype=1的时候为普通分页模式 
        /// example：   共4523条记录, [首页] [上页] 1 2 3 4 5 6 7 8 9 10 [下页] [尾页] 
        /// 当@subpagetype=2的时候为经典分页模式 
        /// example：   共4523条记录,每页显示10条,共10页，当前第1/453页 首页 上页 1 2 3 4 5 6 7 8 9 10 下页 尾页 
        /// </summary>
        /// <returns></returns>
        public string SubPages()
        {
            //计算总页数       
            if (subnums % subeachnums == 0)
            {
                subpagenums = subnums / subeachnums;
            }
            else
            {
                subpagenums = (subnums / subeachnums) + 1;
            }

            // 如何当前页大于总页数，当前页为最后一页
            if (subcurrentpage > subpagenums)
            {
                subcurrentpage = subpagenums;
            }

            //分页样式
            switch (subpagetype)
            {
                //当@subpagetype=1的时候为普通分页模式 
                case 1:
                    pageurl = CreateSimplePages();
                    break;
                //当@subpagetype=2的时候为经典分页模式 
                case 2:
                    pageurl = CreatePages();
                    break;
                default:
                    break;

            }
            //返回分页标签
            return pageurl;
        }

        /// <summary>
        /// 当@subpagetype=1的时候为普通分页模式 
        /// example：   共4523条记录, [首页] [上页] 1 2 3 4 5 6 7 8 9 10 [下页] [尾页] 
        /// </summary>
        /// <returns></returns>
        private string CreateSimplePages()
        {
            // 总条目数大于每页显示条目数
            if (subnums > subeachnums)
            {
                //分页标签
                string subpagecss = "";
                subpagecss = subpagecss + "共" + subnums + "条记录，";

                //当前页不是第一页
                if (subcurrentpage > 1)
                {
                    // string firstPageUrl=subpagelink+"1";  
                    // int prewnum=subcurrentpage-1;
                    // string prewPageUrl=subpagelink+prewnum.ToString();
                    int pagenow = subcurrentpage - 1;// 上一页
                    subpagecss = subpagecss + "[<a href=\"javascript:" + functionPageName.ToString() + "('1')\">首页</a>] ";
                    subpagecss = subpagecss + "[<a href=\"javascript:" + functionPageName.ToString() + "('" + pagenow + "')\">上一页</a>] ";
                }
                else //当前页第一页
                {
                    subpagecss = subpagecss + "[首页] ";
                    subpagecss = subpagecss + "[上一页] ";
                }

                // 初始化页面标签数组
                string[] linka = InitPageNums();

                // 循环生成<a>标签
                for (int i = 0; i < linka.Length; i++)
                {
                    string s = linka[i]; // 标签内容  

                    // 当前页
                    if (s == subcurrentpage.ToString())
                    {
                        subpagecss = subpagecss + "[<span style='color:red;font-weight:bold;'>" + s + "</span>]";
                    }
                    else// 非当前页
                    {
                        // string url=subpagelink+s;
                        subpagecss = subpagecss + "[<a href=\"javascript:" + functionPageName.ToString() + "('" + s + "')\">" + s + "</a>]";
                    }
                }

                // 非最后一页
                if (subcurrentpage < subpagenums)
                {
                    //string lastPageUrl = subpagelink + subpagenums;  
                    //string nextPageUrl=subpagelink+(subcurrentpage+1).ToString();
                    int pagenow = subcurrentpage + 1;// 下一页数
                    subpagecss = subpagecss + " [<a href=\"javascript:" + functionPageName.ToString() + "('" + pagenow.ToString() + "')\">下一页</a>] ";
                    subpagecss = subpagecss + "[<a href=\"javascript:" + functionPageName.ToString() + "('" + subpagenums.ToString() + "')\">尾页</a>] ";
                }
                else// 最后一页
                {
                    subpagecss = subpagecss + "[下一页] ";
                    subpagecss = subpagecss + "[尾页] ";
                }

                // CreateFormAction():生成翻页提交表单js
                pageurl = subpagecss + CreateFormAction();
            }
            else// 总条目数小于每页显示条目数
            {
                pageurl = "共" + subnums + "条记录  ";
                // CreateFormAction():生成翻页提交表单js
                pageurl = pageurl + CreateFormAction();
            }
            return pageurl;// 返回页码标签
        }

        /// <summary>
        /// 当@subpagetype=2的时候为经典分页模式 
        /// example：   共4523条记录,每页显示10条,共10页，当前第1/453页 首页 上页 1 2 3 4 5 6 7 8 9 10 下页 尾页 
        /// </summary>
        /// <returns></returns>
        private string CreatePages()
        {
            // 总条目数大于每页显示条目数
            if (subnums > subeachnums)
            {
                // 分页标签
                string subpagecss = "";
                subpagecss = subpagecss + "共" + subnums + "条记录，";
                subpagecss = subpagecss + "每页显示" + subeachnums + "条，";
                subpagecss = subpagecss + "共" + subpagenums + "页，";
                subpagecss = subpagecss + "当前第" + subcurrentpage + "/" + subpagenums + "页，";

                // 当前页不是第一页
                if (subcurrentpage > 1)
                {
                    //string firstPageUrl = subpagelink + "1 ";
                    //int prewnum = subcurrentpage - 1;
                    //string prewPageUrl = subpagelink + prewnum.ToString();
                    int pagenow = subcurrentpage - 1;// 上一页
                    subpagecss = subpagecss + "<a href=\"javascript:" + functionPageName.ToString() + "('1')\">首页</a> ";
                    subpagecss = subpagecss + "<a href=\"javascript:" + functionPageName.ToString() + "('" + pagenow + "')\">上一页</a> ";
                }
                else//当前页第一页
                {
                    subpagecss = subpagecss + "首页 ";
                    subpagecss = subpagecss + "上一页 ";
                }

                string[] linka = InitPageNums();// 初始化页面标签数组

                // 循环生成<a>标签
                for (int i = 0; i < linka.Length; i++)
                {
                    string s = linka[i];// 标签内容

                    // 当前页
                    if (s == subcurrentpage.ToString())
                    {
                        subpagecss = subpagecss + "<span style='color:red;font-weight:bold;padding-left:8px;'>" + s + "</span>";
                    }
                    else// 非当前页
                    {
                        //string url = subpagelink + s;
                        subpagecss = subpagecss + "<a style=\";padding-left:8px;\" href=\"javascript:" + functionPageName.ToString() + "('" + s + "')\">" + s + "</a>";
                    }
                }

                // 非最后一页
                if (subcurrentpage < subpagenums)
                {
                    //string lastPageUrl = subpagelink + subpagenums;
                    //string nextPageUrl = subpagelink + (subcurrentpage + 1).ToString();
                    int pagenow = subcurrentpage + 1;// 下一页数
                    subpagecss = subpagecss + " <a href=\"javascript:" + functionPageName.ToString() + "('" + pagenow.ToString() + "')\">下一页</a> ";
                    subpagecss = subpagecss + "<a href=\"javascript:" + functionPageName.ToString() + "('" + subpagenums.ToString() + "')\">尾页</a> ";
                }
                else// 最后一页
                {
                    subpagecss = subpagecss + " 下一页 ";
                    subpagecss = subpagecss + "尾页 ";
                }

                // 添加页面跳转
                subpagecss = subpagecss + "  第<input type=\"text\" id=\"" + subPageId.ToString() + "\" style=\"width:30px;\" />页<input type=\"button\" value=\"跳转\" onclick=" + functionTopageName.ToString() + "('" + subpagenums + "') /> ";

                // CreateFormAction():生成翻页时提交表单js
                pageurl = subpagecss + CreateFormAction();
            }
            else// 总条目数小于每页显示条目数
            {
                pageurl = "共" + subnums + "条记录  ";
                // CreateFormAction():生成翻页提交表单js
                pageurl = pageurl + CreateFormAction();
            }
            return pageurl;// 返回页码标签
        }

        /// <summary>
        /// 生成翻页时提交表单js处理事件
        /// </summary>
        /// <returns></returns>
        private string CreateFormAction()
        {
            // 存储和操作生成表单js处理事件
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            // 隐藏当前页
            sb.Append(" <input id=\"" + hidIdName.ToString() + "\" type=\"hidden\" name=\"" + hidIdName.ToString() + "\">");
            // 翻页  
            sb.Append("<script>function " + functionPageName.ToString() + "(page){$(\"#" + hidIdName.ToString() + "\").val(page);" + functionName.ToString() + "}</script>");
            // 分页跳转
            sb.Append("<script>function " + functionTopageName + "(allpage){");
            // 判断输入跳转页是否为数字
            sb.Append("if($.trim(($(\"#" + subPageId.ToString() + "\").val()))==\"\"){alert(\"请输入数字！\");return false;}");
            sb.Append("if(isNaN($(\"#" + subPageId.ToString() + "\").val())) {alert(\"请输入数字！\");return false;}");
            sb.Append("if(parseInt($(\"#" + subPageId.ToString() + "\").val()) > allpage || parseInt($(\"#" + subPageId.ToString() + "\").val())<=0){alert(\"页索引超出范围！\");return false;}");
            sb.Append(functionPageName.ToString() + "($(\"#" + subPageId.ToString() + "\").val());");
            sb.Append("}</script>");
            return sb.ToString();//返回表单js
        }

        /// <summary>
        /// InitPageNums该函数使用来构造显示的条目
        /// 例如：[1][2][3][4][5][6][7][8][9][10]
        /// </summary>
        /// <returns></returns>
        private string[] InitPageNums()
        {

            // 总页数小于每次显示的页数
            if (subpagenums < subeachpages)
            {
                // 根据总页数初始化分页的数组大小
                current_array = new string[subpagenums];

                // 分页数组赋值
                for (int i = 0; i < subpagenums; i++)
                {
                    current_array[i] = (i + 1).ToString();
                }
            }
            else// 总页数大于每次显示的页数
            {
                // 根每次显示的页数初始化分页的数组大小
                current_array = new string[subeachpages];

                // 建立分页的数组初始化
                current_array = InitArray();

                // 当前页数小于等于每次显示的页数
                if (subcurrentpage <= subeachpages)
                {
                    // 分页数组赋值
                    for (int i = 0; i < current_array.Length; i++)
                    {
                        current_array[i] = (i + 1).ToString();
                    }
                }
                // 当前页数小于等于总页数 并且大于每次显示的页数
                else if (subcurrentpage <= subpagenums && subcurrentpage > subpagenums - subeachpages + 1)
                {
                    // 往后两页生成分页的数组
                    for (int i = 0; i < current_array.Length; i++)
                    {
                        current_array[i] = (subpagenums - subeachpages + 1 + i).ToString();
                    }
                }
                else// 当前页小于每次显示页数 
                {
                    // 往前一页生成分页的数组
                    for (int i = 0; i < current_array.Length; i++)
                    {
                        current_array[i] = (subcurrentpage - 2 + i).ToString();
                    }
                }
            }

            return current_array;// 返回分页的数组
        }

        /// <summary>
        /// 用来给建立分页的数组初始化。 
        /// </summary>
        /// <returns></returns>
        private string[] InitArray()
        {

            page_array = new string[subeachpages];// 根每次显示的页数初始化分页的数组大小

            //初始化分页标签
            for (int i = 0; i < subeachpages; i++)
            {
                page_array[i] = i.ToString();
            }
            return page_array;// 返回分页的初始化数组  
        } 
    }
}