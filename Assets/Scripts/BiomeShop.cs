using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BiomeShop : MonoBehaviour
{
    [SerializeField] private UnlockedBiomesLoader unlockedBiomesLoader;

    [SerializeField] private Transform biomeListObject;
    [SerializeField] private GameObject biomeListObjectPrefab;
    [SerializeField] private ScrollSnapping biomeScroller;

    private List<Sprite> biomes;

    [SerializeField] private List<int> lockedBiomes = new List<int>();

    [SerializeField] private MoneyDisplayer moneyDisplayer;

    // Start is called before the first frame update
    void Awake()
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

        for (int i = 0; i < biomes.Count; i++)
        {
            GameObject obj = Instantiate(biomeListObjectPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Image>().sprite = biomes[i];
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i*100).ToString("N0");;
            obj.transform.SetParent(biomeListObject, false);

            if (lockedBiomes.Contains(i))
            {
                obj.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
            }
        }

        /*        //remove items from scrollable list
                List<Transform> toBeDestroyed = new List<Transform>();
                for (int i = 0; i < lockedBiomes.Count; i++)
                {
                    Transform obj = biomeListObject.transform.GetChild(i);
                    toBeDestroyed.Add(obj);
                }

                foreach(Transform trans in toBeDestroyed)
                {
                    Destroy(trans);
                }*/

    }
    public void PurchaseBiome()
    {
        PlaySound.instance.PlaySFX("Purchase", 1f);

        int index = biomeScroller.closestButtonIndex;

        int money = PlayerPrefs.GetInt("Money", 0);
        int cost = index * 100;

        if (lockedBiomes.Contains(index) && cost <= money)
        {
            string oldUnlockedBiomes = PlayerPrefs.GetString("UnlockedBiomes", "0");
            PlayerPrefs.SetString("UnlockedBiomes", oldUnlockedBiomes + index);

            money -= cost;

            PlayerPrefs.SetInt("Money", money);

            biomeListObject.GetChild(index).GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
            lockedBiomes.Remove(index);

            moneyDisplayer.UpdateMoney();
        }
    }
}
