//Unityちゃんの挙動について定義、UnityChan2Dから改変。
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
    [Tooltip("移動速度（最速）")]
    public  float         maxSpeed      = 10f;
    [Tooltip("ジャンプ力")]
    public  float         jumpPower     = 1000f;
    [Tooltip("地面が存在するレイヤー")]
    public  LayerMask     whatIsGround;
    private Animator      c_animator;
    private BoxCollider2D c_boxcollier2D;
    private Rigidbody2D   c_rigidbody2D;
    
    private bool          m_isGround; //地面に接地しているか？

    private const float   m_offsetOverlapAreaY = 1.5f; //当たり判定のオフセット（本体の中心から下）
    

    void Reset() //初期化
    {
        Awake();

        // UnityChan2DController
        maxSpeed      = 10f;
        jumpPower     = 1000;
        whatIsGround  = 1 << LayerMask.NameToLayer("Ground");

        // Transform
        transform.localScale = new Vector3(1, 1, 1);

        // Rigidbody2D
        c_rigidbody2D.gravityScale   = 3.5f;
        c_rigidbody2D.freezeRotation = true;

        // BoxCollider2D
        c_boxcollier2D.size   = new Vector2(1, 2.5f);
        c_boxcollier2D.offset = new Vector2(0, -0.25f);

        // Animator
        c_animator.applyRootMotion = false;
    }

    void Awake()//起動時の初期化
    {
        c_animator     = GetComponent<Animator>();
        c_boxcollier2D = GetComponent<BoxCollider2D>();
        c_rigidbody2D  = GetComponent<Rigidbody2D>();
    }

    void Update() { Move(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump")); }

    void Move(float move, bool jump) //移動
    {
        float[] T = new float[3];

        if (Mathf.Abs(move) > 0) //方向転換
        {
            T[0] = transform.rotation.x;            //Ｘ軸
            T[1] = Mathf.Sign(move) == 1 ? 0 : 180; //Ｙ軸
            T[2] = transform.rotation.z;            //Ｚ軸
            transform.rotation = Quaternion.Euler(T[0], T[1], T[2]);
        }
        
        //加速
        T[0] = move * maxSpeed;          //Ｘ軸
        T[1] = c_rigidbody2D.velocity.y; //Ｙ軸
        c_rigidbody2D.velocity = new Vector2(T[0], T[1]);

        //アニメーションに情報を送る
        c_animator.SetFloat("Horizontal", move);
        c_animator.SetFloat("Vertical",   c_rigidbody2D.velocity.y);
        c_animator.SetBool ("isGround",   m_isGround);

        if (jump && m_isGround)
        {
            //アニメーションに情報を送る
            c_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);

            //上向きに力を加えてジャンプ
            c_rigidbody2D.AddForce(Vector2.up * jumpPower);
        }
    }

    void FixedUpdate() //物理の更新
    {
        float[] T = new float[4];

        //T[0] = pointCenter.x  T[1] = overlapArea.x  T[2] = pointCenter.y  T[3] = overlapArea.y
        T[0] = transform.position.x;
        T[1] = c_boxcollier2D.size.x * 0.49f;
        T[2] = transform.position.y - (m_offsetOverlapAreaY * transform.localScale.y);
        T[3] = 0.05f;

        //左上の座標  upperleft = pointCenter + overlapArea;
        Vector2 upperleft   = new Vector2(T[0]+T[1], T[2]+T[3]);
        //右下の座標  bottomright = pointCenter - overlapArea;
        Vector2 bottomright = new Vector2(T[0]-T[1], T[2]-T[3]);
        
        //当たり判定
        m_isGround = Physics2D.OverlapArea(upperleft, bottomright, whatIsGround);

        //アニメーションに結果を送る
        c_animator.SetBool("isGround", m_isGround);
    }
}