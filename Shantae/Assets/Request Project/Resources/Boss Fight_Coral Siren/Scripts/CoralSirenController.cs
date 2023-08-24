using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSirenController : MonoBehaviour
{
    // 피격 시 색상 교체를 위함
    private Color originColor = default;
    private Color transparentColor = default;

    // Start is called before the first frame update
    void Start()
    {
        originColor = transform.GetComponent<SpriteRenderer>().color;
        transparentColor = originColor;
        transparentColor.a = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (HitController.coralDamaged == true)
        {
            StartCoroutine(FlashCoral());
        }
    }

    private IEnumerator FlashCoral()
    {
        float blinkTime = 1;
        float nowTime = 0;

        while (nowTime < blinkTime)
        {
            Debug.Log("반짝반짝");

            nowTime += Time.deltaTime * 30;

            transform.GetComponent<SpriteRenderer>().color = transparentColor;
            yield return new WaitForSeconds(0.1f);

            transform.GetComponent<SpriteRenderer>().color = originColor;
            yield return new WaitForSeconds(0.1f);
        }

        transform.GetComponent<SpriteRenderer>().color = originColor;
        HitController.coralDamaged = false;

        StopCoroutine(FlashCoral());
    }
}
