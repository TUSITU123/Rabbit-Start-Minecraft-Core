using FastX.Class.Models;
using FastX.Core;
using global::SquareMinecraftLauncher.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SquareMinecraftLauncher;
using SquareMinecraftLauncher.Minecraft;
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

namespace FastX.Core
{
    public class Logoutput
    {
        private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        private Process process = new Process();
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
                        process.Close();
                    }
                    if (this.LogEvent != null)
                    {
                        this.LogEvent(new Log(message));
                    }
                    string str2 = SLC.Replace(message, "Exception", " ");
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

    public delegate void ErrorDel(Error error);

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

    public delegate void LogDel(Log Log);
}