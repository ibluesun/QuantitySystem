using System;
using Microsoft.Scripting.Hosting.Shell;
using ParticleLexer.StandardTokens;
using System.Collections.Generic;


internal class QsHost : ConsoleHost
{
    protected override Type Provider
    {
        get
        {
            return typeof(Qs.Scripting.QsContext);
        }
    }

    protected override CommandLine CreateCommandLine()
    {
        return new QsCommandLine();

    }
    
    [STAThread]
    public static int Main(string[] args)
    {
        
        if (Environment.GetEnvironmentVariable("TERM") == null)
        {
            Environment.SetEnvironmentVariable("TERM", "dumb");
        }

        //Console.BackgroundColor = QsCommands.BackgroundColor;
        //Console.ForegroundColor = QsCommands.ForegroundColor;
        //Console.Clear();

        //Console.WindowWidth = 110;
        //Console.WindowHeight = 34;
        
        //Console.BufferWidth = 110;

        QsCommands.StartConsole();


        QuantitySystem.DynamicQuantitySystem.AddDynamicUnitConverterFunction("Currency", QsRoot.Currency.CurrencyConverter);


        return new QsHost().Run(args);

    }


}
