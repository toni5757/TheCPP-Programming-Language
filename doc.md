# Documentation
## Hello World!
It´s a tradition of programming to make a Hello World programm. In my programming language the code of this programm is that:

    function main() {
	    print("Hello World!");
	}
So that was it.
It prints when you run it "Hello World".

    >TheCPP-Language-Compiler HelloWorld.txt
    Hello World!
   
## Comments
It´s exists only a few (2) types of comments.
One line comments. Comments for one line designed for small line description

    //comment
    print("Hello World!"); //Print´s Hello World
 Multi line comment. For more than one upto unlimeted lines that ignored. You can also comment only characters and not lines.
 

    /*Multi
    Line
    comment
    */
    print(/*Comment*/"ABC");

## Preprocessor
The language gives you a small but powerful preprocessor set.
>#define
> #ifdef
> #ifndef
> #endif

### #define
You can use define, for define const things. Defined things will never ignored. When you define a function named as a bevor defined define the funktion is the bevor defined define.

    #define ABC "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    print(ABC);
When you run it:

    >TheCPP-Language-Code defineTest.txt
    ABCDEFGHIJKLMNOPQRSTUVWXYZ
 When you define a define with nothing:
 

    #define ABC
then it will ever the same thing you write:

    #define ABC //#define ABC 1
### #ifdef
Check if a thing is defined. Compilies the appropriate block of code only when the thing is defined.

    #define run
    #ifdef run
    print("Hello World");
    #endif

> You must ever use at the end of a block `#endif

So. When you compile that:

    >TheCPP-Language-Code ifdefTest.txt
    HelloWorld
When you delte the `#define run` then it will print nothing.
### ifndef
Does the same thing as `#ifdef`. But in negativ.

    #ifndef TEST
    print("TEST is not defined\nI define now TEST");
	#define TEST "TEST"
	#endif
When you run that:

    >TheCPP-Language-Code ifndefTest.txt
    TEST is not defined
    I define now TEST

### endif
Says that a ifdef/ifndef area it at end
A small example:

    print("ABC");
    #ifdef TEST
    print("DEFG");
    #endif
    print("HIJKL");
  When you run that it will print you ABCHIJKL:
  

    >TheCPP-Lanuage-Code endifTest.txt
    ABCHIJKL

## Inline Assembler
My TheCPP-Language-Code includes a inline assembler.
You can use it simple with `asm`.

    asm("nop"); //do nothing
  So it works:
  

 - It´s look what´s between the cling
 - copy that into the assembler code
