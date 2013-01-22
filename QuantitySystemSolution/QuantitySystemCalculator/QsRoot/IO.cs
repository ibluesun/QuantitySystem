using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;
using Qs.Types;
using System.IO;
using Qs;

namespace QsRoot
{
    /// <summary>
    /// Standard I/O for quantity system.
    /// </summary>
    public static class IO
    {

        public static QsValue InputLine(string prompt)
        {           
            Console.Write(prompt);
            return InputLine();
        }

        public static QsValue InputLine()
        {
            return new QsText(Console.ReadLine());
        }
    
        public static QsValue PrintLine([QsParamInfo(QsParamType.Text)] QsParameter val)
        {
            Console.WriteLine(val.UnknownValueText);
            return null;
        }

        public static QsValue ReadBinaryFile([QsParamInfo(QsParamType.Text)]QsParameter fileName)
        {
            byte[] data = File.ReadAllBytes(fileName.UnknownValueText);

            return data.ToQsVector();
        }

        public static QsValue Speak(
            [QsParamInfo(QsParamType.Text)] QsParameter text
            )
        {
            System.Speech.Synthesis.SpeechSynthesizer ss = new System.Speech.Synthesis.SpeechSynthesizer();

            ss.SpeakAsync(text.UnknownValueText);

            return null;
        }
    }
}
