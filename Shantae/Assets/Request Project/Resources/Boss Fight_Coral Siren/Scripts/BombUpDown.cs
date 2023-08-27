using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ��ź�� �� �Ʒ� �̵��� �����ϰ�, �ִϸ��̼��� �����ϴ� Ŭ����
/// </summary>

public class BombUpDown : MonoBehaviour
{
    private float upSpeed = 10f;
    private float downSpeed = 17.0f;
    private Animator bombAnimator;

    public bool upDone = false;
    public bool falling = false;
    public bool alreadyRun = false;
    public bool backThePool = false;
    public bool allBack = false;

    private Vector2 poolPosition_bomb = new Vector2(0f, 10f);
    private Vector2 originScale;

    private Animator animator;
    private SpriteRenderer sparkRenderer;

    // Start is called before the first frame update
    void Start()
    {
        bombAnimator = GetComponent<Animator>();

        // ��ź Ȯ�� �� ���� ũ��
        originScale = transform.localScale;

        animator = GetComponent<Animator>();

        // ����ũ�� ǥ�õǴ� ������
        sparkRenderer = transform.GetChild(0).
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CoralSirenMoving.fireBomb == true && upDone == false)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

            // ������ ��ź�� ���� �߻�
            transform.Translate(Vector2.up * upSpeed * Time.deltaTime);
        }

        // ��ź�� ���� ��ǥ y�� 7�� �����ϸ� ũ�� Ű���
        if (CoralSirenMoving.fireBomb == true && 
            transform.position.y >= 7f && alreadyRun == false)
        {
            upDone = true;

            transform.localScale = originScale * 1.5f;
            falling = true;
        }

        // �Ʒ��� ������
        if (CoralSirenMoving.fireBomb == true && falling == true)
        {
            transform.Translate(Vector2.down * downSpeed * Time.deltaTime);
        }

        // �ִϸ��̼� ����� ������ �ڽĿ�����Ʈ(0) ������ ���� Ǯ�� ����
        if (CoralSirenMoving.fireBomb == true && backThePool == true)
        {
            // Ǯ�� �ǵ����� ũ�� �������
            transform.position = poolPosition_bomb;
            transform.localScale = originScale;

            allBack = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� �ٴ� �ݶ��̴��� �浹�ϸ�
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Sand"))
        { 
            // ����ũ ������ ����
            sparkRenderer.enabled = false;

            // �߶��� ����
            falling = false;

            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

            // �ִϸ��̼� ���
            transform.GetComponent<Animator>().enabled = true;

            StartCoroutine(BombAnimationTimer());
        }
    }

    IEnumerator BombAnimationTimer()
    {
        // �ִϸ��̼� ��� �ð����� ��ٷȴٰ� Ǯ�� ����
        yield return new WaitForSeconds
               (bombAnimator.GetCurrentAnimatorStateInfo(0).length);
        // �ִϸ��̼� ����
        transform.GetComponent<Animator>().enabled = false;
        // ����ũ ������ ����
        sparkRenderer.enabled = true;

        animator.Rebind();
        

        alreadyRun = true;
        backThePool = true;
    }
}
