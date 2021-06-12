using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpownPoint : MonoBehaviour
{
    private float respownTime;
    private GameObject gameManagerObj;
    private ItemsManager itemsManager;
    private ObjectsPoolController pool;
    private GameObject item;
    private string itemsFolderPath;
    private List<string> itemsNames = new List<string>();
    void Start()
    {
        gameManagerObj = GameObject.FindGameObjectWithTag("GameController");
        itemsManager = gameManagerObj.GetComponent<ItemsManager>();
        pool = gameManagerObj.GetComponent<ObjectsPoolController>();
        respownTime = itemsManager.GetRespownTime();
        itemsFolderPath = itemsManager.GetItemsFolderPath();
        itemsNames = itemsManager.GetItemsNames();
        itemsManager.AddSpownPoint(transform);

        SpownItem();

        itemsManager.AddItem(item);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (item && !item.activeSelf)
        {
            item = null;
            Invoke("SpownItem", respownTime);
        }
    }
    
    private void SpownItem()
    {
        string name = itemsNames[Random.Range(0, itemsNames.Count)];
        if (pool.TryFindObjectByName(name)) 
        {
            //Debug.Log("В ПУЛЕ НАЙДЕН ОБЪЕКТ");
            item = pool.GetObjectFromPool(name);
            
            item.transform.position = transform.position;
            item.transform.rotation = transform.rotation;
            
        }
        else
        {
            //Debug.Log("В ПУЛЕ НЕ НАЙДЕН ОБЪЕКТ, СОЗДАН НОВЫЙ");
            item = Instantiate((GameObject)Resources.Load(itemsFolderPath+name), transform.position, Quaternion.identity, null);
        }
        Rigidbody rigidbody = item.GetComponent<Rigidbody>();
        item.SetActive(true);
        item.GetComponent<BoxCollider>().enabled = true;
        item.GetComponent<Collider>().enabled = false;
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        itemsManager.AddItem(item);
    }


    public void SetRespownTime(float time)
    {
        respownTime = time;
    }
}
