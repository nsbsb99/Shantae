using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject LeftD;
    public GameObject RightD;

    private float moveSpeed = 1;
    private bool isMoving = false; 
    private float timer = 0.0f;
    private bool triggerA = false;
    public bool isEmpress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timer);
        if (triggerA)
        {
            timer += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) && isMoving)
        {
            triggerA = true;
            if (timer < 5f)
            {
                // LeftD와 RightD 게임 오브젝트를 왼쪽과 오른쪽으로 이동시킵니다.
                LeftD.GetComponent<Rigidbody2D>().velocity = Vector2.left * moveSpeed;
                RightD.GetComponent<Rigidbody2D>().velocity = Vector2.right * moveSpeed;
            }
        }
        if (timer >= 5f)
        {
            LeftD.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            RightD.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (isEmpress)
            {
                BGMManager.playing = false;
                SceneManager.LoadScene("Boss Fight Mega_Empress Siren");
            }
            else
            {
                BGMManager.playing = false;
                SceneManager.LoadScene("Boss Fight_Coral Siren");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            isMoving = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            isMoving = false;
        }
    }
}
