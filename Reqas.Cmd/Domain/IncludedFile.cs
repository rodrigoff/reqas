namespace Reqas.Cmd.Domain
{
    public class IncludedFile
    {
        public IncludedFile(string fileName, string includeName, params IncludedFile[] dependencies)
        {
            FileName = fileName;
            IncludeName = includeName;
            Dependencies = dependencies;
        }

        public string FileName { get; }
        public string IncludeName { get; }
        public IncludedFile[] Dependencies { get; }
    }
}
