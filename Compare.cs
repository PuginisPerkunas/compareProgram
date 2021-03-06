﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pirmaUzduotis
{

    class Compare
    {
        string firstFile, secondFile;
        public static int DUPLICATES_FOUND = 0;
        public string SEARCH_ENTRY_POINT;
        const int BYTES_TO_READ = sizeof(Int64);

        //constructor
        public Compare(String SEARCH_ENTRY_POINT)
        {
            this.SEARCH_ENTRY_POINT = SEARCH_ENTRY_POINT;
        }

        public void Start()
        {
            //Recursively iterate through directories 
            //on every file iterate through directories again
            //compare every file now, if two files are equal respond to 
            Console.WriteLine("Started");
            ProcessDirectory(SEARCH_ENTRY_POINT);
            Console.WriteLine("finish");
        }
        //This method is used to iterate through all the files. 
        private void ProcessDirectory(String path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                FileInfo mainFile = new FileInfo(file);
                RecursiveComparisonIteration(SEARCH_ENTRY_POINT, mainFile);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(path);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory);
            }
        }

        //This method is used to iterate through all the files and compare them against provided file. 
        private void RecursiveComparisonIteration(string path, FileInfo mainFile)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                FileInfo comparedFile = new FileInfo(file);


                if (mainFile.FullName != comparedFile.FullName)
                {
                    if (FilesAreEqual(mainFile, comparedFile))
                    {
                        DUPLICATES_FOUND++;
                        firstFile = mainFile.FullName;
                        secondFile = comparedFile.FullName;
                    }
                }
            }

            string[] subdirectoryEntries = Directory.GetDirectories(path);
            foreach (string subdirectory in subdirectoryEntries)
            {
                RecursiveComparisonIteration(subdirectory, mainFile);
            }
        }

        static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
            {

                return false;
            }
            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

            using (FileStream fileStream1 = first.OpenRead())
            using (FileStream fileStream2 = second.OpenRead())
            {
                byte[] one = new Byte[BYTES_TO_READ];
                byte[] two = new Byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fileStream1.Read(one, 0, BYTES_TO_READ);
                    fileStream2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public String getFirstString()
        {
            return firstFile;
        }
        public String getSecondString()
        {
            return secondFile;
        }


    }//class
}//namespace
