# ReportFNSUtility
Как работать с ФН

public Fs.Native.StatusData GetFsStatus()- получение статуса ФН, если есть документы то работаем 
Fs.Native.IArchive -Fs.Native.FsAnswer GetDocument(uint number, out Fs.Native.ArchiveDoc doc)= получение документа
                   -Fs.Native.FsAnswer GetAcknowledge(uint number, out Fs.Native.ArcAck ack)= получение поддтверждения обмена с ОФД                  
# Структура отчёта в программае

## ReportFS

Описание

### Поля
  |название      | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |header        | ReportHeader                  |Описание
  |fDLongStorage | List<Structurs>               |Описание
  
### Конструкторы 

**ReportFS(BinaryReader reader)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |reader        | BinaryReader                  |Описание
  
  
**ReportFS()**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |              |                               |        
  
### Методы

**InitHeader(string name, string programm, string numberKKT, string numberFS, byte versionFFD, uint countShift, uint fiscalDoc)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |name          | string                        |Описание
  |programm      | string                        |Описание
  |numberFS      | string                        |Описание
  |versionFFD    | byte                          |Описание
  |countShift    | uint                          |Описание
  |fiscalDoc     | uint                          |Описание
  
  **AddValue(UInt16 tag)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |tag           | UInt16                        |Описание
  
**Возвращаемое значение**

  | Тип                           |Описание
  |-------------------------------|-------- 
  | Structurs                     | 
  
  **WriteFile(BinaryWriter writer)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |writer        | BinaryWriter                  |Описание
