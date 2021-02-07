using System;
using System.Collections.Generic;
using System.IO;

namespace TSVFileChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            string file1Line;
            string file2Line;

            //Read user_email_hash.1m.tsv and ip.1m.tsv into dictionary

            var dictionary = new Dictionary<string, Data>();

            var file1StreamReader = new StreamReader("D:\\user_email_hash.1m.tsv");
            var file2StreamReader = new StreamReader("D:\\ip_1m.tsv");

            //Skip header line
            file1StreamReader.ReadLine();
            file2StreamReader.ReadLine();

            while ((file1Line = file1StreamReader.ReadLine()) != null)
            {
                var file1TabSeperatedLine = file1Line.Split('\t');

                file2Line = file2StreamReader.ReadLine();

                var file2TabSeparatedLine = file2Line.Split('\t');

                var data = new Data();

                data.Id = file1TabSeperatedLine[0];
                data.Username = file1TabSeperatedLine[1];
                data.Email = file1TabSeperatedLine[2];
                data.HashedPassword = file1TabSeperatedLine[3];
                data.IP = file2TabSeparatedLine[1];

                dictionary[data.Email] = data;
            }

            file1StreamReader.Close();
            file2StreamReader.Close();



            //Iterate plain_32m.tsv and generate final output

            string file3Line;

            var outputFileWriter = new StreamWriter(@"D:\\output.tsv");

            outputFileWriter.WriteLine($"id\tusername\temail\thashed_password\tplaintext_password\tip");

            var file3StreamReader = new StreamReader("D:\\plain_32m.tsv");

            //Skip header line
            file3StreamReader.ReadLine();

            while ((file3Line = file3StreamReader.ReadLine()) != null)
            {
                var file3TabSeparatedLine = file3Line.Split('\t');

                var email = file3TabSeparatedLine[0];
                var plaintextPassword = file3TabSeparatedLine[1];

                Data relevantData;

                //Try searching above dictionary by key email and generate output file entry if exist
                if (dictionary.TryGetValue(email, out relevantData))
                {
                    outputFileWriter.WriteLine($"{relevantData.Id}\t{relevantData.Username}\t{relevantData.Email}\t{relevantData.HashedPassword}\t{plaintextPassword}\t{relevantData.IP}");
                }
            }

            file3StreamReader.Close();

            outputFileWriter.Close();
        }
    }

    class Data
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string HashedPassword { get; set; }

        public string IP { get; set; }
    }
}
