using System;

namespace KDMagic.WPF.Models
{

    /// <summary>
    /// UI model for the <see cref="KDMagic.Library.KDMFile"/> class
    /// </summary>
    public class KDMFileModel
    {

        #region Properties

        public string MovieName { get; private set; }

        /// <summary>
        /// Formatted string displaying the timespan in which this KDM is valid
        /// </summary>
        public string TimeSpan
        {
            get
            {
                return string.Format("Valid timespan: {0,-10} - {1,-10}", ValidFrom.ToShortDateString(), ValidTo.ToShortDateString());
            }
        }

        public DateTime ValidFrom { get; private set; }

        public DateTime ValidTo { get; private set; }

        #endregion

        #region Constructors

        public KDMFileModel(string movieName, DateTime validFrom, DateTime validTo)
        {
            MovieName = movieName;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        #endregion

    }
}
