using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Orb_elec : MonoBehaviour
{
    public float delay; // 실행 딜레이 (초)
    private float duration = 1.0f; // 변경되는 시간 (초)
    private Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Color targetColor = new Color(1f, 1f, 1f, 0f); // 투명도

    private Vector3 initialScale;
    private Color initialColor;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private IEnumerator Start()
    {
        initialScale = transform.localScale;
        initialColor = GetComponent<SpriteRenderer>().color;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Set the initial transparency
        Color transparentColor = originalColor;
        transparentColor.a = 0.0f; // 0.0f means fully transparent
        spriteRenderer.color = transparentColor;
        yield return new WaitForSeconds(delay);
        // Start the coroutine to restore the original color
        StartCoroutine(RestoreOriginalColorAfterDelay(3.0f));
        while (true)
        {
            // Shrink and fade out
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                spriteRenderer.color = Color.Lerp(initialColor, targetColor, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure final state
            transform.localScale = targetScale;
            spriteRenderer.color = targetColor;

            // 초기화
            transform.localScale = initialScale;
            spriteRenderer.color = initialColor;

        }
    }
    private IEnumerator RestoreOriginalColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restore the original color
        spriteRenderer.color = originalColor;
    }
}
