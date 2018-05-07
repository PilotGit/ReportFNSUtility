using DamienG.Security.Cryptography;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportFNSUtility
{
    class ReportFNS
    {
        private ReportHeader _reportHeader;
        private TreeOfTags _treeOfTags;

        public ReportHeader reportHeader { get => _reportHeader; }
        public TreeOfTags treeOfTags { get => _treeOfTags; }

        public ReportFNS()
        {
            _reportHeader = new ReportHeader();
            _treeOfTags = new TreeOfTags();
        }

        public class ReportHeader
        {
            private string name;
            public string Name
            {
                get => name;
                set
                {
                    if (value?.Length >= 53)
                    {
                        this.name = value.Substring(0, 53);
                    }
                    else
                    {
                        this.name = string.Format($"{value,-53}");
                    }
                }
            }

            private string nameProgram;
            public string NameProgram
            {
                get => nameProgram;
                set
                {
                    if (value?.Length >= 256)
                    {
                        this.nameProgram = value.Substring(0, 256);
                    }
                    else
                    {
                        this.nameProgram = string.Format($"{value,-256}");
                    }
                }
            }

            private string numberECR;
            public string NumberECR
            {
                get => numberECR;
                set
                {
                    if (value?.Length >= 20)
                    {
                        this.numberECR = value.Substring(0, 20);
                    }
                    else
                    {
                        this.numberECR = string.Format($"{value,-20}");
                    }
                }
            }

            private string numberFS;
            public string NumberFS
            {
                get => numberFS;
                set
                {
                    if (value?.Length >= 16)
                    {
                        this.numberFS = value.Substring(0, 16);
                    }
                    else
                    {
                        this.numberFS = string.Format($"{value,-16}");
                    }
                }
            }

            private byte versionFFD;
            public byte VersionFFD
            {
                get => versionFFD;
                set => versionFFD = value;
            }

            private uint countShift;
            public uint CountShift
            {
                get => countShift;
                set => countShift = value;
            }

            private uint countFiscalDoc;
            public uint CountFiscalDoc
            {
                get => countFiscalDoc;
                set => countFiscalDoc = value;
            }

            private uint hash;
            public uint Hash
            {
                get => hash;
            }

            public ReportHeader(string name = null, string programm = null, string numberKKT = null, string numberFS = null, byte versionFFD = 0, uint countShift = 0, uint fiscalDoc = 0)
            {
                Name = name;
                NameProgram = programm;
                NumberECR = numberKKT;
                NumberFS = numberFS;
                VersionFFD = versionFFD;
                CountShift = countShift;
                CountFiscalDoc = fiscalDoc;
            }
            /// <summary>
            /// Обновляет данные заголовка согласно переданному потоку чтения
            /// </summary>
            /// <param name="stream">поток чтения</param>
            /// <returns></returns>
            public bool UpdateFromStream(BinaryReader stream)
            {
                //Проеверка длиннны потока на присутствие в ней заголовка.
                if (stream.BaseStream.Length >= 358)
                {
                    //Считывание название документа
                    Encoding encoding = Encoding.GetEncoding(866);
                    byte[] name = new byte[53];
                    stream.Read(name, 0, 53);
                    Name = encoding.GetString(name);
                    //Считывание названия программы
                    byte[] programm = new byte[256];
                    stream.Read(programm, 0, 256);
                    NameProgram = encoding.GetString(programm);
                    //Считывание номера ККТ
                    byte[] numberECR = new byte[20];
                    stream.Read(numberECR, 0, 20);
                    NumberECR = encoding.GetString(numberECR);
                    //Считывание номер фискального накопителя
                    byte[] numberFS = new byte[16];
                    stream.Read(numberFS, 0, 16);
                    NumberFS = encoding.GetString(numberFS);
                    //Считывания версии ФФД, количества смен, количества фискальных документов и хеша
                    this.versionFFD = stream.ReadByte();
                    this.countShift = stream.ReadUInt32();
                    this.countFiscalDoc = stream.ReadUInt32();
                    this.hash = stream.ReadUInt32();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// Сверяет хеш написанный в файле и посчитанный программой.
            /// </summary>
            /// <param name="stream">Поток данных для вычисления и вычленения хеша</param>
            /// <returns>bool, совпадают ли хеши.</returns>
            public bool ChekHash(Stream stream)
            {
                //подготовка
                long _pos = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);
                MemoryStream _tmpMemory = new MemoryStream();
                byte[] _tmpBytes = new byte[354];
                //копирование потока в поток памяти без хеша
                stream.Read(_tmpBytes, 0, 354);
                _tmpMemory.Write(_tmpBytes, 0, 354);
                stream.Read(_tmpBytes, 0, 4);
                UInt32 _hashInStream = BitConverter.ToUInt32(_tmpBytes, 0);
                stream.CopyTo(_tmpMemory);
                //Возвращаем позицию в потоке в исходную
                stream.Seek(_pos, SeekOrigin.Begin);
                //Вычисление хеша и закрытие потока
                uint _hashCompute = ComputeHesh(_tmpMemory);
                _tmpMemory.Close();
                //Возврат результата сравнения
                return _hashCompute == _hashInStream;
            }
            /// <summary>
            /// Возвращает хеш CRC-32 для переданного потока
            /// </summary>
            /// <param name="stream">Поток данных</param>
            /// <returns>Uint, Хеш для переданного потока</returns>
            public uint ComputeHesh(Stream stream)
            {
                long _pos = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);
                Crc32 crc32 = new Crc32();
                byte[] _hash = crc32.ComputeHash(stream);
                Array.Reverse(_hash);
                stream.Seek(_pos, SeekOrigin.Begin);
                return BitConverter.ToUInt32(_hash, 0);
            }
        }

        public class TreeOfTags
        {
            MemoryStream memoryStream;
            BinaryReader streamReader;
            Thread computeStats;

            PosAndLen[] PositionNodeOfStream;

            Statistic stat;

            public uint CountDocs { get => (uint)PositionNodeOfStream.Length; }
            internal Statistic Stat { get => stat; }

            public TreeOfTags()
            {
                stat = new Statistic();
            }
            /// <summary>
            /// Создаёт копию переданного потока данных и вычисляет все позиции документов в потоке. После создаёт поток формирования статистики.
            /// </summary>
            /// <param name="stream"></param>
            /// <returns></returns>
            public bool UpdateFromStream(BinaryReader stream)
            {
                //подготовка переменных
                if (computeStats?.IsAlive ?? false)
                {
                    computeStats?.Abort();
                    computeStats?.Join();
                }
                streamReader?.BaseStream?.Close();
                memoryStream?.Close();
                memoryStream = new MemoryStream();
                //копирование потоков, создание потока чтения 
                stream.BaseStream.Seek(358, SeekOrigin.Begin);
                stream.BaseStream.CopyTo(memoryStream);
                if (memoryStream.Length < 8) return false;
                memoryStream.Seek(0, SeekOrigin.Begin);
                streamReader = new BinaryReader(memoryStream);
                //формирование массива позиций и длинн документов в потоке
                List<PosAndLen> _tmpList = new List<PosAndLen>();
                while (streamReader.BaseStream.Position != streamReader.BaseStream.Length)
                {
                    streamReader.ReadUInt16();
                    Int16 len = streamReader.ReadInt16();
                    _tmpList.Add(new PosAndLen((memoryStream.Position - 4), (short)(len + 4)));
                    streamReader.BaseStream.Seek(len, SeekOrigin.Current);
                }
                PositionNodeOfStream = _tmpList.ToArray();
                memoryStream.Seek(0, SeekOrigin.Begin);
                computeStats = new Thread((ThreadStart)delegate { Stat.Reset(); Program.form.Invoke((MethodInvoker)delegate { Program.form.ReadStats(); }); Stat.UpdateFromStream(memoryStream, PositionNodeOfStream); Program.form.Invoke((MethodInvoker)delegate { Program.form.ReadStats(); }); });
                computeStats.Start();
                return true;
            }
            /// <summary>
            /// Возвращает ветку представляющую один документ.
            /// </summary>
            /// <param name="startNumberDoc">Начальный индекс выводимых документов</param>
            /// <param name="endNumberDoc">Конечный индекс выводимых документов</param>
            /// <returns>Ветка, представляющая один документ</returns>
            public IEnumerable<TreeNode> GetNodes(UInt32 startNumberDoc, UInt32 endNumberDoc)
            {
                for (uint i = startNumberDoc; i <= endNumberDoc; i++)
                {
                    memoryStream.Seek(PositionNodeOfStream[i].Position, SeekOrigin.Begin);
                    byte[] _tmp = new byte[PositionNodeOfStream[i].Length];
                    memoryStream.Read(_tmp, 0, PositionNodeOfStream[i].Length);
                    yield return CreateNode(new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(_tmp));
                }
            }
            /// <summary>
            /// Рекурсивная функция формирующая ветвь
            /// </summary>
            /// <param name="tLVWrapper">tlVWraper, документа</param>
            /// <returns></returns>
            private TreeNode CreateNode(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> tLVWrapper)
            {
                TreeNode node;
                if (tLVWrapper.Source.Tag == Fw16.Model.TLVTag._Anonymous)
                {
                    return CreateNode((tLVWrapper.Value as List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)[0]);
                }
                if (tLVWrapper.Value is List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>> _tmpList)
                {
                    node = new TreeNode($"[{(int)tLVWrapper.Source.Tag}]   {tLVWrapper.Description}");
                    foreach (var _tmpWrap in _tmpList)
                    {
                        node.Nodes.Add(CreateNode(_tmpWrap));
                    }
                }
                else
                {
                    node = new TreeNode($"[{(int)tLVWrapper.Source.Tag}]   {tLVWrapper.Value}   {tLVWrapper.Description}");
                }
                return node;
            }

            public class PosAndLen
            {
                Int64 position;
                Int16 length;

                public long Position { get => position; }
                public short Length { get => length; }

                public PosAndLen(Int64 position, Int16 length)
                {
                    this.position = position;
                    this.length = length;
                }
            }

            public class Statistic
            {
                /// <summary>
                /// Элементы статистики
                /// </summary>
                public enum StatsName
                {
                    err = 0,
                    incomeCount = 1,
                    incomeSum,
                    incomeBackCount,
                    incomeBackSum,
                    outcomeCount,
                    outcomeSum,
                    outcomeBackCount,
                    outcomeBackSum,
                    correctionIncomeCount,
                    correctionIncomeSum,
                    correctionOutcomeCount,
                    correctionOutcomeSum
                }

                decimal[] stat;

                public Statistic()
                {
                    stat = new decimal[Enum.GetValues(typeof(StatsName)).Length];
                }
                /// <summary>
                /// Возвращает значение указанного элемента статистики
                /// </summary>
                /// <param name="index">Наименование элемента статистики</param>
                /// <returns>Decimal, значение элемента статистики</returns>
                public decimal this[StatsName index]
                {
                    get
                    {
                        return stat[(int)index];
                    }

                    set
                    {
                        stat[(int)index] = value;
                    }
                }
                /// <summary>
                /// Создаёт копию переденного потока и вычисляет статистику для документов зашифрованных в потоке.
                /// </summary>
                /// <param name="stream">Поток данных</param>
                /// <param name="PositionNodeOfStream">Позиции и длянны в байтах документов в потоке</param>
                public void UpdateFromStream(Stream stream, PosAndLen[] PositionNodeOfStream)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    long _pos = stream.Position;
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(memoryStream);
                    stream.Seek(_pos, SeekOrigin.Begin);
                    try
                    {
                        for (int i = 0; i < PositionNodeOfStream.Length; i++)
                        {
                            Program.form.Invoke((MethodInvoker)delegate { Program.form.UpdateProgressBar(i + 1, PositionNodeOfStream.Length); });
                            Fs.Native.DocumentType docType = Fs.Native.DocumentType.NoDocument;
                            Fw16.Model.ReceiptKind receiptKind = Fw16.Model.ReceiptKind.NotAvailable;
                            decimal sum = 0;
                            memoryStream.Seek(PositionNodeOfStream[i].Position, SeekOrigin.Begin);
                            byte[] _tmp = new byte[PositionNodeOfStream[i].Length];
                            memoryStream.Read(_tmp, 0, PositionNodeOfStream[i].Length);
                            foreach (var item in new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(_tmp).Value as List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                            {
                                GetDataDoc(item, ref docType, ref receiptKind, ref sum);
                            }
                            switch (docType)
                            {
                                case Fs.Native.DocumentType.Receipt:
                                    switch (receiptKind)
                                    {
                                        case Fw16.Model.ReceiptKind.Income:
                                            this[StatsName.incomeCount]++;
                                            this[StatsName.incomeSum] += sum;
                                            break;
                                        case Fw16.Model.ReceiptKind.IncomeBack:
                                            this[StatsName.incomeBackCount]++;
                                            this[StatsName.incomeBackSum] += sum;
                                            break;
                                        case Fw16.Model.ReceiptKind.Outcome:
                                            this[StatsName.outcomeCount]++;
                                            this[StatsName.outcomeSum] += sum;
                                            break;
                                        case Fw16.Model.ReceiptKind.OutcomeBack:
                                            this[StatsName.outcomeBackCount]++;
                                            this[StatsName.outcomeBackSum] += sum;
                                            break;
                                    }
                                    break;
                                case Fs.Native.DocumentType.ReciptCorrection:
                                    switch (receiptKind)
                                    {
                                        case Fw16.Model.ReceiptKind.Income:
                                            this[StatsName.correctionIncomeCount]++;
                                            this[StatsName.correctionIncomeSum] += sum;
                                            break;
                                        case Fw16.Model.ReceiptKind.Outcome:
                                            this[StatsName.correctionOutcomeCount]++;
                                            this[StatsName.correctionOutcomeSum] += sum;
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    finally
                    {
                        Program.form.Invoke((MethodInvoker)delegate { Program.form.UpdateProgressBar(0); });
                        memoryStream.Close();
                    }
                }
                /// <summary>
                /// Рекурсивная функция подсчитывающая статистику в переданном документе
                /// </summary>
                /// <param name="tLVWrapper">Документ</param>
                /// <param name="docType">Тип документа</param>
                /// <param name="receiptKind">Тип рассчёта</param>
                /// <param name="sum">Сумма</param>
                public void GetDataDoc(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> tLVWrapper, ref Fs.Native.DocumentType docType, ref Fw16.Model.ReceiptKind receiptKind, ref decimal sum)
                {
                    switch ((int)tLVWrapper.Source.Tag)
                    {
                        case 103://чек
                            docType = Fs.Native.DocumentType.Receipt;
                            break;
                        case 131://чек коррекции
                            docType = Fs.Native.DocumentType.ReciptCorrection;
                            break;
                        case 1054://признак рассчёта
                            receiptKind = (Fw16.Model.ReceiptKind)tLVWrapper.Value;
                            break;
                        case 1020://сумма
                            sum = Decimal.Parse(tLVWrapper.Value.ToString());
                            break;
                        default:
                            break;
                    }
                    if (tLVWrapper.Value is List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>> list)
                    {
                        foreach (var item in list)
                        {
                            GetDataDoc(item, ref docType, ref receiptKind, ref sum);
                        }
                    }
                }
                /// <summary>
                /// Сбрасывает всю статистику
                /// </summary>
                public void Reset()
                {
                    for (int i = 0; i < stat.Length; i++)
                    {
                        stat[i] = 0;
                    }
                }
            }
        }

    }



    /// <summary>
    /// Отчёт о считывнии данных из ФН
    /// </summary>
    class ReportFS
    {
        /// <summary>
        /// Заголовок файла отчёта
        /// </summary>
        ReportHeader header;

        ///// <summary>
        ///// Фискальные даннные длительного хранения
        ///// </summary>
        //List<Structurs> fDLongStorage = new List<Structurs>();

        /// <summary>
        /// 
        /// </summary>
        List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>> fdLongStorage = new List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>();

        /// <summary>
        /// Конструктор получающий данные из файлового потока отчёта
        /// </summary>
        /// <param name="reader">Поток файла отчёта</param>
        public ReportFS(BinaryReader reader)
        {
            header = new ReportHeader(reader);
            TreeNodeCollection nodes = Program.form.TV_TreeTags.Nodes;
            Program.form.Invoke((MethodInvoker)delegate { Program.form.progressBar1.Value = (int)(((double)reader.BaseStream.Position / (double)reader.BaseStream.Length) * 100); });
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                reader.ReadUInt16();
                UInt16 len = reader.ReadUInt16();
                reader.BaseStream.Seek(-4, SeekOrigin.Current);
                fdLongStorage.Add(new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(reader.ReadBytes(len + 4)));
                Program.form?.GB_PreviewReport?.Invoke((MethodInvoker)delegate
                {
                    STLV.ShowTree(fdLongStorage.Last(), nodes);
                });
                Program.form.Invoke((MethodInvoker)delegate { Program.form.progressBar1.Value = (int)(((double)reader.BaseStream.Position / (double)reader.BaseStream.Length) * 100); });
            }
            Program.form.Invoke((MethodInvoker)delegate
            {
                Program.form.progressBar1.Value = 0;
                Program.form.B_UpdateStop.Text = "Обновить";
            });
        }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ReportFS()
        {

        }

        /// <summary>
        /// Создаёт заголовок с заданными значениями
        /// </summary>
        /// <param name="name">Наименование файла выгрузки </param>
        /// <param name="programm">программа выгрузки</param>
        /// <param name="numberKKT">Номер ККТ</param>
        /// <param name="numberFS">Номер фискального накопителя</param>
        /// <param name="versionFFD">Версия ФФД</param>
        /// <param name="countShift">Количество смен</param>
        /// <param name="fiscalDoc">Количество фискальных документов</param>
        public void InitHeader(string name, string programm, string numberKKT, string numberFS, byte versionFFD, uint countShift, uint fiscalDoc)
        {
            header = new ReportHeader(name, programm, numberKKT, numberFS, versionFFD, countShift, fiscalDoc);
        }

        ///// <summary>
        ///// Добавить STLV структуру (650XX)
        ///// </summary>
        ///// <param name="tag">Тег добавляемой структуры</param>
        ///// <returns>Добавленная структура типа STLV</returns>
        //public Structurs AddValue(UInt16 tag)
        //{
        //    try
        //    {
        //        fDLongStorage.Add(new STLV(tag, null));
        //    }
        //    catch
        //    {
        //        throw new Exception("Произошла непредвиденная ошибка при добавлении значения в структуру отчёта.");
        //    }
        //    return fDLongStorage.Last();
        //}

        ///// <summary>
        ///// Запускает процесс записи данных в файл
        ///// </summary>
        ///// <param name="writer"></param>
        //public void WriteFile(string way)
        //{
        //    FileStream fileStream = new FileStream(way, FileMode.Open);
        //    BinaryWriter writer = new BinaryWriter(fileStream);

        //    header.WriteFile(writer);
        //    foreach (var item in fDLongStorage)
        //    {
        //        (item as STLV).WriteFile(writer);
        //    }
        //    header.AddHesh(writer);
        //}
    }

    /// <summary>
    /// Заголовок отчёта о считывании данных из ФН
    /// </summary>
    class ReportHeader
    {
        /// <summary>
        /// Наименование файла выгрузки
        /// </summary>
        String name;
        /// <summary>
        /// Длинна наименования файла выгрузки
        /// </summary>
        const int lenName = 53;
        /// <summary>
        /// Программа выгрузки
        /// </summary>
        String programm;
        /// <summary>
        /// Длинна наименования программы вгрузки
        /// </summary>
        const int lenProgramm = 256;
        /// <summary>
        /// Номер ККТ
        /// </summary>
        String numberKKT;
        /// <summary>
        /// Длинна строки "Номер ККТ"
        /// </summary>
        const int lenNumberKKT = 20;
        /// <summary>
        /// Номер фискального накопителя
        /// </summary>
        String numberFS;
        /// <summary>
        /// Длинна строки "Длинна номера фискального накопителя"
        /// </summary>
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
        UInt32 hash;

        /// <summary>
        /// Конструктор получающий все значения
        /// </summary>
        /// <param name="name">Наименование файла выгрузки </param>
        /// <param name="programm">программа выгрузки</param>
        /// <param name="numberKKT">Номер ККТ</param>
        /// <param name="numberFS">Номер фискального накопителя</param>
        /// <param name="versionFFD">Версия ФФД</param>
        /// <param name="countShift">Количество смен</param>
        /// <param name="fiscalDoc">Количество фискальных документов</param>
        public ReportHeader(string name, string programm, string numberKKT, string numberFS, byte versionFFD, uint countShift, uint fiscalDoc)
        {
            if (name.Length >= lenName)
            {
                this.name = name.Substring(0, lenName);
            }
            else
            {
                this.name += string.Format($"{ name,-lenName}");
            }
            if (programm.Length >= lenProgramm)
            {
                this.programm = programm.Substring(0, lenProgramm);
            }
            else
            {
                this.programm = string.Format($"{programm,-lenProgramm}");
            }
            if (numberKKT.Length >= lenNumberKKT)
            {
                this.numberKKT = numberKKT.Substring(0, lenNumberKKT);
            }
            else
            {
                this.numberKKT = string.Format($"{numberKKT,-lenNumberKKT}");
            }
            if (numberFS.Length >= lenNumberFS)
            {
                this.numberFS = numberFS.Substring(0, lenNumberFS);
            }
            else
            {
                this.numberFS = string.Format($"{numberFS,-lenNumberFS}");
            }
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
            byte[] numberKKT = new byte[lenNumberKKT];
            reader.Read(numberKKT, 0, lenNumberKKT);
            this.numberKKT = encoding.GetString(numberKKT);
            //Считывание номер фискального накопителя
            byte[] numberFS = new byte[lenNumberFS];
            reader.Read(numberFS, 0, lenNumberFS);
            this.numberFS = encoding.GetString(numberFS);
            //Считывания версии ФФД, количества смен, количества фискальных документов и хеша
            this.versionFFD = reader.ReadByte();
            this.countShift = reader.ReadUInt32();
            this.countfiscalDoc = reader.ReadUInt32();
            this.hash = reader.ReadUInt32();
            Program.form.Invoke((MethodInvoker)delegate
            {
                Program.form.TB_Name.Text = this.name;
                Program.form.TB_Program.Text = this.programm;
                Program.form.TB_NumberECR.Text = this.numberKKT;
                Program.form.TB_NumberFS.Text = this.numberFS;
                Program.form.TB_VersionFFD.Text = this.versionFFD.ToString();
                Program.form.TB_CountShift.Text = this.countShift.ToString();
                Program.form.TB_CountFiscalDoc.Text = this.countfiscalDoc.ToString();
                Program.form.TB_Hash.Text = this.hash.ToString();
                //Program.form.treeView1.Nodes.Add("Header");
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_1_saveFile.Text);
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_2_UnloadingProgram.Text = this.programm);
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_3_RegNumber.Text = this.numberKKT);
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_4_NumberFN.Text = this.numberFS);
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_5_NumberFFD.Text = this.versionFFD.ToString());
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_6_NumberOfShifts.Text = this.countShift.ToString());
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_7_NumberOfFiscalDOC.Text = this.countfiscalDoc.ToString());
                //Program.form.treeView1.Nodes[0].Nodes.Add(Program.form.TB_8_CheckSum.Text = this.hesh.ToString());
            });
        }

        /// <summary>
        /// Записывает в поток все поля заголовка кроме хеша.
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void WriteFile(BinaryWriter writer)
        {
            try
            {
                writer.Write(Encoding.GetEncoding(866).GetBytes(name));
                writer.Write(Encoding.GetEncoding(866).GetBytes(programm));
                writer.Write(Encoding.GetEncoding(866).GetBytes(numberKKT));
                writer.Write(Encoding.GetEncoding(866).GetBytes(numberFS));
                writer.Write(versionFFD);
                writer.Write(BitConverter.GetBytes(countShift));
                writer.Write(BitConverter.GetBytes(countfiscalDoc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Записывает хеш в файл
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void AddHesh(BinaryWriter writer)
        {
            //считаем хеш
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            Crc32 crc32 = new Crc32();
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
            writer.Close();
            memoryStream.Close();
        }
    }

    /// <summary>
    /// Базовый класс для реализации TLV и STLV структур
    /// </summary>
    class Structurs
    {
        /// <summary>
        /// тип структуры true-считывание из ККТ, false-расшифровка файла.  
        /// </summary>
        protected bool fromKKT = true;

        /// <summary>
        /// STLV структура в которой находится этот объект
        /// </summary>
        Structurs parent;

        /// <summary>
        /// Тег STLV или TLV структуры
        /// </summary>
        UInt16 tag;
        /// <summary>
        /// Длинна структуры.
        /// </summary>
        UInt16 len;

        /// <summary>
        /// Свойство для доступа к длинне STLV или TLV структуре
        /// </summary>
        public UInt16 Len
        {
            get => len;
            set
            {
                if (this.fromKKT)
                {
                    try
                    {
                        UInt16 tmp = (UInt16)(value - this.len);
                        if (parent != null)
                            parent.Len += tmp;
                        this.len = value;
                    }
                    catch
                    {
                        throw new Exception("Милорд, мы не смогли посчитать наши запасы.");
                    }
                }
                else
                {
                    Debug.Write("Не возможно изменять значение длинны при считывании файла.");
                }
            }
        }

        /// <summary>
        /// Тег STLV или TLV структуры
        /// </summary>
        public UInt16 Tag { get => tag; }

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        /// <param name="parent">STLV структура в которою происходит добавление</param>
        public Structurs(UInt16 tag, UInt16 len, Structurs parent)
        {
            this.tag = tag;
            this.len = len;
            this.parent = parent;
            fromKKT = false;//тип - считывание данных из файла
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="parent">STLV структура в которою происходит добавление</param>
        public Structurs(UInt16 tag, Structurs parent)
        {
            this.tag = tag;
            this.parent = parent;
            fromKKT = true;//тип - считывание данных из ККТ
        }
    }
    /// <summary>
    /// Реализация TLV структуры
    /// </summary>
    class TLV : Structurs
    {
        /// <summary>
        /// Значение в TLV структуре
        /// </summary>
        byte[] value;

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        /// <param name="parent">STLV структура в которою добавляется эта TLV структура</param>
        public TLV(UInt16 tag, UInt16 len, Structurs parent) : base(tag, len, parent)
        {
            value = new byte[Len];
        }

        /// <summary>
        /// Формирует ветки в полученной коллекции ветвей данными из полученного TLVWrapper
        /// </summary>
        /// <param name="tlv">Структруа с данными ветви</param>
        /// <param name="nodes">Коллекция ветвей</param>
        public static void ShowTree(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> tlv, TreeNodeCollection nodes = null)
        {
            if (tlv.Value is Byte[] val)
            {
                string str = "";
                foreach (var item in val)
                {
                    str += $"{item,2:X2} ";
                }
                nodes.Add($"{string.Format($"({(int)tlv.Source.Tag})[{tlv.Source.Length}]", -13)}  {str}            [{tlv.Description}]");
            }
            else
            {
                nodes.Add($"{string.Format($"({(int)tlv.Source.Tag})[{tlv.Source.Length}]", -13)}  {tlv.Value.ToString()}            [{tlv.Description}]");
            }
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="parent">Родительная STLV структура.</param>
        public TLV(UInt16 tag, Structurs parent) : base(tag, parent)
        {
            if (parent != null)
                parent.Len += 4;
        }

        /// <summary>
        /// Присваивает значение TLV структуре
        /// </summary>
        /// <param name="value">Значение в виде массива байтов</param>
        /// <returns>0-операция завершилась успешно</returns>
        public void AddValue(byte[] value)
        {
            this.value = value;
            Len = (UInt16)value.Length;
        }

        /// <summary>
        /// Запись в файл тега, длинны и значения
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void WriteFile(BinaryWriter writer)
        {
            try
            {
                writer.Write(Tag);
                writer.Write(Len);
                writer.Write(value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"tag - {Tag}; Exception message: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Реализация STLV структуры
    /// </summary>
    class STLV : Structurs
    {
        /// <summary>
        /// Структуры в составе STLV структур
        /// </summary>
        List<Structurs> value = new List<Structurs>();

        /// <summary>
        /// Конструктор используемы при считыывании данных из файла
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="len">Длинна</param>
        /// <param name="parent">STLV структура в которою добавляется эта STLV структура</param>
        public STLV(UInt16 tag, UInt16 len, Structurs parent) : base(tag, len, parent)
        {

        }

        /// <summary>
        /// Формирует ветки в полученной коллекции ветвей данными из полученного TLVWrapper
        /// </summary>
        /// <param name="stlv">Структруа с данными ветви</param>
        /// <param name="nodes">Коллекция ветвей</param>
        public static void ShowTree(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> stlv, TreeNodeCollection nodes)
        {
            if (stlv.Source.Tag != Fw16.Model.TLVTag._Anonymous)//Игнорирование анонимного тега
            {
                TreeNode tmp = nodes.Add($"{$"({(int)stlv.Source.Tag})[{stlv.Source.Length}]",-13}            [{stlv.Description}]");
                foreach (var item in stlv.Value as List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                {
                    if (item.Value is List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                    {
                        STLV.ShowTree(item, tmp.Nodes);
                    }
                    else
                    {
                        TLV.ShowTree(item, tmp.Nodes);
                    }
                }

            }
            else
            {
                foreach (var item in stlv.Value as List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                    STLV.ShowTree(item, nodes);
            }
        }

        /// <summary>
        /// конструктор используемый для считывания данных из ККТ
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <param name="parent">STLV структура в которою добавляется эта STLV структура</param>
        public STLV(UInt16 tag, Structurs parent) : base(tag, parent)
        {
            if (parent != null)
                parent.Len += 4;
        }

        /// <summary>
        /// Добавить значение в STLV структуру
        /// </summary>
        /// <param name="tag">Тег добавляемой структуры</param>
        /// <returns>Добавленная структура типа STLV</returns>
        public Structurs AddValue(UInt16 tag)
        {
            try
            {

                if (Program.GetTypeTLV((Fw16.Model.TLVTag)tag) is Fw16.Model.TLVType.STLV)
                {
                    value.Add(new STLV(tag, this));
                }
                else
                {
                    value.Add(new TLV(tag, this));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла непредвиденная ошибка при добавлении значения в структуру отчёта. \nException\n{ex.Message}");
            }
            return value.Last();
        }

        /// <summary>
        /// Запись в файл тега длинны и вызов записи в файл для всех вложенных объектов
        /// </summary>
        /// <param name="writer">Поток записи</param>
        public void WriteFile(BinaryWriter writer)
        {
            try
            {
                writer.Write(Tag);
                writer.Write(Len);
                foreach (var item in value)
                {
                    if (item is TLV itemTlv)
                    {
                        itemTlv.WriteFile(writer);
                    }
                    else
                    {
                        (item as STLV).WriteFile(writer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
