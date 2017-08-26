namespace SaveSystem.Utility {
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public static class CollectionUtilities {



        public static List<T> GetPopulatedListValType<T>(int amount) {
            List<T> list = new List<T>(amount);
            for (int i = 0; i < amount; i++) {
                list.Add(default(T));
            }
            return list;
        }

        public static List<T> GetPopulatedListRefType<T>(int amount) where T : new() {
            List<T> list = new List<T>(amount);
            for (int i = 0; i < amount; i++) {
                list.Add(new T());
            }
            return list;
        }

        public static Stack<T> GetPopulatedStackRefType<T>(int amount) where T : new() {
            Stack<T> stack = new Stack<T>(amount);
            for (int i = 0; i < amount; i++) {
                stack.Push(new T());
            }
            return stack;
        }

        public static Stack<T> GetPopulatedStackValType<T>(int amount) {
            Stack<T> stack = new Stack<T>(amount);
            for (int i = 0; i < amount; i++) {
                stack.Push(default(T));
            }
            return stack;
        }

        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;

            }
        }

        public static bool NullOrEmpty<T>(this List<T> list) {
            if (list == null)
                return true;
            if (list.Count == 0)
                return true;
            int c = list.Count;
            for (int i = 0; i < c; i++) {
                if (list[i] == null)
                    continue;
                else
                    return false;
            }
            return true;
        }

        public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dic, K key) {
            V ret;
            bool found = dic.TryGetValue(key, out ret);
            if (found) {
                return ret;
            }
            return default(V);
        }

        public static void Clear<T>(this T[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = default(T);
            }
        }

        public static T RandomEnumValue<T>() {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length));
        }

        static System.Random listRandom;
        public static T RandomListValue<T>(this List<T> list) {

            if (listRandom == null)
                listRandom = new System.Random();

            return (T)list[listRandom.Next(list.Count)];
        }

        public static string[] ListToString<T>(List<T> list) {
            string[] result = new string[list.Count];
            for (int i = 0; i < list.Count; i++) {
                result[i] = list[i].ToString();
            }
            return result;
        }


    }

}