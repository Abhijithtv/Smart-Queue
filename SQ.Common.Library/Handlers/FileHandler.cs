using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SQ.Common.Library.Handlers
{
    public class FileHandler
    {
        private  readonly static string path = @"D:\\dev\\Git\\Smart-Queue\\CacheHouse\\";

        private readonly static string start = "###---***";

        private readonly static string end = "***---###";
        public static bool Write(string fileName, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }
                
            if(!File.Exists(path + fileName))
            {
                var status = CreateFile(fileName);
                if(!status) return false;
            }

            using (var file = File.Open(path + fileName,FileMode.Append))
            using (var writer = new StreamWriter(file))
            {
                writer.Write(data);
                writer.Flush();
            }

            return true;
        }

        public static bool CreateFile(string fileName)
        {
            if (File.Exists(fileName)) { return false; }
            try
            {
                using (FileStream fs = File.Create(path + fileName)) { }
            }
            catch(Exception ex) 
            {
                //
                return false; 
            };
            
            return true;
        }

        public static string? TryGetValue(string fileName, string Encodedkey) 
        {
            if (!File.Exists(path + fileName)) { return null; }

            StreamReader file = new StreamReader(path + fileName);

            string? line;
            
            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(Encodedkey))
                {
                    line = file.ReadLine();
                    break;
                }
            }

            file.Close();
            return line;
        }

        public static string GroupIdToFileName(string GroupId)
        {
            return $"{GroupId}.txt";
        }
    }
}
