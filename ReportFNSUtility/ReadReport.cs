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
        /// <summary>
        /// Обновление данных в заголовке и дереве тегов из входной строки
        /// </summary>
        /// <param name="path">Строка, указывающая абсолютный путь к файлу</param>
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
        /// <summary>
        /// Выводит документы в treeView
        /// </summary>
        /// <param name="startIndexDoc">Начальный индекс документа</param>
        /// <param name="endIndexDoc">Конечный индекс документа</param>
        /// <returns>УСпешность завершения операции</returns>
        public bool GetNodes(UInt32 startIndexDoc, UInt32 endIndexDoc)
        {
            foreach (var item in Program.reportFNS.treeOfTags.GetNodes(startIndexDoc, endIndexDoc))
            {
                Program.form.Invoke((MethodInvoker)delegate { Program.form.TV_TreeTags.Nodes.Add(item); });
            }
            return true;
        }
    }
}
