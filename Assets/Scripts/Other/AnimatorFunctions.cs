using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Animator setBoolInAnimator;
    void Start()
    {
        if (!audioSource) audioSource = NewPlayer.Instance.audioSource;
    }

    public void HidePlayer(bool hide)
    {
        NewPlayer.Instance.Hide(hide);
    }
  
    public void JumpPlayer(float power = 1f)
    {
        NewPlayer.Instance.Jump(power);
    }

    void FreezePlayer(bool freeze)
    {
        NewPlayer.Instance.Freeze(freeze);
    }

    void PlaySound(AudioClip whichSound)
    {
        audioSource.PlayOneShot(whichSound, GameManager.Instance.audioSource.volume);
    }

    public void EmitParticles(int amount)
    {
        particleSystem.Emit(amount);
    }

    public void ScreenShake(float power)
    {
        NewPlayer.Instance.cameraEffects.Shake(power, 1f);
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void SetAnimBoolToFalse(string boolName)
    {
        setBoolInAnimator.SetBool(boolName, false);
    }

    public void SetAnimBoolToTrue(string boolName)
    {
        setBoolInAnimator.SetBool(boolName, true);
    }

    public void FadeOutMusic()
    {
       GameManager.Instance.gameMusic.GetComponent<AudioTrigger>().maxVolume = 0f;
    }

    public void LoadScene(string whichLevel)
    {
        SceneManager.LoadScene(whichLevel);
    }

    public void SetTimeScaleTo(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
    