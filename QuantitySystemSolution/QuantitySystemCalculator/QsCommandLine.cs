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
        
        Qs.Runtime.QsScriptCode.LastLine = base.ReadLine(autoIndentSize);



        if (!string.IsNullOrEmpty(Qs.Runtime.QsScriptCode.LastLine))
        {
            QsCommands.Engine = this.Engine;

            QsCommands.ScriptScope = this.ScriptScope;


            QsCommands.CheckCommand(Qs.Runtime.QsScriptCode.LastLine, this.Scope);

            if (QsCommands.CommandProcessed)
            {
                QsCommands.CommandProcessed = false;
                return string.Empty;
            }
        }

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

        System.Console.ForegroundColor = QsCommands.ExceptionColor;

        
        
        Console.ErrorOutput.WriteLine("{0}: {1}", e.GetType().Name, e.Message);

        if (e.InnerException != null)
        {
            Console.ErrorOutput.WriteLine("{0}: {1}", e.InnerException.GetType().Name, e.InnerException.Message);
        }
        
        System.Console.ForegroundColor = QsCommands.ForegroundColor;
    }

}
