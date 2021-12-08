using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day8_2 {
    class Program {
        static void Main(string[] args) {
            var lines = File.ReadAllLines("input")
                .Select(l => l.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                .Select(l => l.Select(ll => ll.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToArray());
            long ans = 0;
            foreach (var line in lines) {
                ans += Decode(line);
            }
            Console.WriteLine(ans);
        }

        private static long Decode(string[][] line) {
            Dictionary<char, char> map = new Dictionary<char, char>();
            string for1 = line[0].First(l => l.Length == 2);
            string for7 = line[0].First(l => l.Length == 3);
            string for4 = line[0].First(l => l.Length == 4);
            string for8 = line[0].First(l => l.Length == 7);
            foreach (char c in for7) {
                if (!for1.Contains(c)) {
                    map.Add(c, 'a');
                }
            }

            var for069 = line[0].Where(l => l.Length == 6).ToArray();
            var bd = for4.Where(c => !for1.Contains(c)).ToArray();
            string for0 = for069.First(l => !bd.All(l.Contains));
            foreach (char c in bd) {
                map.Add(c, for0.Contains(c) ? 'b' : 'd');
            }

            var for96 = for069.Where(l => !for0.Equals(l)).ToArray();
            var for9 = for96.First(l => for1.All(l.Contains));
            var for6 = for96.First(l => !for9.Equals(l));
            foreach (var c in for9) {
                if (!for6.Contains(c)) {
                    map.Add(c, 'c');
                }
            }
            foreach (var c in for6) {
                if (!for9.Contains(c)) {
                    map.Add(c, 'e');
                }
            }
            foreach (var c in for1) {
                if (!map.ContainsKey(c)) {
                    map.Add(c, 'f');
                }
            }
            foreach (var c in "abcdefg") {
                if (!map.ContainsKey(c)) {
                    map.Add(c, 'g');
                }
            }

            var digits = new[] {
                "abcefg",
                "cf",
                "acdeg",
                "acdfg",
                "bcdf",
                "abdfg",
                "abdefg",
                "acf",
                "abcdefg",
                "abcdfg"
            };

            int ans = 0;
            foreach (var s in line[1]) {
                var newS = new char[s.Length];
                for (int i = 0; i < s.Length; i++) {
                    newS[i] = map[s[i]];
                }
                Array.Sort(newS);
                var nn = new string(newS);
                ans *= 10;
                ans += Array.IndexOf(digits, nn);
            }

            return ans;
        }
    }
}