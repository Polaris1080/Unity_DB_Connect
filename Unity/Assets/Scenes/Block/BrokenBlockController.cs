using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BrokenBlockController : MonoBehaviour
{
    public AudioClip breakClip;                    //破壊音
    public Vector2 force = new Vector2(250, 1000); //破壊時に加わる力
    private Rigidbody2D[] collision;               //コリジョン


    void Awake()
    {
        collision = GetComponentsInChildren<Rigidbody2D>(); //コリジョンを設定
    }

    void Start()
    {
        //グループ化
        IEnumerable<IGrouping<float, Rigidbody2D>> groupBy = collision.GroupBy(r => r.transform.localPosition.y);
        //グループ毎に力を加える
        foreach (IGrouping<float, Rigidbody2D> grouping in groupBy)
        {   foreach (var r in grouping)
            { r.AddForce(new Vector2(Mathf.Sign(r.transform.localPosition.x) * force.x, force.y + (100 * grouping.Key))); }   }
        AudioSourceController.instance.PlayOneShot(breakClip); //破壊音を再生
        Destroy(gameObject, 3);                                //自身を破壊
    }
}
