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

using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace MipsSim.View.Controls
{
    /// <summary>
    /// Code editor control
    /// </summary>
    public class AssemblerEditor : TextBox
    {
        public delegate void LineCountChangedHandler(int lineCount);
        public event LineCountChangedHandler LineCountChanged;

        // Extend textbox control
        public AssemblerEditor() : base()
        {
            this.AcceptsTab = true;
            this.AcceptsReturn = true;
            this.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        }

        // fire alretLineChanged event
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            AlertLineChange();
        }

        // insert tabs in text according to previous line
        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
            if (e.Key == Key.Enter) // fix tabs
            {
                string tabs = string.Empty;

                int cursorPosition = SelectionStart;

                if (cursorPosition == 0) return;
                string lineText = GetLineText(GetLineIndexFromCharacterIndex(cursorPosition-1));

                int tabCount = lineText.Contains('\t') ? lineText.Split('\t').Count()-1 : 0;

                for (int i = 0; i < tabCount; i++) tabs += "\t";

                Text = Text.Insert(cursorPosition, tabs);
                SelectionStart = cursorPosition + tabCount;
            }
            AlertLineChange();
        }

        private void AlertLineChange()
        {
            if (LineCountChanged != null) LineCountChanged(GetLastVisibleLineIndex() + 1);
        }
    }
}
