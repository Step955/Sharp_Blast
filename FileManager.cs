using System;
using System.IO;

namespace Sharp_Blast
{
    class FileManager
    {
        static string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string highscoreFile = Path.Combine(folder, "highscore.txt");
        
        public static void save(int newHighScore) { 
        
            using (FileStream fs = new FileStream(highscoreFile, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binarrywriter = new BinaryWriter(fs))
                {
                    binarrywriter.Write(newHighScore);
                }
            }

        }

        public static int load()
        {
            if (File.Exists(highscoreFile))
            {
                using (FileStream fs = new FileStream(highscoreFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader binaryreader = new BinaryReader(fs))
                    {
                        return binaryreader.ReadInt32();
                    }
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
