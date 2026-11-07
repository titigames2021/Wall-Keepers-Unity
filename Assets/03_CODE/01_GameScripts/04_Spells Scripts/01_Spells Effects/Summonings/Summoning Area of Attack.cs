using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningAreaofAttack : MonoBehaviour
{
    [SerializeField] Basic_Summoning sript_basicSummoningEffect;

    #region Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == sript_basicSummoningEffect.GetEnemyLayer())
        {
            sript_basicSummoningEffect.AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == sript_basicSummoningEffect.GetEnemyLayer())
        {
            sript_basicSummoningEffect.RemoveFromEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }
    #endregion
}
