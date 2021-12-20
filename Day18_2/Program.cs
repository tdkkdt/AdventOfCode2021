using System;
using System.Collections.Generic;
using System.IO;

namespace Day18_2 {
    class Program {
        abstract class Entry {
            public abstract long Magnitude();
            public Pair Parent;
            public abstract Entry Copy();
        }

        class Regular : Entry {
            public long Value;

            public override long Magnitude() {
                return Value;
            }

            public override Entry Copy() {
                return new Regular(Value);
            }

            public Regular(long value) {
                Value = value;
            }

            public override string ToString() {
                return Value.ToString();
            }
        }

        class Pair : Entry {
            public Entry Left;
            public Entry Right;

            public Pair(Entry left, Entry right) {
                Left = left;
                Right = right;
                left.Parent = this;
                right.Parent = this;
            }

            public override long Magnitude() {
                return 3 * Left.Magnitude() + 2 * Right.Magnitude();
            }

            public override Entry Copy() {
                return new Pair(Left.Copy(), Right.Copy());
            }

            public override string ToString() {
                return $"[{Left},{Right}]";
            }
        }

        static Entry Parse(string s, ref int p) {
            if (s[p] == '[') {
                p++; // [
                var left = Parse(s, ref p);
                p++; // ,
                var right = Parse(s, ref p);
                p++; // ]
                return new Pair(left, right);
            }
            else {
                long v = 0;
                while (char.IsDigit(s[p])) {
                    v *= 10;
                    v += s[p] - '0';
                    p++;
                }
                return new Regular(v);
            }
        }

        static void Main(string[] args) {
            List<Entry> entries = new List<Entry>();
            foreach (var line in File.ReadLines("input")) {
                int p = 0;
                var entry = Parse(line, ref p);
                entries.Add(entry);
            }
            long ans = 0;
            for (int i = 0; i < entries.Count; i++) {
                for (int j = 0; j < entries.Count; j++) {
                    if (i == j) {
                        continue;
                    }
                    Entry sum = new Pair(entries[i].Copy(), entries[j].Copy());
                    bool wasReduce = false;
                    do {
                        (sum, wasReduce) = Reduce(sum);
                    } while (wasReduce);
                    ans = Math.Max(ans, sum.Magnitude());
                }
            }
            Console.WriteLine(ans);
        }

        private static (Entry, bool) Reduce(Entry entry) {
            var (newResult, wasExplode) = Explode(entry, 0);
            if (wasExplode) {
                return (newResult, true);
            }
            return Split(entry);
        }

        private static (Entry, bool) Split(Entry entry) {
            if (entry is Pair pair) {
                var (newLeft, wasOnLeft) = Split(pair.Left);
                if (wasOnLeft) {
                    return (new Pair(newLeft, pair.Right), true);
                }
                var (newRight, wasOnRight) = Split(pair.Right);
                if (wasOnRight) {
                    return (new Pair(pair.Left, newRight), true);
                }
                return (entry, false);
            }
            var rr = (Regular)entry;
            if (rr.Value >= 10) {
                return (new Pair(new Regular(rr.Value / 2), new Regular(rr.Value / 2 + rr.Value % 2)), true);
            }
            return (entry, false);
        }

        private static (Entry, bool) Explode(Entry entry, int d) {
            if (entry is Regular) {
                return (entry, false);
            }
            var pair = (Pair)entry;
            if (d < 4) {
                var (newLeft, wasOnLeft) = Explode(pair.Left, d + 1);
                if (wasOnLeft) {
                    return (new Pair(newLeft, pair.Right), true);
                }
                var (newRight, wasOnRight) = Explode(pair.Right, d + 1);
                if (wasOnRight) {
                    return (new Pair(pair.Left, newRight), true);
                }
                return (entry, false);
            }
            if (!(pair.Left is Regular && pair.Right is Regular)) {
                throw new Exception();
            }
            AddToLeft(entry, ((Regular)pair.Left).Value);
            AddToRight(entry, ((Regular)pair.Right).Value);
            return (new Regular(0), true);
        }

        private static void AddToRight(Entry entry, long value) {
            var parent = entry.Parent;
            while (parent != null && parent.Right == entry) {
                entry = parent;
                parent = parent.Parent;
            }
            if (parent == null) {
                return;
            }
            Entry e = parent.Right;
            while (e is Pair pair) {
                e = pair.Left;
            }
            var rr = (Regular)e;
            rr.Value += value;
        }

        private static void AddToLeft(Entry entry, long value) {
            var parent = entry.Parent;
            while (parent != null && parent.Left == entry) {
                entry = parent;
                parent = parent.Parent;
            }
            if (parent == null) {
                return;
            }
            Entry e = parent.Left;
            while (e is Pair pair) {
                e = pair.Right;
            }
            var rr = (Regular)e;
            rr.Value += value;
        }
    }
}