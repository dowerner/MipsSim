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
using System.Globalization;

namespace MipsSim.View.Converter
{
    /// <summary>
    /// This converter is used by the UI to convert bytes to strings
    /// </summary>
    public class DataModeConverter : IMultiValueConverter
    {

        public delegate void ConversionFailedHandler();
        public static event ConversionFailedHandler ConversionFailed;

        /// <summary>
        /// Converts byte acording to current view mode settings
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte value = (byte)values[0];   // get data to convert
            DataDisplayMode mode = (DataDisplayMode)values[1];  // get display mode

            // decide which string version to return
            if (mode == DataDisplayMode.Bits) return value.ToBits().ToBitString();
            if (mode == DataDisplayMode.Decimal) return ((int)value).ToString();
            if (mode == DataDisplayMode.Bytes) return string.Format("0x{0}", value.ToString("X2"));
            return value;
        }

        /// <summary>
        /// Convert string to byte
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
            byte result = 0;

            // choose conversion acording to input string
            if (data.Length > 2 && data.Substring(0, 2) == "0x") success = byte.TryParse(data.Substring(2, data.Length - 2), NumberStyles.HexNumber, null, out result); // convert from hex format
            if (!success && data.Length == 8 && onlyContainsZeroOne(data))  // convert from binary format
            {
                result = Extensions.GetBitsBitString(data).ToByte();
                success = true;
            }
            if (!success) success = byte.TryParse(data, out result);    // convert from int format

            if (!success && ConversionFailed != null) ConversionFailed();   // alert if conversion failed

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
