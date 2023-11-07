using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlantableSpot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cropRenderer;
    [SerializeField] RectTransform _UIOptions;
    [SerializeField] List<Button> _options;
    private PlantData currentPlantData;
    float _harvestTime;
    [SerializeField] float _optionsScaleSpeed;
    bool _planted;
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] AudioSource _audioSource;

    [SerializeField] FloatMoneyCount _floatMoneyCount;


    public void PlantCrop(PlantData plantData)
    {
        if(!_planted){
            currentPlantData = plantData;
            _audioSource.PlayOneShot(_audioClips[0]);
            StartCoroutine(GrowPlant());
            _UIOptions.gameObject.SetActive(false);
        }
    }


    private IEnumerator GrowPlant()
    {
        cropRenderer.sprite = currentPlantData.seedSprite;
        yield return new WaitForSeconds(currentPlantData.growthTime);
        _audioSource.PlayOneShot(_audioClips[1]);
        cropRenderer.sprite = currentPlantData.grownSprite;
        _harvestTime = Time.time + currentPlantData.deadTime;
        yield return new WaitForSeconds(currentPlantData.deadTime);
        cropRenderer.sprite = currentPlantData.deadSprite;
        yield return new WaitForSeconds(currentPlantData.deadTime);
    }

    public int Harvest() {
        if (currentPlantData != null && cropRenderer.sprite == currentPlantData.grownSprite) {
            // Calculate money based on time since harvest peak
            float timeLeftToWilt = _harvestTime - Time.time;
            float fractionOfValueLost = Mathf.Clamp01((currentPlantData.deadTime - timeLeftToWilt) / currentPlantData.deadTime);
            int moneyEarned = Mathf.RoundToInt(currentPlantData.baseMoney * (1f - fractionOfValueLost));
            _audioSource.PlayOneShot(_audioClips[2]);
            _floatMoneyCount.RegisterHarvest(moneyEarned);
            ResetPlantableSpot();
            return moneyEarned;
        } else if (currentPlantData != null && cropRenderer.sprite == currentPlantData.deadSprite) {
            ResetPlantableSpot();
            return 0; // No money earned for dead plants
        }
        return 0; // Default return if neither condition is met
    }

    void ResetPlantableSpot(){
        cropRenderer.sprite = null;
        currentPlantData = null; // Reset plant data after harvesting
        _planted = false;
        _harvestTime = 0;
        StopAllCoroutines();
    }

    public void SpotPressedOn(){
        if(!_planted) StartCoroutine(ShowOptions());
    }

    private IEnumerator ShowOptions() {
        _UIOptions.gameObject.SetActive(true);
        _UIOptions.gameObject.SetActive(true);
        _UIOptions.localScale = Vector3.zero;
        // Scale up the UI options until the mouse button is released
        while (Mouse.current.leftButton.isPressed) {
            _UIOptions.localScale = Vector3.MoveTowards(_UIOptions.localScale, Vector3.one, _optionsScaleSpeed * Time.deltaTime);
            yield return null;
        }
        // After mouse release
        ClickClosestButton();
        _planted = true;
    }

    private void ClickClosestButton()
    {
        if(!_planted){
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Button closestButton = null;
            float closestDistance = float.MaxValue;

            foreach (Button button in _options)
            {
                Vector3 buttonPosition = Camera.main.WorldToScreenPoint(button.transform.position);
                
                float distance = (mouseScreenPosition - (Vector2)buttonPosition).sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestButton = button;
                    closestDistance = distance;
                }
            }

            closestButton?.onClick.Invoke(); // Check if closestButton is not null and then invoke click
        }
    }
}
