using System;
using System.Collections.Generic;
using System.IO;

namespace Day10_1 {
    class Program {
        static void Main(string[] args) {
            var pair = new Dictionary<char, char>();
            pair.Add('(', ')');
            pair.Add('[', ']');
            pair.Add('{', '}');
            pair.Add('<', '>');

            var scores = new Dictionary<char, long>();
            scores.Add(')', 3);
            scores.Add(']', 57);
            scores.Add('}', 1197);
            scores.Add('>', 25137);

            long ans = 0;
            foreach (var line in File.ReadLines("input")) {
                Stack<char> stack = new Stack<char>();
                foreach (var c in line) {
                    if (pair.ContainsKey(c)) {
                        stack.Push(pair[c]);
                    }
                    else {
                        if (stack.Pop() != c) {
                            ans += scores[c];
                            break;
                        }
                    }
                }
            }
            Console.WriteLine(ans);
        }
    }
}