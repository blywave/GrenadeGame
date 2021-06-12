using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public enum MoveType
    {
        Towards = 0,
        Lerp = 1
    }
    public MoveType moveType;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
            switch (moveType)
            {
                case 0:
                    TowardsMove();
                    break;
                default:
                    LerpMove();
                    break;
            }
    }

    private void TowardsMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
    private void LerpMove()
    {
        transform.position = Vector3.Lerp(transform.position, startPosition + direction, speed * Time.deltaTime);
    }
    private void OnEnable()
    {
        startPosition = transform.position;
    }
}
