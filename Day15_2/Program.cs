using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15_2 {
    class Program {
        static void Main(string[] args) {
            var map = File.ReadLines("input")
                .Select(l => l.ToCharArray())
                .Select(l => l.Select(ll => ll - '0').ToArray())
                .ToArray();

            int mx = map[0].Length;
            int my = map.Length;
            
            int maxx = map[0].Length * 5;
            int maxy = map.Length * 5;

            long[,] dp = new long[maxx, maxy];
            for (int i = 0; i < maxx; i++) {
                for (int j = 0; j < maxy; j++) {
                    dp[i, j] = long.MaxValue;
                }
            }
            dp[0, 0] = 0;
            Queue<int> sx = new Queue<int>();
            Queue<int> sy = new Queue<int>();
            var dd = new[] {
                new[] { -1, 0 },
                new[] { 1, 0 },
                new[] { 0, -1 },
                new[] { 0, 1 }
            };
            sx.Enqueue(0);
            sy.Enqueue(0);
            while (sx.Count > 0) {
                int x = sx.Dequeue();
                int y = sy.Dequeue();
                long v = dp[x, y];
                foreach (var d in dd) {
                    int nx = x + d[0];
                    int ny = y + d[1];
                    if (nx < 0 || nx >= maxx || ny < 0 || ny >= maxy) {
                        continue;
                    }
                    int mm = map[ny % my][nx % mx];
                    int ddd = ny / my + nx / mx;
                    mm += ddd;
                    if (mm >= 10) {
                        mm %= 10;
                        mm += 1;
                    }
                    if (v + mm < dp[nx, ny]) {
                        dp[nx, ny] = v + mm;
                        sx.Enqueue(nx);
                        sy.Enqueue(ny);
                    }
                }
            }
            Console.WriteLine(dp[maxx - 1, maxy - 1]);
        }
    }
}