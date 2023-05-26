using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]PlayerInput input;

    [SerializeField]float moveSpeed = 10f;

    new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.gravityScale = 0f;//èdóÕÇ0

        input.EnableGameplayInput();
    }

    void Move(Vector2 moveInput)
    {
        Vector2 moveAmount = moveInput * moveSpeed;

        rigidbody.velocity = moveAmount;
    }

    void StopMove()
    {
        rigidbody.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
