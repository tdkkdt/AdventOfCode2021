using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19_1 {
    class Program {
        struct Point:IComparable<Point> {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public Point(int x, int y, int z) {
                X = x;
                Y = y;
                Z = z;
            }

            public bool Equals(Point other) {
                return X == other.X && Y == other.Y && Z == other.Z;
            }

            public int CompareTo(Point other) {
                int dx = X.CompareTo(other.X);
                int dy = Y.CompareTo(other.Y);
                int dz = Z.CompareTo(other.Z);
                if (dx != 0) {
                    return dx;
                }
                if (dy != 0) {
                    return dy;
                }
                if (dz != 0) {
                    return dz;
                }
                return 0;
            }

            public override bool Equals(object obj) {
                return obj is Point other && Equals(other);
            }

            public override int GetHashCode() {
                return HashCode.Combine(X, Y, Z);
            }

            public override string ToString() {
                return $"({X}, {Y}, {Z})";
            }
        }

        static void Main(string[] args) {
            var lines = File.ReadAllLines("input");

            List<List<Point>> scannersData = new List<List<Point>>();

            foreach (string line in lines) {
                if (string.IsNullOrEmpty(line)) {
                    continue;
                }
                if (line.Contains("---")) {
                    scannersData.Add(new List<Point>());
                }
                else {
                    var data = line.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    scannersData[^1].Add(new Point(data[0], data[1], data[2]));
                }
            }
            foreach (var data in scannersData) {
                data.Sort();
            }
            var result = new HashSet<Point>();
            List<List<Point>> calced = new List<List<Point>>();
            calced.Add(scannersData[0]);
            for (int i = 0; i < calced.Count; i++) {
                foreach (List<Point> second in scannersData) {
                    if (calced.Contains(second)) {
                        continue;
                    }
                    if (Calc(calced[i], second)) {
                        calced.Add(second);
                    }
                }
                foreach (Point point in calced[i]) {
                    result.Add(point);
                }
            }
            Console.WriteLine(result.Count);
        }

        private static bool Calc(List<Point> first, List<Point> second) {
            List<Point> ss = new List<Point>(second);
            HashSet<Point> ff = new HashSet<Point>(first);
            List<Point> bestAns = null;
            int bestAnsC = 0;
            for (int yz = 0; yz < 4; yz++) {
                for (int xz = 0; xz < 4; xz++) {
                    for (int xy = 0; xy < 4; xy++) {
                        foreach (var secondP in ss) {
                            foreach (var firstP in first) {
                                List<Point> recalculatedSecond = Move(
                                    ss,
                                    firstP.X - secondP.X, firstP.Y - secondP.Y, firstP.Z - secondP.Z
                                );
                                int count = recalculatedSecond.Count(recSec => ff.Contains(recSec));
                                if (count >= 12) {
                                    if (bestAnsC < count) {
                                        bestAnsC = count;
                                        bestAns = new List<Point>(recalculatedSecond);
                                    }
                                }
                            }
                        }
                        ss = Rotate90XY(ss);
                    }
                    ss = Rotate90XZ(ss);
                }
                ss = Rotate90YZ(ss);
            }
            if (bestAns != null) {
                second.Clear();
                second.AddRange(bestAns);
                return true;
            }
            return false;
        }

        private static List<Point> Rotate90YZ(List<Point> points) {
            return points.Select(Rotate90YZ).ToList();
        }

        private static Point Rotate90YZ(Point p) {
            return new Point(p.X, p.Z, -p.Y);
        }

        private static List<Point> Rotate90XZ(List<Point> points) {
            return points.Select(Rotate90XZ).ToList();
        }

        private static Point Rotate90XZ(Point p) {
            return new Point(p.Z, p.Y, -p.X);
        }

        private static List<Point> Rotate90XY(List<Point> points) {
            return points.Select(Rotate90XY).ToList();
        }

        private static Point Rotate90XY(Point p) {
            return new Point(-p.Y, p.X, p.Z);
        }

        private static List<Point> Move(List<Point> points, int dx, int dy, int dz) {
            return points.Select(point => Move(dx, dy, dz, point)).ToList();
        }

        private static Point Move(int dx, int dy, int dz, Point point) {
            return new Point(point.X + dx, point.Y + dy, point.Z + dz);
        }
    }
}