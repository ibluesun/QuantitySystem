using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting.Shell;


public class QsCommandLine : CommandLine
{
    
    protected override string Prompt
    {
        get
        {

            return "Qs> ";
        }
    }


    protected override ICommandDispatcher CreateCommandDispatcher()
    {
        Console.WriteLine("Command Dispatcher", Style.Prompt);

        return base.CreateCommandDispatcher();
    }

    protected override int RunCommand(string command)
    {
        Console.WriteLine(command, Style.Prompt);

        return base.RunCommand(command);
    }
    
}
