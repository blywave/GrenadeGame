using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public float respownTime;
    public string itemsFolderPath;
    public List<string> itemsNames = new List<string>();
    public List<Transform> spownPoints = new List<Transform>();

    private List<Transform> items = new List<Transform>();
    private Transform player;
    private InventoryController inventoryController;
    private float playerPickUpDistance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventoryController = player.GetComponent<InventoryController>();
        playerPickUpDistance = player.GetComponent<PlayerController>().GetPickUpDistance();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(Vector3.Distance(items[i].position, player.position) < playerPickUpDistance)
            {
                Ray ray = new Ray(items[i].position, player.position- items[i].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Player")
                {
                    if(inventoryController.CheckInventorySpace())
                    {
                        inventoryController.AddToInventory(items[i].gameObject);
                        items.Remove(items[i]);
                    }
                    
                }
            }
        }
    }
    public void AddItem(GameObject item)
    {
        items.Add(item.transform);
    }
    public void AddItem(Transform item)
    {
        items.Add(item);
    }
    public float GetRespownTime()
    {
        return respownTime;
    }
    public string GetItemsFolderPath()
    {
        return itemsFolderPath;
    }
    public List<string> GetItemsNames()
    {
        return itemsNames;
    }
    public void AddSpownPoint(Transform spownPoint)
    {
        spownPoints.Add(spownPoint);
    }
}
