using System;
using System.Collections.Generic;
using System.IO;

namespace Day17_2 {
    class Program {
        struct Point {
            public int x;
            public int y;

            public Point(int x, int y) {
                this.x = x;
                this.y = y;
            }

            public bool Equals(Point other) {
                return x == other.x && y == other.y;
            }

            public override bool Equals(object obj) {
                return obj is Point other && Equals(other);
            }

            public override int GetHashCode() {
                return HashCode.Combine(x, y);
            }
        }
        static void Main(string[] args) {
            var line = File.ReadAllLines("input")[0];
            var ll = line.Split("target area: x=.., y=..".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            long dx1 = int.Parse(ll[0]);
            long dx2 = int.Parse(ll[1]);
            long dy1 = int.Parse(ll[2]);
            long dy2 = int.Parse(ll[3]);
            HashSet<Point> set = new();
            for (long dx = dx1; dx <= dx2; dx++) {
                for (long dy = dy1; dy <= dy2; dy++) {
                    Simulate(dx, dy, set);
                }
            }
            Console.WriteLine(set.Count);
        }

        private static long Simulate(long dx, long dy, HashSet<Point> set) {
            long ans = -1;
            for (int vx = 0; vx < 300; vx++) {
                for (int vy = -300; vy < 300; vy++) {
                    if (set.Contains(new Point(vx, vy))) {
                        continue;
                    }
                    long calc = Calc(dx, dy, vx, vy);
                    if (calc != -1) {
                        set.Add(new Point(vx, vy));
                    }
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