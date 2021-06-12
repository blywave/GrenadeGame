using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpownPoint : MonoBehaviour
{
    public float respownTime;
    private GameObject gameManagerObj;
    private ObjectsPoolController pool;
    private GameObject enemy;
    public string enemyFolderPath;
    public string enemyName;
    void Start()
    {
        gameManagerObj = GameObject.FindGameObjectWithTag("GameController");
        pool = gameManagerObj.GetComponent<ObjectsPoolController>();

        SpownEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy && !enemy.activeSelf)
        {
            enemy = null;
            Invoke("SpownItem", respownTime);
        }
    }

    private void SpownEnemy()
    {
        if (pool.TryFindObjectByName(enemyName))
        {
            //Debug.Log("В ПУЛЕ НАЙДЕН ОБЪЕКТ");
            enemy = pool.GetObjectFromPool(enemyName);
            enemy.transform.position = transform.position;
            enemy.transform.rotation = transform.rotation;
            enemy.SetActive(true);
        }
        else
        {
            //Debug.Log("В ПУЛЕ НЕ НАЙДЕН ОБЪЕКТ, СОЗДАН НОВЫЙ");
            enemy = Instantiate((GameObject)Resources.Load(enemyFolderPath + enemyName), transform.position, Quaternion.identity, null);
        }
    }


    public void SetRespownTime(float time)
    {
        respownTime = time;
    }
}
