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
            Dictionary<string, string> fieldInfo = new Dictionary<string, string>();
            AddClassField(fieldInfo, obj);
            return fieldInfo;
        }

        private static void AddClassField(Dictionary<string, string> fieldInfo, System.Object obj)
        {
            System.Reflection.FieldInfo[] informations = obj.GetType().GetFields();

            for(int i = 0; i < informations.Length; i++)
            {

                if (IsComplex(informations[i].GetValue(obj).GetType()))
                {
                    AddClassField(fieldInfo, informations[i].GetValue(obj));
                }
                else
                {
                    if (fieldInfo.ContainsKey(informations[i].Name))
                        continue;

                    fieldInfo.Add(informations[i].Name, informations[i].GetValue(obj).ToString() ?? "");
                }
            }
        }

        public static bool IsComplex(System.Type typeIn)
        {
            if (typeIn.IsSubclassOf(typeof(System.ValueType)) || typeIn.Equals(typeof(string))) //|| typeIn.IsPrimitive
                return false;
            else
                return true;

        }

        public static void Post<T>(string mathod, GoogleSheetReqPacket insertData, System.Action<T> callback = null) where T : GoogleSheetResPacket
        {
            StaticCoroutine.DoCoroutine(PostCoroutine(mathod, insertData, callback));
        }

        static IEnumerator PostCoroutine<T>(string mathod, GoogleSheetReqPacket insertData, System.Action<T> callback) where T : GoogleSheetResPacket
        {
            //보내는 데이터 만들기
            WWWForm form = new WWWForm();
            form.AddField("mathod", mathod);

            //클래스 데이터로 인풋
            Dictionary<string, string> dataDic = ConvertClassToDictionary(insertData);

            foreach (string key in dataDic.Keys)
                form.AddField(key, dataDic[key]);


            //통신 시작
            UnityWebRequest www = UnityWebRequest.Post(string.Format("{0}", GoogleServerSettings.Instance.url) , form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(string.Format("Packet Received : {0}", www.downloadHandler.text));
                callback?.Invoke(JsonUtility.FromJson<T>(www.downloadHandler.text));
            }
        }



        public static void GetData<T>(string sheetName, System.Action<T> callback = null) where T : GoogleSheetResPacket
        {
            StaticCoroutine.DoCoroutine(GetDataCoroutine(sheetName, callback));
        }


        private static IEnumerator GetDataCoroutine<T>(string sheetName, System.Action<T> callback) where T : GoogleSheetResPacket
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
                callback?.Invoke(JsonUtility.FromJson<T>(www.downloadHandler.text));
            }
        }
    }
}
