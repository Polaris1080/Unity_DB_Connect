using UnityEngine;

public class BlockController : MonoBehaviour
{
    public LayerMask whatIsPlayer;//コリジョン2Dレイヤー
    public GameObject brokenBlock;//破壊後にスポーン
    public bool Breakable;//破壊可能
    private BoxCollider2D collision; //コリジョン
    GameObject Level;


    private void Awake()
    {
        collision = GetComponent<BoxCollider2D>(); //コリジョンを設定
        this.Level = GameObject.Find("Level");
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {
            //接触を検証
            Vector2 groundCheck = new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y);
            Vector2 groundArea = new Vector2(collision.size.x * transform.lossyScale.y * 0.45f, 0.05f);
            var col2D = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsPlayer);

            if (col2D && Breakable)
            {
                this.Level.GetComponent<Level>().IncrementBlock();
                GameObject broken = Instantiate(brokenBlock, transform.position, transform.rotation);//スポーン
                broken.transform.localScale = transform.lossyScale;//自身と同位置に移動
                Destroy(gameObject);//自身を破壊
            }
        }
    }
}