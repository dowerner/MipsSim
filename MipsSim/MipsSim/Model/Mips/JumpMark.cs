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
    /// Jump mark that links the name in code with the memory address
    /// </summary>
    public class JumpMark
    {
        /// <summary>
        /// Name in code
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Memory address during runtime
        /// </summary>
        public bool[] Address { get; private set; }

        public JumpMark(string name, bool[] startAddress, int codeLine)
        {
            Name = name;
            codeLine *= 4;
            Address = Extensions.AddBinary(startAddress, codeLine.ToBits());
        }
    }
}
