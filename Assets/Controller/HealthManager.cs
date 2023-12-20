using UnityEngine;
using Photon.Pun;
using TMPro;

public class HealthManager : PlayerView
{
    [Header("Settings")]
    [SerializeField] float health;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject deathPanel;
    public float Health { get => health; set => health = value; }

    bool isDead = false;
    public bool IsDead { get => isDead; set => isDead = value; }

    private void Start()
    {
    }

    [PunRPC]
    public void RPC_OnTakeDamage(float dmg, int playerID)
    {
        Health -= dmg;
        if(Health <= 0)
        {
            IsDead = true;
            healthText.text = Health.ToString();
            deathPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void OnRespawn()
    {
        Health = 100f;
        healthText.text = Health.ToString();
        IsDead = false;
        deathPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

}
