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
        /// <param name="name"></param>
        /// <param name="programm"></param>
        /// <param name="numberKKT"></param>
        /// <param name="numberFS"></param>
        /// <param name="versionFFD"></param>
        /// <param name="countShift"></param>
        /// <param name="fiscalDoc"></param>
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
        /// Тег STLV или TLV структуры
        /// </summary>
        UInt16 tag;
        /// <summary>
        /// Длинна структуры.
        /// </summary>
        UInt16 len;

        /// <summary>
        /// Массив тегов TLV структур где значение строка
        /// </summary>
        public UInt16[] tlString = {1000, 1048, 1018, 1037, 1036, 1013, 1021, 1203, 1009, 1187, 1060, 1117, 1017, 1046, 1188, 1041, 1226 };
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
        public UInt16[] tlByteMass = { 1077, 304, 301, 1078, 1162,300 };
        /// <summary>
        /// Массив тегов STLV структур
        /// </summary>
        public UInt16[] stlv = { 101, 111, 102, 121, 103, 131, 104, 141, 105, 106, 107, 1, 11, 2, 21, 3, 31, 4, 41, 5, 6, 7, 1059, 1157 };

        /// <summary>
        /// тип структуры true-считывание из ККТ, false-расшифровка файла.  
        /// </summary>
        protected bool type = true;

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

        public int ReadValue(BinaryReader reader, TreeNode node = null)
        {
            reader.Read(value, 0, Len);
            if (node != null)
            {
                node.Text = node.Text + value.ToString();
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
                    if (Form1.form.checkBox1.Checked)
                    {
                        nodes.Add($"({tag})[{len}]");
                        (tlsTmp as TLV).ReadValue(reader, nodes[value.Count]);
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
