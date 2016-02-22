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

using System.Collections.Generic;
using System.Linq;
using MipsSim.Model.Mips.Hardware;

namespace MipsSim.Model.Mips
{
    /// <summary>
    /// This class is used to keep track between the editor code and the binary instructions that are beeing executed
    /// </summary>
    public class Debugger
    {
        public delegate void ProgramModeChangedHandler(ProgramMode mode);
        public event ProgramModeChangedHandler ProgramModeChanged;

        private List<DebuggerRelationship> relations;

        /// <summary>
        /// Starts new debugging operation
        /// </summary>
        public void StartNew()
        {
            relations.Clear();  // clear all previous editor associations
        }

        /// <summary>
        /// Initial PC
        /// </summary>
        public bool[] InitiaclPC { get; set; }

        /// <summary>
        /// Gets or sets the current running mode of the program
        /// </summary>
        public ProgramMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                if (ProgramModeChanged != null) ProgramModeChanged(_mode);
            }
        }
        private ProgramMode _mode;

        /// <summary>
        /// Adds new relationship between memory and code
        /// </summary>
        /// <param name="editorLine">line in code</param>
        /// <param name="instructionAddress">address in memory</param>
        public void AddRelationship(int editorLine, bool[] instructionAddress)
        {
            relations.Add(new DebuggerRelationship(editorLine, instructionAddress));
        }

        /// <summary>
        /// Gets code line from memory address
        /// </summary>
        /// <param name="address">memory address</param>
        /// <returns>line</returns>
        public int GetLineFromAddress(bool[] address)
        {
            var relation = relations.FirstOrDefault(item => item.InstructionAddress.ToInt() == address.ToInt());
            return (relation != null) ? relation.EditorLine : -1;
        }

        /// <summary>
        /// Returns wether a line that belongs to an address exists
        /// </summary>
        /// <param name="address">address</param>
        /// <returns></returns>
        public bool LineExists(bool[] address)
        {
            return (relations.Find(item => item.InstructionAddress.ToInt() == address.ToInt()) != null);
        }

        /// <summary>
        /// Stops the execution of the program
        /// </summary>
        public void StopProgramm()
        {
            Mode = ProgramMode.Stopped;
            Cpu.Instance.SetRegData(Cpu.Instance.PC.RegisterName, Debugger.Instance.InitiaclPC);
        }

        /// <summary>
        /// Gets memory address from code line
        /// </summary>
        /// <param name="line">line</param>
        /// <returns>memory address</returns>
        public bool[] GetAddressFromLine(int line)
        {
            return relations.First(item => item.EditorLine == line).InstructionAddress;
        }

        #region Singleton
        private Debugger()
        {
            relations = new List<DebuggerRelationship>();
            _mode = ProgramMode.Stopped;
        }

        public static Debugger Instance
        {
            get
            {
                if (_instance == null) _instance = new Debugger();
                return _instance;
            }
        }
        private static Debugger _instance;
        #endregion
    }
}
