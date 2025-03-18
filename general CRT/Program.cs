using System;
using System.Linq;
using System.Runtime.InteropServices;


class Program
{
    // we make a greater common divisor function to check coprimeness
    // https://stackoverflow.com/questions/18541832/c-sharp-find-the-greatest-common-divisor
    // i used algorithm by this website

    // it is called euler algortihm.
    // altough i copypasted it, i know how it works now
    // way it works is assume 2 numbers that a > b
    // instead of finding gcd(a,b), we go for gcd(b,a mod b)
    // we repeat untill a mod b or b mod a reaches 0

    // example:
    // gcd of 85 and 45 which is gcd(85,45)
    // which is equal to gcd(45, 85mod45) which is gcd(45,40)
    // we take that and turn it into gcd(40, 45mod40) which is gcd(40, 5)
    // we take that and we have 40mod5 which is 0
    // which means gcd is 5
    static int GCD(int num1, int num2)
    {
        while (num1 != 0 && num2 != 0)
        {
            if (num1 > num2)
                num1 %= num2;
            else
                num2 %= num1;
        }
        return num1 | num2;
    }

    // this is the part we find the CRT

    /*
plan 

Example:
1mod12
2mod5

Make big for loop
First iteration is 1 (a)
Second iteration is 13 (a+n)
.
.
.
Last iteration is 1+12*(5-1) [a+n*(m-1)]
5 (m) iterations in total

In iterations take mod5(modm) of the number
If the answer is equal to b (2), return
Else continue 
 ^ this works for 2 modules, but not for list of modules.

    */

    static long CRT(List<int> modulesA, List<int> modulesN)
    {
        // we store the first modules variables
        // variables = a (remainder), n (modulo). amodn

        long firstModuleA = modulesA[0];
        long firstModuleN = modulesN[0];

        // we start from second variables
        for (int i = 1; i < modulesA.Count; i++)
        {
            // we extract the [i]th variables
            long currnetModuleA = modulesA[i];
            long currentModuleN = modulesN[i];

            // we take the remainder of the module with the second module
            
            while (firstModuleA % currentModuleN != currnetModuleA % currentModuleN)
            {
                // we increase the first remainder by its own module untill we reach the remainder of other module is equal
                // like this 5mod6 and 2mod7
                // 5, 11, 16, 21 (it doesnt stop here) but the while loop ends
                // we find out that 21mod7 is 2 so we stop it
                firstModuleA += firstModuleN;

                // here, after the while loop resolves, we get 21 which is CRT.
                // and it stored into firstModuleA
                // and then the next ith module will be compared with this new firstModuleA
            }

            // this is the part where we progresively increase the modulos.
            // we had 5mod6 and 2mod7, we multiply the N values and achive 42
            // our next module which is 21mod42 is stored as the first module
            firstModuleN *= currentModuleN;
        }

        // after all of the iterations are complete, we return the A part of the new module.
        return firstModuleA;
    }


    // main code 
    static void Main()
    {
        string tempString = "";
        int tempIndex = 0;
        
        

        
        Console.WriteLine("this is a basic CRT solving program that solves entered modules and finds x that satistifies all\n\nfirst of all, you need to enter the amount of modules you want to enter\nafter entering you amount, enter modules as amodb\nexample: 3mod5");
        // we get amount of modules
        int amountofModules = int.Parse(Console.ReadLine());
        Console.WriteLine($"\nyou entered{amountofModules}.\n");

        // we create 2 arrays for bot a's and n's for amodn with the size of amountofmodules
        var listofAs = new List<int>(amountofModules);
        var listofNs = new List<int>(amountofModules);



        for (int i = 0; i < amountofModules; i++)
        {
            Console.WriteLine($"enter your {i+1}th module.\n");

            string moduleInput = Console.ReadLine();

            // this code extractes the numbers from input

            // this loops untill every character in the string is readed
            foreach (char value in moduleInput)
            {
                // we can check if the char we are looking at is number or not with isdigit function
                // then we write the entered value as string
                if (char.IsDigit(value))
                {
                    tempString += value;
                }
                // if the number is not digit then we throw it into else which throws it into another if function
                else if (tempString.Length > 0)
                {
                    if (tempIndex == 0)
                    {
                        listofAs.Add(int.Parse(tempString));
                        tempIndex++;
                    }
                    // we take the values untill we reach a non int value, then we reset the temp
                    tempString = "";
                }
            }

            // and we store the rest of the numbers here
            if (tempString.Length > 0)
            {
                listofNs.Add(int.Parse(tempString));
            }

            Console.WriteLine($"your {i+1}th module is: {listofAs[i]}mod{listofNs[i]}\n\n");

        // resetting both temp values dor the next module
        tempString = "";
        tempIndex = 0;
        }


        // at this point, we have listed values of modules
        // now we need to find the x for CRT that satistifies all
        // we check it one by one 1-2  1-3  1-4  1-5   2-3  2-4  2-5   3-4  3-5   4-5
        // _____________________ [0,1][0,2][0,3][0,4] [1,2][1,3][1,4] [2,3][2,4] [3,4]
        // first of all, we check if the n numbers are coprime numbers
        for (int i = 0; i < amountofModules - 1; i++)
        {
            for (int j = i+1; j < amountofModules; j++)
            {
                if (GCD(listofNs[i], listofNs[j]) != 1)
                {
                    Console.WriteLine($"\n\n{listofNs[i]} and {listofNs[j]} are not coprime");
                    return;
                }
            }
        }
        Console.WriteLine("\n");

        // instead of checking and storing all the values one by one and progresively changing x from here, we feed the function the list of variables instead
        long answer = CRT(listofAs, listofNs);
        
        

        // prints the answer
        Console.WriteLine($"answer x which satisfies all is:\n{answer}");







    }
}