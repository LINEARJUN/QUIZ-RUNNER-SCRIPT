using UnityEngine;
using QuizRunner.Struct;

public class SkillManager : Singleton<SkillManager>
{
    [System.Serializable]
    public struct Skill
    {
        public string name;
        public bool isActive;
        public SkillState skillState;
        public GameObject skillEffect;

        //Skill Luncher
        public delegate void Del(int level, float duration);
        public Del handler;

        public Skill(string _name)
        {
            name = _name;
            isActive = false;
            skillState = new SkillState();
            handler = null;
            skillEffect = null;
        }
    }

    public Skill[] skills = new Skill[]
    {
        new Skill("ghost"),
        new Skill("heal"),
        new Skill("shield"),
        new Skill("overhealth")
    };

    private void Start()
    {
        skills[0].handler = Ghost;
        skills[1].handler = Heal;
        skills[2].handler = Shield;
        skills[3].handler = OverHealth;
    }

    /// <summary>
    /// 스킬 상태 업데이트 메소드
    /// </summary>
    /// <param name="index">스킬 코드</param>
    /// <param name="enable">스킬 활성 상태</param>
    /// <param name="level">스킬 레벨</param>
    /// <param name="duration">지속 시간</param>
    public void UpdateSkill(int index, bool enable, int level = 1, float duration = 1)
    {
        SkillState result = new SkillState();

        skills[index].isActive = enable;

        for (int i = 0; i < skills.Length; i++)
        {
            result.isGod = result.isGod | (skills[i].isActive & skills[i].skillState.isGod); //active와 스킬스탯을 논리 and 후 논리 or
            //result.isWalk...이런식으로 밑으로 계속 추가
        }

        //Send result to player
        ObjectManager.instance.player.skillState = result;

        if (enable)
        {
            skills[index].handler.Invoke(level, duration);

            //Place particle
            if (skills[index].skillEffect != null)
            {
                Destroy(Instantiate(skills[index].skillEffect, ObjectManager.instance.player.transform.position, skills[index].skillEffect.transform.rotation, ObjectManager.instance.player.transform), duration);
            }
        }
    }

    #region Skill Method

    /// <summary>
    /// 고스트 스킬 메소드
    /// </summary>
    /// <param name="level">스킬 레벨</param>
    /// <param name="duration">지속 시간</param>
    public void Ghost(int level, float duration)
    {
        float increase = 30;
        float original = Map.instance.initialSpeed; //초기속도를 기준으로 합니다.

        if (level == 1)
        {
            Map.instance.ChangeSpeed(Map.instance.speed + increase, .5f, () =>
            {
                Map.instance.ChangeSpeed(original, duration - .5f, () =>
                {
                    UpdateSkill(0, false, 0);
                });
            });
        }
    }

    /// <summary>
    /// 회복 스킬 메소드
    /// </summary>
    /// <param name="level">스킬 레벨</param>
    /// <param name="duration">지속 시간</param>
    public void Heal(int level, float duration) //Ignore Duration
    {
        if (level == 1)
        {
            ObjectManager.instance.player.GetHeal(ObjectManager.instance.player.GetHealthAmount(5));
        }
        else if (level == 2)
        {
            //...
        }
        UpdateSkill(1, false);
    }

    /// <summary>
    /// 방벽 스킬 메소드
    /// </summary>
    /// <param name="level">스킬 레벨</param>
    /// <param name="duration">지속 시간</param>
    public void Shield(int level, float duration) //Ignore Duration
    {
        ObjectManager.instance.player.GetShield(50f);
        UpdateSkill(2, false);
    }

    /// <summary>
    /// 최대 체력 증가 스킬 메소드
    /// </summary>
    /// <param name="level">스킬 레벨</param>
    /// <param name="duration">지속 시간 (무시)</param>
    public void OverHealth(int level, float duration) //Ignore Duration
    {
        if (level == 1)
        {
            ObjectManager.instance.player.OverHealth(ObjectManager.instance.player.GetHealthAmount(5));
        }
        else if (level == 2)
        {
            //...
        }
        UpdateSkill(3, false);
    }
    #endregion
}
