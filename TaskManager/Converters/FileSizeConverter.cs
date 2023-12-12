using System;
using System.Globalization;
using System.Windows.Data;

namespace TaskManager.Converters
{
    public class FileSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long fileSizeInBytes)
            {
                return FormatFileSize(fileSizeInBytes);
            }

            return value; // Return the original value if it's not a long
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string FormatFileSize(long fileSizeInBytes)
        {
            const int byteConversion = 1024;
            double bytes = fileSizeInBytes;

            string[] suffixes = { "B", "Kb", "Mb", "Gb", "Tb", "Pb", "Eb", "Zb", "Yb" };

            int suffixIndex = 0;
            while (bytes >= byteConversion && suffixIndex < suffixes.Length - 1)
            {
                bytes /= byteConversion;
                suffixIndex++;
            }

            return $"{bytes:0.##} {suffixes[suffixIndex]}";
        }
    }

}
