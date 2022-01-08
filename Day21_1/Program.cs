using System;
using System.IO;

namespace Day21_1 {
    class Program {
        static void Main(string[] args) {
            var f = File.OpenText("input");
            var p1 = int.Parse(f.ReadLine().Substring("Player 1 starting position: ".Length));
            var p2 = int.Parse(f.ReadLine().Substring("Player 2 starting position: ".Length));
            long s1 = 0;
            long s2 = 0;
            long ds = 0;
            int dd = 1;
            bool turn = true;
            while (s1 < 1000 && s2 < 1000) {
                int ddd = 0;
                for (int i = 0; i < 3; i++) {
                    ddd += dd;
                    dd++;
                    if (dd == 101) {
                        dd = 1;
                    }
                    ds++;
                }
                if (turn) {
                    p1 = ((p1 - 1 + 10) % 10 + ddd) % 10 + 1;
                    s1 += p1;
                }
                else {
                    p2 = ((p2 - 1 + 10) % 10 + ddd) % 10 + 1;
                    s2 += p2;
                }
                turn = !turn;
            }
            long ans = 0;
            if (s1 < 1000) {
                ans = s1 * ds;
            }
            else {
                ans = s2 * ds;
            }
            Console.WriteLine(ans);
        }
    }
}