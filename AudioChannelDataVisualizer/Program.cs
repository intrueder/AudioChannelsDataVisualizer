using System;
using System.Linq;
using System.Text;
using NAudio.CoreAudioApi;

namespace AudioChannelsDataVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            MMDeviceEnumerator de = new MMDeviceEnumerator();
            var devices = de.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            int pos;
            do
            {
                pos = 0;
                Console.Out.WriteLine("Choose device:");
                var menu = devices.Aggregate("\n", (acc, d) => acc += string.Format("{0}. {1}\n", ++pos, d));
                Console.Out.WriteLine(menu);
                pos = 0;

                Int32.TryParse(Console.ReadLine(), out pos);
                Console.Clear();
            } while (pos <= 0 || pos > devices.Count);

            var device = devices[pos - 1];

            var bars = Encoding.GetEncoding(866)
                        .GetString(Enumerable.Repeat<byte>(178, 27)
                        .Concat(Enumerable.Repeat<byte>(177, 27))
                        .Concat(Enumerable.Repeat<byte>(176, 27)).ToArray());

            var nums = Enumerable.Range(1, 10).Select(n => n + " > ").ToArray();

            while (true)
            {
                var values = device.AudioMeterInformation.PeakValues;
                for (int i = 0; i < values.Count; i++)
                {
                    Console.Out.Write(nums[i]);
                    Console.Out.WriteLine(bars.Substring(0, (int)(75 * values[i])));
                }

                System.Threading.Thread.Sleep(150);
                Console.Clear();
            }
        }
    }
}
