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

    [SerializeField] private List<int> unlockedCharacters = new List<int>();

    private void Awake()
    {
        //add items to scrollable list
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            DontDestroyOnLoad(gameObject);

            //load unlocked characters

            string num = PlayerPrefs.GetString("UnlockedCharacters", "01234567");

            for (int i = 0; i < num.Length; i++)
            {
                unlockedCharacters.Add(num[i] - '0');
            }

            for (int i = 0; i < unlockedCharacters.Count; i++)
            {
                GameObject obj = Instantiate(characterListObjectPrefab, transform.position, Quaternion.identity);
                obj.GetComponent<Image>().sprite = characters[unlockedCharacters[i]].characterSprite;
                obj.transform.SetParent(characterListObject.transform, false);
            }
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        if(next.buildIndex != 0)
        {
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.SetCharacter(characters[unlockedCharacters[selectedCharacterIndex]]);
            Debug.Log("set character:" + unlockedCharacters[selectedCharacterIndex]);
        }
    }

    public void UpdateIndex()
    {
        selectedCharacterIndex = characterScroller.closestButtonIndex;
        Debug.Log("closest index:" + selectedCharacterIndex);
    }
}
