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
        EcrCtrl ecrCtrl;
        /// <summary>
        /// Отчёт о счиывании данных с фискального накопителя
        /// </summary>
        ReportFS reportFS;
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

        public WriteReport(EcrCtrl ecrCtrl)
        {
            //Заполнение преременных 
            this.ecrCtrl = ecrCtrl;
            if (ecrCtrl.Fw16.FsDirect.GetFsStatus().LastDocNum > 0)
            {
                statusData = ecrCtrl.Fw16.FsDirect.GetFsStatus();
                FsId = Encoding.GetEncoding(866).GetBytes(statusData.FsId);
                lastDocNum = statusData.LastDocNum;
                reportFS = new ReportFS();
            }

        }
        //ЧУТЬ ЧУТЬ ПЕРЕГРУЗОК__________________________________________________________________
        /// <summary>
        /// Получение времени в байтах в секундах
        /// </summary>
        /// <param name="dateTime">время из ФН</param>
        /// <returns></returns>
        public byte[] GetByte(DateTime dateTime)
        {
            string dt;
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = Math.Floor((dateTime.ToUniversalTime() - origin).TotalSeconds).ToString();
            return Encoding.GetEncoding(866).GetBytes(dt);
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
            return BitConverter.GetBytes(dt);
        }
        //конец перегрузок|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        void qq()
        {
            //составить список регистраций. ключ номер фискального докумнета
            //Fs.Native.IService svc=null;
            //svc.GetRegStat(out Fs.Native.RegStat rst);
            //var regcount = rst.RegCount;
            var regcount = lastDocNum;
            var dd = new Dictionary<uint, string> ();

            for(byte i = 0; i < regcount; i++)
            {
                //Fs.Native.IArchive arc;
                if (ecrCtrl.Fw16.FsDirect is Fs.Native.IArchive arc)
                {
                    arc.BeginReadReg(i);

                    uint fiscalNumber=1;
                    string ofdTaxId = null;
                    while (arc.NextReadReg(out Fw16.Model.TLV<Fw16.Model.TLVTag> tlv) == Fs.Native.FsAnswer.Success)
                    {
                        if (tlv.Tag == Fw16.Model.TLVTag.OfdTaxId)
                        {
                            ofdTaxId = Encoding.GetEncoding(866).GetString(tlv.Value);
                        }
                        if (tlv.Tag == Fw16.Model.TLVTag.FiscalNumber)
                        {

                            var ms = new System.IO.MemoryStream(tlv.Value);
                            var br = new System.IO.BinaryReader(ms);
                            fiscalNumber=br.ReadUInt32();

                            var w = new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(tlv.Value);
                            fiscalNumber = Convert.ToUInt32(w.Value.ToString());


                        }
                    }
                    dd.Add(fiscalNumber, ofdTaxId);
                }
            }

        }


        public void WriteReportStartParseFNS()
        {
            //qq();
            //Чтение всех документов
            for (uint i = 0; i < lastDocNum; i++)
            {
                //Обращение к документам длительного хранения
                if (ecrCtrl.Fw16.FsDirect is Fs.Native.IArchive fsArc)
                {
                    //Получение документа
                    if (fsArc.GetDocument(i + 1, out Fs.Native.ArchiveDoc ad) != Fs.Native.FsAnswer.Success)
                        throw new Exception("Error.WriteReportStartParseFNS fsArc.GetDocument");
                    if (reportFS.AddValue(tagFDn) is STLV currentARC)
                    {
                        if (ad.Data is Fs.Native.ArcRegChange rch)
                        {
                            ///!!!!!
                            ///
                            if (currentARC.AddValue((ushort)(ad.TlvTag + 100)) is STLV currentRegChange)
                            {
                                if (currentRegChange.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV dateTime) //добавление времени
                                    dateTime.AddValue(GetByte(rch.Base.Freq.DT));
                                else
                                    MessageBox.Show("!");
                            }
                        }
                        else if (ad.Data is Fs.Native.ArcReg reg)
                        {
                            ///!!!!!
                            ///

                        }
                        else if (ad.Data is Fs.Native.ArcReceipt rcpt)     //ЭТО ЧЕК!_! 103
                        {
                            if (currentARC.AddValue((ushort)(ad.TlvTag+100)) is STLV currentReceipt)
                            {
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV dateTime) //добавление времени
                                    dateTime.AddValue(GetByte(rcpt.Freq.DT));
                                else
                                    MessageBox.Show("rcpt 1");
                                if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.Operation) is TLV operation) //добавление признака расчета
                                    operation.AddValue(GetByte((byte)rcpt.Operation));
                                else
                                    MessageBox.Show("rcpt 2");
                                //if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.Total) is TLV Total) //добавление сцммы по чеку
                                //    Total.AddValue(BitConverter.GetBytes((rcpt.Total)));
                                //else
                                //    MessageBox.Show("rcpt 3");
                                //if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.FiscalNumber) is TLV FiscalNumber) //добавление номер ФД
                                //    FiscalNumber.AddValue(BitConverter.GetBytes((rcpt.Freq.FiscalNumber)));
                                //else
                                //    MessageBox.Show("rcpt 4");
                                //if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.FsId) is TLV FsId)//номер ФН
                                //    FsId.AddValue(this.FsId);
                                //else
                                //    MessageBox.Show("rcpt 5");
                                //if (currentReceipt.AddValue((int)Fw16.Model.TLVTag.FsSignature) is TLV FsSignature)//ФПД
                                //    FsSignature.AddValue(BitConverter.GetBytes(rcpt.Freq.FiscalSignature));
                                //else
                                //    MessageBox.Show("rcpt 6");
                            }
                            else
                                MessageBox.Show("rcpt 0");
                        }


                        else if (ad.Data is Fs.Native.ArcShift shift) //Закрытие\откытие смены 102\105
                        {
                            if (currentARC.AddValue((ushort)(ad.TlvTag+100)) is STLV curentShift)
                            {
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.DateTime) is TLV dateTime) //добавление времени
                                    dateTime.AddValue(GetByte(shift.Freq.DT));
                                else
                                    MessageBox.Show("shift 1");
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.ShiftNumber) is TLV ShiftNumber) //добавление Номера смены
                                    ShiftNumber.AddValue(GetByte(shift.Number));
                                else
                                    MessageBox.Show("shift 2");
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.FiscalNumber) is TLV FiscalNumber) //добавление фискального номера
                                    FiscalNumber.AddValue(GetByte(shift.Freq.FiscalNumber));
                                else
                                    MessageBox.Show("shift 3");
                                if (curentShift.AddValue((int)Fw16.Model.TLVTag.FsSignature) is TLV FsSignature) //добавление ФПД
                                    FsSignature.AddValue(GetByte(shift.Freq.FiscalSignature));
                                else
                                    MessageBox.Show("shift 4");
                            }
                            else
                                MessageBox.Show("shift 0");


                            //string str;
                            //if (ad.TlvTag == Fs.Native.FiscalDocumentTag.OpenShift)
                            //    str = ((int)Fw16.Model.TLVTag.RptDocOpenShift).ToString() + " " + ((int)Fw16.Model.TLVTag.RegModeStandalone).ToString();
                            //else
                            //    str = ((int)Fw16.Model.TLVTag.RptDocCloseShift).ToString();
                            //listBox1.Items.Add(str + " "
                            //    + (int)Fw16.Model.TLVTag.DateTime + shift.Freq.DT.ToString()
                            //    + (int)Fw16.Model.TLVTag.ShiftNumber + shift.Number.ToString()
                            //    + (int)Fw16.Model.TLVTag.FiscalNumber + shift.Freq.FiscalNumber.ToString()
                            //    + (int)Fw16.Model.TLVTag.FsSignature + shift.Freq.FiscalSignature.ToString());

                            //checkAcknowledg(fsArc, ad, i); //проверка поддтверждения 
                        }
                        else
                        {
                            MessageBox.Show(ad.TlvTag.ToString());
                        }
                    }
                    //throw new Exception("Error.WriteReportStartParseFNS reportFS.AddValue");
                    MessageBox.Show(Program.GetTypeTLV(Fw16.Model.TLVTag.FsSignature).ToString());

                }
            }

            //Пишем в конце заголовок!
            reportFS.InitHeader((Directory.GetCurrentDirectory() + @"\" + statusData.FsId + ".fnc"), Form1.form.Text, ecrCtrl.Info.FactoryInfo.FwBuild.ToString(), statusData.FsId, (byte)ecrCtrl.Info.FfdVersion, maxShift, lastDocNum);


            //выгрузка дерева в файл
            BinaryWriter binaryWriter= new BinaryWriter(new FileStream(@"путь.bin",FileMode.Create));
            reportFS.WriteFile(binaryWriter);
            (ecrCtrl as IDisposable).Dispose();
            ecrCtrl = new EcrCtrl();
            Form1.form.B_startParse.Enabled = true;
            binaryWriter.Close();
        }
        

        //private void checkAcknowledg(Fs.Native.IArchive fsArc, Fs.Native.ArchiveDoc ad, uint count)
        //{
        //    if (ad.Acknowledged)
        //    {
        //        fsArc.GetAcknowledge(count + 1, out Fs.Native.ArcAck arcAck);
        //        byte signature;
        //        foreach (var item in arcAck.Signature)
        //        {
        //            if (signature)
        //                signature += item;
        //        }
        //        listBox1.Items.Add((int)Fw16.Model.TLVTag.RptDocAck + " "

        //            + Fw16.Model.TLVTag.DateTime + arcAck.DT.ToString()
        //            + Fw16.Model.TLVTag.OfdSignature + signature);
        //    }
        //}
    }
}
