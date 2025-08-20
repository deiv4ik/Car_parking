using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Openscene : MonoBehaviour
{
    public void OpenNewScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
