using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static SimdSpike.Utilities;

namespace SimdSpike {
    public static class TestHelper {
        public static void ValidateAdditionMethods(params Action<float[], float[], float[]>[] sumActions) {
            ValidateAdditionMethods((IEnumerable<Action<float[], float[], float[]>>)sumActions);
        }

        public static void ValidateAdditionMethodsUnchecked(params Action<ushort[], ushort[], ushort[]>[] sumActions) {
            ValidateAdditionMethodsUnchecked((IEnumerable<Action<ushort[], ushort[], ushort[]>>)sumActions);
        }

        public static void ValidateAdditionInPlaceMethods(params Action<float[], float[]>[] additionInPlaceActions) {
            ValidateAdditionInPlaceMethods((IEnumerable<Action<float[], float[]>>)additionInPlaceActions);
        }

        public static void ValidateAdditionInPlaceMethodsUnchecked(params Action<ushort[], ushort[]>[] additionInPlaceActions) {
            ValidateAdditionInPlaceMethodsUnchecked((IEnumerable<Action<ushort[], ushort[]>>)additionInPlaceActions);
        }

        public static void ValidateAdditionFuncs(params Func<float[], float[], float[]>[] sumActions) {
            ValidateAdditionFuncs((IEnumerable<Func<float[], float[], float[]>>)sumActions);
        }

        public static void ValidateAdditionFuncsUnchecked(params Func<ushort[], ushort[], ushort[]>[] sumActions) {
            ValidateAdditionFuncsUnchecked((IEnumerable<Func<ushort[], ushort[], ushort[]>>)sumActions);
        }

        private static void ValidateAdditionMethods(IEnumerable<Action<float[], float[], float[]>> sumActions) {
            foreach (var sumAction in sumActions) {
                ValidateAdditionMethod(sumAction);
            }
        }

        private static void ValidateAdditionMethodsUnchecked(IEnumerable<Action<ushort[], ushort[], ushort[]>> sumActions) {
            foreach (var sumAction in sumActions) {
                ValidateAdditionMethodUnchecked(sumAction);
            }
        }

        private static void ValidateAdditionMethod(Action<float[], float[], float[]> sumAction) {
            const int count = 23;
            var one = Enumerable.Range(0, count).Select(x => RandomFloat()).ToArray();
            var two = Enumerable.Range(0, count).Select(x => RandomFloat()).ToArray();
            var sum = new float[one.Length];
            sumAction(one, two, sum);

            for (var i = 0; i < one.Length; i++) {
                Assert.AreEqual(one[i] + two[i], sum[i], $"Sum differs at index {i}, {one[i]} + {two[i]}");
            }
        }

        private static void ValidateAdditionMethodUnchecked(Action<ushort[], ushort[], ushort[]> sumAction) {
            const int count = 23;
            var one = Enumerable.Range(0, count).Select(x => RandomUShort()).ToArray();
            var two = Enumerable.Range(0, count).Select(x => RandomUShort()).ToArray();
            var sum = new ushort[one.Length];
            sumAction(one, two, sum);

            for (var i = 0; i < one.Length; i++) {
                Assert.AreEqual((ushort)(one[i] + two[i]), sum[i], $"Sum differs at index {i}, {one[i]} + {two[i]}");
            }
        }

        private static void ValidateAdditionInPlaceMethods(IEnumerable<Action<float[], float[]>> additionInPlaceActions) {
            additionInPlaceActions.Select(x => {
                return new Action<float[], float[], float[]>((lhs, rhs, result) => {
                    lhs.CopyTo(result, 0);
                    x(result, rhs);
                });
            }).ToList().ForEach(ValidateAdditionMethod);
        }

        private static void ValidateAdditionInPlaceMethodsUnchecked(IEnumerable<Action<ushort[], ushort[]>> additionInPlaceActions) {
            additionInPlaceActions.Select(x => {
                return new Action<ushort[], ushort[], ushort[]>((lhs, rhs, result) => {
                    lhs.CopyTo(result, 0);
                    x(result, rhs);
                });
            }).ToList().ForEach(ValidateAdditionMethodUnchecked);
        }

        private static void ValidateAdditionFuncs(IEnumerable<Func<float[], float[], float[]>> additionFuncs) {
            additionFuncs.Select(x => {
                return new Action<float[], float[], float[]>((lhs, rhs, result) => {
                    var temp = x(lhs, rhs);
                    temp.CopyTo(result, 0);
                });
            }).ToList().ForEach(ValidateAdditionMethod);
        }

        private static void ValidateAdditionFuncsUnchecked(IEnumerable<Func<ushort[], ushort[], ushort[]>> additionFuncs) {
            additionFuncs.Select(x => {
                return new Action<ushort[], ushort[], ushort[]>((lhs, rhs, result) => {
                    var temp = x(lhs, rhs);
                    temp.CopyTo(result, 0);
                });
            }).ToList().ForEach(ValidateAdditionMethodUnchecked);
        }
    }
}