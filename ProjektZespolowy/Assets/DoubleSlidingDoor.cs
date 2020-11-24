using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlidingDoor : MonoBehaviour
{
  public enum SlidingDoorState { None, Open, Close }

  [Header("References")]
  [SerializeField] Transform rightDoor = null;
  [SerializeField] Transform leftDoor = null;

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

  IEnumerator IE_StartAnimating = null, IE_Animate = null, IE_RightDoor = null, IE_LeftDoor = null;
  #endregion

  private void Start()
  {
    closeZPos = Mathf.Abs(leftDoor.transform.localPosition.z);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

    inRange.Add(other.transform);

    state = SlidingDoorState.Open;
    StartAnimating();
  }

  private void OnTriggerExit(Collider other)
  {
    if (((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

    inRange.Remove(other.transform);

    if (inRange.Count <= 0)
    { state = SlidingDoorState.Close; StartAnimating(); }
  }

  void StartAnimating()
  {
    if (IE_StartAnimating != null)
    { StopCoroutine(IE_StartAnimating); }

    IE_StartAnimating = Begin();
    StartCoroutine(IE_StartAnimating);
  }

  private IEnumerator Begin()
  {
    while (animating) { yield return null; }

    if (IE_Animate != null) { StopCoroutine(IE_Animate); }
    IE_Animate = Animate(state.Equals(SlidingDoorState.Open) ? openZPos : -closeZPos);

    StartCoroutine(IE_Animate);
  }

  private IEnumerator Animate(float zPos)
  {
    if (Utility.Approximately(leftDoor.transform.localPosition.z, zPos, .001f)) { yield break; }

    animatingState = state;

    yield return new WaitForSeconds(state.Equals(SlidingDoorState.Close) ? delay / 2 : delay);

    if (IE_RightDoor != null) { StopCoroutine(IE_RightDoor); }
    if (IE_LeftDoor != null) { StopCoroutine(IE_LeftDoor); }

    IE_RightDoor = Move(rightDoor, -zPos);
    IE_LeftDoor = Move(leftDoor, zPos);

    PlaySound(state.Equals(SlidingDoorState.Open) ? openSFX : closeSFX);

    StartCoroutine(IE_RightDoor);
    StartCoroutine(IE_LeftDoor);


    while (animating)
    {
      yield return null;
    }
  }

  private void PlaySound(AudioClip clip)
  {
    Source.clip = clip;
    Source.Play();
  }

  IEnumerator Move(Transform transform, float zPos)
  {
    animating = true;

    while (!Utility.Approximately(transform.localPosition.z, zPos, 0.001f))
    {
      float newZPos = transform.localPosition.z;
      newZPos = Mathf.Lerp(newZPos, zPos, speed * Time.deltaTime);
      transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZPos);
      yield return null;
    }
    animating = false;
  }

}
