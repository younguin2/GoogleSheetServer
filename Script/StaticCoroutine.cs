using UnityEngine;
using System;
using System.Collections;

namespace GoogleSheetServer
{
    public class StaticCoroutine : MonoBehaviour
    {

        private static StaticCoroutine mInstance = null;

        private static StaticCoroutine instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = GameObject.FindObjectOfType(typeof(StaticCoroutine)) as StaticCoroutine;

                    if (mInstance == null)
                    {
                        mInstance = new GameObject("StaticCoroutine").AddComponent<StaticCoroutine>();
                        mInstance.gameObject.hideFlags = HideFlags.HideAndDontSave;
                    }
                }
                return mInstance;
            }
        }

        void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as StaticCoroutine;
                DontDestroyOnLoad(gameObject);
            }
        }

        IEnumerator Perform(IEnumerator coroutine)
        {
            yield return StartCoroutine(coroutine);
        }

        public static void DoCoroutine(IEnumerator coroutine)
        {
            //여기서 인스턴스에 있는 코루틴이 실행될 것이다.
            instance.StartCoroutine(instance.Perform(coroutine));
        }

        void OnApplicationQuit()
        {
            mInstance = null;
        }
    }
}

