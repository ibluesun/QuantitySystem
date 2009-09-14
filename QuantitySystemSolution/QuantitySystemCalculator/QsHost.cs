using System;
using Microsoft.Scripting.Hosting.Shell;


internal class QsHost : ConsoleHost
{
    protected override Type Provider
    {
        get
        {
            return typeof(Qs.Runtime.QsContext);
        }
    }

    protected override CommandLine CreateCommandLine()
    {
        return new QsCommandLine();

    }
    


    protected override void UnhandledException(Microsoft.Scripting.Hosting.ScriptEngine engine, Exception e)
    {
        base.UnhandledException(engine, e);
    }

    
    

    [STAThread]
    public static int Main(string[] args)
    {
        

        if (Environment.GetEnvironmentVariable("TERM") == null)
        {
            Environment.SetEnvironmentVariable("TERM", "dumb");
        }

        Console.BackgroundColor = ConsoleColor.DarkMagenta;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();

        QsCommands.StartConsole();

        return new QsHost().Run(args);
        //QuantitySystemCalculator.Program.OldMain(args);
        //return 0;



    }



}
