using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fw16;
using Fw16.Model;

namespace ReportFNSUtility
{
    static class Program
    {
        public static bool canRewrite = false;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Form1 form = new Form1();
                Application.Run(form);
            }
            else
            {
                EcrCtrl ecrCtrl=new Fw16.EcrCtrl();
                string way="";
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
                            if(item=="rw")
                                canRewrite = true;
                            break;
                        default:
                            Console.WriteLine("");
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
