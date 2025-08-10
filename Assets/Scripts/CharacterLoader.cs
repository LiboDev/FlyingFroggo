using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLoader : MonoBehaviour
{
    public Characters character;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        if (next.buildIndex != 0)
        {
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.SetCharacter(character);

            Debug.Log("set character:" + character);
        }
    }
}
