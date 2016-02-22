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

namespace MipsSim.View
{
    /// <summary>
    /// This class handles view related functionality
    /// </summary>
    public class ViewDirector
    {

        private LoadingView _loadingView;

        /// <summary>
        /// Shows loading indication window
        /// </summary>
        public void ShowMemoryLoadingView()
        {
            if(_loadingView == null) _loadingView = new LoadingView();
            _loadingView.Show();
        }

        /// <summary>
        /// Hides loading indication window
        /// </summary>
        public void HideMemoryLoadingView()
        {
            if (_loadingView != null) _loadingView.Close();
        }

        #region Singleton
        private ViewDirector()
        {
        }

        public static ViewDirector Instance
        {
            get
            {
                if (_instance == null) _instance = new ViewDirector();
                return _instance;
            }
        }
        private static ViewDirector _instance;
        #endregion
    }
}
