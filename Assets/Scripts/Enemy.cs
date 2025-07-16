using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float HP ;
    public float def ;
    public float speed = 2.5f;
    public float AttackPower = 2;
    public float ChaseRange = 5f;
    public float AttackRange = 3f;
    [Header("Mechanic")]
    public bool facingright;
    public Transform Player;
    public LayerMask Ground;
    public Transform DetectPoint;
    public float Distance = 1f;
    public Animator Animator;
    public float DistanceFromPlayer;
    public Rigidbody2D rb;
    [Header("Attack")] 
    public Transform AttackPosition;
    public float AttackRadius = 1f;
    public LayerMask AttackLayer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        facingright = true;
        rb = GetComponent<Rigidbody2D>();  
        
    }

    // Update is called once per frame
    void Update()
    {
        
        DistanceFromPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (Vector2.Distance(transform.position,Player.transform.position) <= ChaseRange) // nếu kẻ địch ở trong tầm đuổi thì sẽ đi về hướng người chơi
        {
            if(transform.position.x > Player.position.x && facingright == true)
            {
                Flip(false);
            }
            else if(transform.position.x < Player.position.x && facingright == false)
            {
                Flip(true);
            }
                
            if(Vector2.Distance(transform.position,Player.transform.position) > AttackRange)
            {
                Vector2 Target = new Vector2(Player.position.x,rb.position.y);
                rb.MovePosition(Vector2.MoveTowards(transform.position, Target, speed * Time.deltaTime));
                
            }
            else
            {
               
                Animator.SetBool("Attack",true); 
                
            }
        }
        else 
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            RaycastHit2D hit = Physics2D.Raycast(DetectPoint.position, Vector2.down, Distance, Ground);
            
            if (!hit)
            {
                if(facingright )
                {
                    transform.eulerAngles = new Vector3(0, -180f,0);
                    facingright=false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    facingright=true;    
                }
            }

        }
    }
    public void Attack()
    {
       Collider2D colinfo =  Physics2D.OverlapCircle(AttackPosition.position, AttackRadius, AttackLayer);
        if(colinfo)
        {
            if (colinfo.gameObject.GetComponent<Char1>() != null)
            {
                colinfo.gameObject.GetComponent<Char1>().GetHit(AttackPower);

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(DetectPoint == null)
        {
            return;
        }    
        Gizmos.color = Color.green;
        Gizmos.DrawRay(DetectPoint.position, Vector2.down * Distance);
        Gizmos.color= Color.red;
        if(AttackPosition == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(AttackPosition.position, AttackRadius);
        
        
    }
    public void GetHit(float Damage)
    {
        if (HP <= 0)
        {
            Die();
        }

        HP -= (Damage - def/2);
            Animator.SetTrigger("Hurt");
        
    }
    public void Die()
    {
            Destroy(gameObject);
        
    }
    void Flip(bool faceRight)
    {
        if (faceRight)
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        else
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        facingright = faceRight;
    }
}
