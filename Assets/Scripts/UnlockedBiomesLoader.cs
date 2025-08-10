using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnlockedBiomesLoader : MonoBehaviour
{
    [SerializeField] private GameObject biomeListObject;
    [SerializeField] private GameObject biomeListObjectPrefab;
    [SerializeField] private ScrollSnapping biomeScroller;

    public List<Sprite> biomes;

    [SerializeField] private List<int> unlockedBiomes = new List<int>();

    // Start is called before the first frame update
    void Awake()
    {
        //set unlocked biomes

        PlayerPrefs.SetString("UnlockedBiomes", "0");

        string num = PlayerPrefs.GetString("UnlockedBiomes", "0");

        for (int i = 0; i < num.Length; i++)
        {
            unlockedBiomes.Add(num[i] - '0');
        }

        unlockedBiomes.Sort();

        //add items to scrollable list
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < unlockedBiomes.Count; i++)
            {
                GameObject obj = Instantiate(biomeListObjectPrefab, transform.position, Quaternion.identity);
                obj.GetComponent<Image>().sprite = biomes[unlockedBiomes[i]];
                obj.transform.SetParent(biomeListObject.transform, false);
            }
        }
    }
}
