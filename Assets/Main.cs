using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Main : MonoBehaviour {

    public Button btnRecordPage;

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void toRecordPage();
#endif

    private void Awake()
    {    
        btnRecordPage.onClick.AddListener(() => {
            RecordHandler();
        });
    }

    private void RecordHandler() {
#if UNITY_IOS && !UNITY_EDITOR
        toRecordPage();
#else
        print("Dummy RecordHandler method");
#endif
    }

}
