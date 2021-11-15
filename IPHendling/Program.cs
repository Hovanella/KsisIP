using System;
using System.Collections;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace IPHendling
{
    internal class Program
    {
        public static void Main(string[] args)
        {
           
                try
                {
                    // маску только в двоичном коде,если что сделаю метод,который её автоматом валидирует,если она задана числом
                    var IP = "31.255.255.255";
                    var mask = "192.0.0.0";
                    var IPNumbers = ParseIP(IP);
                    var maskNumbers = ParseMask( mask);
                    var shortMask = ParseShortMask(maskNumbers);
                    
                    var hostID = FindHostID(IPNumbers,maskNumbers);
                    var networkID= FindNetworkID(IPNumbers,maskNumbers);
                    var broadcastID = FindBroadcastID(IPNumbers,maskNumbers);
                    Console.WriteLine($"Сокращённая форма маски- /{shortMask}");
                    Console.WriteLine($"Network ID - {string.Join(".",networkID)}");
                    Console.WriteLine($"Host ID - {string.Join(".",hostID)}");
                    Console.WriteLine($"Broadcast ID - {string.Join(".",broadcastID)}");
                    FindTypeOfPacket(hostID,shortMask);
                    
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                }
        }

        private static byte ParseShortMask(byte[] maskNumbers)
        {
            var bitsForMask = new BitArray(maskNumbers);

            byte counter = 0;
            
            foreach (bool bit in bitsForMask)
            {
                if (bit)
                    counter++;
            }
            return counter;
        }

        private static byte[] FindBroadcastID(byte[] IPNumbers, byte[] maskNumbers)
        {
            byte[] invertedMaskNumber = maskNumbers.Select(x => (byte)~x).ToArray();
            
            var broadcastID = new byte[4];
            
            for (var i = 0; i < 4; i++) broadcastID[i] = (byte)(IPNumbers[i] & maskNumbers[i] | invertedMaskNumber[i]);
            
            return broadcastID;
        }

        public static void FirstTask(string IPNumbers, string maskNumbers)
        {
            /*11.	Запишите IP-адрес для отправки пакета всем узлам сети (широковещательным образом) с NETWORK ID равным IP, маской mask */

            byte[] invertedMaskNumber = maskNumbers.Select(x => (byte)~x).ToArray();
            var IPResultNumber = new byte[4];

            for (var i = 0; i < 4; i++) IPResultNumber[i] = (byte)(IPNumbers[i] | invertedMaskNumber[i]);

            foreach (byte number in IPResultNumber) Console.Write($"{number}.");
        }

        public static byte[] FindNetworkID(byte[] IPNumbers, byte[] maskNumbers)
        {
            var NetworkID = new byte[4];

            for (var i = 0; i < 4; i++) NetworkID[i] = (byte)(IPNumbers[i] & maskNumbers[i]);
            
            return NetworkID;
        }

        public static byte[] FindHostID(byte[] IPNumbers, byte[] maskNumbers)
        {
            byte[] invertedMaskNumber = maskNumbers.Select(x => (byte)~x).ToArray();
            var HostID = new byte[4];

            for (var i = 0; i < 4; i++) HostID[i] = (byte)(IPNumbers[i] & invertedMaskNumber[i]);
            
            return HostID;
        }

        public static void FindTypeOfPacket(byte[] hostID, byte shortMask)
        {
            var bits = new BitArray[4];

            var allbits = new StringBuilder();
                
            for (int i = 0; i < 4; i++)
            {
                bits[i] = new BitArray(new byte[1] { hostID[i]});
                
                for (var j = 7; j >-1 ; j--)
                {
                    if (bits[i][j])
                        allbits.Append("1");
                    else
                        allbits.Append("0");
                }
            }

            allbits.Remove(0,shortMask);

            if(allbits.ToString().Contains('0'))
                Console.WriteLine("Пакет не является широковещательным");
            else
                Console.WriteLine("Пакет является широковещательным");
            
                
                
        }
        
        private static byte[] ParseIP(string IP)
        {
            byte[] IPNumbers = IP.Split('.').Select(x => byte.Parse(x)).ToArray();
            if (IPNumbers.Length != 4)
                throw new Exception("Неправильный формат входной строки");
            if (ValidateIP(IPNumbers) == false)
                throw new Exception("Айпи несуществующий какой-то");
            return IPNumbers;
        }

        private static byte[] ParseMask(string mask)
        {
            byte[] maskNumbers = mask.Split('.').Select(x => byte.Parse(x)).ToArray();

            if ( maskNumbers.Length != 4) throw new Exception("Неправильный формат входной строки");

            if (ValidateMask(maskNumbers) == false)
                throw new Exception("Масочка-то не валидная");
            
            return maskNumbers;
        }

        private static bool ValidateMask(byte[] MaskNumbers)
        {
            var validMaskNumbers = new byte[] { 255, 254, 252, 248, 240, 224, 192, 128, 0 };

            if (MaskNumbers[0] == 0 || validMaskNumbers.Contains(MaskNumbers[0]) == false)
                return false;

            byte previousNumber = MaskNumbers[0];
            for (var i = 1; i < 4; i++)
            {
                if (validMaskNumbers.Contains(MaskNumbers[i]) == false)
                    return false;
                if (previousNumber != 255 && MaskNumbers[i] != 0)
                    return false;
                previousNumber = MaskNumbers[i];
            }

            return true;
        }

        private static bool ValidateIP(byte[] ipNumbers)
        {
            return true;
        }
    }
}