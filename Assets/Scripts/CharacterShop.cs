using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterShop : MonoBehaviour
{
    //tracking
    [SerializeField] private int selectedCharacterIndex = 0;

    //presets
    [SerializeField] private List<Characters> characters;

    //inventory
    private int coins;

    private List<int> unlockedCharacters;



    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.SetCharacter(characters[selectedCharacterIndex]);
        }
    }
}
