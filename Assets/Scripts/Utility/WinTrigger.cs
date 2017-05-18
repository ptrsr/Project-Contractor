using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour {
    private bool _open = false;
    
    private Transform _top;

    private void Start()
    {
        _top = GetComponentInChildren<Transform>();

    }
    
    private void OnTriggerStay(Collider other)
    {
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos = Camera.main.ScreenToWorldPoint(pos);
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector3.Distance(pos, transform.parent.position) < 20)
            {
                Debug.Log("tap");
                StartCoroutine(Delay());
            }
        }
        if (other.gameObject.CompareTag("Player") && _open)
        {
            WinText winText = FindObjectOfType<WinText>();
            winText.GetComponent<Canvas>().enabled = true;
        }
    }
    
    IEnumerator Delay()
    {
        _top.Translate(0, 1f, 0);
        //Start animation for opening here
        //and wait for seconds (however long the animation is)
        yield return new WaitForSeconds(1);
        _open = true;
    }
}
