using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //public bool startTimer = false;
    public GameObject gemBreakPrefab;
    private int gemHP = 3;
    private EmpressAnimation[] empressAnimations;

    private float blinkDuration = 0.1f;

    private Renderer parentRenderer;

    private AudioSource audioSource;
    public AudioClip gemHit;

    // Start is called before the first frame update
    void Start()
    {
        empressAnimations = FindObjectsOfType<EmpressAnimation>();

        parentRenderer = transform.parent.GetComponent<Renderer>();

        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gemHP <= 0)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = transform.rotation; 
            foreach (EmpressAnimation empressAnimation in empressAnimations)
            {
                empressAnimation.EmpressDamage();
            }
            // 프리팹을 소환
            Instantiate(gemBreakPrefab, spawnPosition, spawnRotation);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("PlayerAttack"))
        {
            audioSource.clip = gemHit;
            audioSource.Play();
            gemHP -= 1;
            StartBlinkingOnce();
            //Debug.Log(gemHP);
        }

    }
    public void StartBlinkingOnce()
    {
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        
        Color blinkColor = Color.gray;
        Color originalColor = Color.white;
        parentRenderer.material.color = blinkColor;
        yield return new WaitForSeconds(blinkDuration);
        parentRenderer.material.color = originalColor;
    }

   
}
