using S7.Net;
using Sortieranlage_View_WPF.Persistenz;

namespace Sortieranlage_View_WPF.Model
{
    class KompressorModel
    {
        private bool _activated = false;

        public bool Activated
        {
            get { return _activated; }
            set { _activated = value; }
        }
        public KompressorModel()
        {

        }

        public void SwitchMode(bool boolvalue, Plc plc)
        {
            
            PlcKompressor.WriteBoolInDB(boolvalue, plc);
            Activated = boolvalue;

        }

    }
}
