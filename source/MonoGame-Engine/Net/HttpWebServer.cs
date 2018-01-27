using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace IntergalacticTransmissionService.Net
{
    public class WebServer
    {
        private readonly string Result404 = "<HTML><BODY>File not found!</BODY></HTML>";
        private readonly Dictionary<string, string> Files = new Dictionary<string, string>();
        private readonly HttpListener HttpListener = new HttpListener();

        internal void RegisterFile(string key, string file)
        {
            Files[key] = file;
        }

        public WebServer(string prefix)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("HttpListener not supported");
            HttpListener.Prefixes.Add(prefix);
            HttpListener.Start();
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    while (HttpListener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr;
                                if (Files.ContainsKey(ctx.Request.RawUrl))
                                    rstr = File.ReadAllText(Files[ctx.Request.RawUrl]);
                                else
                                    rstr = Result404;
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { }
                            finally
                            {
                                ctx.Response.OutputStream.Close();
                            }
                        }, HttpListener.GetContext());
                    }
                }
                catch (Exception exc)
                {
                    Console.Error.WriteLine(exc.Message);
                }
            });
        }

        public void Dispose()
        {
            HttpListener.Stop();
            HttpListener.Close();
        }
    }
}