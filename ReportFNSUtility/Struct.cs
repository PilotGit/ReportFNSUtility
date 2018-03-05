using System;
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
        /// Уонструктор формирующий получающий данные из файлового потока отчёта
        /// </summary>
        /// <param name="reader">Поток файла отчёта</param>
        public ReportFS(BinaryReader reader)
        {
            header = new ReportHeader(reader);
            TreeNodeCollection nodes = Form1.form.treeView1.Nodes;
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                Form1.form.Invoke((MethodInvoker)delegate { Form1.form.progressBar1.Value = (int)(((double)reader.BaseStream.Position / (double)reader.BaseStream.Length) * 100); });
                reader.ReadUInt16();
                UInt16 len = reader.ReadUInt16();
                reader.BaseStream.Seek(-4, SeekOrigin.Current);
                fdLongStorage.Add(new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(reader.ReadBytes(len + 4)));
            }
            Form1.form.Invoke((MethodInvoker)delegate { Form1.form.progressBar1.Value = (int)(((double)reader.BaseStream.Position / (double)reader.BaseStream.Length) * 100); });
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

        public void WriteFile(BinaryWriter writer)
        {
            header.WriteFile(writer);
            header.AddHesh(writer);
            foreach (var item in fDLongStorage)
            {
                (item as STLV).WriteFile(writer);
            }
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
        const int lenName = 53;
        /// <summary>
        /// программа выгрузки
        /// </summary>
        String programm;
        const int lenProgramm = 256;
        /// <summary>
        /// Номер ККТ
        /// </summary>
        String numberKKT;
        const int lenNumberKKT = 20;
        /// <summary>
        /// Номер фискального накопителя
        /// </summary>
        String numberFS;
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
        UInt32 hesh;

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
                this.name += string.Format($"{ name,lenName}");
            }
            if (programm.Length >= lenProgramm)
            {
                this.programm = programm.Substring(0, lenProgramm);
            }
            else
            {
                this.programm = string.Format($"{programm,lenProgramm}");
            }
            if (numberKKT.Length >= lenNumberKKT)
            {
                this.numberKKT = numberKKT.Substring(0, lenNumberKKT);
            }
            else
            {
                this.numberKKT = string.Format($"{numberKKT,lenNumberKKT}");
            }
            if (numberFS.Length >= lenNumberFS)
            {
                this.numberFS = numberFS.Substring(0, lenNumberFS);
            }
            else
            {
                this.numberFS = string.Format($"{numberFS,lenNumberFS}");
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
            this.hesh = reader.ReadUInt32();
            Form1.form.Invoke((MethodInvoker)delegate
            {
                Form1.form.treeView1.Nodes.Add("Header");
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.name);
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.programm);
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.numberKKT);
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.numberFS);
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.versionFFD.ToString());
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.countShift.ToString());
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.countfiscalDoc.ToString());
                Form1.form.treeView1.Nodes[0].Nodes.Add(this.hesh.ToString());
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
                writer.Write(countShift);
                writer.Write(countfiscalDoc);
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
            writer.Write(hesh);
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
        protected bool type = true;

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
                if (this.type)
                {
                    try
                    {
                        if (parent != null)
                            parent.Len += value;
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
            type = false;
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
            type = true;
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
        /// Считывает значение из потока и добавляет ветвь, если былапередана коллекция ветвей
        /// </summary>
        /// <param name="reader">Бинарный поток чтения</param>
        /// <param name="nodes">Коллекция ветвей, в которую будет добавлена новая ветвь</param>
        /// <returns></returns>
        public static void ReadValue(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> tlv, TreeNodeCollection nodes = null)
        {
            Form1.form.Invoke((MethodInvoker)delegate
            {
                nodes.Add($"({tlv.Source.Tag})[{tlv.Source.Length}]\"{tlv.Description}\" {tlv.Value.ToString()}");
            });
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="parent">STLV структура в которою добавляется эта TLV структура</param>
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
                throw ex;
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
        /// Считыват значение из переданного потока чтения длины этого объекта, добавляя ветвь в дерево
        /// </summary>
        /// <param name="reader">Поток чтения</param>
        /// <param name="nodes">Коллеция ветвей в которую будут добавлены новые ветви</param>
        /// <returns></returns>
        public void ReadValue(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> stlv, TreeNodeCollection nodes)
        {
            nodes.Add($"({stlv.Source.Tag})[{stlv.Source.Length}]\"{stlv.Description}\"");
            if (stlv.Value is List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>> list)
            {

                foreach (var item in list)
                {
                    ReadValue(item, nodes[nodes.Count - 1].Nodes);
                }
            }
            else
            {
                TLV.ReadValue(stlv.Value as Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>, nodes[nodes.Count - 1].Nodes);
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
            catch
            {
                throw new Exception("Произошла непредвиденная ошибка при добавлении значения в структуру отчёта.");
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
}
