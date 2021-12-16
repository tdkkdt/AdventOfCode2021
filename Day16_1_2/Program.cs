using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16_1 {
    static class Parser {
        static int Bin2Int(string binary, ref int p, int l) {
            int ans = 0;
            while (l-- > 0) {
                ans <<= 1;                
                ans += binary[p++] - '0';
            }
            return ans;
        }

        public static Packet Parse(string binary, ref int p) {
            int version = Bin2Int(binary, ref p, 3);
            int typeId = Bin2Int(binary, ref p, 3);
            switch (typeId) {
                case 4:
                    long v = ParseValue(binary, ref p);
                    return new ValuePacket(version, typeId, v);
                default:
                    var operatorPacket = new OperatorPacket(version, typeId);
                    int lengthTypeId = binary[p++] - '0';
                    switch (lengthTypeId) {
                        case 0:
                            int totalLength = Bin2Int(binary, ref p, 15);
                            var inner = binary.Substring(p, totalLength);
                            int innerP = 0;
                            while (innerP < totalLength) {
                                operatorPacket.Inner.Add(Parse(inner, ref innerP));
                            }
                            p += totalLength;
                            break;
                        case 1:
                            int innerCount = Bin2Int(binary, ref p, 11);
                            while (innerCount-- > 0) {
                                operatorPacket.Inner.Add(Parse(binary, ref p));
                            }
                            break;
                        default:
                            throw new Exception();
                    }
                    return operatorPacket;
            }
        }

        private static long ParseValue(string binary, ref int p) {
            long ans = 0;
            int pp;
            do {
                pp = p;
                p++;
                var octet = Bin2Int(binary, ref p, 4);
                ans <<= 4;
                ans += octet;
            } while (binary[pp] == '1');
            return ans;
        }
    }

    abstract class Packet {
        public int Version { get; }
        public int TypeID { get; }
        public abstract long Value { get; }

        public Packet(int version, int typeID) {
            Version = version;
            TypeID = typeID;
        }

        public abstract IEnumerable<Packet> Flatten();
    }

    class ValuePacket : Packet {
        public override long Value { get; }

        public ValuePacket(int version, int typeId, long value) : base(version, typeId) {
            Value = value;
        }

        public override IEnumerable<Packet> Flatten() {
            yield return this;
        }
    }

    class OperatorPacket : Packet {
        public List<Packet> Inner { get; }

        public override long Value {
            get {
                var innerValues = Inner.Select(i => i.Value).ToList();
                return TypeID switch {
                    0 => innerValues.Sum(),
                    1 => innerValues.Aggregate(1L, (a, b) => a * b),
                    2 => innerValues.Min(),
                    3 => innerValues.Max(),
                    5 => innerValues[0] > innerValues[1] ? 1 : 0,
                    6 => innerValues[0] < innerValues[1] ? 1 : 0,
                    7 => innerValues[0] == innerValues[1] ? 1 : 0,
                    _ => throw new Exception()
                };
            }
        }

        public OperatorPacket(int version, int typeID) : base(version, typeID) {
            Inner = new List<Packet>();
        }

        public override IEnumerable<Packet> Flatten() {
            yield return this;
            foreach (Packet packet in Inner) {
                foreach (var packet1 in packet.Flatten()) {
                    yield return packet1;
                }
            }
        }
    }
    
    class Program {
        static void Main(string[] args) {
            var map = new Dictionary<char, string> {
                { '0', "0000" },
                { '1', "0001" },
                { '2', "0010" },
                { '3', "0011" },
                { '4', "0100" },
                { '5', "0101" },
                { '6', "0110" },
                { '7', "0111" },
                { '8', "1000" },
                { '9', "1001" },
                { 'A', "1010" },
                { 'B', "1011" },
                { 'C', "1100" },
                { 'D', "1101" },
                { 'E', "1110" },
                { 'F', "1111" }
            };
            foreach (var line in File.ReadLines("input")) {
                var binary = "";
                foreach (char c in line) {
                    binary += map[c];
                }
                int p = 0;
                var packet = Parser.Parse(binary, ref p);
                // Console.WriteLine(packet.Flatten().Select(p => p.Version).Sum());
                Console.WriteLine(packet.Value);
            }
        }
    }
}