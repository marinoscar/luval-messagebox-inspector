using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace luval_messagebox_inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DoWork();
            }
            catch (Exception ex)
            {
                Logger.WriteError("Failed to execute the program with exception {0}\n\nProgram will try to restart in 60 seconds", ex);
                Thread.Sleep(60000);
                DoWork();
            }
        }

        private static void DoWork()
        {
            var windows = DeSerialize();
            while (true)
            {
                foreach (var win in windows)
                {
                    var winHandle = WindowManager.FindWindowByCaption(win.Title);
                    Logger.WriteConsole("Result: {0}", winHandle);
                    if (winHandle != IntPtr.Zero)
                    {
                        Logger.WriteInfo("Windows with title: {0} found", win.Title);
                        Logger.WriteInfo("Waiting 30 seconds before sending message");
                        Thread.Sleep(30000);
                        var focus = WindowManager.SetForegroundWindow(winHandle);
                        if (focus)
                        {
                            Logger.WriteInfo("Windows with title: {0} focused set succesfully", win.Title);
                            var fileName = WindowManager.Capture();
                            Thread.Sleep(1000);
                            Logger.WriteInfo("Windows with title: {0} screenshot saved in {1}", win.Title, Path.Combine(Environment.CurrentDirectory, fileName));
                            foreach (var key in win.KeysToSend)
                            {
                                SendKeys.SendWait(key);
                            }
                            Logger.WriteInfo("Windows with title: {0} sent the following message {1}", win.Title, string.Join(",", win.KeysToSend));
                        }
                        else
                        {
                            Logger.WriteError("Windows with title: {0} failed to set focus, unable to send message", win.Title);
                        }
                        LogProcesses();
                    }
                }
                Thread.Sleep(3000);
            }
        }


        private static void LogProcesses()
        {
            var processes = Process.GetProcesses().Select(i => string.Format("Process Name: {0}, Memory: {1}KB, Window Title: {2}", i.ProcessName, (i.WorkingSet64/1024), i.MainWindowTitle));
            Logger.WriteInfo("{0}", string.Join(Environment.NewLine, processes));
        }

        private static IEnumerable<WindowToInspect> DeSerialize()
        {
            var defaultValue = "[{\"Title\":\"SAP GUI for Windows 750\",\"KeysToSend\":[\"N\"]}]";
            var fileName = ConfigurationManager.AppSettings["windowsToInspect"];
            var toDeserialize = string.Empty;
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
            {
                Logger.WriteWarning("File {0} not found, using default configuration {1}", fileName, defaultValue);
                toDeserialize = defaultValue;
            }
            else
            {
                toDeserialize = File.ReadAllText(fileName);
                Logger.WriteInfo("Loading configuration from file {0} with value\n {1}", fileName, toDeserialize);
            }
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<WindowToInspect>>(toDeserialize);
        }
    }
}
