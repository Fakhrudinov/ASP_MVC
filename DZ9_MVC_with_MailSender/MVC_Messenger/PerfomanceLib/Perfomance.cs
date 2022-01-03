using System.Diagnostics;


namespace PerfomanceLib
{
    public class Perfomance : IPerfomance
    {
        public ModelDTO GetPerfomanceData()
        {
            ModelDTO model = new ModelDTO();

            using (Process proc = Process.GetCurrentProcess())
            {

                PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set", proc.ProcessName);
                PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", "_Total");
                PerformanceCounter hddCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "0 C:");

                PerformanceCounterCategory netWorkCategory = new PerformanceCounterCategory("Network Interface");
                string[] networkInstNames = netWorkCategory.GetInstanceNames();
                var netWorkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkInstNames[0]);

                model.Ram = ramCounter.NextValue() / 1024 / 1024;
                model.Cpu = cpuCounter.NextValue();
                model.NetWork = Convert.ToInt32(netWorkCounter.NextValue());
                model.Hdd = Convert.ToInt32(hddCounter.NextValue());
            }

            return model;
        }
    }
}
