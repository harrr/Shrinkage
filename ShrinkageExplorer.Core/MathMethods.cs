using System;
using System.Collections.Generic;

namespace ShrinkageExplorer.Core
{
    public static class MathMethods
    {
        private static void Shuffle<T>(this IList<T> list)
        {
            var rnd = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private static void Exch<T>(IList<T> a, int i, int j)
        {
            T swap = a[i];
            a[i] = a[j];
            a[j] = swap;
        }
        public static void QuickSort<T>(IList<T> a, int lo, int hi)
            where T : IComparable<T>
        {
            if (hi <= lo) return;
            int lt = lo, gt = hi;
            T v = a[lo];
            int i = lo;
            while (i <= gt)
            {
                int cmp = a[i].CompareTo(v);
                if (cmp < 0) Exch(a, lt++, i++);
                else if (cmp > 0) Exch(a, i, gt--);
                else i++;
            }

            // a[lo..lt-1] < v = a[lt..gt] < a[gt+1..hi]. 
            QuickSort(a, lo, lt - 1);
            QuickSort(a, gt + 1, hi);

        }
        public static void QuickSort<T>(IList<T> collection)
            where T : IComparable<T>
        {
            Shuffle(collection);
            QuickSort(collection, 0, collection.Count-1);
        }

        public static double SimpsonIntegral(Func<double, double> func, double a, double b, int N)
        {
            if (a == b || N <= 0)
                return 0;
            double x = a;
            double Jh = (b - a) / 2 / N;
            double J = 0;
            double Res = 0;
            for (int i = 0; i <= 2 * N; i++, x += Jh)
            {
                Res = func(x);
                if ((i == 0) || (i == 2 * N)) J += Res;
                else if (i % 2 == 0) J += 2 * Res;
                else J += 4 * Res;
            }
            return J * Jh / 3;
        }
        public static double ScanRoot(Func<double, double> phi, double a, double b, double eps)
        {
            //double x = (a + b) / 2;
            for (double x = a; x < b; x += eps)
            {
                double f = phi(x);
                if (Math.Abs(f) < eps)
                    return x;
            }
            return Double.NaN;
            //return x;
        }
        public static double BisectRoot(Func<double, double> phi, double a, double b, double eps)
        {
            double x=double.NaN;
            int sign = Math.Sign(phi(a));
            if (b < a)
            {
                double tmp;
                tmp = b;
                b = a;
                a = tmp;
            }
            while ((b - a) > eps)
            {
                x = (a + b) / 2;
                double f = phi(x);
                if (Math.Sign(f) == sign)
                    a = x;
                else
                    b = x;
            }
            return x;
        }
    }
}
