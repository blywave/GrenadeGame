using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsAutoGatherer : MonoBehaviour
{
    public float timeToGathering;
    public bool autoStart;
    private ObjectsPoolController pool;
    private Vector3 startSize;
    public ObjectsAutoGatherer (float time, bool autoStart)
    {
        timeToGathering = time;
        this.autoStart = autoStart;
    }
    private void Start()
    {
        startSize = transform.localScale;
        pool = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectsPoolController>();
        
    }

    public void GoToPoolInTime(float time)
    {
        Invoke("GoToPool", time);
    }

    private void GoToPool()
    {
        transform.localScale = startSize;
        pool.SetObjectToPool(gameObject, transform.name);
    }
    private void OnEnable()
    {
        if (autoStart) Invoke("GoToPool", timeToGathering);
    }
}
