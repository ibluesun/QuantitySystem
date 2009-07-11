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
    
}
