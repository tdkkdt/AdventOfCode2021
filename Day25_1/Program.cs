using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25_1 {
    class Program {
        class Point {
            public int X;
            public int Y;
        }

        static void Print(char[][] map) {
            foreach (var c in map) {
                Console.WriteLine(c);
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Main(string[] args) {
            var map = File.ReadAllLines("input").Select(l => l.ToCharArray()).ToArray();
            bool changed = true;
            int ans = 0;
            int maxy = map.Length;
            int maxx = map[0].Length;
            while (changed) {
                ans++;
                changed = false;
                for (int y = 0; y < maxy; y++) {
                    for (int x = 0; x < maxx; x++) {
                        if (map[y][x] != '>') {
                            continue;
                        }
                        int nx = x + 1;
                        nx %= maxx;
                        if (map[y][nx] == '.') {
                            map[y][nx] = '>';
                            map[y][x] = x == 0 ? '#' : '.';
                            x++;
                            changed = true;
                        }
                    }
                }

                for (int y = 0; y < maxy; y++) {
                    if (map[y][0] == '#') {
                        map[y][0] = '.';
                    }
                }

                for (int x = 0; x < maxx; x++) {
                    for (int y = 0; y < maxy; y++) {
                        if (map[y][x] != 'v') {
                            continue;
                        }
                        int ny = y + 1;
                        ny %= maxy;
                        if (map[ny][x] == '.') {
                            map[ny][x] = 'v';
                            map[y][x] = y == 0 ? '#' : '.';
                            y++;
                            changed = true;
                        }
                    }
                }
                
                for (int x = 0; x < maxx; x++) {
                    if (map[0][x] == '#') {
                        map[0][x] = '.';
                    }
                }

                // Console.WriteLine(ans);
                // Print(map);
            }
            Console.WriteLine(ans);
        }
    }
}