using UnityEngine;
using System.Collections;

public class SoundRandomizer : MonoBehaviour
{
	public AudioClip[] audioClips;
	private AudioSource audioSource;
	// Use this for initialization
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }

	// Update is called once per frame
	void Update()
	{

    }
}

