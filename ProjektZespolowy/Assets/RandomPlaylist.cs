using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlaylist : MonoBehaviour
{
  public AudioClip[] clips;
  private AudioSource source;
  private float newClip;
  private float timer;
  private int clipNum = 0;

  // Start is called before the first frame update
  void Start()
  {
    source = gameObject.AddComponent<AudioSource>();
    source.volume = 0.35f;
  }

  // Update is called once per frame
  void Update()
  {
    if (!source.isPlaying)
      newCLIP();
  }

  void newCLIP()
  {
    if(!source.isPlaying)
    {
      source.loop = true;
      source.PlayOneShot(clips[clipNum]);
    }

    newClip = clips[clipNum].length;

    clipNum++;
    if (clipNum >= clips.Length)
      clipNum = 0;
  }
}
