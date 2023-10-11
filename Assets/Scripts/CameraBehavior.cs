using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        yield return new WaitForSeconds(1);


    }
}
