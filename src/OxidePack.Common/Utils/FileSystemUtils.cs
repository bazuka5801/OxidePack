using System;

namespace OxidePack
{
    public static class FileSystemUtils
    {
        public static (double sizeResult, string sizeTitle) AdjustFileSize(long fileSizeInBytes)
        {
            var names = new[] {"B", "KB", "MB", "GB", "TB"};

            double sizeResult = fileSizeInBytes;
            int nameIndex = 0;
            while (sizeResult > 1024 && nameIndex < names.Length)
            {
                sizeResult /= 1024;
                nameIndex++;
            }

            if (nameIndex > names.Length - 1)
            {
                string lastName = names[nameIndex - 1];
                throw new OverflowException($"AdjustFileSize: fileSizeInBytes >= 1024{lastName}");
            }

            return (sizeResult, names[nameIndex]);
        }
    }
}