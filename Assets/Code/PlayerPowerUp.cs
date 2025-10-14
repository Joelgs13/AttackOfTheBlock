using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    private bool hasKill = false;

    public void GiveKillPowerUp()
    {
        hasKill = true;
        Debug.Log("PowerUp: ¡puedes matar al siguiente enemigo!");
    }

    public bool ConsumeKillIfAvailable()
    {
        if (hasKill)
        {
            hasKill = false;
            return true;
        }
        return false;
    }

    public bool HasKill() => hasKill;
}