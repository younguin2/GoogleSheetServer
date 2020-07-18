using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleSheetServer
{
    [System.Serializable]
    public class ResPacket<T>
    {
        public bool success = false;
        public string error;

        public T[] data;
    }
}
