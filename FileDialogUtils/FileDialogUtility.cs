namespace FileDialogUtils
{
    /// <summary>
    /// Provides utility methods for working with Windows file dialogs and generating safe, timestamped file names.
    /// </summary>
    public static class FileDialogUtility
    {
        /// <summary>
        /// Returns the default directory path used in file dialogs (user's Documents folder).
        /// </summary>
        private static string GetDefaultDirectoryPath() =>
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// Opens a standard Windows <see cref="OpenFileDialog"/> with an optional initial directory.
        /// </summary>
        /// <param name="directoryPath">The initial directory to open. If not specified or invalid, defaults to the user's Documents folder.</param>
        /// <returns>An initialized <see cref="OpenFileDialog"/> instance.</returns>
        public static OpenFileDialog GetOpenFileDialog(string directoryPath = "")
        {
            return new OpenFileDialog
            {
                InitialDirectory = string.IsNullOrEmpty(directoryPath)
                    ? GetDefaultDirectoryPath()
                    : directoryPath,
                Filter = "All files (*.*)|*.*"
            };
        }

        /// <summary>
        /// Creates and returns a configured <see cref="SaveFileDialog"/> instance with optional directory, file name, extension, and filter.
        /// </summary>
        /// <param name="directoryPath">The initial directory to open. If not specified or invalid, defaults to the user's Documents folder.</param>
        /// <param name="fileName">The base name of the file (without extension). Invalid characters will be sanitized.</param>
        /// <param name="fileExtension">The desired file extension (e.g., ".xlsx", ".txt"). Must include the dot.</param>
        /// <param name="filter">The file filter to apply in the dialog (e.g., "Excel files|*.xlsx").</param>
        /// <returns>An initialized <see cref="SaveFileDialog"/> instance with the suggested file name and path.</returns>
        public static SaveFileDialog GetSaveFileDialog(
            string directoryPath = "",
            string fileName = "NewFile",
            string fileExtension = ".txt",
            string filter = "All files (*.*)|*.*")
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                directoryPath = GetDefaultDirectoryPath();

            EnsureDirectoryExists(directoryPath);

            string safeFileName = MakeFileNameSafe(fileName);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string suggestedName = $"{TruncateIfFileLengthExceeds(safeFileName)}_{timestamp}{fileExtension}";

            return new SaveFileDialog
            {
                InitialDirectory = directoryPath,
                FileName = suggestedName,
                Filter = filter,
                DefaultExt = fileExtension.TrimStart('.') 
            };
        }

        public static string TruncateIfFileLengthExceeds(string fileName)
        {
            return fileName.Length > 193 ? fileName.Substring(0, 193) : fileName;
        }
        private static void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(GetDefaultDirectoryPath() + "/" +directoryPath))
                Directory.CreateDirectory(GetDefaultDirectoryPath() + "/" + directoryPath);
        }

        /// <summary>
        /// Generates a safe and unique file name by appending the current timestamp to the given base name.
        /// </summary>
        /// <param name="baseName">The base name of the file (e.g., "report"). Must not be null or whitespace.</param>
        /// <returns>A sanitized file name in the format: "baseName_yyyyMMdd_HHmmss.txt".</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="baseName"/> is null or empty.</exception>
        public static string GenerateFileNameWithTimestamp(string baseName)
        {
            if (string.IsNullOrWhiteSpace(baseName))
                throw new ArgumentException("Base name cannot be null or empty.", nameof(baseName));

            string safeBase = MakeFileNameSafe(baseName);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            return $"{safeBase}_{timestamp}";
        }

        /// <summary>
        /// Removes invalid characters from a string so it can be safely used as a file name.
        /// </summary>
        /// <param name="input">The file name input to sanitize.</param>
        /// <returns>A file name string with invalid characters replaced by underscores.</returns>
        private static string MakeFileNameSafe(string input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            return input.Trim();
        }
    }
}
