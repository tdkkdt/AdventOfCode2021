using System;
using System.IO;
using System.Linq;

namespace Day3_1 {
    class Program {
        static void Main(string[] args) {
            var lines = File.ReadAllLines("input").ToArray();
            int[] g = new int[lines[0].Length];
            int[] e = new int[lines[0].Length];
            int[] gt = new int[lines[0].Length];
            for (int i = 0; i < lines[0].Length; i++) {
                foreach (var line in lines) {
                    gt[i] += line[i] - '0';
                }
                gt[i] = gt[i] / (lines.Length / 2);
                g[i] = gt[i];
                e[i] = 1 - gt[i];
            }
            var gg = binToDec(g);
            var ee = binToDec(e);
            Console.WriteLine(gg * ee);
        }

        static int binToDec(int[] c) {
            int p = 1;
            int ans = 0;
            for (int i = c.Length - 1; i >= 0; i--) {
                ans += p * c[i];
                p *= 2;
            }
            return ans;
        }
    }
}