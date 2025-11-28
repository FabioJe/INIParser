using System.Text;
namespace INIParser
{
    public sealed class IniFile
    {
        private readonly Dictionary<string, Dictionary<string, string?>> _data = new();
        private string[]? sections;

        public IniFile() { }
        public IniFile(string path)
        {
            LoadFile(path);
        }
        public IniFile(string path, Encoding encoding)
        {
            LoadFile(path, encoding);
        }
        public void LoadFile(string path)
        {
            var strings = File.ReadAllLines(path);
            ParseText(strings);
        }

        public void LoadFile(string path, Encoding encoding)
        {
            var strings = File.ReadAllLines(path, encoding);
            ParseText(strings);
        }

        public async Task LoadFileAsync(string path)
        {
            var strings = await File.ReadAllLinesAsync(path);
            ParseText(strings);
        }

        public async Task LoadFileAsync(string path, Encoding encoding)
        {
            var strings = await File.ReadAllLinesAsync(path, encoding);
            ParseText(strings);
        }

        private void ParseText(IEnumerable<string> strings)
        {
            _data.Clear();
            sections = null;
            Dictionary<string, string?>? lastSec = null; ;
            foreach (var line in strings)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var pureText = line;
                if (char.IsWhiteSpace(pureText[0]))
                    pureText = pureText.Trim();
                if (pureText[0] == ';') continue;
                if (pureText[0] == '[' && pureText[^1] == ']')
                {
                    var lastBlock = pureText[1..^1];
                    if (!_data.ContainsKey(lastBlock))
                    {
                        lastSec = new Dictionary<string, string?>();
                        _data.Add(lastBlock, lastSec);
                    }
                    else
                        lastSec = _data[lastBlock];
                    continue;
                }
                if (lastSec is null) continue;
                var data = pureText.Split('=', 2);
                if (data.Length != 2) { continue; }              
                var key = Trim(data[0]);
                if (lastSec.ContainsKey(key))
                    lastSec[key] = Trim(data[1]);
                else
                    lastSec.Add(key, Trim(data[1]));
            }

        }

        private static string Trim(string t)
        {
            if (t.Length == 0) return t;
            if (char.IsWhiteSpace(t[0]) || char.IsWhiteSpace(t[^1]))
                return t.Trim();
            return t;
        }

        public string? this[string section, string name]
        {
            get
            {
                if (_data.TryGetValue(section, out var dsection))
                    if (dsection.TryGetValue(name, out var text))
                        return text;
                return null;
            }
        }
        public string this[string section, string name, string defaultValue]
        {
            get
            {
                var result = this[section, name];
                if (string.IsNullOrEmpty(result)) return defaultValue;
                return result;
            }
        }

        public string[] Sections
        {
            get
            {
                sections ??= [.. _data.Keys];
                return sections;

            }
        }

        public string[] GetKeys(string section)
        {
            if (_data.TryGetValue(section, out var dsection))
                return [.. dsection.Keys];
            return [];
        }


    }
}