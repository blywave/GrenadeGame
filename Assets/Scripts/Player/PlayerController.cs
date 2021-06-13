using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //базовые параметры
    public int health;
    public float reloadTime;
    public float speedMove;
    public float speedRotation;
    public float deltaGravityForce;
    public float grenadeStartVelocity;
    public float shootAngle;
    public float minShootDistance;
    public float maxShootDistance;
    public float gravityForce;
    public float pickUpDistance;

    private float distance;
    private bool canShoot;

    private Vector3 moveVector;
    private RaycastHit hit;

    //компоненты
    private CharacterController characterController;
    [SerializeField]
    private LineRenderer lineRenderer;
    private InventoryController inventory;
    [SerializeField]
    private Transform GgrenadeSpounPoint;
    private Camera camera;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (!TryGetComponent<InventoryController>(out inventory)) inventory = gameObject.AddComponent<InventoryController>();
        lineRenderer.positionCount = 0;
        camera = Camera.main;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Gravity();
        if (Input.GetMouseButton(0) && inventory.CheckItem()) ShowTrajectory();//если нажата кнопка начинаем прицеливание
        if (Input.GetMouseButtonUp(0) && inventory.CheckItem()) Shoot(); //если кнопка отпущена
    }

    //движение персонажа
    private void Move()
    {
        Vector3 mousePosition;
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * speedMove;
        moveVector.z = Input.GetAxis("Vertical") * speedMove;

        //слежение за мышкой
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            float rotation_y = Mathf.Atan2(transform.position.z - hit.point.z, hit.point.x - transform.position.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, rotation_y, 0f);
        }
        


        moveVector.y = gravityForce;
        characterController.Move(moveVector * Time.deltaTime); //двигаем игрока
    }
    //притяжение действующее на персонажа
    private void Gravity()
    {
        if (!characterController.isGrounded) gravityForce -= deltaGravityForce * Time.deltaTime;
        else gravityForce = -1f;
    }

    private void ShowTrajectory()
    {
        if (hit.collider)
        {
            Vector3 origin = GgrenadeSpounPoint.position;
            Vector3 endPoint = hit.point;
            distance = Vector3.Distance(GgrenadeSpounPoint.position, endPoint);
            if (distance > minShootDistance && distance < maxShootDistance)
            {
                lineRenderer.startColor = Color.yellow;
                lineRenderer.endColor = Color.red;
            }
            else
            {
                lineRenderer.startColor = Color.clear;
                lineRenderer.endColor = Color.black;
            }
            Vector3 origin2 = hit.point - GgrenadeSpounPoint.position;
            Vector3 originXZ = new Vector3(origin2.x, 0f, origin2.z);
            float x = originXZ.magnitude;
            float y = origin2.y;
            float RadiansAngle = shootAngle * Mathf.PI / 180;

            float pow2 = (Physics.gravity.y * x * x) / (2 * (y - Mathf.Tan(RadiansAngle) * x) * Mathf.Pow(Mathf.Cos(RadiansAngle), 2));
            grenadeStartVelocity = Mathf.Sqrt(Mathf.Abs(pow2));


            Vector3[] points = new Vector3[64];
            lineRenderer.positionCount = points.Length;
            for (int i = 0; i < points.Length; i++)
            {
                float time = i * 0.1f;
                points[i] = origin + (GgrenadeSpounPoint.forward * grenadeStartVelocity) * time + Physics.gravity * time * time / 2f;
                if (points[i].y < hit.point.y &&
                    Vector3.Distance(GgrenadeSpounPoint.position, hit.point) < Vector3.Distance(GgrenadeSpounPoint.position, points[i]))
                {
                    lineRenderer.positionCount = i;
                    break;
                }
            }

            lineRenderer.SetPositions(points);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }

    }
    private void Shoot()
    {
        lineRenderer.positionCount = 0;
        if (!canShoot) return;
        canShoot = false;
        if (distance > minShootDistance && distance < maxShootDistance)
        {
            GameObject newBullet;// = Instantiate(inventory.TryGetItem(), GgrenadeSpounPoint.position, GgrenadeSpounPoint.rotation);
            newBullet = inventory.GetItem();
            Rigidbody rigidbody = newBullet.GetComponent<Rigidbody>();
            newBullet.transform.position = GgrenadeSpounPoint.position;
            newBullet.transform.rotation = GgrenadeSpounPoint.rotation;
            newBullet.SetActive(true);
            newBullet.GetComponent<BoxCollider>().enabled = true;
            rigidbody.useGravity = true;
            rigidbody.velocity = GgrenadeSpounPoint.forward * grenadeStartVelocity;
        }
        Invoke("CanShoot", reloadTime);
    }

    public void CanShoot()
    {
        canShoot = true;
    }
    public RaycastHit GetTargetHitPoint()
    {
        return hit;
    }
    public float GetPickUpDistance()
    {
        return pickUpDistance;
    }
}
