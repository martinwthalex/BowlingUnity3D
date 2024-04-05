using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pin : MonoBehaviour
{
    #region Variables
    public bool falled = false;
    #endregion

    #region CollisionEnter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball") 
            || collision.gameObject.CompareTag("pin") 
            || collision.gameObject.CompareTag("pin_max_right")
            || collision.gameObject.CompareTag("pin_max_left"))
        {
            if (!falled)
            {
                falled = true;
                GameManager.Instance.PinFalled(falled, this.gameObject);
            }
        }
    }
    #endregion

}
