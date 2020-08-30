namespace RTCV.CorruptCore
{
    using System;
    using System.IO;

    internal static class MemoryBanks
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static int maxBankSize = 1073741824;
        public static long totalFileSize = 0;

        public static byte[][] ReadFile(string path)
        {
            try
            {
                long fileLength = new System.IO.FileInfo(path).Length;
                totalFileSize = fileLength;
                int tailBankSize = Convert.ToInt32(fileLength % maxBankSize);
                bool multipleBanks = fileLength > maxBankSize;
                int banksCount = 1;

                if (multipleBanks)
                {
                    banksCount = Convert.ToInt32((fileLength - tailBankSize) / maxBankSize);

                    if (tailBankSize != 0) //an addition bank exists if the filesize's length isn't a multiplier of int32 maxvalue
                    {
                        banksCount++;
                    }
                }

                byte[][] Banks = new byte[banksCount][];

                using (Stream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    for (long i = 0; i < banksCount; i++)
                    {
                        int bankSize;
                        long addressStart;

                        if (!multipleBanks)
                        {
                            bankSize = Convert.ToInt32(fileLength);
                            addressStart = 0;
                        }
                        else
                        {
                            bool isLastBank = (i == banksCount - 1);

                            if (isLastBank)
                            {
                                bankSize = tailBankSize;
                            }
                            else
                            {
                                bankSize = maxBankSize;
                            }

                            addressStart = i * maxBankSize;
                        }

                        byte[] readBytes = new byte[bankSize];
                        stream.Position = addressStart;
                        stream.Read(readBytes, 0, bankSize);

                        Banks[i] = readBytes;
                    }
                }

                return Banks;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte PeekByte(this byte[][] data, long Address)
        {
            try
            {
                if (data == null)
                {
                    return 0;
                }

                long bank;
                long relativeAddress = (Address % maxBankSize);

                if (Address < maxBankSize)
                {
                    bank = 0;
                }
                else
                {
                    bank = maxBankSize / (Address - (Address % maxBankSize));
                }

                byte result = data[bank][relativeAddress];
                return result;
            }
            catch (IOException e)
            {
                logger.Error(e, "IOException in MemoryBanks.PeekByte!");
                Exception _e = e.InnerException;
                while (_e != null)
                {
                    logger.Error(e, "InnerException in MemoryBanks.PeekByte!");
                    _e = _e.InnerException;
                }
                return 0;
            }
        }

        public static byte[] PeekBytes(this byte[][] data, long startAddress, long length)
        {
            byte[] result = new byte[length];

            if (data == null)
            {
                return null;
            }

            long startBank;
            long relativeStartAdress = (startAddress % maxBankSize);

            if (startAddress < maxBankSize)
            {
                startBank = 0;
            }
            else
            {
                startBank = maxBankSize / (startAddress - relativeStartAdress);
            }

            long endBank;
            long endAddress = startAddress + length;
            long relativeEndAdress = (endAddress % maxBankSize);

            if (startAddress + length < maxBankSize)
            {
                endBank = 0;
            }
            else
            {
                endBank = maxBankSize / (endAddress - relativeEndAdress);
            }

            if (startBank == endBank)
            {
                Array.Copy(data[startBank], relativeStartAdress, result, 0, length);
            }
            else
            {
                //only supports 2 banks at the same time.
                //could easily add support for any number of banks but I need those precious cycles.
                //anyway a bank is like 1gb so...

                long lengthFromStartBank = maxBankSize - relativeStartAdress;
                long lengthFromEndBank = length - lengthFromStartBank;

                Array.Copy(data[startBank], relativeStartAdress, result, 0, lengthFromStartBank);
                Array.Copy(data[endBank], 0, result, lengthFromStartBank, lengthFromEndBank);
            }

            return result;
        }

        public static void PokeByte(this byte[][] data, long Address, byte value)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                long bank;
                long relativeAddress = (Address % maxBankSize);

                if (Address < maxBankSize)
                {
                    bank = 0;
                }
                else
                {
                    bank = maxBankSize / (Address - (Address % maxBankSize));
                }

                data[bank][relativeAddress] = value;
            }
            catch (IOException e)
            {
                logger.Error(e, "IOException in MemoryBanks.PokeByte!");
                Exception _e = e.InnerException;
                while (_e != null)
                {
                    logger.Error(e, "InnerException in MemoryBanks.PokeByte!");
                    _e = _e.InnerException;
                }
            }
        }

        public static void PokeBytes(this byte[][] data, long startAddress, byte[] values)
        {
            int length = values.Length;

            if (data == null)
            {
                return;
            }

            long startBank;
            long relativeStartAdress = (startAddress * maxBankSize);

            if (startAddress < maxBankSize)
            {
                startBank = 0;
            }
            else
            {
                startBank = maxBankSize / (startAddress - (startAddress % maxBankSize));
            }

            long endBank;
            long endAddress = startAddress + length;

            if (startAddress + length < maxBankSize)
            {
                endBank = 0;
            }
            else
            {
                endBank = maxBankSize / (endAddress - (endAddress % maxBankSize));
            }

            if (startBank == endBank)
            {
                Array.Copy(values, 0, data[startBank], relativeStartAdress, length);
            }
            else
            {
                //only supports 2 banks at the same time.
                //could easily add support for any number of banks but I need those precious cycles.
                //anyway a bank is like 1gb so...

                long lengthFromStartBank = maxBankSize - relativeStartAdress;
                long lengthFromEndBank = length - lengthFromStartBank;

                Array.Copy(values, 0, data[startBank], relativeStartAdress, lengthFromStartBank);
                Array.Copy(values, lengthFromStartBank, data[endBank], 0, lengthFromEndBank);
            }
        }
    }
}
