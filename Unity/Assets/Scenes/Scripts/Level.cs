using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Level : MonoBehaviour {
    public GameObject SpawnTarget; //スポーンさせるブロック
    GameObject UI_ID;
    GameObject UI_NAME;
    GameObject UI_BLOCK;
    float  delda   = 0;     //積算デルタ秒
    string m_name  = "";    //自分の名前
    int    m_block = 0;     //自分が破壊したブロックの数
    const int ID       = 1; //ID=1に固定
    const int interval = 5; //DBにUpdateする間隔
    

    [Serializable]
    public class UserData //DBと送受信するデータを定義
    {
        public string name;
        public int    block;
    }
    
    void Start () {
        //UIへの参照を取得
        this.UI_ID    = GameObject.Find("ID");
        this.UI_NAME  = GameObject.Find("NAME");
        this.UI_BLOCK = GameObject.Find("BLOCK");
        StartCoroutine(LoadDB());         //DBからユーザー情報を読み込む
        StartCoroutine(UpdateRepeater()); //定期的にUpdateDBを実行
    }
	
	void Update ()
    {
        GenerateBlock(); //ブロックを生成
	}

    private void OnApplicationQuit()
    {
        StartCoroutine(UpdateDB()); //DBを更新
    }

    public void IncrementBlock()
    {
        m_block++; //ブロックの破壊数を１増やす
        this.UI_BLOCK.GetComponent<Text>().text = m_block.ToString(); //UIを更新
    }
    
    void GenerateBlock()
    {
        float span;                   //スポーンさせる間隔
        this.delda += Time.deltaTime; //デルタ秒を更新
        int blocks = GameObject.FindGameObjectsWithTag("Block").Length; //Blockの数を数える

        //Blockの数に合わせて、スポーン間隔を調整
        if      ( 0 <= blocks && blocks < 10) { span = 0.5f; }
        else if (10 <= blocks && blocks < 15) { span = 1.0f; }
        else if (15 <= blocks && blocks < 20) { span = 2.0f; }
        else                                  { span = Mathf.Infinity; }
        
        if (this.delda > span) //スポーン出来るだけの間隔が空いたら
        {  
            this.delda      = 0;                                      //間隔を初期化
            GameObject item = Instantiate(SpawnTarget) as GameObject; //スポーン
            int x           = UnityEngine.Random.Range(-8, 9);
            int y           = UnityEngine.Random.Range( 1, 3);
            item.transform.position = new Vector3(x, y, 0);           //位置調整
        }
    }

    private IEnumerator UpdateRepeater() //定期的にUpdateDBを実行
    {
        while (true) //intervalが経過したら
        {
            StartCoroutine(UpdateDB()); //DBを更新
            yield return new WaitForSeconds(interval);
        }
    }
    private IEnumerator LoadDB() //DBからユーザー情報を読み込む
    {
        UnityWebRequest request = UnityWebRequest.Get(string.Format("http://localhost:8000/api/users/{0}/",ID)); //GETリクエストを生成
        yield return request.SendWebRequest();             //送信
        if (request.isHttpError || request.isNetworkError) //エラー判定
        {   Debug.Log(request.error);   }                  //エラー確認
        else
        {
            UserData jsonClass = JsonUtility.FromJson<UserData>(request.downloadHandler.text); //受信

            //受信データを使用して更新
            m_name = jsonClass.name;
            m_block = jsonClass.block;

            //UIの表示の変更
            this.UI_ID.GetComponent<Text>().text    = ID.ToString();
            this.UI_NAME.GetComponent<Text>().text  = m_name;
            this.UI_BLOCK.GetComponent<Text>().text = m_block.ToString();
        }
    }
    private IEnumerator UpdateDB()
    {
        UnityWebRequest request = new UnityWebRequest(string.Format("http://localhost:8000/api/users/{0}/", ID), "PUT"); //PUTリクエストを生成
        UserData data = new UserData() { name = m_name, block = m_block };             //jsonを設定
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)); //jsonをbyte[]に変換

        //ヘッダーにタイプを設定
        request.uploadHandler   = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();             //送信
        if (request.isHttpError || request.isNetworkError) //エラー判定
        { Debug.Log(request.error); }                      //エラー確認

    }




}
