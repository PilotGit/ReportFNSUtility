using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

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
        public BinaryReader reader;
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

    class ReportReader
    {
        public ReportReader()
        {

        }

        public void UpdateData(string path)
        {
            FileStream _fs = new FileStream(path, FileMode.Open);
            try
            {
                if (!Program.reportFNS.reportHeader.UpdateFromStream(new BinaryReader(_fs)))
                {
                    throw new Exception("Файл повреждён. Не удалось считать заголовок.");
                }
                if (!Program.reportFNS.reportHeader.ChekHash(_fs))
                {
                    throw new Exception("Файл повреждён. Не удалось считать дерево тегов.");
                }
                if (!Program.reportFNS.treeOfTags.UpdateFromStream(new BinaryReader(_fs)))
                {
                    throw new Exception("Файл повреждён. Не удалось считать дерево тегов.");
                }
            }
            catch (Exception ex)
            {
                _fs.Close();
                throw ex;
            }
        }

        public bool GetNodes(UInt32 startNumberDoc, UInt32 endNumberDoc)
        {
            foreach (var item in Program.reportFNS.treeOfTags.GetNodes(startNumberDoc, endNumberDoc))
            {
                Form1.form.Invoke((MethodInvoker)delegate { Form1.form.TV_TreeTags.Nodes.Add(item); });
            }
            return true;
        }
    }
}
