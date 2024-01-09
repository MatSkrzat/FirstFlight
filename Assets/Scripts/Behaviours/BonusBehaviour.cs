using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{
    public void RemoveBonusGameObject()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
