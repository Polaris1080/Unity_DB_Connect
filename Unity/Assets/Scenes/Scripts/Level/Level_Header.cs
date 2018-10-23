//Level_X関連のパラメーターを管理するヘッダー
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Header : MonoBehaviour {
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
    public float[]    span_spawn      = new float[5];
    [Tooltip("Blockのしきい値")]
    public float[]    threshold_spawn = new float[4];
    [Tooltip("X_min,X_max,Y_min,Y_max")]
    public sbyte[]    range_spawn     = new sbyte[4];
    [Header("Network")]
    [Tooltip("DBにUpdateする間隔")]
    public int netInterval = 5;
}
