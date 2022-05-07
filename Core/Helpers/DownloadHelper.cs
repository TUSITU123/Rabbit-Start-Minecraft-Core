using FastX.Class.Models;
using FastX.Core.Listener;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Threading.ThreadState;

namespace FastX.Core.Helpers
{
    /// <summary>
    /// 文件下载帮助器
    /// </summary>
    public class DownloadHelper
    {
            public int ThreadNum = 3;
            List<Thread> list = new List<Thread>();
            public DownloadHelper()
            {
                doSendMsg += Change;
            }
            private void Change(DownMsg msg)
            {
                if (msg.Tag == DownStatus.Error || msg.Tag == DownStatus.End)
                {
                    StartDown(1);
                }
            }

        /// <summary>
        /// 取MC列表
        /// </summary>
        /// <returns></returns>
        public async Task<MCVersionList[]> GetMCVersionList()
        {
            HttpHelper http =new HttpHelper();
            string text = null;
            await Task.Factory.StartNew(() =>
            {
                text = http.getHtml("https://launchermeta.mojang.com/mc/game/version_manifest.json");
            });
            if (text == null)
            {
                throw new LauncherException("请求失败");
            }
            List<MCVersionList> list = new List<MCVersionList>();
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(new NewsHelper().TakeTheMiddle(text, "\"versions\":", "]}") + "]"))
            {
                string str2 = SLC.MCVersionAnalysis(token["type"].ToString());
                MCVersionList item = new MCVersionList
                {
                    type = str2,
                    id = token["id"].ToString(),
                    releaseTime = token["releaseTime"].ToString()
                };
                list.Add(item);
            }
            return list.ToArray();
        }


        public void AddDown(string DownUrl, string Dir, string suffixname,string FileName = "", int Id = 0)
            {
                Thread tsk = new Thread(() =>
                {
                    download(DownUrl, Dir, FileName + "."+suffixname, Id);
                });
                list.Add(tsk);
            }
            public void StartDown(int StartNum)
            {
                if (StartNum == 0)
                {
                    StartNum = 3;
                }
                for (int i2 = 0; i2 < StartNum; i2++)
                {
                    lock (list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].ThreadState == System.Threading.ThreadState.Unstarted || list[i].ThreadState == ThreadState.Suspended)
                            {
                                list[i].Start();
                                break;
                            }
                        }
                    }
                }

            }
            public delegate void dlgSendMsg(DownMsg msg);
            public event dlgSendMsg doSendMsg;
            //public event doSendMsg;
            //public dlgSendMsg doSendMsg = null;
            private void download(string path, string dir, string filename, int id = 0)
            {

                try
                {
                    DownMsg msg = new DownMsg();
                    msg.Id = id;
                    msg.Tag = 0;
                    doSendMsg(msg);
                    FileDownloadModel loader = new FileDownloadModel(path, dir, filename, ThreadNum);
                    loader.data.Clear();
                    msg.Tag = DownStatus.Start;
                    msg.Length = (int)loader.getFileSize(); ;
                    doSendMsg(msg);
                    DownloadProgressListener linstenter = new DownloadProgressListener(msg);
                    linstenter.doSendMsg = new DownloadProgressListener.dlgSendMsg(doSendMsg);
                    loader.download(linstenter);
                }
                catch (Exception ex)
                {
                    DownMsg msg = new DownMsg();
                    msg.Id = id;
                    msg.Length = 0;
                    msg.Tag = DownStatus.Error;
                    msg.ErrMessage = ex.Message;
                    doSendMsg(msg);

                    Console.WriteLine(ex.Message);
                }
        }

    }
}
