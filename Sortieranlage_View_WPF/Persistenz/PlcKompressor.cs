using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortieranlage_View_WPF.Persistenz
{
    class PlcKompressor
    {
        public static void WriteBoolInDB(bool boolvalue, Plc plc)
        {

            try
            {
                if (plc.IsConnected)
                {
                    plc.Write($"DB1.DBX0.3", boolvalue);
                }

            }
            catch (Exception)
            {

                throw new Exception($"Fehler beim Senden");
            }
        }
    }
}
