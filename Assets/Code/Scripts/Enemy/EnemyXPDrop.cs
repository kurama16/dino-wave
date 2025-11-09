using UnityEngine;

public class EnemyXPDrop : MonoBehaviour
{
    [SerializeField] GameObject xpPrefab;
    [SerializeField] int xpAmount = 1;

    public void DropXP()
    {
        for (int i = 0; i < xpAmount; i++)
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }
    }
}