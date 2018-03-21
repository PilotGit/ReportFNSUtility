using DamienG.Security.Cryptography;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportFNSUtility
{
    /// <summary>
    /// Отчёт о считывнии данных из ФН
    /// </summary>
    class ReportFS
    {
        /// <summary>
        /// Заголовок файла отчёта
        /// </summary>
        ReportHeader header;

        /// <summary>
        /// Фискальные даннные длительного хранения
        /// </summary>
        List<Structurs> fDLongStorage = new List<Structurs>();

        /// <summary>
        /// 
        /// </summary>
        List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>> fdLongStorage = new List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>();

        /// <summary>
        /// Конструктор получающий данные из файлового потока отчёта
        /// </summary>
        /// <param name="reader">Поток файла отчёта</param>
        public ReportFS(BinaryReader reader)
        {
            header = new ReportHeader(reader);
            TreeNodeCollection nodes = Form1.form.treeView1.Nodes;
            Form1.form.Invoke((MethodInvoker)delegate { Form1.form.progressBar1.Value = (int)(((double)reader.BaseStream.Position / (double)reader.BaseStream.Length) * 100); });
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                reader.ReadUInt16();
                UInt16 len = reader.ReadUInt16();
                reader.BaseStream.Seek(-4, SeekOrigin.Current);
                fdLongStorage.Add(new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(reader.ReadBytes(len + 4)));
                Form1.form?.GB_PreviewReport?.Invoke((MethodInvoker)delegate
                {
                    STLV.ShowTree(fdLongStorage.Last(), nodes);
                });
                Form1.form.Invoke((MethodInvoker)delegate { Form1.form.progressBar1.Value = (int)(((double)reader.BaseStream.Position / (double)reader.BaseStream.Length) * 100); });
            }
            Form1.form.Invoke((MethodInvoker)delegate
            {
                Form1.form.progressBar1.Value = 0;
                Form1.form.B_UpdateStop.Text = "Обновить";
            });
        }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ReportFS()
        {

        }

        /// <summary>
        /// Создаёт заголовок с заданными значениями
        /// </summary>
        /// <param name="name">Наименование файла выгрузки </param>
        /// <param name="programm">программа выгрузки</param>
        /// <param name="numberKKT">Номер ККТ</param>
        /// <param name="numberFS">Номер фискального накопителя</param>
        /// <param name="versionFFD">Версия ФФД</param>
        /// <param name="countShift">Количество смен</param>
        /// <param name="fiscalDoc">Количество фискальных документов</param>
        public void InitHeader(string name, string programm, string numberKKT, string numberFS, byte versionFFD, uint countShift, uint fiscalDoc)
        {
            header = new ReportHeader(name, programm, numberKKT, numberFS, versionFFD, countShift, fiscalDoc);
        }

        /// <summary>
        /// Добавить STLV структуру (650XX)
        /// </summary>
        /// <param name="tag">Тег добавляемой структуры</param>
        /// <returns>Добавленная структура типа STLV</returns>
        public Structurs AddValue(UInt16 tag)
        {
            try
            {
                fDLongStorage.Add(new STLV(tag, null));
            }
            catch
            {
                throw new Exception("Произошла непредвиденная ошибка при добавлении значения в структуру отчёта.");
            }
            return fDLongStorage.Last();
        }

        /// <summary>
        /// Запускает процесс записи данных в файл
        /// </summary>
        /// <param name="writer"></param>
        public void WriteFile(string way)
        {
            FileStream fileStream = new FileStream(way, FileMode.Open);
            BinaryWriter writer = new BinaryWriter(fileStream);

            header.WriteFile(writer);
            foreach (var item in fDLongStorage)
            {
                (item as STLV).WriteFile(writer);
            }
            header.AddHesh(writer);
        }
    }

    /// <summary>
    /// Заголовок отчёта о считывании данных из ФН
    /// </summary>
    class ReportHeader
    {
        /// <summary>
        /// Наименование файла выгрузки
        /// </summary>
        String name;
        /// <summary>
        /// Длинна наименования файла выгрузки
        /// </summary>
        const int lenName = 53;
        /// <summary>
        /// Программа выгрузки
        /// </summary>
        String programm;
        /// <summary>
        /// Длинна наименования программы вгрузки
        /// </summary>
        const int lenProgramm = 256;
        /// <summary>
        /// Номер ККТ
        /// </summary>
        String numberKKT;
        /// <summary>
        /// Длинна строки "Номер ККТ"
        /// </summary>
        const int lenNumberKKT = 20;
        /// <summary>
        /// Номер фискального накопителя
        /// </summary>
        String numberFS;
        /// <summary>
        /// Длинна строки "Длинна номера фискального накопителя"
        /// </summary>
        const int lenNumberFS = 16;
        /// <summary>
        /// Версия ФФД
        /// </summary>
        Byte versionFFD;
        /// <summary>
        /// Количество смен
        /// </summary>
        UInt32 countShift;
        /// <summary>
        /// Количество фискальных документов
        /// </summary>
        UInt32 countfiscalDoc;
        /// <summary>
        /// Хеш
        /// </summary>
        UInt32 hash;

        /// <summary>
        /// Конструктор получающий все значения
        /// </summary>
        /// <param name="name">Наименование файла выгрузки </param>
        /// <param name="programm">программа выгрузки</param>
        /// <param name="numberKKT">Номер ККТ</param>
        /// <param name="numberFS">Номер фискального накопителя</param>
        /// <param name="versionFFD">Версия ФФД</param>
        /// <param name="countShift">Количество смен</param>
        /// <param name="fiscalDoc">Количество фискальных документов</param>
        public ReportHeader(string name, string programm, string numberKKT, string numberFS, byte versionFFD, uint countShift, uint fiscalDoc)
        {
            if (name.Length >= lenName)
            {
                this.name = name.Substring(0, lenName);
            }
            else
            {
                this.name += string.Format($"{ name,-lenName}");
            }
            if (programm.Length >= lenProgramm)
            {
                this.programm = programm.Substring(0, lenProgramm);
            }
            else
            {
                this.programm = string.Format($"{programm,-lenProgramm}");
            }
            if (numberKKT.Length >= lenNumberKKT)
            {
                this.numberKKT = numberKKT.Substring(0, lenNumberKKT);
            }
            else
            {
                this.numberKKT = string.Format($"{numberKKT,-lenNumberKKT}");
            }
            if (numberFS.Length >= lenNumberFS)
            {
                this.numberFS = numberFS.Substring(0, lenNumberFS);
            }
            else
            {
                this.numberFS = string.Format($"{numberFS,-lenNumberFS}");
            }
            this.versionFFD = versionFFD;
            this.countShift = countShift;
            this.countfiscalDoc = fiscalDoc;
        }

        /// <summary>
        /// Конструктор считывающий все данные заголовка из потока файла отчёта
        /// </summary>
        /// <param name="reader">Поток файла отчёта</param>
        public ReportHeader(BinaryReader reader)
        {
            //Считывание название документа
            Encoding encoding = Encoding.GetEncoding(866);
            byte[] name = new byte[lenName];
            reader.Read(name, 0, lenName);
            this.name = encoding.GetString(name);
            //Считывание названия программы
            byte[] programm = new byte[lenProgramm];
            reader.Read(programm, 0, lenProgramm);
            this.programm = encoding.GetString(programm);
            //Считывание номера ККТ
            byte[] numberKKT = new byte[lenNumberKKT];
            reader.Read(numberKKT, 0, lenNumberKKT);
            this.numberKKT = encoding.GetString(numberKKT);
            //Считывание номер фискального накопителя
            byte[] numberFS = new byte[lenNumberFS];
            reader.Read(numberFS, 0, lenNumberFS);
            this.numberFS = encoding.GetString(numberFS);
            //Считывания версии ФФД, количества смен, количества фискальных документов и хеша
            this.versionFFD = reader.ReadByte();
            this.countShift = reader.ReadUInt32();
            this.countfiscalDoc = reader.ReadUInt32();
            this.hash = reader.ReadUInt32();
            Form1.form.Invoke((MethodInvoker)delegate
            {
                Form1.form.tabControl1.SelectTab(Form1.form.T_page_headInfo);
                Form1.form.TB_1_saveFile.Text = this.name;
                Form1.form.TB_2_UnloadingProgram.Text = this.programm;
                Form1.form.TB_3_RegNumber.Text = this.numberKKT;
                Form1.form.TB_4_NumberFN.Text = this.numberFS;
                Form1.form.TB_5_NumberFFD.Text = this.versionFFD.ToString();
                Form1.form.TB_6_NumberOfShifts.Text = this.countShift.ToString();
                Form1.form.TB_7_NumberOfFiscalDOC.Text = this.countfiscalDoc.ToString();
                Form1.form.TB_8_CheckSum.Text = this.hash.ToString();
                //Form1.form.treeView1.Nodes.Add("Header");
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_1_saveFile.Text);
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_2_UnloadingProgram.Text = this.programm);
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_3_RegNumber.Text = this.numberKKT);
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_4_NumberFN.Text = this.numberFS);
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_5_NumberFFD.Text = this.versionFFD.ToString());
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_6_NumberOfShifts.Text = this.countShift.ToString());
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_7_NumberOfFiscalDOC.Text = this.countfiscalDoc.ToString());
                //Form1.form.treeView1.Nodes[0].Nodes.Add(Form1.form.TB_8_CheckSum.Text = this.hesh.ToString());
            });
        }

        /// <summary>
        /// Записывает в поток все поля заголовка кроме хеша.
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void WriteFile(BinaryWriter writer)
        {
            try
            {
                writer.Write(Encoding.GetEncoding(866).GetBytes(name));
                writer.Write(Encoding.GetEncoding(866).GetBytes(programm));
                writer.Write(Encoding.GetEncoding(866).GetBytes(numberKKT));
                writer.Write(Encoding.GetEncoding(866).GetBytes(numberFS));
                writer.Write(versionFFD);
                writer.Write(BitConverter.GetBytes(countShift));
                writer.Write(BitConverter.GetBytes(countfiscalDoc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Записывает хеш в файл
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void AddHesh(BinaryWriter writer)
        {
            //считаем хеш
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            Crc32 crc32 = new Crc32();
            byte[] _hash = crc32.ComputeHash(writer.BaseStream);
            Array.Reverse(_hash);
            hash = BitConverter.ToUInt32(_hash,0);
            //копируем в memorystream дерево тегов
            writer.BaseStream.Seek(354, SeekOrigin.Begin);
            MemoryStream memoryStream = new MemoryStream();
            writer.BaseStream.CopyTo(memoryStream);
            //пишем хеш
            writer.BaseStream.Seek(354, SeekOrigin.Begin);
            writer.Write(hash);
            //дописываем дерево тегов
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.CopyTo(writer.BaseStream);
            writer.Close();
            memoryStream.Close();
        }
    }

    /// <summary>
    /// Базовый класс для реализации TLV и STLV структур
    /// </summary>
    class Structurs
    {
        /// <summary>
        /// тип структуры true-считывание из ККТ, false-расшифровка файла.  
        /// </summary>
        protected bool fromKKT = true;

        /// <summary>
        /// STLV структура в которой находится этот объект
        /// </summary>
        Structurs parent;

        /// <summary>
        /// Тег STLV или TLV структуры
        /// </summary>
        UInt16 tag;
        /// <summary>
        /// Длинна структуры.
        /// </summary>
        UInt16 len;

        /// <summary>
        /// Свойство для доступа к длинне STLV или TLV структуре
        /// </summary>
        public UInt16 Len
        {
            get => len;
            set
            {
                if (this.fromKKT)
                {
                    try
                    {
                        UInt16 tmp = (UInt16)(value - this.len);
                        if (parent != null)
                            parent.Len += tmp;
                        this.len = value;
                    }
                    catch
                    {
                        throw new Exception("Милорд, мы не смогли посчитать наши запасы.");
                    }
                }
                else
                {
                    Debug.Write("Не возможно изменять значение длинны при считывании файла.");
                }
            }
        }

        /// <summary>
        /// Тег STLV или TLV структуры
        /// </summary>
        public UInt16 Tag { get => tag; }

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        /// <param name="parent">STLV структура в которою происходит добавление</param>
        public Structurs(UInt16 tag, UInt16 len, Structurs parent)
        {
            this.tag = tag;
            this.len = len;
            this.parent = parent;
            fromKKT = false;//тип - считывание данных из файла
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="parent">STLV структура в которою происходит добавление</param>
        public Structurs(UInt16 tag, Structurs parent)
        {
            this.tag = tag;
            this.parent = parent;
            fromKKT = true;//тип - считывание данных из ККТ
        }
    }
    /// <summary>
    /// Реализация TLV структуры
    /// </summary>
    class TLV : Structurs
    {
        /// <summary>
        /// Значение в TLV структуре
        /// </summary>
        byte[] value;

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        /// <param name="parent">STLV структура в которою добавляется эта TLV структура</param>
        public TLV(UInt16 tag, UInt16 len, Structurs parent) : base(tag, len, parent)
        {
            value = new byte[Len];
        }

        /// <summary>
        /// Формирует ветки в полученной коллекции ветвей данными из полученного TLVWrapper
        /// </summary>
        /// <param name="tlv">Структруа с данными ветви</param>
        /// <param name="nodes">Коллекция ветвей</param>
        public static void ShowTree(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> tlv, TreeNodeCollection nodes = null)
        {
            if (tlv.Value is Byte[] val)
            {
                string str = "";
                foreach (var item in val)
                {
                    str += $"{item,2:X2} ";
                }
                nodes.Add($"{string.Format($"({(int)tlv.Source.Tag})[{tlv.Source.Length}]", -13)}  {str}            [{tlv.Description}]");
            }
            else
            {
                nodes.Add($"{string.Format($"({(int)tlv.Source.Tag})[{tlv.Source.Length}]", -13)}  {tlv.Value.ToString()}            [{tlv.Description}]");
            }
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="parent">Родительная STLV структура.</param>
        public TLV(UInt16 tag, Structurs parent) : base(tag, parent)
        {
            if (parent != null)
                parent.Len += 4;
        }

        /// <summary>
        /// Присваивает значение TLV структуре
        /// </summary>
        /// <param name="value">Значение в виде массива байтов</param>
        /// <returns>0-операция завершилась успешно</returns>
        public void AddValue(byte[] value)
        {
            this.value = value;
            Len = (UInt16)value.Length;
        }

        /// <summary>
        /// Запись в файл тега, длинны и значения
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void WriteFile(BinaryWriter writer)
        {
            try
            {
                writer.Write(Tag);
                writer.Write(Len);
                writer.Write(value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"tag - {Tag}; Exception message: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Реализация STLV структуры
    /// </summary>
    class STLV : Structurs
    {
        /// <summary>
        /// Структуры в составе STLV структур
        /// </summary>
        List<Structurs> value = new List<Structurs>();

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        /// <param name="parent">STLV структура в которою добавляется эта STLV структура</param>
        public STLV(UInt16 tag, UInt16 len, Structurs parent) : base(tag, len, parent)
        {

        }

        /// <summary>
        /// Формирует ветки в полученной коллекции ветвей данными из полученного TLVWrapper
        /// </summary>
        /// <param name="stlv">Структруа с данными ветви</param>
        /// <param name="nodes">Коллекция ветвей</param>
        public static void ShowTree(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> stlv, TreeNodeCollection nodes)
        {
            if (stlv.Source.Tag != Fw16.Model.TLVTag._Anonymous)//Игнорирование анонимного тега
            {
                TreeNode tmp = nodes.Add($"{$"({(int)stlv.Source.Tag})[{stlv.Source.Length}]",-13}            [{stlv.Description}]");
                foreach (var item in stlv.Value as List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                {
                    if (item.Value is List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                    {
                        STLV.ShowTree(item, tmp.Nodes);
                    }
                    else
                    {
                        TLV.ShowTree(item, tmp.Nodes);
                    }
                }

            }
            else
            {
                foreach (var item in stlv.Value as List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                    STLV.ShowTree(item, nodes);
            }
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="parent">STLV структура в которою добавляется эта STLV структура</param>
        public STLV(UInt16 tag, Structurs parent) : base(tag, parent)
        {
            if (parent != null)
                parent.Len += 4;
        }

        /// <summary>
        /// Добавить значение в STLV структуру
        /// </summary>
        /// <param name="tag">Тег добавляемой структуры</param>
        /// <returns>Добавленная структура типа STLV</returns>
        public Structurs AddValue(UInt16 tag)
        {
            try
            {

                if (Program.GetTypeTLV((Fw16.Model.TLVTag)tag) is Fw16.Model.TLVType.STLV)
                {
                    value.Add(new STLV(tag, this));
                }
                else
                {
                    value.Add(new TLV(tag, this));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла непредвиденная ошибка при добавлении значения в структуру отчёта. \nException\n{ex.Message}");
            }
            return value.Last();
        }

        /// <summary>
        /// Запись в файл тега длинны и вызов записи в файл для всех вложенных объектов
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void WriteFile(BinaryWriter writer)
        {
            try
            {
                writer.Write(Tag);
                writer.Write(Len);
                foreach (var item in value)
                {
                    if (item is TLV itemTlv)
                    {
                        itemTlv.WriteFile(writer);
                    }
                    else
                    {
                        (item as STLV).WriteFile(writer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    class TreeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if (ty.Parent == null)
            {
                return -1;
            }

            if ((tx.Parent?.Text ?? "") == "Header")
            {
                return 1;
            }

            return string.Compare(tx.Text, ty.Text);
        }
    }
}
