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
using MipsSim.View.Controls;
using System.Collections.ObjectModel;
using MipsSim.Model.Mips.Hardware;
using MipsSim.Model.Mips;
using System.IO;
using MipsSim.Model;
using System.Windows.Input;
using MipsSim.ViewModel.Commands;
using System.Windows;

namespace MipsSim.ViewModel
{
    /// <summary>
    /// View Model of MemoryViewer
    /// </summary>
    public class MemoryViewerViewModel : BaseViewModel
    {
        /// <summary>
        /// Line Indices
        /// </summary>
        public ObservableCollection<LineIndex> Indices { get; private set; }

        /// <summary>
        /// Line Indices (for word view)
        /// </summary>
        public ObservableCollection<LineIndex> WordIndices { get; private set; }

        /// <summary>
        /// Gets which part of the memory the viewer does observe
        /// </summary>
        public MemoryViewMode ViewMode { get; private set; }

        public MemoryViewerViewModel OtherViewer { get; set; }

        private const int offset = 20;
        private const int memoryRangeTolerance = 1000;
        private const int maxViewRange = 300;   // ListView crashes if this is too large

        private const uint dummyLength = 20;

        private uint startIndex;
        private uint endIndex;

        public ICommand SaveMemoryCommand { get; private set; }
        public ICommand LoadMemoryCommand { get; private set; }

        /// <summary>
        /// Memory Data
        /// </summary>
        public List<MemoryViewEntry> MemoryData
        {
            get 
            {
                _memoryData = new List<MemoryViewEntry>();
                for (uint i = startIndex; i < endIndex; i++) _memoryData.Add(new MemoryViewEntry(i, Memory.Instance.Data[i]));
                return _memoryData; 
            }
        }
        private List<MemoryViewEntry> _memoryData;

        /// <summary>
        /// Memory Data (for word view)
        /// </summary>
        public List<MemoryWordEntry> WordMemoryData
        {
            get
            {
                _wordMemoryData = new List<MemoryWordEntry>();
                for (uint i = startIndex; i < endIndex; i += 4) _wordMemoryData.Add(new MemoryWordEntry(i));
                return _wordMemoryData;
            }
        }
        private List<MemoryWordEntry> _wordMemoryData;

        private bool IsInRange(uint address)
        {
            return (address >= startIndex - memoryRangeTolerance && address <= endIndex + memoryRangeTolerance);
        }

        /// <summary>
        /// Display data in (bits, bytes, decimal)
        /// </summary>
        public DataDisplayMode DisplayMemoryMode
        {
            get { return _displayMemoryMode; }
            set
            {
                _displayMemoryMode = value;
                NotifyPropertyChanged("DisplayMemoryMode");
            }
        }
        private DataDisplayMode _displayMemoryMode;

        private uint length;

        public MemoryViewerViewModel(MemoryViewMode mode)
        {
            ViewMode = mode;
            length = dummyLength;
            Indices = new ObservableCollection<LineIndex>();
            WordIndices = new ObservableCollection<LineIndex>();
            Memory.Instance.MemoryDataChanged += MemoryDataChanged;
            Memory.Instance.MemorySizeChanged += MemorySizeChanged;
            Compiler.Instance.CompilerCompleted += CompilerCompleted;

            Indices.CollectionChanged += Indices_CollectionChanged;

            if (ViewMode == MemoryViewMode.InstructionSection)
            {
                FillViewer(length, (uint)Memory.Instance.GetProgramStartingAddress((int)length).ToInt());
            }
            else
            {
                FillViewer(length, (uint)Memory.Instance.GetStackPointerAddress().ToInt());
            }

            SaveMemoryCommand = new SaveMemoryCommand(this);
            LoadMemoryCommand = new LoadMemoryCommand(this);
        }

        /// <summary>
        /// Keep track of the word view indices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Indices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset) WordIndices.Clear();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && ((LineIndex)e.NewItems[0]).Index % 4 == 0) WordIndices.Add((LineIndex)e.NewItems[0]);
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && ((LineIndex)e.NewItems[0]).Index % 4 == 0)
            {
                for(int i = 0; i < WordIndices.Count; i++)
                {
                    if (WordIndices[i].Index == ((LineIndex)e.NewItems[0]).Index)
                    {
                        WordIndices.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void CompilerCompleted(int programLength)
        {
            if (ViewMode == MemoryViewMode.DataSection) return;
            length = (programLength < maxViewRange) ? (uint)programLength : maxViewRange;   // safety
            FillViewer(length, (uint)Cpu.Instance.PC.Data.ToInt());
        }

        private void MemorySizeChanged(uint size)
        {
            if (ViewMode == MemoryViewMode.InstructionSection) FillViewer(length, (uint)Memory.Instance.GetProgramStartingAddress((int)length).ToInt());
            else FillViewer(length, (uint)Memory.Instance.GetStackPointerAddress().ToInt());
            NotifyPropertyChanged("WordIndices");
            NotifyPropertyChanged("WordMemoryData");
        }

        private void MemoryDataChanged(uint address)
        {
            if (Debugger.Instance.Mode == ProgramMode.Stopped) return;
            //if (!IsInRange(address)) return;    // ignore if this was for a different memory section

            if (ViewMode == MemoryViewMode.DataSection)
            {
                int sp = Cpu.Instance.GetRegValue("$sp").ToInt();
                int fp = Cpu.Instance.GetRegValue("$fp").ToInt();
                FillViewer((uint)(fp - sp), (uint)sp);
            }

            SetActiveLine(address);
            NotifyPropertyChanged("MemoryData");
            NotifyPropertyChanged("WordMemoryData");
            NotifyPropertyChanged("Indices");
            NotifyPropertyChanged("WordIndices");
        }

        /// <summary>
        /// Sets a line active according to memory address
        /// </summary>
        /// <param name="address">address</param>
        public void SetActiveLine(uint address)
        {
            foreach (LineIndex line in Indices)
                if (line.LineState == LineState.ActiveLine) line.Reset();
            if (address >= startIndex && address < Indices.Count + startIndex) Indices.First(item => item.Index == address).SetActive();
            else foreach (LineIndex line in Indices) line.Reset();
        }

        public void UnsetLines()
        {
            foreach (LineIndex line in Indices) line.Reset();
        }

        /// <summary>
        /// Sets a line error according to memory address
        /// </summary>
        /// <param name="address">address</param>
        public void SetErrorLine(int index)
        {
            foreach (LineIndex line in Indices)
                if (line.LineState == LineState.ErrorLine) line.Reset();
            if (index >= startIndex && index < Indices.Count + startIndex) Indices.First(item=>item.Index == index).SetError();
        }

        public void Renew(bool renewBothViews)
        {
            foreach (MemoryViewEntry data in MemoryData) data.Update();

            NotifyPropertyChanged("MemoryData");
            NotifyPropertyChanged("WordMemoryData");

            if (renewBothViews) OtherViewer.Renew(false);
        }

        public void SaveCurrentMemory()
        {
            string path = SimWindowViewModel.GetPathFromFileSaveDialog(Directory.GetCurrentDirectory(), ".mem", "Memory Files (*.mem)|*.mem");
            if (path == string.Empty) return;
            FileDirector.Instance.SaveMemory(path);            
        }

        public void LoadMemoryFromFile()
        {
            try
            {
                string path = SimWindowViewModel.GetPathFromFileOpenDialog(Directory.GetCurrentDirectory(), ".mem", "Memory Files (*.mem)|*.mem");
                FileDirector.Instance.LoadMemoryFromFile(path);
            }
            catch
            {
                MessageBox.Show("Unable to load memory file. Make sure you open a proper file.", "Loading error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Fill Indices new (in case of memory resize or program compile)
        /// </summary>
        /// <param name="sectionLength"></param>
        /// <param name="startAddress"></param>
        private void FillViewer(uint sectionLength, uint startAddress)
        {
            startIndex = startAddress - offset;
            endIndex = startAddress + sectionLength*4 + offset;

            Indices.Clear();
            for (uint i = startIndex; i < endIndex; i++)
                Indices.Add(new LineIndex((int)i));
            NotifyPropertyChanged("MemoryData");
            NotifyPropertyChanged("Indices");
            NotifyPropertyChanged("WordIndices");
            NotifyPropertyChanged("WordMemoryData");
        }
    }
}
