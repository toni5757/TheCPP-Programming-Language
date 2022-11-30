# Welcome to TheCPP-Language-Code programming language
## Small Description
This is a simple programming languag written by TheCPP in under 1.000 lines of code.
It includes:
>preprocessor definitions
>variables (using is in process)

And I want to add much more things.
### [TODO]
|Type of change| Done|
|--|--|
| Variable dectaction | X|
| Variable using| X |
| Funktion dectaction | X |
| Funktion calling| X |
| Funktion using |X |
| Inline Assembler| Done |

## Using

 1. You must compile the code with Visual Studio.
 2. Then you can write your TheCPP-Language-Code in a file named by **Programm.txt**
 3. Now you can compile the TheCPP-Language-Code. The compiler print the created the assembl code
 4. Then you must only assemlber the code and you can run it.
## Example   
### Hello World

    function main() {
	    print("Hello World!");
	}
It´s create a function named by **main**.
In that function we call the **print** function that prints **Hello World!**
When you run this programm:

    >TheCPP-Language-Code HelloWorld.txt HelloWorld
    Hello World!

### Preprocessor definition

    #define HELLO_WORLD "Hello World"
    function main() {
	    print(HELLO_WORLD);
    }
When you run it, it prints Hello World.

    >TheCPP-Language-Code HelloWorld.txt HelloWorld
    Hello World!
## Bit of words
I hope that you download my language.  If you detect an error please report that. Much is in progress. Thank you for reading.
