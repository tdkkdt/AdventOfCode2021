using System;
using System.IO;
using System.Linq;

namespace Day1 {
    class Program {
        static void Main(string[] args) {
            int prev = int.MaxValue;
            var ans = File.ReadLines("input")
                .Select(int.Parse)
                .Count(l => {
                    var r = l > prev;
                    prev = l;
                    return r;
                });
            Console.WriteLine(ans);
        }
    }
}