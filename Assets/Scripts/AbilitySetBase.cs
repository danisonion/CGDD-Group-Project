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
        if (Time.time == end_time || !is_running)
        {
            is_running = false;
            return false;
        }

        is_running = true;
        return is_running;
    }
}

abstract public class AbilitySetBase : MonoBehaviour
{
    public bool usingAbility1;
    public bool usingAbility2;
    public bool usingAbility3;

    public Timer abilityTimer1;
    public Timer abilityTimer2;
    public Timer abilityTimer3;

    //region We probably don't need this, but I'm keeping it here just in case
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //endregion

    // Call ability1(player) to start the ability. Once ability is started, if needed, make sure to call ability1OnFrame
    // inside the Update() method while the ability is active. Once the ability is inactive, be sure to set usingAbility1
    // to false.
    abstract public void ability1(GameObject player);
    public void ability1OnFrame(GameObject player)
    {
        return;
    }

    abstract public void ability2(GameObject player);
    public void ability2OnFrame(GameObject player)
    {
        return;
    }

    abstract public void ability3(GameObject player);
    public void ability3OnFrame(GameObject player)
    {
        return;
    }
}
