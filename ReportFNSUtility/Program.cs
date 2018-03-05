﻿using System;
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
            var tLV = TLVAttribute.GetCustomAttribute(typeof(TLVTag).GetField(tag.ToString()), typeof(TLVType));
            byte[] d = BitConverter.GetBytes((Int16)tag);
            byte[] d2 = new byte[12];
            
            d2[0] = d[0];
            d2[1] = d[1];
            d2[2] = 1;
            Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> tLVWrapper = new TLVWrapper<TLVTag>(d2);
            return tLVWrapper.TlvType;
        }
    }
}
