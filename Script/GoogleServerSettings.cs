using UnityEngine;
//using UnityEditor;
using System.IO;

namespace GoogleSheetServer
{
    [CreateAssetMenu]
    public class GoogleServerSettings : ScriptableObject
    {
        private static readonly string settingsFile = "GoogleServerSettings";

        private static GoogleServerSettings instance;

        public static GoogleServerSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GoogleServerSettings>(settingsFile);
                }

                return instance;
            }
        }



        [Header("GoogleServer Settings")]

        [TextArea(3, 5)]
        [Tooltip("웹앱에 배포된 주소")]
        public string url = "";
    }
}

