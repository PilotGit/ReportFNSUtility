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
            public static uint ComputeHesh(Stream stream)
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
            public Thread computeStats;

            PosAndLen[] Nodes;
            public readonly Statistic stat;

            public uint CountDocs { get => (uint)Nodes.Length; }

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

                memoryStream?.Close();
                memoryStream = new MemoryStream();
                //копирование потоков, создание потока чтения 
                stream.BaseStream.Seek(358, SeekOrigin.Begin);
                stream.BaseStream.CopyTo(memoryStream);
                if (memoryStream.Length < 8) return false;
                memoryStream.Seek(0, SeekOrigin.Begin);
                BinaryReader streamReader = new BinaryReader(memoryStream);
                //формирование массива позиций и длинн документов в потоке
                List<PosAndLen> _tmpList = new List<PosAndLen>();
                while (streamReader.BaseStream.Position != streamReader.BaseStream.Length)
                {
                    UInt16 tag = streamReader.ReadUInt16();
                    UInt16 len = streamReader.ReadUInt16();
                    if (len > streamReader.BaseStream.Length - streamReader.BaseStream.Position) return false;
                    _tmpList.Add(new PosAndLen((memoryStream.Position - 4), (short)(len + 4)));
                    streamReader.BaseStream.Seek(len, SeekOrigin.Current);
                }
                Nodes = _tmpList.ToArray();
                memoryStream.Seek(0, SeekOrigin.Begin);
                computeStats = new Thread((ThreadStart)delegate
                {
                    stat.Reset();
                    Program.form?.Invoke((MethodInvoker)delegate { Program.form.ReadStats(); });
                    stat.UpdateFromStream(memoryStream, Nodes);
                    Program.form?.Invoke((MethodInvoker)delegate
                    {
                        Program.form.B_UpdateStop.Text = "Обновить";
                        Program.form.ReadStats();
                        Program.form.UpdateProgressBar(0);
                    });
                });
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
                    memoryStream.Seek(Nodes[i].Position, SeekOrigin.Begin);
                    byte[] _tmp = new byte[Nodes[i].Length];
                    memoryStream.Read(_tmp, 0, Nodes[i].Length);
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
                    string _tmp = "";
                    if (tLVWrapper.Value is byte[] val)
                    {
                        foreach (var item in val)
                        {
                            _tmp += item.ToString("X2") + " ";
                        }

                    }
                    node = new TreeNode($"[{(int)tLVWrapper.Source.Tag}]   {(_tmp.Length > 0 ? _tmp : tLVWrapper.Value)}   {tLVWrapper.Description}");
                }
                return node;
            }

            public void StopComputeStats()
            {

                if (computeStats?.IsAlive ?? false)
                {
                    computeStats?.Abort();
                    computeStats?.Join();
                    Program.form?.Invoke((MethodInvoker)delegate { Program.form.UpdateProgressBar(0); });
                }
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

                decimal[] stats;

                public Statistic()
                {
                    stats = new decimal[Enum.GetValues(typeof(StatsName)).Length];
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
                        return stats[(int)index];
                    }

                    set
                    {
                        stats[(int)index] = value;
                    }
                }
                /// <summary>
                /// Создаёт копию переденного потока и вычисляет статистику для документов зашифрованных в потоке.
                /// </summary>
                /// <param name="stream">Поток данных</param>
                /// <param name="nodes">Позиции и длянны в байтах документов в потоке</param>
                public void UpdateFromStream(Stream stream, PosAndLen[] nodes)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    long _pos = stream.Position;
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(memoryStream);
                    stream.Seek(_pos, SeekOrigin.Begin);
                    try
                    {
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            Program.form?.Invoke((MethodInvoker)delegate { Program.form.UpdateProgressBar(i + 1, nodes.Length); });
                            Fs.Native.DocumentType docType = Fs.Native.DocumentType.NoDocument;
                            Fw16.Model.ReceiptKind receiptKind = Fw16.Model.ReceiptKind.NotAvailable;
                            decimal sum = 0;
                            memoryStream.Seek(nodes[i].Position, SeekOrigin.Begin);
                            byte[] _tmp = new byte[nodes[i].Length];
                            memoryStream.Read(_tmp, 0, nodes[i].Length);
                            foreach (var item in new Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>(_tmp).Value as List<Fw16.Model.TLVWrapper<Fw16.Model.TLVTag>>)
                            {
                                GetStatsDoc(item, ref docType, ref receiptKind, ref sum);
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
                public void GetStatsDoc(Fw16.Model.TLVWrapper<Fw16.Model.TLVTag> tLVWrapper, ref Fs.Native.DocumentType docType, ref Fw16.Model.ReceiptKind receiptKind, ref decimal sum)
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
                            GetStatsDoc(item, ref docType, ref receiptKind, ref sum);
                        }
                    }
                }
                /// <summary>
                /// Сбрасывает всю статистику
                /// </summary>
                public void Reset()
                {
                    for (int i = 0; i < stats.Length; i++)
                    {
                        stats[i] = 0;
                    }
                }
            }
        }

    }
}
