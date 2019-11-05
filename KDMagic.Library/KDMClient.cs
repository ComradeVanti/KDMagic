using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

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
            // Create xml document

            XmlDocument doc = new XmlDocument();

            // Create list of KDM files

            List<KDMFile> files = new List<KDMFile>();

            // Go through each file in the directory

            foreach (string filePath in Directory.GetFiles(directoryPath))
            {

                // Check if file is an xml file

                if (Path.GetExtension(filePath) == ".xml")
                {
                    // Load xml from file

                    doc.LoadXml(File.ReadAllText(filePath));

                    // Check if file is KDM file

                    if (doc.GetElementsByTagName("DCinemaSecurityMessage").Count == 1)
                    {

                        // Extract values

                        string movieName = doc.GetElementsByTagName("ContentTitleText")[0].InnerText.Split("_").First();
                        DateTime validFrom = DateTime.Parse(doc.GetElementsByTagName("ContentKeysNotValidBefore")[0].InnerText);
                        DateTime validTo = DateTime.Parse(doc.GetElementsByTagName("ContentKeysNotValidAfter")[0].InnerText);

                        // Create KDM file

                        KDMFile file = new KDMFile(
                            filePath,
                            movieName,
                            validFrom,
                            validTo
                            );

                        // Add file to list

                        files.Add(file);
                    }
                }
            }

            // Return list as array

            return files.ToArray();
        }

        /// <summary>
        /// Deletes the given KDM files
        /// </summary>
        /// <param name="files">The files that should be deleted</param>
        public static void DeleteFiles(KDMFile[] files)
        {
            // Go through each KDM file

            foreach (KDMFile file in files)

                // Delete the xml file at its path

                File.Delete(file.Path);
        }

        #endregion

    }
}
