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

## ReportHeader

Описание

### Поля
  |название      | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |name        | String                  |Описание
  |programm        | String                  |Описание
  |numberKKT        | String                  |Описание
  |numberFS        | String                  |Описание
  |versionFFD        | Byte                  |Описание
  |countShift        | UInt32                  |Описание
  |countfiscalDoc        | UInt32                  |Описание
  |hesh        | UInt32                  |Описание
  
### Конструкторы 

**ReportHeader(BinaryReader reader)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |reader        | BinaryReader                  |Описание
  
  
**ReportHeader(string name, string programm, string numberKKT, string numberFS, byte versionFFD, uint countShift, uint fiscalDoc)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |name          | string                        |Описание
  |programm      | string                        |Описание
  |numberFS      | string                        |Описание
  |versionFFD    | byte                          |Описание
  |countShift    | uint                          |Описание
  |fiscalDoc     | uint                          |Описание 
  
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
  
  **AddHesh(UInt16 tag)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |tag           | BinaryWriter                        |Описание
  
  **WriteFile(BinaryWriter writer)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |writer        | BinaryWriter                  |Описание

## Structurs

Описание

### Поля
  |название      | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |tlString        | UInt16[]                  |Описание
  |tlInt        | UInt16[]                  |Описание
  |tlDouble        | UInt16[]                  |Описание
  |tlBit        | UInt16[]                  |Описание
  |tlUnixTime        | UInt16[]                  |Описание
  |tlByteMass        | UInt16[]                  |Описание
  |stlv        | UInt16[]                  |Описание
  |type        | bool                  |Описание
  |parent        | Structurs                  |Описание
  |Len        | UInt16                  |Описание
  |Tag        | UInt16                  |Описание
  
### Конструкторы 

**Structurs((UInt16 tag, UInt16 len, Structurs parent)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |parent        | Structurs                  |Описание
  |Len        | UInt16                  |Описание
  |Tag        | UInt16                  |Описание
  
  
**Structurs((UInt16 tag, Structurs parent)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |parent        | Structurs                  |Описание
  |Tag        | UInt16                  |Описание
  
## STLV : Structurs

Описание

### Поля
  |название      | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |value        | byte[]                  |Описание
  
### Конструкторы 

**STLV((UInt16 tag, UInt16 len, Structurs parent)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |parent        | Structurs                  |Описание
  |Len        | UInt16                  |Описание
  |Tag        | UInt16                  |Описание
  
  
**STLV((UInt16 tag, Structurs parent)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |parent        | Structurs                  |Описание
  |Tag        | UInt16                  |Описание
  
### Методы

** ReadValue(BinaryReader reader, TreeNodeCollection node)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |reader          | BinaryReader                        |Описание
  |node      | TreeNodeCollection                        |Описание
  
  **AddValue(byte[] value)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |tag           | UInt16                       |Описание
  
  **WriteFile(BinaryWriter writer)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |writer        | BinaryWriter                  |Описание
  
  ## TLV : Structurs

Описание

### Поля
  |название      | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |value        | byte[]                  |Описание
  
### Конструкторы 

**TLV((UInt16 tag, UInt16 len, Structurs parent)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |parent        | Structurs                  |Описание
  |Len        | UInt16                  |Описание
  |Tag        | UInt16                  |Описание
  
  
**TLV((UInt16 tag, Structurs parent)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |parent        | Structurs                  |Описание
  |Tag        | UInt16                  |Описание
  
### Методы

** ReadValue(BinaryReader reader, TreeNodeCollection node = null)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |reader          | BinaryReader                        |Описание
  |node      | TreeNodeCollection                        |Описание
  
  **AddValue(byte[] value)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |value           | byte[]                        |Описание
  
  **Возвращаемое значение**

  | Тип                           |Описание
  |-------------------------------|-------- 
  | Structurs                     | 
  
  **WriteFile(BinaryWriter writer)**

Описание

  |Переменная    | Тип                           |Описание
  |--------------|-------------------------------|-------- 
  |writer        | BinaryWriter                  |Описание
