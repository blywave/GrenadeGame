using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    public bool findTargetByTag;
    public float speed;
    public string targetTag;
    public Vector3 cameraOffset;

    public Transform target;
    private LookAtConstraint lookAt;

    // Start is called before the first frame update
    void Start()
    {
        if(!gameObject.GetComponent<LookAtConstraint>())
        {
            lookAt = gameObject.AddComponent<LookAtConstraint>();
        }
        if (findTargetByTag)
        {
            if (targetTag.Length == 0) targetTag = "Player";
            target = GameObject.FindGameObjectWithTag(targetTag).transform;
        }
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + cameraOffset, speed * Time.deltaTime);
    }
}
