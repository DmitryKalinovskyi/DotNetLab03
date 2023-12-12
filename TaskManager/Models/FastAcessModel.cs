using System.IO;
using System.Windows.Media;
using TaskManager.Helpers;

namespace TaskManager.Models
{
    public class FastAcessModel
    {
        public ImageSource Image => ImageHelper.GetImageSourceFromFilename(Fullname);

        public string Title { get; set; }
        
        public string Filename => Path.GetFileName(Fullname);
        
        public string Fullname;

        public FastAcessModel(string title, string fullname) 
        {
            Title = title;
            Fullname = fullname;
        }
    }
}
