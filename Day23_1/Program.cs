using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23_1 {
    class Program {
        class Amni {
            public char C { get; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Index { get; }
            public int Koef { get; }

            public Amni(char c, int index, int x, int y) {
                C = c;
                Index = index;
                X = x;
                Y = y;
                Koef = C switch {
                    'A' => 1,
                    'B' => 10,
                    'C' => 100,
                    'D' => 1000,
                    _ => throw new Exception()
                };
            }

            public override string ToString() {
                return $"{nameof(C)}: {C}, {nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Index)}: {Index}";
            }
        }

        private static int[] dest;
        private static Amni[] amnis;
        private static bool[] was1;
        private static bool[] was2;
        private static char[][] map;
        private static long fullAns;

        static bool IsFinished(Amni amni) {
            return dest[amni.Index] == amni.X && (amni.Y == 2 || amni.Y == 3);
        }
        
        static void Print() {
            Console.WriteLine("---------");
            foreach (char[] c in map) {
                Console.WriteLine(c);
            }
            Console.WriteLine("---------");
        }

        struct State {
            private readonly int a1x, a1y, a2x, a2y, b1x, b1y, b2x, b2y, c1x, c1y, c2x, c2y, d1x, d1y, d2x, d2y;

            public State(Amni[] amnis) {
                a1x = amnis[0].X;
                a1y = amnis[0].Y;
                a2x = amnis[1].X;
                a2y = amnis[1].Y;
                b1x = amnis[2].X;
                b1y = amnis[2].Y;
                b2x = amnis[3].X;
                b2y = amnis[3].Y;
                c1x = amnis[4].X;
                c1y = amnis[4].Y;
                c2x = amnis[5].X;
                c2y = amnis[5].Y;
                d1x = amnis[6].X;
                d1y = amnis[6].Y;
                d2x = amnis[7].X;
                d2y = amnis[7].Y;
            }

            public bool Equals(State other) {
                return a1x == other.a1x && a1y == other.a1y && a2x == other.a2x && a2y == other.a2y &&
                       b1x == other.b1x && b1y == other.b1y && b2x == other.b2x && b2y == other.b2y &&
                       c1x == other.c1x && c1y == other.c1y && c2x == other.c2x && c2y == other.c2y &&
                       d1x == other.d1x && d1y == other.d1y && d2x == other.d2x && d2y == other.d2y;
            }

            public override bool Equals(object obj) {
                return obj is State other && Equals(other);
            }

            public override int GetHashCode() {
                HashCode hashCode = new();
                hashCode.Add(a1x);
                hashCode.Add(a1y);
                hashCode.Add(a2x);
                hashCode.Add(a2y);
                hashCode.Add(b1x);
                hashCode.Add(b1y);
                hashCode.Add(b2x);
                hashCode.Add(b2y);
                hashCode.Add(c1x);
                hashCode.Add(c1y);
                hashCode.Add(c2x);
                hashCode.Add(c2y);
                hashCode.Add(d1x);
                hashCode.Add(d1y);
                hashCode.Add(d2x);
                hashCode.Add(d2y);
                return hashCode.ToHashCode();
            }
        }

        private static Dictionary<State, long> cache = new();

        static void Perebor(long res, int finished) {
            void MoveTo(Amni amni, int x, int y, long score, int newFinished) {
                map[amni.Y][amni.X] = '.';
                map[y][x] = amni.C;
                var prevX = amni.X;
                var prevY = amni.Y;
                amni.X = x;
                amni.Y = y;
                Perebor(res + score, newFinished);
                amni.X = prevX;
                amni.Y = prevY;
                map[amni.Y][amni.X] = amni.C;
                map[y][x] = '.';
            }

            var state = new State(amnis);
            if (cache.TryGetValue(state, out long prevScore) && prevScore <= res) {
                return;
            }

            cache[state] = res;
            
            if (res >= fullAns) {
                return;
            }
            if (finished == 8) {
                if (res < fullAns) {
                    fullAns = res;
                }
                return;
            }
            foreach (Amni amni in amnis) {
                if (was1[amni.Index])
                    continue;
                for (int x = 1; x <= 11; x++) {
                    if (x == 3 || x == 5 || x == 7 || x == 9) {
                        continue;
                    }
                    long score = CalcScoreFor(amni, x, 1);
                    if (score == -1) {
                        continue;
                    }
                    was1[amni.Index] = true;
                    var newFinished = IsFinished(amni) ? finished - 1 : finished;
                    MoveTo(amni, x, 1, score, newFinished);
                    was1[amni.Index] = false;
                }
            }
            foreach (Amni amni in amnis) {
                if (was2[amni.Index])
                    continue;
                if (!was1[amni.Index]) {
                    continue;
                }
                if (IsFinished(amni)) {
                    continue;
                }
                var x = dest[amni.Index];
                if (map[3][x] == '.') {
                    long score = CalcScoreFor(amni, x, 3);
                    if (score == -1) {
                        continue;
                    }
                    was2[amni.Index] = true;
                    MoveTo(amni, x, 3, score, finished + 1);
                    was2[amni.Index] = false;
                }
                else if (map[2][x] == '.' && map[3][x] == amni.C && amni.X != x) {
                    long score = CalcScoreFor(amni, x, 2);
                    if (score == -1) {
                        continue;
                    }
                    was2[amni.Index] = true;
                    MoveTo(amni, x, 2, score, finished + 1);
                    was2[amni.Index] = false;
                }
            }
        }

        private static long CalcScoreFor(Amni amni, int x, int y) {
            if (map[y][x] != '.') {
                return -1;
            }
            var baseScore = CalcScoreForCore(amni.X, amni.Y, x, y, 0);
            if (baseScore == -1) {
                return baseScore;
            }
            return baseScore * amni.Koef;
        }

        private static long CalcScoreForCore(int sx, int sy, int dx, int dy, long score) {
            while (true) {
                if (sx == dx && sy == dy) {
                    return score;
                }
                int nx;
                int ny;
                if (dy == 1) {
                    switch (sy) {
                        case 3:
                            nx = sx;
                            ny = 2;
                            break;
                        case 2:
                            nx = sx;
                            ny = 1;
                            break;
                        case 1:
                            nx = dx < sx ? sx - 1 : sx + 1;
                            ny = 1;
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else {
                    if (sx == dx) {
                        nx = sx;
                        ny = sy + 1;
                    }
                    else {
                        ny = 1;
                        nx = dx < sx ? sx - 1 : sx + 1;
                    }
                }
                if (map[ny][nx] == '.') {
                    sx = nx;
                    sy = ny;
                    score = score + 1;
                    continue;
                }
                return -1;
            }
        }

        static void Main(string[] args) {
            map = File.ReadAllLines("input").Select(l => l.ToCharArray()).ToArray();
            was1 = new bool[8];
            was2 = new bool[8];
            dest = new int[8];
            amnis = new Amni[8];
            fullAns = long.MaxValue;
            for (int y = 2; y < 4; y++) {
                for (int x = 3; x <= 9; x += 2) {
                    int index = map[y][x] switch {
                        'A' => amnis[0] == null ? 0 : 1,
                        'B' => amnis[2] == null ? 2 : 3,
                        'C' => amnis[4] == null ? 4 : 5,
                        'D' => amnis[6] == null ? 6 : 7,
                        _ => throw new Exception()
                    };
                    Amni amni = new(map[y][x], index, x, y);
                    amnis[amni.Index] = amni;
                }
            }
            dest[0] = 3;
            dest[1] = 3;
            dest[2] = 5;
            dest[3] = 5;
            dest[4] = 7;
            dest[5] = 7;
            dest[6] = 9;
            dest[7] = 9;
            int finished = amnis.Count(IsFinished);
            Perebor(0, finished);
            Console.WriteLine(fullAns);
        }
    }
}