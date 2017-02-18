using System.Collections.Generic;

namespace Reqas.Cmd.Domain
{
    public class ProcessedFile
    {
        public ProcessedFile(string fileName, List<string> contents)
        {
            FileName = fileName;
            Contents = contents;
        }
        public string FileName { get; }
        public List<string> Contents { get; }
    }
}