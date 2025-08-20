using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSound : MonoBehaviour
{
    private AudioSource _Audio;
    void Start()
    {
        _Audio = GetComponent<AudioSource>();
        StartCoroutine(SoundBackground());
    }

    IEnumerator SoundBackground()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 8f));
            _Audio.Play();
        }
    }
}
