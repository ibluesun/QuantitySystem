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


    protected override string ReadLine(int autoIndentSize)
    {
        Qs.Runtime.QsScriptCode.LastLine =  base.ReadLine(autoIndentSize);
        return Qs.Runtime.QsScriptCode.LastLine;
    }

    protected override void UnhandledException(Exception e)
    {
        if (e is Qs.QsException) PrintError(e);
        else
        base.UnhandledException(e);
    }

    public void PrintError(Exception e)
    {
        
        ConsoleColor cc = System.Console.ForegroundColor;

        System.Console.ForegroundColor = ConsoleColor.Red;
        Console.ErrorOutput.WriteLine("{0}: {1}, {2}", e.GetType().Name, e.InnerException.GetType().Name, e.Message);

        System.Console.ForegroundColor = ConsoleColor.White;
    }

}
