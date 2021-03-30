using Imprint.Network;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace _91crawler
{
    class Program
    {
        static WebSession session = new WebSession();

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// 获取m3u8 url
        /// </summary>
        /// <param name="url"></param>
        public static string GetDownloadUrl(string url)
        {
            var html = session.Get(url) as string;
            if (!String.IsNullOrEmpty(html))
            {
                // 截取video标签
                var script = StrHelper.GetStrBetween(html, "<video id=\"player_one\"", "</script>");
                if (!String.IsNullOrEmpty(script))
                {
                    // document.write(strencode2("  xxxx ")

                    var m3u8url = StrHelper.GetStrBetween(script, "document.write(strencode2(\"", "\"");
                    if (!String.IsNullOrEmpty(url))
                    {
                        m3u8url = StrHelper.URLDecode(m3u8url);

                        // <source src='xxx.m3u8' type='application/x-mpegURL'>

                        m3u8url = StrHelper.GetStrBetween(m3u8url, "<source src='", "'");
                        return m3u8url;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 启动hlsdl下载m3u8
        /// </summary>
        /// <param name="m3u8url"></param>
        public static Process StartHlsDl(string m3u8url)
        {
            var urlInfo = new Uri(m3u8url);
            var name = Path.GetFileName(urlInfo.LocalPath);
            // -b best quality
            var cmd = $"./hlsdl";
            var param = $" -b -o \"./download/{name}.ts\" \"{m3u8url}\"";
            Log("启动hlsdl...");
            Log(cmd + param);
            // XXX: 只管启动hlsdl
            var info = new ProcessStartInfo(cmd, param);
            var proc = Process.Start(info);

            return proc;
        }

        /// <summary>
        /// check是否有hlsdl
        /// </summary>
        /// <returns></returns>
        public static bool CheckHlsdl()
        {
            return File.Exists("./hlsdl");
        }

        public static void Main(string[] args)
        {
            // 本地测试代理
            // session.Proxy = "127.0.0.1:8118";

            if (args.Length == 0)
            {
                Log("使用方法: dotnet 91crawler.dll url");
                return;
            }
            if (!CheckHlsdl())
            {
                Log("你需要先编译hlsdl, 给执行权限并放在同目录下");
                return;
            }
            if (!Directory.Exists("./download"))
            {
                Directory.CreateDirectory("./download");
            }
            var url = args[0];
            var m3u8url = GetDownloadUrl(url);
            StartHlsDl(m3u8url);
        }
    }
}