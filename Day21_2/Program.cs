using System;
using System.Collections.Generic;
using System.IO;

namespace Day21_2 {
    class Program {
        struct Universe {
            public readonly int Score1;
            public readonly int Score2;
            public readonly int Pos1;
            public readonly int Pos2;

            public Universe(int score1, int score2, int pos1, int pos2) {
                Score1 = score1;
                Score2 = score2;
                Pos1 = pos1;
                Pos2 = pos2;
            }

            public override string ToString() {
                return $"{nameof(Score1)}: {Score1}, {nameof(Score2)}: {Score2}, {nameof(Pos1)}: {Pos1}, {nameof(Pos2)}: {Pos2}";
            }

            public bool Equals(Universe other) {
                return Score1 == other.Score1 && Score2 == other.Score2 && Pos1 == other.Pos1 && Pos2 == other.Pos2;
            }

            public override bool Equals(object obj) {
                return obj is Universe other && Equals(other);
            }

            public override int GetHashCode() {
                return HashCode.Combine(Score1, Score2, Pos1, Pos2);
            }
        }

        private static void AddUni(Dictionary<Universe, long> unis, Universe uni, long count) {
            unis.TryGetValue(uni, out long currentCount);
            unis[uni] = count + currentCount;
        }

        const int wins = 21;
        
        static void Main(string[] args) {
            var f = File.OpenText("input");
            var p1 = int.Parse(f.ReadLine().Substring("Player 1 starting position: ".Length));
            var p2 = int.Parse(f.ReadLine().Substring("Player 2 starting position: ".Length));
            var first = new Universe(0, 0, p1, p2);
            long win1 = 0, win2 = 0;
            Dictionary<Universe, long> unis = new();
            AddUni(unis, first, 1);
            bool firstPlayer = true;
            while (unis.Count > 0) {
                Dictionary<Universe, long> next;
                for (int i = 0; i < 3; i++) {
                    next = new();
                    foreach ((Universe uni, long count) in unis) {
                        if (firstPlayer) {
                            AddUni(next, new Universe(uni.Score1, uni.Score2, uni.Pos1 + 1, uni.Pos2), count);
                            AddUni(next, new Universe(uni.Score1, uni.Score2, uni.Pos1 + 2, uni.Pos2), count);
                            AddUni(next, new Universe(uni.Score1, uni.Score2, uni.Pos1 + 3, uni.Pos2), count);
                        }
                        else {
                            AddUni(next, new Universe(uni.Score1, uni.Score2, uni.Pos1, uni.Pos2 + 1), count);
                            AddUni(next, new Universe(uni.Score1, uni.Score2, uni.Pos1, uni.Pos2 + 2), count);
                            AddUni(next, new Universe(uni.Score1, uni.Score2, uni.Pos1, uni.Pos2 + 3), count);
                        }
                    }
                    unis = next;
                }
                next = new();
                foreach ((Universe uni, long count) in unis) {
                    if (firstPlayer) {
                        var newPos = (uni.Pos1 - 1) % 10 + 1;
                        var newScore = uni.Score1 + newPos;
                        if (newScore >= wins) {
                            win1 += count;
                        }
                        else {
                            AddUni(next, new Universe(newScore, uni.Score2, newPos, uni.Pos2), count);
                        }
                    }
                    else {
                        var newPos = (uni.Pos2 - 1) % 10 + 1;
                        var newScore = uni.Score2 + newPos;
                        if (newScore >= wins) {
                            win2 += count;
                        }
                        else {
                            AddUni(next, new Universe(uni.Score1, newScore, uni.Pos1, newPos), count);
                        }
                    }
                }
                unis = next;
                firstPlayer = !firstPlayer;
            }
            Console.WriteLine(Math.Max(win1, win2));
        }
    }
}