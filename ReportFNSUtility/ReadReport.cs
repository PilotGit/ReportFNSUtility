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
        /// <summary>
        /// Путь к файлу отчёта
        /// </summary>
        String path;
        /// <summary>
        /// Двоичный reader потока файла отчёта
        /// </summary>
        BinaryReader reader;
        /// <summary>
        /// Отчёт о счиывании данных с фискального накопителя
        /// </summary>
        ReportFS reportFS;


        public ReadReport(string path)
        {
            this.path = path;
            reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }

        public int Read()
        {
            reportFS = new ReportFS(reader);
            reader.Close();
            return 0;
        }
    }
}
