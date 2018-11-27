using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCutter : MonoBehaviour {
    void OnCollisionEnter(Collision collision)
    {
        var cutable = collision.gameObject.GetComponent<ICutable>();
        if (cutable != null)
        {
            cutable.TakeCut();
        }
    }
}
