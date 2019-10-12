using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Monitor
{
    public static class FileUtils
    {
        public static long getFileLenth(string fn)
        {
            var f = new FileInfo(fn);
            return f.Length;


        }
    }
}
