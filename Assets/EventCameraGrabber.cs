using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCameraGrabber : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
