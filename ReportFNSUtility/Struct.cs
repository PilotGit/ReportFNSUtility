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
        List<STLV> fDLongStorage = new List<STLV>();

        /// <summary>
        /// Уонструктор формирующий получающий данные из файлового потока отчёта
        /// </summary>
        /// <param name="reader">Поток файла отчёта</param>
        public ReportFS(BinaryReader reader)
        {
            header = new ReportHeader(reader);
            TreeNodeCollection nodes = Form1.form.treeView1.Nodes;
            var progressBar = Form1.form.progressBar1;
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                progressBar.Value = (int)(((double)reader.BaseStream.Position / (double)reader.BaseStream.Length) * 100);
                progressBar.Refresh();
                UInt16 tag = reader.ReadUInt16();
                UInt16 len = reader.ReadUInt16();
                nodes.Add($"({tag})[{len}]");
                TLS tlsTmp = new TLS(tag, len);
                fDLongStorage.Add(tlsTmp);
                tlsTmp.ReadValue(reader, nodes[fDLongStorage.Count].Nodes);
            }
            progressBar.Value = 100;
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
            this.name = name;
            this.programm = programm;
            this.numberKKT = numberKKT;
            this.numberFS = numberFS;
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
            Form1.form.treeView1.Nodes.Add("Header");
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.name);
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.programm);
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.numberKKT);
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.numberFS);
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.numberFS);
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.versionFFD.ToString());
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.countShift.ToString());
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.countfiscalDoc.ToString());
            Form1.form.treeView1.Nodes[0].Nodes.Add(this.hesh.ToString());

        }
    }
    /// <summary>
    /// Базовый класс для реализации TLV и STLV структур
    /// </summary>
    class STLV
    {

        /// <summary>
        /// Массив тегов TLV структур где значение строка
        /// </summary>
        public UInt16[] tlString = { 1000, 1048, 1018, 1037, 1036, 1013, 1021, 1203, 1009, 1187, 1060, 1117, 1017, 1046, 1188, 1041, 1226 };
        /// <summary>
        /// Массив тегов TLV структур где значение число
        /// </summary>
        public UInt16[] tlInt = { 1029, 1062, 1002, 1221, 1110, 1056, 1001, 1108, 1207, 1109, 1193, 1126, 1189, 1190, 1213, 1040, 1101, 1209, 1038, 1054, 1214, 1212, 1097 };
        /// <summary>
        /// Массив тегов TLV структур где значение число c фиксированной точкой
        /// </summary>
        public UInt16[] tlDouble = { 1020, 1031, 1081, 1215, 1216, 1217, 1102, 1103, 1104, 1105, 1106, 1107, 1043, 1023 }; //1023-Пл. точка FVLN 
        /// <summary>
        /// Массив тегов TLV структур где значение массив битов
        /// </summary>
        public UInt16[] tlBit = { 1057, 1205, 1222 };
        /// <summary>
        /// Массив тегов TLV структур где значение дата и время
        /// </summary>
        public UInt16[] tlUnixTime = { 1012, 1098 };
        /// <summary>
        /// Массив тегов TLV структур где значение массив байтов
        /// </summary>
        public UInt16[] tlByteMass = { 1077, 304, 301, 1078, 1162, 300 };
        /// <summary>
        /// Массив тегов STLV структур
        /// </summary>
        public UInt16[] stlv = { 101, 111, 102, 121, 103, 131, 104, 141, 105, 106, 107, 1, 11, 2, 21, 3, 31, 4, 41, 5, 6, 7, 1059, 1157 };

        /// <summary>
        /// тип структуры true-считывание из ККТ, false-расшифровка файла.  
        /// </summary>
        protected bool type = true;

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
                    this.len = value;
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
        public ushort Tag { get => tag; }

        /// <summary>
        /// текущая позиция считывания относительного этого блока (Необходим для проверки превышения длинны)
        /// </summary>
        protected UInt16 currentByte = 0;

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        public STLV(UInt16 tag, UInt16 len)
        {
            this.tag = tag;
            this.len = len;
            type = false;
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        public STLV(UInt16 tag)
        {
            this.tag = tag;
            type = true;
        }

    }

    /// <summary>
    /// Реализация TLV структуры
    /// </summary>
    class TLV : STLV
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
        public TLV(UInt16 tag, UInt16 len) : base(tag, len)
        {
            value = new byte[Len];
        }
        /// <summary>
        /// Считывает значение из потока и добавляет ветвь, если былапередана коллекция ветвей
        /// </summary>
        /// <param name="reader">Бинарный поток чтения</param>
        /// <param name="node">Коллекция ветвей, в которую будет добавлена новая ветвь</param>
        /// <returns></returns>
        public int ReadValue(BinaryReader reader, TreeNodeCollection node = null)
        {
            reader.Read(value, 0, Len);
            //Если была передана коллекция ветвей илёт добавление ветки
            if (node != null)
            {
                Encoding encoding = Encoding.GetEncoding(866);
                if (Array.IndexOf(tlByteMass, Tag) != -1)       //Сверка тега с массивом тегов для преобразования в массив битов
                {
                    string s = "";
                    foreach (var item in value)
                    {
                        s += $"{item:X} ";
                    }
                    node.Add($"({Tag})[{Len}] {s}");
                }
                if (Array.IndexOf(tlString, Tag) != -1)         //Сверка тега с массивом тегов для преобразования в строку
                {
                    node.Add($"({Tag})[{Len}] {encoding.GetString(value)}");
                }
                if (Array.IndexOf(tlUnixTime, Tag) != -1)       //Сверка тега с массивом тегов для преобразования в дату и время
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(BitConverter.ToUInt32(value, 0));
                    node.Add($"({Tag})[{Len}]  {(date).ToString("dd:MM:yyyy HH:mm:ss")}");
                }
                if (Array.IndexOf(tlInt, Tag) != -1)            //Сверка тега с массивом тегов для преобразования в целое число
                {
                    switch (Len)
                    {
                        case 1: node.Add($"({Tag})[{Len}]  {value[0]}"); break;
                        case 2: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt16(value, 0)}"); break;
                        case 4: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt32(value, 0)}"); break;
                        case 8: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt64(value, 0)}"); break;
                        default:
                            break;
                    }
                }
                if (Array.IndexOf(tlDouble, Tag) != -1)         //Сверка тега с массивом тегов для преобразования в дробное число с 2 знаками после зпт
                {
                    switch (Len)
                    {
                        case 1: node.Add($"({Tag})[{Len}]  {value[0] / 100d}"); break;
                        case 2: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt16(value, 0) / 100d}"); break;
                        case 3: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt32(new byte[] { 0, value[0], value[1], value[2] }, 0) / 100d}"); break;
                        case 4: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt32(value, 0) / 100d}"); break;
                        case 5: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt64(new byte[] { 0, 0, 0, value[0], value[1], value[2], value[3], value[4] }, 0) / 100d}"); break;
                        case 6: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt64(new byte[] { 0, 0, value[0], value[1], value[2], value[3], value[4], value[5] }, 0) / 100d}"); break;
                        case 7: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt64(new byte[] { 0, value[0], value[1], value[2], value[3], value[4], value[5], value[6] }, 0) / 100d}"); break;
                        case 8: node.Add($"({Tag})[{Len}]  {BitConverter.ToUInt64(value, 0) / 100d}"); break;
                    }
                }
                if (Array.IndexOf(tlBit, Tag) != -1)            //Сверка тега с массивом тегов для преобразования в массив битов
                {
                    node.Add($"({Tag})[{Len}]  {Convert.ToString(value[0])}");
                }
            }
            return 0;
        }


        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        public TLV(UInt16 tag) : base(tag)
        {
        }

    }

    /// <summary>
    /// Реализация STLV структуры
    /// </summary>
    class TLS : STLV
    {
        /// <summary>
        /// Структуры в составе STLV структур
        /// </summary>
        List<STLV> value = new List<STLV>();

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        public TLS(UInt16 tag, UInt16 len) : base(tag, len)
        {

        }

        /// <summary>
        /// Считыват значение из переданного потока чтения длины этого объекта, добавляя ветвь в дерево
        /// </summary>
        /// <param name="reader">Поток чтения</param>
        /// <param name="nodes">Коллеция ветвей в которую будут добавлены новые ветви</param>
        /// <returns></returns>
        public int ReadValue(BinaryReader reader, TreeNodeCollection nodes)
        {
            long endPosition = reader.BaseStream.Position + Len;
            while (reader.BaseStream.Position != endPosition)
            {
                UInt16 tag = reader.ReadUInt16();
                UInt16 len = reader.ReadUInt16();
                STLV tlsTmp;
                if (Array.IndexOf(this.stlv, tag) != -1)
                {
                    nodes.Add($"({tag})[{len}]");
                    tlsTmp = new TLS(tag, len);
                    (tlsTmp as TLS).ReadValue(reader, nodes[value.Count].Nodes);
                }
                else
                {
                    tlsTmp = new TLV(tag, len);
                    if (Form1.form.ChB_VisibleValue.Checked)
                    {
                        (tlsTmp as TLV).ReadValue(reader, nodes);
                    }
                    else
                    {
                        (tlsTmp as TLV).ReadValue(reader);
                    }
                }
                value.Add(tlsTmp);
            }
            return 0;
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        public TLS(UInt16 tag) : base(tag)
        {

        }
    }
}
