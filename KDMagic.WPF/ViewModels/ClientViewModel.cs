using Caliburn.Micro;
using KDMagic.Library;
using System;
using System.IO;

namespace KDMagic.WPF.ViewModels
{

    /// <summary>
    /// Viewmodel for the main client view
    /// </summary>
    public class ClientViewModel : Screen
    {

        #region Fields

        private string directoryPath;
        private KDMFile[] invalidFiles = new KDMFile[0];

        #endregion

        #region Properties

        /// <summary>
        /// True if the user has selected a valid directory
        /// </summary>
        public bool CanScan
        {
            get
            {
                return Directory.Exists(DirectoryPath);
            }
        }

        /// <summary>
        /// True if there is at least one invalid KDM file
        /// </summary>
        public bool CanDelete
        {
            get
            {
                return InvalidCount > 0;
            }
        }

        /// <summary>
        /// The number of invalid KDM files
        /// </summary>
        public int InvalidCount
        {
            get
            {
                return invalidFiles.Length;
            }
        }

        /// <summary>
        /// The current selected KDM folder path
        /// </summary>
        public string DirectoryPath
        {
            get { return directoryPath; }
            set
            {
                directoryPath = value;
                NotifyOfPropertyChange(() => DirectoryPath);
                NotifyOfPropertyChange(() => CanScan);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allows the user to browse the explorer for a browser path
        /// </summary>
        public void Browse()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Scans the folder for KDM files
        /// </summary>
        public void Scan()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the selected KDM files
        /// </summary>
        public void Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
