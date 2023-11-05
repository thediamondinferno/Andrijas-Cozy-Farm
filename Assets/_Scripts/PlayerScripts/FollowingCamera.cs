using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    void Update()
    {
        Camera.main.transform.position = transform.position + new Vector3(0, 0, -10);
    }
}
