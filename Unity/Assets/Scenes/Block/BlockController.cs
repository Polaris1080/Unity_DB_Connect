//ブロックの挙動について定義、UnityChan2Dから改変。
using UnityEngine;
using System;
[RequireComponent(typeof(BoxCollider2D))]

public class BlockController : MonoBehaviour
{
    [Tooltip("破壊後にスポーンされるブロック")]
    public                   GameObject    brokenBlock;
    [Tooltip("破壊後にスポーンされるコイン")]
    public                   GameObject    spawnCoin;
    [Tooltip("破壊可能かどうか")]
    public                   bool          breakable;
    [Tooltip("コインの出現率")]
    public                   float         coin_spawn = 0.5f;
    [Tooltip("コリジョン2Dレイヤー")]
    [SerializeField] private LayerMask     whatIsPlayer;
    private                  BoxCollider2D c_collision;
    private                  GameObject    m_level;


    private void Awake()
    {
        c_collision  = GetComponent<BoxCollider2D>(); //コリジョンを設定
        this.m_level = GameObject.Find("Level");      //Levelを検索
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {
            //当たり判定の確認
            bool col2D = MyLibrary.OverlapArea.Check(transform.position.x,       transform.position.y - transform.lossyScale.y,
                                                     c_collision.size.x * 0.49f, 0.05f,
                                                     whatIsPlayer);
            if (col2D && breakable)
            {
                this.m_level.GetComponent<Level_Header>().Block++;    //Level_HeaderにBlockを破壊したことを伝える

                MyLibrary.Spawn.Sameplace(brokenBlock,transform);     //ブロックのスポーン

                if (UnityEngine.Random.Range(0f, 1f) <= coin_spawn)   //乱数が出現率以下なら、
                { MyLibrary.Spawn.Sameplace(spawnCoin, transform); }  //コインをスポーンさせる

                Destroy(gameObject);                                  //自身を破壊 
            }
        }
    }
}