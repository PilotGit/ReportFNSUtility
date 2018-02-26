using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        STLV[] fDLongStorage;

        /// <summary>
        /// Уонструктор формирующий получающий данные из файлового потока отчёта
        /// </summary>
        /// <param name="reader">Поток файла отчёта</param>
        public ReportFS(BinaryReader reader)
        {
            header = new ReportHeader(reader);
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
            byte[] NumberKKT = new byte[lenNumberKKT];
            reader.Read(NumberKKT, 0, lenNumberKKT);
            this.numberKKT = encoding.GetString(NumberKKT);
            //Считывание номер фискального накопителя
            byte[] numberFS = new byte[lenNumberFS];
            reader.Read(numberFS, 0, lenNumberFS);
            this.numberFS = encoding.GetString(numberFS);
            //Считывания версии ФФД, количества смен, количества фискальных документов и хеша
            this.versionFFD = reader.ReadByte();
            this.countShift = reader.ReadUInt32();
            this.countfiscalDoc = reader.ReadUInt32();
            this.hesh = reader.ReadUInt32();
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
        Int16 tag;
        /// <summary>
        /// Длинна структуры.
        /// </summary>
        Int16 len;

        /// <summary>
        /// Массив тегов TLV структур где значение строка
        /// </summary>
        Int16[] tlString = { };
        /// <summary>
        /// Массив тегов TLV структур где значение число
        /// </summary>
        Int16[] tlInt = { };
        /// <summary>
        /// Массив тегов TLV структур где значение массив битов
        /// </summary>
        Int16[] tlBit = { };
        /// <summary>
        /// Массив тегов TLV структур где значение дата и время
        /// </summary>
        Int16[] tlUnixTime = { };
        /// <summary>
        /// Массив тегов TLV структур где значение массив байтов
        /// </summary>
        Int16[] tlByteMass = { };
        /// <summary>
        /// Массив тегов STLV структур
        /// </summary>
        Int16[] stlv = { };

        /// <summary>
        /// тип структуры true-считывание из ККТ, false-расшифровка файла.  
        /// </summary>
        protected bool type = true;

        /// <summary>
        /// Свойство для доступа к длинне STLV или TLV структуре
        /// </summary>
        public Int16 Len
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
        protected Int16 currentByte = 0;

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        public STLV(short tag, short len)
        {
            this.tag = tag;
            this.len = len;
            type = false;
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        public STLV(short tag)
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
        public TLV(short tag, short len) : base(tag, len)
        {

        }

        public int ReadValue()
        {
            value = /*Считываниезначения из файла value длинны len*/value;
            return 0;
        }


        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        public TLV(short tag) : base(tag)
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
        STLV[] value;

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        public TLS(short tag, short len) : base(tag, len)
        {

        }

        public int ReadValue()
        {
            value = /*Считываниезначения из файла*/value;
            return 0;
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        public TLS(short tag) : base(tag)
        {

        }
    }
}
