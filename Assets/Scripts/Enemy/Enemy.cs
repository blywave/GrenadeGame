using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public string name = "Enemy";
    public int maxHealth = 10;
    public Transform healthBarLine;

    public string folderPath;
    public string textPrefabName;

    private int health;
    private GameObject gameController;
    private GameObject damageTextGO;
    private ObjectsPoolController pool;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gameObject.name = name;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        pool = gameController.GetComponent<ObjectsPoolController>();
    }


    public void DamageReceiver(int damage)
    {
        IncomeDamageText(damage);
        if (health - damage <= 0)
        {
            health = maxHealth;
            UpdateHealthBar();
            GoToPool();
        }
        else
        {
            health -= damage;
            UpdateHealthBar(damage);
        }
    }
    private void IncomeDamageText(int damage)
    {
        if (pool.TryFindObjectByName(textPrefabName))
        {
            damageTextGO = pool.GetObjectFromPool(textPrefabName);
            damageTextGO.transform.position = healthBarLine.parent.position;
            damageTextGO.transform.rotation = Quaternion.identity;
            damageTextGO.SetActive(true);
        }
        else
        {
            damageTextGO = Instantiate((GameObject)Resources.Load(folderPath + textPrefabName), healthBarLine.parent.position, Quaternion.identity, null);
        }
        damageTextGO.name = textPrefabName;
        damageTextGO.GetComponent<TextMeshPro>().text = "-" + damage;
        
    }
    private void UpdateHealthBar()
    {
        healthBarLine.localPosition = Vector3.zero;
    }
    private void UpdateHealthBar(int damage)
    {
        
        Vector3 newPosition = healthBarLine.localPosition;
        float xPosDelta = (healthBarLine.localScale.x * 2 / 100) * (damage /((float)maxHealth / 100));
        newPosition.x += xPosDelta;
        healthBarLine.localPosition = newPosition;
    }
    private void GoToPool()
    {
        //Debug.Log("ГРАНАТА ВЫЗВАЛА ОТПРАВКУ В ПУЛ");
        pool.SetObjectToPool(gameObject, transform.name);
    }
}
