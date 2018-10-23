//Unityちゃんの挙動について定義、UnityChan2Dから改変。
using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
    public  float         maxSpeed      = 10f;
    public  float         jumpPower     = 1000f;
    public  Vector2       backwardForce = new Vector2(-4.5f, 5.4f);
    public  LayerMask     whatIsGround;
    private Animator      m_animator;
    private BoxCollider2D m_boxcollier2D;
    private Rigidbody2D   m_rigidbody2D;
    private bool          m_isGround;
    private const float   m_centerY     = 1.5f;
    

    void Reset() //初期化
    {
        Awake();

        // UnityChan2DController
        maxSpeed      = 10f;
        jumpPower     = 1000;
        backwardForce = new Vector2(-4.5f, 5.4f);
        whatIsGround  = 1 << LayerMask.NameToLayer("Ground");

        // Transform
        transform.localScale = new Vector3(1, 1, 1);

        // Rigidbody2D
        m_rigidbody2D.gravityScale   = 3.5f;
        m_rigidbody2D.freezeRotation = true;

        // BoxCollider2D
        m_boxcollier2D.size   = new Vector2(1, 2.5f);
        m_boxcollier2D.offset = new Vector2(0, -0.25f);

        // Animator
        m_animator.applyRootMotion = false;
    }

    void Awake()//起動時の初期化
    {
        m_animator     = GetComponent<Animator>();
        m_boxcollier2D = GetComponent<BoxCollider2D>();
        m_rigidbody2D  = GetComponent<Rigidbody2D>();
    }

    void Update() { Move(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump")); }

    void Move(float move, bool jump) //移動
    {
        if (Mathf.Abs(move) > 0)
        {   Quaternion rot     = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);   }
        m_rigidbody2D.velocity = new Vector2(move * maxSpeed, m_rigidbody2D.velocity.y);

        m_animator.SetFloat("Horizontal", move);
        m_animator.SetFloat("Vertical",   m_rigidbody2D.velocity.y);
        m_animator.SetBool ("isGround",   m_isGround);

        if (jump && m_isGround)
        {   m_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
            m_rigidbody2D.AddForce(Vector2.up * jumpPower);                 }
    }

    void FixedUpdate() //物理の更新
    {
        Vector2 pos         = transform.position;
        Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
        Vector2 groundArea  = new Vector2(m_boxcollier2D.size.x * 0.49f, 0.05f);

        m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
        m_animator.SetBool("isGround", m_isGround);
    }
}
