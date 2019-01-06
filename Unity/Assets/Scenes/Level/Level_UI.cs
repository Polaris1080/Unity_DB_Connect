//UIの挙動について定義。
using UnityEngine;
using UnityEngine.UI;

public class Level_UI : MonoBehaviour {
    [SerializeField] private Level_Header Header;
    private GameObject UI_ID, UI_NAME, UI_BLOCK;


    void Start()
    {
        //UIへの参照を取得
        this.UI_ID    = GameObject.Find("ID");
        this.UI_NAME  = GameObject.Find("NAME");
        this.UI_BLOCK = GameObject.Find("BLOCK");
    }
    void Update()
    {
        //int -> string
        string _ID    = this.Header.ID   .ToString();
        string _NAME  = this.Header.Name .ToString();
        string _BLOCK = this.Header.Block.ToString();
        //UIを更新
        this.UI_ID   .GetComponent<Text>().text = _ID;
        this.UI_NAME .GetComponent<Text>().text = _NAME;
        this.UI_BLOCK.GetComponent<Text>().text = _BLOCK;
    }
}
