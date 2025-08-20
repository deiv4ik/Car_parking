using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectLevel : MonoBehaviour
{

    public Text CountMoves;

    [Serializable]
    public struct Levels
    {
        public GameObject level;
        public int moves;
    }
    public Levels[] levels;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Game Level"))
        {
            PlayerPrefs.SetInt("Game Level", 1);
        }

        if (PlayerPrefs.GetInt("Game Level") >= levels.Length) PlayerPrefs.SetInt("Game Level", levels.Length);

        Levels now = levels[PlayerPrefs.GetInt("Game Levels") - 1];

        now.level.SetActive(true);
        CountMoves.text = now.moves.ToString();
    }
}
