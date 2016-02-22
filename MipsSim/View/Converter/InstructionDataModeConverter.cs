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
using System.Windows.Data;
using MipsSim.ViewModel;
using MipsSim.Model.Mips;

namespace MipsSim.View.Converter
{
    /// <summary>
    /// This converter is used by the UI to convert bool-Arrays to strings
    /// </summary>
    public class InstructionDataModeConverter : IMultiValueConverter
    {
        public delegate void BoolConversionFailedHandler();
        public static event BoolConversionFailedHandler BoolConversionFailed;

        /// <summary>
        /// Converts bool-array acording to current view mode settings
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool[] value = (bool[])values[0];
            DataDisplayMode mode = (DataDisplayMode)values[1];  // get a hold of the view mode settings

            // decide which format should be returned
            if (mode == DataDisplayMode.Bits) return value.ToBitString();
            if (mode == DataDisplayMode.Decimal) return value.ToInt().ToString();
            if (mode == DataDisplayMode.Bytes) return string.Format("0x{0}", value.ToInt().ToString("X8"));
            return value;
        }

        /// <summary>
        /// Convert string to bool array.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            string data = (string)value;
            bool success = false;
            bool[] result = new bool[8];
            for (int i = 0; i < 8; i++) result[i] = false;

            // evaluate which input format was used
            if (data.Length > 2 && data.Substring(0, 2) == "0x") success = Extensions.TryParseHexWord(data, out result);    // if user entered hex
            if (!success && onlyContainsZeroOne(data)) // if user entered binary format
            {
                result = Extensions.GetBitsBitString(data);
                success = true;
            }
            if (!success) success = Extensions.TryParseIntWord(data, out result);   // if user entered int format

            if (!success && BoolConversionFailed != null) BoolConversionFailed();   /// if conversion failed alert

            object[] returnValue = new object[1];
            returnValue[0] = result;

            return returnValue;
        }

        private bool onlyContainsZeroOne(string data)
        {
            foreach (char c in data.ToCharArray()) if (c != '0' && c != '1') return false;
            return true;
        }
    }
}
