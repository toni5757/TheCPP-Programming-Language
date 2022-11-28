using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
class vars
{
    private List<int> groessen = new List<int>();
    private List<string> namen = new List<string>();
    private List<string> varType = new List<string>();
    private List<int> varSize = new List<int>();
    public void Add(string name, int size)
    {
        groessen.Add(size);
        namen.Add(name);
    }
    public int getAdr(string name)
    {
        int index = namen.BinarySearch(name);
        int result = 0;
        if (index != 0)
        {
            for (int i = 0; i < index; i++)
            {
                result += groessen[i];
            }
            //result += 1;
        }
        return result;
    }
    public int getVarSizeFromType(string type)
    {
        int size = 0;
        int index = this.varType.BinarySearch(type);
        size = this.varSize[index];
        return size;
    }
    public void RegisterNewVarTypeAndHisSize(string typeName, int size)
    {
        this.varType.Add(typeName);
        this.varSize.Add(size);
    }
    public void printVars()
    {
        for (int i=0;i<namen.Count;i++)
        {
            Console.WriteLine(namen[i] + " -> " + groessen[i]);
        }
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
            Regex regex1 = new Regex(@"({)|(})|(\()|(\))|(,)|(')|("")|(\\)|(//)|(/)|(=)|( )");
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
        vars variablen = new vars();
        variablen.RegisterNewVarTypeAndHisSize("int", 4);
        variablen.RegisterNewVarTypeAndHisSize("short", 2);
        variablen.RegisterNewVarTypeAndHisSize("byte", 1);
        variablen.RegisterNewVarTypeAndHisSize("char", 1);

        //In Build Asm funktion
        bool ASM_FUNKTION = false;
        string ASM_FUNKTION_BUFFER = "";

        string result = "";
        bool isInString = false;
        bool ignore     = false;
        bool big_ignore = false;
        bool funktion   = false;
        bool varDef     = false;
        int defin       = 0;
        string varType  = "";
        string varName  = "";
        string varData  = "";
        foreach (List<string> line in tockens)
        {
            int instring = 0;
            for (int i = 0; i < line.Count; i++)
            {
                string tocken_bevor = "";
                string tocken = line[i];
                string t_b_b = ""; //Tocken bevor bevor (2 x bevor)
                string t_b_b_b = ""; //Tocken (3 x bevor)
                try
                {
                    tocken_bevor = line[i - 1];
                    t_b_b = line[i - 2];
                    t_b_b_b = line[i - 3];
                }
                catch { /*...*/ }
                //######### STRING #########//
                bool sonderfall = tocken_bevor == "\\" || t_b_b == "\\";
                if (tocken == "\""&& !sonderfall)
                {
                    instring = 0;
                    isInString = !isInString;
                }
                //PRINT (can delte)
                if (isInString && !ignore)
                {
                    if (instring >= 1)
                    {
                        //Console.Write(tocken); // print when it´s in string
                        if (ASM_FUNKTION)
                        {
                            ASM_FUNKTION_BUFFER += tocken;
                        }
                    }
                    instring++;
                } //end can delte
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
                //######COMMENTS  END########//

                if (!isInString && !ignore) {
                    if (!funktion)
                    {
                        if (tocken == "(") funktion = true;
                        if (tocken_bevor == "asm") ASM_FUNKTION = true;
                        //##### VARS START #####//
                        if (tocken == "int")
                        {
                            varDef = true;
                        }
                        else if (tocken == "short")
                        {
                            varDef = true;
                        }
                        else if (tocken == "byte")
                        {
                            varDef = true;
                        }
                        //##### VARS  END #####//
                    }
                    else {
                        if (tocken == ")") funktion = false;
                    }
                    //##### VAR    DETACTION #####//
                    if (varDef && tocken != " " && tocken != "" && tocken != "=") //Verbugt
                    {
                        if (defin == 0)
                        {
                            varType = tocken;
                        }
                        else if (defin == 1)
                        {
                            varName = tocken;
                        }
                        else if (defin == 2)
                        {
                            if (tocken != ";") {
                                varData = tocken.Replace(";","");
                                if (varData == "{")
                                {
                                    if (varType != "string") varData = "0";
                                    else  varData = "";
                                }
                                varDef = false;
                                defin = -2; // Because -2 + 1 = -1
                            }
                        }
                        defin++;
                    }
                    if (defin == -1)
                    {
                        defin = 0;
                        variablen.Add(varName, variablen.getVarSizeFromType(varType));
                        Console.WriteLine($"[]->mov [{variablen.getAdr(varName)}], 0");
                    }
                    //##### VAR DETACTION END #####//
                }
                if (ASM_FUNKTION)
                {
                    if (funktion == false)
                    {
                        ASM_FUNKTION = false;
                        Console.WriteLine($"[]->{ASM_FUNKTION_BUFFER}");
                        ASM_FUNKTION_BUFFER = "";
                    }
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
    }
}