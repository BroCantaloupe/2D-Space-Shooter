using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindnessRoutine : MonoBehaviour
{
    [SerializeField]
    private Animator _blindAnim;
    void OnEnable()
    {
        _blindAnim.SetTrigger("BlindGet");
        StartCoroutine(BlindnessPowerDownRoutine());
    }

    IEnumerator BlindnessPowerDownRoutine()
    {
        yield return new WaitForSeconds(8.5f);
        _blindAnim.SetTrigger("BlindPowerDown");
    }
}
