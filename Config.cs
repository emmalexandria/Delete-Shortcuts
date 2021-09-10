using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;


namespace DeleteShortcuts
{
    public struct FileType
    {
        public string extension;
        public Action action;
        public string moveToDir;

        public FileType(string e, Action a, string mtd = "")
        {
            extension = e;
            action = a;
            moveToDir = mtd;
        }
    }
    
    
    public static class Config
    {
        public static FileType[] readTypes()
        {
            string json = File.ReadAllText("config.json");
            
            
            FileType[] returnArray = JsonConvert.DeserializeObject<FileType[]>(json);
            Console.WriteLine($"Read {returnArray.Length} file extensions successfully");

            return returnArray;
        }

        public static void createExampleJSON()
        {
            FileType txtType = new FileType("txt", Action.Ask);
            FileType psdType = new FileType("psd", Action.Move);

            var types = new List<FileType>();
            types.Add(txtType);
            types.Add(psdType);
            
            string output = JsonConvert.SerializeObject(types, Formatting.Indented);
            

            File.WriteAllText("config.json", output);
            Console.WriteLine(output);
        }
    }
}