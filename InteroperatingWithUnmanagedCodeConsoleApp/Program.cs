using System;
using InteroperatingWithUnmanagedCode;
using InteroperatingWithUnmanagedCode.Models;

namespace InteroperatingWithUnmanagedCodeConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new PowerManager();

            Console.WriteLine(manager.GetLastSleepTime());

            Console.WriteLine(manager.GetLastWakeTime());

            Console.WriteLine(manager.GetSystemBatteryState());

            Console.WriteLine(manager.GetSystemPowerInformation());

            // manager.HibernateFileManipulation(HibernateFileManipulations.Reserve);

            // manager.HibernateFileManipulation(HibernateFileManipulations.Delete);

            // manager.SetSuspendState(true, false, false);

            // manager.SetSuspendState(false, false, false);

            Console.ReadKey();
        }
    }
}
