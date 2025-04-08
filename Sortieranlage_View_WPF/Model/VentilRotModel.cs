using S7.Net;
using Sortieranlage_View_WPF.Persistenz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortieranlage_View_WPF.Model
{
    class VentilRotModel
    {
        private bool _activated = false;
        public bool Activated
        {
            get { return _activated; }
            set { _activated = value; }
        }
        public VentilRotModel()
        {

        }

        public void SwitchMode(bool boolvalue, Plc plc)
        {
            PlcVentilRot.WriteBoolInDB(boolvalue, plc);
            Activated = boolvalue;
        }
        
    }
}
