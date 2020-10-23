using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
  public GameObject movePlatform;

  private void OnTriggerStay()
  {
    movePlatform.transform.position += movePlatform.transform.up * 10 * Time.deltaTime;
  }
}
