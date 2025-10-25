using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject itemPrototype;

    [SerializeField] private Button toyButton;

    [SerializeField] private Button foodButton;
    [SerializeField] private Button soapButton;


    public void Start()
    {
        
    }


    /// <summary>
    /// Open up a pop up screen to buy items. Currently just adding 1 of 3 items for the time being.
    /// </summary>
    public void OnShopButtonPress()
    {
        gameController.inventory.AddItem(0, 1);
        gameController.inventory.AddItem(1, 1);
        gameController.inventory.AddItem(2, 1);

        //if item count is greater than 0, light up the button.
    }

    /// <summary>
    /// Open up a pop up screen to change settings.
    /// </summary>
    public void OnSettingsButtonPress()
    {
        //
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
        if (gameController.inventory.HasItem(0))
        {
            //instance the item.
            Instantiate(itemPrototype);

            //then if the item is used on the pet, remove a count from the item.
            gameController.inventory.RemoveItem(0, 1);

            //if item count is 0 or less, overlay button with partial alpha of black screen to dim.
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnFoodButtonPress()
    {
        if (gameController.inventory.HasItem(1))
        {
            //instance the item being dragged.
            //then if the item is used on the pet, remove a count from the item.
            gameController.inventory.RemoveItem(1, 1);

            //if item count is 0 or less, overlay button with partial alpha of black screen to dim.
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void OnSoapButtonPress()
    {
        if(gameController.inventory.HasItem(2))
        {
            //instance the item being dragged.
            //then if the item is used on the pet, remove a count from the item.
            gameController.inventory.RemoveItem(2, 1);

            //if item count is 0 or less, overlay button with partial alpha of black screen to dim.
        }
    }

}
