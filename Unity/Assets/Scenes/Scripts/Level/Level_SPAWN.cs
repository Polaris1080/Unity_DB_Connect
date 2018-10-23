//ブロックの生成について定義。
using UnityEngine;

public class Level_SPAWN : MonoBehaviour{
    [SerializeField] private Level_Header Header;
    private float delda_spawn = 0; //積算デルタ秒

    void Update()
    {
        float span = 0; //スポーンさせる間隔
        this.delda_spawn += Time.deltaTime; //デルタ秒を更新
        int blocks = GameObject.FindGameObjectsWithTag("Block").Length; //Level内に存在するBlockを数える

        //Blockの数に合わせて、スポーン間隔を調整
        if      (                             0 <= blocks && blocks < this.Header.threshold_spawn[0]) { span = this.Header.span_spawn[0]; }
        else if (this.Header.threshold_spawn[0] <= blocks && blocks < this.Header.threshold_spawn[1]) { span = this.Header.span_spawn[1]; }
        else if (this.Header.threshold_spawn[1] <= blocks && blocks < this.Header.threshold_spawn[2]) { span = this.Header.span_spawn[2]; }
        else if (this.Header.threshold_spawn[2] <= blocks && blocks < this.Header.threshold_spawn[3]) { span = this.Header.span_spawn[3]; }
        else                                                                                          { span = this.Header.span_spawn[4]; }

        if (this.delda_spawn > span) //スポーン出来るだけの間隔が空いたら
        {
            this.delda_spawn = 0; //積算デルタ秒を初期化
            GameObject item = Instantiate(this.Header.spawntarget) as GameObject; //スポーン
            int x = UnityEngine.Random.Range(this.Header.range_spawn[0], this.Header.range_spawn[1]);
            int y = UnityEngine.Random.Range(this.Header.range_spawn[2], this.Header.range_spawn[3]);
            item.transform.position = new Vector3(x, y, 0); //位置調整
        }
    }
}
