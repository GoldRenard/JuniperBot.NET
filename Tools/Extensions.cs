using System;

namespace JuniperBot.Tools {

    internal static class Extensions {

        public static T[] SubArray<T>(this T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static T[] SubArray<T>(this T[] data, int index) {
            return SubArray(data, index, data.Length - index);
        }
    }
}