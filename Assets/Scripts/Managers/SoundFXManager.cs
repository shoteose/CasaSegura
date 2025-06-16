using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [Header("Soms")]
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioClip correto;
    [SerializeField] private AudioClip errado;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip dados;


    [SerializeField] private float volume = 1.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Som(bool acertou)
    {

        AudioSource audioSource = Instantiate(soundFXObject, new Vector3(0,0,0) , Quaternion.identity);

        audioSource.clip = this.errado;
        if (acertou) audioSource.clip = this.correto;

        audioSource.volume = this.volume;
        audioSource.Play();
        float duracaoClip = audioSource.clip.length;

        Destroy(audioSource.gameObject, duracaoClip );


    }

    public void Click()
    {
        AudioSource audioSource = Instantiate(soundFXObject, new Vector3(0, 0, 0), Quaternion.identity);

        audioSource.clip = this.click;

        audioSource.volume = this.volume;
        audioSource.Play();
        float duracaoClip = audioSource.clip.length;

        Destroy(audioSource.gameObject, duracaoClip);
    }

    public void Dados()
    {
        AudioSource audioSource = Instantiate(soundFXObject, new Vector3(0, 0, 0), Quaternion.identity);

        audioSource.clip = this.dados;

        audioSource.volume = this.volume;
        audioSource.Play();
        float duracaoClip = audioSource.clip.length;

        Destroy(audioSource.gameObject, duracaoClip);
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
    }

}
