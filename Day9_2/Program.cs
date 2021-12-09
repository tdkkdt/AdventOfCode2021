using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9_2 {
    class Program {
        static void Main(string[] args) {
            var map = File.ReadAllLines("input")
                .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
                .ToArray();
            var dd = new[] { new[] { -1, 0 }, new[] { 1, 0 }, new[] { 0, 1 }, new[] { 0, -1 } };
            int maxx = map[0].Length;
            int maxy = map.Length;
            long ans;
            List<int> basins = new List<int>();
            bool[,] was = new bool[maxy, maxx];
            for (int x = 0; x < maxx; x++) {
                for (int y = 0; y < maxy; y++) {
                    if (map[y][x] == 9) {
                        continue;
                    }
                    if (!was[y, x]) {
                        int size = 0;
                        var qx = new Queue<int>();
                        var qy = new Queue<int>();
                        qx.Enqueue(x);
                        qy.Enqueue(y);
                        was[y, x] = true;
                        while (qx.Count > 0) {
                            size++;
                            var xx = qx.Dequeue();
                            var yy = qy.Dequeue();
                            foreach (var d in dd) {
                                int nx = xx + d[0];
                                int ny = yy + d[1];
                                if (nx < 0 || nx >= maxx || ny < 0 || ny >= maxy) {
                                    continue;
                                }
                                if (was[ny, nx]) {
                                    continue;
                                }
                                if (map[ny][nx] == 9) {
                                    continue;
                                }
                                qx.Enqueue(nx);
                                qy.Enqueue(ny);
                                was[ny, nx] = true;
                            }
                        }
                        basins.Add(size);
                    }
                }
            }
            basins.Sort();
            ans = basins[^1] * basins[^2] * basins[^3];
            Console.WriteLine(ans);
        }
    }
}