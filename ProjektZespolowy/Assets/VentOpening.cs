using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentOpening : MonoBehaviour
{
  [Header("References")]
  [SerializeField] Transform door = null;
  [SerializeField] RoomSpawner roomSpawner = null;

  [Header("Audio")]
  [SerializeField] AudioClip openSFX = null;

  [Header("Settings")]
  [SerializeField] LayerMask layersToDetect = 0;
  [Range(1, 10)]
  [SerializeField] float speed = 5;
  [Range(.0f, 4.0f)]
  [SerializeField] float delay = 1;

  [SerializeField] float openZRotation = 0.0f;


  private bool isOpened = false;

  private AudioSource source = null;
  public AudioSource Source
  {
    get
    {
      if (source == null)
      {
        source = GetComponent<AudioSource>();
        if (source == null)
        {
          source = gameObject.AddComponent<AudioSource>();
        }
      }

      return source;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if(!isOpened && (roomSpawner == null || (roomSpawner != null && roomSpawner.isRoomCompleted())))
    {
      if (((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

      StartAnimating();
      isOpened = true;
    }
  }

  private void StartAnimating()
  {
    PlaySound();

    //float newZRotation = door.localRotation.z;
    //newZRotation = Mathf.Lerp(newZRotation, openZRotation, 5 * Time.deltaTime);

    Vector3 rotationVector = new Vector3(door.localRotation.x, door.localRotation.y, 110);
    door.localRotation = door.localRotation * Quaternion.Euler(rotationVector);
  }

  private void PlaySound()
  {
    Source.clip = openSFX;
    Source.Play();
  }
}
