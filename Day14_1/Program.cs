using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14_1 {
    class Program {
        static void Main(string[] args) {
            using var f = File.OpenText("input");
            var q = f.ReadLine().ToList();
            f.ReadLine();
            char[,] r = new char[26, 26];
            while (!f.EndOfStream) {
                var rr = f.ReadLine();
                r[rr[0] - 'A', rr[1] - 'A'] = rr[6];
            }
            long[,] pairs = new long[26, 26];
            for (int i = 0; i < q.Count - 1; i++) {
                pairs[q[i] - 'A', q[i + 1] - 'A']++;
            }
            for (int d = 0; d < 40; d++) {
                long[,] newPair = new long[26, 26];
                for (int i = 0; i < 26; i++) {
                    for (int j = 0; j < 26; j++) {
                        var newC = r[i, j];
                        if (newC == '\0') {
                            continue;
                        }
                        newPair[i, newC - 'A'] += pairs[i, j];
                        newPair[newC - 'A', j] += pairs[i, j];
                    }
                }
                pairs = newPair;
            }
            long[] counts = new long[26];
            for (int i = 0; i < 26; i++) {
                for (int j = 0; j < 26; j++) {
                    counts[i] += pairs[i, j];
                    counts[j] += pairs[i, j];
                }
            }
            counts[q[0] - 'A']++;
            counts[q[^1] - 'A']++;
            var min = counts.Where(c => c > 0).Min();
            var max = counts.Max();
            Console.WriteLine((max - min) / 2);
        }
    }
}