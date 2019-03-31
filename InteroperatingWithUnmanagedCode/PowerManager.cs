using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using InteroperatingWithUnmanagedCode.Models;

namespace InteroperatingWithUnmanagedCode
{
    public class PowerManager
    {
        private static T GetPowerInformation<T>(PowerInformationLevel informationLevel)
        {
            var outputPtrSize = Marshal.SizeOf<T>();
            var outputPtr = Marshal.AllocCoTaskMem(outputPtrSize);

            var result = PowerManagementInterop.CallNtPowerInformation(
                (int)informationLevel,
                IntPtr.Zero,
                0,
                outputPtr,
                outputPtrSize);

            if (result == PowerManagementInterop.SUCCESS_STATUS)
            {
                var obj = Marshal.PtrToStructure<T>(outputPtr);
                Marshal.FreeHGlobal(outputPtr);
                return obj;
            }
            Marshal.FreeHGlobal(outputPtr);
            throw new Win32Exception();
        }

        public DateTime GetLastSleepTime()
        {
            var lastSleepTimeTicks = GetPowerInformation<long>(PowerInformationLevel.LastSleepTime);

            var bootUpTime = GetLastBootUpTime();
            return bootUpTime.AddTicks(lastSleepTimeTicks);
        }

        private static DateTime GetLastBootUpTime()
        {
            var managementClass = new ManagementClass("Win32_OperatingSystem");
            var properies = new List<PropertyData>();

            foreach (var queryObj in managementClass.GetInstances())
            {
                properies.AddRange(queryObj.Properties.Cast<PropertyData>());
            }

            var lastBootUpTime = properies.First(x => x.Name == "LastBootUpTime");
            return ManagementDateTimeConverter.ToDateTime(lastBootUpTime.Value.ToString());
        }

        public DateTime GetLastWakeTime()
        {
            var lastWakeTimeTicks = GetPowerInformation<long>(PowerInformationLevel.LastWakeTime);
            var bootUpTime = GetLastBootUpTime();
            return bootUpTime.AddTicks(lastWakeTimeTicks);
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            return GetPowerInformation<SystemBatteryState>(PowerInformationLevel.SystemBatteryState);
        }

        public ProcessorPowerInformation[] GetSystemPowerInformation()
        {
            var procCount = Environment.ProcessorCount;
            var procInfo = new ProcessorPowerInformation[procCount];
            var procInfoSize = procInfo.Length * Marshal.SizeOf(typeof(ProcessorPowerInformation));

            var result = PowerManagementInterop.CallNtPowerInformation(
                (int)PowerInformationLevel.ProcessorInformation,
                IntPtr.Zero,
                0,
                procInfo,
                procInfoSize
                );

            if (result == PowerManagementInterop.SUCCESS_STATUS)
            {
                return procInfo;
            }
            throw new Win32Exception();
        }

        public void HibernateFileManipulation(HibernateFileManipulations fileManipulations)
        {
            var boolSize = Marshal.SizeOf<bool>();
            var boolPtr = Marshal.AllocCoTaskMem(boolSize);
            Marshal.WriteByte(boolPtr, (byte)fileManipulations);

            var resutl = PowerManagementInterop.CallNtPowerInformation(
                (int)PowerInformationLevel.SystemReserveHiberFile,
                boolPtr,
                boolSize,
                IntPtr.Zero,
                0);

            Marshal.FreeHGlobal(boolPtr);
            if (resutl != PowerManagementInterop.SUCCESS_STATUS)
            {
                throw new Win32Exception();
            }
        }

        public void SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent)
        {    
            var result = PowerManagementInterop.SetSuspendState(hibernate, forceCritical, disableWakeEvent);
            if (result == 0)
            {
                throw new Win32Exception();
            }
        }
    }
}
