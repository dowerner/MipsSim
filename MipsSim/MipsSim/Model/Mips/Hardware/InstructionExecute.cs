/*
    Copyright 2016 Dominik Werner

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;

namespace MipsSim.Model.Mips.Hardware
{
    /// <summary>
    /// This class is able to execute all real mips instructions
    /// </summary>
    public class InstructionExecute
    {
        #region constants
        private const int OpCodeStart = 0;
        private const int OpCodeEnd = 5;
        private const int OpCodeLength = 6;

        private const int RsStart = 6;
        private const int RsEnd = 10;
        private const int RsLength = 5;

        private const int RtStart = 11;
        private const int RtEnd = 15;
        private const int RtLength = 5;

        private const int TargetStart = 6;
        private const int TargetEnd = 31;
        private const int TargetLength = 26;

        private const int ImmediateStart = 16;
        private const int ImmediateEnd = 31;
        private const int ImmediateLength = 16;

        private const int RdStart = 16;
        private const int RdEnd = 20;
        private const int RdLength = 5;

        private const int ShamtStart = 21;
        private const int ShamtEnd = 25;
        private const int ShamtLength = 5;

        private const int FunctStart = 26;
        private const int FunctEnd = 31;
        private const int FunctLength = 6;
        #endregion

        /// <summary>
        /// Execute R Type instruction
        /// </summary>
        /// <param name="data">instruction in binary form</param>
        public void ExecuteRType(bool[] data)
        {
            bool[] rs = data.GetRange(RsStart, RsLength);
            bool[] rt = data.GetRange(RtStart, RtLength);
            bool[] rd = data.GetRange(RdStart, RdLength);
            bool[] shamt = data.GetRange(ShamtStart, ShamtLength);
            bool[] funct = data.GetRange(FunctStart, FunctLength);

            string functCode = funct.ToBitString();
            bool[] rsValue = Cpu.Instance.GetRegValue(rs);
            bool[] rtValue = Cpu.Instance.GetRegValue(rt);
            bool[] result = { false };

            bool write = false;
            bool increasePC = false;

            switch (functCode)
            {
                case AssemblerInstructions.AddFunct:
                    result = Extensions.AddBinary(rsValue, rtValue);
                    write = true;
                    increasePC = true;
                    if (!rsValue[0] && !rtValue[0] && result[0]) throw new Exception("arithmetic overflow exception");
                    break;
                case AssemblerInstructions.AdduFunct:
                    result = Extensions.AddBinary(rsValue, rtValue);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.AndFunct:
                    result = rsValue.BitWiseAnd(rtValue);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.DivFunct:
                    Cpu.Instance.SetRegData(Cpu.Instance.Lo.RegisterName, (rsValue.ToInt() / rtValue.ToInt()).ToBits());
                    Cpu.Instance.SetRegData(Cpu.Instance.Hi.RegisterName, (rsValue.ToInt() % rtValue.ToInt()).ToBits());
                    increasePC = true;
                    break;
                case AssemblerInstructions.DivuFunct:
                    Cpu.Instance.SetRegData(Cpu.Instance.Lo.RegisterName, (rsValue.ToInt() / rtValue.ToInt()).ToBits());
                    Cpu.Instance.SetRegData(Cpu.Instance.Hi.RegisterName, (rsValue.ToInt() % rtValue.ToInt()).ToBits());
                    increasePC = true;
                    break;
                case AssemblerInstructions.JalrFunct:
                    result = (Cpu.Instance.PC.Data.ToInt() + 4).ToBits();
                    Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, rsValue);
                    write = true;
                    break;
                case AssemblerInstructions.JrFunct:
                    Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, rsValue);
                    break;
                case AssemblerInstructions.MfhiFunct:
                    result = Cpu.Instance.Hi.Data;
                    increasePC = true;
                    write = true;
                    break;
                case AssemblerInstructions.MfloFunct:
                    result = Cpu.Instance.Lo.Data;
                    increasePC = true;
                    write = true;
                    break;
                case AssemblerInstructions.MthiFunct:
                    Cpu.Instance.SetRegData(Cpu.Instance.Hi.RegisterName, Cpu.Instance.GetRegValue(rd));
                    increasePC = true;
                    break;
                case AssemblerInstructions.MtloFunct:
                    Cpu.Instance.SetRegData(Cpu.Instance.Lo.RegisterName, Cpu.Instance.GetRegValue(rd));
                    increasePC = true;
                    break;
                case AssemblerInstructions.MultFunct:
                    int a = rsValue.ToInt();
                    int b = rtValue.ToInt();
                    bool[] product = (rsValue.ToInt() * rtValue.ToInt()).ToBits().ToLength(64);
                    int p = product.ToInt();
                    Cpu.Instance.SetRegData(Cpu.Instance.Hi.RegisterName, product.GetRange(0, 32));
                    Cpu.Instance.SetRegData(Cpu.Instance.Lo.RegisterName, product.GetRange(32, 32));
                    increasePC = true;
                    break;
                case AssemblerInstructions.MultuFunct:
                    bool[] productu = (rsValue.ToInt() * rtValue.ToInt()).ToBits().ToLength(64);
                    Cpu.Instance.SetRegData(Cpu.Instance.Hi.RegisterName, productu.GetRange(0, 32));
                    Cpu.Instance.SetRegData(Cpu.Instance.Lo.RegisterName, productu.GetRange(32, 32));
                    increasePC = true;
                    break;
                case AssemblerInstructions.NorFunct:
                    result = rsValue.BitWiseOr(rtValue).BitWiseNegate();
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.OrFunct:
                    result = rsValue.BitWiseOr(rtValue);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SllFunct:
                    result = rtValue.ShiftLeft(shamt.ToInt());
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SllvFunct:
                    result = rtValue.ShiftLeft(rsValue.ToInt());
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SltFunct:
                    result = (rsValue.ToInt() < rtValue.ToInt()) ? Extensions.GetBitsBitString("01").ToLength(32) : (0).ToBits().ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SltuFunct:
                    result = (rsValue.ToInt() < rtValue.ToInt()) ? Extensions.GetBitsBitString("01").ToLength(32) : (0).ToBits().ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SraFunct:
                    result = rtValue.ShiftRight(getSraShamtTerm(shamt.ToInt(), rtValue));
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SravFunct:
                    result = rtValue.ShiftRight(getSraShamtTerm(rsValue.ToInt(), rtValue));
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SrlFunct:
                    result = rtValue.ShiftRight(shamt.ToInt());
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SrlvFunct:
                    result = rtValue.ShiftRight(rsValue.ToInt());
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SubFunct:
                    result = Extensions.SubstractBinary(rsValue, rtValue);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SubuFunct:
                    result = Extensions.SubstractBinary(rsValue, rtValue);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.XorFunct:
                    result = rsValue.BitWiseXor(rtValue);
                    write = true;
                    increasePC = true;
                    break;
            }
            if(write) Cpu.Instance.SetRegData(rd, result.ToLength(32));
            if (increasePC) Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + 4).ToBits());
        }

        /// <summary>
        /// Execute I Type Instruction
        /// </summary>
        /// <param name="data">instruction in binary format</param>
        public void ExecuteIType(bool[] data)
        {
            bool[] opCode = data.GetRange(OpCodeStart, OpCodeLength);
            bool[] rs = data.GetRange(RsStart, RsLength);
            bool[] rt = data.GetRange(RtStart, RtLength);
            bool[] immidiate = data.GetRange(ImmediateStart, ImmediateLength);

            bool[] rsValue = Cpu.Instance.GetRegValue(rs);
            bool[] result = { false };

            bool write = false;
            bool increasePC = false;

            string opCodeString = opCode.ToBitString();

            switch (opCodeString)
            {
                case AssemblerInstructions.AddiOpCode:
                    result = Extensions.AddBinary(rsValue, immidiate);
                    write = true;
                    increasePC = true;
                    if (!rsValue[0] && !immidiate[0] && result[0]) throw new Exception("arithmetic overflow exception");
                    break;
                case AssemblerInstructions.AddiuOpCode:
                    result = Extensions.AddBinary(rsValue, immidiate);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.AndiOpCode:
                    result = rsValue.BitWiseAnd((0).ToBits().JoinArray(immidiate).ToLength(32));
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.BeqOpCode:
                    if (rsValue.ToInt() == Cpu.Instance.GetRegValue(rt).ToInt())
                        Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + immidiate.ToInt() * 4).ToBits());
                    else increasePC = true;
                    break;
                case AssemblerInstructions.BgezOpCode:
                    if (rt.ToInt() == 1)    // bgez
                    {
                        if(rsValue.ToInt() >= 0)
                            Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + immidiate.ToInt() * 4).ToBits());
                        else increasePC = true;
                    }
                    else    // bltz
                    {
                        if (rsValue.ToInt() < 0)
                            Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + immidiate.ToInt() * 4).ToBits());
                        else increasePC = true;
                    }
                    break;
                case AssemblerInstructions.BgtzOpCode:
                    if (rsValue.ToInt() > 0)
                        Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + immidiate.ToInt() * 4).ToBits());
                    else increasePC = true;
                    break;
                case AssemblerInstructions.BlezOpCode:
                    if (rsValue.ToInt() <= 0)
                        Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + immidiate.ToInt() * 4).ToBits());
                    else increasePC = true;
                    break;
                case AssemblerInstructions.BneOpCode:
                    if (rsValue.ToInt() != Cpu.Instance.GetRegValue(rt).ToInt())
                        Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + immidiate.ToInt() * 4).ToBits());
                    else increasePC = true;
                    break;
                case AssemblerInstructions.LbOpCode:
                    result = Memory.Instance.Data[rsValue.ToUnsignedInt() + immidiate.ToInt()].ToBits().ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.LbuOpCode:
                    result = Memory.Instance.Data[rsValue.ToUnsignedInt() + immidiate.ToInt()].ToBits().ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.LhOpCode:
                    bool[] first = Memory.Instance.Data[rsValue.ToUnsignedInt() + immidiate.ToInt()].ToBits();
                    bool[] second = Memory.Instance.Data[rsValue.ToUnsignedInt() + 4 + immidiate.ToInt()].ToBits();
                    result = first.JoinArray(second).ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.LhuOpCode:
                    bool[] firstu = Memory.Instance.Data[rsValue.ToUnsignedInt() + immidiate.ToInt()].ToBits();
                    bool[] secondu = Memory.Instance.Data[rsValue.ToUnsignedInt() + 4 + immidiate.ToInt()].ToBits();
                    result = firstu.JoinArray(secondu).ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.LuiOpCode:
                    result = immidiate.ShiftLeft(16);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.LwOpCode:
                    int offset = immidiate.ToInt();
                    uint address = (offset >= 0) ? rsValue.ToUnsignedInt() + (uint)offset : rsValue.ToUnsignedInt() - (uint)offset;
                    //result = reorderForSavingLoadingToMemory(Memory.Instance.LoadWord(address));
                    result = Memory.Instance.LoadWord(address);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.OriOpCode:
                    result = rsValue.BitWiseOr((0).ToBits().JoinArray(immidiate).ToLength(32));
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SbOpCode:
                    offset = immidiate.ToInt();
                    address = (offset >= 0) ? rsValue.ToUnsignedInt() + (uint)offset : rsValue.ToUnsignedInt() - (uint)offset;
                    //Memory.Instance.SaveWord(address, reorderForSavingLoadingToMemory((0).ToBits().JoinArray(Cpu.Instance.GetRegValue(rt).GetRange(24, 8)).ToLength(32)));
                    Memory.Instance.SaveWord(address, (0).ToBits().JoinArray(Cpu.Instance.GetRegValue(rt).GetRange(24, 8)).ToLength(32));
                    increasePC = true;
                    break;
                case AssemblerInstructions.SltiOpCode:
                    result = (rsValue.ToInt() < immidiate.ToInt()) ? Extensions.GetBitsBitString("01").ToLength(32) : (0).ToBits().ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.SltiuOpCode:
                    result = (rsValue.ToInt() < immidiate.ToInt()) ? Extensions.GetBitsBitString("01").ToLength(32) : (0).ToBits().ToLength(32);
                    write = true;
                    increasePC = true;
                    break;
                case AssemblerInstructions.ShOpCode:
                    offset = immidiate.ToInt();
                    address = (offset >= 0) ? rsValue.ToUnsignedInt() + (uint)offset : rsValue.ToUnsignedInt() - (uint)offset;
                    //Memory.Instance.SaveWord(address, reorderForSavingLoadingToMemory((0).ToBits().JoinArray(Cpu.Instance.GetRegValue(rt).GetRange(16, 16)).ToLength(32)));
                    Memory.Instance.SaveWord(address, (0).ToBits().JoinArray(Cpu.Instance.GetRegValue(rt).GetRange(16, 16)).ToLength(32));
                    increasePC = true;
                    break;
                case AssemblerInstructions.SwOpCode:
                    offset = immidiate.ToInt();
                    address = (offset >= 0) ? rsValue.ToUnsignedInt() + (uint)offset : rsValue.ToUnsignedInt() - (uint)offset;
                    //Memory.Instance.SaveWord(address, reorderForSavingLoadingToMemory(Cpu.Instance.GetRegValue(rt)));
                    Memory.Instance.SaveWord(address, Cpu.Instance.GetRegValue(rt));
                    increasePC = true;
                    break;
                case AssemblerInstructions.XoriOpCode:
                    result = rsValue.BitWiseXor((0).ToBits().JoinArray(immidiate).ToLength(32));
                    write = true;
                    increasePC = true;
                    break;
            }
            if (write) Cpu.Instance.SetRegData(rt, result.ToLength(32));
            if (increasePC) Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, (Cpu.Instance.PC.Data.ToInt() + 4).ToBits());
        }

        /// <summary>
        /// (NOT USED ANYMORE)
        /// Reorder 32-Bit number in order to correctly save it to the memory.
        /// This is nessesary because otherwise the byte order would be wrong
        /// and the save method cannot be changed because it is used by the compiler
        /// </summary>
        /// <param name="data"></param>
        /// <returns>0xFF 0x1A 0xB2 0x7F => 0x7F 0xB2 0x1A  0xFF</returns>
        private bool[] reorderForSavingLoadingToMemory(bool[] data)
        {
            byte[] temp = data.ToMipsInstructionBytes();
            byte[] newTemp = new byte[4];
            newTemp[0] = temp[3];
            newTemp[1] = temp[2];
            newTemp[2] = temp[1];
            newTemp[3] = temp[0];
            return newTemp.ToMipsInstructionBits();
        }

        /// <summary>
        /// Execute J Type Instruction
        /// </summary>
        /// <param name="data">instruction in binary format</param>
        public void ExecuteJType(bool[] data)
        {
            bool[] target = data.GetRange(TargetStart, TargetLength);

            bool[] opCode = data.GetRange(OpCodeStart, OpCodeLength);

            string opCodeString = opCode.ToBitString();

            bool[] suffix = { false, false };

            bool[] jumpDestination;

            switch (opCodeString)
            {
                case AssemblerInstructions.JOpCode:
                    jumpDestination = Cpu.Instance.PC.Data.GetRange(0, 4).JoinArray(target).JoinArray(suffix);
                    Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, jumpDestination);
                    break;
                case AssemblerInstructions.JalOpCode:
                    bool[] ra = Extensions.AddBinary(Cpu.Instance.PC.Data, (4).ToBits());
                    Cpu.Instance.SetRegData("$ra", ra);
                    jumpDestination = Cpu.Instance.PC.Data.GetRange(0, 4).JoinArray(target).JoinArray(suffix);
                    Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, jumpDestination);
                    break;
            }
        }

        /// <summary>
        /// Get shitf amount for shift right instruction
        /// </summary>
        /// <param name="shamt">shamt data</param>
        /// <param name="rtValue">rt data</param>
        /// <returns></returns>
        private int getSraShamtTerm(int shamt, bool[] rtValue)
        {
            int result = 0;

            for (int i = 1; i <= shamt; i++) result += (int)Math.Pow(2, 32 - i);

            result *= rtValue.ShiftRight(31).ToInt();

            result += shamt;

            return result;
        }

        #region Singleton
        private InstructionExecute()
        {
        }

        public static InstructionExecute Instance
        {
            get
            {
                if (_instance == null) _instance = new InstructionExecute();
                return _instance;
            }
        }
        private static InstructionExecute _instance;
        #endregion
    }
}
