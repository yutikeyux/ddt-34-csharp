using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Tank.Request.Illegalcharacters
{
    /// <summary>
    /// Handles the loading and checking of illegal characters/words from files.
    /// </summary>
    public class FileSystem
    {
        private List<string> _illegalWords;

        /// <summary>
        /// Initializes a new instance of the FileSystem class.
        /// </summary>
        /// <param name="illegalCharPath">Physical path to the main illegal characters file.</param>
        /// <param name="illegalDirPath">Physical path to a directory containing additional illegal word files.</param>
        /// <param name="searchPattern">File pattern to look for in the directory (e.g., "*.txt").</param>
        public FileSystem(string illegalCharPath, string illegalDirPath, string searchPattern)
        {
            _illegalWords = new List<string>();

            // 1. Load from the single specific file
            if (!string.IsNullOrEmpty(illegalCharPath) && System.IO.File.Exists(illegalCharPath))
            {
                try
                {
                    string[] words = System.IO.File.ReadAllLines(illegalCharPath);
                    _illegalWords.AddRange(words);
                }
                catch (Exception ex)
                {
                    // Log or silently ignore if the file is locked/missing during startup
                    System.Diagnostics.Debug.WriteLine("Error loading illegal char file: " + ex.Message);
                }
            }

            // 2. Load from the directory
            if (!string.IsNullOrEmpty(illegalDirPath) && System.IO.Directory.Exists(illegalDirPath))
            {
                try
                {
                    // Get all files matching the pattern (e.g., *.txt)
                    string[] files = System.IO.Directory.GetFiles(illegalDirPath, searchPattern, SearchOption.TopDirectoryOnly);

                    foreach (string file in files)
                    {
                        try
                        {
                            string[] words = System.IO.File.ReadAllLines(file);
                            _illegalWords.AddRange(words);
                        }
                        catch { /* Ignore individual file read errors */ }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error scanning illegal directory: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Checks if the input text contains any of the loaded illegal words.
        /// </summary>
        /// <param name="input">The text to check (e.g., Nickname).</param>
        /// <returns>True if an illegal word is found, False otherwise.</returns>
        public bool checkIllegalChar(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // Check against the loaded list
            // Note: Depending on requirements, you might want case-insensitive comparison
            foreach (string word in _illegalWords)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    // Simple Contains check. 
                    // For better performance with large lists, consider using a HashSet or Trie.
                    if (input.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return true; // Illegal character found
                    }
                }
            }

            return false; // Safe
        }
    }
}