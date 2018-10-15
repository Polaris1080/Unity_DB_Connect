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
                m_instance = FindObjectOfType<AudioSourceController>(); //知らなかったら探す
                if (m_instance == null)                                 //探しても見つからなかったら作る
                { m_instance = new GameObject("AudioSourceController").AddComponent<AudioSourceController>(); }
            }
            return m_instance;
        }
    }
    public void PlayOneShot(AudioClip clip){ GetComponent<AudioSource>().PlayOneShot(clip); }
}
