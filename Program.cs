using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
class defines {
    private List<string> name = new List<string>();
    private List<string> what = new List<string>();
    public void Add(string name, string what) {
        this.name.Add(name);
        this.what.Add(what);
    }
    public string get(string name)
    {
        int index = this.name.BinarySearch(name);
        return this.what[index];
    }
    public bool isDef(string name)
    {
        bool result = false;
        return result;
    }


    public void print() {
        for (int i = 0; i < this.name.Count; i++)
        {
            Console.WriteLine($"#define {name[i]} {what[i]}");
        }
    }
    public List<string> getWhats()
    {
        return this.name;
    }
}
class stack
{
    private List<int> STACK = new List<int>();
    public void push(int item)
    {
        this.STACK.Add(item);
    }
    public int cop()
    {
        return this.STACK[STACK.Count - 1]; ;
    }
    public int pop()
    {
        int result = this.cop();
        this.STACK.Remove(result);
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

        //In Build Define funktion
        bool define = false;
        bool ifdef = false;
        bool ifndef = false;
        bool endif = false;

        stack ifdefs = new stack();
        ifdefs.push(1);

        int definePos = 0;
        string what = "";
        string toWhat = "";
        defines defs = new defines();

        //defintions
        string result = "";

        //Comments
        bool ignore     = false;
        bool big_ignore = false;

        //Vars
        bool funktion   = false;
        bool varDef     = false;
        int defin       = 0;
        string varType  = "";
        string varName  = "";
        string varData  = "";
        //Strings
        bool isInString = false;

        foreach (List<string> line in tockens)
        {
            int instring = 0;
            for (int i = 0; i < line.Count; i++)
            {
                if (ifdefs.cop() == 1)
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

                    if (defs.getWhats().Contains(tocken)) //for #define
                    {
                        tocken = tocken.Replace(defs.get(tocken), "");
                    }

                    //######### STRING #########//
                    bool sonderfall = tocken_bevor == "\\" || t_b_b == "\\";
                    if (tocken == "\"" && !sonderfall)
                    {
                        instring = 0;
                        isInString = !isInString;
                    }
                    //PRINT (can delte)
                    if (isInString && !ignore)
                    {
                        if (instring >= 1)
                        {
                            //#### ASM FUNKTION ####//
                            if (ASM_FUNKTION)
                            {
                                ASM_FUNKTION_BUFFER += tocken;
                            }
                            //#### ASM FUNKTION ####//
                            else
                            {
                                //Console.Write(tocken); // print when it´s in string
                            }
                        }
                        instring++;
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
                            if (tocken == "/" && tocken_bevor == "*")
                            {
                                big_ignore = false;
                                ignore = false;
                            }
                        }
                    }
                    //######COMMENTS  END########//

                    if (!isInString && !ignore /*&& !ASM_FUNKTION && !define*/)
                    {
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
                            //#### DEFINE START ####//
                            if (tocken == "#define")
                            {
                                define = true;
                            }
                            if (tocken == "#ifdef")
                            {
                                ifdef = true;
                            }
                            if (tocken == "#ifndef")
                            {
                                ifndef = true;
                            }
                            if (tocken == "#endif")
                            {
                                endif = true;
                            }
                            //#### DEFINE END ####//
                        }
                        else
                        {
                            if (tocken == ")") funktion = false; //funktion dectaction
                        }
                        //##### VAR    DETACTION #####//
                        if (varDef && tocken != " " && tocken != "" && tocken != "=" && !ifdef && !define) //Verbugt
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
                                if (tocken != ";")
                                {
                                    varData = tocken.Replace(";", "");
                                    if (varData == "{")
                                    {
                                        if (varType != "string") varData = "0";
                                        else varData = "";
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
                            result += $"mov [{variablen.getAdr(varName)}], {varData}" + "\n";
                            //Console.WriteLine($"[]->mov [{variablen.getAdr(varName)}], {varData}");
                            varName = "";
                            varType = "";
                            varData = "";
                        }
                        //##### VAR DETACTION END #####//
                    }
                    //#### ASM FUNKTION ####//
                    if (ASM_FUNKTION)
                    {
                        if (funktion == false)
                        {
                            ASM_FUNKTION = false;
                            result += ASM_FUNKTION_BUFFER + "\n";
                            //Console.WriteLine($"[]->{ASM_FUNKTION_BUFFER}");
                            ASM_FUNKTION_BUFFER = "";
                        }
                    }
                    //#### ASM FUNKTION ####//
                    //#### DEFINE START ####//
                    if (define)
                    {
                        if (tocken != " ") definePos++;
                        if (definePos == 0) what = tocken;
                        else toWhat += tocken;
                    }
                    else if (ifdef)
                    {
                        if (defs.getWhats().Contains(tocken)) ifdefs.push(1);
                        else ifdefs.push(0);
                    }
                    else if (ifndef)
                    {
                        if (!defs.getWhats().Contains(tocken)) ifdefs.push(1);
                        else ifdefs.push(0);
                    }
                    else if (endif)
                    {

                    }
                    //#### DEFINE END ####//
                }
            }
            //After Line: -->CLEAN UP<--
            if (!big_ignore) ignore = false; //Comments
            if (define)
            {
                toWhat = toWhat.Replace("#define ", "");
                if (toWhat == "") toWhat = "true";
                defs.Add(what, toWhat);
            }
            define = false; //end of #define somthings something
            ifdef = false;
            ifndef = false;
            endif = false;
            definePos = 0;
            what = "";
            toWhat = "";
        }

        return result;
    }
    public static void Main(string[] args)
    {
        List<List<string>> lines = ToLines("Programm.txt");
        Console.WriteLine(toAsm(lines));
        Input("Press any key to continue ...");
    }
}