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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MipsSim.Model.Mips.Hardware;

namespace MipsSim.Model.Mips
{
    /// <summary>
    /// This class provides methods to convert the code into binary format and save it to the virtual memory
    /// </summary>
    public class Compiler
    {
        #region constants
        public const int InstructionLength = 32;

        public const int OpCodeStart = 31;
        public const int OpCodeEnd = 26;
        public const int OpCodeLength = 6;

        public const int RsStart = 25;
        public const int RsEnd = 21;
        public const int RsLength = 5;

        public const int RtStart = 20;
        public const int RtEnd = 16;
        public const int RtLength = 5;

        public const int TargetStart = 25;
        public const int TargetEnd = 0;
        public const int TargetLength = 26;

        public const int ImmediateStart = 15;
        public const int ImmediateEnd = 0;
        public const int ImmidiateLength = 16;

        public const int RdStart = 15;
        public const int RdEnd = 11;
        public const int RdLength = 5;

        public const int ShamtStart = 10;
        public const int ShamtEnd = 6;
        public const int ShamtLength = 5;

        public const int FunctStart = 5;
        public const int FunctEnd = 0;
        public const int FunctLength = 6;
        #endregion

        public delegate void CompileErrorFoundHandler(int lineIndex, string message);
        public event CompileErrorFoundHandler CompileErrorFound;

        public delegate void CompilerCompletedHandler(int lineCount);
        public event CompilerCompletedHandler CompilerCompleted;

        public bool ResetMemoryOnCompile { get; set; }
        public bool ResetRegisterOnCompile { get; set; }

        /// <summary>
        /// Compile the code or notify if errors occur
        /// </summary>
        /// <param name="code">code to compile</param>
        public void CompileCode(string code)
        {
            Debugger.Instance.Mode = ProgramMode.Compiling; // set current debug mode to compiling
            Debugger.Instance.StartNew();   // clear debugger

            if (code == null)   // check if code editor is empty
            {
                CompileErrorFound(0, "No code.");
                Debugger.Instance.Mode = ProgramMode.Stopped;
                return;
            }

            if (ResetMemoryOnCompile) Memory.Instance.Reset();   // nullify memory if option enabled
            if (ResetRegisterOnCompile) Cpu.Instance.ResetRegisters();   // nullify registers if option enabled

            string[] rawlines = getLines(code);
            string[] lines = rawlines.RemoveEmpties().RemoveUncommented();
            int[] editorLineNumbers = new int[lines.Length];

            int j = 0;
            for (int editorIndex = 0; editorIndex < rawlines.Length; editorIndex++) // keep track of editor line numbers in seperate array
            {
                if (!isEmptyLine(rawlines[editorIndex]) && !isUncommented(rawlines[editorIndex]))
                {
                    editorLineNumbers[j] = editorIndex;
                    j++;
                }
            }

            List<JumpMark> jumpMarks = new List<JumpMark>();

            // Setup stackpointer and framepointer. TODO: determine if this choice is realistic
            if (ResetRegisterOnCompile) Cpu.Instance.SetRegData("$fp", Extensions.AddBinary(Memory.Instance.GetStackPointerAddress(), (4).ToBits()));
            if (ResetRegisterOnCompile) Cpu.Instance.SetRegData("$sp", Memory.Instance.GetStackPointerAddress());

            // Get location to save compiled code to.
            bool[] programStart = (ResetRegisterOnCompile) ? Memory.Instance.GetProgramStartingAddress(lines.Length) : Cpu.Instance.PC.Data;
            uint memoryStartIndex = programStart.ToUnsignedInt();

            // Save starting address to program counter
            Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, programStart);
            Debugger.Instance.InitiaclPC = programStart;

            // calculate jump marks and save them to list
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(":"))
                {
                    string[] jumpParts = lines[i].Split(':');
                    string jumpMark = jumpParts[0].Trim();
                    jumpMarks.Add(new JumpMark(jumpMark, programStart, i));
                }
            }

            // compile all instructions
            for (uint i = 0; i < lines.Length; i++)
            {
                int editorLine = editorLineNumbers[i];
                if (lines[i].Contains(":")) // extract instruction behind jump mark
                {
                    string[] jumpParts = lines[i].Split(':');
                    lines[i] = jumpParts[1];
                }

                try
                {
                    uint memoryIndex = memoryStartIndex + i * 4;    // get next address
                    bool[] instruction = CompileInstruction(lines[i], jumpMarks, memoryIndex);
                    Debugger.Instance.AddRelationship(editorLine, memoryIndex.ToBits());    // keep track of editor code
                    Memory.Instance.WriteInstructionToMemory(memoryIndex, instruction);
                }
                catch (Exception e)
                {
                    Debugger.Instance.Mode = ProgramMode.Stopped;
                    if (CompileErrorFound != null) CompileErrorFound(editorLine, e.Message);
                    return;
                }
            }
            if (CompilerCompleted != null) CompilerCompleted(lines.Length);
            Debugger.Instance.Mode = ProgramMode.Running;
        }

        private bool isUncommented(string line)
        {
            line = line.Trim();
            return line.Substring(0, 2) == "//";
        }

        private bool isEmptyLine(string line)
        {
            line = line.ToLower();
            line = Regex.Replace(line, "\t", "");
            line = Regex.Replace(line, "\n", "");
            line = Regex.Replace(line, "\r", "");
            return line.Trim() == string.Empty;
        }

        private string[] getInstParts(string instruction)
        {
            if (instruction.Contains("//"))
            {
                instruction = Regex.Split(instruction, "//")[0];
            }

            instruction = instruction.Trim();
            if (!instruction.Contains(' ')) throw new Exception(string.Format("Unable To compile: {0}", instruction));

            string[] parts = instruction.Split(' ');

            if (parts.Length > 2)
            {
                for (int i = 2; i < parts.Length; i++) parts[1] += parts[i];
                string[] newParts = new string[2];
                newParts[0] = parts[0];
                newParts[1] = parts[1];
                parts = newParts;
            }
            return parts;
        }

        /// <summary>
        /// Compile mips code to binary
        /// </summary>
        /// <param name="instruction">mips code</param>
        /// <param name="jumpMarks">jump mark list</param>
        /// <param name="instructionAddress">address to save instruction to</param>
        /// <returns></returns>
        private bool[] CompileInstruction(string instruction, List<JumpMark> jumpMarks, uint instructionAddress)
        {
            bool[] result = new bool[InstructionLength];

            if(instruction.Trim() == string.Empty) return result;

            string[] instParts = getInstParts(instruction);

            InstructionType type = instParts[0].ToMipsType();
            if (type == InstructionType.INVALID) throw new Exception("Invalid Mips Instruction.");
            
            bool[] opCode = instParts[0].ToMipsOpCode();

            string[] parts = instParts[1].Split(',');

            bool[] rs, rt, rd, shamt, funct, target, immediate;
            
            switch (type)
            {
                case InstructionType.RType:
                    funct = instParts[0].ToMipsRFunct();
                    if (parts.Length > 2)
                    {
                        rd = Cpu.Instance.GetBinaryCodedAddress(parts[0]).ToLength(RdLength);

                        if (instParts[0].ToLower() == AssemblerInstructions.Sll
                            || instParts[0].ToLower() == AssemblerInstructions.Srl)
                        {
                            rs = (0).ToBits().ToLength(RsLength);
                            rt = Cpu.Instance.GetBinaryCodedAddress(parts[1]).ToLength(RtLength);
                            int amount = -1;
                            if(!int.TryParse(parts[2], out amount)) throw new Exception(string.Format("{0} is not a valid number.", parts[2]));
                            shamt = amount.ToBits().ToLength(ShamtLength);
                        }
                        else
                        {
                            rs = Cpu.Instance.GetBinaryCodedAddress(parts[1]).ToLength(RsLength);
                            rt = getRtOrRS(parts[2]);
                            shamt = getShamt(parts[2]);
                        }
                    }
                    else if (instParts[0].ToLower() == AssemblerInstructions.Mult
                        || instParts[0].ToLower() == AssemblerInstructions.Multu
                        || instParts[0].ToLower() == AssemblerInstructions.Div
                        || instParts[0].ToLower() == AssemblerInstructions.Divu)
                    {
                        rs = Cpu.Instance.GetBinaryCodedAddress(parts[0]).ToLength(RsLength);
                        rt = Cpu.Instance.GetBinaryCodedAddress(parts[1]).ToLength(RtLength);
                        rd = (0).ToBits().ToLength(RdLength);
                        shamt = (0).ToBits().ToLength(ShamtLength);
                    }
                    else if (instParts[0].ToLower() == AssemblerInstructions.Mfhi
                    || instParts[0].ToLower() == AssemblerInstructions.Mflo
                    || instParts[0].ToLower() == AssemblerInstructions.Mthi
                    || instParts[0].ToLower() == AssemblerInstructions.Mtlo)
                    {
                        rs = (0).ToBits().ToLength(RsLength);
                        rt = (0).ToBits().ToLength(RtLength);
                        rd = Cpu.Instance.GetBinaryCodedAddress(parts[0]).ToLength(RdLength);
                        shamt = (0).ToBits().ToLength(ShamtLength);
                    }
                    else
                    {
                        rs = Cpu.Instance.GetBinaryCodedAddress(parts[0]).ToLength(RsLength);
                        rt = (0).ToBits().ToLength(RtLength);
                        rd = (0).ToBits().ToLength(RdLength);
                        shamt = (0).ToBits().ToLength(ShamtLength);
                    }
                    result = opCode.JoinArray(rs, rt, rd, shamt, funct);
                    break;
                case InstructionType.IType:
                    if (instParts[0].ToLower() == AssemblerInstructions.Bgez
                    || instParts[0].ToLower() == AssemblerInstructions.Bgtz
                    || instParts[0].ToLower() == AssemblerInstructions.Blez
                    || instParts[0].ToLower() == AssemblerInstructions.Bltz)
                    {
                        rs = getRtOrRS(parts[0]);
                        rt = (instParts[0].ToLower() == AssemblerInstructions.Bgez) ? Extensions.GetBitsBitString("01").ToLength(RtLength) : (0).ToBits().ToLength(RtLength);
                        immediate = getBranchImmidiate(parts[1], jumpMarks, instructionAddress);
                    }
                    else
                    {
                        rs = getRtOrRS(parts[1]);
                        rt = Cpu.Instance.GetBinaryCodedAddress(parts[0]).ToLength(RtLength);
                        if (instParts[0].ToLower() == AssemblerInstructions.Beq
                        || instParts[0].ToLower() == AssemblerInstructions.Bne)
                        {
                            immediate = getBranchImmidiate(parts[2], jumpMarks, instructionAddress);
                        }
                        else immediate = (parts.Length > 2) ? getImmediate(parts[2]) : getOffset(parts[1]);
                    }
                    result = opCode.JoinArray(rs, rt, immediate);
                    break;
                case InstructionType.JType:
                    target = getTarget(instParts[1], jumpMarks);
                    result = opCode.JoinArray(target);
                    break;
            };
            return result;
        }

        /// <summary>
        /// Get address to branch to
        /// </summary>
        /// <param name="data">jumpmark name</param>
        /// <param name="jumpMarks">jumpmark list</param>
        /// <param name="instructionAddress">address of instruction</param>
        /// <returns></returns>
        private bool[] getBranchImmidiate(string data, List<JumpMark> jumpMarks, uint instructionAddress)
        {
            uint jumpAddress = jumpMarks.First(item => item.Name == data).Address.ToUnsignedInt();
            return (((int)(jumpAddress - instructionAddress) / 4)).ToBits().ToLength(16);
        }

        /// <summary>
        /// Get address for jump
        /// </summary>
        /// <param name="data">name of jumpmark</param>
        /// <param name="jumpMarks">list of jumpmarks</param>
        /// <returns></returns>
        private bool[] getTarget(string data, List<JumpMark> jumpMarks)
        {
            return jumpMarks.First(item => item.Name == data).Address.GetRange(4, 26);
        }

        /// <summary>
        /// Get offset for lw and sw instructions
        /// </summary>
        /// <param name="data">code</param>
        /// <returns></returns>
        private bool[] getOffset(string data)
        {
            if (data.Contains("(") && data.Contains(")"))
            {
                data = data.Split('(')[0];
                int number;
                if (int.TryParse(data, out number)) return number.ToBits().ToLength(ImmidiateLength);
                else throw new Exception(string.Format("{0} is not a valid number.", data));
            }
            else
            {
                throw new Exception("Missing ()");
            }
        }

        /// <summary>
        /// Get immidiate part of I type instructions
        /// </summary>
        /// <param name="data">immidiate part in code</param>
        /// <returns></returns>
        private bool[] getImmediate(string data)
        {
            int number;
            if (int.TryParse(data, out number)) return number.ToBits().ToLength(ImmidiateLength);
            else throw new Exception(string.Format("{0} is not a valid number.", data));
        }

        /// <summary>
        /// Get RT, RS values (for instance address for lw, sw)
        /// </summary>
        /// <param name="data">rt,rs code</param>
        /// <returns></returns>
        private bool[] getRtOrRS(string data)
        {
            if (data.Contains("(") && data.Contains(")"))
            {
                data = data.Split('(')[1];
                return Cpu.Instance.GetBinaryCodedAddress(data.Substring(0, data.Length-1)).ToLength(RtLength);
            }
            else
            {
                return Cpu.Instance.GetBinaryCodedAddress(data).ToLength(RtLength);
            }
        }

        /// <summary>
        /// Get shamt part of instruction
        /// </summary>
        /// <param name="data">shamt code</param>
        /// <returns></returns>
        private bool[] getShamt(string data)
        {
            if (data.Contains("(") && data.Contains(")"))
            {
                return Cpu.Instance.GetRegValue(Regex.Replace(Regex.Replace(data, "(", ""), ")", "")).ToLength(ShamtLength);
            }
            else
            {
                return ((byte)0).ToBits().ToLength(ShamtLength);
            }
        }

        /// <summary>
        /// Extract all spaces and tabs from the code
        /// </summary>
        /// <param name="value">code</param>
        /// <returns></returns>
        private string prepareString(string value)
        {
            value = value.ToLower();
            value = Regex.Replace(value, "\t", "");
            value = Regex.Replace(value, "\n", "");
            value = Regex.Replace(value, "\r", "");
            return value;
        }

        /// <summary>
        /// Split instruction into parts
        /// </summary>
        /// <param name="value">instruction code</param>
        /// <returns></returns>
        private string[] getInstructionParts(string value)
        {
            return value.Split(',');
        }

        /// <summary>
        /// Get lines from whole code
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        private string[] getLines(string code)
        {
            if (code.Contains('\n')) return code.Split('\n');
            else
            {
                string[] single = new string[1];
                single[0] = code;
                return single;
            }
        }

        #region Singleton
        private Compiler()
        {
            ResetMemoryOnCompile = true;
            ResetRegisterOnCompile = true;
        }

        public static Compiler Instance
        {
            get
            {
                if (_instance == null) _instance = new Compiler();
                return _instance;
            }
        }
        private static Compiler _instance;
        #endregion
    }
}
