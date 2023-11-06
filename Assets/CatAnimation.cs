using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimation : MonoBehaviour
{
    int _animState = 10;
    [SerializeField] Animator _catAnimatorController;

    public void OnAnimationComplete(){
        StopAllCoroutines();
        StartCoroutine(PlayRandomAnimation());
    }
    IEnumerator PlayRandomAnimation(){
        _catAnimatorController.SetInteger("State", 0);
        yield return new WaitForSeconds(2f);
        int randomIndex = Random.Range(0, _animState);
        _catAnimatorController.SetInteger("State", randomIndex);
    }
}
