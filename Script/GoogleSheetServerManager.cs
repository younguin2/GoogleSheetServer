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
            StaticCoroutine.DoCoroutine(InsertDataCoroutine(sheetName));
        }


        static IEnumerator InsertDataCoroutine(string sheetName)
        {
            float startTime = Time.time;

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
                Debug.Log("Form upload complete!" + " : " +(Time.time - startTime));
            }
        }



        public static void GetData(string sheetName)
        {
            StaticCoroutine.DoCoroutine(GetDataCoroutine(sheetName));
        }


        static IEnumerator GetDataCoroutine(string sheetName)
        {
            float startTime = Time.time;

            UnityWebRequest www = UnityWebRequest.Get(string.Format("{0}?sheet_name={1}", GoogleServerSettings.Instance.url, sheetName));

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
