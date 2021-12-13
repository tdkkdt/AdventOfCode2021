using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13_1 {
    class Program {
        private readonly struct Point {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y) {
                X = x;
                Y = y;
            }

            public bool Equals(Point other) {
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj) {
                return obj is Point other && Equals(other);
            }

            public override int GetHashCode() {
                return HashCode.Combine(X, Y);
            }
        }

        static void Main(string[] args) {
            var input = File.ReadLines("input");
            var points = new HashSet<Point>();
            var wasEmpty = false;
            var commands = new List<string>();
            foreach (var line in input) {
                if (string.IsNullOrEmpty(line)) {
                    wasEmpty = true;
                    continue;
                }
                if (wasEmpty) {
                    commands.Add(line);
                }
                else {
                    var ss = line.Split(",");
                    points.Add(new Point(int.Parse(ss[0]), int.Parse(ss[1])));
                }
            }
            foreach (var command in commands) {
                var tokens = command.Split(new[] { ' ', '=' });
                var coor = tokens[2];
                var value = int.Parse(tokens[3]);
                HashSet<Point> newPoints = new HashSet<Point>();
                foreach (var point in points) {
                    if (coor == "y") {
                        if (point.Y == value) {
                            continue;
                        }
                        newPoints.Add(new Point(
                            point.X,
                            value - Math.Abs(point.Y - value)
                        ));
                    }
                    else {
                        if (point.X == value) {
                            continue;
                        }
                        newPoints.Add(new Point(
                            value - Math.Abs(point.X - value),
                            point.Y
                        ));
                    }
                }
                Console.WriteLine(newPoints.Count);                
                points = newPoints;
            }
            int maxx = points.Max(p => p.X);
            int maxy = points.Max(p => p.Y);
            for (int y = 0; y <= maxy; y++) {
                for (int x = 0; x <= maxx; x++) {
                    Console.Write(
                        points.Contains(new Point(x, y))
                            ? "#"
                            : "."
                    );
                }
                Console.WriteLine();
            }
        }
    }
}