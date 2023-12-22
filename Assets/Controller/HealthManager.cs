using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class HealthManager : PlayerView
{
    [Header("Settings")]
    [SerializeField] float health;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Button respawnButton;
    [SerializeField] GameObject deathPanel;
    public float Health { get => health; set => health = value; }

    bool isDead = false;
    public bool IsDead { get => isDead; set => isDead = value; }

    private void Start()
    {
        respawnButton.onClick.AddListener(() => playerManager.RespawnPlayerOnline(gameObject));
    }

    [PunRPC]
    public void RPC_OnTakeDamage(float dmg, PhotonMessageInfo info)
    {
        Health -= dmg;
        if(Health <= 0)
        {
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
    }

    void Die()
    {
        IsDead = true;
        healthText.text = Health.ToString();

        deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

}
