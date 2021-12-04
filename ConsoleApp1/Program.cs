using System;
using System.Collections.Generic;

namespace ConsoleApp1 {
    internal static class Program {
        private static void Main() {
            string[] ss = new string[10];
            Do(ss);
        }

        private static void Do(IReadOnlyList<string> ss) {
            Console.WriteLine(ss[0].Length);
        }
    }
}