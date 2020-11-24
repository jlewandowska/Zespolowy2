using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
  public enum SlidingDoorState { None, Open, Close }

  [Header("References")]
  [SerializeField] Transform door = null;

  [Header("Settings")]
  [SerializeField] LayerMask layersToDetect = 0;
  [Range(1, 10)]
  [SerializeField] float speed = 5;
  [Range(.0f, 4.0f)]
  [SerializeField] float delay = 1;

  [SerializeField] float closeZPos = 0.0f;
  [SerializeField] float openZPos = 0.0f;

  [Header("Audio")]
  [SerializeField] AudioClip openSFX = null;
  [SerializeField] AudioClip closeSFX = null;

  #region Private
  private bool animating = false;
  private SlidingDoorState animatingState = SlidingDoorState.None;
  private SlidingDoorState state = SlidingDoorState.None;

  private List<Transform> inRange = new List<Transform>();

  private AudioSource source = null;
  public AudioSource Source
  {
    get
    {
      if(source == null)
      {
        source = GetComponent<AudioSource>();
        if(source == null)
        {
          source = gameObject.AddComponent<AudioSource>();
        }
      }

      return source;
    }
  }

  IEnumerator IE_StartAnimating = null, IE_Animate = null, IE_Door = null;
  #endregion

  private void Start()
  {
    closeZPos = Mathf.Abs(door.transform.localPosition.z);
  }

  private void OnTriggerEnter(Collider other)
  {
    if(((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

    inRange.Add(other.transform);

    state = SlidingDoorState.Open;
    StartAnimating();
  }

  private void OnTriggerExit(Collider other)
  {
    if (((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

    inRange.Remove(other.transform);

    if(inRange.Count <= 0)
    { state = SlidingDoorState.Close; StartAnimating(); }
  }

  void StartAnimating()
  {
    if(IE_StartAnimating != null)
    { StopCoroutine(IE_StartAnimating); }

    IE_StartAnimating = Begin();
    StartCoroutine(IE_StartAnimating);
  }

  private IEnumerator Begin()
  {
    while (animating) { yield return null; }

    if (IE_Animate != null) { StopCoroutine(IE_Animate); }
    IE_Animate = Animate(state.Equals(SlidingDoorState.Open) ? openZPos : closeZPos);

    StartCoroutine(IE_Animate);
  }

  private IEnumerator Animate(float zPos)
  {
    if (Utility.Approximately(door.transform.localPosition.z, zPos, .001f)) { yield break; }

    animatingState = state;

    yield return new WaitForSeconds(delay);

    if (IE_Door != null) { StopCoroutine(IE_Door); }

    IE_Door = Move(zPos);

    PlaySound(state.Equals(SlidingDoorState.Open) ? openSFX : closeSFX);

    StartCoroutine(IE_Door);


    while(animating)
    {
      yield return null;
    }
  }

  private void PlaySound(AudioClip clip)
  {
    Source.clip = clip;
    Source.Play();
  }

  IEnumerator Move(float zPos)
  {
    animating = true;

    while(!Utility.Approximately(door.localPosition.z, zPos, 0.001f))
    {
      float newZPos = door.localPosition.z;
      newZPos = Mathf.Lerp(newZPos, zPos, speed * Time.deltaTime);

      door.localPosition = new Vector3(door.localPosition.x, door.localPosition.y, newZPos);

      yield return null;
    }
    animating = false;
  }

}
