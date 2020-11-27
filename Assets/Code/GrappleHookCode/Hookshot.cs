using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
    public GameObject grappleHook;
    public GameObject playerCamera;

    public GameObject placeholder;
    public GrappleHook grappleScript;

    public bool isGrappling = false;

    void Start()
    {
        grappleHook.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            Destroy(placeholder);
            placeholder = Instantiate(grappleHook, playerCamera.transform.position, playerCamera.transform.rotation);
            grappleScript = placeholder.GetComponent<GrappleHook>();
            placeholder.SetActive(true);
            Debug.Log("Hook tossed!");
        }
        if (Input.GetKeyDown("v"))
        {
            Destroy(placeholder);
            Debug.Log("Hook intentionally lost!");
        }
        if (placeholder != null)
        {
            isGrappling = grappleScript.hasHit;
        } else
        {
            isGrappling = false;
        }
    }
}
