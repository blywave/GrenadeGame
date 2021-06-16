using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public string name = "ExplosiveGrenade";
    public bool isTactics;
    public int damade = 3;
    public int radius = 2;
    public int power = 2;
    public string effectsPath;
    public string effectName;
    private GameObject effect;
    private GameObject gameController;
    private Rigidbody rigidbody;
    private Collider collider;
    private ObjectsPoolController pool;
    private bool enabled = false;
    void Start()
    {
        gameObject.name = name;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        rigidbody = transform.GetComponent<Rigidbody>();
        collider = transform.GetComponent<Collider>();
        pool = gameController.GetComponent<ObjectsPoolController>();
    }

    // Update is called once per frame
    private void ExplosionEffects()
    {
        if (pool.TryFindObjectByName(effectName))
        {
            effect = pool.GetObjectFromPool(effectName);
            effect.transform.position = transform.position;
            effect.SetActive(true);
        }
        else
        {
            effect = Instantiate((GameObject)Resources.Load(effectsPath + effectName), transform.position, Quaternion.identity, null);
        }
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        float sizeE = effect.transform.localScale.x * radius;
        effect.transform.localScale = new Vector3(sizeE, sizeE, sizeE);
        particleSystem.Play();
    }
    private void ExplosionMechanics()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Destructible"));
        foreach (Collider coll in colliders)
        {
            Ray ray = new Ray(transform.position, coll.transform.position - transform.position);
            Debug.DrawRay(transform.position, coll.transform.position - transform.position, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && coll.gameObject == hit.transform.gameObject)
            {
                coll.gameObject.SendMessage("DamageReceiver", damade);
                Rigidbody rb;
                if(coll.TryGetComponent<Rigidbody>(out rb))
                {
                    rb.AddForce(-(transform.position - coll.transform.position) * (power * (1/Mathf.Pow(Vector3.Distance(transform.position, coll.transform.position), 2))), ForceMode.Impulse);
                }
            }
                
        }
    }
    private void GoToPool()
    {
        //Debug.Log("ГРАНАТА ВЫЗВАЛА ОТПРАВКУ В ПУЛ");
        pool.SetObjectToPool(gameObject, transform.name);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(enabled)
        {
            rigidbody.useGravity = false;
            enabled = false;
            ExplosionEffects();
            if(!isTactics)ExplosionMechanics();
            GoToPool();
        }
        
    }
    private void OnEnable()
    {
        enabled = true;
    }

}
