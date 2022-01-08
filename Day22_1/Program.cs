using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22_1 {
    class Program {
        struct Area {
            public int x1;
            public int x2;
            public int y1;
            public int y2;
            public int z1;
            public int z2;
            private int? hashCode;

            public Area(int x1, int x2, int y1, int y2, int z1, int z2) {
                this.x1 = x1;
                this.x2 = x2;
                this.y1 = y1;
                this.y2 = y2;
                this.z1 = z1;
                this.z2 = z2;
                hashCode = null;
            }

            public bool Equals(Area other) {
                return x1 == other.x1 && x2 == other.x2 && y1 == other.y1 && y2 == other.y2 && z1 == other.z1 &&
                       z2 == other.z2;
            }

            public override bool Equals(object obj) {
                return obj is Area other && Equals(other);
            }

            public override int GetHashCode() {
                hashCode ??= HashCode.Combine(x1, x2, y1, y2, z1, z2);
                return hashCode.Value;
            }

            public override string ToString() {
                return
                    $"x = [{x1} - {x2}], y = [{y1} - {y2}], z = [{z1} - {z2}]";
            }

            public bool Validate() {
                return x2 >= x1 && y2 >= y1 && z2 >= z1;
            }

            public (int, int, int) Middle() {
                int mx = MiddleX();
                int my = MiddleY();
                int mz = MiddleZ();
                return (mx, my, mz);
            }

            public int MiddleZ() {
                return z1 + (z2 - z1) / 2;
            }

            public int MiddleY() {
                return y1 + (y2 - y1) / 2;
            }

            public int MiddleX() {
                return x1 + (x2 - x1) / 2;
            }

            public (Area, Area) SplitX(int mx) {
                return (
                    new Area(x1, Math.Min(mx, x2), y1, y2, z1, z2),
                    new Area(Math.Max(mx + 1, x1), x2, y1, y2, z1, z2)
                );
            }

            public (Area, Area) SplitY(int my) {
                return (
                    new Area(x1, x2, y1, Math.Min(my, y2), z1, z2),
                    new Area(x1, x2, Math.Max(my + 1, y1), y2, z1, z2)
                );
            }

            public (Area, Area) SplitZ(int mz) {
                return (
                    new Area(x1, x2, y1, y2, z1, Math.Min(mz, z2)),
                    new Area(x1, x2, y1, y2, Math.Max(mz + 1, z1), z2)
                );
            }

            public bool isOne() {
                return x1 == x2 && y1 == y2 && z1 == z2;
            }

            public long Calc() {
                return (x2 - x1 + 1L) * (z2 - z1 + 1L) * (y2 - y1 + 1L);
            }
        }

        struct Command {
            public Area area;
            public bool value;

            public static Command Parse(string v) {
                var ss = v.Split(new[] { ' ', '=', '.', ',', 'x', 'y', 'z'}, StringSplitOptions.RemoveEmptyEntries);
                var r = new Command();
                r.value = ss[0] == "on";
                var x1 = int.Parse(ss[1]);
                var x2 = int.Parse(ss[2]);
                var y1 = int.Parse(ss[3]);
                var y2 = int.Parse(ss[4]);
                var z1 = int.Parse(ss[5]);
                var z2 = int.Parse(ss[6]);
                r.area = new Area(x1, x2, y1, y2, z1, z2);
                return r;
            }
        }

        class SegmentX {
            public int L { get; private set; }
            public int R { get; private set; }
            public SegmentY Value { get; set; }
            public SegmentX Left { get; set; }
            public SegmentX Right { get; set; }

            public SegmentX(int l, int r) {
                L = l;
                R = r;
            }
        }
        
        class SegmentY {
            public int L { get; private set; }
            public int R { get; private set; }
            public SegmentZ Value { get; set; }
            public SegmentY Left { get; set; }
            public SegmentY Right { get; set; }

            public SegmentY(int l, int r) {
                L = l;
                R = r;
            }
        }

        class SegmentZ {
            public int L { get; private set; }
            public int R { get; private set; }
            public bool? Value { get; set; }
            public SegmentZ Left { get; set; }
            public SegmentZ Right { get; set; }

            public SegmentZ(int l, int r) {
                L = l;
                R = r;
            }
        }

        private static SegmentY Copy(SegmentY val) {
            if (val == null) {
                return null;
            }
            SegmentY result = new(val.L, val.R) {
                Left = Copy(val.Left),
                Right = Copy(val.Right),
                Value = Copy(val.Value)
            };
            return result;
        }

        private static SegmentZ Copy(SegmentZ val) {
            if (val == null) {
                return null;
            }
            SegmentZ result = new(val.L, val.R) {
                Left = Copy(val.Left),
                Right = Copy(val.Right),
                Value = val.Value
            };
            return result;
        }

        private static void ModifyX(Area area, Area valArea, SegmentX segment, bool val, Action<SegmentX> setSegment) {
            void CreateSegment() {
                if (segment == null) {
                    SegmentX newSegment = new(area.x1, area.x2);
                    setSegment(newSegment);
                    segment = newSegment;
                }
            }

            if (!area.Validate()) {
                return;
            }
            if (!valArea.Validate()) {
                return;
            }
            if (valArea.x1 > area.x2 || valArea.x2 < area.x1) {
                return;
            }
            if (valArea.y1 > area.y2 || valArea.y2 < area.y1) {
                return;
            }
            if (valArea.z1 > area.z2 || valArea.z2 < area.z1) {
                return;
            }
            if (area.x1 != area.x2 && (area.x1 != valArea.x1 || area.x2 != valArea.x2 || (segment?.Left != null || segment?.Right != null))) {
                var midX = area.MiddleX();
                var (newArea1, newArea2) = area.SplitX(midX);
                var (newValArea1, newValArea2) = valArea.SplitX(midX);
                if (segment?.Value != null) {
                    segment.Left = new SegmentX(newArea1.x1, newArea1.x2) { Value = segment.Value };
                    segment.Right = new SegmentX(newArea2.x1, newArea2.x2) { Value = Copy(segment.Value) };
                    segment.Value = null;
                }
                ModifyX(newArea1, newValArea1, segment?.Left, val, (s) => {
                    CreateSegment();
                    segment.Left = s;
                });
                ModifyX(newArea2, newValArea2, segment?.Right, val, (s) => {
                    CreateSegment();
                    segment.Right = s;
                });
            }
            else {
                ModifyY(area, valArea, segment?.Value, val, (s) => {
                    CreateSegment();
                    segment.Value = s;
                    segment.Left = null;
                    segment.Right = null;
                });
            }
        }

        private static void ModifyY(Area area, Area valArea, SegmentY segment, bool val, Action<SegmentY> setSegment) {
            void CreateSegment() {
                if (segment == null) {
                    var newSegment = new SegmentY(area.y1, area.y2);
                    setSegment(newSegment);
                    segment = newSegment;
                }
            }

            if (!area.Validate()) {
                return;
            }
            if (!valArea.Validate()) {
                return;
            }
            if (valArea.x1 > area.x2 || valArea.x2 < area.x1) {
                throw new Exception();
            }
            if (valArea.y1 > area.y2 || valArea.y2 < area.y1) {
                return;
            }
            if (valArea.z1 > area.z2 || valArea.z2 < area.z1) {
                return;
            }
            if (area.y1 != area.y2 && (area.y1 != valArea.y1 || area.y2 != valArea.y2 || (segment?.Left != null || segment?.Right != null))) {
                var midY = area.MiddleY();
                var (newArea1, newArea2) = area.SplitY(midY);
                var (newValArea1, newValArea2) = valArea.SplitY(midY);
                if (segment?.Value != null) {
                    segment.Left = new SegmentY(newArea1.y1, newArea1.y2) { Value = segment.Value };
                    segment.Right = new SegmentY(newArea2.y1, newArea2.y2) { Value = Copy(segment.Value) };
                    segment.Value = null;
                }
                ModifyY(newArea1, newValArea1, segment?.Left, val, (s) => {
                    CreateSegment();
                    segment.Left = s;
                });
                ModifyY(newArea2, newValArea2, segment?.Right, val, (s) => {
                    CreateSegment();
                    segment.Right = s;
                });
            } else {
                ModifyZ(area, valArea, segment?.Value, val, (s) => {
                    CreateSegment();
                    segment.Value = s;
                    segment.Left = null;
                    segment.Right = null;
                });
            }
        }

        private static void ModifyZ(Area area, Area valArea, SegmentZ segment, bool val, Action<SegmentZ> setSegment) {
            void CreateSegment() {
                if (segment == null) {
                    var newSegment = new SegmentZ(area.z1, area.z2);
                    setSegment(newSegment);
                    segment = newSegment;
                }
            }

            if (!area.Validate()) {
                return;
            }
            if (!valArea.Validate()) {
                return;
            }
            if (valArea.x1 > area.x2 || valArea.x2 < area.x1) {
                throw new Exception();
            }
            if (valArea.y1 > area.y2 || valArea.y2 < area.y1) {
                throw new Exception();
            }
            if (valArea.z1 > area.z2 || valArea.z2 < area.z1) {
                return;
            }
            if (segment != null && segment.Value == val) {
                return;
            }
            var midZ = area.MiddleZ();
            var (newArea1, newArea2) = area.SplitZ(midZ);
            var (newValArea1, newValArea2) = valArea.SplitZ(midZ);
            if (area.z1 != area.z2 && (area.z1 != valArea.z1 || area.z2 != valArea.z2)) {
                if (segment?.Value != null) {
                    segment.Left = new SegmentZ(newArea1.z1, newArea1.z2) { Value = segment.Value };
                    segment.Right = new SegmentZ(newArea2.z1, newArea2.z2) { Value = segment.Value };
                }
                ModifyZ(newArea1, newValArea1, segment?.Left, val, (s) => {
                    CreateSegment();
                    segment.Left = s;
                });
                ModifyZ(newArea2, newValArea2, segment?.Right, val, (s) => {
                    CreateSegment();
                    segment.Right = s;
                });
                if (segment != null) {
                    segment.Value = null;
                }
            }
            else {
                CreateSegment();
                segment.Left = null;
                segment.Right = null;
                segment.Value = val;
            }
        }

        private static long CalcX(Area area, SegmentX segment) {
            if (!area.Validate()) {
                return 0;
            }
            if (segment == null) {
                return 0;
            }
            if (segment.Value != null) {
                return CalcY(area, segment.Value);
            }
            if (area.x1 == area.x2) {
                return 0;
            }
            var midX = area.MiddleX();
            var (newArea1, newArea2) = area.SplitX(midX);
            var ans1 = CalcX(newArea1, segment.Left);
            var ans2 = CalcX(newArea2, segment.Right);
            return ans1 + ans2;
        }

        private static long CalcY(Area area, SegmentY segment) {
            if (!area.Validate()) {
                return 0;
            }
            if (segment == null) {
                return 0;
            }
            if (segment.Value != null) {
                return CalcZ(area, segment.Value);
            }
            if (area.y1 == area.y2) {
                return 0;
            }
            var midY = area.MiddleY();
            var (newArea1, newArea2) = area.SplitY(midY);
            var ans1 = CalcY(newArea1, segment.Left);
            var ans2 = CalcY(newArea2, segment.Right);
            return ans1 + ans2;
        }

        private static long CalcZ(Area area, SegmentZ segment) {
            if (!area.Validate()) {
                return 0;
            }
            if (segment == null) {
                return 0;
            }
            if (segment.Value.HasValue) {
                return segment.Value.Value ? area.Calc() : 0;
            }
            if (area.z1 == area.z2) {
                return 0;
            }
            var midZ = area.MiddleZ();
            var (newArea1, newArea2) = area.SplitZ(midZ);
            var ans1 = CalcZ(newArea1, segment.Left);
            var ans2 = CalcZ(newArea2, segment.Right);
            return ans1 + ans2;
        }

        static int getMin(List<Command> commands, Func<Area, int> func) {
            return commands.Select(a => func(a.area)).Min();
        }

        static int getMax(List<Command> commands, Func<Area, int> func) {
            return commands.Select(a => func(a.area)).Max();
        }

        static void Main(string[] args) {
            var commands = File.ReadLines("input").Select(Command.Parse).ToList();
            // var wholeArea = new Area(
            //     -50,
            //     50, 
            //     -50,
            //     50, 
            //     -50,
            //     50
            // );
            var wholeArea = new Area(
                getMin(commands, a => Math.Min(a.x1, a.x2)),
                getMax(commands, a => Math.Max(a.x1, a.x2)),
                getMin(commands, a => Math.Min(a.y1, a.y2)),
                getMax(commands, a => Math.Max(a.y1, a.y2)),
                getMin(commands, a => Math.Min(a.z1, a.z2)),
                getMax(commands, a => Math.Max(a.z1, a.z2))
            );


            SegmentX first = null;
            
            long ans = 0;
            
            for (int i = 0; i < commands.Count; i++) {
                var command = commands[i];
                ModifyX(wholeArea, command.area, first, command.value, (s) => first = s);
                Console.WriteLine($"Processed {i}/{commands.Count}");
            }
            ans = CalcX(wholeArea, first);
            Console.WriteLine(ans);
        }
    }
}