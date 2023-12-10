namespace EncryptLibrary
{
    public class FileEncryptionResult
    {
        public enum FileEncryptionState
        {
            None = 0,
            Completed,
            Failed,
            ExceptionRaised
        }

        public FileEncryptionState State { get; set; } = FileEncryptionState.None;

        public DateTime EncryptionStart { get; set; }

        public DateTime EncryptionEnd { get; set; }

        public TimeSpan EncryptionDuration => EncryptionEnd - EncryptionStart;

        public long FileSize { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public FileEncryptionResult(long fileSize, string source, string destination)
        {
            FileSize = fileSize;
            Source = source;
            Destination = destination;
        }
    }
}
