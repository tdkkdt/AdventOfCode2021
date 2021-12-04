using System;
using System.IO;
using System.Linq;

namespace Day2_2 {
    class Program {
        static void Main(string[] args) {
            var comands = File.ReadLines("input").Select(l => l.Split(" "));
            long d = 0;
            long h = 0;
            long a = 0;
            foreach (var c in comands) {
                int v = int.Parse(c[1]);
                switch (c[0]) {
                    case "forward": 
                        h += v;
                        d += a * v;
                        break;
                    case "down":
                        a += v;
                        break;
                    case "up":
                        a -= v;
                        break;
                }
            }
            Console.WriteLine(d * h);
        }
    }
}