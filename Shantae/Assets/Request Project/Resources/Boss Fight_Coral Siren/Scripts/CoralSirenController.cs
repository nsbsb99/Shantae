using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoralSirenController : MonoBehaviour
{
    // �ǰ� �� ���� ��ü�� ����
    private Color originColor = default;
    private Color transparentColor = default;
    // CoralSiren�� HP
    private float coralSirenHP = 13f;
    // Coral Siren_Back
    private GameObject coralSiren_Back;
    // Coral Siren�� ���� �Ŵ���
    private GameObject bossAttackManager;
    // Player
    private GameObject player;
    // Damage �±װ� �޸� ��� ���� ������Ʈ�� ������ ��
    private GameObject[] damageObjects;
    // ���� ���ӿ�����Ʈ
    private GameObject explosionPrefab;
    private GameObject[] explosions;

    private bool already_1 = false;
    private bool already_2 = false;
    private bool leftFalling = false;
    private bool rightFalling = false;
    private bool ending = false;

    // Coral Siren�� �й踦 ����
    public static bool coralDefeated = false;

    public static bool die = false;


    // Start is called before the first frame update
    void Start()
    {
        originColor = transform.GetComponent<SpriteRenderer>().color;
        transparentColor = originColor;
        transparentColor.a = 0.5f;

        coralSiren_Back = GameObject.Find("Coral Siren_Back");
        player = GameObject.Find("Player");

        bossAttackManager = GameObject.Find("Boss Attack Manager");

        explosionPrefab = Resources.Load<GameObject>("Boss Fight_Coral Siren/Prefabs/blow");

        for (int i = 0; i < 15; i++)
        {
            // �ؿ� �����ϱ�
            //explosions[i] = Instantiate(explosionPrefab, )
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HitController.coralDamaged == true)
        {
            // �÷��̾ ���� �������� 4���� 9������ ����
            int getDamage = Random.Range(4, 10);
            // Empress HP ���. 
            coralSirenHP -= getDamage;

            StartCoroutine(FlashCoral());
        }

        if (coralSirenHP <= 0 && already_1 == false) // �й� �� ��� ����
        {
            // ���� ������Ʈ�� ī�޶� ������ �̵�
            damageObjects = GameObject.FindGameObjectsWithTag("Damage");

            foreach (GameObject damageObject in damageObjects)
            {
                damageObject.transform.position = new Vector2(0, -10f);
            }

            // ������ �̵� ������ ����
            if (player.transform.position.x < 0)
            {
                transform.position = new Vector2(4, 9f);

                rightFalling = true;
            }
            else if (player.transform.position.x > 0)
            {
                transform.position = new Vector2(-4, 9f);

                leftFalling = true;
            }
        }

        if (rightFalling == true && already_2 == false|| leftFalling == true && already_2 == false)
        { 
            already_2 = true;

            // ������ ����Ǵ� ��ũ��Ʈ�� �ڷ�ƾ ���� �����ϱ�_CoralSiren_Back
            coralSiren_Back.GetComponent<CoralSirenMoving>().enabled = false;
            coralSiren_Back.GetComponent<FireBomb>().enabled = false;
            coralSiren_Back.GetComponent<GrabLever>().enabled = false;
            coralSiren_Back.GetComponent<CoralSirenMoving>().StopAllCoroutines();
            coralSiren_Back.GetComponent<FireBomb>().StopAllCoroutines();
            coralSiren_Back.GetComponent<GrabLever>().StopAllCoroutines();

            // boss attack manager
            bossAttackManager.GetComponent<Dash>().enabled = false;
            bossAttackManager.GetComponent<FireSpread>().enabled = false;

            // ������ ����Ǵ� ��ũ��Ʈ�� �ڷ�ƾ ���� �����ϱ�_CoralSiren_Front
            transform.GetComponent<FrontGrounded>().enabled = false;
            transform.GetComponent<FrontGrounded>().StopAllCoroutines();

            // �����Ÿ��� �� ����
            transform.GetComponent<SpriteRenderer>().color = originColor;

            for (int i = 0; i < 3; i++) // Coral Siren_Front�� ��ƼŬ �� ȿ�� ����
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            transform.GetComponent<Animator>().SetBool("Fire_Frail", true);

            // ��ġ�� �������ϰ� �̵� ���� ��ũ��Ʈ�� ��� ���� ���� ������ �����ϵ���. 
            already_1 = true;
        }

        if (coralSirenHP <= 0) // coralSiren_Back�� ��� ��ġ �̵��� ����.
        {
            coralSiren_Back.transform.position = new Vector2(10f, 10f);
        }

        // PlayWin() �ڷ�ƾ���� �߶� ��ȣ�� ���޹�����
        if (rightFalling == true) // ���������� �߶�
        {
            Debug.Log("�߶� ��ȣ ����");

            // �̵� ȿ�� ����
            transform.GetChild(3).gameObject.SetActive(true);

            /// <point> �ش� ��ǥ���� ȿ�� ����� ������ �̵��ϵ���.

            Vector2 destination = new Vector2(4, -1.5f);
            transform.position = Vector2.MoveTowards(transform.position, destination,
                Time.deltaTime * 20f);

            if (Vector2.Distance(transform.position, destination) <= 0.01f)
            {
                rightFalling = false;

                transform.position = destination;
                transform.GetComponent<Animator>().Play("Fire_Frail");

                ending = true;
            }
        }
        else if (leftFalling == true) // �������� �߶�
        {
            Debug.Log("�߶� ��ȣ ����");

            Vector2 destination = new Vector2(-4, -1.5f);
            transform.position = Vector2.MoveTowards(transform.position, destination,
                Time.deltaTime * 20f);

            if (Vector2.Distance(transform.position, destination) <= 0.01f)
            {
                leftFalling = false;

                transform.position = destination;
                transform.GetComponent<Animator>().Play("Fire_Frail");

                ending = true;
            }
        }

        if (ending == true)
        {
            // ��ź ȿ�� ���� 
        }
    }

    private IEnumerator FlashCoral()
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

        transform.GetComponent<SpriteRenderer>().color = originColor;
        HitController.coralDamaged = false;

        StopCoroutine(FlashCoral());
    }

    private IEnumerator PlayerWin() // Coral Siren �й�
    {
        yield return new WaitForSeconds(1.5f);

        transform.GetComponent<Animator>().Play("Fire_Drop");

        GameObject player = GameObject.Find("Player");

        yield return null;
    }
}
