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
        /// True if the current date falls into the valid time-span
        /// </summary>
        public bool Valid { get { return ValidFrom < DateTime.Now && DateTime.Now < ValidTo; } }

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

    }
}
