using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SandCloudEffect : MonoBehaviour
{
    #region �𷡱��� ����
    // �𷡱����� ���� ��ġ
    private Vector2 originPosition = default;
    // �𷡱����� ���� ����
    private Color originColor = default;
    // �𷡱����� ���� ��ġ
    private Vector2 cloudDestination = default;
    // �𷡱����� ��� �ӵ�
    private float cloudSpeed = 0.3f;
    // �𷡱����� Ŀ���� ����
    private float cloudScaleUp = 1.1f;
    private Vector2 originScale = default;
    private Vector2 upScale = default;
    // �𷡱����� �÷� (Sprite Renderer)
    private SpriteRenderer cloudColor = default;
    // �𷡱�����  ���� ���İ�
    private float cloudOriginAlpha = default;
    // ���� �ϷḦ �˸�.
    public bool changeFinished = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // ũ�� ����
        originScale = new Vector2(0.65f, 0.65f);
        upScale = new Vector2(1, 1);
        // ��ġ ����
        originPosition = new Vector2(transform.position.x, 0.28f);
        cloudDestination = new Vector2(transform.position.x, transform.position.y + 0.5f);
        // �÷� ����
        originColor = new Color(1, 0.8588f, 0.6196f, 0.5607f);
        cloudOriginAlpha = 0.5607f; // RGB 0-1.0ǥ��

        transform.localScale = originScale;

        // �� ������ SpriteRenderer
        cloudColor = GetComponent<SpriteRenderer>();
        Debug.Assert(cloudColor != null);
        cloudColor.color = originColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (GrabLever.recolor == true && CoralSirenMoving.fourthPatternDone == false)
        {
            changeFinished = false;
            transform.GetComponent<SpriteRenderer>().enabled = true;

            // ���� �����Ϻ��� �۴ٸ�
            if (transform.localScale.x < upScale.x && transform.localScale.y < upScale.y)
            {
                transform.localScale *= cloudScaleUp;
            }
            else // ���� �������� �����ߴٸ� ������ ����
            {
                transform.localScale = upScale;
            }

            // ���� �����Ϻ��� ũ�ٸ�
            if (transform.localScale.x >= upScale.x && transform.localScale.y >= upScale.y)
            {
                transform.position = Vector2.MoveTowards
                    (transform.position, cloudDestination, Time.deltaTime * cloudSpeed);

                StartCoroutine(CloudFadeOut());
            }
        }
        else if (GrabLever.recolor == false && changeFinished == true)
        {
            transform.GetComponent<SpriteRenderer>().enabled = false;

            // �ʱ�ȭ
            transform.localScale = originScale;
            cloudColor.color = originColor;
        }
    }

    IEnumerator CloudFadeOut()
    {
        /// <problem> �ι�° �̻� ������� õõ�� �������� �ʴ´�. 
        while (cloudColor.color.a > 0)
        {
            cloudOriginAlpha -= 0.001f;
            yield return new WaitForSeconds(0.5f);
            cloudColor.color = new Color(1, 0.8588f, 0.6196f, cloudOriginAlpha);
        }

        changeFinished = true;

        yield return null;
    }
}
