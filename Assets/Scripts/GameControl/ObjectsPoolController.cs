using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPoolController : MonoBehaviour
{
    private Dictionary<string, List<GameObject>> objects = new Dictionary<string, List<GameObject>>(); //словарь объектов - string для идентефикации, List<GameObject> - объект в инвентаре.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //запись входящих объектов в пул
    public void SetObjectToPool(GameObject incomeObject, string objectType)
    {
        //Debug.Log("ПУЛ ПРИНЯЛ ОБЪЕКТ");
        if (objects.ContainsKey(objectType))
        {
            //Debug.Log("ОБЪЕКТ ЭТОГО ТИПА УЖЕ БЫЛ В ПУЛЕ, И БЫЛ ДОБАВОЕН");
            objects[objectType].Add(incomeObject);
        }
        else
        {
            //Debug.Log("ОБЪЕКТ ЭТОГО ТИПА ВПЕРВЫЕ ПОПАЛ В ПУЛ, И БЫЛ ДОБАВОЕН");
            objects.Add(objectType, new List<GameObject>());
            objects[objectType].Add(incomeObject);
        }
        //Debug.Log("ОБЪЕКТ СКРЫТ");
        incomeObject.SetActive(false);
    }
    public bool TryFindObjectByName(string name)
    {
        //Debug.Log("ПОИСК ОБЪЕКТА В ПУЛЕ - РЕЗУЛЬТАТ - " + objects.ContainsKey(name));
        return objects.ContainsKey(name);
    }
    public GameObject GetObjectFromPool(string name)
    {
        //Debug.Log("ПУЛ ПЕРЕДАЛ ОБЪЕКТ");
        GameObject objectFromPool = objects[name][objects[name].Count - 1];
        objects[name].Remove(objects[name][objects[name].Count - 1]);
        if (objects[name].Count < 1) objects.Remove(name);
        return objectFromPool;
    }
}
