using UnityEngine;

public class Item : SpawnObject
{
    [Header("Particle")]
    [SerializeField] GameObject solveParticle;
    [SerializeField] GameObject wrongParticle;
    [SerializeField] private float particleLifeTime;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (QuizManager.instance.ChallengeProblem(id))
            {
                //Play SFX
                AudioManager.instance.PlaySFX("correct");

                SkillManager.instance.UpdateSkill(0, true, 1, 2f);
                Destroy(Instantiate(solveParticle, transform.position, solveParticle.transform.rotation), particleLifeTime);
            }
            else
            {
                //Play SFX
                AudioManager.instance.PlaySFX("wrong");

                Vector3 spawnPos = transform.position;
                spawnPos.y += 2f;
                Destroy(Instantiate(wrongParticle, spawnPos, wrongParticle.transform.rotation), particleLifeTime);
            }
        }
        base.OnTriggerEnter(other);
    }
}
