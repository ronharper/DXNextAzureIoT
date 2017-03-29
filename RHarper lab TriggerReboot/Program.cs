using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace TriggerReboot
{
    class Program
    {
        static RegistryManager registryManager;
        static string connString = "HostName=RHarper-Hub1.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=m7yDtsi0RQEb4vjAtIP0UXwXT7jiATDRmAsckuhxhzI=";
        static ServiceClient client;
        static JobClient jobClient;
        static string targetDevice = "myDeviceId";
        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connString);
            StartReboot().Wait();
            QueryTwinRebootReported().Wait();
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
        public static async Task QueryTwinRebootReported()
        {
            Twin twin = await registryManager.GetTwinAsync(targetDevice);
            Console.WriteLine(twin.Properties.Reported.ToJson());
        }
        public static async Task StartReboot()
        {
            client = ServiceClient.CreateFromConnectionString(connString);
            CloudToDeviceMethod method = new CloudToDeviceMethod("reboot");
            method.ResponseTimeout = TimeSpan.FromSeconds(30);

            CloudToDeviceMethodResult result = await client.InvokeDeviceMethodAsync(targetDevice, method);

            Console.WriteLine("Invoked firmware update on device.");
        }
    }
}
