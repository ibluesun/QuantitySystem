using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting.Shell;


internal class QsHost:ConsoleHost
{
    protected override Type Provider
    {
        get
        {
            return typeof(QuantitySystem.Runtime.QsContext);
        }
    }

    protected override CommandLine CreateCommandLine()
    {
        return new QsCommandLine();

    }
    
    protected override void PrintHelp()
    {
    }

    [STAThread]
    public static int Main(string[] args)
    {

        //return new QsHost().Run(args);
        QuantitySystemCalculator.Program.OldMain(args);
        return 0;


        
    }

}
