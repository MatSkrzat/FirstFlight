using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public void RemoveCoin()
    {
        gameObject.SetActive(false);
    }
}
