using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnapping : MonoBehaviour
{
    public float snapStrength = 1;
    public float snapThreshhold = 1;

    public RectTransform panel;
    public RectTransform[] items;
    public RectTransform center;

    public ScrollRect scrollRect;

    private float[] distances;
    private bool dragging = false;
    private int padding = 150;
    public int closestButtonIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //add transforms in scrollable list to an array
        items = new RectTransform[panel.childCount];

        for(int i = 0; i < panel.childCount; i++)
        {
            items[i] = panel.GetChild(i).GetComponent<RectTransform>();
        }

        distances = new float[items.Length];

        //calculate distance between items in list
        padding = (int)panel.GetComponent<HorizontalLayoutGroup>().spacing;
    }

    // Update is called once per frame
    void Update()
    {
        dragging = Input.GetMouseButton(0);

        if (dragging == false)
        {
            float minDistance = 10;

            //find button closest to the center
            for (int i = 0; i < items.Length; i++)
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

        

    }
}
