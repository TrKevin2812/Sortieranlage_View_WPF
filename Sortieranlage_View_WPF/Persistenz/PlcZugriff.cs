using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sortieranlage_View_WPF.Model
{
    internal class PlcZugriff
    {

       
        public PlcZugriff()
        {
            ConnectionToPlc();
        }

        public static Plc ConnectionToPlc()
        {
            Plc _plc = new Plc(CpuType.S71500, "192.168.60.124", 0, 0);

            try
            {
               _plc.Open();
                return _plc;
            }
            catch (Exception ex)
            {
                throw new Exception($"Verbindung konnte nicht aufgebaut werden");
            }
        }

        public static Object[] ReadBoolDB(int db, int startOffSet, int dataSize, Plc plc)
        {
           
            Object[] boolvalue = new Object[dataSize];

            try
            {
                for (int i = 0; i < dataSize; i++)
                {
                    if (plc.IsConnected)
                    {
                        boolvalue[i] = (Object)plc.Read($"DB{db}.DBX{startOffSet}.{i}");
                        
                    }

                }

            }
            catch (Exception ex)
            {

                throw new Exception($"Fehler beim auslesen: {ex.Message}");
            }

            return boolvalue;
        }

        public static UInt16 ReadUInt16DB(int db, int startOffSet, Plc plc)
        {

            UInt16 ret = 0;

            try
            {
                    if (plc.IsConnected)
                    {
                        ret = (UInt16)plc.Read($"DB{db}.DBW{startOffSet}");
                    }
            }
            catch (Exception ex)
            {

                throw new Exception($"Fehler beim auslesen: {ex.Message}");
            }

            return ret;
        }

        public void Close(Plc plc)
        {
            plc.Close();
        }
    }
}
