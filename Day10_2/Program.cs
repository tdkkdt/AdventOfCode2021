using System;
using System.Collections.Generic;
using System.IO;

namespace Day10_2 {
    class Program {
        static void Main(string[] args) {
            var pair = new Dictionary<char, char>();
            pair.Add('(', ')');
            pair.Add('[', ']');
            pair.Add('{', '}');
            pair.Add('<', '>');

            var scores = new Dictionary<char, long>();
            scores.Add(')', 1);
            scores.Add(']', 2);
            scores.Add('}', 3);
            scores.Add('>', 4);

            List<long> ans = new List<long>();
            foreach (var line in File.ReadLines("input")) {
                Stack<char> stack = new Stack<char>();
                foreach (var c in line) {
                    if (pair.ContainsKey(c)) {
                        stack.Push(pair[c]);
                    }
                    else {
                        if (stack.Pop() != c) {
                            stack.Clear();
                            break;
                        }
                    }
                }
                if (stack.Count > 0) {
                    long aa = 0;
                    foreach (var c in stack) {
                        aa *= 5;
                        aa += scores[c];
                    }
                    ans.Add(aa);
                }
            }
            ans.Sort();
            Console.WriteLine(ans[ans.Count / 2]);
        }
    }
}