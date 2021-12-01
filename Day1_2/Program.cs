using System;
using System.IO;
using System.Linq;

namespace Day1_2 {
    class Program {
        static void Main(string[] args) {
            var depths = File.ReadLines("input").Select(int.Parse).ToArray();
            int sum = 0;
            for (int i = 0; i < 3; i++) {
                sum += depths[i];
            }
            int ans = 0;
            for (int i = 3; i < depths.Length; i++) {
                var prevSum = sum;
                sum -= depths[i - 3];
                sum += depths[i];
                if (sum > prevSum) {
                    ans++;
                }
            }
            Console.WriteLine(ans);
        }
    }
}