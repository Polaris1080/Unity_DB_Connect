using UnityEngine;

public class CoinController : MonoBehaviour {
	[Tooltip("Coinsの最大しきい値")]
    [SerializeField] private int   coins_limit = 4;
	[Tooltip("Coin一個毎に上昇するピッチ")]
    [SerializeField] private float pitch_up    = 0.125f;
	[Tooltip("上昇するピッチのぶれ（％）")]
    [SerializeField] private float pitch_shake = 0.25f;
	[Tooltip("寿命")]
    [SerializeField] private float lifetime    = 2.5f;
	
	
	void Start () {
		//Level内に存在するCoinを数える
		int coins_amount = GameObject.FindGameObjectsWithTag("Coin").Length;
		//最大しきい値におさまるよう調整
		coins_amount = (coins_amount>=coins_limit) ? coins_limit:coins_amount;

		//Coin_getのピッチ変更
		AudioSource coin_get = gameObject.GetComponent<AudioSource>();
		float shake    = 1 + Random.Range(pitch_shake * -1f, pitch_shake); //ブレ
		float tone     = (coins_amount - 1) * pitch_up;                    //階調
		coin_get.pitch = 1 + (tone * shake);

		//寿命が来たら破壊
		Destroy(gameObject, lifetime);
	}
}
