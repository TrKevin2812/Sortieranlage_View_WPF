using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortieranlage_View_WPF.Model
{
    class PlcZugriffModel
    {
        
        public PlcZugriffModel()
        {
            
        }

        public static Plc ConnectionToPlc()
        {
            return PlcZugriff.ConnectionToPlc();
        }

        public static Object[] ReadDB(int db, int startOffSet, int dataSize, Plc plc)
        {
            return PlcZugriff.ReadBoolDB(db, startOffSet, dataSize, plc);
        }
        public static UInt16 ReadUInt16DB(int db, int startOffSet, Plc plc)
        {
            return PlcZugriff.ReadUInt16DB(db, startOffSet, plc);
        }

    }
}
