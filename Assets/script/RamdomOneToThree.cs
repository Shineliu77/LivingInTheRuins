using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOneToThree : MonoBehaviour
{
    public GameObject spriteRendererPrefab; // �D����]�]�t�l����^
    public GameObject[] OneToThree; // �s��l���� Prefab �}�C

    void Start()
    {
        if (OneToThree == null || OneToThree.Length == 0)
        {
            Debug.LogWarning("OneToThree �}�C���šA�Цb Inspector ���K�[�l����C");
            return;
        }

        // �T�O�Ҧ��l����@�}�l���O���ê�
        foreach (GameObject obj in OneToThree)
        {
            obj.SetActive(false);
        }

        // �H���M�w��ܴX�ӡ]1 �� 3�^
        int count = Random.Range(1, 4);

        // �ΨӦs��w��ܪ��l������ޡA�T�O������
        List<int> selectedIndexes = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, OneToThree.Length);
            } while (selectedIndexes.Contains(randomIndex)); // �T�O������

            selectedIndexes.Add(randomIndex);

            // �ҥο襤���l����
            OneToThree[randomIndex].SetActive(true);
        }
    }
}