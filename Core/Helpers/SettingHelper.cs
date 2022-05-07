using Newtonsoft.Json;
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Core;
using SquareMinecraftLauncher.Core.Forge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using windows;

namespace FastX.Core.Helpers
{
    public enum ModInformationType
    {
        /// <summary>
        /// Mod名
        /// </summary>
        Name,
        /// <summary>
        /// Mod所在的列表
        /// </summary>
        FullName,
        /// <summary>
        /// Mod创建的时间
        /// </summary>
        CreationTime
    }
    public struct MEMORY_INFO
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
    }
    /// <summary>
    /// 设置帮助器
    /// </summary>
    public class SettingHelper
    {
        
        internal string gamepath;
        HttpHelper web =new HttpHelper();
        protected bool vp;
        private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        private static List<mc> mcV = new List<mc>();
        protected string mainpath;
        protected List<string> lstr = new List<string>();

        #region 抽象方法

        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        #endregion

        /// <summary>
        /// 生成一个用于第三方验证的参数
        /// </summary>
        /// <param name="path">authlib-injector外挂文件路径</param>
        /// <param name="uri">皮肤站认证地址</param>
        /// <returns>生成的jvm参数</returns>
        public string GenerateParameterofYggdrasil(string path, string uri)
        {
            string s = "-javaagent:" + path + "=" + uri;
            return s;
        }

        /// <summary>
        /// 获取Mod列表
        /// </summary>
        /// <param name="rootpath">.minecraft目录</param>
        /// <param name="VersionIsolation">版本隔离</param>
        /// <param name="VersionName">指定的版本（版本隔离选项未开可忽略）</param>
        /// <param name="modInformationType">找到的mod信息类型</param>
        /// <returns>找到的mod列表</returns>
        public string[] GetModList(string rootpath,bool VersionIsolation, string VersionName, ModInformationType modInformationType)
        {
            #region 版本隔离判断
            if (VersionIsolation == true)
            {
                mainpath = rootpath + @"\" + "versions" + @"\" + VersionName + @"\" + "mods";
                Console.WriteLine(mainpath);
            }
            else
            {
                mainpath = rootpath + @"\" + "mods";
                Console.WriteLine(mainpath);
            }

            #endregion

            string s = null;
            DirectoryInfo d = new DirectoryInfo(mainpath);
            FileInfo[] Files = d.GetFiles("*.jar");
            foreach (FileInfo file in Files)
            {
                if(modInformationType == ModInformationType.Name)
                {
                    s = file.Name;
                }
                else if (modInformationType == ModInformationType.FullName)
                    s = file.FullName;
                else if (modInformationType == ModInformationType.CreationTime)
                    s = file.CreationTime.ToString();
                lstr.Add(s);
                Console.WriteLine(s);
            }
            return lstr.ToArray();
        }

        /// <summary>
        /// 取.minecraft所有存在的版本
        /// </summary>
        /// <param name="Path">.minecraft目录 (如与启动器同目录的话请填null或"")</param>
        /// <returns>找到的游戏列表</returns>
        public GameInfoType[] FindAllGame(string Path)
        {
            if(Path != null ||Path != string.Empty)
            {
                System.Directory.Files = Path != null ? Path : System.IO.Directory.GetCurrentDirectory();
                gamepath = Path;
            }
            else
            {
                gamepath = @"\.minecraft\versions";
            }
            new NewsHelper();
            List<GameInfoType> list = new List<GameInfoType>();
            if (!System.IO.Directory.Exists(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions"))
            {
                throw new LauncherException("没有找到任何版本");
            }
            foreach (string str in System.IO.Directory.GetDirectories(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions"))
            {
                string str2 = this.SLC.app(str, Convert.ToChar(@"\"), "versions");
                if (File.Exists(str + @"\" + str2 + ".jar") && File.Exists(str + @"\" + str2 + ".json"))
                {
                    ForgeY.Root root = new ForgeY.Root();
                    try
                    {
                        root = JsonConvert.DeserializeObject<ForgeY.Root>(this.SLC.GetFile(str + @"\" + str2 + ".json"));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    GameInfoType item = new GameInfoType { Path = str, Version = str2 };
                    if (!this.vp)
                    {
                        new Thread(new ThreadStart(this.SLC.MCVersion)) { IsBackground = true }.Start();
                        this.vp = true;
                    }
                    if (mcV.ToArray().Length == 0)
                    {
                        ForgeInstallCore.Delay(2000);
                        char[] separator = new char[] { '|' };
                        string[] strArray2 = this.SLC.GetFile(@".minecraft\version.Sika").Split(separator);
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            char[] chArray2 = new char[] { '&' };
                            string[] strArray3 = strArray2[i].Split(chArray2);
                            mc mc = new mc
                            {
                                version = strArray3[0],
                                url = strArray3[1]
                            };
                            mcV.Add(mc);
                        }
                    }
                    try
                    {
                        foreach (mc mc2 in mcV)
                        {
                            if (mc2.url == root.downloads.client.url)
                            {
                                item.Id = mc2.version;
                                break;
                            }
                        }
                        if (item.Id != null)
                        {
                            list.Add(item);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 从用户电脑寻找可能的javaw.exe路径
        /// </summary>
        /// <returns>可能构成的列表</returns>
        public List<JavaInfo> GetJavaPath()
        {
            SearchBase searchBase = new SearchBase();
            searchBase.addSubDirectory();
            return searchBase.vs;
        }

        /// <summary>
        /// 从Java版本自动分配最合适内存
        /// </summary>
        /// <param name="JavaPath">Java路径</param>
        /// <returns></returns>
        public MemoryInformation GetMemoryInfo(string JavaPath)
        {
            MEMORY_INFO meminfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref meminfo);
            MemoryInformation information = new MemoryInformation
            {
                TotalMemory = (int)(meminfo.dwTotalVirtual / 0x100000)
            };
            if (information.TotalMemory == 0)
            {
                information.AppropriateMemory = 512;
                return information;
            }
            if (this.GetOSBit() == 64)
            {
                if (JavaPath.IndexOf("x86") >= 0)
                {
                    information.AppropriateMemory = 512;
                    return information;
                }
                information.AppropriateMemory = ((1024 * information.TotalMemory) / 1024) / 2;
                return information;
            }
            if (information.TotalMemory <= 1024)
            {
                information.AppropriateMemory = 512;
                return information;
            }
            information.AppropriateMemory = 1024;
            return information;
        }

        /// <summary>
        /// 获取本机系统位数
        /// </summary>
        /// <returns></returns>
        public int GetOSBit()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return 0x40;
            }
            return 0x20;
        }

        /// <summary>
        /// 获取正版用户皮肤
        /// </summary>
        /// <returns></returns>
        public string GetSkin(string uuid)
        {
            string json = web.getHtml("https://sessionserver.mojang.com/session/minecraft/profile/" + uuid);
            var obj1 = JsonConvert.DeserializeObject<MinecraftSkin.Root>(json);
            byte[] outputb = Convert.FromBase64String(obj1.properties[0].value);
            string orgStr = Encoding.Default.GetString(outputb);
            var obj = JsonConvert.DeserializeObject<MinecraftSkinItem.Root>(orgStr);
            return obj.textures.SKIN.url;

        }

    }
}
