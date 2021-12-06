using System;
using System.IO;
using System.Linq;

namespace Day6_1 {
    class Program {
        static void Main(string[] args) {
            var lines = File.ReadLines("input")
                .Select(l => l.Split(",", StringSplitOptions.RemoveEmptyEntries))
                .ToArray();
            var fishes = lines[0].Select(l => int.Parse(l)).ToList();
            long[] ff = new long[9];
            foreach (int f in fishes) {
                ff[f]++;
            }
            for (int d = 0; d < 256; d++) {
                long[] newFF = new long[9];
                for (int i = 0; i < 9; i++) {
                    newFF[i] = ff[(i + 1) % 9];
                }
                newFF[6] += ff[0];
                ff = newFF;
            }
            Console.WriteLine(ff.Sum());
        }
    }
}