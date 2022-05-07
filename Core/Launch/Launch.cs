namespace System
{
    using FastX.Class.Models;
    using FastX.Core;
    using FastX.Core.Authenticator;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SquareMinecraftLauncher;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using windows;

    public sealed class Game
    {
        internal string mininc ="512";
        internal int hei = 450;
        internal int wih = 856;
        internal string qw = "FastX";
        protected OfflineAuthenticator offline =new OfflineAuthenticator();
        private Process process = new Process();
        private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        private ProcessStartInfo start = new ProcessStartInfo();
        private Tools tools = new Tools();

        [field: CompilerGenerated]
        public event ErrorDel ErrorEvent;

        [field: CompilerGenerated]
        public event LogDel LogEvent;

        internal void errormonitoring()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            StreamReader standardError = this.process.StandardError;
            string sE = standardError.ReadToEnd();
            if (sE != "")
            {
                Error error = new Error(null, sE);
                if (this.ErrorEvent != null)
                {
                    this.ErrorEvent(error);
                }
                else
                {
                    standardError.Close();
                }
            }
            try
            {
                this.process.WaitForExit();
                standardError.Close();
                this.process.Close();
            }
            catch (Exception)
            {
            }
        }

        internal void monitoring()
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                StreamReader standardOutput = this.process.StandardOutput;
                while (!standardOutput.EndOfStream)
                {
                    string message = standardOutput.ReadLine();
                    if (WinAPI.GetHandle("LWJGL") == null && WinAPI.GetHandle("GLFW30") == null)
                    {
                        standardOutput.Close();
                        this.process.Close();
                    }
                    if (this.LogEvent != null)
                    {
                        this.LogEvent(new Log(message));
                    }
                    string str2 = this.SLC.Replace(message, "Exception", " ");
                    if ((str2 != null) && (str2 != message))
                    {
                        if (windows.WinAPI.GetHandle("LWJGL") == (IntPtr)0 && windows.WinAPI.GetHandle("GLFW30") == (IntPtr)0)
                        {

                            if (this.ErrorEvent != null)
                            {
                                this.ErrorEvent(new Error(null, standardOutput.ReadToEnd()));
                            }
                        }
                    }
                    if (this.ErrorEvent != null)
                    {
                        this.ErrorEvent(new Error(message, null));
                    }
                }
                this.process.WaitForExit();
                standardOutput.Close();
                this.process.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        internal async Task ModelLaunch(string version, string java, int RAM, string name)
        {
            await this.ModelLaunch(version, java, RAM, name, "", "");
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="username">邮箱</param>
        /// internal<param name="password">密码</param>
        internal async Task ModelLaunch(string version, string java, int RAM, string username, string password)
        {
            await this.ModelLaunch(version, java, RAM, username, password, "", "");
        }



        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        internal async Task ModelLaunch(string version, string java, int RAM, string name, string JVMparameter, string RearParameter)
        {
            await this.ModelLaunch(version, java, RAM, name, this.SLC.uuid(name), this.SLC.token(), JVMparameter, RearParameter);
        }


        
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        internal async Task<LaunchError> ModelLaunch(string version, string java, int RAM, string username, string password, string JVMparameter, string RearParameter)
        {
            Getlogin getlogin = null;
            try
            {
                getlogin = this.tools.MinecraftLogin(username, password);
                await this.ModelLaunch(version, java, RAM, getlogin.name, getlogin.uuid, getlogin.token, JVMparameter, RearParameter);
            }
            catch
            {
                return new LaunchError { Error = "启动失败，验证时出现错误！" };
            }
            return new LaunchError { Error = string.Empty };
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="uuid">uuid</param>
        /// <param name="token">token</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        internal async Task ModelLaunch(string version, string java, int RAM, string name, string uuid, string token, string JVMparameter, string RearParameter)
        {
            if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(java) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(uuid) || string.IsNullOrEmpty(token) || RAM == 0)
            {
                throw new LauncherException("任何一项都不能为空");
            }
            string Game = null;
            Tools tools = new Tools();
            if (SLC.FileExist(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar") != null)
            {
                throw new LauncherException("启动失败,未找到该版本");
            }
            //download[] MLib = tools.GetMissingLibrary(version);
            //if (MLib.Length != 0)
            //{
            //    throw new LauncherException("缺少Libraries文件");
            //}
            MCDownload[] natives1 = new MCDownload[0];
            MCDownload[] natives = tools.GetMissingNatives(version);
            if (natives.Length == 0)
            {
                natives1 = tools.GetAllNatives(version);
                if (natives1.Length == 0)
                {
                    throw new LauncherException("Natives获取失败，请检查文件是否完整");
                }
                string nativespath = SLC.nativeszip(version);
                if (SLC.FileExist(java) != "")
                {

                    string bx = "-Dminecraft.client.jar=" + System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar" + " -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=16M -XX:-UseAdaptiveSizePolicy -XX:-OmitStackTraceInFastThrow -Xmn"+mininc+" -Xmx" + RAM + "m -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump ";
                    if (JVMparameter == "" || JVMparameter == null)
                    {
                        Game = bx + "-Djava.library.path=\"" + nativespath + "\" -cp ";
                    }
                    else
                    {
                        Game = bx + JVMparameter + " -Djava.library.path=\"" + nativespath + "\" -cp ";
                    }
                    MCDownload[] Lib = tools.GetAllLibrary(version);
                    string Libname = "\"";
                    foreach (var Libname1 in Lib)
                    {
                        Libname += Libname1.path + ";";
                    }
                    Game += Libname + System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar\"";
                    var jo = SLC.versionjson<main.mainclass>(version);
                    string[] mA = null;
                    if (jo.minecraftArguments != null)
                    {
                        mA = jo.minecraftArguments.Split(' ');
                    }
                    else
                    {
                        StreamReader sr;
                        try
                        {
                            sr = new StreamReader(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", Encoding.Default);
                        }
                        catch (System.IO.DirectoryNotFoundException ex)
                        {
                            throw new LauncherException("未找到该版本");
                        }
                        var c = (JObject)JsonConvert.DeserializeObject(sr.ReadToEnd());
                        List<string> jo3 = new List<string>();
                        for (int i = 1; c["arguments"]["game"].ToArray().Length - 1 > 0; i += 2)
                        {
                            try
                            {

                                c["arguments"]["game"][i].ToString();
                                if (c["arguments"]["game"][i - 1].ToString()[0] == '-' || c["arguments"]["game"][i].ToString()[0] == '$')
                                {
                                    jo3.Add(c["arguments"]["game"][i - 1].ToString());
                                    jo3.Add(c["arguments"]["game"][i].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }
                        string[] arg = jo3.ToArray();
                        string a = arg[0] + " " + arg[1];
                        for (int i = 2; i < arg.Length; i += 2)
                        {
                            a += " " + arg[i] + " " + arg[i + 1];
                        }
                        mA = a.Split(' ');
                    }
                    var jo2 = SLC.versionjson<json4.Root>(version);
                    string main = " --width "+wih+" --height "+hei;
                    for (int i = 0; mA.Length > i; i += 2)
                    {
                        switch (mA[i])
                        {
                            case "--username":
                                main += " " + mA[i] + " \"" + name + "\"";
                                break;
                            case "--version":
                                main += " " + mA[i] + " \"" + jo2.id + "\"";
                                break;
                            case "--gameDir":
                                main += " " + mA[i] + " \"" + System.Directory.GetCurrentDirectory() + @"\.minecraft" + "\"";
                                break;
                            case "--assetsDir":
                                main += " " + mA[i] + " \"" + System.Directory.GetCurrentDirectory() + @"\.minecraft\assets" + "\"";
                                break;
                            case "--assetIndex":
                                main += " " + mA[i] + " " + jo2.assets;
                                break;
                            case "--uuid":
                                main += " " + mA[i] + " " + uuid;
                                break;
                            case "--accessToken":
                                main += " " + mA[i] + " " + token;
                                break;
                            case "--userType":
                                main += " " + mA[i] + " " + "Legacy";
                                break;
                            case "--versionType":
                                main += " " + mA[i] + " " + "\"" + qw + "\"";
                                break;
                            case "--userProperties":
                                main += " " + mA[i] + " " + "{}";
                                break;
                            default:
                                main += " " + mA[i] + " " + mA[i + 1];
                                break;

                        }
                    }
                    if ((RearParameter == "") || (RearParameter == null))
                    {
                        Game = Game + " " + jo.mainClass + main;
                    }
                    else
                    {
                        string[] textArray14 = new string[] { Game, " ", jo.mainClass, main, " ", RearParameter };
                        Game = string.Concat(textArray14);
                    }
                    Console.WriteLine("\n\n\n\n\n\n" + Game);
                    this.start.FileName = java;
                    this.start.Arguments = Game;
                    this.start.CreateNoWindow = true;
                    this.start.RedirectStandardOutput = true;
                    this.start.RedirectStandardInput = true;
                    this.start.UseShellExecute = false;
                    this.start.RedirectStandardError = true;
                    Thread thread1 = new Thread(new ThreadStart(this.monitoring))
                    {
                        IsBackground = true
                    };
                    Thread thread2 = new Thread(new ThreadStart(this.errormonitoring))
                    {
                        IsBackground = true
                    };
                    this.process = Process.Start(this.start);
                    thread2.Start();
                    thread1.Start();
                    try
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            if (process.WaitForInputIdle())
                            {
                                return;
                            }

                        });
                    }
                    catch (Exception ex)
                    {
                        throw new LauncherException("等待时发生异常：" + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="uuid">uuid</param>
        /// <param name="token">token</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        internal async Task StartgameofServer(string version, string java, int RAM, string name, string uuid, string token, string JVMparameter, string RearParameter,string ip,int post)
        {
            if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(java) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(uuid) || string.IsNullOrEmpty(token) || RAM == 0)
            {
                throw new LauncherException("任何一项都不能为空");
            }
            string Game = null;
            Tools tools = new Tools();
            if (SLC.FileExist(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar") != null)
            {
                throw new LauncherException("启动失败,未找到该版本");
            }
            //download[] MLib = tools.GetMissingLibrary(version);
            //if (MLib.Length != 0)
            //{
            //    throw new LauncherException("缺少Libraries文件");
            //}
            MCDownload[] natives1 = new MCDownload[0];
            MCDownload[] natives = tools.GetMissingNatives(version);
            if (natives.Length == 0)
            {
                natives1 = tools.GetAllNatives(version);
                if (natives1.Length == 0)
                {
                    throw new LauncherException("Natives获取失败，请检查文件是否完整");
                }
                string nativespath = SLC.nativeszip(version);
                if (SLC.FileExist(java) != "")
                {

                    string bx = "-Dminecraft.client.jar=" + System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar" + " -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=16M -XX:-UseAdaptiveSizePolicy -XX:-OmitStackTraceInFastThrow -Xmn" + mininc + " -Xmx" + RAM + "m -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump ";
                    if (JVMparameter == "" || JVMparameter == null)
                    {
                        Game = bx + "-Djava.library.path=\"" + nativespath + "\" -cp ";
                    }
                    else
                    {
                        Game = bx + JVMparameter + " -Djava.library.path=\"" + nativespath + "\" -cp ";
                    }
                    MCDownload[] Lib = tools.GetAllLibrary(version);
                    string Libname = "\"";
                    foreach (var Libname1 in Lib)
                    {
                        Libname += Libname1.path + ";";
                    }
                    Game += Libname + System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar\"";
                    var jo = SLC.versionjson<main.mainclass>(version);
                    string[] mA = null;
                    if (jo.minecraftArguments != null)
                    {
                        mA = jo.minecraftArguments.Split(' ');
                    }
                    else
                    {
                        StreamReader sr;
                        try
                        {
                            sr = new StreamReader(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", Encoding.Default);
                        }
                        catch (System.IO.DirectoryNotFoundException ex)
                        {
                            throw new LauncherException("未找到该版本");
                        }
                        var c = (JObject)JsonConvert.DeserializeObject(sr.ReadToEnd());
                        List<string> jo3 = new List<string>();
                        for (int i = 1; c["arguments"]["game"].ToArray().Length - 1 > 0; i += 2)
                        {
                            try
                            {

                                c["arguments"]["game"][i].ToString();
                                if (c["arguments"]["game"][i - 1].ToString()[0] == '-' || c["arguments"]["game"][i].ToString()[0] == '$')
                                {
                                    jo3.Add(c["arguments"]["game"][i - 1].ToString());
                                    jo3.Add(c["arguments"]["game"][i].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }
                        string[] arg = jo3.ToArray();
                        string a = arg[0] + " " + arg[1];
                        for (int i = 2; i < arg.Length; i += 2)
                        {
                            a += " " + arg[i] + " " + arg[i + 1];
                        }
                        mA = a.Split(' ');
                    }
                    var jo2 = SLC.versionjson<json4.Root>(version);
                    //外接启动参数
                    string main = " --width " + wih + " --height " + hei + " --server " + ip + " --port " + post;
                    for (int i = 0; mA.Length > i; i += 2)
                    {
                        switch (mA[i])
                        {
                            case "--username":
                                main += " " + mA[i] + " \"" + name + "\"";
                                break;
                            case "--version":
                                main += " " + mA[i] + " \"" + jo2.id + "\"";
                                break;
                            case "--gameDir":
                                main += " " + mA[i] + " \"" + System.Directory.GetCurrentDirectory() + @"\.minecraft" + "\"";
                                break;
                            case "--assetsDir":
                                main += " " + mA[i] + " \"" + System.Directory.GetCurrentDirectory() + @"\.minecraft\assets" + "\"";
                                break;
                            case "--assetIndex":
                                main += " " + mA[i] + " " + jo2.assets;
                                break;
                            case "--uuid":
                                main += " " + mA[i] + " " + uuid;
                                break;
                            case "--accessToken":
                                main += " " + mA[i] + " " + token;
                                break;
                            case "--userType":
                                main += " " + mA[i] + " " + "Legacy";
                                break;
                            case "--versionType":
                                main += " " + mA[i] + " " + "\"" + qw + "\"";
                                break;
                            case "--userProperties":
                                main += " " + mA[i] + " " + "{}";
                                break;
                            //case "--server":
                            //    main += " " + mA[i] + " \"" + ip + "\"";
                            //    break;
                            //case "--port":
                            //    main += " " + mA[i] + " \"" + post + "\"";
                            //    break;
                            default:
                                main += " " + mA[i] + " " + mA[i + 1];
                                break;

                        }
                    }
                    if ((RearParameter == "") || (RearParameter == null))
                    {
                        Game = Game + " " + jo.mainClass + main;
                    }
                    else
                    {
                        string[] textArray14 = new string[] { Game, " ", jo.mainClass, main, " ", RearParameter };
                        Game = string.Concat(textArray14);
                    }
                    Console.WriteLine("\n\n\n\n\n\n" + Game);
                    this.start.FileName = java;
                    this.start.Arguments = Game;
                    this.start.CreateNoWindow = true;
                    this.start.RedirectStandardOutput = true;
                    this.start.RedirectStandardInput = true;
                    this.start.UseShellExecute = false;
                    this.start.RedirectStandardError = true;
                    Thread thread1 = new Thread(new ThreadStart(this.monitoring))
                    {
                        IsBackground = true
                    };
                    Thread thread2 = new Thread(new ThreadStart(this.errormonitoring))
                    {
                        IsBackground = true
                    };
                    this.process = Process.Start(this.start);
                    thread2.Start();
                    thread1.Start();
                    try
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            if (process.WaitForInputIdle())
                            {
                                return;
                            }

                        });
                    }
                    catch (Exception ex)
                    {
                        throw new LauncherException("等待时发生异常：" + ex.Message);
                    }
                }
            }
        }
        public class Error : EventArgs
        {
            private string message;
            private string SE;
            private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();

            internal Error(string Message, string SE)
            {
                if ((Message == null) && (SE != null))
                {
                    this.SE = SE;
                }
                else
                {
                    string str = this.SLC.Replace(Message, "ERROR", " ");
                    if ((str != null) && (str != Message))
                    {
                        this.message = Message;
                    }
                }
            }

            public string Message
            {
                get
                {
                    return this.message;
                }
            }

            public string SeriousError
            {
                get
                {
                    return this.SE;
                }
            }
        }

        public delegate void ErrorDel(Game.Error error);

        public class Log : EventArgs
        {
            private string message;

            internal Log(string Message)
            {
                this.message = Message;
            }

            public string Message
            {
                get
                {
                    return this.message;
                }
            }
        }

        public delegate void LogDel(Game.Log Log);
    }
}

