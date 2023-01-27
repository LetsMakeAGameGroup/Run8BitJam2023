using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [HideInInspector] public int batteries = 1;
    [HideInInspector] public int maxBatteries = 3;

    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private int tempDecrease = 1;
    [SerializeField] private int tempIncrease = 5;
    [HideInInspector] public int currentTemp = 50;
    private int fireTicks = 0;
    [SerializeField] private float warningTimer = 5f;
    private float currentWarningTime = 5f;
    private float tempTimer = 1f;

    [SerializeField] private int maxTempWarning = 95;
    [SerializeField] private int minTempWarning = 25;
    private bool isWarning = false;

    public bool tempIsActive = false;

    Animator playeranimator;

    private void Awake()
    {
        playeranimator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        maxBatteries = HUDManager.Instance.batteries.Length;
        currentWarningTime = warningTimer;
    }

    private void Update() {

        // Naturally, decrease the currentTemp by tempDecrease every second. If the player is on fire, increase currentTemp by tempIncrease every second.
        if (tempIsActive && !PauseController.Instance.isPaused)
        {
            if (tempTimer > 0)
            {
                tempTimer -= Time.deltaTime;
            }
            else
            {
                if (fireTicks > 0)
                {
                    if (currentTemp + tempIncrease <= 100) currentTemp += tempIncrease;
                    else currentTemp = 100;
                }
                else
                {
                    if (currentTemp - tempDecrease >= 0) currentTemp -= tempDecrease;
                    else currentTemp = 0;
                }
                HUDManager.Instance.UpdateTemperature(currentTemp);
                tempTimer = 1f;
            }
        }

        // If the player's temperature is out of the ideal range, warn the player they're in danger of losing. If in danger for warningTimer seconds, then the game is lost.
        if (currentTemp <= minTempWarning || currentTemp >= maxTempWarning) {
            isWarning = true;
            if (currentWarningTime > 0) {
                currentWarningTime -= Time.deltaTime;
                HUDManager.Instance.UpdateWarning(currentWarningTime);
            } else {
                //Disable player for now
                if (GameMode.Instance != null)
                {
                    GameMode.Instance.EndGame();
                }

                gameObject.SetActive(false);
                // TODO:  Bring up game over screen here
            }
        } else if (isWarning) {
            isWarning = false;
            currentWarningTime = warningTimer;
            HUDManager.Instance.DisableWarning();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseController.Instance.TogglePause();
        }

        if (IsOnFire())
        {
            playeranimator.SetBool("OnFire", true);
            playeranimator.SetLayerWeight(1, 1);
        }
        else 
        {
            playeranimator.SetBool("OnFire", false);
            playeranimator.SetLayerWeight(1, 0);
        }
    }

    public bool IsOnFire() {
        if (fireTicks > 0) return true;
        else return false;
    }

    public void SetOnFire(int fireHP) {
        fireParticles.Play();
        fireTicks = fireHP;
    }

    public void SetOffFire() {
        fireParticles.Stop();
        fireTicks = 0;
    }
}
