using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15_1 {
    class Program {
        static void Main(string[] args) {
            var map = File.ReadLines("input")
                .Select(l => l.ToCharArray())
                .Select(l => l.Select(ll => ll - '0').ToArray())
                .ToArray();

            int maxx = map[0].Length;
            int maxy = map.Length;

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
                    if (v + map[ny][nx] < dp[nx, ny]) {
                        dp[nx, ny] = v + map[ny][nx];
                        sx.Enqueue(nx);
                        sy.Enqueue(ny);
                    }
                }
            }
            Console.WriteLine(dp[maxx - 1, maxy - 1]);
        }
    }
}