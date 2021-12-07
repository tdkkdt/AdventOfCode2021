using System;
using System.IO;
using System.Linq;

namespace Day7_1 {
    class Program {
        static void Main(string[] args) {
            var nums = File.OpenText("input").ReadLine().Split(",").Select(int.Parse).ToArray();
            Array.Sort(nums);
            int i = nums[nums.Length / 2];
            long ans = 0;
            foreach (int num in nums) {
                ans += Math.Abs(num - i);
            }
            Console.WriteLine(ans);
        }
    }
}