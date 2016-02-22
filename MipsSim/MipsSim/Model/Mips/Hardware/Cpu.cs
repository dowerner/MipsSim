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
using System.Linq;

namespace MipsSim.Model.Mips.Hardware
{
    /// <summary>
    /// Representation of CPU registers
    /// </summary>
    public class Cpu
    {
        // Prepare alert mechanism for register changes
        public delegate void RegisterChangedHandler(Register register);
        public event RegisterChangedHandler RegisterChanged;

        // Prepare runtime exception alerts in case an invalid instruction is encountered
        public delegate void RuntimeExceptionThrownHandler(int line, string message);
        public event RuntimeExceptionThrownHandler RuntimeExceptionThrow;

        #region Registers
        /// <summary>
        /// Register Count
        /// </summary>
        public const int RegisterCount = 32;

        public void ResetRegisters()
        {
            foreach (Register reg in Registers) SetRegData(reg.RegisterName, (0).ToBits().ToLength(32));
        }

        /// <summary>
        /// Registers of the Mips CPU
        /// </summary>
        public Register[] Registers { get; private set; }

        /// <summary>
        /// Program Counter
        /// </summary>
        public Register PC { get; private set; }

        /// <summary>
        /// Register hi for multiplication
        /// </summary>
        public Register Hi { get; private set; }

        /// <summary>
        /// Register lo for multiplication
        /// </summary>
        public Register Lo { get; private set; }

        /// <summary>
        /// Get data from register
        /// </summary>
        /// <param name="reg">reg address</param>
        /// <returns>data</returns>
        public bool[] GetRegValue(string reg)
        {
            bool[] result = null;
            if (reg.Contains("$")) result = Registers.First(item => item.RegisterName == reg.ToLower()).Data;
            if (result == null) throw new Exception(string.Format("Invalid register: {0}", reg));
            return result;
        }

        /// <summary>
        /// Get data from register
        /// </summary>
        /// <param name="reg">reg address</param>
        /// <returns>data</returns>
        public bool[] GetRegValue(bool[] reg)
        {
            uint address = reg.ToUnsignedInt();
            Register result;
            result = Registers.First(item => item.Index == address);
            if (result == null) throw new Exception(string.Format("Invalid register: {0}", reg));
            return result.Data;
        }

        /// <summary>
        /// Sets data to Register
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="data"></param>
        public void SetRegData(bool[] reg, bool[] data)
        {
            uint address = reg.ToUnsignedInt();
            if (address == 0) throw new Exception("Invalid Register access!");  // $zero cannot be overwritten
            Register result;
            result = Registers.First(item => item.Index == address);
            if (result == null) throw new Exception(string.Format("Invalid register: {0}", reg));
            SetRegData(result.RegisterName, data);
        }

        /// <summary>
        /// Sets data to Register
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="data"></param>
        public void SetRegData(string reg, bool[] data)
        {
            if (reg == PC.RegisterName)
            {
                PC.SetRegData(data);
                if (RegisterChanged != null) RegisterChanged(PC);
            }
            else if (reg == Lo.RegisterName)
            {
                Lo.SetRegData(data);
                if (RegisterChanged != null) RegisterChanged(Lo);
            }
            else if (reg == Hi.RegisterName)
            {
                Hi.SetRegData(data);
                if (RegisterChanged != null) RegisterChanged(Hi);
            }
            else
            {
                Register register = Registers.First(item => item.RegisterName == reg);
                register.SetRegData(data);
                if (RegisterChanged != null) RegisterChanged(register);
            }
        }

        /// <summary>
        /// Get data from register
        /// </summary>
        /// <param name="reg">reg address</param>
        /// <returns>data</returns>
        public byte[] GetRegValueBytes(string reg)
        {
            return GetRegValue(reg).ToMipsInstructionBytes();
        }

        /// <summary>
        /// Get binary coded address
        /// </summary>
        /// <param name="reg">reg address</param>
        /// <returns>bit address</returns>
        public bool[] GetBinaryCodedAddress(string reg)
        {
            bool[] result = null;
            if (reg.Contains("$")) result = ((byte)Registers.First(item => item.RegisterName == reg.ToLower()).Index).ToBits();
            if (result == null) throw new Exception(string.Format("Invalid register: {0}", reg));
            return result;
        }

        /// <summary>
        /// Create all the registers
        /// </summary>
        private void FillRegisters()
        {
            Registers[0] = new Register("$zero", 0);
            Registers[1] = new Register("$at", 1);
            Registers[2] = new Register("$v0", 2);
            Registers[3] = new Register("$v1", 3);
            Registers[4] = new Register("$a0", 4);
            Registers[5] = new Register("$a1", 5);
            Registers[6] = new Register("$a2", 6);
            Registers[7] = new Register("$a3", 7);
            Registers[8] = new Register("$t0", 8);
            Registers[9] = new Register("$t1", 9);
            Registers[10] = new Register("$t2", 10);
            Registers[11] = new Register("$t3", 11);
            Registers[12] = new Register("$t4", 12);
            Registers[13] = new Register("$t5", 13);
            Registers[14] = new Register("$t6", 14);
            Registers[15] = new Register("$t7", 15);
            Registers[16] = new Register("$s0", 16);
            Registers[17] = new Register("$s1", 17);
            Registers[18] = new Register("$s2", 18);
            Registers[19] = new Register("$s3", 19);
            Registers[20] = new Register("$s4", 20);
            Registers[21] = new Register("$s5", 21);
            Registers[22] = new Register("$s6", 22);
            Registers[23] = new Register("$s7", 23);
            Registers[24] = new Register("$t8", 24);
            Registers[25] = new Register("$t9", 25);
            Registers[26] = new Register("$k0", 26);
            Registers[27] = new Register("$k1", 27);
            Registers[28] = new Register("$gp", 28);
            Registers[29] = new Register("$sp", 29);
            Registers[30] = new Register("$fp", 30);
            Registers[31] = new Register("$ra", 31);
            Registers[0].SetRegData((0).ToBits());      // set $zero to 0
        }
        #endregion

        // FlipFlops for pipelining
        #region DFlipFlops
        private DFLipFlop IF_ID;
        private DFLipFlop ID_EX;
        private DFLipFlop EX_MEM;
        private DFLipFlop MEM_WB;
        #endregion

        #region Processing
        /// <summary>
        /// Loads next instruction from memory
        /// </summary>
        public void InstructionFetch()
        {
            IF_ID.Input = Memory.Instance.LoadWord(PC.Data.ToUnsignedInt());
            Clock();
        }

        /// <summary>
        /// Decodes the instruction for execution
        /// </summary>
        public void InstructionDecode()
        {
            ID_EX.Input = IF_ID.Output;

            Clock();
        }

        private const int OpCodeStart = 0;
        private const int OpCodeEnd = 5;
        private const int OpCodeLength = 6;

        /// <summary>
        /// Executes instruction. TODO: In the present state instructions get executed straigth forward. A possible extension would be to implement the underlying hardware and remove this method.
        /// </summary>
        public void ExecuteInstruction()
        {
            EX_MEM.Input = ID_EX.Output;

            bool[] opCode = ID_EX.Output.GetRange(OpCodeStart, OpCodeLength);

            InstructionType type;

            if (opCode.ToInt() == 0) type = InstructionType.RType;
            else if (opCode.ToBitString() == AssemblerInstructions.JOpCode
                || opCode.ToBitString() == AssemblerInstructions.JalOpCode) type = InstructionType.JType;
            else type = InstructionType.IType;

            // try to execute next instruction
            try
            {
                switch (type)
                {
                    case InstructionType.RType:
                        InstructionExecute.Instance.ExecuteRType(ID_EX.Output);
                        break;
                    case InstructionType.IType:
                        InstructionExecute.Instance.ExecuteIType(ID_EX.Output);
                        break;
                    case InstructionType.JType:
                        InstructionExecute.Instance.ExecuteJType(ID_EX.Output);
                        break;
                }
            }
            catch (Exception e)
            {
                // throw exception for simulator to handle on invalid instructions
                if(RuntimeExceptionThrow != null) RuntimeExceptionThrow(Debugger.Instance.GetLineFromAddress(PC.Data), e.Message);
            }

            Clock();
        }

        /// <summary>
        /// Write to memory (Currently not used realistically)
        /// </summary>
        public void WriteToMemory()
        {
            MEM_WB.Input = EX_MEM.Output;

            Clock();
        }

        /// <summary>
        /// Write to Registers (Currently not used realistically)
        /// </summary>
        public void WriteBack()
        {

            Clock();
        }

        /// <summary>
        /// Executes next instruction in memory according to PC
        /// (Single clock)
        /// </summary>
        public void ProcessNextInstruction()
        {
            InstructionFetch();
            InstructionDecode();
            ExecuteInstruction();
            WriteToMemory();
            WriteBack();
        }

        /// <summary>
        /// Continues to next clock flank which forwards the input to the output
        /// </summary>
        private void Clock()
        {
            IF_ID.NextClockSlope();
            ID_EX.NextClockSlope();
            EX_MEM.NextClockSlope();
            MEM_WB.NextClockSlope();
        }
        #endregion

        #region Singleton
        private Cpu()
        {
            Registers = new Register[RegisterCount];
            PC = new Register("PC", 0);
            Hi = new Register("hi", 0);
            Lo = new Register("lo", 0);
            IF_ID = new DFLipFlop();
            ID_EX = new DFLipFlop();
            EX_MEM = new DFLipFlop();
            MEM_WB = new DFLipFlop();
            FillRegisters();
        }

        public static Cpu Instance
        {
            get
            {
                if (_instance == null) _instance = new Cpu();
                return _instance;
            }
        }
        private static Cpu _instance;
        #endregion
    }
}
