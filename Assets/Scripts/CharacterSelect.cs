using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    //scene
    [SerializeField] private GameObject characterListObject;
    [SerializeField] private GameObject characterListObjectPrefab;
    [SerializeField] private ScrollSnapping characterScroller;

    [SerializeField] private GameObject characterLoader;

    //tracking
    [SerializeField] private int selectedCharacterIndex = 0;

    //presets
    [SerializeField] public List<Characters> characters;

    [SerializeField] private List<int> unlockedCharacters = new List<int>();

    [SerializeField] private TextMeshProUGUI tmpro;

    private void Awake()
    {
        LoadUnlockedCharacters();
        ChangeDescription();
    }

    private void LoadUnlockedCharacters()
    {
        //load unlocked characters

        unlockedCharacters.Clear();

        string num = PlayerPrefs.GetString("UnlockedCharacters", "01234567");
        Debug.Log("Unlocked Characters: " + num);

        for (int i = 0; i < num.Length; i++)
        {
            unlockedCharacters.Add(num[i] - '0');
        }

        unlockedCharacters.Sort();

        for (int i = 0; i < unlockedCharacters.Count; i++)
        {
            GameObject obj = Instantiate(characterListObjectPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Image>().sprite = characters[unlockedCharacters[i]].characterSprite;
            obj.transform.SetParent(characterListObject.transform, false);
        }
    }

    public void UpdateIndex()
    {
        selectedCharacterIndex = characterScroller.closestButtonIndex;
        Debug.Log("closest index:" + selectedCharacterIndex);

        GameObject cl = Instantiate(characterLoader, transform.position, Quaternion.identity);
        cl.GetComponent<CharacterLoader>().character = characters[selectedCharacterIndex];
    }

    public void ChangeDescription()
    {
        tmpro.text = characters[characterScroller.closestButtonIndex].description;
    }
}
