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
    /// Main Memory of the computer
    /// </summary>
    public class Memory
    {
        public const uint defaultSize = 524288;  // 512 kByte

        private const uint kilo = 1024;
        private const uint mega = 1048576;

        public delegate void MemoryDataChangedHandler(uint address);
        public event MemoryDataChangedHandler MemoryDataChanged;

        public delegate void MemorySizeChangedHandler(uint size);
        public event MemorySizeChangedHandler MemorySizeChanged;

        /// <summary>
        /// RAM bytes
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Set new memory size (memory data will be lost!)
        /// </summary>
        /// <param name="size">new size</param>
        public void SetNewSizeInBytes(uint size)
        {
            Data = new byte[size];
            if (MemorySizeChanged != null) MemorySizeChanged(size);
        }

        /// <summary>
        /// Set new memory size (memory data will be lost!)
        /// </summary>
        /// <param name="size">new size</param>
        public void SetNewSizeInKBytes(uint size)
        {
            Data = new byte[size * kilo];
            if (MemorySizeChanged != null) MemorySizeChanged(size*kilo);
        }

        /// <summary>
        /// Set new memory size (memory data will be lost!)
        /// </summary>
        /// <param name="size">new size</param>
        public void SetNewSizeInMBytes(uint size)
        {
            Data = new byte[size * mega];
            if (MemorySizeChanged != null) MemorySizeChanged(size*mega);
        }

        /// <summary>
        /// Writes instruction to memory
        /// </summary>
        /// <param name="startAddress">start address (mod 4)</param>
        /// <param name="instruction">32-bit instruction</param>
        public void WriteInstructionToMemory(uint startAddress, bool[] instruction)
        {
            SaveWord(startAddress, instruction);
        }

        /// <summary>
        /// Empty the memory
        /// </summary>
        public void Reset()
        {
            Data = new byte[Data.Length];
        }

        /// <summary>
        /// Writes register-data to memory
        /// </summary>
        /// <param name="startAddress">start address (mod 4)</param>
        /// <param name="instruction">32-bit register data</param>
        public void SaveWord(uint startAddress, bool[] registerData)
        {
            byte[] memoryData = registerData.ToMipsInstructionBytes();
            for (uint i = 0; i < memoryData.Length; i++)
            {
                Data[startAddress + i] = memoryData[i];
            }
            if (MemoryDataChanged != null) MemoryDataChanged(startAddress);
        }

        /// <summary>
        /// Loads word from memory
        /// </summary>
        /// <param name="startAddress">start address of the word</param>
        public bool[] LoadWord(uint startAddress)
        {
            bool[] result = Data[startAddress].ToBits();
            for (uint i = 1; i < 4; i++)
            {
                result = result.JoinArray(Data[startAddress + i].ToBits());
            }
            return result;
        }

        /// <summary>
        /// Gets starting Address from the memory
        /// The location lies in the upper 5th of the memory
        /// </summary>
        /// <returns>Address</returns>
        public bool[] GetProgramStartingAddress(int instructionCount)
        {
            int startAddress = Data.Length - Data.Length / 5 - instructionCount;

            string bitString = string.Empty;
            bool[] bits = startAddress.ToBits();

            for (int i = 0; i < bits.Length - 2; i++) bitString += bits[i] ? "1" : "0";

            bitString += "00";

            return Extensions.GetBitsBitString(bitString);
        }

        /// <summary>
        /// Gets StackPointer Address from the memory
        /// The location lies in the lower 5th of the memory
        /// </summary>
        /// <returns>Address</returns>
        public bool[] GetStackPointerAddress()
        {
            int startAddress = Data.Length / 5;

            string bitString = string.Empty;
            bool[] bits = startAddress.ToBits();

            for (int i = 0; i < bits.Length - 2; i++) bitString += bits[i] ? "1" : "0";

            bitString += "00";

            return Extensions.GetBitsBitString(bitString);
        }

        #region Singleton
        private Memory()
        {
            Data = new byte[defaultSize];
        }

        public static Memory Instance
        {
            get
            {
                if (_instance == null) _instance = new Memory();
                return _instance;
            }
        }
        private static Memory _instance;
        #endregion
    }
}
