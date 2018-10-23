//ブロックの挙動について定義、UnityChan2Dから改変。
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public  GameObject    brokenBlock;  //破壊後にスポーン
    public  LayerMask     whatIsPlayer; //コリジョン2Dレイヤー
    public  bool          breakable;    //破壊可能
    private BoxCollider2D collision;    //コリジョン
    private GameObject    level;


    private void Awake()
    {
        collision  = GetComponent<BoxCollider2D>(); //コリジョンを設定
        this.level = GameObject.Find("Level");      //Levelを検索
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {
            //接触を検証
            Vector2 groundCheck = new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y);
            Vector2 groundArea  = new Vector2(collision.size.x * transform.lossyScale.y * 0.45f, 0.05f);
            var col2D = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsPlayer);

            if (col2D && breakable)
            {
                this.level.GetComponent<Level_Header>().Block++;                                      //Level_HeaderにBlockを破壊したことを伝える
                GameObject broken = Instantiate(brokenBlock, transform.position, transform.rotation); //スポーン
                broken.transform.localScale = transform.lossyScale;    Destroy(gameObject);           //自身と同位置に移動  //自身を破壊                                      
            }
        }
    }
}