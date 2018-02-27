# ReportFNSUtility
Как работать с ФН

public Fs.Native.StatusData GetFsStatus()- получение статуса ФН, если есть документы то работаем 
Fs.Native.IArchive -Fs.Native.FsAnswer GetDocument(uint number, out Fs.Native.ArchiveDoc doc)= получение документа
                   -Fs.Native.FsAnswer GetAcknowledge(uint number, out Fs.Native.ArcAck ack)= получение поддтверждения обмена с ОФД                  
