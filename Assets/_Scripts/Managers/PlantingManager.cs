using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlantingManager : MonoBehaviour
{
    [SerializeField] GameObject plantableSpotPrefab; // Assign in inspector
    [SerializeField] Camera mainCamera; // Assign main camera in inspector
    [SerializeField] Transform playerTransform; // Assign player transform in inspector
    [SerializeField] float plantingDistance = 2f; // Max distance the player can plant from
    [SerializeField] GameObject _toolCursor;
    [SerializeField] Sprite[] _toolSprites;
    [SerializeField] TMP_Text _toolSelectText;

    PlayerControls _playerControls; // Assuming you have the 'Controls' class from the new Input System

    SpriteRenderer _tempSprite;
    Color _tempColor;

    int _toolType;

    void Awake() {
        _playerControls = new PlayerControls();
        _playerControls.Player.SelectTool1.performed += context => SelectTool(1);
        _playerControls.Player.SelectTool2.performed += context => SelectTool(2);
        _playerControls.Player.SelectTool3.performed += context => SelectTool(3);
        _tempSprite = _toolCursor.GetComponent<SpriteRenderer>();
        _tempColor = _tempSprite.color;
    }

    void OnEnable() {
        _playerControls.Enable();
    }

    void OnDisable() {
        _playerControls.Disable();
    }
    bool _buzzing;
    bool _planting;

    void Start(){
        SelectTool(1);
    }

    void Update() {
        UseTool();
    }

    void UseTool(){
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        // Convert the screen position to world position
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
        _toolCursor.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0); // You might need to adjust the Z position depending on your camera setup

        if(Vector2.Distance(worldPosition, playerTransform.position) <= plantingDistance){
            if(!_buzzing) _tempSprite.color = _tempColor;
            if(Mouse.current.leftButton.wasPressedThisFrame){
                Collider2D hitCollider = Physics2D.OverlapCircle(worldPosition, 0.1f); // Adjust the radius as needed for your game
                
                switch(_toolType){
                    case 1: // Plow Tool
                        if(!_planting){
                            PlantAtMousePosition();
                            StartCoroutine(Planting());
                        }
                        break;
                    case 2: // Seed Tool
                        if(hitCollider != null){
                            if (hitCollider.tag == "PlantableSpot") {
                                hitCollider.GetComponent<PlantableSpot>().SpotPressedOn();
                                _toolCursor.SetActive(false);
                            }
                        }
                        break;
                    case 3: // Harvest Tool
                        if(hitCollider != null){
                            if (hitCollider.tag == "PlantableSpot") {
                                PlayerInventoryManager.Instance.AddMoney(hitCollider.GetComponent<PlantableSpot>().Harvest());
                            }
                            else StartCoroutine(BuzzRed());
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else{
            _tempSprite.color = Color.red - new Color(0, 0, 0, 0.5f);
        }
        if(!Mouse.current.leftButton.isPressed) _toolCursor.SetActive(true);
    }


    IEnumerator Planting(){ // For preventing mouse bounceback clicks
        _planting = true;
        yield return new WaitForSeconds(.2f);
        _planting = false;
    }

    void SelectTool(int toolType){
        _toolSelectText.color = Color.white;
        _toolCursor.GetComponent<SpriteRenderer>().sprite = _toolSprites[toolType-1];
        _toolCursor.SetActive(true);
        _toolType = toolType;
        if(toolType == 1) _toolSelectText.text = "Plow selected";
        else if(toolType == 2) _toolSelectText.text = "Seed selected";
        else if(toolType == 3) _toolSelectText.text = "Harvester selected";
        StopAllCoroutines();
        StartCoroutine(FadeSelectText());
    }

    IEnumerator FadeSelectText(){
        yield return new WaitForSeconds(2);
        while(_toolSelectText.alpha > 0){
            _toolSelectText.color -= new Color(0, 0, 0, .01f);
            yield return new WaitForSeconds(Time.deltaTime * 1f);
        }
    }

    void PlantAtMousePosition() {
        //Debug.Log("Trying to plant");
        // Convert mouse position to world position
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -mainCamera.transform.position.z));
        // Ensure that z position is set to 0 to work with 2D physics
        mouseWorldPosition.z = 0;

        // Calculate the grid position for snapping to the nearest integer coordinates
        Vector2 gridPosition = new Vector2(Mathf.RoundToInt(mouseWorldPosition.x), Mathf.RoundToInt(mouseWorldPosition.y));

        // Check the distance from the player to the grid position
        if (Vector2.Distance(gridPosition, playerTransform.position) <= plantingDistance) {
            // Cast a ray to check for existing plantable spots at the grid position
            Collider2D hitCollider = Physics2D.OverlapCircle(gridPosition, 0.1f); // Adjust the radius as needed for your game

            if (!hitCollider) {
                // We hit an existing plantable spot or another object, so planting is not allowed
                CreatePlantableSpot(gridPosition);
            }
            else {
                StartCoroutine(BuzzRed());
            }

        } else {
            // The desired plant position is too far from the player
            StartCoroutine(BuzzRed());
        }
    }


    IEnumerator BuzzRed() {
        _tempSprite.color = Color.red;
        _buzzing = true;
        yield return new WaitForSeconds(.1f);
        _tempSprite.color = _tempColor;
        _buzzing = false;
    }

    void CreatePlantableSpot(/*PlantData selectedPlant,*/ Vector2 position) {
        // Instantiate and setup the new plantable spot
        GameObject spotInstance = Instantiate(plantableSpotPrefab, position, Quaternion.identity);
        PlantableSpot plantableSpotComponent = spotInstance.GetComponent<PlantableSpot>();
        if (plantableSpotComponent != null) {
            // plantableSpotComponent.PlantCrop(selectedPlant);
        }
    }
}
