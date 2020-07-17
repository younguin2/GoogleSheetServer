using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GoogleSheetServer
{
    public static class GoogleSheetServerManager
    {
        public static void InsertData(string sheetName)
        {
            StaticCoroutine.DoCoroutine(NetRequestCoroutine(sheetName));
        }


        static IEnumerator NetRequestCoroutine(string sheetName)
        {
            WWWForm form = new WWWForm();
            form.AddField("device_id", SystemInfo.deviceUniqueIdentifier);
            form.AddField("name", "Guest");

            UnityWebRequest www = UnityWebRequest.Post(string.Format("{0}?sheet_name={1}", GoogleServerSettings.Instance.url,sheetName) , form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
