using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace IPHendling
{
    internal class IPCalculator
    {
        public static byte ParseShortMask(byte[] maskNumbers)
        {
            var bitsForMask = new BitArray(maskNumbers);

            byte counter = 0;

            foreach (bool bit in bitsForMask)
                if (bit)
                    counter++;
            return counter;
        }

        public static byte[] FindBroadcastID(byte[] IPNumbers, byte[] maskNumbers)
        {
            byte[] invertedMaskNumber = maskNumbers.Select(x => (byte)~x).ToArray();

            var broadcastID = new byte[4];

            for (var i = 0; i < 4; i++) broadcastID[i] = (byte)((IPNumbers[i] & maskNumbers[i]) | invertedMaskNumber[i]);

            return broadcastID;
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

        public static bool FindTypeOfPacket(byte[] hostID, byte shortMask)
        {
         

            var allbits = GetBitsString(hostID);
            
            allbits.Remove(0, shortMask);

            if (allbits.ToString().Contains('0'))
                return false;
            return true;
        }

        public static StringBuilder GetBitsString(byte[] bytes)
        {
            var bits = new BitArray[4];
            var allMaskBits = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                bits[i] = new BitArray(new[] { bytes[i] });

                for (var j = 7; j > -1; j--)
                    if (bits[i][j])
                        allMaskBits.Append("1");
                    else
                        allMaskBits.Append("0");
            }

            return allMaskBits;
        }
        
        public static StringBuilder GetBitsStringWithDots(byte[] bytes)
        {
            //To separate octets
            var bits = new BitArray[4];
            var allMaskBits = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                bits[i] = new BitArray(new[] { bytes[i] });

                for (var j = 7; j > -1; j--)
                    if (bits[i][j])
                        allMaskBits.Append("1");
                    else
                        allMaskBits.Append("0");
            }

            allMaskBits.Insert(8, '.');
            allMaskBits.Insert(17, '.');
            allMaskBits.Insert(26, '.');
            return allMaskBits;
        }

        public static byte[] ParseIP(string IP)
        {
            byte[] IPNumbers = IP.Split('.').Select(x => byte.Parse(x)).ToArray();
            if (IPNumbers.Length != 4)
                throw new Exception("Неправильный формат входной строки");
            if (ValidateIP(IPNumbers) == false)
                throw new Exception("Айпи несуществующий какой-то");
            return IPNumbers;
        }

        public static byte[] ParseMask(string mask)
        {
            byte[] maskNumbers = mask.Split('.').Select(x => byte.Parse(x)).ToArray();

            if (maskNumbers.Length != 4) throw new Exception("Неправильный формат входной строки");

            if (ValidateMask(maskNumbers) == false)
                throw new Exception("Масочка-то не валидная");

            return maskNumbers;
        }

        public static bool ValidateMask(byte[] MaskNumbers)
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

        public static bool ValidateIP(byte[] ipNumbers)
        {
            //TODO
            return true;
        }
    }
}