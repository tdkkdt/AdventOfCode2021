using System;
using System.IO;
using System.Linq;

namespace Day8_1 {
    class Program {
        static void Main(string[] args) {
            var lines = File.ReadAllLines("input")
                .Select(l => l.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                .Select(l => l[1])
                .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            var ans = lines
                .Select(ll => ll.Count(lll => lll.Length == 2 || lll.Length == 3 || lll.Length == 4 || lll.Length == 7))
                .Sum();
            Console.WriteLine(ans);
        }
    }
}