using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;

namespace QsRoot.Processor
{
    public class EightBitProcessor
    {
        public QsValue AF { get; set; }
        public QsValue BC { get; set; }
        public QsValue DE { get; set; }
        public QsValue HL { get; set; }

        public QsValue IX { get; set; }
        public QsValue IY { get; set; }

        public QsValue SP { get; set; }
        public QsValue PC { get; set; }

        public EightBitProcessor()
        {
            AF = QsScalar.Zero;
            BC = QsScalar.Zero;
            DE = QsScalar.Zero;
            HL = QsScalar.Zero;
            IX = QsScalar.Zero;
            IY = QsScalar.Zero;
            SP = QsScalar.Zero;
            PC = QsScalar.Zero;
        }

        /// <summary>
        /// this function takes the object of z80 and return its PC  (Pointer Counter)
        /// </summary>
        /// <param name="z80"></param>
        /// <returns></returns>
        public static QsValue GetPC(EightBitProcessor epc)
        {
            return epc.PC;
        }


    }
}
