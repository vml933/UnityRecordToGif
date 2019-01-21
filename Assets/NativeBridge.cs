using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeBridge : MonoBehaviour {

    public void NativeReceiveMsg(string msg) {
        Debug.Log("hihi msg from native:" + msg);
    }
}
