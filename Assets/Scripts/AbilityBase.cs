using UnityEngine;

// Timer general class. Used to (shocker) set, start, and detect when timers end
// Can be set with an initial timer, or use default constructor and set later
public class Timer
{
    private float start_time;
    private float end_time;
    private float duration;

    private bool is_running;

    public Timer(float duration)
    {
        this.duration = duration;
        this.start_time = Time.time;
    }

    public Timer() : this(0) { }

    public void setDuration(float duration)
    {
        this.duration = duration;
    }

    public void start()
    {
        is_running = true;
        start_time = Time.time;
        end_time = Time.time + duration;
    }

    public void stop()
    {
        is_running = false;
    }

    public bool IsRunning()
    {
        if (Time.time >= end_time || !is_running)
        {
            is_running = false;
            return false;
        }

        is_running = true;
        return is_running;
    }
}

[System.Serializable]
abstract public class AbilityBase : Object
{
    public bool usingAbility;

    public Timer abilityCooldown;

    public GameObject player;

    public AbilityBase(GameObject player, Timer cooldown)
    {
        this.player = player;
        this.abilityCooldown = cooldown;
    }

    // Call ability(player) to start the ability. Once ability is started, if needed, make sure to call abilityOnFrame
    // inside the Update() method while the ability is active. Once the ability is inactive, be sure to set usingAbility
    // to false.
    abstract public void Ability();

    // Called once every FixedUpdate (may change to Update, idk)
    public void AbilityOnFrame()
    {
        return;
    }

    // Override this method if custom condition is required (i.e. Can only be used once the player has hit the ground,
    // pogo'd off an enemy, etc.
    public bool CanUseAbility()
    {
        return !usingAbility && !abilityCooldown.IsRunning();
    }
}
