using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDown : MonoBehaviour
{
    private RectTransform originPosition = default;
    private Vector2 destination = new Vector2(0, 95);
    private float moveSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //95까지 내려오기
        originPosition.anchoredPosition = Vector2.MoveTowards
            (originPosition.anchoredPosition, destination, Time.deltaTime * moveSpeed);
    }
}
