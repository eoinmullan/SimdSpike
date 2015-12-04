using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace SimdSpike {
    public class HarewareAcceleratedArray<T> where T : struct {
        private readonly Vector<T>[] values;
        private readonly int chunkSize = Vector<T>.Count;

        public int Count { get; }

        public HarewareAcceleratedArray(params T[] input) {
            values = new Vector<T>[input.Length / chunkSize + 1];
            Count = input.Length;
            var i = 0;
            var j = 0;
            for (; i <= input.Length - chunkSize; i += chunkSize) {
                values[j++] = new Vector<T>(input, i);
            }
            var elementsRemaining = input.Length - i;
            if (elementsRemaining > 0) {
                var padding = chunkSize - elementsRemaining;
                values[j] = new Vector<T>(input.Skip(i).Concat(Enumerable.Range(0, padding).Select(x => default(T))).ToArray());
            }
        }

        private HarewareAcceleratedArray(int noChunks, int count) {
            values = new Vector<T>[noChunks];
            Count = count;
        }

        public T this[int i] {
            get {
                if (i < 0 || i >= Count) {
                    throw new ArgumentOutOfRangeException();
                }

                return values[i/chunkSize][i%chunkSize];
            }
        }

        public static HarewareAcceleratedArray<T> operator +(HarewareAcceleratedArray<T> left, HarewareAcceleratedArray<T> right) {
            var sumArray = new HarewareAcceleratedArray<T>(left.values.Length, left.Count);
            var j = 0;
            for (var i = 0; i < left.values.Length; i++) {
                sumArray.values[j++] = left.values[i] + right.values[i];
            }
//            var enumerable = left.values.Zip(right.values, (l, r) => l + r);
//            foreach (var vector in enumerable) {
//                sumArray.values.Add(vector);
//            }
            return sumArray;
        }
    }
}