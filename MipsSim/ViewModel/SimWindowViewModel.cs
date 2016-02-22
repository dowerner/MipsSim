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
using System.IO;
using System.Windows.Input;
using MipsSim.ViewModel.Commands;
using MipsSim.Model.Mips;
using MipsSim.Model.Mips.Hardware;
using MipsSim.Model;
using System.Windows;
using MipsSim.View;

namespace MipsSim.ViewModel
{
    public class SimWindowViewModel : BaseViewModel
    {
        public readonly string WindowName = "Mips Sim";

        #region Properties
        /// <summary>
        /// Gets or sets the assembler code in the editor
        /// </summary>
        public string AssemblerCode
        {
            get { return _assemblerCode; }
            set
            {
                _assemblerCode = value;
                NotifyPropertyChanged("AssemblerCode");
                TextEdited = true;
            }
        }
        private string _assemblerCode;

        /// <summary>
        /// Display data in (bits, bytes, decimal)
        /// </summary>
        public DataDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set
            {
                _displayMode = value;
                if (InstructionViewModel != null) InstructionViewModel.DisplayMemoryMode = _displayMode;
                if (DataViewModel != null) DataViewModel.DisplayMemoryMode = _displayMode;
                if (RegisterViewModel != null) RegisterViewModel.DisplayRegisterMode = _displayMode;
                NotifyPropertyChanged("DisplayMode");
            }
        }
        private DataDisplayMode _displayMode;

        /// <summary>
        /// Indicated wether the WindowTitle has a (*) in it
        /// </summary>
        public bool TextEdited
        {
            get { return _textEdited; }
            set
            {
                _textEdited = value;
                if((_textEdited && !WindowTitle.Contains('*'))
                    || !_textEdited)
                    WindowTitle = _textEdited ? string.Format("{0}*", WindowTitle) : WindowTitle.Replace('*', ' ');
                NotifyPropertyChanged("TextEdited");
            }
        }
        private bool _textEdited;

        /// <summary>
        /// Gets or sets the active Index
        /// </summary>
        public int ActiveIndex
        {
            get { return _activeIndex; }
            set
            {
                _activeIndex = value;
                NotifyPropertyChanged("ActiveIndex");
            }
        }
        private int _activeIndex;

        /// <summary>
        /// Gets or sets the error index
        /// </summary>
        public int ErrorIndex
        {
            get { return _errorIndex; }
            set
            {
                _errorIndex = value;
                NotifyPropertyChanged("ErrorIndex");
            }
        }
        private int _errorIndex;

        /// <summary>
        /// Gets or sets the FilePath to look for files
        /// </summary>
        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
                NotifyPropertyChanged("FolderPath");
            }
        }
        private string _folderPath;

        /// <summary>
        /// Gets or sets the currently opened file
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }
        private string _filePath;

        public string WindowTitle
        {
            get { return _WindowTitle; }
            set
            {
                _WindowTitle = value;
                NotifyPropertyChanged("WindowTitle");
            }
        }
        private string _WindowTitle;

        public bool ResetMemoryOnCompile
        {
            get { return Compiler.Instance.ResetMemoryOnCompile; }
            set
            {
                Compiler.Instance.ResetMemoryOnCompile = value;
                NotifyPropertyChanged("ResetMemoryOnCompile");
            }
        }

        public bool ResetRegisterOnCompile
        {
            get { return Compiler.Instance.ResetRegisterOnCompile; }
            set
            {
                Compiler.Instance.ResetRegisterOnCompile = value;
                NotifyPropertyChanged("ResetRegisterOnCompile");
            }
        }

        public bool IsEditorEnabled
        {
            get { return Debugger.Instance.Mode == ProgramMode.Stopped; }
        }
        #endregion

        #region MemoryViewModels
        public MemoryViewerViewModel InstructionViewModel { get; set; }
        public MemoryViewerViewModel DataViewModel { get; set; }
        public RegisterViewerViewModel RegisterViewModel { get; set; }
        #endregion

        #region Commands
        public ICommand OpenFileCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand RunCommand { get; private set; }
        public ICommand StepCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand OpenHelpCommand { get; private set; }
        public ICommand NewFileCommand { get; private set; }
        public ICommand SaveMemoryCommand { get; private set; }
        public ICommand LoadMemoryCommand { get; private set; }
        public ICommand SaveCpuCommand { get; private set; }
        public ICommand LoadCpuCommand { get; private set; }
        #endregion

        public SimWindowViewModel()
        {
            FolderPath = Directory.GetCurrentDirectory();
            WindowTitle = WindowName;

            // Memory View Models
            InstructionViewModel = new MemoryViewerViewModel(MemoryViewMode.InstructionSection);
            DataViewModel = new MemoryViewerViewModel(MemoryViewMode.DataSection);
            RegisterViewModel = new RegisterViewerViewModel();

            InstructionViewModel.OtherViewer = DataViewModel;
            DataViewModel.OtherViewer = InstructionViewModel;

            // commands
            OpenFileCommand = new OpenFileCommand(this);
            SaveCommand = new SaveCommand(this);
            SaveAsCommand = new SaveAsCommand(this);
            RunCommand = new RunCommand(this);
            StepCommand = new StepCommand(this);
            StopCommand = new StopCommand(this);
            OpenSettingsCommand = new OpenSettingsCommand(this);
            AboutCommand = new AboutCommand();
            OpenHelpCommand = new OpenHelpCommand();
            NewFileCommand = new NewFileCommand(this);
            SaveMemoryCommand = new SaveMemoryCommand(InstructionViewModel);
            LoadMemoryCommand = new LoadMemoryCommand(InstructionViewModel);
            SaveCpuCommand = new SaveCpuCommand(RegisterViewModel);
            LoadCpuCommand = new LoadCpuCommand(RegisterViewModel);

            // compiler handling
            Compiler.Instance.CompileErrorFound += CompileErrorFound;

            // debugger handling
            Debugger.Instance.ProgramModeChanged += ProgramModeChanged;

            // Cpu handling
            Cpu.Instance.RuntimeExceptionThrow += Instance_RuntimeExceptionThrow;
        }

        private void Instance_RuntimeExceptionThrow(int line, string message)
        {
            RuntimeError(line, message);
        }

        public void ShowActiveLineInEditor(bool[] pc)
        {
            var test = Memory.Instance.Data[pc.ToInt()];
            //if (Debugger.Instance.LineExists(pc) || Memory.Instance.Data[pc.ToInt()] != 0)    // would allow code injection
            if (Debugger.Instance.LineExists(pc))
                ActiveIndex = Debugger.Instance.GetLineFromAddress(pc); // terminates execution as soon as code which was written in editor finishes
            else
            {
                Debugger.Instance.StopProgramm();
                ActiveIndex = -1;
                InstructionViewModel.UnsetLines();
                DataViewModel.UnsetLines();
            }
        }

        private void ProgramModeChanged(ProgramMode mode)
        {
            NotifyPropertyChanged("IsEditorEnabled");
        }

        private void CompileErrorFound(int lineIndex, string message)
        {
            CompilerError(lineIndex, message);
        }

        private void CompilerError(int lineIndex, string message)
        {
            ErrorIndex = lineIndex;
            MessageBox.Show(message, "Compiler Error", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void RuntimeError(int lineIndex, string message)
        {
            ErrorIndex = lineIndex;
            MessageBox.Show(message, "Runtime Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
            Debugger.Instance.StopProgramm();
        }

        /// <summary>
        /// Save current file
        /// </summary>
        /// <param name="path">path</param>
        public void Save(string path)
        {
            FileDirector.Instance.SaveAssemblerFile(path, _assemblerCode);
            this.TextEdited = false;
        }

        /// <summary>
        /// Save current file by save dialog
        /// </summary>
        public void SaveAs()
        {
            string path = GetPathFromFileSaveDialog(FilePath);
            if (path == string.Empty) return;
            FilePath = path;
            Save(path);
        }

        /// <summary>
        /// Returns a path to a file or string.Empty if aborted
        /// </summary>
        /// <param name="defaultPath">starting path</param>
        /// <returns>path</returns>
        public static string GetPathFromFileSaveDialog(string defaultPath, string extension, string description)
        {
            string path = string.Empty;

            // Create OpenFileDialog 
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = extension;
            dlg.Filter = description;
            dlg.InitialDirectory = defaultPath;


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name
            if (result == true) path = dlg.FileName;
            return path;
        }

        public static string GetPathFromFileSaveDialog(string defaultPath)
        {
            return GetPathFromFileSaveDialog(defaultPath, ".a", "Assembler Files (*.a)|*.a");
        }

        /// <summary>
        /// Returns a path to a file or string.Empty if aborted
        /// </summary>
        /// <param name="defaultPath">starting path</param>
        /// <returns>path</returns>
        public static string GetPathFromFileOpenDialog(string defaultPath, string extension, string description)
        {
            string path = string.Empty;

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = extension;
            dlg.Filter = description;
            dlg.InitialDirectory = defaultPath;


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name
            if (result == true) path = dlg.FileName;
            return path;
        }

        public static string GetPathFromFileOpenDialog(string defaultPath)
        {
            return GetPathFromFileOpenDialog(defaultPath, ".a", "Assembler Files (*.a)|*.a");
        }

        public void OpenSettings()
        {
            SettingsView settings = new SettingsView();
            settings.ShowDialog();
        }
    }
}
