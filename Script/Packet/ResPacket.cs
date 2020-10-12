using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleSheetServer
{
    [System.Serializable]
    public class GoogleSheetResPacket
    {
        public bool success = false;
        public string error;
    }

    [System.Serializable]
    public class GoogleSheetReqPacket
    {

    }
}
