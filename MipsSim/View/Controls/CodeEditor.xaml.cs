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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using MipsSim.ViewModel;
using System.ComponentModel;

namespace MipsSim.View.Controls
{
    /// <summary>
    /// Interaction logic for CodeEditor.xaml
    /// </summary>
    public partial class CodeEditor : UserControl
    {
        private const string ActiveIndexProperty = "ActiveIndex";
        private const string ErrorIndexProperty = "ErrorIndex";

        public ObservableCollection<LineIndex> LineIndices { get; set; }

        private SimWindowViewModel _vm;

        public CodeEditor()
        {
            InitializeComponent();
            LineIndices = new ObservableCollection<LineIndex>();
            IndexList.ItemsSource = LineIndices;
            Editor.LineCountChanged += Editor_LineCountChanged;
            DataContextChanged += CodeEditor_DataContextChanged;
        }

        private void CodeEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is SimWindowViewModel)) return;
            _vm = e.NewValue as SimWindowViewModel;
            _vm.PropertyChanged += _vm_PropertyChanged;
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case ActiveIndexProperty:
                    SetActiveLine(_vm.ActiveIndex);
                    break;
                case ErrorIndexProperty:
                    SetErrorLine(_vm.ErrorIndex);
                    break;
            }
        }

        private void SetActiveLine(int index)
        {
            foreach (LineIndex line in LineIndices) 
                if(line.LineState == LineState.ActiveLine) line.Reset();
            if (index >= 0 && index < LineIndices.Count) LineIndices[index].SetActive();
        }

        private void SetErrorLine(int index)
        {
            foreach (LineIndex line in LineIndices)
                if (line.LineState == LineState.ErrorLine) line.Reset();
            if (index >= 0 && index < LineIndices.Count) LineIndices[index].SetError();
        }

        private void Editor_LineCountChanged(int lineCount)
        {
            if (lineCount >= LineIndices.Count)
            {
                for (int i = LineIndices.Count; i <= lineCount; i++) LineIndices.Add(new LineIndex(i));
            }
            if(lineCount < LineIndices.Count)
            {
                for (int i = LineIndices.Count-1; i >= lineCount; i--) LineIndices.RemoveAt(i);
            }
        }
    }

    public enum LineState
    {
        ActiveLine,
        ErrorLine,
        NormalLine
    }

    /// <summary>
    /// Represents a line index that can be colored for debugging
    /// </summary>
    public class LineIndex : BaseViewModel
    {
        private readonly SolidColorBrush red;
        private readonly SolidColorBrush yellow;
        private readonly SolidColorBrush green;
        private readonly SolidColorBrush black;

        public uint Index
        {
            get { return _index; }
            set
            {
                _index = value;
                NotifyPropertyChanged("Index");
            }
        }
        private uint _index;

        public SolidColorBrush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                NotifyPropertyChanged("Background");
            }
        }
        private SolidColorBrush _background;

        public SolidColorBrush Foreground
        {
            get { return _foreground; }
            set
            {
                _foreground = value;
                NotifyPropertyChanged("Foreground");
            }
        }
        private SolidColorBrush _foreground;

        /// <summary>
        /// Sets the line as active (it becomes green)
        /// </summary>
        public void SetActive()
        {
            LineState = Controls.LineState.ActiveLine;
            Background = green;
            Foreground = black;
        }

        /// <summary>
        /// Indicates that there is an error in this line (it becomes red)
        /// </summary>
        public void SetError()
        {
            LineState = Controls.LineState.ErrorLine;
            Background = red;
            Foreground = yellow;
        }

        /// <summary>
        /// Sets that this line has no special properties
        /// </summary>
        public void Reset()
        {
            LineState = Controls.LineState.NormalLine;
            Background = null;
            Foreground = black;
        }

        public LineState LineState { get; private set; }

        public LineIndex(int index)
        {
            // colors
            red = new SolidColorBrush(Colors.Red);
            yellow = new SolidColorBrush(Colors.Yellow);
            green = new SolidColorBrush(Colors.LightGreen);
            black = new SolidColorBrush(Colors.Black);

            // initialization
            Index = (uint)index;
            LineState = Controls.LineState.NormalLine;
            Reset();
        }
    }
}
