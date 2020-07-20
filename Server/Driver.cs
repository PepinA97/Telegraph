using MyServer.Networking;
using System;

namespace MyServer
{
    class Driver
    {
        static void Main(string[] args)
        {
            ParseCommandlineArguments(args);

            Server.Start();

            TakeConsoleInput();
        }

        static void ParseCommandlineArguments(string[] arguments)
        {
            // Take port
            for(int i = 0; i < arguments.Length; i++)
            {
                Console.Out.WriteLine(arguments[i]);

                int number;
                switch (arguments[i])
                {
                    case "-p":
                        bool success;

                        try
                        {
                            success = int.TryParse(arguments[i + 1], out number);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.Out.WriteLine("No value supplied for '-p': ");

                            continue;
                        }

                        if (success)
                        {
                            Server.AssignPort(number);
                        }
                        else
                        {
                            Console.Out.WriteLine("Invalid value for '-p': " + arguments[i + 1]);
                        }

                        i++;

                        break;
                    default:
                        Console.Out.WriteLine("Unrecognized argument: " + arguments[i]);

                        break;
                }
            }
        }

        static void TakeConsoleInput()
        {
            Console.Out.WriteLine("Enter \"exit\" to close the application.");

            string input;
            while (true)
            {
                input = Console.In.ReadLine();

                switch (input)
                {
                    // Put commands for database manipulation
                    case "exit":
                        Server.Stop();

                        return;
                    default:
                        Console.Out.WriteLine("The command \"" + input + "\" does not exist.");
                        break;
                }
            }
        }
    }
}
