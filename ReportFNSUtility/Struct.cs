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
        /// Массив тегов TLV структур
        /// </summary>
        Int16[] tlv = { };
        /// <summary>
        /// Массив тегов STLV структур
        /// </summary>
        Int16[] stlv = { };

        public STLV(short tag, short len)
        {
            this.tag = tag;
            this.len = len;
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
    }

    /// <summary>
    /// Реализация STLV структуры
    /// </summary>
    class TLS : STLV
    {
        STLV structur;

        public TLS(short tag, short len) : base(tag, len)
        {
        }
    }
}
