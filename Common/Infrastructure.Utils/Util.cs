// <copyright file="Util.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/10/30 02:56:43 </date>
// <summary> 工具类 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/10/30 02:56:43
//      修改描述：新建 Util.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace Infrastructure.Utils
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Markup;

    using Infrastructure.Log;

    #endregion

    /// <summary>
    /// 公共方法
    /// </summary>
    public static class Util
    {
        #region Static Fields
        private static string[] telStarts = "134,135,136,137,138,139,150,151,152,157,158,159,130,131,132,155,156,133,153,180,181,182,183,185,186,176,187,188,189,177,178".Split(',');

        private static string[] emailExt = "qq.com,21cn.com,163.com,126.com,hotmail.com,sohu.com,icloud.com,sina.com,sina.cn".Split(',');

        /// <summary>
        /// 登录信息保存的文件名称
        /// </summary>
        private static readonly string LoginInfoFileName = "login.info";

        /// <summary>
        /// 登录信息保存的路径
        /// </summary>
        private static readonly string LoginInfoPath = AppDomain.CurrentDomain.BaseDirectory + "User";

        /// <summary>
        /// N/A标识字符
        /// </summary>
        private static string flagNA;

        /// <summary>
        /// 线程同步标识
        /// </summary>
        private static object synacFlag = new object();

        #endregion

        #region Public Methods and Operators



        /// <summary>
        /// 随机生成电话号码
        /// </summary>
        /// <returns></returns>
        public static string GetRandomTel()
        {
            Random ran = new Random();
            int n = ran.Next(10, 1000);
            int index = ran.Next(0, telStarts.Length - 1);
            string first = telStarts[index];
            string second = (ran.Next(100, 888) + 10000).ToString().Substring(1);
            string thrid = (ran.Next(1, 9100) + 10000).ToString().Substring(1);
            int ext = ran.Next(0, 9);
            return first + second + thrid + ext;
        }

        public static string GetEmail(string name)
        {
            Random ran = new Random();
            int n = ran.Next(0, 8);
            var ext = emailExt[n];
            return name + "@" + ext;
        }
        public static string GenerateSurname()
        {
            string name = string.Empty;
            string[] currentConsonant;
            string[] vowels = "a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,i,i,i,o,o,o,u,y,ee,ee,ea,ea,ey,eau,eigh,oa,oo,ou,ough,ay".Split(',');
            string[] commonConsonants = "s,s,s,s,t,t,t,t,t,n,n,r,l,d,sm,sl,sh,sh,th,th,th".Split(',');
            string[] averageConsonants = "sh,sh,st,st,b,c,f,g,h,k,l,m,p,p,ph,wh".Split(',');
            string[] middleConsonants = "x,ss,ss,ch,ch,ck,ck,dd,kn,rt,gh,mm,nd,nd,nn,pp,ps,tt,ff,rr,rk,mp,ll".Split(','); //Can't start
            string[] rareConsonants = "j,j,j,v,v,w,w,w,z,qu,qu".Split(',');
            Random rng = new Random(Guid.NewGuid().GetHashCode()); //http://codebetter.com/blogs/59496.aspx
            int[] lengthArray = new int[] { 2, 2, 2, 2, 2, 2, 3, 3, 3, 4 }; //Favor shorter names but allow longer ones
            int length = lengthArray[rng.Next(lengthArray.Length)];
            for (int i = 0; i < length; i++)
            {
                int letterType = rng.Next(1000);
                if (letterType < 775) currentConsonant = commonConsonants;
                else if (letterType < 875 && i > 0) currentConsonant = middleConsonants;
                else if (letterType < 985) currentConsonant = averageConsonants;
                else currentConsonant = rareConsonants;
                name += currentConsonant[rng.Next(currentConsonant.Length)];
                name += vowels[rng.Next(vowels.Length)];
                if (name.Length > 4 && rng.Next(1000) < 800) break; //Getting long, must roll to save
                if (name.Length > 6 && rng.Next(1000) < 950) break; //Really long, roll again to save
                if (name.Length > 7) break; //Probably ridiculous, stop building and add ending
            }
            int endingType = rng.Next(1000);
            if (name.Length > 6)
                endingType -= (name.Length * 25); //Don't add long endings if already long
            else
                endingType += (name.Length * 10); //Favor long endings if short
            if (endingType < 400) { } // Ends with vowel
            else if (endingType < 775) name += commonConsonants[rng.Next(commonConsonants.Length)];
            else if (endingType < 825) name += averageConsonants[rng.Next(averageConsonants.Length)];
            else if (endingType < 840) name += "ski";
            else if (endingType < 860) name += "son";
            else if (Regex.IsMatch(name, "(.+)(ay|e|ee|ea|oo)$") || name.Length < 5)
            {
                name = "Mc" + name.Substring(0, 1).ToUpper() + name.Substring(1);
                return name;
            }
            else name += "ez";
            name = name.Substring(0, 1).ToUpper() + name.Substring(1); //Capitalize first letter
            return name;
        }

        /// <summary>
        /// 获取无效的标识字符
        /// </summary>
        /// <returns>无效的标识字符</returns>
        public static string GetNaFlag()
        {
            if (flagNA == null)
            {
                lock (synacFlag)
                {
                    if (flagNA == null)
                    {
                        var target = Application.Current.FindResource("NA");
                        if (target == null)
                        {
                            flagNA = "N/A";
                        }

                        // TODO：暂时不使用资源文件
                        flagNA = "N/A";
                    }
                }
            }

            return flagNA;
        }

        /// <summary>
        /// 获取资源文件内容
        /// </summary>
        /// <param name="resourcePaths">
        /// 资源文件路径列表
        /// </param>
        /// <returns>
        /// 键值对对应字典
        /// </returns>
        public static Dictionary<string, string> GetStringResource(params string[] resourcePaths)
        {
            var resultDic = new Dictionary<string, string>();
            if (resourcePaths == null || resourcePaths.Length < 1)
            {
                return resultDic;
            }

            foreach (var path in resourcePaths)
            {
                try
                {
                    var fullPath = new FileInfo(path).FullName;
                    if (!File.Exists(fullPath))
                    {
                        Infrastructure.Log.TraceManager.Error.Write("Util", "资源文件不存在{0}", fullPath);
                    }

                    var resourceStream = new FileStream(fullPath, FileMode.Open);
                    var resources = (ResourceDictionary)XamlReader.Load(resourceStream);
                    foreach (DictionaryEntry item in resources)
                    {
                        if (resultDic.ContainsKey(item.Key.ToString()))
                        {
                            resultDic[item.Key.ToString()] = item.Value.ToString();
                            continue;
                        }

                        resultDic.Add(item.Key.ToString(), item.Value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Infrastructure.Log.TraceManager.Error.Write("Util", ex, "获取资源文件内容出错");
                    resultDic.Clear();
                    return resultDic;
                }
            }

            return resultDic;
        }

        /// <summary>
        /// 获取资源文件内容
        /// </summary>
        /// <param name="resouceUris">
        /// The resouce Uris.
        /// </param>
        /// <returns>
        /// 键值对对应字典
        /// </returns>
        public static Dictionary<string, string> GetStringResource(params Uri[] resouceUris)
        {
            var resultDic = new Dictionary<string, string>();
            try
            {
                foreach (var resourceUri in resouceUris)
                {
                    var resourceDictionary = Application.LoadComponent(resourceUri) as ResourceDictionary;
                    GetResourceItem(resourceDictionary, resultDic);
                }
            }
            catch (Exception exception)
            {
                Infrastructure.Log.TraceManager.Error.Write("Util", exception, "通过资源路径获取资源文件出错");
            }

            return resultDic;
        }

        /// <summary>
        /// 将文件夹迁移到用户路径下
        /// </summary>
        /// <param name="path">
        /// 原路径
        /// </param>
        /// <param name="tarDicName">
        /// 目标目录名称
        /// </param>
        /// <returns>
        /// 用户路径
        /// </returns>
        public static string MoveDicToUserPath(string path, string tarDicName)
        {
            var sourcePathInfo = new DirectoryInfo(path);
            string targetDicPath = Path.Combine(
                System.IO.Path.GetTempPath(),
                Process.GetCurrentProcess().MainModule.ModuleName,
                tarDicName);
            if (!System.IO.Directory.Exists(targetDicPath))
            {
                Directory.CreateDirectory(targetDicPath);
            }

            string targetDirecPath = Path.Combine(targetDicPath, sourcePathInfo.Name);
            CopyDirectory(path, targetDirecPath);
            return targetDirecPath;
        }

        /// <summary>
        /// 将文件迁移到用户路径下
        /// </summary>
        /// <param name="path">
        /// 原路径
        /// </param>
        /// <param name="tarDicName">
        /// 目标目录名称
        /// </param>
        /// <returns>
        /// 用户路径
        /// </returns>
        public static string MoveFileToUserPath(string path, string tarDicName)
        {
            var sourcePathInfo = new FileInfo(path);
            string targetDicPath = Path.Combine(
                System.IO.Path.GetTempPath(),
                Process.GetCurrentProcess().MainModule.ModuleName,
                tarDicName);
            if (!System.IO.Directory.Exists(targetDicPath))
            {
                Directory.CreateDirectory(targetDicPath);
            }

            string targetPath = Path.Combine(targetDicPath, sourcePathInfo.Name);
            File.Copy(path, targetPath, true);
            return targetPath;
        }

        /// <summary>
        /// 注册证书
        /// </summary>
        /// <param name="certPath">
        /// 证书路径
        /// </param>
        public static void RegCert(string certPath)
        {
            var certificate = new X509Certificate2(certPath);
            if (certificate.SubjectName.Name == null)
            {
                throw new Exception("证书异常，找不到其证书SubjectName");
            }

            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            var certs = store.Certificates.Find(
                X509FindType.FindBySubjectName,
                certificate.SubjectName.Name.Remove(0, 3),
                false);
            if (certs.Count != 0)
            {
                return;
            }

            store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
            Infrastructure.Log.TraceManager.Info.Write("Util", "导入证书:{0}成功", certificate.SubjectName.Name.Remove(0, 3), false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 复制文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="sourcePath">
        /// 待复制的文件夹路径
        /// </param>
        /// <param name="destinationPath">
        /// 目标路径
        /// </param>
        private static void CopyDirectory(string sourcePath, string destinationPath)
        {
            var info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string destName = Path.Combine(destinationPath, fsi.Name);

                // 如果是文件，复制文件
                if (fsi is System.IO.FileInfo)
                {
                    File.Copy(fsi.FullName, destName, true);
                }
                else
                {
                    // 如果是文件夹，新建文件夹，递归
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        /// <summary>
        /// 获取资源字典中的项
        /// </summary>
        /// <param name="resourceDictionary">
        /// 资源字典
        /// </param>
        /// <param name="resultDic">
        /// 目标字典集合
        /// </param>
        private static void GetResourceItem(ResourceDictionary resourceDictionary, Dictionary<string, string> resultDic)
        {
            if (resourceDictionary == null)
            {
                return;
            }

            foreach (DictionaryEntry item in resourceDictionary)
            {
                if (!resultDic.ContainsKey(item.Key.ToString()))
                {
                    resultDic.Add(item.Key.ToString(), item.Value.ToString());
                }
            }

            foreach (var dictionary in resourceDictionary.MergedDictionaries)
            {
                GetResourceItem(dictionary, resultDic);
            }
        }

        #endregion
    }
}