using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneLightningEffect : BaseSpellsEffects
{
    public float f_stormDmg;
    public bool b_lightningInstantiated;
    #region Unity Life Cycle
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
    }
    #endregion
    # region OneLightning Methods
        public void SetDmg(float dmg)//This method is called by LightningStormArea before enabling this object.
        {
            f_stormDmg= dmg;
        }
        internal void ResetLightning()
        {
            enemiesScripts_List.Clear();
            b_spellEffectActiveInBattlefield = false;;
            gameObject.SetActive(false);
        }
        internal virtual void HurtEnemiesStorm()//Called by cast animation
        {
            foreach (Enemy enemy in enemiesScripts_List)
            {
                enemy.ReceiveDamage((int)f_stormDmg, e_spellElement);
            }
        }
        #endregion
    #region Collisions
    // When the enemy collides with this effect (when the collider is enabled), it calls the enemy's ReceiveDamage function.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == i_EnemyLayer)
            {
                AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
            }
        }
    #endregion
}
