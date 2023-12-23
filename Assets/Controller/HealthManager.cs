using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class HealthManager : PlayerView
{
    [Header("Settings")]
    [SerializeField] float health;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject deathPanel;

    [Header("Disable on Death")]
    [SerializeField] CapsuleCollider capsuleCollider;
    MeshRenderer[] meshRenderers;
    public float Health { get => health; set => health = value; }

    bool isDead = false;
    public bool IsDead { get => isDead; set => isDead = value; }

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
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
    public void Die()
    {
        IsDead = true;
        healthText.text = Health.ToString();
        deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        view.RPC(nameof(OnPlayerDie), RpcTarget.Others);
        playerManager.Die();
    }

    [PunRPC]
    void OnPlayerDie()
    {
        if (view.IsMine) return;
        capsuleCollider.enabled = false;
        foreach(MeshRenderer mr in meshRenderers)
        {
            mr.enabled = false;
        }
    }
}
