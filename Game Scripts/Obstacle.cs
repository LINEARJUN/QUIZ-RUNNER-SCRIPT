using UnityEngine;

public class Obstacle : SpawnObject
{
    public float damage;

    [Header("Particle")]
    [SerializeField] GameObject destroyParticle;
    [SerializeField] private float destoryParticleLifeTime;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //Play SFX
            AudioManager.instance.PlaySFX("crash");

            other.GetComponent<Player>().GetDamage(damage);
            Destroy(Instantiate(destroyParticle, transform.position, destroyParticle.transform.rotation), destoryParticleLifeTime);
        }
        base.OnTriggerEnter(other);
    }
}
