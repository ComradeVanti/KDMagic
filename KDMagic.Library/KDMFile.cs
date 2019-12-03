using System;

namespace KDMagic.Library
{

    /// <summary>
    /// Represents a single KDM file and exposes some information
    /// </summary>
    public class KDMFile
    {

        #region Properties

        /// <summary>
        /// The path of this file
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// The name of the movie this kdm validates
        /// </summary>
        public string MovieName { get; private set; }

        /// <summary>
        /// The start time of the valid time-span
        /// </summary>
        public DateTime ValidFrom { get; private set; }

        /// <summary>
        /// The end time of the valid time-span
        /// </summary>
        public DateTime ValidTo { get; private set; }

        #endregion

        #region Constructor

        public KDMFile(string path, string movieName, DateTime validFrom, DateTime validTo)
        {
            Path = path;
            MovieName = movieName;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Indicated wether the file is currently valid
        /// </summary>
        /// <param name="onlyInvalidateOutdated">Controls wether the file is considered invalid when not yet valid</param>
        /// <returns>True if the file is valid, false otherwise</returns>
        public bool IsValid(bool onlyInvalidateOutdated)
        {
            if (onlyInvalidateOutdated)
                return DateTime.Now < ValidTo;
            else
                return ValidFrom < DateTime.Now && DateTime.Now < ValidTo;
        }

        #endregion

    }
}
