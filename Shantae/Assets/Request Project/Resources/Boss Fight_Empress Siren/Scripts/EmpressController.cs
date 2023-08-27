using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Empress Siren�� �й� ������ �ٷ�� ��ũ��Ʈ. "PlayerAttack" �±׿� �浹�ϸ� 
/// Empress Siren�� HP�� ����.
/// </summary>

public class EmpressController : MonoBehaviour
{

    public Transform targetTransform; // �ٸ� ������Ʈ�� Transform
    private string player = "Player";
    private GameObject playerTransform;

    private bool playerControllerOff = false;

    #region Empress Siren�� �ǰ�, �й� ���� Ȯ�� ����
    // Empress Siren�� HP
    public static float empressHP = default;
    // �÷��̾� ������ ���� (Empress �ǰ� ����)
    private float getDamage = default;
    // �ִϸ�����
    private Animator animator;
    // Empress Siren�� �ڷ���Ʈ 
    private GameObject teleportAnimator = default;
    private GameObject teleportParticle = default;
    // �ǰ� �� ���� ��ü�� ����
    private Color originColor = default;
    private Color transparentColor = default;
    // �÷��̾� ������
    private Vector2 playerPosition = default;

    private bool alreadyRun = false;
    #endregion

    private void Start()
    {
        playerTransform = GameObject.Find(player);
        // ���� Ǯ�� ���ư��ٸ� �ش� ��ũ��Ʈ�� ������� �ʵ��� �ϱ�. 
        targetTransform = playerTransform.transform;

        // Empress Siren ���� ������Ʈ�� !null���� Ȯ��
        Debug.Assert(this.gameObject != null);

        animator = GetComponent<Animator>();
        teleportAnimator = transform.GetChild(0).gameObject;
        teleportParticle = transform.GetChild(1).gameObject;

        empressHP = 10f;

        originColor = transform.GetComponent<SpriteRenderer>().color;
        transparentColor = originColor;
        transparentColor.a = 0.5f;
    }

    private void Update()
    {
        // Empress Siren�� �й� Ȯ��
        if (empressHP <= 0 && alreadyRun == false)
        {
            alreadyRun = true;

            // EmpressMoving ���� �޼��� �߰�
            transform.GetComponent<EmpressMoving>().enabled = false;
            transform.GetComponent<EmpressMoving>().StopAllCoroutines();

            transform.GetComponent<CeilingAttack>().enabled = false;
            transform.GetComponent<CeilingAttack>().StopAllCoroutines();

            transform.GetComponent<BlowKissAttack>().enabled = false;
            transform.GetComponent<BlowKissAttack>().StopAllCoroutines();

            transform.GetComponent<SurfAttack>().enabled = false;
            transform.GetComponent<SurfAttack>().StopAllCoroutines();

            transform.GetComponent<HopBackAttack>().enabled = false;
            transform.GetComponent<HopBackAttack>().StopAllCoroutines();

            // �ǰ� ���� ���� ���
            transform.GetComponent<SpriteRenderer>().color = originColor;

            StartCoroutine(PlayerWin());
        }

        /// <problem> �� ���ۺ��� true?
        Debug.Log(transform.GetComponent<SpriteRenderer>().flipX);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Empress Siren�� �ǰ� Ȯ��
        if (collision.CompareTag("PlayerAttack") && empressHP > 0)
        {
            // �÷��̾ ���� ������
            getDamage = 1;
            // Empress HP ���. 
            empressHP -= getDamage;

            StartCoroutine(FlashEmpress());
        }
    }

    private IEnumerator FlashEmpress()
    {
        float blinkTime = 1;
        float nowTime = 0;

        while (nowTime < blinkTime)
        {
            nowTime += Time.deltaTime * 30;

            transform.GetComponent<SpriteRenderer>().color = transparentColor;
            yield return new WaitForSeconds(0.1f);

            transform.GetComponent<SpriteRenderer>().color = originColor;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("���� Ż��");

        transform.GetComponent<SpriteRenderer>().color = originColor;

        StopCoroutine(FlashEmpress());
    }

    private IEnumerator PlayerWin()
    {
        GameObject.FindWithTag("Player").transform.GetChild(0).
            transform.GetComponent<PlayerExit>().enabled = true;

        yield return new WaitForSeconds(1f);

        playerPosition = playerTransform.transform.position;

        // �й� �� �÷��̾ Empress Siren�� ���ʿ� ��ġ (Empress Siren�� �ü�)
        if (playerPosition.x < 0)
        {
            Debug.Log("���� ����");
            transform.position = new Vector2(4.89f, -1.44f);

            transform.GetComponent<SpriteRenderer>().flipX = true;

            animator.SetTrigger("Empress Lose");

            playerControllerOff = true;
        }
        else if (playerPosition.x >= 0) // Empress Siren�� �����ʿ� ��ġ
        {
            Debug.Log("������ ����");
            transform.position = new Vector2(-4.89f, -1.44f);

            transform.GetComponent<SpriteRenderer>().flipX = false;

            animator.SetTrigger("Empress Lose");

            playerControllerOff = true;
        }

        yield return new WaitForSeconds(3.5f);

        // Empress Siren�� ������� ȿ��
        teleportAnimator.SetActive(true);
        transform.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds
            (teleportAnimator.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        teleportAnimator.SetActive(false);

        Debug.Log(targetTransform.GetComponent<SpriteRenderer>().flipX);

        /// <problem> �÷��̾� ��������Ʈ�� ������ �������� ���ϴ� ���� 
        SpriteRenderer forFlipX = GameObject.Find(player).transform.GetComponent<SpriteRenderer>();

        // �÷��̾��� �̵��� �����ϱ� �� ��������Ʈ ���� ����
        if (forFlipX.flipX == true) // ������ ���� �ִٸ� �������� ������
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (forFlipX.flipX == false) // �������� ���� �ִ��� �������� ������
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }

        // �÷��̾��� �̵� ����
        targetTransform.GetComponent<PlayerController>().enabled = false;

        yield return new WaitForSeconds(3f);

        CameraShake.instance.StartCoroutine(CameraShake.instance.OpenTheDoor());

        StopCoroutine(PlayerWin());

        // �ڱ��ڽ�(Empress Siren ����)
        gameObject.SetActive(false);
    }
}
