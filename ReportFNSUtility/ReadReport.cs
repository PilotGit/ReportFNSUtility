using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReportFNSUtility
{

    class ReadReport
    {
        String path;
        BinaryReader reader;
        ReportFS reportFS;
        public static Form1 form = null;


        public ReadReport(string path)
        {
            this.path = path;
            reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }

        public int Read()
        {
            reportFS = new ReportFS(reader);

            var baseStream = reader.BaseStream;
            reader.Close();
            baseStream.Close();
            return 0;
        }
        ~ReadReport()
        {
        }
    }
}
