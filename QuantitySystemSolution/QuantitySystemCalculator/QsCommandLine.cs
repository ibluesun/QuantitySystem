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


    public static string LastLine { get; set; }

    protected override string ReadLine(int autoIndentSize)
    {
        LastLine =  base.ReadLine(autoIndentSize);
        return LastLine;
    }
    
}
