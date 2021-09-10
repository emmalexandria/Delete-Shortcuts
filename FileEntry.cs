namespace DeleteShortcuts
{
    public enum Action
    {
        Move,
        Delete,
        Ask
    }
    
    //this struct will later be dynamically filled out by read in, so new filetypes can be added easily

    struct FileStruct
    {
        public readonly Action FileAction;
        public string Extension;
        public string Dir;
        public string MoveToDir;

        public FileStruct(Action a, string e, string d, string mvd = "")
        {
            FileAction = a;
            Extension = e;
            Dir = d;
            MoveToDir = mvd;
        }
    }
}