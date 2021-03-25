using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Diagnostics;

using System.Web.Script.Services;
using System.Web.Script.Serialization;


namespace CpuMonitoring
{
    /// <summary>
    /// WebService1의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        public class MonitorInfo
        {
            public string Id { get; set; }
            public float Val { get; set; }
        }

        public class MonitorDiskInfo
        {
            public string Id { get; set; }
            public double Total { get; set; }
            public double Free { get; set; }

        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetMonitor()
        {
            MonitorInfo[] info = new MonitorInfo[] {

                new MonitorInfo()
                {
                    Id  = "CPU",
                    Val = GetCpuUsage()
                },
                new MonitorInfo()
                {
                    Id  = "MEMORY",
                    Val = GetMemoryUsage()
                }
            };

            return new JavaScriptSerializer().Serialize(info);
        }

        private float GetCpuUsage()
        {
            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            cpuCounter.NextValue();

            System.Threading.Thread.Sleep(1000);
            float cpuUsage = cpuCounter.NextValue();

            return cpuUsage;
        }

        private float GetMemoryUsage()
        {
            PerfomanceInfoData perfData = PsApiWrapper.GetPerformanceInfo();

            float free = perfData.PhysicalAvailableBytes;
            float total = perfData.PhysicalTotalBytes;
            float usage = (total - free) / total;


            return usage * 100;
        }


        private int GetCores()
        {
            //foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            //{
            //    Console.WriteLine("Number Of Physical Processors: {0} ", item["NumberOfProcessors"]);
            //}

            return 0;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDiskMonitor()
        {
            List<MonitorDiskInfo> infos = new List<MonitorDiskInfo>();

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            double totalSize = 0;
            double totalFree = 0;




            foreach (DriveInfo d in allDrives)
            {
                MonitorDiskInfo info = new MonitorDiskInfo();

                info.Id = d.Name;
                info.Total = d.TotalSize;
                info.Free = d.TotalFreeSpace;

                infos.Add(info);

                totalFree += d.TotalFreeSpace;
                totalSize += d.TotalSize;
            }

            MonitorDiskInfo info1 = new MonitorDiskInfo();
            info1.Id = "TOTAL";
            info1.Total = totalSize;
            info1.Free = totalFree;

            infos.Add(info1);


            return new JavaScriptSerializer().Serialize(infos);

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetNetworkMonitor()
        {

            PerformanceCounter network;
            PerformanceCounter sent;
            PerformanceCounter received;

            float fTotal;
            float fSent;
            float fReceived;

            network = new PerformanceCounter("Network Interface", "Bytes Total/sec", "Realtek PCIe GBE Family Controller");
            sent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", "Realtek PCIe GBE Family Controller");
            received = new PerformanceCounter("Network Interface", "Bytes Received/sec", "Realtek PCIe GBE Family Controller");


            fTotal = network.NextValue();
            fSent = sent.NextValue();
            fReceived = received.NextValue();

            System.Threading.Thread.Sleep(1000);

            fTotal = network.NextValue();
            fSent = sent.NextValue();
            fReceived = received.NextValue();

            MonitorInfo[] info = new MonitorInfo[] {

                new MonitorInfo()
                {
                    Id  = "TOTAL",
                    Val = fTotal
                },
                new MonitorInfo()
                {
                    Id  = "SENT",
                    Val = fSent
                },
                new MonitorInfo()
                {
                    Id  = "RECEIVED",
                    Val = fReceived
                }
            };

            return new JavaScriptSerializer().Serialize(info);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProcessMonitor()
        {

            var ProcessCount = System.Diagnostics.Process.GetProcesses().Length;
            //var ThreadsCount = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
            var ThreadsCount = 0;

            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                ThreadsCount += p.Threads.Count;
            }

            MonitorInfo[] info = new MonitorInfo[] {

                new MonitorInfo()
                {
                    Id  = "PROCESS",
                    Val = ProcessCount
                },
                new MonitorInfo()
                {
                    Id  = "THREAD",
                    Val = ThreadsCount
                }
            };

            

            return new JavaScriptSerializer().Serialize(info);

        }
    }
}
