using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class ASM
    {
        public static readonly ulong O = 0xF,
            w = 0x01_0,
            x = 0x02_0,
            y = 0x04_0,
            z = 0x08_0,
            A = 0xFF_00,
            B = 0xFF_00_00,
            C = 0xFF_00_00_00;
        public static readonly ulong AB = A|B,
            BC      = B|C,
            BOP     = O|w|x,
            WXYZ    = w|x|y|z;
    }
    public struct InstructionCode
    {
        public ulong Bytecode;

        //  BASIC OPERANDS
        public byte Op { get { return (byte)(Bytecode & ASM.O); } } // Unsigned operator

        public byte uA { get { return (byte)((Bytecode & ASM.A) >> 8); } } // Unsigned argument A
        public byte uB { get { return (byte)((Bytecode & ASM.B) >> 16); } } // Unsigned argument B
        public byte uC { get { return (byte)((Bytecode & ASM.C) >> 24); } } // Unsigned argument C

        public sbyte sA { get { return (sbyte)((Bytecode & ASM.A) >> 8); } } // Signed argument A
        public sbyte sB { get { return (sbyte)((Bytecode & ASM.B) >> 16); } } // Signed argument B
        public sbyte sC { get { return (sbyte)((Bytecode & ASM.C) >> 24); } } // Signed argument C

        public bool W { get { return (Bytecode & ASM.w) >> 4 == 1; } } // Bit flag w
        public bool X { get { return (Bytecode & ASM.x) >> 5 == 1; } } // Bit flag x
        public bool Y { get { return (Bytecode & ASM.y) >> 6 == 1; } } // Bit flag y
        public bool Z { get { return (Bytecode & ASM.z) >> 7 == 1; } } // Bit flag z

        // EXTENDED OPERANDS
        public ushort uAB { get { return (ushort)((Bytecode & ASM.AB) >> 8); } } // Unsigned argument AB
        //public ushort uBC { get { return (ushort)((Bytecode & ASM.BC) >> 16); } } // Unsigned argument BC

        public short sAB { get { return (short)((Bytecode & ASM.AB) >> 8); } } // Signed argument AB
        //public short sBC { get { return (short)((Bytecode & ASM.BC) >> 16); } } // Signed argument BC

        public byte BOP { get { return (byte)(Bytecode & ASM.BOP); } } // Extended unsigned operator (big operator)

        public byte WXYZ { get {return (byte)((Bytecode & ASM.WXYZ) >> 4); } } // Unsigned concatentated flags
    }
    public class VirtualMachine
    {
        public HashSet<Parser.Instruction> instructions;
    }
}
