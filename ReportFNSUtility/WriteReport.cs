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
        /// Отчёт о счиывании данных с фискального накопителя
        /// </summary>
        public ReportFS reportFS;
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
        uint maxShift;
        /// <summary>
        /// фискальные данные 650XX
        /// </summary>
        ushort tagFDn = 65001;
        /// <summary>
        /// Путь к файлу с именем файла
        /// </summary>
        string way;
        /// <summary>
        /// отчеты о регистрации ФН
        /// </summary>
        Dictionary<uint, Dictionary<uint, byte[]>> dictionary;

        BinaryWriter writer;
        public FileStream fileStream;
        ushort count = 0;

        public WriteReport() { }
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
                reportFS = new ReportFS();
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
        }
        //ЧУТЬ-ЧУТЬ ПЕРЕГРУЗОК__________________________________________________________________
        /// <summary>
        /// Получение времени в байтах в секундах
        /// </summary>
        /// <param name="dateTime">время из ФН</param>
        /// <returns></returns>
        public byte[] GetByte(DateTime dateTime)
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
        public byte[] GetByte(byte dt)
        {
            return new byte[] { dt };
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        public byte[] GetByte(ushort dt)
        {
            return BitConverter.GetBytes(dt);
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        public byte[] GetByte(uint dt)
        {
            byte[] bt = (BitConverter.GetBytes(dt));
            return bt;
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        public byte[] GetByte(ulong dt)
        {
            return BitConverter.GetBytes(dt);
        }
        /// <summary>
        /// отправить данные получть массив байтов. функция перегружена
        /// </summary>
        /// <param name="dt">данные</param>
        /// <returns>массив байтов</returns>
        public byte[] GetByte(uint dt, byte a = 0, byte c = 0)
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
        public Dictionary<uint, Dictionary<uint, byte[]>> GetDictionaryREG()
        {
            //составить список регистраций. ключ номер фискального докумнета
            var regcount = lastDocNum;
            var dd = new Dictionary<uint, Dictionary<uint, byte[]>>();
            for (byte i = 0; i < regcount + (byte)1; i++)
            {
                //Обновление прогресбара
                Form1.form?.Invoke((MethodInvoker)delegate { Form1.form.progressBar1.Value = (int)(((double)(i) / ((double)lastDocNum + 1)) * 100); });
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
        /// Заполнение полей объекта для заполнения полей требующих данных из регистрации 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="dictionary"></param>
        void SetCurrentStatic(uint count, Dictionary<uint, Dictionary<uint, byte[]>> dictionary)
        {
            tagFDn = 65000;
            try
            {
                if (BitConverter.ToString(dictionary[count][1209]) != "02")
                    tagFDn += 10;
            }
            catch { }
            tagFDn += BitConverter.ToString(dictionary[count][1002]) != "00" ? (ushort)1 : (ushort)2;
            try { OfdTaxId = dictionary[count][1017]; } catch { }
        }
        /// <summary>
        /// получени поддтверждения документа
        /// </summary>
        /// <param name="fsArc">ссылка на объект IArchive</param>
        /// <param name="currentARC">ссылка на объект в который добавляем тег подтвердждения</param>
        /// <param name="count">номер документа</param>
        private void checkAcknowledg(Fs.Native.IArchive fsArc, STLV currentARC, uint count)
        {
            fsArc.GetAcknowledge(count, out Fs.Native.ArcAck arcAck);
            if (currentARC.AddValue((ushort)107) is STLV currentAcknowledge)
            {
                if (currentAcknowledge.AddValue((int)Fw16.Model.TLVTag.OfdTaxId) is TLV OfdTaxId)
                    OfdTaxId.AddValue(this.OfdTaxId);
                if (currentAcknowledge.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV DateTime)
                    DateTime.AddValue(GetByte(arcAck.DT));
                if (currentAcknowledge.AddValue((int)Fw16.Model.TLVTag.OfdSignature) is TLV OfdSignature)
                    OfdSignature.AddValue((arcAck.Signature));

            }
        }
        /// <summary>
        /// функция для получения массива tlv в байтах
        /// </summary>
        /// <param name="tag">тег</param>
        /// <param name="array">массив данных в байтах, функция для помощи GetByte</param>
        /// <returns></returns>
        private byte[] GetTLV(Fw16.Model.TLVTag tag, byte[] array)
        {
            byte[] newArray = new byte[4+array.Length];
            GetByte((ushort)tag).CopyTo(newArray, 0);
            GetByte((ushort)array.Length).CopyTo(newArray, 2);
            array.CopyTo(newArray, 4);
            return newArray;
        }
        /// <summary>
        /// получения массива TLV в байтах содержит тег 107
        /// </summary>
        /// <param name="fsArc">объект аркхивного документа</param>
        /// <param name="count">номер документа для получения подтверждения</param>
        /// <returns></returns>
        private byte[] checkAcknowledg(Fs.Native.IArchive fsArc, uint count)
        {
            byte[] acknowlegeTLV = new byte[52];
            byte[] buff;
            int currentByte = 0;

            fsArc.GetAcknowledge(count, out Fs.Native.ArcAck arcAck);

            buff = GetTLV(Fw16.Model.TLVTag.RptDocAck, GetByte((ushort)48));
            buff.CopyTo(acknowlegeTLV, currentByte);
            currentByte += buff.Length;
            buff = GetTLV(Fw16.Model.TLVTag.OfdTaxId, OfdTaxId);
            buff.CopyTo(acknowlegeTLV, currentByte);
            currentByte += buff.Length;
            buff = GetTLV(Fw16.Model.TLVTag.DateTime, GetByte(arcAck.DT));
            buff.CopyTo(acknowlegeTLV, currentByte);
            currentByte += buff.Length;
            buff = GetTLV(Fw16.Model.TLVTag.OfdSignature,arcAck.Signature);
            buff.CopyTo(acknowlegeTLV, currentByte);
            return acknowlegeTLV;
        }
        /// <summary>
        /// заполнение head отправте true для обновления head
        /// </summary>
        /// <param name="end7"></param>
        public void headGetStream(bool end7 = false)
        {
            try
            {
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                writer.Write(Encoding.GetEncoding(866).GetBytes(way.Substring(0, 53)));
                writer.Write(Encoding.GetEncoding(866).GetBytes(Program.nameProgram.Substring(0, 256)));
                writer.Write(Encoding.GetEncoding(866).GetBytes(Encoding.GetEncoding(866).GetString(dictionary[1][1037])));
                writer.Write(Encoding.GetEncoding(866).GetBytes(statusData.FsId.Substring(0, 16)));
                writer.Write((byte)ecrCtrl.Info.FfdVersion);
                writer.Write(BitConverter.GetBytes(maxShift));
                writer.Write(BitConverter.GetBytes(lastDocNum));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (end7)
            {
                UInt32 hash;
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                DamienG.Security.Cryptography.Crc32 crc32 = new DamienG.Security.Cryptography.Crc32();
                byte[] _hash = crc32.ComputeHash(writer.BaseStream);
                Array.Reverse(_hash);
                hash = BitConverter.ToUInt32(_hash, 0);
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
                writer.BaseStream.Seek(0, SeekOrigin.End);
                memoryStream.Close();
            }
        }

        //private byte[] GetSTLV(int curentDOCCount)
        //{
        //    byte[] STLV;

        //    return STLV;
        //}
        public void WriteReportStartParseFNS()
        {
            if (lastDocNum <= 0)
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
            ///Создание потока для записи файла
            /// 
            fileStream = null;
            try
            {
                if (Program.canRewrite != false && Form1.form != null)
                    fileStream = new FileStream(way, FileMode.CreateNew);
                else if (Program.canRewrite != false)
                    fileStream = new FileStream(way, FileMode.Create);
                else
                {
                    Console.WriteLine("file can not Rewrite");
                }
            }
            catch
            {
                if (MessageBox.Show("Файл существует. Хотите перезаписать файл?", "Предупреждение", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                    fileStream = new FileStream(way, FileMode.Create);
            }
            //если не получилось выходим из метода
            if (fileStream == null)
            {
                File.Delete(way);
                (ecrCtrl as IDisposable).Dispose();
                ecrCtrl = new EcrCtrl();
                Form1.form.B_startParse.Enabled = true;
                Form1.form.Invoke((MethodInvoker)delegate { Form1.form.B_startParse.Text = "Формировать отчет"; });
                return;
            }

            Dictionary<uint, Dictionary<uint, byte[]>> dictionary = GetDictionaryREG();
            fileStream.Close();
            ///заполнение статических переменных первой регистрации
            ///
            SetCurrentStatic(1, dictionary);
            ///Конец !!"заполнение статических переменных первой регистрации"!!
            ///

            //создание потока для записи файлов.
            //writer = new BinaryWriter(fileStream);
            //headGetStream();



            if (Form1.form == null)
            {
                Console.WriteLine("[||||||||||||||||Обработка|||||||||||||]");
            }
            //Чтение всех документов
            for (uint i = 0; i < lastDocNum; i++)
            {
                //обновление прогресбара
                Form1.form?.Invoke((MethodInvoker)delegate { Form1.form.progressBar1.Value = (int)(((double)(i + 1) / (double)lastDocNum) * 100); });
                if (Form1.form == null)
                {
                    if (((double)(i + 1) / (double)lastDocNum) * 40 > count)
                    {
                        count++;
                        Console.Write("|");
                    }
                }

                //Обращение к документам длительного хранения
                if (ecrCtrl.Fw16.FsDirect is Fs.Native.IArchive fsArc)
                {
                    //Получение документа
                    if (fsArc.GetDocument(i + 1, out Fs.Native.ArchiveDoc ad) != Fs.Native.FsAnswer.Success)
                        throw new Exception("Error.WriteReportStartParseFNS fsArc.GetDocument");
                    if (reportFS.AddValue(tagFDn) is STLV currentARC)
                    {
                        ///ПЕРЕРЕГИСТРАЦИЯ
                        /// ↓
                        if (ad.Data is Fs.Native.ArcRegChange rch)
                        {
                            ///!!!!!
                            ///
                            SetCurrentStatic(i + 1, dictionary);
                            if (currentARC.AddValue((ushort)(ad.TlvTag + 100)) is STLV currentRegChange)

                                if (currentRegChange.AddValue((ushort)(ad.TlvTag)) is STLV currentRegChange2)
                                {
                                    Dictionary<uint, byte[]> dictionaryREG;
                                    dictionary.TryGetValue(i + 1, out dictionaryREG);
                                    foreach (var v in dictionaryREG)
                                    {
                                        if (currentRegChange2.AddValue((ushort)v.Key) is TLV TLVRegChangeTeg)
                                            TLVRegChangeTeg.AddValue(v.Value);
                                        else
                                            MessageBox.Show("TLVRegChangeTeg ARCd= " + i + 1 + " TLVRegChangeTeg.Key= " + v.Key);
                                    }
                                }
                            if (ad.Acknowledged)
                                checkAcknowledg(fsArc, i + 1);
                        }

                        ///РЕГИСТРАЦИЯ
                        /// ↓
                        else if (ad.Data is Fs.Native.ArcReg reg)
                        {
                            ///!!!!!
                            ///
                            SetCurrentStatic(i + 1, dictionary);
                            if (currentARC.AddValue((ushort)(ad.TlvTag + 100)) is STLV currentRegChange)
                            {
                                if (currentRegChange.AddValue((ushort)(ad.TlvTag)) is STLV currentRegChange2)
                                {
                                    Dictionary<uint, byte[]> dictionaryREG;
                                    dictionary.TryGetValue(i + 1, out dictionaryREG);
                                    foreach (var v in dictionaryREG)
                                    {
                                        if (currentRegChange2.AddValue((ushort)v.Key) is TLV TLVRegChangeTeg)
                                            TLVRegChangeTeg.AddValue(v.Value);
                                        else
                                            MessageBox.Show("TLVRegChangeTeg ARCd= " + i + 1 + " TLVRegChangeTeg.Key= " + v.Key);
                                    }
                                }
                                if (ad.Acknowledged)
                                    checkAcknowledg(fsArc, currentARC, i + 1);
                            }
                        }

                        ///ЧЕК
                        /// ↓
                        else if (ad.Data is Fs.Native.ArcReceipt rcpt)     //ЭТО ЧЕК!_! 103
                        {
                            if (currentARC.AddValue((ushort)(ad.TlvTag + 100)) is STLV currentReceipt)
                            {
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV dateTime) //добавление времени
                                    dateTime.AddValue(GetByte(rcpt.Freq.DT));
                                else
                                    MessageBox.Show("rcpt 1");
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.Operation) is TLV operation) //добавление признака расчета
                                    operation.AddValue(GetByte((byte)rcpt.Operation));
                                else
                                    MessageBox.Show("rcpt 2");
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.Total) is TLV Total) //добавление сцммы по чеку
                                    Total.AddValue(GetByte((rcpt.Total)));
                                else
                                    MessageBox.Show("rcpt 3");
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.FiscalNumber) is TLV FiscalNumber) //добавление номер ФД
                                    FiscalNumber.AddValue(GetByte((rcpt.Freq.FiscalNumber)));
                                else
                                    MessageBox.Show("rcpt 4");
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.FsId) is TLV FsId)//номер ФН
                                    FsId.AddValue(this.FsId);
                                else
                                    MessageBox.Show("rcpt 5");
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.FsSignature) is TLV FsSignature)//ФПД
                                    FsSignature.AddValue(GetByte(rcpt.Freq.FiscalSignature, 0));
                                else
                                    MessageBox.Show("rcpt 6");
                                if (ad.Acknowledged)
                                    checkAcknowledg(fsArc, currentARC, i + 1);
                            }
                            else
                                MessageBox.Show("rcpt 0");
                        }

                        ///ОТКРЫТИЕ\ЗАКРЫТИЕ СМЕНЫ
                        /// ↓
                        else if (ad.Data is Fs.Native.ArcShift shift) //Закрытие\откытие смены 102\105
                        {
                            if (currentARC.AddValue((ushort)(ad.TlvTag + 100)) is STLV curentShift)
                            {
                                //добавление 1 в счетчик смен если это открытие смены
                                if ((ushort)(ad.TlvTag + 100) == 102)
                                    maxShift++;
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV dateTime) //добавление времени
                                    dateTime.AddValue(GetByte(shift.Freq.DT));
                                else
                                    MessageBox.Show("shift 1");
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.ShiftNumber) is TLV ShiftNumber) //добавление Номера смены
                                    ShiftNumber.AddValue(GetByte((uint)shift.Number));
                                else
                                    MessageBox.Show("shift 2");
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.FiscalNumber) is TLV FiscalNumber) //добавление фискального номера
                                    FiscalNumber.AddValue(GetByte(shift.Freq.FiscalNumber));
                                else
                                    MessageBox.Show("shift 3");
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.FsSignature) is TLV FsSignature) //добавление ФПД
                                    FsSignature.AddValue(GetByte(shift.Freq.FiscalSignature, 0));
                                else
                                    MessageBox.Show("shift 4");
                                if (ad.Acknowledged)
                                    checkAcknowledg(fsArc, currentARC, i + 1);
                            }
                            else
                                MessageBox.Show("shift 0");

                        }

                        //ОТЧЕТ О СОСТОЯНИИ ФН
                        /// ↓
                        else if (ad.Data is Fs.Native.ArcReport arcReport)
                        {
                            if (currentARC.AddValue((ushort)(ad.TlvTag + 100)) is STLV curentArcReport)
                            {
                                if (curentArcReport.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV dateTime) //добавление времени
                                    dateTime.AddValue(GetByte(arcReport.Freq.DT));
                                else
                                    MessageBox.Show("arcReport 1");
                                if (curentArcReport.AddValue((int)Fw16.Model.TLVTag.NoAckCounter) is TLV NoAckCounter) //добавление Не переданных документов
                                    NoAckCounter.AddValue(GetByte(arcReport.NoAckCounter));
                                else
                                    MessageBox.Show("arcReport 2");
                                if (curentArcReport.AddValue((int)Fw16.Model.TLVTag.FirstNoAckDate) is TLV FirstNoAckDate) //добавление даты и времени первого из непереданных ФД
                                    FirstNoAckDate.AddValue(GetByte(arcReport.FirstNoAckDate));
                                else
                                    MessageBox.Show("arcReport 3");
                                if (curentArcReport.AddValue((int)Fw16.Model.TLVTag.FiscalNumber) is TLV FiscalNumber) //добавление ФД
                                    FiscalNumber.AddValue(GetByte(arcReport.Freq.FiscalNumber));
                                else
                                    MessageBox.Show("arcReport 4");
                                if (curentArcReport.AddValue((int)Fw16.Model.TLVTag.FsSignature) is TLV FsSignature) //добавление ФПД
                                    FsSignature.AddValue(GetByte(arcReport.Freq.FiscalSignature, 0));
                                else
                                    MessageBox.Show("arcReport 5");
                                if (ad.Acknowledged)
                                    checkAcknowledg(fsArc, currentARC, i + 1);
                            }
                        }

                        //ОТЧЕТ О ЗАКРЫТИИ ФН
                        /// ↓
                        else if (ad.Data is Fs.Native.ArcCloseFs arcCloseFs)
                        {
                            if (currentARC.AddValue((ushort)(ad.TlvTag + 100)) is STLV curentArcCloseFs)
                            {
                                if (currentARC.AddValue((ushort)(ad.TlvTag)) is STLV curentArcCloseFsAcknowledge)
                                {
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.FsId) is TLV FsId) //добавление Номера ФН
                                        FsId.AddValue(this.FsId);
                                    else
                                        MessageBox.Show("arcCloseFs 1");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.RegId) is TLV RegId) //добавление Регистрационного номера ККТ
                                        RegId.AddValue(Encoding.GetEncoding(866).GetBytes(arcCloseFs.RegId));
                                    else
                                        MessageBox.Show("arcCloseFs 2");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.OwnerTaxId) is TLV OwnerTaxId) //добавление ИНН пользователя
                                        OwnerTaxId.AddValue(dictionary.Values.Last()[1018]);
                                    else
                                        MessageBox.Show("arcCloseFs 3");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.FiscalNumber) is TLV FiscalNumber) //добавление Номера ФД
                                        FiscalNumber.AddValue(GetByte(arcCloseFs.Freq.FiscalNumber));
                                    else
                                        MessageBox.Show("arcCloseFs 4");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV DateTime) //добавление Дата
                                        DateTime.AddValue(GetByte(arcCloseFs.Freq.DT));
                                    else
                                        MessageBox.Show("arcCloseFs 5");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.FsSignature) is TLV FsSignature) //добавление ФПД
                                        FsSignature.AddValue(GetByte(arcCloseFs.Freq.FiscalSignature, 0));
                                    else
                                        MessageBox.Show("arcCloseFs 6");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.t1209) is TLV t1209) //добавление версия ФФД
                                        t1209.AddValue(dictionary.Values.Last()[1209]);
                                    else
                                        MessageBox.Show("arcCloseFs 7");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.Operator) is TLV Operator) //добавление кассир
                                        Operator.AddValue(dictionary.Values.Last()[1021]);
                                    else
                                        MessageBox.Show("arcCloseFs 8");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.OwnerName) is TLV OwnerName) //добавление наименование пользователя
                                        OwnerName.AddValue(dictionary.Values.Last()[1048]);
                                    else
                                        MessageBox.Show("arcCloseFs 9");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.Location) is TLV Location) //добавление Место расчетов
                                        Location.AddValue(dictionary.Values.Last()[1187]);
                                    else
                                        MessageBox.Show("arcCloseFs 10");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.LocationAddress) is TLV LocationAddress) //добавление Адес расчетов
                                        LocationAddress.AddValue(dictionary.Values.Last()[1009]);
                                    else
                                        MessageBox.Show("arcCloseFs 11");
                                    if (curentArcCloseFsAcknowledge.AddValue((int)Fw16.Model.TLVTag.ShiftNumber) is TLV ShiftNumber) //добавление номер смены
                                        ShiftNumber.AddValue(GetByte(maxShift));
                                    else
                                        MessageBox.Show("arcCloseFs 12");

                                    if (ad.Acknowledged)
                                        checkAcknowledg(fsArc, currentARC, i + 1);
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show(ad.TlvTag.ToString());
                        }
                    }
                }
            }


            //Пишем в конце заголовок!
            reportFS.InitHeader((Directory.GetCurrentDirectory() + @"\" + statusData.FsId + ".fnc"), Program.nameProgram, Encoding.GetEncoding(866).GetString(dictionary[1][1037]), statusData.FsId, (byte)ecrCtrl.Info.FfdVersion, maxShift, lastDocNum);
            //выгрузка дерева в файл
            System.Threading.Thread thread = new System.Threading.Thread((System.Threading.ThreadStart)delegate { reportFS.WriteFile(way); });
            thread.Start();
            thread.Join();
            fileStream = new FileStream(way, FileMode.OpenOrCreate);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Seek(0, SeekOrigin.End);
            binaryWriter.Write(checkAcknowledg(ecrCtrl.Fw16.FsDirect as Fs.Native.IArchive, 1));
            fileStream.Close();
            //создание нового объекта для работы с ККТ
            (ecrCtrl as IDisposable).Dispose();
            if (Form1.form == null)
                Console.WriteLine("\nКонец обработки. файл сохранён в {0}", way);
            else
                Form1.form?.Invoke((MethodInvoker)delegate
                {
                    Form1.form.TB_Patch.Text = way;
                    if (MessageBox.Show("Создание отчёта завершено.\nОткрыть файл для проверки?", "Готово", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Form1.form.B_UpdateStop_Click(Form1.form.B_UpdateStop, new EventArgs());
                    }
                    Form1.form.progressBar1.Value = 0;
                    Form1.form.B_startParse.Text = "Формировать отчет";
                });
        }
    }
}
