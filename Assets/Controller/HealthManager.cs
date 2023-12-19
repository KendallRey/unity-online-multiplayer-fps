using UnityEngine;
using Photon.Pun;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float health;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject deathPanel;
    public float Health { get => health; set => health = value; }

    bool isDead = false;
    public bool IsDead { get => isDead; set => isDead = value; }

    SpawnPlayers spawner;
    public SpawnPlayers Spawner { get => spawner; set => spawner = value; }

    private void Update()
    {
        if (IsDead) return;
        healthText.text = Health.ToString();
    }


    [PunRPC]
    public void OnTakeDamage(float dmg, string playerID)
    {
        Health -= dmg;
        if(Health <= 0)
        {
            Debug.Log("Health:" + Health);
            Debug.Log("Hitter:" + playerID);
            IsDead = true;
            deathPanel.SetActive(true);
        }
    }
    public void OnRespawn()
    {
        Vector3 randomPosition = Spawner.GetRandomPosition();
        Debug.Log("Respawn: " + randomPosition);
        Health = 100f;
        IsDead = false;
        deathPanel.SetActive(false);
        gameObject.transform.position = randomPosition;
    }

}
