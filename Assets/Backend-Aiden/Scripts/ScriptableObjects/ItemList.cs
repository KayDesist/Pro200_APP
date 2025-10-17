using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 1)]
public class ItemList : ScriptableObjectBase
{
    // List of all items in the game. Item IDs correspond to their index in this array.
    public GameObject[] items;
}
