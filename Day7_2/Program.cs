using System;

namespace Day7_2 {
    class Program {
        static void Main(string[] args) {
            var nums = File.OpenText("input").ReadLine().Split(",").Select(int.Parse).ToArray();
            Array.Sort(nums);
            int min = nums[0];
            int max = nums[^1];
            long ans = long.MaxValue;
            for (int i = min; i <= max; i++) {
                long s = 0;
                foreach (int num in nums) {
                    var d = Math.Abs(num - i);
                    long dd = d * (d + 1) / 2;
                    s += dd;
                }
                ans = Math.Min(ans, s);
            }
            Console.WriteLine(ans);
        }
    }
}