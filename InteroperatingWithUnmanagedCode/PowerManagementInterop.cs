using System;
using System.Runtime.InteropServices;
using InteroperatingWithUnmanagedCode.Models;

namespace InteroperatingWithUnmanagedCode
{
    internal class PowerManagementInterop
    {
        internal const int SUCCESS_STATUS = 0;

        [DllImport("PowrProf.dll", SetLastError = true)]
        public static extern uint CallNtPowerInformation(
            int InformationLevel,
            IntPtr lpInputBuffer,
            int nInputBufferSize,
            IntPtr lpOutputBuffer,
            int nOutputBufferSize);


        [DllImport("PowrProf.dll", SetLastError = true)]
        public static extern uint CallNtPowerInformation(
            int InformationLevel,
            IntPtr lpInputBuffer,
            int nInputBufferSize,
            [Out] ProcessorPowerInformation[] lpOutputBuffer,
            int nOutputBufferSize
            );


        [DllImport("PowrProf.dll", SetLastError = true)]
        public static extern uint SetSuspendState(
            bool Hibernate,
            bool ForceCritical,
            bool DisableWakeEvent
            );
    }
}
