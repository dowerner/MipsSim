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
using System.Windows.Input;
using MipsSim.ViewModel;

namespace MipsSim.View.Controls
{
    /// <summary>
    /// Interaction logic for MemoryViewer.xaml
    /// </summary>
    public partial class MemoryViewer : UserControl
    {
        private MemoryViewerViewModel _vm;

        public MemoryViewer()
        {
            InitializeComponent();

            DataContextChanged += MemoryViewer_DataContextChanged;
            TabControl.SelectionChanged += TabControl_SelectionChanged;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_vm != null) _vm.Renew(false);
        }

        private void MemoryViewer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MemoryViewerViewModel) _vm = (MemoryViewerViewModel)e.NewValue;
        }

        private void EditMemoryEntryClick(object sender, MouseButtonEventArgs e)
        {
            MemoryViewEntry entry = (MemoryViewEntry)((TextBlock)sender).DataContext;
            _vm.SetActiveLine(entry.Index);
            MemoryEditView editWindow = new MemoryEditView();
            editWindow.DataContext = new MemoryEditViewModel((int)entry.Index, entry.Data, _vm, editWindow);
            editWindow.ShowDialog();
            _vm.UnsetLines();
        }

        private void EditWordEntryClick(object sender, MouseButtonEventArgs e)
        {
            MemoryWordEntry entry = (MemoryWordEntry)((TextBlock)sender).DataContext;

            if (_vm.ViewMode == MemoryViewMode.InstructionSection)
            {
                _vm.SetActiveLine(entry.Index);
                InstructionEditViewModel vm = new InstructionEditViewModel(entry, _vm);
                InstructionEditView editWindow = new InstructionEditView(vm);
                vm.View = editWindow;
                editWindow.ShowDialog();
            }
            else
            {
                DataEditView editWindow = new DataEditView();
                editWindow.DataContext = new DataEditViewModel(entry, _vm, editWindow);
                editWindow.ShowDialog();
            }
            _vm.UnsetLines();
        }
    }
}
