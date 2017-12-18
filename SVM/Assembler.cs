using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace SVM
{
    class Assembler
    {
        Instruction[] instructions;

        public Assembler()
        {
            instructions = Instruction.GetAllInstructions();
        }

        public byte[] Compile(string program)
        {
            byte[] mem = new byte[VM.MEMSIZE];

            var lines = (from l in program.Replace("\t", " ").Split(Environment.NewLine)
                         where !l.StartsWith('#') && !String.IsNullOrWhiteSpace(l)
                         select l.Split('#').First().Trim().ToUpper()).ToArray();

            int i = 0;
            ushort mempos = 0;
            ushort lastpos = 0;
            string line;
            string[] parts;
            Dictionary<string, ushort> markers = new Dictionary<string, ushort>();
            Dictionary<string, List<ushort>> markerUses = new Dictionary<string, List<ushort>>();
            //Encode ops until memory section
            for (; i < lines.Length; i++)
            {
                line = lines[i];
                if (line == "MEMORY")
                {
                    i++;
                    break;
                }

                //Remove multiple spaces
                while (line.IndexOf("  ") != -1)
                {
                    line = line.Replace("  ", " ");
                }

                //Records marker locations
                if (line.StartsWith(':'))
                {
                    var markerParts = line.Split(" ", 2);
                    markers.Add(markerParts[0].Substring(1), mempos);
                    if (markerParts.Length > 1 && !string.IsNullOrWhiteSpace(markerParts[1]))
                    {
                        line = markerParts[1].Trim();
                    }
                    else
                    {
                        continue;
                    }
                }

                parts = line.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries);
                var op = parts[0];
                if (op == "ORIGIN")
                {
                    Debug.Assert(parts.Length == 2);
                    if (parts[1].StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                    {
                        mempos = ushort.Parse(parts[1].Substring(2), System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        mempos = ushort.Parse(parts[1]);
                    }
                    if (mempos > lastpos) lastpos = mempos;
                    continue;
                }

                var instr = instructions.First(x => x.ASM == op);

                Dictionary<string, ushort> markerRefs = new Dictionary<string, ushort>();
                var bytecode = instr.Encode(parts.Length > 1 ? parts[1] : string.Empty, markerRefs);
                if (markerRefs != null && markerRefs.Count > 0)
                {
                    foreach (var mRef in markerRefs)
                    {
                        if (markerUses.ContainsKey(mRef.Key))
                        {
                            markerUses[mRef.Key].Add((ushort)(mempos + mRef.Value));
                        }
                        else
                        {
                            markerUses.Add(mRef.Key, new List<ushort>() { (ushort)(mempos + mRef.Value) });
                        }
                    }
                }
                Array.Copy(bytecode, 0, mem, mempos, bytecode.Length);
                mempos += (ushort)bytecode.Length;
                if (mempos > lastpos) lastpos = mempos;
            }

            foreach(var mUse in markerUses)
            {
                if (!markers.ContainsKey(mUse.Key))
                {
                    throw new Exception(string.Format("Use of undefined marker {0}", mUse.Key));
                }
                foreach(var loc in mUse.Value)
                {
                    mem[loc] = markers[mUse.Key].HiByte();
                    mem[loc+1] = markers[mUse.Key].LoByte();
                }
            }

            for (; i < lines.Length; i++)
            {
                line = lines[i];
                parts = line.Split(" ", 2);

                ushort dataOrigin = 0;
                if (parts[0].StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                {
                    dataOrigin = ushort.Parse(parts[0].Substring(2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    dataOrigin = ushort.Parse(parts[0]);
                }

                byte[] lineData = new byte[0];
                parts[1] = parts[1].Trim();
                if (parts[1].StartsWith('\'') || parts[1].StartsWith('"'))
                {
                    string asciiContent = string.Empty;
                    if (parts[1].StartsWith('\''))
                    {
                        asciiContent = parts[1].Trim('\'');
                    }
                    else if (parts[1].StartsWith('"'))
                    {
                        asciiContent = parts[1].Trim('"');
                    }
                    lineData = Encoding.ASCII.GetBytes(asciiContent);
                    //Zero terminate
                    Array.Resize(ref lineData, lineData.Length + 1);
                    lineData[lineData.Length - 1] = 0;
                }
                else if (parts[1].StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                {
                    var hex = parts[1].Substring(2).Replace(" ", "");
                    if (hex.Length % 2 != 0)
                    {
                        hex = "0" + hex;
                    }
                    lineData = new byte[hex.Length / 2];
                    for (int di = 0, ldi = 0; di < hex.Length; di += 2, ldi++)
                    {
                        lineData[ldi] = byte.Parse(hex.Substring(di, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                }
                Array.Copy(lineData, 0, mem, dataOrigin, lineData.Length);
                if (lastpos < dataOrigin + lineData.Length)
                {
                    lastpos = (ushort)(dataOrigin + lineData.Length);
                }

            }

            Array.Resize<byte>(ref mem, lastpos);
            return mem;
        }
    }
}
