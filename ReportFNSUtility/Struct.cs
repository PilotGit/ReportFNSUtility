using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportFNSUtility
{
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
        Int16[] tlInt= { };
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
        bool type;

        public STLV(short tag, short len)
        {
            this.tag = tag;
            this.len = len;
            type = false;
        }

        public STLV(short tag)
        {
            this.tag = tag;
            type = true;
        }
    }

    /// <summary>
    /// Реализация TLV структуры
    /// </summary>
    class TLV:STLV
    {
        byte[] value;

        public TLV(short tag, short len) : base(tag, len)
        {
        }

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
        STLV[] structur;

        public TLS(short tag, short len) : base(tag, len)
        {

        }

        public TLS(short tag) : base(tag)
        {

        }
    }
}
