using System;
using System.Runtime.CompilerServices;

namespace TicTacToe.Utilities
{
    internal static class BitHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetBits2(ref int bits, int index)
        {
            // Get the bitmask.
            int mask = 0b11 << index;

            // Extract the value.
            int result = (bits & mask) >> index;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SetBits2(ref int bits, int index, int value)
        {
            // Get the bitmask.
            int mask = 0b11 << index;

            // Calculate the value to set.
            int valueToSet = (value & 0b11) << index;

            // Clear the bits at the index.
            bits &= ~mask;

            // Write the value to the bits.
            bits |= valueToSet;

            return bits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetBits4(ref int bits, int index)
        {
            // Get the bitmask.
            int mask = 0b1111 << index;

            // Extract the value.
            int result = (bits & mask) >> index;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SetBits4(ref int bits, int index, int value)
        {
            // Get the bitmask.
            int mask = 0b1111 << index;

            // Calculate the value to set.
            int valueToSet = (value & 0b1111) << index;

            // Clear the bits at the index.
            bits &= ~mask;

            // Write the value to the bits.
            bits |= valueToSet;

            return bits;
        }

        internal static string ByteToBitString(int value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }

        internal static string IntToBitString(int value)
        {
            return Convert.ToString(value, 2).PadLeft(32, '0');
        }
    }
}