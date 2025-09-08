using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MGGJ25.Shared;
using UnityEngine.InputSystem;
public class Graze : MonoBehaviour
{
    public static Graze instance;
    [Header("Graze Meter")]
    [SerializeField] private int grazeAmount;
    public int maxGrazeAmount;
    public UnityEvent<int, int> updateGrazeValue = new();
    public UnityEvent endSkillTimer = new();
    // key == ID of gameObject
    // value == # of times the projectile has touched the area.
    private Dictionary<int, int> projectileContact = new();

    [Header("Graze Radius")]
    public float grazeRadius = 5f;
    [Header("Graze Visual")]
    [SerializeField] private Group grazePattern;
    [SerializeField] private GameObject grazeImage;
    [SerializeField] private Transform grazeImageParent;

    [Header("Skill Duration")]
    [SerializeField] private float skillDuration = 1f;
    private float skillTimer = 0f;
    private bool startTimer = false;

    [Header("Controller Haptics")]
    [SerializeField] private bool isRumbleOn = true;
    [SerializeField][Tooltip("In Seconds")] private float rumbleDuration;
    [SerializeField] private float leftMotorSpeed;
    [SerializeField] private float rightMotorSpeed;
    private bool startRumbleTimer = false;
    private float rumbleTimer = 0f;

    [Header("SkillBar Draining Effect")]
    [SerializeField] private AnimationCurve curve;

    [Header("For Debugging Purposes")]
    public bool turnOnGizmos = false;

    private readonly List<Vector3> _hits = new();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < grazePattern.projectileCount; i++)
        {
            float angle = HelperFunctions.CalculateProjectilePositionAngle(i, grazePattern);
            float x = Mathf.Cos(angle) * grazeRadius + transform.position.x;
            float y = Mathf.Sin(angle) * grazeRadius + transform.position.y;
            Instantiate(grazeImage, new Vector3(x, y, 0f), Quaternion.identity, grazeImageParent);
            if (isRumbleOn)
            {
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(leftMotorSpeed, rightMotorSpeed);
                    Gamepad.current.PauseHaptics();
                }
            }
        }
    }

    void OnEnable()
    {
        LevelManager.OnLevelUnload += ClearGrazeCount;
        LevelManager.OnLevelUnload += ResetGrazeCountUI;
        LevelManager.OnLevelUnload += StopSkillTimer;
    }

    void OnDisable()
    {
        LevelManager.OnLevelUnload -= ClearGrazeCount;
        LevelManager.OnLevelUnload -= ResetGrazeCountUI;
        LevelManager.OnLevelUnload -= StopSkillTimer;
    }

    void Update()
    {
        if (startTimer)
        {
            SkillTimer();
        }
        if (isRumbleOn && startRumbleTimer)
        {
            RumbleTimer();
        }
    }

    public void AddGrazeCount(int instanceID, Vector3 position)
    {
        if (!startTimer)
        {
            if (!projectileContact.ContainsKey(instanceID))
            {
                projectileContact.Add(instanceID, 1);
                grazeAmount = Mathf.Clamp(grazeAmount + 1, 0, maxGrazeAmount);
                updateGrazeValue.Invoke(grazeAmount, maxGrazeAmount);
                PlayerData.UpdateGraze(1);
                AudioManager.Instance.PlayPlayerGraze_SFX();
                _hits.Add(position);
                startRumbleTimer = true;
                if (Gamepad.current != null)
                {
                    Gamepad.current.ResumeHaptics();
                }
            }
            else
            {
                return;
            }

            if (IsGrazeFull()) AudioManager.Instance.PlayPlayerGrazeFull_SFX();
        }
    }

    public bool IsGrazeFull()
    {
        return grazeAmount >= maxGrazeAmount;
    }

    public void StartSkillTimer()
    {
        startTimer = true;
    }

    public void StopSkillTimer()
    {
        startTimer = false;
        skillTimer = 0f;
        ClearGrazeCount();
        endSkillTimer.Invoke();
    }

    private void SkillTimer()
    {
        skillTimer += Time.deltaTime;
        if (skillTimer >= skillDuration)
        {
            StopSkillTimer();
        }
        else
        {
            GrazeBarCountDown(skillTimer, skillDuration);
        }
    }

    private void RumbleTimer()
    {
        rumbleTimer += Time.deltaTime;
        if (rumbleTimer >= rumbleDuration)
        {
            startRumbleTimer = false;
            rumbleTimer = 0f;
            if (Gamepad.current != null)
            {
                Gamepad.current.PauseHaptics();
            }
        }
    }
    private void GrazeBarCountDown(float time, float maxTime)
    {
        float ev = curve.Evaluate(time / maxTime);
        float value = Mathf.Lerp(0f, (float)maxGrazeAmount, ev);
        updateGrazeValue.Invoke((int)value, maxGrazeAmount);
    }
    public void ClearGrazeCount()
    {
        projectileContact.Clear();
        grazeAmount = 0;
        updateGrazeValue.Invoke(grazeAmount, maxGrazeAmount);
        _hits.Clear();
    }

    public void ResetGrazeCountUI()
    {
        PlayerData.ClearGraze();
    }

    public void RemoveGrazeCount(int instanceID)
    {
        if (projectileContact.ContainsKey(instanceID))
        {
            projectileContact.Remove(instanceID);
        }
    }
    void OnDrawGizmos()
    {
        if (turnOnGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, grazeRadius);
            Gizmos.color = Color.red;
            for (int i = 0; i < _hits.Count; i++)
            {
                Gizmos.DrawWireSphere(_hits[i], .25f);
            }
        }
    }
}
