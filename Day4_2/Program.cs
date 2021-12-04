using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4_2 {
    class Program {
        class Board {
            public int[][] inner;
            public bool[,] was;

            public Board(int[][] inner) {
                this.inner = inner;
                was = new bool[5, 5];
            }

            public bool Set(int n) {
                for (int i = 0; i < 5; i++) {
                    for (int j = 0; j < 5; j++) {
                        if (inner[i][j] == n) {
                            was[i, j] = true;
                            int rc = 0;
                            int cc = 0;
                            for (int k = 0; k < 5; k++) {
                                if (was[i, k]) rc++;
                                if (was[k, j]) cc++;
                            }
                            if (rc == 5 || cc == 5) {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            public int SumLast() {
                int ans = 0;
                for (int i = 0; i < 5; i++) {
                    for (int j = 0; j < 5; j++) {
                        if (!was[i,j]) {
                            ans+=inner[i][j];
                        }
                    }
                }
                return ans;
            }
        }

        static void Main(string[] args) {
            using (var f = File.OpenText("input")) {
                var numbers = f.ReadLine().Split(",").Select(int.Parse).ToArray();
                List<Board> boards = new List<Board>();
                while (!f.EndOfStream) {
                    f.ReadLine();
                    var board = new int[5][];
                    for (int i = 0; i < 5; i++) {
                        board[i] = f.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    }
                    boards.Add(new Board(board));
                }

                foreach (int n in numbers) {
                    for (int i = boards.Count - 1; i >= 0; i--) {
                        if (boards[i].Set(n)) {
                            if (boards.Count == 1) {
                                Console.WriteLine(n * boards[i].SumLast());
                                return;
                            }
                            boards.RemoveAt(i);
                        }
                    }
                }                
            }
        }
    }
}