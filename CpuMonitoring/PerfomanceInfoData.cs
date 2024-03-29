﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CpuMonitoring
{
    public class PerfomanceInfoData
    {
        public Int64 CommitTotalPages;
        public Int64 CommitLimitPages;
        public Int64 CommitPeakPages;
        public Int64 PhysicalTotalBytes;
        public Int64 PhysicalAvailableBytes;
        public Int64 SystemCacheBytes;
        public Int64 KernelTotalBytes;
        public Int64 KernelPagedBytes;
        public Int64 KernelNonPagedBytes;
        public Int64 PageSizeBytes;
        public int HandlesCount;
        public int ProcessCount;
        public int ThreadCount;
    }

    public static class PsApiWrapper
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPerformanceInfo([Out] out PsApiPerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PsApiPerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        public static PerfomanceInfoData GetPerformanceInfo()
        {
            PerfomanceInfoData data = new PerfomanceInfoData();
            PsApiPerformanceInformation perfInfo = new PsApiPerformanceInformation();
            if (GetPerformanceInfo(out perfInfo, Marshal.SizeOf(perfInfo)))
            {
                /// data in pages
                data.CommitTotalPages = perfInfo.CommitTotal.ToInt64();
                data.CommitLimitPages = perfInfo.CommitLimit.ToInt64();
                data.CommitPeakPages = perfInfo.CommitPeak.ToInt64();

                /// data in bytes
                Int64 pageSize = perfInfo.PageSize.ToInt64();
                data.PhysicalTotalBytes = perfInfo.PhysicalTotal.ToInt64() * pageSize;
                data.PhysicalAvailableBytes = perfInfo.PhysicalAvailable.ToInt64() * pageSize;
                data.SystemCacheBytes = perfInfo.SystemCache.ToInt64() * pageSize;
                data.KernelTotalBytes = perfInfo.KernelTotal.ToInt64() * pageSize;
                data.KernelPagedBytes = perfInfo.KernelPaged.ToInt64() * pageSize;
                data.KernelNonPagedBytes = perfInfo.KernelNonPaged.ToInt64() * pageSize;
                data.PageSizeBytes = pageSize;

                /// counters
                data.HandlesCount = perfInfo.HandlesCount;
                data.ProcessCount = perfInfo.ProcessCount;
                data.ThreadCount = perfInfo.ThreadCount;
            }
            return data;
        }
    }
}