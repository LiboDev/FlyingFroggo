using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private bool randomPos;
    [SerializeField] private float randomPosScale;
    [SerializeField] private int amount;

    [SerializeField] private List<Vector3> spawnPos;
    [SerializeField] private List<GameObject> objects;

    private Vector3 pos;
    private List<Vector3> posList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        if (randomPos)
        {
            if (amount > (1 + 8 / (int)randomPosScale))
            {
                Debug.LogError("amount of objects spawned per chunk is too high for " + gameObject.name);
                enabled = false;
            }

            int rangeX = 8 / (int)randomPosScale;
            int rangeY = 4 / (int)randomPosScale;

            for (int i = 0; i < amount; i++)
            {
                pos = new Vector3(Random.Range(-rangeX, rangeX+1) * randomPosScale, Random.Range(-rangeY, rangeY+1) * randomPosScale, 0);
                while (Overlap())
                {
                    pos = new Vector3(Random.Range(-rangeX, rangeX+1) * randomPosScale, Random.Range(-rangeY, rangeY + 1) * randomPosScale, 0);
                }
                posList.Add(pos);

                GameObject obj = Instantiate(objects[Random.Range(0, objects.Count)], pos + transform.position, Quaternion.identity);
                obj.transform.parent = transform;
            }
        }
        else
        {
            var log = false;
            GameObject obj;

            for(int i = 0; i < spawnPos.Count; i++)
            {
                var random = Random.Range(0, objects.Count);

                if(random == 1)
                {
                    log = true;   
                }

                if(i == spawnPos.Count - 1 && log == false)
                {
                    obj = Instantiate(objects[1], spawnPos[i] + transform.position, Quaternion.identity);
                }
                else
                {
                    obj = Instantiate(objects[random], spawnPos[i] + transform.position, Quaternion.identity);
                }

                obj.transform.parent = transform;
            }
        }

    }

    private bool Overlap()
    {
        for (int j = 0; j < posList.Count; j++)
        {
            if (pos.x == posList[j].x || pos.y == posList[j].y)
            {
                return true;
            }
        }
        return false;
    }
}
