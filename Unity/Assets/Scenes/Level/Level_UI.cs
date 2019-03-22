//UIの挙動について定義。
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class Level_UI : MonoBehaviour 
    {
        [SerializeField] private Level_Header Header;
        private GameObject UI_ID;
        private int        T_ID;    //以前のIDの値
        private GameObject UI_NAME;
        private string     T_NAME;  //以前のNAMEの値
        private GameObject UI_BLOCK;
        private int        T_BLOCK; //以前のBLOCKの値

        void Start()
        {
            //UIへの参照を取得
            this.UI_ID    = GameObject.Find("ID");
            this.UI_NAME  = GameObject.Find("NAME");
            this.UI_BLOCK = GameObject.Find("BLOCK");

            //初期化
            T_ID    = this.Header.ID;
            T_NAME  = this.Header.Name;
            T_BLOCK = this.Header.Block;
        }
        void Update()
        {
            //値が変化した時のみ更新
            if (T_ID    != this.Header.ID)
            {
                T_ID    = this.Header.ID;                                  //以前の値を更新
                this.UI_ID.GetComponent<Text>().text    = T_ID.ToString(); //UIを更新
            }
            if (T_NAME  != this.Header.Name)
            {
                T_NAME  = this.Header.Name;
                this.UI_NAME.GetComponent<Text>().text  = T_NAME;
            }
            if (T_BLOCK != this.Header.Block)
            {
                T_BLOCK = this.Header.Block;
                this.UI_BLOCK.GetComponent<Text>().text = T_BLOCK.ToString();
            }
        }
    }
}