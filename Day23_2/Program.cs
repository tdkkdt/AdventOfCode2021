using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace Day23_2 {
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
            return dest[amni.Index] == amni.X && (Array.IndexOf(finishY, amni.Y) != -1);
        }

        static void Print() {
            Console.WriteLine("---------");
            foreach (char[] c in map) {
                Console.WriteLine(c);
            }
            Console.WriteLine("---------");
        }

        struct State {
            private List<int> v;

            public State(Amni[] amnis) {
                v = new List<int>();
                foreach (var amni in amnis) {
                    v.Add(amni.X);
                    v.Add(amni.Y);
                }
            }

            public bool Equals(State other) {
                for (int i = 0; i < v.Count; i++) {
                    if (other.v[i] != v[i]) {
                        return false;
                    }
                }
                return true;
            }

            public override bool Equals(object obj) {
                return obj is State other && Equals(other);
            }

            public override int GetHashCode() {
                HashCode hashCode = new();
                foreach (int i in v) {
                    hashCode.Add(i);
                }
                return hashCode.ToHashCode();
            }
        }

        private static Dictionary<State, long> cache = new();

        private static int[] finishY = { 5, 4, 3, 2 };

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
            if (finished == amnis.Length) {
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
                for (int i = 0; i < finishY.Length; i++) {
                    int y = finishY[i];
                    if (map[y][x] == '.' && (i == 0 || (map[finishY[i - 1]][x] == amni.C && amni.X != x))) {
                        long score = CalcScoreFor(amni, x, y);
                        if (score == -1) {
                            continue;
                        }
                        was2[amni.Index] = true;
                        MoveTo(amni, x, y, score, finished + 1);
                        was2[amni.Index] = false;
                        break;
                    }
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
                    if (Array.IndexOf(finishY, sy) != -1) {
                        nx = sx;
                        ny = sy - 1;
                    }
                    else {
                        nx = dx < sx ? sx - 1 : sx + 1;
                        ny = 1;
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
                    score += 1;
                    continue;
                }
                return -1;
            }
        }

        static void Main(string[] args) {
            List<string> lines = File.ReadAllLines("input").ToList();
            lines.Insert(3, "  #D#B#A#C#");
            lines.Insert(3, "  #D#C#B#A#");
            map = lines.Select(l => l.ToCharArray()).ToArray();
            int max = finishY.Length * 4;
            was1 = new bool[max];
            was2 = new bool[max];
            dest = new int[max];
            amnis = new Amni[max];
            fullAns = long.MaxValue;
            foreach(var y in finishY) {
                for (int x = 3; x <= 9; x += 2) {
                    int index = (map[y][x] - 'A') * finishY.Length;
                    while (amnis[index] != null) {
                        index++;
                    }
                    Amni amni = new(map[y][x], index, x, y);
                    amnis[amni.Index] = amni;
                }
            }
            for (int i = 0; i < max; i++) {
                dest[i] = (amnis[i].C - 'A') * 2 + 3;
            }

            int finished = amnis.Count(IsFinished);
            Perebor(0, finished);
            Console.WriteLine(fullAns);
        }
    }
}