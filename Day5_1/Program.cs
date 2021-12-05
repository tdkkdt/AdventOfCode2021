using System;
using System.IO;
using System.Linq;

namespace Day5_1 {
    class Program {
        struct Line {
            public int l;
            public int r;
            public int t;
            public int b;

            public Line(int[] ll) {
                l = Math.Min(ll[0], ll[2]);
                r = Math.Max(ll[0], ll[2]);
                t = Math.Min(ll[1], ll[3]);
                b = Math.Max(ll[1], ll[3]);
            }
        }

        static void Main(string[] args) {
            var lines = File.ReadLines("input")
                .Select(l => l.Split(new[] { ',', ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(l => l.Select(int.Parse).ToArray())
                .Select(l => new Line(l))
                .ToArray();
            int minX = lines.Select(l => l.l).Min();
            int maxX = lines.Select(l => l.r).Max();
            int minY = lines.Select(l => l.t).Min();
            int maxY = lines.Select(l => l.b).Max();
            int ans = 0;
            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    var gg = lines.Where(l => l.l <= x && x <= l.r && l.t <= y && y <= l.b && (l.l == l.r || l.t == l.b)).ToArray();
                    var c = gg.Count();
                    if (c > 1) {
                        ans++;
                    }
                }
            }
            Console.WriteLine(ans);
        }
    }
}