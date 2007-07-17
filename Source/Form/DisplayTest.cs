using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using Slickr.Util;
using System.Runtime.InteropServices;
using Tao.Platform.Windows;

namespace Slickr
{
	/// <summary>
	/// Test for getting at device info.
	/// </summary>
    class DisplayTest
    {

        static void Main(string[] args)
        {
            //DISPLAY_DEVICE info = new DISPLAY_DEVICE();
            DISPLAY_DEVICE info = new DISPLAY_DEVICE();
            info.cb = Marshal.SizeOf(info);

            int id = 0;
            int dwf = 0;
            while (WindowsUtils.EnumDisplayDevices(null, id, info, dwf))
            {
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}", id, info.DeviceName, info.DeviceString, info.StateFlags, info.DeviceID, info.DeviceKey));

                
                info.cb = Marshal.SizeOf(info);
                WindowsUtils.EnumDisplayDevices(info.DeviceName, 0, info, dwf);
                Console.WriteLine(String.Format("Device string: {0}, Flags: {1}", info.DeviceString, info.StateFlags));

                id++;
                info.cb = Marshal.SizeOf(info);
            }


            Gdi.DEVMODE devMode = new Gdi.DEVMODE();
            User.EnumDisplaySettings(Screen.PrimaryScreen.DeviceName, 0, out devMode);

            bool powerOffActive = false;
            SystemParams.SystemParametersInfoGetBool(SPI.SPI_GETPOWEROFFACTIVE, 0, ref powerOffActive, 0);
            Console.WriteLine("Power off active: " + powerOffActive);

            uint powerOffTimeout = 0;
            SystemParams.SystemParametersInfoGet(SPI.SPI_GETPOWEROFFTIMEOUT, 0, ref powerOffTimeout, 0);
            Console.WriteLine("Power off timeout: " + powerOffTimeout);

            /**
            DISPLAY d = new DISPLAY();
            d.cb = Marshal.SizeOf(d);
            int id = 0;
            int dwf = 0;

            while (Utils.EnumDisplayDevices(IntPtr.Zero, id, d, dwf))
            {
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}", id, d.DeviceName, d.DeviceString, d.StateFlags, d.DeviceID, d.DeviceKey));
                id++;
                d.cb = Marshal.SizeOf(d);
            }*/          
        }

    }

}
