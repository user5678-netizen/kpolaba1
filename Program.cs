using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace genProgect
{
    public struct GenData
    {
        public string name;
        public string organism;
        public string formula;
    }
    class Program
    {
        static List<GenData> data = new List<GenData>();
        static int count = 1;
        static string GetFormula(string proteinName)
        {
            foreach (GenData item in data)
            {
                if (item.name.Equals(proteinName)) return item.formula;
            }
            return null;
        }
        static void ReadGenData(string fileName)
        {
            StreamReader reader = new(fileName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] fragments = line.Split('\t');
                GenData protein;
                protein.name = fragments[0];
                protein.organism = fragments[1];
                protein.formula = fragments[2];
                data.Add(protein);
                count++;
            }
            reader.Close();
        }
        static void ReadHandleCommands(string fileName)
        {
            StreamReader reader = new(fileName);
            StreamWriter sw = new("answer.txt");
            sw.WriteLine("Valentina Presnyakova \nGenetic Searching");
            sw.WriteLine("================================================");

            int counter = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine(); counter++;
                string[] command = line.Split('\t');
                if (command[0].Equals("search"))
                {
                    sw.WriteLine($"{counter:D3}   {"search"}   {Decoding(command[1])}");
                    sw.WriteLine($"{"organism"}                {"protein"}");
                    int index = Search(command[1]);
                    if (index != -1)
                    {
                        sw.WriteLine($"{data[index].organism}    {data[index].name}");
                    }
                    else
                    {
                        sw.WriteLine("NOT FOUND");
                    }
                    sw.WriteLine("================================================");
                }
                if (command[0].Equals("diff"))
                {
                    sw.WriteLine($"{counter:D3}   {"diff"}   {command[1]} \t {command[2]}");
                    int cou = Diff(command[1], command[2]);
                    if (cou != -1)
                    {
                        sw.WriteLine($"{"amino - acids difference:"} \n {cou}");
                    }
                    else
                    {
                        sw.WriteLine("NOT FOUND");
                    }
                    sw.WriteLine("================================================");
                }
                if (command[0].Equals("mode"))
                {
                    sw.WriteLine($"{counter:D3}   {"mode"}   {Decoding(command[1])}");
                    Mode(command[1], sw);
                }
            }
            reader.Close();
            sw.Close();
        }
        static bool IsValid(string formula)
        {
            List<char> letters = new() { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y' };
            foreach (char ch in formula)
            {
                if (!letters.Contains(ch)) return false;
            }
            return true;
        }
        static string Encoding(string formula)
        {
            string encoded = String.Empty;
            for (int i = 0; i < formula.Length; i++)
            {
                char ch = formula[i];
                int count = 1;
                while (i < formula.Length - 1 && formula[i + 1] == ch)
                {
                    count++;
                    i++;
                }
                if (count > 2) encoded = encoded + count + ch;
                if (count == 1) encoded = encoded + ch;
                if (count == 2) encoded = encoded + ch + ch;

            }
            return encoded;
        }
        static string Decoding(string formula)
        {
            string decoded = String.Empty;
            for (int i = 0; i < formula.Length; i++)
            {
                if (char.IsDigit(formula[i]))
                {
                    char letter = formula[i + 1];
                    int conversion = formula[i] - '0';
                    for (int j = 0; j < conversion - 1; j++) decoded = decoded + letter;
                }
                else decoded = decoded + formula[i];
            }
            return decoded;
        }
        static int Search(string amino_acid)
        {
            string decoded = Decoding(amino_acid);
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].formula.Contains(decoded)) return i;
            }
            return -1;
        }
        static int Diff(string protein1, string protein2)
        {
            string decoded1 = Decoding(GetFormula(protein1));
            string decoded2 = Decoding(GetFormula(protein2));
            if (decoded1 == null || decoded2 == null)
            {
                return -1;
            }
            int counter = 0;
            int minLength = Math.Min(decoded1.Length, decoded2.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (decoded1[i] != decoded2[i])
                {
                    counter++;
                }
            }
            counter += Math.Abs(decoded1.Length - decoded2.Length);
            return counter;
        }

        static void Mode(string protein, StreamWriter sw)
        {
            string decoded = Decoding(GetFormula(protein));
            char[] arr = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y' };
            char res = '0';
            int maxCount = 0;
            foreach (char amino in arr)
            {
                int count = decoded.Count(ch => ch == amino);
                if (count > maxCount)
                {
                    res = amino;
                    maxCount = count;
                }
            }
            if (res != '0')
            {
                string result = $"amino - acids occurs:\n{res}\t{maxCount}";
                sw.WriteLine(result);
            }
            else
            {
                sw.WriteLine("NOT FOUND");
            }
            sw.WriteLine("================================================");
        }
        static void Main(string[] args)
        {
            ReadGenData("sequences.1.txt");
            ReadHandleCommands("commands.1.txt");
        }
    }
}