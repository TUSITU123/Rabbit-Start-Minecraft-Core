using FastX.Class.Models;
using FastX.Core.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastX.Core.Helpers;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace FastX.Core.Launch
{
    public class LaunchAsyncs
    {
        #region 实例化集

        protected int jg = 1;
        protected int launchtime;
        protected string launch;
        SettingHelper settingHelper = new SettingHelper();
        protected OfflineAuthenticator offline = new OfflineAuthenticator();
        protected Game game = new Game();

        #endregion

        #region 服务器启动方法模板

        protected async Task ModelLaunch(string serverpath,int Maxmemory,int? Minimemory,string Ygguri ,bool writeEula)
        {
            try
            {
                #region 判断是否签订Eula
                if (writeEula)
                {
                    File.WriteAllText(@"eula.txt", "eula=true");
                }
                else
                {
                    var v1 =  File.ReadAllText(@"eula.txt");
                    if(v1 != "eula=true")
                    {
                        File.WriteAllText(@"eula.txt", "eula=true");
                    }
                }
                #endregion

                #region 参数配置

                /*参数配置
                 * 外置服务器参数：-javaagent:authlib-injector.jar= 
                 * 判断文件夹： 
                 *  DirectoryInfo d = new DirectoryInfo(mainpath);
                 *  FileInfo[] Files = d.GetFiles("*.jar");
                 *  
                 * 
                 * 
                 *
                 * 
                 */
                string arg1 = null;
                string arg2 = null;
                string serverarg = null;
                if (Minimemory == 0)
                {
                    Minimemory = 1;
                }
                if (Minimemory != null)
                {
                    arg1 = "Java -Xmx" + Maxmemory + "G " + "-Xms" + Minimemory + "G" + " -jar " + serverpath;
                }
                else
                {
                    arg1 = "Java -Xmx" + Maxmemory + "G " + " -jar " + serverpath;
                }
                if (Ygguri != ""||Ygguri!=null)
                {
                    arg2 = "-javaagent:authlib-injector.jar=" + Ygguri;
                }
                serverarg = arg1 + arg2;
                Console.WriteLine(serverarg);
                var v = new Process();

                #endregion

                #region 启动服务器

                //路径判断
                if (File.Exists(serverpath))
                {
                    //启动
                    v.StartInfo.FileName = "cmd.exe";
                    v.StartInfo.RedirectStandardInput = true;
                    v.StartInfo.Arguments = serverarg;
                    v.StartInfo.CreateNoWindow = true;
                    v.StartInfo.UseShellExecute = false;
                    v.Start();
                    v.StandardInput.WriteLine(serverarg);
                }
                else
                {
                    throw new LauncherException("服务器核心路径错误或不存在！");
                }

                try
                {
                    await Task.Factory.StartNew(() =>
                    {
                        if (v.WaitForInputIdle())
                        {
                            return;
                        }

                    });
                }
                catch (Exception ex)
                {
                    throw new LauncherException("等待时发生异常：" + ex.Message);
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new LauncherException("启动失败，原因：" + ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 启动游戏方法
        /// </summary>
        public async Task LaunchTaskAsync(LaunchModel model)
        {

            #region 可配置项逻辑
            SquareMinecraftLauncherCore core =new SquareMinecraftLauncherCore();
            launch = "启动配置项中";
            game.hei = model.Height;
            game.wih = model.Width;
            game.mininc = model.Minimemory.ToString();
            if(model.LauncherName != string.Empty || model.LauncherName != null)
            {
                launch = "修改启动水印中";
                game.qw = model.LauncherName;
            }
            else
            {
                game.qw = "FastX -Baka黑手";
            }

            #endregion

            #region Log4j缓解措施
            launch = "施展Log4j缓解措施";
            string log4j = "-XX:+UseG1GC -XX:-UseAdaptiveSizePolicy -XX:-OmitStackTraceInFastThrow -Dfml.ignoreInvalidMinecraftCertificates=True -Dfml.ignorePatchDiscrepancies=True -Dlog4j2.formatMsgNoLookups=true";
            var v = model.AddJVMparameters + log4j; 
            #endregion

            #region 启动游戏并判断验证方式
            try
            {
                launch = "判断验证方式中";
                //判断验证方式
                if (model.Authenticator == LaunchTypeModel.Offline)
                {
                    //离线验证
                    launch = "启动游戏中(离线验证)";
                    if(model.AddJVMparameters == string.Empty || model.AddJVMparameters == null)
                    {
                        model.AddJVMparameters = "";
                    }
                    //判断是否直连服务器
                    if (model.ServerIp != null || model.ServerIp !="" || model.ServerPort > 100)
                    {
                        await game.StartgameofServer(model.Version, model.JavaExecutable, model.Maxmemory, model.Name, core.uuid(model.Name), core.token(),v,"", model.ServerIp, model.ServerPort);
                    }
                    else
                    {
                        await game.ModelLaunch(model.Version, model.JavaExecutable, model.Maxmemory, model.Name, v, "");
                    }
                }
                else if (model.Authenticator == LaunchTypeModel.Yggdrasil)
                {
                    //第三方验证
                    launch = "启动游戏中(第三方验证)";
                    if (model.AddJVMparameters == string.Empty || model.AddJVMparameters == null || model.ParameterofYggdrasil ==string.Empty || model.ParameterofYggdrasil ==null)
                    {
                        model.AddJVMparameters = "";
                        model.ParameterofYggdrasil = "";
                    }
                    if (model.ServerIp != null || model.ServerIp != "" || model.ServerPort > 100)
                    {
                        await game.StartgameofServer(model.Version, model.JavaExecutable, model.Maxmemory, model.Name, model.UUID, model.Token, v, model.ParameterofYggdrasil, model.ServerIp, model.ServerPort);
                    }
                    else
                    {
                        await game.ModelLaunch(model.Version, model.JavaExecutable, model.Maxmemory, model.Name, model.UUID, model.Token, v,model.ParameterofYggdrasil);
                    }
                }
                else if (model.Authenticator == LaunchTypeModel.Microsoft)
                {
                    //微软验证
                    launch = "启动游戏中(微软验证)";
                    if (model.AddJVMparameters == string.Empty || model.AddJVMparameters == null)
                    {
                        model.AddJVMparameters = "";
                    }
                    if(model.ServerIp != null || model.ServerIp != "" || model.ServerPort > 100)
                    {
                        await game.StartgameofServer(model.Version, model.JavaExecutable, model.Maxmemory, model.Name, model.UUID, model.Token, v, "", model.ServerIp, model.ServerPort);
                    }
                    else
                    {
                        await game.ModelLaunch(model.Version, model.JavaExecutable, model.Maxmemory, model.Name, model.UUID, model.Token, v, "");
                    }
                }
            }
            catch (Exception ex)
            {
                jg = 0;
                throw new LauncherException("启动失败，具体原因："+ex);
            }
            finally
            {
                if(jg == 1)
                {
                    launch = "游戏启动成功！";
                }
                else
                {
                    launch = "游戏启动失败！";
                }
            }
            #endregion

            #region 弃用方法

            if (model.WindowTitle != string.Empty || model.WindowTitle != null)
            {
                launch = "修改窗口标题中";
                var str1 = new Tools().ChangeTheTitle(model.WindowTitle);
                switch (str1)
                {
                    case true:
                        Console.Write("标题修改成功");
                        launch = "标题修改成功";
                        break;
                    case false:
                        Console.Write("标题修改失败");
                        launch = "标题修改失败";
                        break;
                }
            }


            //if (model.Authenticator == new OfflineAuthenticator(string.Empty))
            //{
            //    Console.WriteLine("离线启动");
            //   // await game.StartGame(model.Version, model.JavaExecutable, model.Memory,new OfflineAuthenticator("").Username);
            //}
            //else if(model.Authenticator == new YggdrasilAuthenticator(string.Empty))
            //{
            //    Console.WriteLine("外置启动");
            //}



            //int result = -1;
            //try
            //{
            //    result = 0;
            //    
            //}
            //catch(Exception e)
            //{
            //    result = 1;
            //}
            //finally
            //{
            //    if (result == 1)
            //    {
            //        throw new LauncherException("启动失败");
            //    }
            //}
            #endregion

        }
        /// <summary>
        /// 服务端启动方法(制作ing)
        /// </summary>
        protected async void ServerLaunch()
        {
           await ModelLaunch("c",1,null,"",true);
        }
        /// <summary>
        /// 监控启动进度
        /// </summary>
        public void MonitorLaunchprogress()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if(launchtime == 100)
                    {
                        Console.WriteLine("结束监控任务");
                        break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                        break;
                    }
                }
            });
        }
    }
}
