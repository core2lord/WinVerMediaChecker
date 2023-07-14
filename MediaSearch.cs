using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WinVerMediaChecker
{
    internal class MediaSearch
    {
        #region Public Constructors

        public MediaSearch(MainWindow? mainWindow)
        {
            _mainWindow = mainWindow;
        }

        #endregion

        #region Private Fields

        private readonly MainWindow? _mainWindow;

        #endregion

        #region Public Fields

        /// <summary>
        /// Readonly <see cref="Dictionary{int, char}"/> containing all-lower characters of alphabet 'a' to 'z' using an index starting with '1'.
        /// </summary>
        public readonly Dictionary<int, char> _driveLetterList = new()
        {
            {1, 'a'},
            {2, 'b'},
            {3, 'c'},
            {4, 'd'},
            {5, 'e'},
            {6, 'f'},
            {7, 'g'},
            {8, 'h'},
            {9, 'i'},
            {10, 'j'},
            {11, 'k'},
            {12, 'l'},
            {13, 'm'},
            {14, 'n'},
            {15, 'o'},
            {16, 'p'},
            {17, 'q'},
            {18, 'r'},
            {19, 's'},
            {20, 't'},
            {21, 'u'},
            {22, 'v'},
            {23, 'w'},
            {24, 'x'},
            {25, 'y'},
            {26, 'z'}
        };

        #endregion

        #region Public Properties

        public char[] ReadyDriveArray { get; set; }

        #endregion

        #region Public Methods

        public IEnumerable<DriveInfo> FindActiveDrives()
        {
            var driveLetterIndex = Enumerable.Range(1, 26);

            while (_mainWindow is not null)
            {
                foreach (var driveLetterKey in driveLetterIndex)
                {
                    var _driveInfo = new DriveInfo(_driveLetterList[driveLetterKey].ToString());
                    if (_driveInfo.IsReady == true)
                    {
                        yield return _driveInfo;
                    }
                }
            }
        }

        #endregion
    }
}