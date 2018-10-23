//音楽について定義、UnityChan2Dから改変。
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour
{
    private static AudioSourceController m_instance;
    public  static AudioSourceController instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<AudioSourceController>(); //知らないなら探す、見つからないなら作る
                if (m_instance == null){ m_instance = new GameObject("AudioSourceController").AddComponent<AudioSourceController>(); }
            }
            return m_instance;
        }
    }
    public void PlayOneShot(AudioClip clip){ GetComponent<AudioSource>().PlayOneShot(clip); }
}
