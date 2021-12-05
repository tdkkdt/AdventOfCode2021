using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5_2 {
    class Program {
        struct Line {
            public int x1;
            public int x2;
            public int y1;
            public int y2;

            public Line(int[] ll) {
                x1 = ll[0];
                x2 = ll[2];
                y1 = ll[1];
                y2 = ll[3];
            }

            public bool Contains(int x, int y) {
                if (x1 == x2) {
                    return Math.Min(y1, y2) <= y && y <= Math.Max(y1, y2) && x1 == x;
                }
                if (y1 == y2) {
                    return Math.Min(x1, x2) <= x && x <= Math.Max(x1, x2) && y1 == y;
                }
                if (x == x1 && y == y1) {
                    return true;
                }
                if (x == x2 && y == y2) {
                    return true;
                }
                int dx = x - x1;
                int dy = y - y1;
                if (Math.Abs(dx) != Math.Abs(dy)) {
                    return false;
                }
                var q = Math.Sign(dx) == Math.Sign(x2 - x1) && Math.Sign(dy) == Math.Sign(y2 - y1) &&
                        Math.Sign(x - x2) == Math.Sign(x1 - x2) && Math.Sign(y - y2) == Math.Sign(y1 - y2);
                return q;
            }

            public override string ToString() {
                return $"{x1}, {y1} -> {x2}, {y2}";
            }
        }

        static void Main(string[] args) {
            var lines = File.ReadLines("input")
                .Select(l => l.Split(new[] { ',', ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(l => l.Select(int.Parse).ToArray())
                .Select(l => new Line(l))
                .ToArray();
            int minX = lines.Select(l => Math.Min(l.x1, l.x2)).Min();
            int maxX = lines.Select(l => Math.Max(l.x2, l.x1)).Max();
            int minY = lines.Select(l => Math.Min(l.y1, l.y2)).Min();
            int maxY = lines.Select(l => Math.Max(l.y2, l.y1)).Max();
            int ans = 0;
            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    var gg = lines.Where(l => l.Contains(x, y)).ToArray();
                    var c = gg.Count();
                    if (c > 1) {
                        ans++;
                    }
                }
            }

            // foreach (var line in lines) {
            //     int l = Math.Max(Math.Abs(line.x1 - line.x2), Math.Abs(line.y1 - line.y2)) + 1;
            //     int x = line.x1;
            //     int y = line.y1;
            //     List<(int, int)> pp = new List<(int, int)>();
            //     while (l-- > 0) {
            //         pp.Add((x, y));
            //         x += Math.Sign(line.x2 - line.x1);
            //         y += Math.Sign(line.y2 - line.y1);
            //     }
            //     for (x = minX; x <= maxX; x++) {
            //         for (y = minY; y <= maxY; y++) {
            //             var inLine1 = line.Contains(x, y);
            //             var inLine2 = pp.Any((p) => p.Item1 == x && p.Item2 == y);
            //             if (inLine1 != inLine2) {
            //                 Console.WriteLine("Alarm!");
            //             }
            //         }
            //     }
            // }

            Console.WriteLine(ans);
        }
    }
}