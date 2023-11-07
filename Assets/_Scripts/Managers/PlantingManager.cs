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
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
        _toolCursor.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0); 

        if(Vector2.Distance(worldPosition, playerTransform.position) <= plantingDistance){
            if(!_buzzing) _tempSprite.color = _tempColor;
            if(Mouse.current.leftButton.wasPressedThisFrame){
                Collider2D hitCollider = Physics2D.OverlapCircle(worldPosition, 0.1f); 
                
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
        _planting = false;
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
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -mainCamera.transform.position.z));
        mouseWorldPosition.z = 0;

        Vector2 gridPosition = new Vector2(Mathf.RoundToInt(mouseWorldPosition.x), Mathf.RoundToInt(mouseWorldPosition.y));

        if (Vector2.Distance(gridPosition, playerTransform.position) <= plantingDistance) {
            Collider2D hitCollider = Physics2D.OverlapCircle(gridPosition, 0.1f); 

            if (!hitCollider) {
                CreatePlantableSpot(gridPosition);
            }
            else {
                StartCoroutine(BuzzRed());
            }

        } else {
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

    void CreatePlantableSpot(Vector2 position) {
        GameObject spotInstance = Instantiate(plantableSpotPrefab, position, Quaternion.identity);
        PlantableSpot plantableSpotComponent = spotInstance.GetComponent<PlantableSpot>();
    }
}
