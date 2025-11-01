using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEditor.Rendering.Universal;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GameController gameController;
    [SerializeField] private PetController petController;

    [Header("Pet Stuff")]
    [SerializeField] private GameObject itemPrototype;
    [SerializeField] private GameObject currentPet;

    [Header("Main UI Buttons")]
    [SerializeField] private Button toyButton;
    [SerializeField] private Button foodButton;
    [SerializeField] private Button soapButton;

    [Header("Item Examples")]
    [SerializeField] private GameObject soap;
    [SerializeField] private GameObject toy;
    [SerializeField] private GameObject food;

    [Header("UI Panels")]
    [SerializeField] private GameObject dimPanel;
    [SerializeField] private GameObject shopPanel;

    [Header("UI Meter Text")]
    [SerializeField] private TextMeshProUGUI affectionMeterText;
    [SerializeField] private TextMeshProUGUI hungerMeterText;
    [SerializeField] private TextMeshProUGUI energyMeterText;

    //reference to current item instantiated in scene
    private GameObject currentItem = null;

    //variables
    private float GameTickTimer = 1.0f;
    private float currentTimer = 0;

    public void Start()
    {
        //
    }


    /// <summary>
    /// Open up a pop up screen to buy items. Currently just adding 1 of 3 items for the time being.
    /// </summary>
    public void OnShopButtonPress()
    {
        dimPanel.SetActive(true);
        shopPanel.SetActive(true);
    }

    /// <summary>
    /// Open up a pop up screen to change settings.
    /// </summary>
    public void OnSettingsButtonPress()
    {
        dimPanel.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnTalkButtonPress()
    {
        //
    }

    /// <summary>
    /// If not in Living Room, move to Living Room
    /// </summary>
    public void OnLivingRoomButtonPress()
    {
        if (gameController.CurrentRoom != RoomEnum.MainRoom) gameController.ChangeRoom(RoomEnum.MainRoom);
    }

    /// <summary>
    /// If not in Bathroom, move to Bathroom
    /// </summary>
    public void OnBathroomButtonPress()
    {
        if (gameController.CurrentRoom != RoomEnum.Bathroom) gameController.ChangeRoom(RoomEnum.Bathroom);
    }

    /// <summary>
    /// If not in Yard, move to Yard
    /// </summary>
    public void OnYardButtonPress()
    {
        if (gameController.CurrentRoom != RoomEnum.Yard) gameController.ChangeRoom(RoomEnum.Yard);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnToyButtonPress()
    {
        if (gameController.inventory.HasItem(0) && !currentItem)
        {
            //instance the item.
            GameObject temp = Instantiate(itemPrototype, new Vector3(500, -113, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);



            //then if the item is used on the pet, remove a count from the item.
            gameController.inventory.RemoveItem(0, 1);

            //if item count is 0 or less, overlay button with partial alpha of black screen to dim.
            if (!gameController.inventory.HasItem(0)) toyButton.enabled = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnFoodButtonPress()
    {
        if (gameController.inventory.HasItem(1) && !currentItem)
        {
            //instance the item being dragged.
            //instance the item.
            GameObject temp = Instantiate(itemPrototype, new Vector3(500, -113, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);

            //then if the item is used on the pet, remove a count from the item.
            gameController.inventory.RemoveItem(1, 1);

            //if item count is 0 or less, overlay button with partial alpha of black screen to dim.
            if (!gameController.inventory.HasItem(1)) foodButton.enabled = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnSoapButtonPress()
    {
        if (gameController.inventory.HasItem(2) && !currentItem)
        {
            //instance the item being dragged.
            //instance the item.
            GameObject temp = Instantiate(itemPrototype, new Vector3(500, -113, 0), Quaternion.identity);
            temp.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);

            //then if the item is used on the pet, remove a count from the item.
            gameController.inventory.RemoveItem(2, 1);

            //if item count is 0 or less, overlay button with partial alpha of black screen to dim.
            if (!gameController.inventory.HasItem(2)) soapButton.enabled = false;
        }
    }



    public void OnAddToyButton()
    {
        //do if money
        gameController.inventory.AddItem(0, 1);
        toyButton.enabled = true;
    }

    public void OnAddFoodButton()
    {
        //do if money
        gameController.inventory.AddItem(1, 1);
        foodButton.enabled = true;
    }

    public void OnAddSoapButton()
    {
        //do if money
        gameController.inventory.AddItem(2, 1);
        soapButton.enabled = true;
    }


    public void OnExitShopButton()
    {
        dimPanel.SetActive(false);
        shopPanel.SetActive(false);
    }


    void Update()
    {
        if (currentTimer < 1.0f)
        {
            currentTimer += Time.deltaTime;
        }
        else
        {
            currentTimer -= 1.0f;
            affectionMeterText.SetText(petController.CurrentAffection.ToString());
            hungerMeterText.SetText(petController.CurrentHunger.ToString());
            energyMeterText.SetText(petController.CurrentEnergy.ToString());
        }
    }
}
