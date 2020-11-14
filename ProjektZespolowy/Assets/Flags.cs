using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{
  Renderer m_renderer;
  GameObject player;
  string visibilityStatus;

  void Start()
  {
    m_renderer = GetComponent<Renderer>();
    player = GameObject.Find("Player");
  }

  void FixedUpdate()
  {
    if(m_renderer.isVisible)
    {
      if (Physics.Linecast(gameObject.transform.position, player.transform.position))
        visibilityStatus = "Niewidoczne";
      else
        visibilityStatus = "Widoczne";
    }
    else
      visibilityStatus = "Niewidoczne";
  }

  void OnGUI()
  {
    GUI.Label(new Rect(10, 10, 1920, 20), visibilityStatus);
  }
}
