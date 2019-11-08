using System;
using System.Collections.Generic;
using System.IO;

namespace SyncJs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Dictionary<string, string> pathMap = new Dictionary<string, string>
            {
                ["admin-lte/dist"] = "admin-lte/dist/",
                ["jquery/dist"] = "jquery/dist",
                ["jquery-validation/dist"] = "jquery-validation/dist/",
                ["jquery-validation-unobtrusive/dist"] = "jquery-validation-unobtrusive/dist/",
                ["jquery-slimscroll/jquery.slimscroll.js"] = "jquery-slimscroll/",
                ["jquery-slimscroll/jquery.slimscroll.min.js"] = "jquery-slimscroll/",
                ["bootstrap/dist"] = "bootstrap/dist/",
                ["bootstrap-select/dist"] = "bootstrap-select/dist/",
                ["bootstrap-table/dist"] = "bootstrap-table/dist/",
                ["bootbox/dist"] = "bootbox/dist/",
                ["toastr/build"] = "toastr/build/",
                ["toastr/toastr.js"] = "toastr/",
                ["toastr/toastr.less"] = "toastr/",
                ["toastr/toastr.scss"] = "toastr/",
                ["abp-web-resources/Abp"] = "abp-web-resources/Abp/",
                ["signalr/jquery.signalR.js"] = "signalr/",
                ["signalr/jquery.signalR.min.js"] = "signalr/",
                ["@aspnet/signalr/dist"] = "@aspnet-signalr/dist/",
                ["@ztree/ztree_v3/js"] = "ztree_v3/js/",
                ["@ztree/ztree_v3/css"] = "ztree_v3/css/",
                ["moment/min"] = "moment/min/",
                ["spin.js/spin.js"] = "spin.js/",
                ["spin.js/jquery.spin.js"] = "spin.js/",
                ["spin.js/spin.min.js"] = "spin.js/",
                ["waves/dist"] = "waves/dist/",
                ["sweetalert/dist"] = "sweetalert/dist/",
                ["push.js/bin"] = "push.js/",
                ["icheck/skins"] = "icheck/skins/",
                ["icheck/icheck.js"] = "icheck/",
                ["icheck/icheck.min.js"] = "icheck/",
                ["famfamfam-flags/dist"] = "famfamfam-flags/dist/",
                ["@fortawesome/fontawesome-free/"] = "fontawesome-free/",
                ["sweetalert2/dist"] = "sweetalert2/dist/",
            };
            string sourcePath = "node_modules",
                targetPath = Path.Combine("wwwroot", "lib");
            string absolutePath = @"D:\per\git\AbpCustomerAuth\aspnet-core\src\Snow.Template.Web.Mvc";// Directory.GetCurrentDirectory();
            string absoluteSourcePath = Path.Combine(absolutePath, sourcePath);
            string absoluteTargetPath = Path.Combine(absolutePath, targetPath);
            if (Directory.Exists(absoluteTargetPath))
            {
                Directory.Delete(absoluteTargetPath, true);
            }
            Directory.CreateDirectory(absoluteTargetPath);
            foreach (KeyValuePair<string, string> path in pathMap)
            {
                Copy(Path.Combine(absoluteSourcePath, path.Key), Path.Combine(absoluteTargetPath, path.Value));
            }
            Console.WriteLine("拷贝完成！");
            Console.ReadKey();
        }

        private static void Copy(string source, string target)
        {
            CreateDirectory(target);
            if (File.GetAttributes(source).HasFlag(FileAttributes.Directory))
            {
                CopyFolder(source, target);
            }
            else
            {
                CopyFile(source, Path.Combine(target, Path.GetFileName(source)));
            }
        }

        private static void CreateDirectory(string tempPath)
        {
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
        }

        private static void CopyFile(string source, string target)
        {
            File.Copy(source, target, true);//true代表可以覆盖同名文件
        }

        /// <summary>
        /// 复制源文件夹下的所有内容到新文件夹
        /// </summary>
        /// <param name="sources">源文件夹路径</param>
        /// <param name="dest">新文件夹路径</param>
        private static void CopyFolder(string sources, string dest)
        {
            DirectoryInfo dinfo = new DirectoryInfo(sources);
            //注，这里面传的是路径，并不是文件，所以不能包含带后缀的文件
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                //目标路径destName = 新文件夹路径 + 源文件夹下的子文件(或文件夹)名字
                //Path.Combine(string a ,string b) 为合并两个字符串
                string destName = Path.Combine(dest, f.Name);
                if (f is FileInfo)
                {
                    CopyFile(f.FullName, destName);
                }
                else
                {
                    //如果是文件夹就创建文件夹，然后递归复制
                    Directory.CreateDirectory(destName);
                    CopyFolder(f.FullName, destName);
                }
            }
        }
    }
}