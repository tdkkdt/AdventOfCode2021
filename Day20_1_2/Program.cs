using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20_1 {
    class Program {
        struct Point {
            public int X;
            public int Y;
            public int D;

            public Point(int x, int y, int d) {
                X = x;
                Y = y;
                D = d;
            }

            public bool Equals(Point other) {
                return X == other.X && Y == other.Y && D == other.D;
            }

            public override bool Equals(object obj) {
                return obj is Point other && Equals(other);
            }

            public override int GetHashCode() {
                return HashCode.Combine(X, Y, D);
            }
        }

        private static Dictionary<Point, char> cache = new Dictionary<Point, char>();

        private static char Calc(int x, int y, int d) {
            if (!cache.TryGetValue(new Point(x, y, d), out char r)) {
                r = CalcCore(x, y, d);
                cache.Add(new Point(x, y, d), r);
            }
            return r;
        }

        private static char CalcCore(int x, int y, int d) {
            if (d == 0) {
                return 0 <= x && x < map[0].Count && 0 <= y && y < map.Count
                    ? map[y][x]
                    : '.';
            }
            int bin = 0;
            for (int dy = -1; dy <= 1; dy++) {
                for (int dx = -1; dx <= 1; dx++) {
                    bin <<= 1;
                    bin += Calc(x + dx, y + dy, d - 1) == '#' ? 1 : 0;
                }
            }
            return mask[bin];
        }

        private static List<List<char>> map;
        private static string mask;
        
        static void Main(string[] args) {
            var f = File.OpenText("input");
            mask = f.ReadLine();
            f.ReadLine(); //skip empty line

            map = new List<List<char>>();

            while (!f.EndOfStream) {
                map.Add(
                    f.ReadLine().ToCharArray().ToList()
                );
            }
            int ans = 0;
            int d = 50;
            for (int y = -d * 5; y <= map.Count + 5 * d; y++) {
                for (int x = -5 * d; x <= map[0].Count + 5 * d; x++) {
                    char c = Calc(x, y, d);
                    if (c == '#') {
                        ans++;
                    }
                    // Console.Write(c);
                }
                // Console.WriteLine();
            }
            Console.WriteLine(ans);
        }
    }
}