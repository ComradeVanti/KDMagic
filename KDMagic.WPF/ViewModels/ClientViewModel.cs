using Caliburn.Micro;
using KDMagic.Library;
using KDMagic.WPF.Models;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KDMagic.WPF.ViewModels
{

    /// <summary>
    /// Viewmodel for the main client view
    /// </summary>
    public class ClientViewModel : Screen
    {

        #region Fields

        private bool inProcess;
        private bool onlySelectOutdated;
        private string directoryPath;
        private KDMFile[] invalidFiles = new KDMFile[0];
        private BindingList<KDMFileModel> invalidFileModels = new BindingList<KDMFileModel>();
        private KDMFileModel selectedFile;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if there is currently a asynchronous process running
        /// </summary>
        public bool InProcess
        {
            get { return inProcess; }
            set
            {
                inProcess = value;
                NotifyOfPropertyChange(() => InProcess);
                NotifyOfPropertyChange(() => LoadingBarVisible);
                NotifyOfPropertyChange(() => CanScan);
                NotifyOfPropertyChange(() => CanDelete);
            }
        }

        /// <summary>
        /// True if the user has selected a valid directory
        /// </summary>
        public bool CanScan
        {
            get
            {
                return !InProcess && Directory.Exists(DirectoryPath);
            }
        }

        /// <summary>
        /// True if there is at least one invalid KDM file
        /// </summary>
        public bool CanDelete
        {
            get
            {
                return !InProcess && InvalidCount > 0;
            }
        }

        /// <summary>
        /// Indicated wether all invalid or only outdated KDMs should be selected
        /// If false KDMs that are not yet valid will also be selected
        /// </summary>
        public bool OnlySelectOutdated
        {
            get { return onlySelectOutdated; }
            set
            {
                onlySelectOutdated = value;
                NotifyOfPropertyChange(() => OnlySelectOutdated);
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

        /// <summary>
        /// Display string for the number of invalid KDMS
        /// </summary>
        public string InvalidCountDisplayString { get { return $"{InvalidCount} invalid KDMs found!"; } }

        /// <summary>
        /// Bound property that controlls the visibility of the loading bar
        /// </summary>
        public Visibility LoadingBarVisible { get { return InProcess ? Visibility.Visible : Visibility.Hidden; } }

        /// <summary>
        /// List of the models representing the currently selected invalid files
        /// </summary>
        public BindingList<KDMFileModel> InvalidFileModels
        {
            get { return invalidFileModels; }
            set
            {
                invalidFileModels = value;
                NotifyOfPropertyChange(() => InvalidFileModels);
            }
        }

        /// <summary>
        /// The selected KDM file model
        /// </summary>
        public KDMFileModel SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                NotifyOfPropertyChange(() => SelectedFile);
            }
        }


        /// <summary>
        /// The number of invalid KDM files
        /// </summary>
        private int InvalidCount
        {
            get
            {
                return InvalidFiles.Length;
            }
        }

        /// <summary>
        /// The currently scanned invalid files
        /// </summary>
        private KDMFile[] InvalidFiles
        {
            get
            {
                return invalidFiles;
            }
            set
            {
                invalidFiles = value;

                NotifyOfPropertyChange(() => InvalidCountDisplayString);
                NotifyOfPropertyChange(() => CanDelete);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allows the user to browse the explorer for a browser path
        /// </summary>
        public void Browse()
        {
            // Open a dialog box to select a path

            var dialog = new VistaFolderBrowserDialog();
            var result = dialog.ShowDialog();

            // If a path was returned store it in the DirectoryPath property

            if (result.HasValue && result.Value)
                DirectoryPath = dialog.SelectedPath;
        }

        /// <summary>
        /// Scans the folder for KDM files
        /// </summary>
        public async Task Scan()
        {
            // Set process flag

            InProcess = true;

            // Get the files in the directorypath

            KDMFile[] files = await KDMClient.GetFiles(DirectoryPath);

            // Select only invalid ones

            InvalidFiles = files.Where(f => !f.Valid).ToArray();

            // Populate model list

            InvalidFileModels.Clear();

            foreach (KDMFile file in InvalidFiles)
                invalidFileModels.Add(new KDMFileModel(
                    file.MovieName,
                    file.ValidFrom,
                    file.ValidTo
                    ));

            // Select first item (If there are any in the list)

            if (InvalidFileModels.Count > 0)
                SelectedFile = InvalidFileModels[0];

            // Reset process flag

            InProcess = false;
        }

        /// <summary>
        /// Deletes the selected KDM files
        /// </summary>
        public async Task Delete()
        {
            // Set process flag

            InProcess = true;

            // Delete files

            await KDMClient.DeleteFiles(invalidFiles);

            // Reset array

            InvalidFiles = new KDMFile[0];

            // Clear model list

            InvalidFileModels.Clear();

            // Reset process flag

            InProcess = false;
        }

        #endregion

    }
}
