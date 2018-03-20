using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fw16;
using Fw16.Model;
using System.Runtime.InteropServices;

namespace ReportFNSUtility
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static bool canRewrite = false;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
                Form1 form = new Form1();
                Application.Run(form);
            }
            else
            {
                EcrCtrl ecrCtrl = new Fw16.EcrCtrl();
                string way = "";
                foreach (var item in args)
                {
                    switch (item[0])
                    {
                        case 'P':
                            ecrCtrl.Init(item.Substring(1));
                            break;
                        case 'D':
                            way = item.Substring(1);
                            break;
                        case 'r':
                            if (item == "rw")
                                canRewrite = true;
                            break;
                        default:
                            Console.WriteLine("rw – перезаписать файл  отчёта при совпадении имени\n" +
                                "P < Номер_Порта > -порт подключения к ККТ\n" +
                                "D”< Абсолютный_Путь_К_Директории >” – Путь к директории в которой будет создан файл отчёта.");
                            return;
                    }
                }
                WriteReport writeReport = new WriteReport(ecrCtrl, way);
                writeReport.WriteReportStartParseFNS();
            }


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
            TLVTagInfoAttribute tLV = TLVAttribute.GetCustomAttribute(typeof(TLVTag).GetField(tag.ToString()), typeof(TLVTagInfoAttribute)) as TLVTagInfoAttribute;
            return tLV?.TlvType ?? TLVType.Auto;
        }
    }
}
