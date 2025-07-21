using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RTCV.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 16)]
    public struct UInt128 :
        IComparable,
        IFormattable,
        IComparable<UInt128>,
        IEquatable<UInt128>
    {
        private ulong _low;
        private ulong _high;
        
        public UInt128(ulong low, ulong high)
        {
            _low = low;
            _high = high;
        }
        public UInt128(byte[] bytes)
        {
            if (bytes.Length != 16)
                throw new ArgumentException("Byte array must be 16 bytes long");
            
            _low = BitConverter.ToUInt64(bytes, 0);
            _high = BitConverter.ToUInt64(bytes, 8);
        }
        
        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[16];
            Buffer.BlockCopy(BitConverter.GetBytes(_low), 0, bytes, 0, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(_high), 0, bytes, 8, 8);
            return bytes;
        }
        
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is UInt128 other))
                throw new ArgumentException("UInt128 must be compared to another UInt128");
            
            return CompareTo(other);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{_high:X16}{_low:X16}"; // i have no desire to implement this properly
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public int CompareTo(UInt128 other)
        {
            if (_high > other._high)
                return 1;
            if (_high < other._high)
                return -1;
            if (_low > other._low)
                return 1;
            if (_low < other._low)
                return -1;

            return 0;
        }

        public bool Equals(UInt128 other)
        {
            return _high == other._high && _low == other._low;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 LeftShift(ulong value, int shift)
        {
            if (shift == 0)
                return new UInt128(value, 0);
            if (shift < 0)
                throw new ArgumentOutOfRangeException(nameof(shift), "Shift must be non-negative");
            if (shift >= 128)
                return new UInt128(0, 0);
            if (shift >= 64)
                return new UInt128(0, value << (shift - 64));
            return new UInt128(value << shift, value >> (64 - shift));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator ~(UInt128 self) =>
            new UInt128(~self._low, ~self._high);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator &(UInt128 self, UInt128 other) =>
            new UInt128(self._low & other._low, self._high & other._high);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator |(UInt128 self, UInt128 other) =>
            new UInt128(self._low | other._low, self._high | other._high);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UInt128 self, UInt128 other) =>
            self._low == other._low && self._high == other._high;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UInt128 self, UInt128 other) =>
            !(self == other);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt128(ulong self) =>
            new UInt128(self, 0);
    }
}