using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.iOS;
using UnityEngine.Apple.ReplayKit;
using System;

public class Main : MonoBehaviour {

    public Button btnRecordPage;
    public Button btnReplayKitRecord;
    public Button btnReplayKitPreview;
    public Button btnReplayKitDiscard;

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void toRecordPage();
#endif

    private void Awake()
    {    
        btnRecordPage.onClick.AddListener(() => {
            RecordNativeHandler();
        });

        btnReplayKitRecord.onClick.AddListener(() => {
            ReplayKitHandler();
        });

        btnReplayKitPreview.onClick.AddListener(() => {
            if (!ReplayKit.APIAvailable)
                return;

            ReplayKit.Preview();
        });

        btnReplayKitDiscard.onClick.AddListener(() =>
        {
            if (!ReplayKit.APIAvailable)
                return;

            ReplayKit.Discard();
        });
    }

    private void RecordNativeHandler() {
#if UNITY_IOS && !UNITY_EDITOR
        toRecordPage();
#else
        print("Dummy RecordHandler method");
#endif
    }

    private void ReplayKitHandler() {

        if (!ReplayKit.APIAvailable)
            return;

        var recording = ReplayKit.isRecording;
        try
        {
            if (!recording)
            {
                ReplayKit.StartRecording();
            }
            else {
                ReplayKit.StopRecording();
            }
            recording = !recording;

        }
        catch(Exception e) {
            Debug.Log(e.ToString());
        }

    }

}
