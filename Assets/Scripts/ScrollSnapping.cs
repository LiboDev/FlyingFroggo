using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScrollSnapping : MonoBehaviour
{
    public float snapStrength = 1;
    public float snapThreshhold = 1;

    public RectTransform panel;
    public List<RectTransform> items;
    public RectTransform center;

    public ScrollRect scrollRect;

    private float[] distances;
    private bool dragging = false;
    private int padding = 150;
    public int closestButtonIndex = 0;
    private int previousButtonIndex = 0;

    public UnityEvent changeIndex = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        if (panel.childCount == 0)
        {
            Debug.Log("no items in scrollable list");
            Destroy(gameObject);
        }

        //add transforms in scrollable list to an array
        items = new List<RectTransform>();

        for(int i = 0; i < panel.childCount; i++)
        {
            items.Add(panel.GetChild(i).GetComponent<RectTransform>());
        }

        distances = new float[items.Count];

        //calculate distance between items in list
        padding = (int)panel.GetComponent<HorizontalLayoutGroup>().spacing;

        changeIndex.AddListener(OnIndexChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.childCount == 0)
        {
            Debug.Log("no items in scrollable list");
            Destroy(gameObject);
        }

        dragging = Input.GetMouseButton(0);

        if (dragging == false)
        {
            float minDistance = 10;

            //find button closest to the center
            for (int i = 0; i < items.Count; i++)
            {
                float distance = Mathf.Abs(center.transform.position.x - items[i].transform.position.x);
                distances[i] = distance;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestButtonIndex = i;
                }
            }

            /*            if(scrollRect.velocity.magnitude < snapThreshhold)
                        {*/
/*            if(minDistance == 0)
            {

            }
            else if (minDistance < 0.01f)
            {
                scrollRect.horizontalNormalizedPosition += center.transform.position.x - closestButtonIndex.transform.position.x;
            }*/
            if (minDistance < 0.1f)
            {
                scrollRect.velocity *= 0.9f;
            }
            else
            {
                float direction = center.transform.position.x - items[closestButtonIndex].transform.position.x;
                scrollRect.velocity += Vector2.right * direction * snapStrength;
            }
            //}

        }

        if(previousButtonIndex != closestButtonIndex)
        {
            changeIndex.Invoke();
        }

        previousButtonIndex = closestButtonIndex;
    }

    private void OnIndexChange()
    {
        PlaySound.instance.PlaySFX("ScrollSnap", 1f, 0.01f);
    }
}
