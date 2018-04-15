using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ChildWatchApi.Utility.Logging
{
    public class Logger
    {
        // Folder for the logger to write to.
        public string LogFolder { get; set; }
        // Name of the file to log to. Will create a new file with the name and today's date.
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                DateTime d = DateTime.Now;
                fileName = value  + d.Year.ToString() + d.Month.ToString() + d.Day.ToString() + ".log";
            }
        }
        public bool AutoSave { get; set; }
        private string fileName;

        private Queue<string> queue;
        private StringBuilder builder;

        protected FileInfo LogLocation
        {
            get
            {
                return new FileInfo(Path.Combine(LogFolder, FileName));
            }
        }
        public Logger() : this(@"C:\ProgramData\Logs\","Log")
        {
            
        }

        public Logger(string folder, string fileprefix) :this(folder, fileprefix,true)
        {
            
        }

        public Logger(string folder, string fileprefix, bool auto)
        {
            queue = new Queue<string>();
            builder = new StringBuilder();
            LogFolder = folder;
            FileName = fileprefix;
            AutoSave = auto;
        }

        // Default of information
        public void Write(string message)
        {
            Write(LogType.Information, message);           
        }

        public void Write(LogType type, string message)
        {
            Write(type, message, null);
        }

        public void Write(LogType type, string message, string source)
        {
            builder.Clear();
            builder.AppendLine(GetLogType(type));
            builder.AppendLine("Source: " + source ?? "No source provided");
            builder.AppendLine("Date: " + DateTime.Now.ToUniversalTime());
            builder.AppendLine("Output: " + message);
            
            builder.AppendLine();
            queue.Enqueue(builder.ToString());
            _localSave(AutoSave);
        }

        public void Write(Exception ex, string source)
        {
            builder.Clear();
            builder.AppendLine("");
            builder.AppendLine("Message:" + ex.Message);
            builder.AppendLine("Stack Trace: " + ex.StackTrace);
            builder.AppendLine("In Method Named: " + ex.TargetSite.Name);          
            Write(LogType.Error, builder.ToString(), source);
        }

        public void Save()
        {
            _localSave(true);
        }

        private void _localSave(bool write)
        {
            if(write && queue.Count > 0)
            {
               if(!LogLocation.Exists)
                {
                    if (!LogLocation.Directory.Exists)
                        LogLocation.Directory.Create();

                    LogLocation.Create();
                }
                while (queue.Count > 0)
                    File.AppendAllText(LogLocation.FullName, queue.Dequeue());
            }
        }

        protected string GetLogType(LogType t)
        { 
            string type_val = "";
            switch (t)
            {
                case LogType.Error:
                    type_val = "Error";
                    break;
                case LogType.Critical:
                    type_val = "Critical";
                    break;
                case LogType.Unknown:
                    type_val = "Unknown";
                    break;
                case LogType.Warning:
                    type_val = "Warning";
                    break;
                default:
                    type_val = "Information";
                    break;
            }
            return type_val;
        }
    }
    public enum LogType
    {
        Critical,
        Error,
        Information,
        Warning,
        Unknown
    }
}
