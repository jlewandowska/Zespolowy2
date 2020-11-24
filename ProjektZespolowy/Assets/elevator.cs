using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
  public enum ElevatorState { None, Up, Down }

  [Header("References")]
  [SerializeField] Transform movePlatform = null;

  [Header("Settings")]
  [SerializeField] LayerMask layersToDetect = 0;
  [Range(1, 10)]
  [SerializeField] float speed = 5;
  [Range(.0f, 4.0f)]
  [SerializeField] float delay = 1;

  [SerializeField] float downYPos = 0.0f;
  [SerializeField] float upYPos = 0.0f;

  [Header("Audio")]
  [SerializeField] AudioClip upSFX = null;
  [SerializeField] AudioClip downSFX = null;

  #region Private
  private bool animating = false;
  private ElevatorState animatingState = ElevatorState.None;
  private ElevatorState state = ElevatorState.None;

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

  IEnumerator IE_StartAnimating = null, IE_Animate = null, IE_Elevator = null;
  #endregion

  private void Start()
  {
    downYPos = Mathf.Abs(movePlatform.transform.localPosition.y);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

    inRange.Add(other.transform);

    state = ElevatorState.Up;
    StartAnimating();
  }

  private void OnTriggerExit(Collider other)
  {
    if (((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

    inRange.Remove(other.transform);

    if (inRange.Count <= 0)
    { state = ElevatorState.Down; StartAnimating(); }
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
    IE_Animate = Animate(state.Equals(ElevatorState.Up) ? upYPos : downYPos);

    StartCoroutine(IE_Animate);
  }

  private IEnumerator Animate(float yPos)
  {
    if (Utility.Approximately(movePlatform.transform.localPosition.y, yPos, .001f)) { yield break; }

    animatingState = state;

    yield return new WaitForSeconds(delay);

    if (IE_Elevator != null) { StopCoroutine(IE_Elevator); }

    IE_Elevator = Move(yPos);

    PlaySound(state.Equals(ElevatorState.Up) ? upSFX : downSFX);

    StartCoroutine(IE_Elevator);


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

  IEnumerator Move(float yPos)
  {
    animating = true;

    while (!Utility.Approximately(movePlatform.localPosition.y, yPos, 0.001f))
    {
      float newYPos = movePlatform.localPosition.y;
      newYPos = Mathf.Lerp(newYPos, yPos, speed * Time.deltaTime);

      if (inRange.Count > 0)
      {
        inRange[0].position = movePlatform.transform.up + new Vector3(inRange[0].localPosition.x, newYPos, inRange[0].localPosition.z);
      }

      movePlatform.localPosition = new Vector3(movePlatform.localPosition.x, newYPos, movePlatform.localPosition.z);

      yield return null;
    }
    animating = false;
  }

}
