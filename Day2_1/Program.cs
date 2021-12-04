using System;
using System.IO;
using System.Linq;

namespace Day2_1 {
    class Program {
        static void Main(string[] args) {
            var comands = File.ReadLines("input").Select(l => l.Split(" "));
            int d = 0;
            int h = 0;
            foreach (var c in comands) {
                int v = int.Parse(c[1]);
                switch (c[0]) {
                    case "forward": h += v;
                        break;
                    case "down":
                        d += v;
                        break;
                    case "up":
                        d -= v;
                        break;
                }
            }
            Console.WriteLine(d * h);
        }
    }
}