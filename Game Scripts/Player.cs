using System.Collections;
using UnityEngine;
using QuizRunner.Struct;

public class Player : MonoBehaviour
{
    #region Variable Declaration
    [Header("Property")]
    public float speed;
    private float health;
    private float maxHealth = 100f;
    private float shield;
    private float maxShield = 100f;

    [Header("State")]
    private bool isDead;
    private bool isMoving;

    [Header("Skill State")]
    public SkillState skillState = new SkillState();

    [Header("Movement")]
    private int playerLimit;
    public int playerPosIndex = -1;
    private Vector3 destination;

    [Header("Particle")]
    public GameObject shieldParticle;
    public GameObject deathParticle;
    #endregion

    #region Life Cycle

    private void OnEnable()
    {
        GameManager.GameManagerInitialized += OnInitialized;
    }
    private void OnDisable()
    {
        GameManager.GameManagerInitialized -= OnInitialized;
    }

    private void OnInitialized()
    {
        //Health Initialize
        health = maxHealth;

        //플레이어의 움직임 좌표를 계산
        Calculation();

        //Coroutine Call
        StartCoroutine(LerpUpdate());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 pos = Input.mousePosition;
            int xLimit = Screen.width / 2;
            int yLimit = Screen.height - Screen.height / 4;
            if (pos.y < yLimit)
            {
                if (pos.x < xLimit)
                {
                    if (playerPosIndex > 0)
                    {
                        playerPosIndex--;
                        SetDestination(Map.instance.movePoint[playerPosIndex]);
                    }
                }
                else
                {
                    if (playerPosIndex < playerLimit - 1)
                    {
                        playerPosIndex++;
                        SetDestination(Map.instance.movePoint[playerPosIndex]);
                    }
                }
            }
        }
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (playerPosIndex > 0)
                {
                    playerPosIndex--;
                    SetDestination(Map.instance.movePoint[playerPosIndex]);
                }

            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (playerPosIndex < playerLimit - 1)
                {
                    playerPosIndex++;
                    SetDestination(Map.instance.movePoint[playerPosIndex]);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SkillManager.instance.UpdateSkill(0, true, 1, 2f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SkillManager.instance.UpdateSkill(1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SkillManager.instance.UpdateSkill(2, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SkillManager.instance.UpdateSkill(3, true);
        }
    }

    #endregion

    #region Health Management
    private void UpdateHealth(float value)
    {
        //Update your health
        health += value;

        //Then check if player is dead
        if (health <= 0)
        {
            //Call Method-------------------------
            OnDead();
            GameManager.instance.GameOver();
            //Done---------------------------------
            return;
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void GetHeal(float value)
    {
        UpdateHealth(value);
    }
    public void OverHealth(float value)
    {
        maxHealth += value;
    }
    public float GetHealthAmount(int divide)
    {
        return maxHealth / divide;
    }
    public float GetHealthPer()
    {
        return health / maxHealth;
    }
    #endregion

    #region Shield Management
    private void UpdateShield(float value)
    {
        //Update your health
        shield += value;

        //Then check if player is dead
        if (shield <= 0)
        {
            shieldParticle.SetActive(false);
            return;
        }
        else if (shield > maxShield)
        {
            shield = maxShield;
        }
        shieldParticle.SetActive(true);
    }
    public void GetShield(float value)
    {
        UpdateShield(value);
    }
    public float GetShieldAmount(int divide)
    {
        return maxShield / divide;
    }
    public float GetShieldPer()
    {
        return shield / maxShield;
    }
    #endregion

    #region Health & Sheild
    public void GetDamage(float value)
    {
        float result = 0;
        if (CanDamage())
        {
            result = shield - value;
            if (shield > 0) UpdateShield(-value);
            if (result <= 0) UpdateHealth(result);
        }
    }
    #endregion

    #region Movement

    //움직이기 전 플레이어 초기화
    public void Calculation()
    {
        playerLimit = Map.instance.mapLimit;
        if (playerPosIndex == -1)
        {
            playerPosIndex = Map.instance.mapLimit / 2;
            SetDestination(Map.instance.movePoint[playerPosIndex]);
        }
    }

    //실질적인 움직임 처리
    public void SetDestination(Vector3 destination)
    {
        if (CanMove()) this.destination = destination;
    }

    private IEnumerator LerpUpdate()
    {
        //We'll gonna never stop this loop
        for (; ; )
        {
            if (Vector3.Distance(transform.position, destination) < 1f)
            {
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }
            destination.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
            yield return null;
        }
    }
    #endregion

    #region Player State
    private void OnDead()
    {
        //Set isDead option
        isDead = true;

        //Invisible
        GetComponent<MeshRenderer>().enabled = false;

        //Particle Instantiate
        var par = Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
        DynamicManager.instance.RegisterObject(par);
        Destroy(par, 2);
    }
    #endregion

    #region Availability
    public bool CanMove()
    {
        if (!isDead) //조건을 계속 추가해 나가시오
        {
            return true;
        }
        return false;
    }
    public bool CanDamage()
    {
        if (!isDead && !skillState.isGod)
        {
            return true;
        }
        return false;
    }
    #endregion
}