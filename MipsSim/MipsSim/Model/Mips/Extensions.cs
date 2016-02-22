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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MipsSim.Model.Mips
{
    /// <summary>
    /// This class is an extension on existing datatypes used primarily for conversion purposes
    /// </summary>
    public static class Extensions
    {
        public static readonly int InstructionLength = 32;

        /// <summary>
        /// Covert byte to 8-bit-array
        /// </summary>
        /// <param name="sourceByte">byte to convert</param>
        /// <returns>8-bit-array</returns>
        public static bool[] ToBits(this byte sourceByte)
        {
            string byteString = sourceByte.ToString("X2");
            bool[] bits = new bool[8];

            int i = 0;

            foreach (char hex in byteString)
            {
                string bitsToAddString = "0000";
                switch(hex)
                {
                    case '0':
                        bitsToAddString = "0000";
                        break;
                    case '1':
                        bitsToAddString = "0001";
                        break;
                    case '2':
                        bitsToAddString = "0010";
                        break;
                    case '3':
                        bitsToAddString = "0011";
                        break;
                    case '4':
                        bitsToAddString = "0100";
                        break;
                    case '5':
                        bitsToAddString = "0101";
                        break;
                    case '6':
                        bitsToAddString = "0110";
                        break;
                    case '7':
                        bitsToAddString = "0111";
                        break;
                    case '8':
                        bitsToAddString = "1000";
                        break;
                    case '9':
                        bitsToAddString = "1001";
                        break;
                    case 'A':
                        bitsToAddString = "1010";
                        break;
                    case 'B':
                        bitsToAddString = "1011";
                        break;
                    case 'C':
                        bitsToAddString = "1100";
                        break;
                    case 'D':
                        bitsToAddString = "1101";
                        break;
                    case 'E':
                        bitsToAddString = "1110";
                        break;
                    case 'F':
                        bitsToAddString = "1111";
                        break;
                }

                bool[] bitsToAdd = GetBitsBitString(bitsToAddString);

                for (int j = i; j < i + 4; j++) bits[j] = bitsToAdd[j-i];
                i += 4;
            }
            return bits;
        }

        /// <summary>
        /// Convert byte-Mips-instruction to bit-Mips-instruction
        /// </summary>
        /// <param name="instruction">4-byte-Mips-instruction</param>
        /// <returns>bits-Mips-instruction</returns>
        public static bool[] ToMipsInstructionBits(this byte[] instruction)
        {
            bool[] bits = new bool[InstructionLength];
            for (int i = 0; i < instruction.Length; i++ )
            {
                bool[] bitsToAdd = instruction[i].ToBits();

                for (int j = i*8; j < i*8+8; j++) bits[j] = bitsToAdd[j - i*8];
            }
            return bits;
        }

        /// <summary>
        /// Convert 32-bit instruction to 4-byte instruction
        /// </summary>
        /// <param name="bits">32-bit</param>
        /// <returns>4-byte instruction</returns>
        public static byte[] ToMipsInstructionBytes(this bool[] bits)
        {
            byte[] result = new byte[InstructionLength / 8];

            for (int i = bits.Length - 1; i >= 0; i -= 8)
            {
                bool[] byteBits = new bool[8];
                for (int j = i; j > i - 8; j--)
                {
                    byteBits[j - (i - 8) - 1] = bits[j];
                }
                result[i / 8] = byteBits.ToByte();
            }

            return result;
        }

        /// <summary>
        /// Make a bitwise and comparison
        /// </summary>
        /// <param name="source"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool[] BitWiseAnd(this bool[] source, bool[] compare)
        {
            bool[] result = new bool[source.Length];
            compare = compare.ToLength(source.Length);

            for (int i = 0; i < source.Length; i++) result[i] = source[i] & compare[i];

            return result;
        }

        /// <summary>
        /// Make a bitwise or comparison
        /// </summary>
        /// <param name="source"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool[] BitWiseOr(this bool[] source, bool[] compare)
        {
            bool[] result = new bool[source.Length];
            compare = compare.ToLength(source.Length);

            for (int i = 0; i < source.Length; i++) result[i] = source[i] | compare[i];

            return result;
        }

        /// <summary>
        /// Bitwise negate data
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool[] BitWiseNegate(this bool[] source)
        {
            for (int i = 0; i < source.Length; i++) source[i] = !source[i];
            return source;
        }

        /// <summary>
        /// Make a bitwise xor comparison
        /// </summary>
        /// <param name="source"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool[] BitWiseXor(this bool[] source, bool[] compare)
        {
            bool[] result = new bool[source.Length];
            compare = compare.ToLength(source.Length);

            for (int i = 0; i < source.Length; i++) result[i] = source[i] ^ compare[i];

            return result;
        }

        /// <summary>
        /// Shift bits left
        /// </summary>
        /// <param name="source"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static bool[] ShiftLeft(this bool[] source, int amount)
        {
            bool[] shift = new bool[amount];
            for (int i = 0; i < amount; i++) shift[i] = false;

            return source.JoinArray(shift).ToLength(source.Length);
        }

        /// <summary>
        /// Shift bits right
        /// </summary>
        /// <param name="source"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static bool[] ShiftRight(this bool[] source, int amount)
        {
            bool[] shift = new bool[amount];
            for (int i = 0; i < amount; i++) shift[i] = false;

            return shift.JoinArray(source.GetRange(1, source.Length - amount - 1));
        }

        /// <summary>
        /// Convert bit-array to single byte
        /// </summary>
        /// <param name="bits">8-bit-array</param>
        /// <returns>byte</returns>
        public static byte ToByte(this bool[] bits)
        {
            if (bits.Length != 8) throw new Exception("Invalid bit array.");

            byte result = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                result += bits[i] ? (byte)Math.Pow(2, bits.Length - i - 1) : (byte)0;
            }
            return result;
        }

        /// <summary>
        /// Return bit string
        /// </summary>
        /// <param name="bits">bit-array</param>
        /// <returns>bit string</returns>
        public static string ToBitString(this bool[] bits)
        {
            string result = string.Empty;

            for (int i = 0; i < bits.Length; i++)
            {
                result += bits[i] ? "1" : "0";
            }
            return result;
        }

        /// <summary>
        /// Converts bit-array to integer
        /// </summary>
        /// <param name="bits">bit-array</param>
        /// <returns>interger value</returns>
        public static int ToInt(this bool[] bits)
        {
            int result = 0;

            bool positiveValue = !bits[0];   // check if msb is 1

            if (!positiveValue)
            {
                bool[] minusOne = { true };

                bits = AddBinary(bits, minusOne);

                for (int i = 0; i < bits.Length; i++) bits[i] = !bits[i];
            }

            result = (int)bits.ToUnsignedInt();

            if (!positiveValue) result *= -1;

            return result;
        }

        /// <summary>
        /// Converts bit-array to unsigned integer
        /// </summary>
        /// <param name="bits">bit-array</param>
        /// <returns>interger value</returns>
        public static uint ToUnsignedInt(this bool[] bits)
        {
            uint result = 0;
            for (uint i = 0; i < bits.Length; i++)
            {
                result += bits[i] ? (uint)Math.Pow(2, bits.Length - i - 1) : 0;
            }
            return result;
        }

        /// <summary>
        /// Substract binary from binary
        /// </summary>
        /// <param name="number">binary to substract form</param>
        /// <param name="subtract">binary to substract with</param>
        /// <returns>result</returns>
        public static bool[] SubstractBinary(bool[] number, bool[] subtract)
        {
            bool[] minusOne = { true };

            subtract = AddBinary(subtract, minusOne);

            for (int i = 0; i < subtract.Length; i++) subtract[i] = !subtract[i];

            return AddBinary(number, subtract);
        }

        /// <summary>
        /// Converts interger number to bits
        /// </summary>
        /// <param name="number">number to convert</param>
        /// <returns>bit array</returns>
        public static bool[] ToBits(this int number)
        {
            byte[] intBytes = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);

            bool[] result = new bool[8 * intBytes.Length];

            for (int i = 0; i < result.Length; i += 8)
            {
                bool[] digit = intBytes[i / 8].ToBits();
                for (int j = i; j < i + 8; j++) result[j] = digit[j - i];
            }
            return result;
        }

        /// <summary>
        /// Converts interger number to bits
        /// </summary>
        /// <param name="number">number to convert</param>
        /// <returns>bit array</returns>
        public static bool[] ToBits(this uint number)
        {
            byte[] intBytes = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);

            bool[] result = new bool[8 * intBytes.Length];

            for (uint i = 0; i < result.Length; i += 8)
            {
                bool[] digit = intBytes[i / 8].ToBits();
                for (uint j = i; j < i + 8; j++) result[j] = digit[j - i];
            }
            return result;
        }

        /// <summary>
        /// Gets a range of elements from bool array
        /// </summary>
        /// <param name="array">array</param>
        /// <param name="start">start index</param>
        /// <param name="length">length of sequence (number of items)</param>
        /// <returns></returns>
        public static bool[] GetRange(this bool[] array, int start, int length)
        {
            return array.ToList().GetRange(start, length).ToArray();
        }

        /// <summary>
        /// Join arrays. The arrays are joined according to the order of the parameters
        /// </summary>
        /// <param name="first">first array</param>
        /// <param name="others">other arrays</param>
        /// <returns>combined arrays</returns>
        public static bool[] JoinArray(this bool[] first, params bool[][] others)
        {
            foreach (bool[] array in others)
                first = first.Concat(array.ToList()).ToArray();
            return first;
        }

        /// <summary>
        /// Add operation
        /// </summary>
        /// <param name="number">number 1</param>
        /// <param name="summand">number 2</param>
        /// <returns>sum</returns>
        public static bool[] AddBinary(bool[] number, bool[] summand)
        {
            if (summand.Length != number.Length)
            {
                if (summand.Length > number.Length) number = FillNumber(number, summand.Length);
                else summand = FillNumber(summand, number.Length);
            }

            bool[] result = new bool[number.Length];
            bool carry = false;

            for (int i = number.Length-1; i >= 0; i--)
            {
                result[i] = number[i] ^ summand[i] ^ carry;
                carry = number[i] & summand[i] || number[i] & carry || carry & summand[i];
            }
            return result;
        }

        /// <summary>
        /// Fill binary number up with msb value
        /// </summary>
        /// <param name="number">number to fill</param>
        /// <param name="length">legth to fill</param>
        /// <returns>filled number</returns>
        public static bool[] FillNumber(bool[] number, int length)
        {
            bool[] newNumber = new bool[length];
            for (int i = number.Length-1; i >= 0; i--)
            {
                newNumber[length - number.Length + i] = number[i];
            }
            for (int i = length - number.Length; i >= 0; i--) newNumber[i] = number.First();
            return newNumber;
        }

        /// <summary>
        /// Set binary number to length
        /// </summary>
        /// <param name="number">binary number</param>
        /// <param name="length">new length</param>
        /// <returns>number</returns>
        public static bool[] ToLength(this bool[] number, int length)
        {
            if (number.Length < length) return FillNumber(number, length);
            else if (number.Length > length)
            {
                bool[] newNumber = new bool[length];
                for (int i = number.Length - 1; i >= number.Length - length; i--)
                {
                    newNumber[i - (number.Length - length)] = number[i];
                }
                return newNumber;
            }
            return number;
        }

        /// <summary>
        /// Removes empty strings from string array
        /// </summary>
        /// <param name="array">string array</param>
        /// <returns>string arrays without empty strings</returns>
        public static string[] RemoveEmpties(this string[] array)
        {
            List<string> newList = new List<string>();
            foreach (string item in array) if (prepareString(item).Trim() != string.Empty) newList.Add(item);
            return newList.ToArray();
        }

        public static string[] RemoveUncommented(this string[] array)
        {
            List<string> newList = new List<string>();
            foreach (string item in array) if (prepareString(item).Trim().Substring(0,2) != "//") newList.Add(item);
            return newList.ToArray();
        }

        private static string prepareString(string value)
        {
            value = value.ToLower();
            value = Regex.Replace(value, "\t", "");
            value = Regex.Replace(value, "\n", "");
            value = Regex.Replace(value, "\r", "");
            return value;
        }

        public static bool TryParseHexWord(string word, out bool[] result)
        {
            result = new bool[32];
            for (int i = 0; i < 32; i++) result[i] = false;

            int number = 0;
            bool success;
            success = int.TryParse(word.Substring(2, word.Length - 2), NumberStyles.HexNumber, null, out number);

            if (!success) return false;

            result = number.ToBits();

            return true;
        }

        public static bool TryParseIntWord(string word, out bool[] result)
        {
            result = new bool[32];
            for (int i = 0; i < 32; i++) result[i] = false;

            int number = 0;
            bool success;
            success = int.TryParse(word, out number);

            if (!success) return false;

            result = number.ToBits();

            return true;
        }

        /// <summary>
        /// Get the Type of Mips instruction
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static InstructionType ToMipsType(this string op)
        {
            InstructionType result = InstructionType.INVALID;

            switch (op)
            {
                case AssemblerInstructions.Add:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Addu:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.And:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Div:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Divu:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Jalr:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Jr:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Mfhi:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Mflo:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Mthi:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Mtlo:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Mult:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Multu:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Nor:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Or:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Sll:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Sllv:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Slt:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Sltu:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Sra:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Srav:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Srl:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Srlv:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Sub:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Subu:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Xor:
                    result = InstructionType.RType;
                    break;
                case AssemblerInstructions.Addi:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Addiu:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Andi:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Beq:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Bgez:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Bgtz:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Blez:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Bltz:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Bne:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Lb:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Lbu:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Lh:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Lhu:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Lui:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Lw:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Ori:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Sb:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Slti:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Sltiu:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Sh:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Sw:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.Xori:
                    result = InstructionType.IType;
                    break;
                case AssemblerInstructions.J:
                    result = InstructionType.JType;
                    break;
                case AssemblerInstructions.Jal:
                    result = InstructionType.JType;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get bit-array from string
        /// </summary>
        /// <param name="bits">string</param>
        /// <returns>bit-array</returns>
        public static bool[] GetBitsBitString(string bits)
        {
            bool[] opCode = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
            {
                opCode[i] = int.Parse(bits[i].ToString()) == 1;
            }
            return opCode;
        }

        public static bool[] ToMipsRFunct(this string op)
        {
            bool[] result = new bool[6];

            switch (op)
            {
                case AssemblerInstructions.Add:
                    result = GetBitsBitString("100000");
                    break;
                case AssemblerInstructions.Addu:
                    result = GetBitsBitString("100001");
                    break;
                case AssemblerInstructions.And:
                    result = GetBitsBitString("100100");
                    break;
                case AssemblerInstructions.Div:
                    result = GetBitsBitString("011010");
                    break;
                case AssemblerInstructions.Divu:
                    result = GetBitsBitString("011011");
                    break;
                case AssemblerInstructions.Jalr:
                    result = GetBitsBitString("001001");
                    break;
                case AssemblerInstructions.Jr:
                    result = GetBitsBitString("001000");
                    break;
                case AssemblerInstructions.Mfhi:
                    result = GetBitsBitString("010000");
                    break;
                case AssemblerInstructions.Mflo:
                    result = GetBitsBitString("010010");
                    break;
                case AssemblerInstructions.Mthi:
                    result = GetBitsBitString("010001");
                    break;
                case AssemblerInstructions.Mtlo:
                    result = GetBitsBitString("010011");
                    break;
                case AssemblerInstructions.Mult:
                    result = GetBitsBitString("011000");
                    break;
                case AssemblerInstructions.Multu:
                    result = GetBitsBitString("011001");
                    break;
                case AssemblerInstructions.Nor:
                    result = GetBitsBitString("100111");
                    break;
                case AssemblerInstructions.Or:
                    result = GetBitsBitString("100101");
                    break;
                case AssemblerInstructions.Sll:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Sllv:
                    result = GetBitsBitString("000100");
                    break;
                case AssemblerInstructions.Slt:
                    result = GetBitsBitString("101010");
                    break;
                case AssemblerInstructions.Sltu:
                    result = GetBitsBitString("101011");
                    break;
                case AssemblerInstructions.Sra:
                    result = GetBitsBitString("000011");
                    break;
                case AssemblerInstructions.Srav:
                    result = GetBitsBitString("000111");
                    break;
                case AssemblerInstructions.Srl:
                    result = GetBitsBitString("000010");
                    break;
                case AssemblerInstructions.Srlv:
                    result = GetBitsBitString("000110");
                    break;
                case AssemblerInstructions.Sub:
                    result = GetBitsBitString("100010");
                    break;
                case AssemblerInstructions.Subu:
                    result = GetBitsBitString("100011");
                    break;
                case AssemblerInstructions.Xor:
                    result = GetBitsBitString("100110");
                    break;
            }
            return result;
        }

        /// <summary>
        /// Get the Mips OpCode
        /// </summary>
        /// <param name="op">Operation (add, addi, ...)</param>
        /// <returns>op code bits</returns>
        public static bool[] ToMipsOpCode(this string op)
        {
            bool[] result = new bool[6];

            switch (op)
            {
                case AssemblerInstructions.Add:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Addu:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.And:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Div:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Divu:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Jalr:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Jr:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Mfhi:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Mflo:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Mthi:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Mtlo:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Mult:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Multu:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Nor:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Or:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Sll:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Sllv:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Slt:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Sltu:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Sra:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Srav:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Srl:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Srlv:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Sub:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Subu:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Xor:
                    result = GetBitsBitString("000000");
                    break;
                case AssemblerInstructions.Addi:
                    result = GetBitsBitString("001000");
                    break;
                case AssemblerInstructions.Addiu:
                    result = GetBitsBitString("001001");
                    break;
                case AssemblerInstructions.Andi:
                    result = GetBitsBitString("001100");
                    break;
                case AssemblerInstructions.Beq:
                    result = GetBitsBitString("000100");
                    break;
                case AssemblerInstructions.Bgez:
                    result = GetBitsBitString("000001");
                    break;
                case AssemblerInstructions.Bgtz:
                    result = GetBitsBitString("000111");
                    break;
                case AssemblerInstructions.Blez:
                    result = GetBitsBitString("000110");
                    break;
                case AssemblerInstructions.Bltz:
                    result = GetBitsBitString("000001");
                    break;
                case AssemblerInstructions.Bne:
                    result = GetBitsBitString("000101");
                    break;
                case AssemblerInstructions.Lb:
                    result = GetBitsBitString("100000");
                    break;
                case AssemblerInstructions.Lbu:
                    result = GetBitsBitString("100100");
                    break;
                case AssemblerInstructions.Lh:
                    result = GetBitsBitString("100001");
                    break;
                case AssemblerInstructions.Lhu:
                    result = GetBitsBitString("100101");
                    break;
                case AssemblerInstructions.Lui:
                    result = GetBitsBitString("001111");
                    break;
                case AssemblerInstructions.Lw:
                    result = GetBitsBitString("100011");
                    break;
                case AssemblerInstructions.Ori:
                    result = GetBitsBitString("001101");
                    break;
                case AssemblerInstructions.Sb:
                    result = GetBitsBitString("101000");
                    break;
                case AssemblerInstructions.Slti:
                    result = GetBitsBitString("001001");
                    break;
                case AssemblerInstructions.Sltiu:
                    result = GetBitsBitString("001011");
                    break;
                case AssemblerInstructions.Sh:
                    result = GetBitsBitString("101001");
                    break;
                case AssemblerInstructions.Sw:
                    result = GetBitsBitString("101011");
                    break;
                case AssemblerInstructions.Xori:
                    result = GetBitsBitString("001110");
                    break;
                case AssemblerInstructions.J:
                    result = GetBitsBitString("000010");
                    break;
                case AssemblerInstructions.Jal:
                    result = GetBitsBitString("000011");
                    break;
            }

            return result;
        }
    }
}
