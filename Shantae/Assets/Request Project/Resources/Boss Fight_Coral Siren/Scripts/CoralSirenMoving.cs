using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� Coral Siren�� ���� ������ ���ϴ� Ŭ����. 
/// </summary>

public class CoralSirenMoving : MonoBehaviour
{
    public static CoralSirenMoving instance;

    private int randomAttack = default;
    public static bool fireBomb = false;
    public static bool dash = false;
    public static bool fireSpread = false;
    public static bool grabLever = false;
    private Animator animator;
    public static Vector2 newBossPosition;

    public static bool firstPatternDone = false;
    public static bool secondPatternDone = false;
    public static bool thirdPatternDone = false;
    public static bool fourthPatternDone = false;
    private bool patternPlay = false;

    private GameObject sandGroup;
    private GameObject firstSand;
    private GameObject secondSand;
    private GameObject thirdSand;
    private GameObject fourthSand;

    // ù ��� �� �ƽ� ���� �����ϵ���
    private bool firstCutScene = default;

    // �ߺ� ���� ���׸� ���� ����. 
    private bool notDuplication = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        sandGroup = GameObject.Find("Sand");
        Debug.Assert(sandGroup != null);

        firstSand = sandGroup.transform.GetChild(0).GetChild(1).gameObject;
        secondSand = sandGroup.transform.GetChild(1).GetChild(1).gameObject;
        thirdSand = sandGroup.transform.GetChild(2).GetChild(1).gameObject;
        fourthSand = sandGroup.transform.GetChild(3).GetChild(1).gameObject;

        // ���� ����
        StartCoroutine(RandomMoving());
    }

    private void Update()
    {
        // �� �Ѹ��Ⱑ ������ �ʱ�ȭ ��ȣ �Ѹ���
        if (FireSpread.allStop == true)
        {
            fireSpread = false;
        }

        // ������ ���� �׼��� ���ߴٸ� �𷡸� ä��� �ʱ�ȭ ��ȣ �Ѹ���
        if (GrabLever.sandActive == true)
        {
            for (int i = 0; i < 6; i++)
            {
                firstSand.transform.GetChild(i).gameObject.SetActive(true);
                secondSand.transform.GetChild(i).gameObject.SetActive(true);
                thirdSand.transform.GetChild(i).gameObject.SetActive(true);
                fourthSand.transform.GetChild(i).gameObject.SetActive(true);
            }
            GrabLever.sandActive = false;
        }

        // �� ���� �� �ϳ��� ������ �����ٸ�
        if ((firstPatternDone == true || secondPatternDone == true
            || thirdPatternDone == true || fourthPatternDone == true))
        {
            StopCoroutine(RandomMoving());

            /// <problem> �� �� ���� Ȯ���� ���� ������ ���ÿ� ����. �� �� �� �ʱ�ȭ
            fireBomb = false;
            dash = false;
            fireSpread = false;
            grabLever = false;

            // ���� �ʱ�ȭ�ϰ� ���� �ڷ�ƾ �ٽ� ���� 
            firstPatternDone = false;
            secondPatternDone = false;
            thirdPatternDone = false;
            fourthPatternDone = false;

            patternPlay = false;

            if (CoralSirenController.coralDefeated == false)
            {
                StartCoroutine(RandomMoving());
            }
        }
    }

    // Update is called once per frame
    IEnumerator RandomMoving()
    {
        if (firstCutScene == false)
        {
            yield return new WaitForSeconds(3.5f);
            firstCutScene = true;
        }

        // �� ä��� �ߵ� ���� üũ
        for (int i = 0; i < 6; i++)
        {
            if (firstSand.transform.GetChild(i).gameObject.activeSelf == false ||
                secondSand.transform.GetChild(i).gameObject.activeSelf == false ||
                thirdSand.transform.GetChild(i).gameObject.activeSelf == false ||
                fourthSand.transform.GetChild(i).gameObject.activeSelf == false)
            {
                // �� �� �ϳ��� ����ִٸ� �ٷ� �� ä��� ���� ����
                randomAttack = 3;
            }
            else
            {   
                randomAttack = Random.Range(0, 3);
                //randomAttack = Random.Range(1, 3); // �ӽ�
            }   
        }
       
        // ���� ���� ��� ��� �ð�
        yield return new WaitForSeconds(1.5f);

        if (randomAttack == 0 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("1. ��ź �߻�");

            // ��ź �߻�
            animator.SetBool("Fire Bomb", true);

            fireBomb = true;

            // ��ź �߻� �Ϸ�
            if (FireBomb.doneFire == true)
            {
                animator.SetBool("Fire Bomb", false);

                fireBomb = false;
            }
        }
        else if (randomAttack == 1 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("2. ���");

            // ��� �غ� (DashCharging)
            dash = true;
        }
        else if (randomAttack == 2 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("3. �� �Ѹ���");

            // �� �Ѹ��� �غ� (FireSpread)
            fireSpread = true;
        }
        else if (randomAttack == 3 && patternPlay == false)
        {
            patternPlay = true;

            Debug.Log("4. �� ä���");

            // �� ä��� �غ�
            grabLever = true;
        }
    }
}
