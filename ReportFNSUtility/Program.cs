using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fw16.Model;

namespace ReportFNSUtility
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            Application.Run(form);
        }
        /// <summary>
        /// Получить описание enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string _GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field == null)
                return String.Format("<{0}>", value);

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute?.Description;
        }

        internal static TLVType GetTypeTLV(TLVTag tag)
        {
            TLVTagInfoAttribute tLV = TLVAttribute.GetCustomAttribute(typeof(TLVTag).GetField(tag.ToString()),typeof(TLVTagInfoAttribute) ) as TLVTagInfoAttribute;
            return tLV.TlvType;
        }
    }
}
