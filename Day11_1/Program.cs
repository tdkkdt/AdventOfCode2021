using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11_1 {
    class Program {
        static void Main(string[] args) {
            var map = File.ReadLines("input")
                .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
                .ToArray();
            long ans = 0;
            var dd = new[] {
                new[] { -1, -1 },
                new[] { -1, 0 },
                new[] { -1, 1 },
                new[] { 0, 1 },
                new[] { 1, 1 },
                new[] { 1, 0 },
                new[] { 1, -1 },
                new[] { 0, -1 }
            };
            int maxx = map[0].Length;
            int maxy = map.Length;
            for (int s = 0; s < 100; s++) {
                var qx = new Queue<int>();
                var qy = new Queue<int>();
                for (int x = 0; x < 10; x++) {
                    for (int y = 0; y < 10; y++) {
                        map[y][x]++;
                        if (map[y][x] >= 10) {
                            qx.Enqueue(x);
                            qy.Enqueue(y);
                        }
                    }
                }
                while (qx.Count > 0) {
                    int xx = qx.Dequeue();
                    int yy = qy.Dequeue();
                    ans++;
                    foreach (var d in dd) {
                        int nx = xx + d[0];
                        int ny = yy + d[1];
                        if (nx < 0 || nx >= maxx || ny < 0 || ny >= maxy) {
                            continue;
                        }
                        map[ny][nx]++;
                        if (map[ny][nx] == 10) {
                            qx.Enqueue(nx);
                            qy.Enqueue(ny);
                        }
                    }
                }
                for (int x = 0; x < 10; x++) {
                    for (int y = 0; y < 10; y++) {
                        if (map[y][x] >= 10) {
                            map[y][x] = 0;
                        }
                    }
                }
            }
            Console.WriteLine(ans);
        }
    }
}