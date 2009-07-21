﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using QuantitySystem;
using QuantitySystem.Units;
using Qs.Runtime;

namespace Qs
{
    public static class QsCommands
    {

        public static bool CommandProcessed;


        /// <summary>
        /// Console Commands.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool CheckCommand(string command, QsEvaluator qse)
        {
            string[] commands = command.ToLower().Split(' ');

            if (commands[0] == "quit") return false;

            if (commands[0] == "exit")
            {
                Console.WriteLine("Press CTRL+Z to exit");
                CommandProcessed = true;
            }

            if (commands[0] == "help")
            {
                PrintHelp();
                CommandProcessed = true;
            }

            if (commands[0] == "list")
            {
                string param = string.Empty;

                if (commands.Length < 2)
                {
                    ListVariables(qse);
                }
                else
                {
                    if (commands[1] == "quantities")
                        ListQuantities();

                    if (commands[1] == "units")
                    {
                        if (commands.Length < 3)
                        {
                            ListUnits(string.Empty);
                        }
                        else
                        {
                            ListUnits(commands[2]);
                        }
                    }
                }

                CommandProcessed = true;
            }

            if (commands[0] == "new")
            {
                qse.New();
                CommandProcessed = true;
            }

            if (commands[0] == "cls")
            {
                Console.Clear();
                CommandProcessed = true;
            }

            if (commands[0] == "copyright")
            {
                PrintCopyright();
                CommandProcessed = true;
            }



            return true;

        }



        #region Console commands

        public static void StartConsole()
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Clear();

            PrintCopyright();

            Console.WriteLine();
            Console.WriteLine("Type \"help\" for more information.");

            Console.ForegroundColor = ConsoleColor.White;

        }

        internal static void PrintCopyright()
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            var qsc_ver = (AssemblyFileVersionAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];

            var lib_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(QuantityDimension)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];

            Console.WriteLine("Quantity System Framework  ver " + lib_ver.Version);

            Console.WriteLine("Quantity System Calculator ver " + qsc_ver.Version);


            var qsc_cwr = (AssemblyCopyrightAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
            Console.WriteLine(qsc_cwr.Copyright);
            Console.WriteLine();
            Console.WriteLine("http://QuantitySystem.CodePlex.com");
            Console.WriteLine("-----------------------------------------------------------");

            Console.WriteLine("Email: Ahmed.Sadek@LostParticles.net; Ahmed.Amara@gmail.com");

            Console.ForegroundColor = ConsoleColor.White;

        }

        /// <summary>
        /// Help Command.
        /// </summary>
        internal static void PrintHelp()
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("    Type \"<unit>\" for information [case sensitive]");
            Console.WriteLine("         Example: <kn> for knot");
            Console.WriteLine();
            Console.WriteLine("       - \"var = number[<unit>]\" for quantity");
            Console.WriteLine("         Omit <unit> for dimensionless quantity");
            Console.WriteLine();
            Console.WriteLine("       - \"f(x,y,z) = x+y+z\" to make a function");
            Console.WriteLine();
            Console.WriteLine("       - \"var = number[Quantity Name]\" ");
            Console.WriteLine("         to make a variable with default SI units of the quantity");
            Console.WriteLine();
            Console.WriteLine("       - \"variable name\" alone to show its information");
            Console.WriteLine();
            Console.WriteLine("       - \"<unit> to <unit>\" i.e. <m/s> to <kn> for conversion factor");
            Console.WriteLine();
            Console.WriteLine("       - \"List \" for list of variables");
            Console.WriteLine("         \"List [Quantities]\" for list of available Quantities");
            Console.WriteLine("         \"List [[Units] [Quantity Name]]\" for list of available Units ");
            Console.WriteLine();
            Console.WriteLine("       - \"New\" to clear all variables.");
            Console.WriteLine();
            Console.WriteLine("       - \"Cls\" to clear the screen.");
            Console.WriteLine();
            Console.WriteLine("       - \"copyright\", and \"CTRL+Z\" to terminate the console.");


            Console.ForegroundColor = ConsoleColor.White;


        }


        /// <summary>
        /// List Command
        /// </summary>
        internal static void ListVariables(QsEvaluator qe)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (string var in qe.VariablesKeys)
            {
                Console.WriteLine("    " + var + "= " + qe.GetVariable(var).ToString());
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        /// <summary>
        /// List Quantities Command
        /// </summary>
        internal static void ListQuantities()
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine();

            foreach (Type QType in QuantitySystem.QuantityDimension.CurrentQuantitiesDictionary.Values)
            {
                Console.WriteLine("    " + QType.Name.Substring(0, QType.Name.Length - 2));
            }
            Console.ForegroundColor = ConsoleColor.White;
        }


        /// <summary>
        /// List Units Command
        /// </summary>
        internal static void ListUnits(string quantity)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Type utype in Unit.UnitTypes)
            {
                QuantitySystem.Attributes.UnitAttribute ua = Unit.GetUnitAttribute(utype);
                if (ua != null)
                {
                    string uname = utype.Name.PadRight(16);
                    string symbol = "<" + ua.Symbol + ">";
                    symbol = symbol.PadRight(10);
                    string system = utype.Namespace.Substring("QuantitySystem.Units".Length + 1).PadRight(16);

                    string qtype = ua.QuantityType.ToString().Substring(ua.QuantityType.Namespace.Length + 1).TrimEnd("`1[T]".ToCharArray());

                    if (string.IsNullOrEmpty(quantity))
                    {

                        Console.WriteLine("    " + uname + " " + symbol + " " + system + qtype);
                    }
                    else
                    {
                        //print if only the unit is for this quantity
                        if (qtype.Equals(quantity, StringComparison.InvariantCultureIgnoreCase)) Console.WriteLine("    " + uname + " " + symbol + " " + system + qtype);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion

    }
}