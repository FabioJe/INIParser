# INI Parser

Lightweight INI parser optimised for speed to quickly read and process large amounts of INI files. The INI parser is exclusively a reader and does not support writing files.

#### Requires .NET 8 or higher

### How to use?

The simplest option is to pass the file path in the constructor.
```csharp
using INIParser;

var iniFile = new IniFile("file.ini");
string? textValue = iniFile["SectionName", "key"];
```

Alternatively, asynchronous loading of the file is also supported. 

```csharp
using INIParser;

var iniFile = new IniFile();
await iniFile.LoadFileAsync("file.ini");
string? textValue = iniFile["SectionName", "key"];
```


### FAQ
#### Can the files also be written?
The INI parser is exclusively a reader and does not support writing files.

#### Can comments be read from the ini file?
Comments are not read during parsing and cannot be accessed. 