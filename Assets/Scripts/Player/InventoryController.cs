using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public string noAmmoTxt = "OUT OF GRENADES";
    public int maxItems;
    public Text text;
    private Dictionary<string, List<GameObject>> items = new Dictionary<string, List<GameObject>>(); //словарь вещей - string для идентефикации, List<GameObject> - объект в инвентаре.
    [SerializeField]
    private List<string> itemsTypes = new List<string>();
    private int currentType = -1;
    private string txtStore;
    // Start is called before the first frame update
    void Start()
    {
        currentType = -1;
        text.text = noAmmoTxt;
    }

    // Update is called once per frame
    void Update()
    {
        ChooseItemInput();
    }

    private void LookArownd()
    {

    }
    public bool CheckItem()
    {
        if(itemsTypes.Count > 0) return true;
            else return false;
    }
    public bool CheckInventorySpace()
    {
        if (maxItems <= 0) return false;
        else return true;
    }
    public GameObject GetItem()
    {
        //Debug.Log(items.Count);
        GameObject item = items[itemsTypes[currentType]][items[itemsTypes[currentType]].Count-1];
        items[itemsTypes[currentType]].Remove(item);
        maxItems += 1;
        if (items[itemsTypes[currentType]].Count == 0)
        {
            items.Remove(itemsTypes[currentType]);
            itemsTypes.Remove(itemsTypes[currentType]);
            currentType = itemsTypes.Count - 1;
        }
        if (itemsTypes.Count <= 0)
        {
            currentType = -1;
        }
        UpdateInventoryUI();
        return item;
    }
    
    public void AddToInventory(GameObject item)
    {
        //Debug.Log(items.Count);
        string itemName = item.name;
        if (!items.ContainsKey(itemName))
        {
            if (items.Count == 0) currentType = 0;
            items.Add(itemName, new List<GameObject>());
            items[itemName].Add(item);
            itemsTypes.Add(itemName);
            item.SetActive(false);
            maxItems -= 1;
        }
        else
        {
            if (!items[itemName].Contains(item))
            {
                if (items.Count == 0) currentType = 0;
                items[itemName].Add(item);
                item.SetActive(false);
                maxItems -= 1;
            }
        }
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        if(items.Count > 0)
        {
            if (currentType >= 0)
            {
                txtStore = itemsTypes[currentType] + ": " + items[itemsTypes[currentType]].Count;
            }
            else currentType = 0;
        }
        else
        {
            txtStore = noAmmoTxt;
            currentType = -1;
        }
        text.text = txtStore;
    }

    private void ChooseItemInput()
    {
        if(items.Count > 0)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentType -= 1;
                if (currentType < 0) currentType = items.Count - 1;
                UpdateInventoryUI();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentType += 1;
                if (currentType >= items.Count) currentType = 0;
                UpdateInventoryUI();
            }
        }
    }
}
