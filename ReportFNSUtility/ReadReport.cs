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

        public ReadReport(string path)
        {
            this.path = path;
            reader = new BinaryReader(new FileStream(path,FileMode.Open));
        }

        public int Read()
        {
            ReportHeader reportHeader = new ReportHeader(reader);
            return 0;
        }
    }
}
