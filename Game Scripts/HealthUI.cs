using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public RectTransform healthRect;
    public RectTransform shieldRect;
    public Text healthText;

    private Vector2 healthVec;
    private Vector2 shieldVec;
    public float speed;

    private void Update()
    {
        healthVec = healthRect.transform.localScale;
        healthVec.x = GameManager.instance.GetPlayerHealthPer();
        healthRect.transform.localScale = Vector3.Lerp(healthRect.transform.localScale, healthVec, Time.deltaTime * speed);
        healthText.text = "HP " + (GameManager.instance.GetPlayerHealthPer() * 100).ToString() + "%";

        shieldVec = shieldRect.transform.localScale;
        shieldVec.x = GameManager.instance.GetPlayerShieldPer();
        shieldRect.transform.localScale = Vector3.Lerp(shieldRect.transform.localScale, shieldVec, Time.deltaTime * speed);
    }
}
