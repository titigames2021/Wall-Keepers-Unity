using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyProjectile : MonoBehaviour
{
    #region General variables 
    [Header("Variables")]
    private float f_journeyLength;
    private Vector3 v3_origin, v3_target;
    public bool b_startProjectileMovement, b_damageAlreadyDealth;
    private float f_lerpValue = 0.0f;
    private float f_lerpSpeed;
    private int i_damage;
    #endregion
    #region Animations 
    [Header("Animations")]
    [SerializeField] private Animator animator_projectile;
    private int i_EndHash = Animator.StringToHash("Enter End"),
        i_LoopHash = Animator.StringToHash("Enter Loop");
    private GameObject go_targetObject;
    #endregion
    #region Additional SFX
    [Header("SFX")]
    [SerializeField] AudioClip aC_hitSfx;
    [SerializeField] AudioSource aS_projectailAudioSource;
    #endregion

    private void FixedUpdate()
    {
        if (LevelsGnralManager.b_stopLevel)
        {
            return;
        }

        if (b_startProjectileMovement)
        {
           ProjectailMovement();
        }
    }
    public void SetStats(Vector3 origin, GameObject targetObject, float speed, int damage)
    {
        v3_origin = origin;
        v3_target = targetObject.transform.position; 
        f_journeyLength = Vector3.Distance(origin, targetObject.transform.position);
        f_lerpSpeed = speed;
        i_damage = damage;
        go_targetObject = targetObject;
        ProjectileLookAtTarget();
    }
    public void ProjectailMovement()
    {
        f_lerpValue += Time.deltaTime * f_lerpSpeed;//Increment lerp value
        transform.position = Vector3.Lerp(v3_origin, v3_target, f_lerpValue);//Movement
        if (gameObject.transform.position == v3_target)
        {
            animator_projectile.SetTrigger(i_EndHash);
        }
    }
    internal void StartLoopAnimation()
    {
        animator_projectile.SetTrigger(i_LoopHash);
        b_startProjectileMovement = true;
    }
    private void ProjectileLookAtTarget()
    {
        Vector3 diff = go_targetObject.transform.position - transform.position; 
        diff.Normalize(); 
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
    internal void ResetProjectile()
    {
        gameObject.SetActive(false);
        b_startProjectileMovement = false;
        v3_origin = Vector3.zero;
        v3_target = Vector3.zero;
        f_journeyLength = 0;
        f_lerpSpeed = 0;
        f_lerpValue = 0;
        b_damageAlreadyDealth = false;
    }
    internal void DoDamage()
    {
        if (go_targetObject.GetComponent<WallManager>() != null)
        {
            if(go_targetObject.GetComponent<WallManager>().f_wallLife <= 0)
            {
                return;
            }
        }
        
        if (!b_damageAlreadyDealth)
        {
            b_damageAlreadyDealth = true;
            if (go_targetObject.GetComponent<WallManager>() != null)
            {
                go_targetObject.GetComponent<WallManager>().ReceiveHit(i_damage);
            }

            if (go_targetObject.GetComponent<Obstacle_StaticInstant>() != null)
            {
                go_targetObject.GetComponent<Obstacle_StaticInstant>().ReceiveDamage(i_damage);
            }
        }

    }

    internal void PlayHitSfx()
    {
        aS_projectailAudioSource.Stop();
        aS_projectailAudioSource.PlayOneShot(aC_hitSfx);
    }

}
