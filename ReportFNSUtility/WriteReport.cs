using Fw16;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportFNSUtility
{
    class WriteReport
    {
        /// <summary>
        /// Объект для работы с ККТ
        /// </summary>
        public EcrCtrl ecrCtrl;
        /// <summary>
        /// Id фискального накопителя
        /// </summary>
        readonly byte[] FsId;
        /// <summary>
        /// Id OФД
        /// </summary>
        byte[] OfdTaxId;
        uint lastDocNum;
        /// <summary>
        /// статус фискального накопителя
        /// </summary>
        Fs.Native.StatusData statusData;
        uint maxShift=0;
        /// <summary>
        /// фискальные данные 650XX
        /// </summary>
        ushort tagFDn = 65000;
        /// <summary>
        /// Путь к файлу с именем файла
        /// </summary>
        string way;

        Dictionary<uint, Dictionary<uint, byte[]>> dictionary;

        public FileStream fileStream;
        ushort count = 0;
        public BinaryWriter writer;

        /// <summary>
        /// создание объекта для наполнения дерева тегов
        /// </summary>
        /// <param name="ecrCtrl">ссылка на объект работы с ккт</param>
        /// <param name="way">путь к файлу</param>
        /// <param name="fileName">имя файла</param>
        public WriteReport(EcrCtrl ecrCtrl, string way, string fileName = "")
        {
            //Заполнение преременных 
            this.ecrCtrl = ecrCtrl;
            if (ecrCtrl.Fw16.FsDirect.GetFsStatus().LastDocNum > 0)
            {
                statusData = ecrCtrl.Fw16.FsDirect.GetFsStatus();
                FsId = Encoding.GetEncoding(866).GetBytes(statusData.FsId);
                lastDocNum = statusData.LastDocNum;
                way = way == "" ? Application.StartupPath : way;
                if (fileName == "")
                {
                    this.way = way + @"\" + statusData.FsId + "_" + DateTime.Now.ToString("d") + ".fnc";
                }
                else
                {
                    this.way = way + @"\" + (fileName.IndexOf(".") > 0 ? fileName : fileName + ".fnc");
                }
                dictionary = GetDictionaryREG();
            }
            else
            {
                if (Form1.form != null)
                {
                    Form1.form?.Invoke((MethodInvoker)delegate { Form1.form.B_startParse.Text = "Формировать отчет"; });
                    MessageBox.Show("Нет документов для чтения", "Состояние ФН", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Console.WriteLine("Нет документов для чтения");
                }
                (ecrCtrl as IDisposable).Dispose();
                return;
            }
        }

        //ЧУТЬ-ЧУТЬ ПЕРЕГРУЗОК__________________________________________________________________
        /// <summary>
        /// Получение времени в байтах в секундах
        /// </summary>
        /// <param name="dateTime">время из ФН</param>
        /// <returns></returns>
        private byte[] GetByte(DateTime dateTime)
        {
            long unixTimestamp = dateTime.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return BitConverter.GetBytes((int)unixTimestamp);

            //!!!!!!!!!!
            //Возможно произойдет косяк и опять работать с этим. Ибо 5 байт по ТЗ а возвращяю 4
            //!!!!!!!!!!
            //DateTimeOffset origin = dateTime;
            //origin.ToLocalTime();
            //byte[] dt = BitConverter.GetBytes((int)origin.ToUnixTimeSeconds());
            //byte[] dtnew = new byte[] {dt[0], dt[1], dt[2], dt[3] };
            //return dt;
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        private byte[] GetByte(byte dt)
        {
            return new byte[] { dt };
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        private byte[] GetByte(short dt)
        {
            return BitConverter.GetBytes(dt);
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        private byte[] GetByte(ushort dt)
        {
            return BitConverter.GetBytes(dt);
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        private byte[] GetByte(uint dt)
        {
            byte[] bt = (BitConverter.GetBytes(dt));
            return bt;
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        private byte[] GetByte(string dt)
        {
            byte[] bt = Encoding.GetEncoding(866).GetBytes(dt);
            return bt;
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        private byte[] GetByte(ulong dt)
        {
            return BitConverter.GetBytes(dt);
        }
        //↓ странная перегрузка. нужна что бы работал тег 1077
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        private byte[] GetByte(uint dt, byte a = 0, byte c = 0)
        {
            byte[] b = BitConverter.GetBytes(dt);
            b = new byte[] { b[3], b[2], b[1], b[0], a, c };
            Array.Reverse(b);
            return b;
        }
        //конец перегрузок|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        /// <summary>
        /// Получение колекции регистраций ККТ
        /// </summary>
        /// <returns>колекция регистраций</returns>
        private Dictionary<uint, Dictionary<uint, byte[]>> GetDictionaryREG()
        {
            //составить список регистраций. ключ номер фискального докумнета
            var regcount = lastDocNum;
            var dd = new Dictionary<uint, Dictionary<uint, byte[]>>();
            for (byte i = 0; i < regcount + (byte)1; i++)
            {
                //Обновление прогресбара
                UpdateProgressBar(i);
                uint fiscalNumber = 0;
                var ofdTaxId = new Dictionary<uint, byte[]>();
                if (ecrCtrl.Fw16.FsDirect is Fs.Native.IArchive2 arc)
                {
                    arc.BeginReadReg(i);

                    while (arc.NextReadReg(out Fw16.Model.TLV<Fw16.Model.TLVTag> tlv) == Fs.Native.FsAnswer.Success)
                    {
                        ofdTaxId.Add((uint)tlv.Tag, tlv.Value);
                        if (tlv.Tag == Fw16.Model.TLVTag.FiscalNumber)
                        {
                            var w = new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(tlv);
                            if (fiscalNumber == Convert.ToUInt32(w.Value))
                                break;
                            fiscalNumber = Convert.ToUInt32(w.Value);
                        }
                    }
                    try { dd.Add(fiscalNumber, ofdTaxId); }
                    catch (Exception ex) { return dd; }
                }
            }
            return dd;
        }

        /// <summary>
        /// Обновление информации о текущем состояния ККТ
        /// </summary>
        /// <param name="count"></param>
        /// <param name="dictionary"></param>
        void SetCurrentStat(uint count, Dictionary<uint, Dictionary<uint, byte[]>> dictionary)
        {
            tagFDn = 65000;
            try
            {
                if (BitConverter.ToString(dictionary[count][1209]) != "02")
                    tagFDn += 10;
            }
            catch { }
            tagFDn += BitConverter.ToString(dictionary[count][1002]) != "00" ? (ushort)1 : (ushort)2;
            try { OfdTaxId = dictionary[count][1017]; } catch { OfdTaxId = null; }
        }

        /// <summary>
        /// получение поддтверждения документа
        /// </summary>
        /// <param name="fsArc">ссылка на объект IArchive</param>
        /// <param name="count">номер документа</param>
        private byte[] GetAcknowledg(Fs.Native.IArchive fsArc, uint count)
        {
            fsArc.GetAcknowledge(count, out Fs.Native.ArcAck arcAck);
            return GetTLV(((short)107),
                GetTLV(((int)Fw16.Model.TLVTag.OfdTaxId), (this.OfdTaxId)),
                GetTLV(((int)Fw16.Model.TLVTag.DateTime), GetByte(arcAck.DT)),
                GetTLV(((int)Fw16.Model.TLVTag.OfdSignature), arcAck.Signature)
                );
        }

        public string CutString(string str,int len)
        {
            if(str.Length>len)
                str = str.Substring(str.Length-len, len);
            else
            {
                str=str.PadRight(len);
            }
            return str;
        }

        /// <summary>
        /// Записывает в поток все поля заголовка кроме хеша.
        /// </summary>
        /// <param name="writer">Поток записи</param>
        private void WriteHeaderToFile(BinaryWriter writer)
        {
            Stream stream = writer.BaseStream;
            long _pos = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            writer.Write(GetByte(CutString(Directory.GetCurrentDirectory() + @"\" + statusData.FsId + ".fnc", 53)));
            writer.Write(GetByte(CutString(Program.nameProgram, 256)));
            writer.Write(dictionary[1][1037]);
            writer.Write(GetByte(CutString(statusData.FsId, 16)));
            writer.Write(ByteArrayMerging(GetByte((byte)ecrCtrl.Info.FfdVersion), GetByte(maxShift), GetByte(lastDocNum)));
            stream.Seek(_pos, SeekOrigin.Begin);
        }

        /// <summary>
        /// Записывает хеш в файл
        /// </summary>
        /// <param name="writer">Поток записи</param>
        private void AddHeshToFile(BinaryWriter writer)
        {
            WriteHeaderToFile(writer);
            //считаем хеш
            uint hash = ReportFNS.ReportHeader.ComputeHesh(writer.BaseStream);
            //копируем в memorystream дерево тегов
            writer.BaseStream.Seek(354, SeekOrigin.Begin);
            MemoryStream memoryStream = new MemoryStream();
            writer.BaseStream.CopyTo(memoryStream);
            //пишем хеш
            writer.BaseStream.Seek(354, SeekOrigin.Begin);
            writer.Write(hash);
            //дописываем дерево тегов
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.CopyTo(writer.BaseStream);
            writer.Close();
            memoryStream.Close();
        }

        /// <summary>
        /// установка прогрес бара в положение полученного значения относительно всех докуметов
        /// </summary>
        /// <param name="i">значение</param>
        private void UpdateProgressBar(int i)
        {
            Form1.form?.Invoke((MethodInvoker)delegate { Form1.form.progressBar1.Value = (int)(((double)(i + 1) / (double)lastDocNum) * 100); });
            if (Form1.form == null)
            {
                if (((double)(i + 1) / (double)lastDocNum) * 40 > count)
                {
                    count++;
                    Console.Write("|");
                }
            }
        }

        /// <summary>
        /// копирование всех массивов в один
        /// </summary>
        /// <param name="aray">массив</param>
        /// <returns>массив полученных массивов</returns>
        public byte[] ByteArrayMerging(params byte[][] aray)
        {
            int length = 0;
            aray=aray.Where(x => x != null).ToArray();
            foreach (var item in aray)
            {
                length += item.Length;
            }
            byte[] newArray = new byte[length];
            length = 0;
            foreach (var item in aray)
            {
                item.CopyTo(newArray, length);
                length += item.Length;
            }
            return newArray;
        }

        /// <summary>
        /// создание массива по принципу TLV
        /// </summary>
        /// <param name="tag">тег</param>
        /// <param name="BIGBytearray">Значение</param>
        /// <returns>TLV</returns>
        public byte[] GetTLV(short tag, params byte[][] BIGBytearray)
        {
            byte[] bytearray = ByteArrayMerging(BIGBytearray);
            byte[] TLVarray = new byte[bytearray.Length + 4];
            GetByte(tag).CopyTo(TLVarray, 0);
            GetByte((short)bytearray.Length).CopyTo(TLVarray, 2);
            bytearray.CopyTo(TLVarray, 4);
            return TLVarray;
        }

        //~~~~~~~Кусок с форматированием документов в TLV
        private byte[] ToTLVArcRegChange(uint i, Fs.Native.IArchive fsArc, Fs.Native.ArchiveDoc ad)
        {
            byte[] TLV = null;
            ///ПЕРЕРЕГИСТРАЦИЯ
            /// ↓
            if (ad.Data is Fs.Native.ArcRegChange rch)
            {
                ///!!!!!
                ///важно обновить информацию о текущей регистрации 
                SetCurrentStat(i + 1, dictionary);
                Dictionary<uint, byte[]> dictionaryREG;
                dictionary.TryGetValue(i + 1, out dictionaryREG);
                foreach (var v in dictionaryREG)
                {
                    TLV = ByteArrayMerging(TLV, GetTLV((short)v.Key, v.Value));
                }
                if (ad.Acknowledged)
                    ByteArrayMerging(TLV, GetAcknowledg(fsArc, i + 1));
                TLV = GetTLV((short)(ad.TlvTag + 100),
                    GetTLV((short)(ad.TlvTag), TLV
                        )
                    );
            }
            return TLV;
        }
        private byte[] ToTLVArcReg(uint i, Fs.Native.IArchive fsArc, Fs.Native.ArchiveDoc ad)
        {
            byte[] TLV = null;
            ///РЕГИСТРАЦИЯ
            /// ↓
            if (ad.Data is Fs.Native.ArcReg reg)
            {
                ///!!!!!
                ///важно обновить информацию о текущей регистрации 
                SetCurrentStat(i + 1, dictionary);
                Dictionary<uint, byte[]> dictionaryREG;
                dictionary.TryGetValue(i + 1, out dictionaryREG);
                foreach (var v in dictionaryREG)
                {
                    TLV = ByteArrayMerging(TLV, GetTLV((short)v.Key, v.Value));
                }
                if (ad.Acknowledged)
                    ByteArrayMerging(TLV, GetAcknowledg(fsArc, i + 1));
                TLV = GetTLV((short)(ad.TlvTag + 100),
                    GetTLV((short)(ad.TlvTag), TLV
                        )
                    );
            }
            return TLV;
        }
        private byte[] ToTLVArcReceipt(uint i, Fs.Native.IArchive fsArc, Fs.Native.ArchiveDoc ad)
        {
            byte[] TLV = null;
            if (ad.Data is Fs.Native.ArcReceipt rcpt)     //ЭТО ЧЕК!_! 103
            {
                TLV = GetTLV((short)(ad.TlvTag + 100),
                    GetTLV((int)Fw16.Model.TLVTag.DateTime, GetByte(rcpt.Freq.DT)),
                    GetTLV((int)Fw16.Model.TLVTag.Operation, GetByte((byte)rcpt.Operation)),
                    GetTLV((int)Fw16.Model.TLVTag.Total, GetByte((rcpt.Total))),
                    GetTLV((int)Fw16.Model.TLVTag.FiscalNumber, GetByte((rcpt.Freq.FiscalNumber))),
                    GetTLV((int)Fw16.Model.TLVTag.FsId, this.FsId),
                    GetTLV((int)Fw16.Model.TLVTag.FsSignature, GetByte(rcpt.Freq.FiscalSignature, 0)),
                    GetAcknowledg(fsArc, i)
                    );
            }
            return TLV;
        }
        private byte[] ToTLVArcShift(uint i, Fs.Native.IArchive fsArc, Fs.Native.ArchiveDoc ad)
        {
            byte[] TLV = null;
            ///ОТКРЫТИЕ\ЗАКРЫТИЕ СМЕНЫ
            /// ↓
            if (ad.Data is Fs.Native.ArcShift shift) //Закрытие\откытие смены 102\105
            {
                //добавление 1 в счетчик смен если это открытие смены
                if ((ushort)(ad.TlvTag + 100) == 102)
                    maxShift++;
                TLV = GetTLV((short)(ad.TlvTag + 100),
                    GetTLV((int)Fw16.Model.TLVTag.DateTime, GetByte(shift.Freq.DT)),
                    GetTLV((int)Fw16.Model.TLVTag.ShiftNumber, GetByte((uint)shift.Number)),
                    GetTLV((int)Fw16.Model.TLVTag.FiscalNumber, GetByte(shift.Freq.FiscalNumber)),
                    GetTLV((int)Fw16.Model.TLVTag.FsSignature, GetByte(shift.Freq.FiscalSignature, 0)),
                    GetAcknowledg(fsArc, i)
                    );
            }
            return TLV;
        }
        private byte[] ToTLVArcReport(uint i, Fs.Native.IArchive fsArc, Fs.Native.ArchiveDoc ad)
        {
            byte[] TLV = null;
            //ОТЧЕТ О СОСТОЯНИИ ФН
            /// ↓
            if (ad.Data is Fs.Native.ArcReport arcReport)
            {
                TLV = GetTLV((short)(ad.TlvTag + 100),
                      GetTLV((int)Fw16.Model.TLVTag.DateTime, GetByte(arcReport.Freq.DT)),
                      GetTLV((int)Fw16.Model.TLVTag.NoAckCounter, GetByte(arcReport.NoAckCounter)),
                      GetTLV((int)Fw16.Model.TLVTag.FirstNoAckDate, GetByte(arcReport.FirstNoAckDate)),
                      GetTLV((int)Fw16.Model.TLVTag.FiscalNumber, GetByte(arcReport.Freq.FiscalNumber)),
                      GetTLV((int)Fw16.Model.TLVTag.FsSignature, GetByte(arcReport.Freq.FiscalSignature, 0)),
                      GetAcknowledg(fsArc, i)
                      );
            }
            return TLV;
        }
        private byte[] ToTLVArcCloseFs(uint i, Fs.Native.IArchive fsArc, Fs.Native.ArchiveDoc ad)
        {
            byte[] TLV = null;
            //ОТЧЕТ О ЗАКРЫТИИ ФН
            /// ↓
            if (ad.Data is Fs.Native.ArcCloseFs arcCloseFs)
            {
                TLV = GetTLV((short)(ad.TlvTag + 100), GetTLV((short)(ad.TlvTag),
                      GetTLV((int)Fw16.Model.TLVTag.FsId, this.FsId),
                      GetTLV((int)Fw16.Model.TLVTag.RegId, GetByte(arcCloseFs.RegId)),
                      GetTLV((int)Fw16.Model.TLVTag.OwnerTaxId, dictionary.Values.Last()[1018]),
                      GetTLV((int)Fw16.Model.TLVTag.FiscalNumber, GetByte(arcCloseFs.Freq.FiscalNumber)),
                      GetTLV((int)Fw16.Model.TLVTag.DateTime, GetByte(arcCloseFs.Freq.DT)),
                      GetTLV((int)Fw16.Model.TLVTag.FsSignature, GetByte(arcCloseFs.Freq.FiscalSignature, 0)),
                      GetTLV((int)Fw16.Model.TLVTag.t1209, dictionary.Values.Last()[1209]),
                      GetTLV((int)Fw16.Model.TLVTag.Operator, dictionary.Values.Last()[1021]),
                      GetTLV((int)Fw16.Model.TLVTag.OwnerName, dictionary.Values.Last()[1048]),
                      GetTLV((int)Fw16.Model.TLVTag.Location, dictionary.Values.Last()[1187]),
                      GetTLV((int)Fw16.Model.TLVTag.LocationAddress, dictionary.Values.Last()[1009]),
                      GetTLV((int)Fw16.Model.TLVTag.ShiftNumber, GetByte(maxShift)),
                      GetAcknowledg(fsArc, i)
                      ));
            }

            return TLV;
        }
        //~~~~~~~Конец куска с форматированием докуметов в TLV

        /// <summary>
        /// запись обработанного документа в поток
        /// </summary>
        /// <param name="i">номер документа</param>
        private void AddNewDocument(uint i)
        {
            //обновление прогресбара
            UpdateProgressBar((int)i);

            //Обращение к документам длительного хранения
            if (ecrCtrl.Fw16.FsDirect is Fs.Native.IArchive fsArc)
            {
                if (fsArc.GetDocument(i + 1, out Fs.Native.ArchiveDoc ad) == Fs.Native.FsAnswer.Success)
                {
                    if (ad.Data is Fs.Native.ArcRegChange ArcRegChange)
                        writer.Write(GetTLV((short)tagFDn, ToTLVArcRegChange(i, fsArc, ad)));
                    else if (ad.Data is Fs.Native.ArcReg ArcReg)
                        writer.Write(GetTLV((short)tagFDn, ToTLVArcReg(i, fsArc, ad)));
                    else if (ad.Data is Fs.Native.ArcReceipt ArcReceipt)
                        writer.Write(GetTLV((short)tagFDn, ToTLVArcReceipt(i, fsArc, ad)));
                    else if (ad.Data is Fs.Native.ArcShift ArcShift)
                        writer.Write(GetTLV((short)tagFDn, ToTLVArcShift(i, fsArc, ad)));
                    else if (ad.Data is Fs.Native.ArcReport ArcReport)
                        writer.Write(GetTLV((short)tagFDn, ToTLVArcReport(i, fsArc, ad)));
                    else if (ad.Data is Fs.Native.ArcCloseFs ArcCloseFs)
                        writer.Write(GetTLV((short)tagFDn, ToTLVArcCloseFs(i, fsArc, ad)));
                    else
                    {
                        MessageBox.Show(ad.TlvTag.ToString() + " не имеет определения для записи в файл", "Пропущен архивный документ");
                    }
                }
            }
        }
        
        /// <summary>
        /// проверка возможности обработки файла
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        private FileStream TryGetWayToWrite()
        {
            FileStream fileStream = null;
            try
            {
                if (Program.canRewrite != false && Form1.form != null)
                    return fileStream = new FileStream(way, FileMode.CreateNew);
                else if (Program.canRewrite != false)
                    return fileStream = new FileStream(way, FileMode.Create);
                else
                {
                    Console.WriteLine("Файл не может быть перезаписан");
                    return null;
                }
            }
            catch
            {
                if (MessageBox.Show("Файл существует. Хотите перезаписать файл?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    return fileStream = new FileStream(way, FileMode.Create);
            }
                File.Delete(way);
                (ecrCtrl as IDisposable).Dispose();
                ecrCtrl = new EcrCtrl();
                fileStream?.Close();
                Form1.form.B_startParse.Enabled = true;
                Form1.form.Invoke((MethodInvoker)delegate { Form1.form.B_startParse.Text = "Формировать отчет"; });
                return null;
        }

        /// <summary>
        /// Запуск создания отчета
        /// </summary>
        public void WriteReportStartParseFNS()
        {
            //выход из метода если не получили путь к файлу
            if ((fileStream = TryGetWayToWrite()) != null)
            {
                ///Обновление информации о состоянии ККТ
                SetCurrentStat(1, dictionary);
                //вывод строки закгрузки
                if (Form1.form == null)
                {
                    Console.WriteLine("[||||||||||||||||Обработка|||||||||||||]");
                }
                //Открытие потока для записи
                fileStream.Close();
                fileStream = new FileStream(way, FileMode.Open);
                writer = new BinaryWriter(fileStream);
                //Написание заголовка
                writer.Write("".PadLeft(354).ToCharArray());
                //Чтение всех документов
                for (uint i = 0; i < lastDocNum; i++)
                {
                    AddNewDocument(i);
                }
                //Дописывание хеша
                AddHeshToFile(writer);
                fileStream.Close();
                //создание нового объекта для работы с ККТ
                (ecrCtrl as IDisposable).Dispose();
                if (Form1.form == null)
                    Console.WriteLine("\nКонец обработки. файл сохранён в {0}", way);
                else
                    Form1.form?.Invoke((MethodInvoker)delegate
                    {
                        Form1.form.TB_Patch.Text = way;
                        if (MessageBox.Show("Конец обработки. файл сохранён в: " + way + "\nОткрыть файл для проверки?", "Отчет",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Form1.form.B_UpdateStop_Click(Form1.form.B_UpdateStop, new EventArgs());
                        }
                        Form1.form.progressBar1.Value = 0;
                        Form1.form.B_startParse.Text = "Формировать отчет";
                    });
            }
        }
    }
}
