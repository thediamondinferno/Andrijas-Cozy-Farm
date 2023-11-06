using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance {get; private set;}

    [SerializeField] List<GameObject> _buyButtons;
    [SerializeField] List<GameObject> _equipButtons;

    [SerializeField] Sprite _buttonON;
    [SerializeField] Sprite _buttonOFF;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;
    void Awake(){
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    public void Buy(int button){ // was short on time to implement an extended button functionality that takes multiple parameters
        int amount = 0;
        if(button == 0) amount = 50;
        else if(button == 1) amount = 300; 
        else if(button == 2) amount = 1000;
        if(PlayerInventoryManager.Instance.SpendMoney(amount)){
            _buyButtons[button].SetActive(false);
            _audioSource.PlayOneShot(_audioClips[0]);
            _equipButtons[button].SetActive(true);
        }
        else StartCoroutine(BuzzRed(button));
    }

    IEnumerator BuzzRed(int button){
        _buyButtons[button].GetComponent<Image>().color = Color.red;
        _audioSource.PlayOneShot(_audioClips[1]);
        yield return new WaitForSeconds(.2f);
        _buyButtons[button].GetComponent<Image>().color = Color.white;
    }

    public void Equip(int button){
        foreach (var equipButton in _equipButtons)
        {
            equipButton.GetComponent<Image>().sprite = _buttonOFF;
        }
        PlayerMovement.Instance.ActivateOutfit(button+1);
        _audioSource.PlayOneShot(_audioClips[2]);
        _equipButtons[button].GetComponent<Image>().sprite = _buttonON;
    }
    bool _shopOpen;

    public void CloseShop(){
        if(_shopOpen) {
            LeanTween.moveLocalY(gameObject, gameObject.transform.localPosition.y + Screen.height, 1);
            _shopOpen = false;
        }
    }
    public void OpenShop(){
        if(!_shopOpen) {
            _shopOpen = true;
            LeanTween.moveLocalY(gameObject, gameObject.transform.localPosition.y - Screen.height, 1);
        }
    }
}
