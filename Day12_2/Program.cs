using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12_2 {
    class Program {
        static long Perebor(Dictionary<string, List<string>> routes, HashSet<string> wasSmall, string cur, string twice) {
            if (cur == "end") {
                return 1;
            }
            long ans = 0;
            foreach (var route in routes[cur]) {
                var small = route.All(c => 'a' <= c && c <= 'z');
                if (!wasSmall.Contains(route)) {
                    if (small) {
                        wasSmall.Add(route);
                    }
                    ans += Perebor(routes, wasSmall, route, twice);
                    if (small) {
                        wasSmall.Remove(route);
                    }
                }
                else if (small && twice == null && route != "start") {
                    ans += Perebor(routes, wasSmall, route, route);
                }
            }
            return ans;
        }

        static void Main(string[] args) {
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();
            foreach (string l in File.ReadLines("input")) {
                var ss = l.Split("-", StringSplitOptions.RemoveEmptyEntries);
                List<string> sss;
                if (!map.TryGetValue(ss[0], out sss)) {
                    sss = new List<string>();
                    map[ss[0]] = sss;
                }
                sss.Add(ss[1]);
                if (!map.TryGetValue(ss[1], out sss)) {
                    sss = new List<string>();
                    map[ss[1]] = sss;
                }
                sss.Add(ss[0]);
            }
            var wasSmall = new HashSet<string>();
            wasSmall.Add("start");
            var ans = Perebor(map, wasSmall, "start", null);
            Console.WriteLine(ans);
        }
    }
}