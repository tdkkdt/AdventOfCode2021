using System;
using System.IO;

namespace Day17_1 {
    class Program {
        static void Main(string[] args) {
            var line = File.ReadAllLines("input")[0];
            var ll = line.Split("target area: x=.., y=..".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            long dx1 = int.Parse(ll[0]);
            long dx2 = int.Parse(ll[1]);
            long dy1 = int.Parse(ll[2]);
            long dy2 = int.Parse(ll[3]);
            long ans = 0;
            long bySimAns = 0;
            for (long dx = dx1; dx <= dx2; dx++) {
                for (long dy = dy1; dy <= dy2; dy++) {
                    long ansForD = Calc(dx, dy);
                    long byS = Simulate(dx, dy);
                    if (ansForD != byS) {
                        Console.WriteLine("ERROR");
                    }
                    ans = Math.Max(ansForD, ans);
                    bySimAns = Math.Max(bySimAns, byS);
                }
            }
            Console.WriteLine(ans);
            Console.WriteLine(bySimAns);
        }

        private static long Simulate(long dx, long dy) {
            long ans = -1;
            for (int vx = 0; vx < 300; vx++) {
                for (int vy = 1; vy < 300; vy++) {
                    long calc = Calc(dx, dy, vx, vy);
                    ans = Math.Max(calc, ans);
                }
            }
            return ans;
        }

        private static long Calc(long dx, long dy, int vx, int vy) {
            long x = 0;
            long y = 0;
            long ans = 0;
            while (y > dy) {
                x += vx;
                y += vy;
                ans = Math.Max(y, ans);
                vx--;
                vy--;
                if (vx < 0) {
                    vx = 0;
                }
            }
            return x == dx && y == dy ? ans : -1;
        }

        private static long Calc(long dx, long dy) {
            long minXV = (int)Math.Sqrt(dx * 2);
            long maxXV = dx;
            long ansForD = -1;
            for (long xv = minXV; xv <= maxXV; xv++) {
                long n = TryToReachByX(dx, xv);
                if (n < 0) {
                    continue;
                }
                long ss = 2 * dy + n * n - n;
                if (ss % (2 * n) != 0) {
                    continue;
                }
                long vy = ss / 2 / n;
                long ans1 = vy * (vy + 1) / 2;
                ansForD = Math.Max(ans1, ansForD);
            }
            return ansForD;
        }

        private static long TryToReachByX(long dx, long xv) {
            long a = -1;
            long b = 2 * xv + 1;
            long c = -2 * dx;
            var D = b * b - 4 * a * c;
            var d = (long)Math.Sqrt(D);
            if ((d + 1) * (d + 1) == D) {
                d++;
            }
            if (d * d != D) {
                return -1;
            }
            long n1 = -b + d;
            long n2 = -b - d;
            long n = -1;
            if (n1 <= 0 && n1 % (2 * a) == 0) {
                n = n1 / 2 / a;
            }
            if (n2 <= 0 && n2 % (2 * a) == 0) {
                long qqq = n2 / 2 / a;
                if (qqq > n) {
                    n = qqq;
                }
            }
            return n;
        }
    }
}