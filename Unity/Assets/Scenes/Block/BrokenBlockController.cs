//破壊されたブロックの挙動について定義、UnityChan2Dから改変。
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrokenBlockController : MonoBehaviour
{
    public  Vector2       force = new Vector2(250, 1000); //破壊時に加わる力
    public  AudioClip     breakClip; //破壊音
    private Rigidbody2D[] collision; //コリジョン


    void Awake(){ collision = GetComponentsInChildren<Rigidbody2D>(); }//コリジョンを設定

    void Start()
    {
        //グループ化してから、グループ毎に力を加える
        IEnumerable<IGrouping<float, Rigidbody2D>> groupBy = collision.GroupBy(r => r.transform.localPosition.y);
        foreach (IGrouping<float, Rigidbody2D> grouping in groupBy)
        {   foreach (var r in grouping)
            { r.AddForce(new Vector2(Mathf.Sign(r.transform.localPosition.x) * force.x, force.y + (100 * grouping.Key))); }   }

        //破壊音を再生した後、自身を破壊
        AudioSourceController.instance.PlayOneShot(breakClip);  Destroy(gameObject, 2);
    }
}
