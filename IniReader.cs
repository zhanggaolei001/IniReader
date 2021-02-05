using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IniHelper
{
    public class IniReader
    {
        public string FilePath { get; set; }
        public IniReader(string filePath)
        {
            this.FilePath = filePath;
            var lines = File.ReadAllLines(FilePath).Where(line => line.Contains("[") || line.Contains("=")).ToList();
            Section newSection = null;
            foreach (var line in lines)
            {
                try
                {
                    if (line.Contains("[") && lines.Contains("]"))
                    {
                        var name = line.Split(new[] { '[', ']' })[1];
                        newSection = new Section(name);
                    }
                    if ((line.Trim().StartsWith(";")) || !line.Contains("=")) continue;
                    var arr = line.Split(new[] { '=', ';' });
                    if (newSection != null)
                    {
                        newSection.KeyValues[arr[0]] = arr[1];
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("ini文件读取异常,请检查字符串:"+line);
                }
            }
        }

        public List<Section> Sections { get; set; }
        public string Read(string section, string key)
        {
            return Sections.FirstOrDefault(s => s.Name == section)?.KeyValues[key];
        }
    }

    public class Section
    {
        public Section(string name)
        {
            this.Name = name;
            KeyValues = new Dictionary<string, string>();
        }
        public string Name { get; set; }
        public Dictionary<string, string> KeyValues { get; set; }
    }
}
