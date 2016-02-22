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
using System.IO;
using System.Xml.Serialization;
using MipsSim.Model.Mips.Hardware;
using MipsSim.Model.Mips;
using MipsSim.View;

namespace MipsSim.Model
{
    /// <summary>
    /// Handles File opening and saveing
    /// </summary>
    public class FileDirector
    {
        private const string tempSuffix = ".tmp";

        /// <summary>
        /// Open text file
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>text from file</returns>
        public string OpenAssemblerFile(string path)
        {
            string result = string.Empty;
            TextReader reader = new StreamReader(path);
            result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        /// <summary>
        /// Save text file
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="data">data to save</param>
        public void SaveAssemblerFile(string path, string data)
        {
            // create a security copy in case the file gets corrupted
            if (File.Exists(path)) File.Copy(path, path + tempSuffix);

            bool success = false;

            while (!success)    // try writing to file until it works
            {
                try
                {
                    TextWriter writer = new StreamWriter(path);
                    writer.Write(data);
                    writer.Close();
                    success = true;
                    File.Delete(path + tempSuffix); // delete copy
                }
                catch { }
            }
        }

        /// <summary>
        /// Save memory to file (bytewise)
        /// </summary>
        /// <param name="path">file path</param>
        public void SaveMemory(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);

            foreach (byte data in Memory.Instance.Data) fs.WriteByte(data);

            fs.Close();
        }

        /// <summary>
        /// Save registers to file (bytewise)
        /// </summary>
        /// <param name="path">file path</param>
        public void SaveRegister(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            byte[] temp;
            byte[] regData = new byte[(Cpu.Instance.Registers.Length + 3)*4];

            for (int i = 0; i < Cpu.Instance.Registers.Length; i++)
            {
                temp = Cpu.Instance.Registers[i].Data.ToMipsInstructionBytes();
                regData[i * 4] = temp[0];
                regData[i * 4 + 1] = temp[1];
                regData[i * 4 + 2] = temp[2];
                regData[i * 4 + 3] = temp[3];
            }

            temp = Cpu.Instance.Hi.Data.ToMipsInstructionBytes();
            regData[Cpu.Instance.Registers.Length * 4]  = temp[0];
            regData[Cpu.Instance.Registers.Length * 4 + 1] = temp[1];
            regData[Cpu.Instance.Registers.Length * 4 + 2] = temp[2];
            regData[Cpu.Instance.Registers.Length * 4 + 3] = temp[3];

            temp = Cpu.Instance.Lo.Data.ToMipsInstructionBytes();
            regData[Cpu.Instance.Registers.Length * 4 + 4] = temp[0];
            regData[Cpu.Instance.Registers.Length * 4 + 5] = temp[1];
            regData[Cpu.Instance.Registers.Length * 4 + 6] = temp[2];
            regData[Cpu.Instance.Registers.Length * 4 + 7] = temp[3];

            temp = Cpu.Instance.PC.Data.ToMipsInstructionBytes();
            regData[Cpu.Instance.Registers.Length * 4 + 8] = temp[0];
            regData[Cpu.Instance.Registers.Length * 4 + 9] = temp[1];
            regData[Cpu.Instance.Registers.Length * 4 + 10] = temp[2];
            regData[Cpu.Instance.Registers.Length * 4 + 11] = temp[3];

            foreach (byte data in regData) fs.WriteByte(data);

            fs.Close();
        }

        /// <summary>
        /// Load bytes from file and load them into the CPU registers
        /// </summary>
        /// <param name="path">file path</param>
        public void LoadRegisterFromFile(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] regData = new byte[(Cpu.Instance.Registers.Length + 3) * 4];
                byte[] temp = new byte[4];

                for (int i = 0; i < fs.Length; i++) regData[i] = (byte)fs.ReadByte();

                for (int i = 0; i < Cpu.Instance.Registers.Length; i++)
                {
                    temp[0] = regData[i * 4];
                    temp[1] = regData[i * 4 + 1];
                    temp[2] = regData[i * 4 + 2];
                    temp[3] = regData[i * 4 + 3];
                    Cpu.Instance.Registers[i].SetRegData(temp.ToMipsInstructionBits());
                }

                temp[0] = regData[Cpu.Instance.Registers.Length * 4];
                temp[1] = regData[Cpu.Instance.Registers.Length * 4 + 1];
                temp[2] = regData[Cpu.Instance.Registers.Length * 4 + 2];
                temp[3] = regData[Cpu.Instance.Registers.Length * 4 + 3];
                Cpu.Instance.Hi.SetRegData(temp.ToMipsInstructionBits());

                temp[0] = regData[Cpu.Instance.Registers.Length * 4 + 4];
                temp[1] = regData[Cpu.Instance.Registers.Length * 4 + 5];
                temp[2] = regData[Cpu.Instance.Registers.Length * 4 + 6];
                temp[3] = regData[Cpu.Instance.Registers.Length * 4 + 7];
                Cpu.Instance.Lo.SetRegData(temp.ToMipsInstructionBits());

                temp[0] = regData[Cpu.Instance.Registers.Length * 4 + 8];
                temp[1] = regData[Cpu.Instance.Registers.Length * 4 + 9];
                temp[2] = regData[Cpu.Instance.Registers.Length * 4 + 10];
                temp[3] = regData[Cpu.Instance.Registers.Length * 4 + 11];
                Cpu.Instance.PC.SetRegData(temp.ToMipsInstructionBits());

                fs.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Load memory file into main memory
        /// </summary>
        /// <param name="path">file path</param>
        public void LoadMemoryFromFile(string path)
        {
            ViewDirector.Instance.ShowMemoryLoadingView();  // shows loading indicator

            try
            {
                ViewDirector.Instance.ShowMemoryLoadingView();

                FileStream fs = new FileStream(path, FileMode.Open);
                Memory.Instance.SetNewSizeInBytes((uint)fs.Length);

                for (int i = 0; i < fs.Length; i++) Memory.Instance.Data[i] = (byte)fs.ReadByte();

                fs.Close();

                ViewDirector.Instance.HideMemoryLoadingView();  // hides loading indicator
            }
            catch (Exception e)
            {
                ViewDirector.Instance.HideMemoryLoadingView();  // hides loading indicator
                throw e;
            }
        }

        /// <summary>
        /// Generic xml desirialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private T OpenXmlFile<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(path);
            object result = serializer.Deserialize(reader);
            reader.Close();
            return (T)result;
        }


        #region Singleton
        private FileDirector()
        {
        }

        public static FileDirector Instance
        {
            get
            {
                if (_instance == null) _instance = new FileDirector();
                return _instance;
            }
        }
        private static FileDirector _instance;
        #endregion
    }
}
