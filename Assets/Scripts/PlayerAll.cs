using UnityEngine;
using Unity.Mathematics;

public class PlayerAll : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float HP = 10;
    public float MP;
    public float Speed;
    public float EP;
    public float AtackPower = 3f;
    public float Def;
    public float CritChance;
    public float CritDame;
    public float Movement;
    public bool FacingRight;
    public Rigidbody2D RB;
    public float JumpHeight;
    public bool InGround;
    public bool Jumping;
    public Animator animator;
    public bool WallSlide;
    [Header ("Attack")]
    public Transform AttackPosition;
    public float AttackRadius = 1f;
    public LayerMask AttackLayer;
    protected virtual void Start()
    {
        FacingRight = true;
        RB = GetComponent<Rigidbody2D>();
        InGround = true;
        Jumping = false;
        WallSlide = false;
    }




    public void Run()
    {
        Movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(Movement, 0f, 0f) * Speed * Time.deltaTime;
        if (Movement < 0f && FacingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180, 0f);
            FacingRight = false;
        }
        else if (Movement > 0f && !FacingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            FacingRight = true;
        }
        if (math.abs(Movement) > 0.1f && InGround) //chay
        {
            animator.SetFloat("Run", 1f);
        }
        else
        {
            animator.SetFloat("Run", 0f);
        }

    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ( InGround ||  WallSlide) )
        {
            RB.AddForce(new Vector2(0f,JumpHeight),ForceMode2D.Impulse);
            InGround = false;
            animator.SetBool("Grounded", false);
            animator.SetTrigger("Jump");
            

        }
    }
    public void Roll()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Roll");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            InGround = true;
            WallSlide = false;
            animator.SetBool("Grounded", true);
            animator.SetBool("WallSlide", false);
        }
        if(collision.gameObject.tag == "Wall")
        {
            animator.SetBool("WallSlide", true);
            WallSlide = true;
            InGround = false;
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))//danh yeu neu nhan chuot trai
        {
            animator.SetTrigger("Attack1");
        }
        else if (Input.GetMouseButtonDown(1)) //danh manh neu an chuot phai
        {
            animator.SetTrigger("Attack2");
        }
    }
    public void Hit()
    {
       Collider2D colinfo = Physics2D.OverlapCircle(AttackPosition.position, AttackRadius, AttackLayer);
        if (colinfo)
        {
            if (colinfo.gameObject.GetComponent<Enemy>() != null)
            {
                colinfo.gameObject.GetComponent<Enemy>().GetHit(AtackPower);

            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPosition.position, AttackRadius);
    }
    public void GetHit(float Damage)
    {
        if (HP <= 0)
        {
            Die();
        }

        HP -= (Damage - Def / 2);
        animator.SetTrigger("Hurt");

    }
    public void Die()
    {
        Destroy(gameObject);

    }



}
