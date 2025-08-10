using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterShop : MonoBehaviour
{
    [SerializeField] private CharacterSelect unlockedCharactersLoader;

    [SerializeField] private Transform characterListObject;
    [SerializeField] private GameObject characterListObjectPrefab;
    [SerializeField] private ScrollSnapping characterScroller;

    private List<Characters> characters;

    [SerializeField] private List<int> lockedCharacters = new List<int>();

    [SerializeField] private MoneyDisplayer moneyDisplayer;

    [SerializeField] private TextMeshProUGUI tmpro;

    // Start is called before the first frame update
    void Awake()
    {
        characters = unlockedCharactersLoader.characters;

        for (int i = 0; i < characters.Count; i++)
        {
            lockedCharacters.Add(i);
        }

        string num = PlayerPrefs.GetString("UnlockedCharacters", "0");

        for (int i = 0; i < num.Length; i++)
        {
            lockedCharacters.Remove(num[i] - '0');
        }

        for(int i = 0; i < characters.Count; i++)
        {
            GameObject obj = Instantiate(characterListObjectPrefab, transform.position, Quaternion.identity);
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = characters[i].cost.ToString("N0");
            obj.transform.SetParent(characterListObject, false);
            obj.GetComponent<Image>().sprite = characters[i].characterSprite;

            if (lockedCharacters.Contains(i))
            {
                obj.GetComponent<Image>().color = new Color(0.25f,0.25f,0.25f,1);
            }
        }


/*        //remove items from scrollable list
        List<Transform> toBeDestroyed = new List<Transform>();
        for (int i = 0; i < lockedCharacters.Count; i++)
        {
            Transform obj = characterListObject.transform.GetChild(i);
            toBeDestroyed.Add(obj);
        }

        foreach (Transform trans in toBeDestroyed)
        {
            Destroy(trans);
        }*/

    }

    private void Update()
    {
        tmpro.text = characters[characterScroller.closestButtonIndex].description;
    }

    public void PurchaseCharacter()
    {
        PlaySound.instance.PlaySFX("Purchase", 1f);

        int index = characterScroller.closestButtonIndex;

        int money = PlayerPrefs.GetInt("Money", 0);
        int cost = characters[index].cost;

        if (lockedCharacters.Contains(index) && cost <= money)
        {
            string oldUnlockedCharacters = PlayerPrefs.GetString("UnlockedCharacters", "0");
            PlayerPrefs.SetString("UnlockedCharacters", oldUnlockedCharacters + index);

            money -= cost;

            PlayerPrefs.SetInt("Money", money);

            characterListObject.GetChild(index).GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
            lockedCharacters.Remove(index);

            moneyDisplayer.UpdateMoney();
        }
    }

    public void ChangeDescription()
    {
        tmpro.text = characters[characterScroller.closestButtonIndex].description;
    }
}