using System.Text.RegularExpressions;
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
        char[] delimiterChars = { ';', '\n' };
        lines_backup = file.Split(delimiterChars).ToList();

        for (int i = 0; i < lines_backup.Count(); i++)
        {
            string line = lines_backup[i] + ";";
            line = line.Replace("\n", "");
            Regex regex1 = new Regex(@"({)|(})|(\()|(\))|(,)|(')|("")|(\\)|(//)|(/)|( )");
            List<string> final = regex1.Split(line).ToList();

            lines.Add(final);
        }
        return lines;
    }
    public static void DisplayStringList_2D(List<List<string>> myList)
    {
        // Displaying the elements of List
        foreach (var b in myList)
        {
            foreach (var k in b)
            {
                Console.Write(": " +  k);
            }
            Console.WriteLine();
        }
    }
    public static string toAsm(List<List<string>> tockens)
    {
        List<vars> variablen = new List<vars>();
        string result = "";
        bool isInString = false;
        bool ignore = false;
        bool big_ignore = false;
        foreach (List<string> line in tockens)
        {
            //string lineS = "";
            int instring = 0;
            for (int i = 0; i < line.Count; i++)
            {
                string tocken_bevor = "";
                string tocken = line[i];
                string t_b_b = ""; //Tocken bevor bevor (2 x bevor)
                //lineS += tocken;
                try
                {
                    tocken_bevor = line[i - 1];
                    t_b_b = line[i - 2];
                }
                catch { /*...*/ }
                //######### STRING #########//
                bool sonderfall = tocken_bevor == "\\" || t_b_b == "\\";
                if (tocken == "\""&& !sonderfall)
                {
                    instring = 0;
                    isInString = !isInString;
                }
                //###### STRING END ########//
                //###### COMMENTS   ########//
                if (!isInString)
                {
                    if (tocken == "//" && !big_ignore) ignore = true;
                    else if (tocken == "*" && tocken_bevor == "/")
                    {
                        big_ignore = true;
                        ignore = true;
                    }
                    if (big_ignore == true)
                    {
                        if (tocken == "/" && tocken_bevor == "*") {
                            big_ignore = false;
                            ignore = false;
                        }
                    }
                }
                
                //###### COMMENTS   ########//
                if (isInString && !ignore) {
                    if (instring >= 1)
                    {
                        Console.Write(tocken); // print when it´s in string
                    }
                    instring++;
                }
                else
                {
                    // Is not in string then:
                }
            }
            if (!big_ignore) ignore = false;
        }
        return result;
    }
    public static void Main(string[] args)
    {
        List<List<string>> lines = ToLines("Programm.txt");
        toAsm(lines);
        //DisplayStringList_2D(lines);
    }
}
