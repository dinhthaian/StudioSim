using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float knockBackForce = 3f;
    public float knockBackDuration = 0.3f;

    public Rigidbody2D rb;
    public Animator animator;
    public BoxCollider2D moveBounds;

    Vector2 movement;
    Vector2 minPos;
    Vector2 maxPos;

    bool canMove = true;

    void Start()
    {
        if (moveBounds != null)
        {
            Bounds bounds = moveBounds.bounds;
            minPos = bounds.min;
            maxPos = bounds.max;
        }
    }

    void Update()
    {
        if (!canMove) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("isWalking", movement != Vector2.zero);

        if (movement != Vector2.zero)
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
        }
        // Flip trái phải (nếu đi ngang)
        if (movement.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        Vector2 nextPos = rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;

        bool outOfBounds = nextPos.x < minPos.x || nextPos.x > maxPos.x || nextPos.y < minPos.y || nextPos.y > maxPos.y;

        if (outOfBounds)
        {
            StartCoroutine(KnockBack(-movement.normalized));
        }
        else
        {
            rb.MovePosition(nextPos);
        }
    }

    System.Collections.IEnumerator KnockBack(Vector2 dir)
    {
        canMove = false;
        animator.SetBool("bump",true);  // Animation té

        rb.AddForce(dir * knockBackForce, ForceMode2D.Impulse);

        // Stun 2s
        yield return new WaitForSeconds(knockBackDuration); // Thời gian knockback bay lùi
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("bump", false);

        canMove = true;

    }

}
