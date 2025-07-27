using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BiomeShop : MonoBehaviour
{
    [SerializeField] private UnlockedBiomesLoader unlockedBiomesLoader;

    [SerializeField] private GameObject biomeListObject;
    [SerializeField] private GameObject biomeListObjectPrefab;
    [SerializeField] private ScrollSnapping biomeScroller;

    private List<Sprite> biomes;

    [SerializeField] private List<int> lockedBiomes = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        biomes = unlockedBiomesLoader.biomes;

        for(int i = 0; i< biomes.Count; i++)
        {
            lockedBiomes.Add(i);
        }

        string num = PlayerPrefs.GetString("UnlockedBiomes", "0");

        for (int i = 0; i < num.Length; i++)
        {
            lockedBiomes.Remove(num[i] - '0');
        }

        //remove items from scrollable list
        List<Transform> toBeDestroyed = new List<Transform>();
        for (int i = 0; i < lockedBiomes.Count; i++)
        {
            Transform obj = biomeListObject.transform.GetChild(i);
            toBeDestroyed.Add(obj);
        }

        foreach(Transform trans in toBeDestroyed)
        {
            Destroy(trans);
        }

    }
    public void PurchaseBiome(int index)
    {
        string oldUnlockedBiomes = PlayerPrefs.GetString("UnlockedBiomes", "0");
        PlayerPrefs.SetString("UnlockedBiomes", oldUnlockedBiomes + index);
    }
}
