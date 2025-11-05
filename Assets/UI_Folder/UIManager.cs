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

    [Header("Money Text Update")]
    [SerializeField] private TextMeshProUGUI toyCost;
    [SerializeField] private TextMeshProUGUI foodCost;
    [SerializeField] private TextMeshProUGUI soapCost;
    [SerializeField] private TextMeshProUGUI playerMoney;



    [Header("Item Examples - Temp")]
    [SerializeField] private GameObject soap;
    [SerializeField] private GameObject toy;
    [SerializeField] private GameObject food;

    [Header("UI Panels")]
    [SerializeField] private GameObject dimPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("UI Meter Text")]
    [SerializeField] private TextMeshProUGUI affectionMeterText;
    [SerializeField] private TextMeshProUGUI hungerMeterText;
    [SerializeField] private TextMeshProUGUI energyMeterText;

    [Header("Item Options")]
    [SerializeField] private ItemBase foodItemBase;
    [SerializeField] private ItemBase toyItemBase;
    [SerializeField] private ItemBase soapItemBase;

    //reference to current item instantiated in scene
    private GameObject currentItem = null;

    //variables
    private const float GameTickTimer = 1.0f;
    private float currentTimer = 0;

    public void Start()
    {
        updateShopCosts();
    }

    /// <summary>
    /// Grab the item's cost and update the shop prices
    /// </summary>
    public void updateShopCosts()
    {
        toyCost.SetText("$ " + toyItemBase.Cost.ToString());
        foodCost.SetText("$ " + foodItemBase.Cost.ToString());
        soapCost.SetText("$ " + soapItemBase.Cost.ToString());
    }

    public void updateMoney()
    {
        playerMoney.SetText(gameController.inventory.Currency.ToString());
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
        settingsPanel.SetActive(true);
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
        if (gameController.CurrentRoom != RoomEnum.Bedroom) gameController.ChangeRoom(RoomEnum.Bedroom);
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
        bool got_item = gameController.inventory.AddItem(0, 1);
        if(got_item) toyButton.enabled = true;
    }

    public void OnAddFoodButton()
    {
        bool got_item = gameController.inventory.AddItem(1, 1);
        if(got_item) foodButton.enabled = true;
    }

    public void OnAddSoapButton()
    {
        //do if money
        bool got_item = gameController.inventory.AddItem(2, 1);
        if(got_item) soapButton.enabled = true;
    }


    public void OnExitShopButton()
    {
        dimPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void OnExitSettingsButton()
    {
        dimPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }


    void FixedUpdate()
    {
        updateMoney();
        if (currentTimer < GameTickTimer)
        {
            currentTimer += Time.deltaTime;
        }
        else
        {
            currentTimer -= GameTickTimer;
            affectionMeterText.SetText(petController.CurrentAffection.ToString());
            hungerMeterText.SetText(petController.CurrentHunger.ToString());
            energyMeterText.SetText(petController.CurrentEnergy.ToString());
        }
    }

    void Update()
    {
        //
    }
}
