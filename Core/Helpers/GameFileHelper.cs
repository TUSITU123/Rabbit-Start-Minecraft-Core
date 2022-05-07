using FastX.Class.Models;
using json4;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Core.Forge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Core.Helpers
{
    /// <summary>
    /// 游戏文件帮助器
    /// </summary>
    public class GameFileHelper
    {
        #region
        protected static string DSI = "https://bmclapi2.bangbang93.com/libraries/";
        private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        #endregion

        /// <summary>
        /// Optifine是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool OptifineExist(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "optifine") && (strArray[1] == "OptiFine"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Optifine是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="OptifineVersion">返回Optifine版本</param>
        /// <returns></returns>
        public bool OptifineExist(string version, ref string OptifineVersion)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "optifine") && (strArray[1] == "OptiFine"))
                    {
                        OptifineVersion = strArray[2];
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 取对应版本的安装Forge版本
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public string GetLocalForgeVersion(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if (strArray[0] == "net.minecraftforge")
                    {
                        return strArray[2];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// forge是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool ForgeExist(string version)
        {
            return (GetLocalForgeVersion(version) != null);
        }

        /// <summary>
        /// forge是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool ForgeExist(string version, ref string ForgeVersion)
        {
            ForgeVersion = GetLocalForgeVersion(version);
            if (ForgeVersion != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否存在Fabric
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="FabricVersion">返回 Fabric版本</param>
        /// <returns></returns>
        public bool FabricExist(string version, ref string FabricVersion)
        {
            var libraries = JsonConvert.DeserializeObject<ForgeY.Root>(SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json"));
            foreach (var i in libraries.libraries)
            {
                string[] n = i.name.Split(':');
                if (n[0] == "net.fabricmc")
                {
                    FabricVersion = n[2];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否存在Fabric
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool FabricExist(string version)
        {
            string a = "";
            return FabricExist(version, ref a);
        }

        /// <summary>
        /// 取Liteloader是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool LiteloaderExist(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "com.mumfrey") && (strArray[1] == "liteloader"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Liteloader是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="LiteloaderVersion">返回Liteloader版本</param>
        /// <returns></returns>
        public bool LiteloaderExist(string version, ref string LiteloaderVersion)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "com.mumfrey") && (strArray[1] == "liteloader"))
                    {
                        LiteloaderVersion = strArray[2];
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 取缺少的Asset
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public DownloadModel[] GetMissingAsset(string version)
        {
            List<DownloadModel> list = new List<DownloadModel>();
            foreach (DownloadModel download in this.GetAllTheAsset(version))
            {
                string path = download.path;
                if (this.SLC.FileExist(path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 取缺少的依赖库包括（Natives）
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public DownloadModel[] GetMissingFile(string version)
        {

            List<DownloadModel> list = new List<DownloadModel>();
            foreach (DownloadModel download in this.GetAllFile(version))
            {
                if (this.SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 取缺少的依赖库
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public DownloadModel[] GetMissingLibrary(string version)
        {
            List<DownloadModel> list = new List<DownloadModel>();
            foreach (DownloadModel download in this.GetAllLibrary(version))
            {
                if (this.SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 取缺少的Natives
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public DownloadModel[] GetMissingNatives(string version)
        {

            List<DownloadModel> list = new List<DownloadModel>();
            foreach (DownloadModel download in this.GetAllNatives(version))
            {
                if (this.SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 取所有依赖库包括（Natives）
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public DownloadModel[] GetAllFile(string version)
        {
            DownloadModel[] downloadArray;
            MinecraftDownload download = new MinecraftDownload();

            try
            {
                var root = this.SLC.versionjson<json4.Root>(version);
                List<DownloadModel> list = new List<DownloadModel>();
                string dSI = DSI;
                if (DSI == "Minecraft")
                {
                    dSI = "https://libraries.minecraft.net/";
                }
                else if (DSI == null)
                {
                    dSI = "https://bmclapi2.bangbang93.com/libraries/";
                }
                foreach (LibrariesItem item in root.libraries)
                {
                    string str2 = null;
                    if (item.natives != null)
                    {
                        if (item.natives.windows == null)
                        {
                            continue;
                        }
                        str2 = this.SLC.libAnalysis(item.name, false, item.natives.windows);
                    }
                    else
                    {
                        str2 = this.SLC.libAnalysis(item.name, false, "");
                    }
                    DownloadModel download2 = new DownloadModel
                    {
                        name = item.name,
                        mainClass = root.mainClass
                    };
                    string[] strArray = item.name.Split(':');
                    if (strArray[1].IndexOf("lwjgl") >= 0 && strArray[2] == "3.2.1") continue;
                    download2.Url = dSI + str2.Replace('\\', Convert.ToChar("/"));
                    download2.path = System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2;
                    if ((item.downloads != null) && (item.downloads.artifact != null))
                    {
                        if (item.downloads.artifact.url.IndexOf("libraries.minecraft.net") < 0 && item.downloads.artifact.url.IndexOf("files.minecraftforge.net") < 0)
                        {
                            if (item.downloads.artifact.url != "" && item.downloads.artifact.url != null && item.downloads.artifact.url.IndexOf(" ") < 0)
                            {
                                download2.Url = item.downloads.artifact.url + str2.Replace('\\', Convert.ToChar("/"));
                            }
                        }
                        if ((item.downloads.artifact.url.IndexOf("files.minecraftforge.net") != -1))
                        {
                            char[] chArray2 = new char[] { ':' };
                            string[] strArray2 = item.name.Split(chArray2);
                            string str3 = strArray2[2];
                            if (strArray2[2].IndexOf('-') != -1)
                            {
                                string[] urlArray = strArray2[2].Split('-');
                                if (urlArray.Length == 3)
                                {
                                    str3 = urlArray[0] + "-" + urlArray[1];
                                }
                                else
                                {
                                    str3 = urlArray[0];
                                    if (urlArray[1].IndexOf('.') != -1) str3 = strArray2[2];
                                }
                            }
                            string[] textArray1 = new string[] { strArray2[0].Replace('.', Convert.ToChar(@"\")), @"\", strArray2[1], @"\", str3, @"\", strArray2[1], "-", strArray2[2], ".jar" };
                            str2 = string.Concat(textArray1);
                            download2.Url = "https://bmclapi2.bangbang93.com/maven/" + str2.Replace('\\', Convert.ToChar("/"));
                        }

                        else if (strArray[1] == "liteloader")
                        {
                            download2.Url = item.downloads.artifact.url;
                        }
                    }
                    if (strArray[1] == "OptiFine")
                    {
                        string OpVersion = strArray[2];
                        MCDownload op = download.DownloadOptifine(version, "OptiFine_" + OpVersion + ".jar");
                        download2.path = System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\optifine\OptiFine\" + OpVersion + @"\" + OpVersion + ".jar";
                        download2.Url = op.Url;
                        download2.name = op.name;
                    }
                    if (strArray[1] == "forge")
                    {
                        char[] chArray4 = new char[] { '-' };
                        string[] strArray3 = strArray[2].Split(chArray4);
                        download2.Url = download.ForgeCoreDownload(version, strArray3[1]).Url;
                        list.Add(download2);
                        download2.name = "";
                    }
                    list.Add(download2);
                }
                //if (ForgeExist(version))
                //    downloadArray = this.SLC.screening(list.ToArray());
                //else
                downloadArray = list.ToArray();
            }
            catch (Exception)
            {
                throw new LauncherException("版本有问题，请重新下载");
            }
            return downloadArray;
        }

        /// <summary>
        /// 取所有依赖库
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public DownloadModel[] GetAllLibrary(string version)
        {
            List<DownloadModel> list = new List<DownloadModel>();
            DownloadModel[] allFile = this.GetAllFile(version);
            DownloadModel[] allNatives = this.GetAllNatives(version);
            for (int i = 0; i < allFile.Length; i++)
            {
                int index = 0;
                while (index < allNatives.Length)
                {
                    if (allFile[i].path == allNatives[index].path)
                    {
                        break;
                    }
                    index++;
                }
                if (index == allNatives.Length)
                {
                    list.Add(allFile[i]);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 取所有Natives
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public DownloadModel[] GetAllNatives(string version)
        {
            DownloadModel[] downloadArray;

            try
            {
                List<DownloadModel> list = new List<DownloadModel>();
                string dSI = DSI;
                if (DSI == "Minecraft")
                {
                    dSI = "https://libraries.minecraft.net/";
                }
                else if (DSI == null)
                {
                    dSI = "https://bmclapi2.bangbang93.com/libraries/";
                }
                foreach (JToken token in this.SLC.versionjson(version)["libraries"])
                {
                    if ((token["natives"] != null) && (token["natives"]["windows"] != null))
                    {
                        string str2 = this.SLC.libAnalysis(token["name"].ToString(), false, token["natives"]["windows"].ToString());
                        DownloadModel item = new DownloadModel
                        {
                            Url = dSI + str2.Replace('\\', '/'),
                            path = System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2
                        };
                        list.Add(item);
                    }
                }
                downloadArray = list.ToArray();
            }
            catch (Exception)
            {
                throw new LauncherException("版本有问题，请重新下载");
            }
            return downloadArray;
        }

        /// <summary>
        /// 取所有Asset
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public DownloadModel[] GetAllTheAsset(string version)
        {
            try
            {
                HttpHelper web =new HttpHelper();
                SLC.SetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\assets");
                SLC.SetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\assets\indexes");
                var jo = SLC.versionjson<json4.Root>(version);
                string json = web.getHtml(jo.AssetIndex.url.Replace("launchermeta.mojang.com", "bmclapi2.bangbang93.com"));
                var FileName = jo.AssetIndex.url.Split('/');
                SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\assets\indexes\" + FileName[FileName.Length - 1], json);
                mcbbs.mcbbsnews mcbbs = new mcbbs.mcbbsnews();
                string[] str = new string[0];
                List<DownloadModel> str2 = new List<DownloadModel>();
                var j = JObject.Parse(json).Value<JObject>("objects");
                JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
                string jstr;
                string dsi = "https://bmclapi2.bangbang93.com/assets";
                if (DSI != null && DSI != "Minecraft") dsi = "https://download.mcbbs.net/assets";
                foreach (var o in j.Properties())
                {
                    jstr = o.Name;
                    DownloadModel assets = new DownloadModel();
                    var hash = json1["objects"][o.Name]["hash"].ToString();
                    assets.path = System.Directory.GetCurrentDirectory() + @"\.minecraft\assets\objects\" + hash[0] + hash[1] + "\\" + hash;
                    assets.Url = dsi + @"/" + hash[0] + hash[1] + "/" + hash;
                    str2.Add(assets);
                }
                return str2.ToArray();
            }
            catch (Exception)
            {
                throw new LauncherException("无法连接网络");
            }
        }

    }
}
