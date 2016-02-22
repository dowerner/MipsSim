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

namespace MipsSim.Model.Mips.Hardware
{
    /// <summary>
    /// A CPU register
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Register length
        /// </summary>
        public const int RegisterLength = 32;

        /// <summary>
        /// Data in bits
        /// </summary>
        public bool[] Data { get; private set; }

        /// <summary>
        /// Register Index
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Register Name
        /// </summary>
        public string RegisterName { get; private set; }

        public Register(string name, int index)
        {
            Data = new bool[RegisterLength];
            RegisterName = name;
            Index = index;
        }

        /// <summary>
        /// Sets the register data
        /// </summary>
        /// <param name="data">data</param>
        public void SetRegData(bool[] data)
        {
            for (int i = 0; i < RegisterLength; i++) Data[i] = data[i];
        }

        /// <summary>
        /// Sets the register data
        /// </summary>
        /// <param name="data">data</param>
        public void SetRegData(byte[] data)
        {
            bool[] bitData = data.ToMipsInstructionBits();
            for (int i = 0; i < RegisterLength; i++) Data[i] = bitData[i];
        }

        /// <summary>
        /// Sets the register data
        /// </summary>
        /// <param name="data">data</param>
        public void SetRegData(string data)
        {
            bool[] bitData = Extensions.GetBitsBitString(data);
            for (int i = 0; i < RegisterLength; i++) Data[i] = bitData[i];
        }
    }
}
