//http通信について定義。
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Level_Network : MonoBehaviour {
    [SerializeField] private Level_Header Header;
                     private int          block_last; //前回送信前のブロック破壊数 
    //送受信するデータの定義
    [Serializable] public class Data_DB  { public string name;  public int block; }
    [Serializable] public class Data_LOG { public int    id;    public int add;   }


    private void Start() //起動時
    {
        StartCoroutine(LoadDB());         //DBからユーザー情報を読み込む
        StartCoroutine(UpdateRepeater()); //定期的にUpdateDBを実行
    }
    private void OnApplicationQuit() { StartCoroutine(UpdateDB()); } //終了時
    
    private IEnumerator UpdateRepeater() //定期的にUpdateDBを実行
    {
        while (true) //intervalが経過したら
        {
            StartCoroutine(UpdateDB());    //DBを更新
            StartCoroutine(AddBlockLOG()); //LOGに追記
            yield return new WaitForSeconds(this.Header.netInterval);
        }
    }

    private IEnumerator LoadDB()     //DBからユーザー情報を読み込む
    {
        //GETリクエストを生成
        UnityWebRequest request = UnityWebRequest.Get($"http://127.0.0.1:8000/Unity/DB/{this.Header.ID}");
        //送信
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError) { Debug.Log(request.error); }          //エラー確認
        else { Data_DB jsonClass = JsonUtility.FromJson<Data_DB>(request.downloadHandler.text);   //受信
               this.Header.Name = jsonClass.name;  this.Header.Block = jsonClass.block;         } //受信データを使用して更新
    }
    private IEnumerator UpdateDB()   //DB上のユーザー情報を更新する
    {
        //PUTリクエストを生成
        UnityWebRequest request = new UnityWebRequest($"http://127.0.0.1:8000/Unity/DB/{this.Header.ID}", "PUT");
        Data_DB data    = new Data_DB() { name = this.Header.Name, block = this.Header.Block }; //jsonを生成
        byte[]   bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));        //byteに変換
        //ヘッダーにタイプを設定
        request.uploadHandler   = (UploadHandler)  new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader  ("Content-Type", "application/json");
        //送信
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError) { Debug.Log(request.error); } //エラー判定・確認
    }
    private IEnumerator AddBlockLOG() //LOGにブロック破壊数を追記
    {
        //POSTリクエストを生成
        UnityWebRequest request = new UnityWebRequest($"http://127.0.0.1:8000/Unity/log", "POST");
        Data_LOG data = new Data_LOG() { id = this.Header.ID, add = this.Header.Block - this.block_last }; //jsonを設定
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));                     //byteに変換
        //ヘッダーにタイプを設定
        request.uploadHandler   = (UploadHandler)  new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader  ("Content-Type", "application/json");
        //送信
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError) { Debug.Log(request.error); } //エラー判定・確認
        this.block_last = this.Header.Block;                                             //現在のブロック破壊数を記録
    }
}
