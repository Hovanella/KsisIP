using System;
using System.Linq;

namespace IPHendling
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // маску только в двоичном коде,если что сделаю метод,который её автоматом валидирует,если она задана числом
                var IP = "178.16.48.200";
                var mask = "255.255.0.0";

                byte[] IPNumbers = IPCalculator.ParseIP(IP);
                byte[] maskNumbers = IPCalculator.ParseMask(mask);
                byte shortMask = IPCalculator.ParseShortMask(maskNumbers);
                byte[] hostID = IPCalculator.FindHostID(IPNumbers, maskNumbers);
                byte[] networkID = IPCalculator.FindNetworkID(IPNumbers, maskNumbers);
                byte[] broadcastID = IPCalculator.FindBroadcastID(IPNumbers, maskNumbers);


                Console.WriteLine($"Сокращённая форма маски- /{shortMask}");
                Console.WriteLine($"Network ID - {string.Join(".", networkID)}");
                Console.WriteLine($"Host ID - {string.Join(".", hostID)}");
                Console.WriteLine($"Broadcast ID - {string.Join(".", broadcastID)}");
                Console.WriteLine(IPCalculator.FindTypeOfPacket(hostID, shortMask)
                    ? "Пакет является широковещательным"
                    : "Пакет не является широковещательным");


                FirstTask(new byte[] { 172, 16, 0, 0 }, new byte[] { 255, 255, 240, 0 });
                FirstTask(new byte[] { 160, 246, 0, 0 }, new byte[] { 255, 254, 0, 0 });
                FirstTask(new byte[] { 160, 160, 0, 0 }, new byte[] { 255, 224, 0, 0 });
                FirstTask(new byte[] { 172, 16, 176, 0 }, new byte[] { 255, 255, 240, 0 });
                FirstTask(new byte[] { 160, 246, 0, 0 }, new byte[] { 255, 224, 0, 0 });
                FirstTask(new byte[] { 160, 150, 16, 0 }, new byte[] { 255, 255, 240, 0 });

                SecondTask(new byte[] { 178, 16, 48, 200 }, new byte[] { 255, 255, 0, 0 });
                SecondTask(new byte[] { 200, 199, 255, 255 }, new byte[] { 255, 224, 0, 0 });
                SecondTask(new byte[] { 130, 192, 250, 127 }, new byte[] { 255, 255, 255, 248 });
                SecondTask(new byte[] { 155, 7, 255, 255 }, new byte[] { 255, 240, 0, 0 });
                SecondTask(new byte[] { 155, 159, 255, 255 }, new byte[] { 255, 224, 0, 0 });
                SecondTask(new byte[] { 200, 199, 255, 255 }, new byte[] { 255, 224, 0, 0 });
                SecondTask(new byte[] { 155, 159, 255, 255 }, new byte[] { 255, 224, 0, 0 });
                SecondTask(new byte[] { 155, 159, 255, 255 }, new byte[] { 255, 224, 0, 0 });
                SecondTask(new byte[] { 100, 101, 102, 171 }, new byte[] { 255, 255, 255, 252 });
                SecondTask(new byte[] { 15, 15, 15, 255 }, new byte[] { 224, 0, 0, 0 });
                SecondTask(new byte[] { 15, 15, 15, 255 }, new byte[] { 192, 0, 0, 0 });
                SecondTask(new byte[] { 155, 127, 255, 255 }, new byte[] { 255, 224, 0, 0 });
                SecondTask(new byte[] { 3, 255, 255, 255 }, new byte[] { 252, 0, 0, 0 });
                SecondTask(new byte[] { 140, 192, 230, 127 }, new byte[] { 255, 255, 255, 248 });
                SecondTask(new byte[] { 31, 1, 1, 1 }, new byte[] { 224, 0, 0, 0 });
                SecondTask(new byte[] { 63, 255, 255, 255 }, new byte[] { 192, 0, 0, 0 });
                SecondTask(new byte[] { 155, 7, 255, 255 }, new byte[] { 255, 248, 0, 0 });
                SecondTask(new byte[] { 155, 7, 255, 255 }, new byte[] { 255, 248, 0, 0 });
                SecondTask(new byte[] { 140, 192, 230, 127 }, new byte[] { 255, 255, 255, 248 });
                //достаточно

                ThirdTask(new byte[] { 172, 16, 0, 0 }, new byte[] { 255, 255, 240, 0 }, 128);
                ThirdTask(new byte[] { 172, 16, 0, 0 }, new byte[] { 255, 240, 0, 0 }, 4);
                ThirdTask(new byte[] { 172, 16, 0, 0 }, new byte[] { 255, 255, 240, 0 }, 64);

                FourthTask(new byte[] { 192, 168, 200, 16 }, new byte[] { 255, 255, 192, 0 }, 60);
                FourthTask(new byte[] { 192, 168, 0, 0 }, new byte[] { 255, 255, 0, 0 }, 1024);
                FourthTask(new byte[] { 192, 168, 0, 0 }, new byte[] { 255, 255, 0, 0 }, 1025);
                FourthTask(new byte[] { 192, 168, 0, 0 }, new byte[] { 255, 255, 0, 0 }, 500);

                FifthTask(new byte[] { 192, 168, 0, 0 }, new byte[] { 255, 255, 0, 0 }, 500);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }
        
        public static void FirstTask(byte[] networkID, byte[] maskNumbers)
        {
            /*11.	Запишите IP-адрес для отправки пакета всем узлам сети (широковещательным образом) с NETWORK ID равным IP, маской mask */

            if (IPCalculator.ValidateMask(maskNumbers) == false)
                throw new Exception("Масочка плохая");
            if (IPCalculator.ValidateIP(networkID) == false)
                throw new Exception("Адрес сети откровенно подкачал");

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(
                $"Запишите IP-адрес для отправки пакета всем узлам сети (широковещательным образом) с network ID равным {string.Join(".", networkID)}, маской {string.Join(".", maskNumbers)}");


            byte[] invertedMaskNumber = maskNumbers.Select(x => (byte)~x).ToArray();
            var IPResultNumber = new byte[4];

            for (var i = 0; i < 4; i++) IPResultNumber[i] = (byte)(networkID[i] | invertedMaskNumber[i]);

            foreach (byte number in IPResultNumber) Console.Write($"{number}.");
            Console.WriteLine("\n");
            Console.ResetColor();
        }

        private static void SecondTask(byte[] IPNumbers, byte[] maskNumbers)
        {
            if (IPCalculator.ValidateMask(maskNumbers) == false)
                throw new Exception("Масочка плохая");
            if (IPCalculator.ValidateIP(IPNumbers) == false)
                throw new Exception("Адрес откровенно подкачал");

            Console.ForegroundColor = ConsoleColor.Green;

            byte shortMask = IPCalculator.ParseShortMask(maskNumbers);
            byte[] hostID = IPCalculator.FindHostID(IPNumbers, maskNumbers);
            byte[] networkID = IPCalculator.FindNetworkID(IPNumbers, maskNumbers);
            bool IsBroadcastPacket = IPCalculator.FindTypeOfPacket(hostID, shortMask);

            Console.WriteLine(
                $"16.Какому узлу (узлам) будет доставлен пакет с адресом {string.Join(".", IPNumbers)} и маской {string.Join(".", maskNumbers)}?");

            if (networkID.All(x => x == 0))
            {
                if (IsBroadcastPacket)
                    Console.WriteLine("Всем узлам в сети отправителя");
                else
                    Console.WriteLine($"Узлу с HOST ID {string.Join(".", hostID)} в сети отправителя");
            }
            else
            {
                if (IsBroadcastPacket)
                    Console.WriteLine($"Всем узлам подсети с Network ID {string.Join(".", networkID)}");
                else
                    Console.WriteLine($"Узлу с Network ID {string.Join(".", networkID)} и HOST ID {string.Join(".", hostID)}");
            }


            Console.WriteLine();
        }

        private static void ThirdTask(byte[] networkID, byte[] subnetMaskNumbers, int networksNumber)
        {
            if (IPCalculator.ValidateMask(subnetMaskNumbers) == false)
                throw new Exception("Масочка плохая");
            if (IPCalculator.ValidateIP(networkID) == false)
                throw new Exception("Адрес откровенно подкачал");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"18 Какую маску необходимо использовать, чтобы структурировать имеющуюся сеть с network ID {string.Join(".", networkID)} (маска {string.Join(".", subnetMaskNumbers)}) на число сетей, равное {networksNumber} с некоторым числом узлов в этих сетях?");

            var allMaskBits = IPCalculator.GetBitsString(subnetMaskNumbers);
            var allNetworkIDBits = IPCalculator.GetBitsString(networkID);

            var counter = 1;
            int currentMaskBit = allMaskBits.ToString().IndexOf('0');
            while (counter < networksNumber)
            {
                if (currentMaskBit == 31)
                    break;

                allMaskBits[currentMaskBit] = '1';
                counter *= 2;
                currentMaskBit++;
            }


            //вывод
            var MaskNumbers = new string[4];
            var MaskByteNumbers = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                MaskNumbers[i] = allMaskBits.ToString().Substring(i * 8, 8);
                MaskByteNumbers[i] = (byte)Convert.ToInt32(MaskNumbers[i], 2);
            }

            Console.WriteLine(allNetworkIDBits);
            Console.WriteLine(string.Join(".", MaskByteNumbers));
        }

        private static void FourthTask(byte[] networkID, byte[] subnetMaskNumbers, int hostsNumber)
        {
            if (IPCalculator.ValidateMask(subnetMaskNumbers) == false)
                throw new Exception("Масочка плохая");
            if (IPCalculator.ValidateIP(networkID) == false)
                throw new Exception("Адрес откровенно подкачал");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"18 Какую маску необходимо использовать, чтобы структурировать имеющуюся сеть с network ID {string.Join(".", networkID)} (маска {string.Join(".", subnetMaskNumbers)}) на некоторое число сетей,с числом узлов в каждой подсети равном {hostsNumber}?");

            var allMaskBits = IPCalculator.GetBitsString(subnetMaskNumbers);
            var allNetworkIDBits = IPCalculator.GetBitsString(networkID);

            var counter = 1;
            int currentMaskBit = allMaskBits.ToString().LastIndexOf('0');

            while (counter < hostsNumber)
            {
                if (currentMaskBit == -1)
                    break;
                counter *= 2;
                currentMaskBit--;
            }

            for (var i = 0; i < currentMaskBit + 1; i++) allMaskBits[i] = '1';


            //вывод
            var MaskNumbers = new string[4];
            var MaskByteNumbers = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                MaskNumbers[i] = allMaskBits.ToString().Substring(i * 8, 8);
                MaskByteNumbers[i] = (byte)Convert.ToInt32(MaskNumbers[i], 2);
            }

            Console.WriteLine(allMaskBits);
            Console.WriteLine(string.Join(".", MaskByteNumbers));
        }

        private static void FifthTask(byte[] networkID, byte[] subnetMaskNumbers, int hostsNumber)
        {
            if (IPCalculator.ValidateMask(subnetMaskNumbers) == false)
                throw new Exception("Масочка плохая");
            if (IPCalculator.ValidateIP(networkID) == false)
                throw new Exception("Адрес откровенно подкачал");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(
                $"18 Какую маску необходимо использовать, чтобы структурировать имеющуюся сеть с network ID {string.Join(".", networkID)} (маска {string.Join(".", subnetMaskNumbers)}) на некоторое число сетей,с числом узлов в этих сетях НЕ БОЛЕЕ {hostsNumber}?");

            var allMaskBits = IPCalculator.GetBitsString(subnetMaskNumbers);
            var allNetworkIDBits = IPCalculator.GetBitsString(networkID);

            var counter = 1;
            int currentMaskBit = allMaskBits.ToString().LastIndexOf('0');

            while (counter < hostsNumber)
            {
                if (currentMaskBit == -1)
                    break;
                counter *= 2;
                currentMaskBit--;
            }

            if (counter > hostsNumber)
                counter /= 2;

            for (var i = 0; i < currentMaskBit + 1; i++) allMaskBits[i] = '1';


            //вывод
            var MaskNumbers = new string[4];
            var MaskByteNumbers = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                MaskNumbers[i] = allMaskBits.ToString().Substring(i * 8, 8);
                MaskByteNumbers[i] = (byte)Convert.ToInt32(MaskNumbers[i], 2);
            }

            Console.WriteLine(allMaskBits);
            Console.WriteLine(string.Join(".", MaskByteNumbers));
        }
    }
}