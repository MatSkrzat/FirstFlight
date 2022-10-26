using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public void RemoveCoin()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
