//破壊されたブロックの挙動について定義、UnityChan2Dから改変。
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class BrokenBlockController : MonoBehaviour
{
    [Tooltip("破壊時に加わる力")]
    public  Vector2        force = new Vector2(250, 1000);
    [Tooltip("上下を分離するために加える力")]
    public  float     splitPower = 100f;
    [Tooltip("randamの範囲（Ｘ軸）")]
    public  Vector2 randamRangeX = new Vector2(0.5f, 2f);
    [Tooltip("randamの範囲（Ｙ軸）")]
    public  Vector2 randamRangeY = new Vector2(0.5f, 2f);
    [Tooltip("破壊音")]
    public  AudioClip     breakClip;

    private Rigidbody2D[] c_collision; //コリジョン


    void Awake() => c_collision = GetComponentsInChildren<Rigidbody2D>();  //コリジョンを設定

    void Start()
    {
        //グループ化してから、グループ毎に力を加える
        IEnumerable<IGrouping<float, Rigidbody2D>> groupBy = c_collision.GroupBy(r => r.transform.localPosition.y);
        foreach (IGrouping<float, Rigidbody2D> grouping in groupBy)
        {   
            foreach (var r in grouping)
            {
                //方角の決定
                float directionX = Mathf.Sign(r.transform.localPosition.x) >=0 ? 1f:-1f;
                float directionY = grouping.Key >= 0 ? 1f:-1f;
                //高度補正
                float correctionAltitude = Mathf.Exp(1/((transform.position.y/2)+1));
                //ランダム
                float randamX = UnityEngine.Random.Range(randamRangeX.x,randamRangeX.y);
                float randamY = UnityEngine.Random.Range(randamRangeY.x,randamRangeY.y);
                //加わる力を算出
                float forceX = force.x * correctionAltitude * directionX * randamX;
                float forceY = force.y + (splitPower * directionY) * randamY;
                
                //力を加える
                r.AddForce(new Vector2(forceX, forceY));
            }
        }
        //破壊音を再生した後、自身を破壊
        GetComponent<AudioSource>().PlayOneShot(breakClip);
        
        Destroy(gameObject, 2);
    }
}
