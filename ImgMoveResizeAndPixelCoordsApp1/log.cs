using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgMoveResizeAndPixelCoordsApp1
{
    public static class log
    {
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ImgMoveResizeAndPixelCoords.txt";

        public static void add(string s, bool NewLine = false)
        {
            try { File.AppendAllLines(path, new string[] { (NewLine ? "\r\n" : "") + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss") + " - " + s }); } catch { }
        }
    }
}
