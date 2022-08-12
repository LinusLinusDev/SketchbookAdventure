using System.Collections;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] private bool autoPlay;
    [SerializeField] private bool controlsTitle;
    [SerializeField] private float fadeSpeed; 
    [SerializeField] private bool loop;
    [SerializeField] private AudioClip sound;
    public float maxVolume;
    private bool triggered;

    void Start()
    {
        Reset(false, sound, 0);
        StartCoroutine(EnableCollider());
    }

    void Update()
    {
        audioSource.loop = loop;

        if (audioSource.volume > maxVolume) audioSource.volume = maxVolume;
        
        if (triggered || autoPlay)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            
            if (audioSource.volume < maxVolume)
            {
                audioSource.volume += fadeSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (audioSource.volume > 0)
            {
                audioSource.volume -= fadeSpeed * Time.deltaTime;
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject == NewPlayer.Instance.gameObject)
        {
            if (!triggered)
            {
                if (controlsTitle)
                {
                    GameManager.Instance.hud.animator.SetBool("showTitle", true);
                }

                triggered = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col == NewPlayer.Instance)
        {
            triggered = false;
        }
    }
    
    public void Reset(bool play, AudioClip clip, float startVolume = 1)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = startVolume;
        audioSource.clip = clip;
        if (play)
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(4f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
