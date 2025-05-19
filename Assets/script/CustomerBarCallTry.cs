using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBarCallTry : MonoBehaviour
{
    private Coroutine patienceCoroutine; // ����@�[�˼ƪ���{
    public float HPMax { get; set; } = 100;

    private Image brokeBarImage; // �ۤv���W�� Image �ե�

    private float hp = 100;
    public float HP
    {
        get => hp;
        set { hp = Mathf.Clamp(value, 0, HPMax); }
    }

    void Awake()
    {
        // ���զb�l���󤤧�� Image ����]�q�` bar prefab �O�a�� image �� UI�^
        brokeBarImage = GetComponentInChildren<Image>();
        if (brokeBarImage == null)
        {
            Debug.LogError("�䤣�� Image �ե�I");
        }
    }

    public void StartPatience()
    {
        // �קK���ƱҰ�
        if (patienceCoroutine == null)
        {
            patienceCoroutine = StartCoroutine(PatienceDownTime());
        }
    }

    private void RefreshPatiencebar()
    {
        if (brokeBarImage != null)
        {
            brokeBarImage.fillAmount = HP / HPMax;
        }
    }

    private IEnumerator PatienceDownTime()
    {
        while (HP > 0)
        {
            HP -= HPMax * 0.08f; // �C���� 0.8%
            RefreshPatiencebar();
            yield return new WaitForSeconds(1f);
        }

        // TODO�G�@�[�� 0 �ɥi�[�J�ʵe�B�P����
    }
}
