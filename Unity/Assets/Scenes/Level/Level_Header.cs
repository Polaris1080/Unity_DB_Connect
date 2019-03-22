//Level_X関連のパラメーターを管理するヘッダー
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Level_Header : MonoBehaviour
    {
        [Header("Default")]
        [Tooltip("自分のＩＤ（基本は１で固定）")]
        public int    ID    = 1;

        [Tooltip("自分の名前（未接続時に適応）")]
        public string Name  = "FooBar";

        [Tooltip("破壊したブロック数（未接続時に適応）")]
        public int    Block = 0;


        [Header("Spawn")]
        [Tooltip("スポーンさせるブロック")]
        public GameObject spawntarget;

        [Tooltip("スポーンさせる間隔")]
        public Vector3 span_spawn;

        [Tooltip("Blockのしきい値")]
        public Vector3Int threshold_spawn;

        [Tooltip("スポーンさせる範囲")]
        public Vector2Int range_spawn;

        [Tooltip("スポーン履歴（記憶されている場所にはスポーンしない）")]
        public int memory_spawn;


        [Header("Network")]
        [Tooltip("DBにUpdateする間隔")]
        public int netInterval = 5;
    }
}

