using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using TaskManager.Models;

namespace TaskManager.Converters
{
    public class ProcessPriorityConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            // first value is process, second is new priority specified by index
            // from system diagnostics
            /*
                Normal = 32,
                Idle = 64,
                High = 128,
                RealTime = 256,
                BelowNormal = 16384,
                AboveNormal = 32768
             */

            return (ProcessPriorityClass)int.Parse((string)values);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
