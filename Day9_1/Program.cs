using System;
using System.IO;
using System.Linq;

namespace Day9_1 {
    class Program {
        static void Main(string[] args) {
            var map = File.ReadAllLines("input")
                .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
                .ToArray();
            var dd = new[] { new[] { -1, 0 }, new[] { 1, 0 }, new[] { 0, 1 }, new[] { 0, -1 } };
            int maxx = map[0].Length;
            int maxy = map.Length;
            int ans = 0;
            for (int x = 0; x < maxx; x++) {
                for (int y = 0; y < maxy; y++) {
                    if (dd.All(d => {
                        int nx = x + d[0];
                        int ny = y + d[1];
                        return nx >= maxx || nx < 0 || ny >= maxy || ny < 0 || map[ny][nx] > map[y][x];
                    })) {
                        ans += map[y][x] + 1;
                    }
                }
            }
            Console.WriteLine(ans);
        }
    }
}