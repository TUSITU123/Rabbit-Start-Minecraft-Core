using FastX.Class.Models;
using json4;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SquareMinecraftLauncher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Core.Helpers
{
    public class GameDownloadHelper
    {
        #region 实例化集合
        HttpHelper http = new HttpHelper();
        SettingHelper setting = new SettingHelper();
        #endregion

        #region 下载方法

        /// <summary>
        /// 下载Liteloader
        /// </summary>
        public DownloadModel DownloadLiteloader(string LiteloaderVersion)
        {
            DownloadModel download = new DownloadModel
            {
                path = System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\liteloader-" + LiteloaderVersion + ".jar"
            };
            string[] textArray1 = new string[] { "https://bmclapi2.bangbang93.com/maven/com/mumfrey/liteloader/", LiteloaderVersion, "/liteloader-", LiteloaderVersion, ".jar" };
            download.Url = string.Concat(textArray1);
            return download;
        }
        /// <summary>
        /// 下载Optifine
        /// </summary>
        public DownloadModel DownloadOptifine(string version, string filename)
        {
            GameInfoType[] allTheExistingVersion = new Tools().GetAllTheExistingVersion();
            foreach (GameInfoType version2 in allTheExistingVersion)
            {
                if (version2.Version == version)
                {
                    version = version2.Id;
                    break;
                }
                if (version2.Version == allTheExistingVersion[allTheExistingVersion.Length - 1].Version)
                {
                    throw new LauncherException("未找到该版本");
                }
            }
            string[] textArray2 = new string[] { "optifine:OptiFine:", filename.Replace("preview_", "").Replace("OptiFine_", "") };
            if (filename.IndexOf("pre") > 0) filename = "preview_" + filename;
            return new DownloadModel { path = System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\" + filename, Url = "https://bmclapi2.bangbang93.com/maven/com/optifine/" + version + "/" + filename, name = string.Concat(textArray2).Replace(".jar", "") };
        }
        /// <summary>
        /// 下载
        /// </summary>
        internal DownloadModel ForgeCoreDownload(string version, string ForgeVersion)
        {
            DownloadModel download = new DownloadModel();
            string str = version;
            version = "";
            foreach (GameInfoType version2 in new Tools().GetAllTheExistingVersion())
            {
                if (version2.Version == str)
                {
                    version = version2.Id;
                    break;
                }
            }
            if (version == "")
            {

                throw new LauncherException("未找到该版本");
            }
            string str2 = "https://bmclapi2.bangbang93.com";
            if (Tools.DSI == "Minecraft")
            {
                str2 = "https://files.minecraftforge.net";
            }
            char[] separator = new char[] { '.' };
            string[] textArray3 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge-", version, "-", ForgeVersion, "-universal.jar" };
            download.path = string.Concat(textArray3);
            string[] textArray4 = new string[] { str2, "/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "/forge-", version, "-", ForgeVersion, "-universal.jar" };
            download.Url = string.Concat(textArray4);
            try
            {
                new Download().CreateGetHttpResponse(download.Url);
            }
            catch (Exception)
            {
                string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge-", version, "-", ForgeVersion, "-", version, "-universal.jar" };
                download.path = string.Concat(textArray1);
                string[] textArray2 = new string[] { "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "-", version, "/forge-", version, "-", ForgeVersion, "-", version, "-universal.jar" };
                download.Url = string.Concat(textArray2);
            }
            return download;
        }
        /// <summary>
        /// 下载Forge
        /// </summary>
        public DownloadModel ForgeDownload(string version, string ForgeVersion)
        {
            DownloadModel download = new DownloadModel();
            string str = version;
            version = "";
            foreach (GameInfoType version2 in new Tools().GetAllTheExistingVersion())
            {
                if (version2.Version == str)
                {
                    version = version2.Id;
                    break;
                }
            }
            if (version == "")
            {
                throw new LauncherException("未找到该版本");
            }
            char[] separator = new char[] { '.' };
            if (Convert.ToInt32(version.Split(separator)[1]) < 9)
            {
                return this.ForgeCoreDownload(str, ForgeVersion);
            }
            string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge - ", version, " - ", ForgeVersion, "-installer.jar" };
            download.path = string.Concat(textArray1);
            string[] textArray2 = new string[] { "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "/forge-", version, "-", ForgeVersion, "-installer.jar" };
            download.Url = string.Concat(textArray2);
            try
            {
                new Download().CreateGetHttpResponse(download.Url);
            }
            catch (Exception)
            {
                string[] textArray3 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge-", version, "-", ForgeVersion, "-", version, "-installer.jar" };
                download.path = string.Concat(textArray3);
                string[] textArray4 = new string[] { "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "-", version, "/forge-", version, "-", ForgeVersion, "-", version, "-installer.jar" };
                download.Url = string.Concat(textArray4);
            }
            return download;
        }
        /// <summary>
        /// 下载Java
        /// </summary>
        public DownloadModel JavaFileDownload()
        {
            string str;
            if (new Tools().GetOSBit() == 0x20)
            {
                str = "jre_x86.exe";
            }
            else
            {
                str = "jre_x64.exe";
            }
            return new DownloadModel { Url = "https://bmclapi.bangbang93.com/java/" + str, path = System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\" + str };
        }
        /// <summary>
        /// 本体Jar下载
        /// </summary>
        public DownloadModel JarDownload(string version)
        {
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            DownloadModel download = new DownloadModel();
            if (Tools.DSI == "Minecraft")
            {
                if (Tools.mcV.ToArray().Length == 0)
                {
                    char[] separator = new char[] { '|' };
                    string[] strArray = SLC.GetFile(@".minecraft\version.Sika").Split(separator);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        char[] chArray2 = new char[] { '&' };
                        string[] strArray2 = strArray[i].Split(chArray2);
                        mc item = new mc
                        {
                            version = strArray2[0],
                            url = strArray2[1]
                        };
                        Tools.mcV.Add(item);
                    }
                }
                foreach (mc mc2 in Tools.mcV)
                {
                    if (mc2.version == version)
                    {
                        download.Url = mc2.url;
                        string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".jar" };
                        download.path = string.Concat(textArray1);
                    }
                }
                return download;
            }
            download.Url = "https://download.mcbbs.net/version/" + version + "/client";
            string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".jar" };
            download.path = string.Concat(textArray2);
            return download;
        }
        /// <summary>
        /// 本体Json下载
        /// </summary>
        public DownloadModel MCjsonDownload(string version)
        {
            HttpHelper web = new HttpHelper();
            DownloadModel download = new DownloadModel();
            if (Tools.DSI == "Minecraft")
            {
                string str = web.getHtml("https://launchermeta.mojang.com/mc/game/version_manifest.json");
                if (str != null)
                {
                    foreach (VersionsItem item in JsonConvert.DeserializeObject<Class.Models.Root>(str).versions)
                    {
                        if (item.id == version)
                        {
                            download.Url = item.url;
                            string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
                            download.path = string.Concat(textArray1);
                        }
                    }
                    return download;
                }
                download.Url = "https://bmclapi2.bangbang93.com/version/" + version + "/json";
                string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
                download.path = string.Concat(textArray2);
                return download;
            }
            download.Url = "https://bmclapi2.bangbang93.com/version/" + version + "/json";
            string[] textArray3 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
            download.path = string.Concat(textArray3);
            return download;
        }
        /// <summary>
        /// 服务器本体Jar下载
        /// </summary>
        public DownloadModel CoreDownload(string version)
        {
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            DownloadModel download = new DownloadModel();
            if (Tools.DSI == "Minecraft")
            {
                if (Tools.mcV.ToArray().Length == 0)
                {
                    char[] separator = new char[] { '|' };
                    string[] strArray = SLC.GetFile(@"version.Sika").Split(separator);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        char[] chArray2 = new char[] { '&' };
                        string[] strArray2 = strArray[i].Split(chArray2);
                        mc item = new mc
                        {
                            version = strArray2[0],
                            url = strArray2[1]
                        };
                        Tools.mcV.Add(item);
                    }
                }
                foreach (mc mc2 in Tools.mcV)
                {
                    if (mc2.version == version)
                    {
                        download.Url = mc2.url;
                        string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\versions\", version, @"\", version, ".jar" };
                        download.path = string.Concat(textArray1);
                    }
                }
                return download;
            }
            download.Url = "https://download.mcbbs.net/version/" + version + "/server";
            string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\versions\", version, @"\", version, ".jar" };
            download.path = string.Concat(textArray2);
            return download;
        }

        #endregion

        #region 列表请求方法

        /// <summary>
        /// 获取Minecraft版本列表
        /// </summary>
        /// <returns></returns>
        public async Task<VersionList[]> GetMCVersionList()
        {
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            string text = null;
            await Task.Factory.StartNew(() =>
            {
                text = this.http.getHtml("https://launchermeta.mojang.com/mc/game/version_manifest.json");
            });
            if (text == null)
            {
                throw new LauncherException("请求失败");
            }
            List<VersionList> list = new List<VersionList>();
            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(new NewsHelper().TakeTheMiddle(text, "\"versions\":", "]}") + "]"))
            {
                string str2 = SLC.MCVersionAnalysis(token["type"].ToString());
                VersionList item = new VersionList
                {
                    type = str2,
                    id = token["id"].ToString(),
                    releaseTime = token["releaseTime"].ToString()
                };
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取Forge版本列表
        /// </summary>
        /// <returns></returns>
        public string[] GetForgeList()
        {
            string str = this.http.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft");
            if (str == null)
            {
                throw new LauncherException("请求失败");
            }
            return JsonConvert.DeserializeObject<List<string>>(str).ToArray();
        }

        /// <summary>
        /// 取对应版本所有Forge列表
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public async Task<ForgeList[]> GetForgeList(string version)
        {
            foreach (GameInfoType version2 in setting.FindAllGame(setting.gamepath))
            {
                if (version2.Version == version)
                {
                    version = version2.Id;
                    break;
                }
            }
            string str2 = null;
            await Task.Factory.StartNew(() =>
            {
                str2 = this.http.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft/" + version);
            });
            if ((str2 != "[]") && (str2 != null))
            {
                List<ForgeList> list = new List<ForgeList>();
                foreach (JToken token in (JArray)JsonConvert.DeserializeObject(str2))
                {
                    ForgeList item = new ForgeList
                    {
                        version = token["mcversion"].ToString(),
                        ForgeVersion = token["version"].ToString(),
                        ForgeTime = token["modified"].ToString()
                    };
                    list.Add(item);
                }
                return list.ToArray();
            }
            if (str2 == null)
            {
                throw new LauncherException("访问失败");
            }
            throw new LauncherException("版本有误或目前没有该版本");
        }

        /// <summary>
        /// 取Liteloader列表
        /// </summary>
        /// <returns></returns>
        public async Task<LiteloaderList[]> GetLiteloaderList()
        {
            List<LiteloaderList> list = new List<LiteloaderList>();
            string text1 = null;
            await Task.Factory.StartNew(() =>
            {
                text1 = http.getHtml("https://bmclapi2.bangbang93.com/liteloader/list");
            });
            if (text1 == null)
            {
                throw new LauncherException("获取失败");
            }
            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(text1))
            {
                LiteloaderList item = new LiteloaderList();
                List<Lib> list3 = new List<Lib>();
                item.version = token["version"].ToString();
                item.mcversion = token["mcversion"].ToString();
                foreach (JToken token2 in token["build"]["libraries"])
                {
                    Lib lib = new Lib
                    {
                        name = token2["name"].ToString()
                    };
                    list3.Add(lib);
                }
                item.lib = list3.ToArray();
                item.tweakClass = token["build"]["tweakClass"].ToString();
                list.Add(item);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 取OptiFine版本
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public async Task<OptiFineList[]> GetOptiFineList(string version)
        {
            GameInfoType[] allTheExistingVersion = setting.FindAllGame(setting.gamepath);
            foreach (GameInfoType version2 in allTheExistingVersion)
            {
                if (version2.Version == version)
                {
                    version = version2.Id;
                    break;
                }
            }
            List<OptiFineList> list = new List<OptiFineList>();
            string str = null;
            await Task.Factory.StartNew(() =>
            {
                str = this.http.getHtml("https://bmclapi2.bangbang93.com/optifine/" + version);
            });
            switch (str)
            {
                case null:
                    throw new LauncherException("获取失败");

                case "[]":
                    throw new LauncherException("OptiFine不支持该版本");
            }
            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(str))
            {
                OptiFineList item = new OptiFineList
                {
                    mcversion = token["mcversion"].ToString(),
                    filename = token["filename"].ToString(),
                    type = token["type"].ToString(),
                    patch = token["patch"].ToString()
                };
                list.Add(item);
            }
            return list.ToArray();
        }

        #endregion

        #region 安装方法
        /// <summary>
        /// Optifine安装
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="patch">patch</param>
        /// <param name="javaPath">Java路径</param>
        /// <returns></returns>
        public async Task<bool> OptifineInstall(string version, string patch, string javaPath)
        {
            OptiFineList[] optiFineList = await GetOptiFineList(version);
            GameInfoType[] allTheExistingVersion = setting.FindAllGame(setting.gamepath);
            string str = version;
            foreach (GameInfoType version2 in allTheExistingVersion)
            {
                if (version2.Version == version)
                {
                    version = version2.Id;
                    break;
                }
                if (version2.Version == allTheExistingVersion[allTheExistingVersion.Length - 1].Version)
                {
                    throw new LauncherException("未找到该版本");
                }
            }
            OptiFineList list = new OptiFineList();
            foreach (OptiFineList list2 in optiFineList)
            {
                if (list2.mcversion == version)
                {
                    list = list2;
                }
            }
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            OptifineCore core = new OptifineCore();
            SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", await core.OptifineJson(str, list, javaPath));
            return true;
        }
        /// <summary>
        /// 卸载扩展包
        /// </summary>
        /// <param name="ExpansionPack">扩展包类型</param>
        /// <param name="version">版本</param>
        /// <param name="java">Java</param>
        public void UninstallTheExpansionPack(ExpansionPack ExpansionPack, string version, string java)
        {
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            string str = version;
            string[] strArray = new string[0];
            using (List<LibrariesItem>.Enumerator enumerator = SLC.versionjson<Root1>(str).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray2 = enumerator.Current.name.Split(separator);
                    if ((strArray2[0] == "optifine") && (strArray2[1] == "OptiFine"))
                    {
                        char[] chArray2 = new char[] { '_' };
                        strArray = strArray2[2].Split(chArray2);
                    }
                }
            }
            GameDownloadHelper Game =new GameDownloadHelper();
            GameFileHelper game = new GameFileHelper();
            bool flag = game.LiteloaderExist(version);
            DownloadModel download2 = Game.MCjsonDownload(version);
            string text = this.http.getHtml(download2.Url);
            switch (ExpansionPack)
            {
                case ExpansionPack.Forge:
                    if (!game.ForgeExist(version))
                    {
                        throw new LauncherException("没有安装Forge");
                    }
                    SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    if (strArray.Length != 0)
                    {
                        SLC.opKeep(version, strArray[strArray.Length - 1], java);
                    }
                    if (!flag)
                    {
                        break;
                    }

                    SLC.liKeep(str);
                    return;

                case ExpansionPack.Liteloader:
                    if (!game.LiteloaderExist(version))
                    {
                        throw new LauncherException("没有安装Liteloader");
                    }
                    if (!SLC.ForgeKeep(version, text))
                    {
                        SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    }
                    if (strArray.Length == 0)
                    {
                        break;
                    }
                    SLC.opKeep(str, strArray[strArray.Length - 1], java);
                    return;

                case ExpansionPack.Optifine:
                    if (!game.OptifineExist(version))
                    {
                        throw new LauncherException("没有安装Optifine");
                    }
                    if (!SLC.ForgeKeep(version, text))
                    {
                        SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    }
                    if (!flag)
                    {
                        break;
                    }
                    SLC.liKeep(version);
                    return;
                case ExpansionPack.Fabric:
                    if (game.FabricExist(version))
                    {
                        fabricUninstall fabricUninstall = new fabricUninstall();
                        string Uninstall = fabricUninstall.Uninstall(version);
                        SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", Uninstall);
                    }
                    else
                    {
                        throw new LauncherException("没有安装Fabric");
                    }
                    break;
                default:
                    return;
            }
        }

        #endregion
    }
}
