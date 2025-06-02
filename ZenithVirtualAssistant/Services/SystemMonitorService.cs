using System.Diagnostics;
using System.IO;

namespace ZenithVirtualAssistant.Services
{
    public class SystemMonitorService
    {
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;

        public SystemMonitorService()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public float GetCpuUsage()
        {
            return _cpuCounter.NextValue();
        }

        public float GetAvailableMemory()
        {
            return _ramCounter.NextValue();
        }

        public float GetTotalDiskUsage()
        {
            var drive = new DriveInfo("C");
            return (float)(drive.TotalSize - drive.AvailableFreeSpace) / drive.TotalSize * 100;
        }
    }
}