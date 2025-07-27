using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterLoader : MonoBehaviour
{
    //scene
    [SerializeField] private GameObject characterListObject;
    [SerializeField] private GameObject characterListObjectPrefab;
    [SerializeField] private ScrollSnapping characterScroller;

    //tracking
    [SerializeField] private int selectedCharacterIndex = 0;

    //presets
    [SerializeField] private List<Characters> characters;

    //inventory
    private int coins;

    [SerializeField] private List<int> unlockedCharacters = new List<int>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //load unlocked characters

        string num = PlayerPrefs.GetString("UnlockedCharacters", "0");

        Debug.Log("unlocked character string:" + num);

        for (int i = 0; i < num.Length; i++)
        {
            unlockedCharacters.Add(num[i] - '0');

            Debug.Log("unlocked character index:" + num[i]);
        }

        //add items to scrollable list
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            for(int i = 0; i < unlockedCharacters.Count; i++)
            {
                GameObject obj = Instantiate(characterListObjectPrefab, transform.position, Quaternion.identity);
                obj.GetComponent<Image>().sprite = characters[unlockedCharacters[i]].characterSprite;
                obj.transform.SetParent(characterListObject.transform, false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.SetCharacter(characters[selectedCharacterIndex]);
            Debug.Log(selectedCharacterIndex);
        }
    }

    public void UpdateIndex()
    {
        selectedCharacterIndex = characterScroller.closestButtonIndex;
        Debug.Log(selectedCharacterIndex);
    }
}
