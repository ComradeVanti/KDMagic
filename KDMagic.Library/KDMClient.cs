using System;
using System.Collections.Generic;
using System.Text;

namespace KDMagic.Library
{

    /// <summary>
    /// Static client class to interact with KDM files
    /// </summary>
    public static class KDMClient
    {

        #region Methods

        /// <summary>
        /// Gets all KDM files in a given direcory
        /// </summary>
        /// <param name="directoryPath">The KDM folder path</param>
        /// <returns>All KDM files in the given directory</returns>
        public static KDMFile[] GetFiles(string directoryPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all invalid KDM files in a given direcory
        /// </summary>
        /// <param name="directoryPath">The KDM folder path</param>
        /// <returns>All invalid KDM files in the given directory</returns>
        public static KDMFile[] GetInvalidFiles(string directoryPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the given KDM files
        /// </summary>
        /// <param name="files">The files that should be deleted</param>
        public static void DeleteFiles(KDMFile[] files)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
