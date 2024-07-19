using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Management.Instrumentation;
using System.Management;
using System.Runtime.InteropServices;
using System.Web.Configuration;
using System.Configuration;
using Microsoft.Win32;
using System.Web.Hosting;
using System.Net;

namespace Song.ViewData
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public sealed class Server
    {
        private static readonly Server _instance = new Server();
        private Server() { }
        public static Server GetServer()
        {
            return _instance;
        }
        private string _ip = string.Empty;
        /// <summary>
        /// 服务器IP
        /// </summary>       
        public  string IP
        {
            get {
                if (string.IsNullOrWhiteSpace(_ip))
                    _ip = WeiSha.Core.Server.IP;
                return _ip;
            }
        }
        /// <summary>
        /// 是否是本机IP
        /// </summary>
        public  bool IsLocalIP
        {
            get
            {
                string ip = this.IP;
                if (ip == "127.0.0.1" || this.Domain== "127.0.0.1") return true;
                if (this.Domain.ToLower().Trim() == "localhost") return true;
                return false;
            }
        }
        /// <summary>
        /// 是否是内网IP
        /// </summary>
        public  bool IsIntranetIP
        {
            get
            {
                if (IsLocalIP) return true;
                string ip = this.IP;
                if (ip.Substring(0, 3) == "10." || ip.Substring(0, 7) == "192.168" || ip.Substring(0, 7) == "172.16.")
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 服务器访问端口
        /// </summary>
        public  string Port
        {
            get {
                string port = "80";
                try
                {
                   
                    if (HttpContext.Current != null)
                        port = System.Web.HttpContext.Current.Request.Url.Port.ToString();
                    if (port == "443") port = "80";
                    return port;
                }
                catch
                {
                    return port;
                }
            }
        }
        /// <summary>
        /// 站点的访问域名
        /// </summary>
        /// <returns></returns>
        public  string Domain
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null)
                        return System.Web.HttpContext.Current.Request.Url.Host.ToString();
                }
                catch
                {
                    return "";
                }
                return "";
            }
        }
        /// <summary>
        /// 站点的访问域名带端口，如:http://www.xx.com/
        /// </summary>
        /// <returns></returns>
        public  string DomainPath
        {
            get
            {
                string path = string.Empty;
                if (HttpContext.Current != null)
                {
                    path = "http://" + Domain + ":" + Port;
                }
                return path;
            }
        }
        private string _os = string.Empty;
        /// <summary>
        /// 服务器操作系统
        /// </summary>  
        public string OS
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_os))
                    _os = WeiSha.Core.Server.OS;
                return _os;
            }

        }
        /// <summary>
        /// IIS版本
        /// </summary>
        public string IISVersion
        {
            get
            {
                //RegistryKey表示 Win注册表中的项级节点.此类是注册表封装
                string issversion = string.Empty;
                RegistryKey getkey = Registry.LocalMachine.OpenSubKey("software\\microsoft\\inetstp");
                if (getkey != null)
                    issversion = System.Convert.ToInt32(getkey.GetValue("majorversion", -1)).ToString();
                return issversion;

            }
        }

        /// <summary>
        /// CPU个数
        /// </summary>
        public int CPUCount
        {
            get
            {
                try
                {
                    string count = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");         //CPU个数：   
                    return System.Convert.ToInt16(count);
                }
                catch
                {
                    return 0;
                }
            }

        }
        private string _CpuHz = string.Empty;
        /// <summary>
        /// CPU主频，单位 GHz
        /// </summary>
        public string CPUHz
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CpuHz))
                    _CpuHz = WeiSha.Core.Server.CPUHz;
                return _CpuHz;
            }
        }
        private double _ramSize = -1;
        /// <summary>
        /// 物理内存大小
        /// </summary>
        public double RamSize
        {
            get
            {
                if (_ramSize < 0) _ramSize = WeiSha.Core.Server.RamSize;
                return _ramSize;
            }
        }

        /// <summary>
        /// .Net FramwWork版本号
        /// </summary>
        public string DotNetVersion
        {
            get
            {
                string netver = Environment.Version.ToString();                    //DotNET 版本  
                if (netver.IndexOf('.') < 0) return netver;
                netver = netver.Substring(0, 3);
                return netver;
            }
        }
        private string _cpu_id = string.Empty;
        /// <summary>
        /// 获取CPU的序列号，由于某些原因，可能获取不到
        /// </summary>
        public string CPU_ID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_cpu_id))
                    _cpu_id = WeiSha.Core.Server.CPU_ID;
                return _cpu_id;
            }
        }
        /// <summary>
        /// 当前应用程序的物理路径
        /// </summary>
        public string ProgramPath => System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath; 
        /// <summary>
        /// 系统创建初始时间
        /// </summary>
        public DateTime InitDateTime => WeiSha.Core.Server.InitDateTime;
    }
}
