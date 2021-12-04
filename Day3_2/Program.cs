using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3_2 {
    class Program {
        static void Main(string[] args) {
            var lines = File.ReadAllLines("input").ToList();
            var o = Filter(lines, true);
            var co = Filter(lines, false);
            var gg = binToDec(o);
            var ee = binToDec(co);
            Console.WriteLine(gg * ee);
        }

        static string Filter(List<string> ll, bool most) {
            int p = 0;
            while (ll.Count > 1) {
                ll = SearchBy(ll, p, most);
                p++;
            }
            return ll[0];
        }
        
        static List<string> SearchBy(List<string> ll, int p, bool most) {
            int gt = 0;
            foreach (var line in ll) {
                gt += line[p] - '0';
            }
            if (gt * 2 > ll.Count) {
                gt = most ? 1 : 0;
            }
            else if (gt * 2 == ll.Count) {
                gt = most ? 1 : 0;
            }
            else {
                gt = most ? 0 : 1;
            }
            return ll.Where(line => line[p] - '0' == gt).ToList();
        }

        static int binToDec(string c) {
            int p = 1;
            int ans = 0;
            for (int i = c.Length - 1; i >= 0; i--) {
                ans += p * (c[i] - '0');
                p *= 2;
            }
            return ans;
        }
    }
}