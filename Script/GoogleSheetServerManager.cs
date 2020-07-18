using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

namespace GoogleSheetServer
{
    public static class GoogleSheetServerManager
    {
        private static Dictionary<string, string> ConvertClassToDictionary(System.Object obj)
        {
            return obj.GetType().GetFields().ToDictionary(x => x.Name, x => x.GetValue(obj)?.ToString() ?? "");
        }

        public static void InsertData<T>(string sheetName, T insertData, System.Action<ResPacket<T>> callback = null)
        {
            StaticCoroutine.DoCoroutine(InsertDataCoroutine(sheetName, insertData, callback));
        }

        static IEnumerator InsertDataCoroutine<T>(string sheetName, T insertData, System.Action<ResPacket<T>> callback)
        {
            //보내는 데이터 만들기
            WWWForm form = new WWWForm();
            Dictionary<string, string> dataDic = ConvertClassToDictionary(insertData);

            foreach (string key in dataDic.Keys)
                form.AddField(key, dataDic[key]);

            //통신 시작
            UnityWebRequest www = UnityWebRequest.Post(string.Format("{0}?sheet_name={1}", GoogleServerSettings.Instance.url,sheetName) , form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(string.Format("Packet Received : {0}", www.downloadHandler.text));
                callback?.Invoke(JsonUtility.FromJson<ResPacket<T>>(www.downloadHandler.text));
            }
        }



        public static void GetData<T>(string sheetName, System.Action<ResPacket<T>> callback = null)
        {
            StaticCoroutine.DoCoroutine(GetDataCoroutine(sheetName, callback));
        }


        private static IEnumerator GetDataCoroutine<T>(string sheetName, System.Action<ResPacket<T>> callback)
        {
            UnityWebRequest www = UnityWebRequest.Get(string.Format("{0}?sheet_name={1}", GoogleServerSettings.Instance.url, sheetName));

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(string.Format("Packet Received : {0}", www.downloadHandler.text));
                callback?.Invoke(JsonUtility.FromJson<ResPacket<T>>(www.downloadHandler.text));
            }
        }
    }
}
