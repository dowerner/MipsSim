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

namespace MipsSim.Model.Mips
{
    /// <summary>
    /// All Mips Instruction Names
    /// </summary>
    public static class AssemblerInstructions
    {
        public const string Add = "add";
        public const string Addu = "addu";
        public const string And = "and";
        public const string Div = "div";
        public const string Divu = "divu";
        public const string Jalr = "jalr";
        public const string Jr = "jr";
        public const string Mfhi = "mfhi";
        public const string Mflo = "mflo";
        public const string Mthi = "mthi";
        public const string Mtlo = "mtlo";
        public const string Mult = "mult";
        public const string Multu = "multu";
        public const string Nor = "nor";
        public const string Or = "or";
        public const string Sll = "sll";
        public const string Sllv = "sllv";
        public const string Slt = "slt";
        public const string Sltu = "sltu";
        public const string Sra = "sra";
        public const string Srav = "srav";
        public const string Srl = "srl";
        public const string Srlv = "srlv";
        public const string Sub = "sub";
        public const string Subu = "subu";
        public const string Xor = "xor";
        public const string Addi = "addi";
        public const string Addiu = "addiu";
        public const string Andi = "andi";
        public const string Beq = "beq";
        public const string Bgez = "bgez";
        public const string Bgtz = "bgtz";
        public const string Blez = "blez";
        public const string Bltz = "bltz";
        public const string Bne = "bne";
        public const string Lb = "lb";
        public const string Lbu = "lbu";
        public const string Lh = "lh";
        public const string Lhu = "lhu";
        public const string Lui = "lui";
        public const string Lw = "lw";
        public const string Ori = "ori";
        public const string Sb = "sb";
        public const string Slti = "slti";
        public const string Sltiu = "sltiu";
        public const string Sh = "sh";
        public const string Sw = "sw";
        public const string Xori = "xori";
        public const string J = "j";
        public const string Jal = "jal";

        public const string AddFunct = "100000";
        public const string AdduFunct = "100001";
        public const string AndFunct = "100100";
        public const string DivFunct = "011010";
        public const string DivuFunct = "011011";
        public const string JalrFunct = "001001";
        public const string JrFunct = "001000";
        public const string MfhiFunct = "010000";
        public const string MfloFunct = "010010";
        public const string MthiFunct = "010001";
        public const string MtloFunct = "010011";
        public const string MultFunct = "011000";
        public const string MultuFunct = "011001";
        public const string NorFunct = "100111";
        public const string OrFunct = "100101";
        public const string SllFunct = "000000";
        public const string SllvFunct = "000100";
        public const string SltFunct = "101010";
        public const string SltuFunct = "101011";
        public const string SraFunct = "000011";
        public const string SravFunct = "000111";
        public const string SrlFunct = "000010";
        public const string SrlvFunct = "000110";
        public const string SubFunct = "100010";
        public const string SubuFunct = "100011";
        public const string XorFunct = "100110";

        public const string AddiOpCode = "001000";
        public const string AddiuOpCode = "001001";
        public const string AndiOpCode = "001100";
        public const string BeqOpCode = "000100";
        public const string BgezOpCode = "000001";
        public const string BgtzOpCode = "000111";
        public const string BlezOpCode = "000110";
        public const string BltzOpCode = "000001";
        public const string BneOpCode = "000101";
        public const string LbOpCode = "100000";
        public const string LbuOpCode = "100100";
        public const string LhOpCode = "100001";
        public const string LhuOpCode = "100101";
        public const string LuiOpCode = "001111";
        public const string LwOpCode = "100011";
        public const string OriOpCode = "001101";
        public const string SbOpCode = "101000";
        public const string SltiOpCode = "001010";
        public const string SltiuOpCode = "001011";
        public const string ShOpCode = "101001";
        public const string SwOpCode = "101011";
        public const string XoriOpCode = "001110";

        public const string JOpCode = "000010";
        public const string JalOpCode = "000011";
    }
}
