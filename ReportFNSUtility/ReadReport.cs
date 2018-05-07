using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ReportFNSUtility
{
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
                throw ex;
            }
            finally
            {
                _fs.Close();
            }
        }

        public bool GetNodes(UInt32 startIndexDoc, UInt32 endIndexDoc)
        {
            foreach (var item in Program.reportFNS.treeOfTags.GetNodes(startIndexDoc, endIndexDoc))
            {
                Form1.form.Invoke((MethodInvoker)delegate { Form1.form.TV_TreeTags.Nodes.Add(item); });
            }
            return true;
        }
    }
}
