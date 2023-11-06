using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FloatMoneyCount : MonoBehaviour
{
    [SerializeField] TMP_Text _moneyText;
    public void RegisterHarvest(int amount){
        _moneyText.text = amount.ToString() + "$";
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + 1, 1);
        StartCoroutine(Reset());
    }

    IEnumerator Reset(){
        yield return new WaitForSeconds(2);
        _moneyText.text = "";
        LeanTween.moveLocalY(gameObject, transform.localPosition.y - 1, 1);
    }
}
