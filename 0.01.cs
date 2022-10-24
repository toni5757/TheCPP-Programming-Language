using System.Text.RegularExpressions;

//0.01
//Copyright: TheCPP
class vars
{
    private List<int> groessen = new List<int>();
    private List<string> namen = new List<string>();
    public void Add(string name, int size)
    {
        groessen.Add(size);
        namen.Add(name);
    }
    public int get(string name)
    {
        int index = namen.BinarySearch(name);
        int result = 0;
        if (index != 0)
        {
            for (int i = 0; i < index; i++)
            {
                result += groessen[i];
            }
            result += 1;
        }
        return result;
    }
}
class Programm
{
    public static string Input(string input)
    {
        Console.Write(input);
        return Console.ReadLine();
    }
    public static List<List<string>> ToLines(string FilePath)             /*
                                                                    * A function that convert a Text to
                                                                    * lines;
                                                                    */
    {
        string file = File.ReadAllText(FilePath);
        List<List<string>> lines = new List<List<string>>();
        List<string> lines_backup = new List<string>();
        lines_backup = file.Split(";").ToList();

        for (int i = 0; i < lines_backup.Count(); i++)
        {
            string line = lines_backup[i] + ";";
            line = line.Replace("\n", "");
            Regex regex1 = new Regex(@"({)|(})|(\()|(\))|(,)|(')|("")|(\\)");
            List<string> final = regex1.Split(line).ToList();

            lines.Add(final);
        }
        return lines;
    }
    public static void DisplayStringList_2D(List<List<string>> myList)
    {
        int p = 0;

        // Displaying the elements of List
        foreach (var b in myList)
        {
            foreach (var k in b)
            {
                Console.WriteLine("Element " + p + ": " + k + "\n");
                p++;
            }
        }
    }
    public static string toAsm(List<List<string>> tockens)
    {
        List<vars> variablen = new List<vars>();
        string result = "";
        bool isInString = false;
        foreach (List<string> line in tockens)
        {
            for (int i = 0; i < line.Count; i++)
            {
                string tocken_bevor = "";
                string tocken = line[i];
                string t_b_b = ""; //Tocken bevor bevor (2 x bevor)
                try
                {
                    tocken_bevor = line[i - 1];
                    t_b_b = line[i - 2];
                }
                catch { /*...*/ }
                if (tocken == "\"")
                {
                    if (tocken_bevor != "\\" && t_b_b != "\\") isInString = !isInString;
                }
                if (isInString && tocken != "\"")
                {
                    Console.Write(tocken);
                }
            }
        }
        return result;
    }
    public static void Main(string[] args)
    {
        List<List<string>> lines = ToLines(args[0]);
        //DisplayStringList_2D(lines);
        Console.WriteLine(toAsm(lines));
        Input("Press any key to contuine ...");
    }
}
