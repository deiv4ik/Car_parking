using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMap : MonoBehaviour
{
    public GameObject city, cyber;

    void Start()
    {
        if (PlayerPrefs.GetString("NowMap") == "Cyber")
        {
            cyber.SetActive(true);
        }
        else
        {
            city.SetActive(true);
        }
    }
}
