//ブロックの挙動について定義、UnityChan2Dから改変。
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class BlockController : MonoBehaviour
{
    [Tooltip("破壊後にスポーンされるオブジェクト")]
    public  GameObject    brokenBlock;
    [Tooltip("コリジョン2Dレイヤー")]
    public  LayerMask     whatIsPlayer;
    [Tooltip("破壊可能かどうか")]
    public  bool          breakable;
    private BoxCollider2D c_collision;
    private GameObject    m_level;


    private void Awake()
    {
        c_collision  = GetComponent<BoxCollider2D>(); //コリジョンを設定
        this.m_level = GameObject.Find("Level");      //Levelを検索
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {            
            float[] T = new float[4];

            //T[0] = pointCenter.x  T[1] = overlapArea.x  T[2] = pointCenter.y  T[3] = overlapArea.y
            T[0] = transform.position.x;
            T[1] = c_collision.size.x * 0.49f;
            T[2] = transform.position.y - transform.lossyScale.y;
            T[3] = 0.05f;

            //左上の座標  upperleft = pointCenter + overlapArea;
            Vector2 upperleft   = new Vector2(T[0]+T[1], T[2]+T[3]);
            //右下の座標  bottomright = pointCenter - overlapArea;
            Vector2 bottomright = new Vector2(T[0]-T[1], T[2]-T[3]);
        
            //当たり判定
            bool col2D   = Physics2D.OverlapArea(upperleft, bottomright, whatIsPlayer);

            if (col2D && breakable)
            {
                //Level_HeaderにBlockを破壊したことを伝える
                this.m_level.GetComponent<Level_Header>().Block++;

                //同位置にスポーンする
                GameObject broken = Instantiate(brokenBlock, transform.position, transform.rotation);
                
                //自身と同位置に移動 
                broken.transform.localScale = transform.lossyScale;

                //自身を破壊 
                Destroy(gameObject);
            }
        }
    }
}