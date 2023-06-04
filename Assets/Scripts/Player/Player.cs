using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]PlayerInput input;

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] float accelerationTime = 3f;   //â¡ë¨éûä‘

    [SerializeField] float decelerationTime = 3f;   //å∏ë¨éûä‘

    [SerializeField] float moveRotationAngle = 50f;

    [SerializeField] float paddingx = 0.2f;

    [SerializeField] float paddingy = 0.2f;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;

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
        // Vector2 moveAmount = moveInput * moveSpeed;
        // rigidbody.velocity = moveAmount;
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y,Vector3.right);
        moveCoroutine =  StartCoroutine(MoveCoroutine(accelerationTime,moveInput.normalized * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StartCoroutine(MovePositionLimitCoroutine());
    }

    void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MoveCoroutine(float time ,Vector2 moveVelocity,Quaternion moveRotation)
    {
        float t = 0f;

        while(t < time)
        {
            t += Time.fixedDeltaTime / decelerationTime;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / time);

            yield return null;
        }
    }

 

    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position,paddingx,paddingy);

            yield return null;
        }
       
    }
}
