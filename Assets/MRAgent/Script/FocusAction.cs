using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class FocusAction : MonoBehaviour, IFocusable {

    public static bool isGazed = true;

    public void OnFocusEnter()
    {
        isGazed = true;
}

    public void OnFocusExit()
    {
        isGazed = false;
    }


}
