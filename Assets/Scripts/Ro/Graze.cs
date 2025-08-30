using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Graze : MonoBehaviour{
   public static Graze instance;
   [Header("Graze Meter")]
   [SerializeField] private int grazeAmount;
   public int maxGrazeAmount;
   public UnityEvent<int, int> updateGrazeValue = new UnityEvent<int, int>();
   public UnityEvent endSkillTimer = new UnityEvent();
    // key == ID of gameObject
    // value == # of times the projectile has touched the area.
   private Dictionary<int, int> projectileContact = new Dictionary<int,int>();

   [Header("Graze Radius")]
    public float grazeRadius = 5f;

    [Header("Skill Duration")]
    [SerializeField] private float skillDuration = 1f;
    private float skillTimer = 0f;
    private bool startTimer = false;

    [Header("SkillBar Draining Effect")]
    [SerializeField] private AnimationCurve curve;

    [Header("For Debugging Purposes")]
    public bool turnOnGizmos = false;
    private List<Vector3> hits = new List<Vector3>();




    void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    void Update(){
        if(startTimer){
            SkillTimer();
        }
    }

   public void AddGrazeCount(int instanceID, Vector3 position){
    if(!startTimer){
        if(!projectileContact.ContainsKey(instanceID)){
            projectileContact.Add(instanceID, 1);
            grazeAmount = Mathf.Clamp(grazeAmount + 1, 0, maxGrazeAmount);
            updateGrazeValue.Invoke(grazeAmount, maxGrazeAmount);
            hits.Add(position);
        } else {
            return;
        }
    }
    
   }

   public bool IsGrazeFull(){
    return grazeAmount >= maxGrazeAmount;
   }

   public void StartSkillTimer(){
        startTimer = true;
   }

   private void SkillTimer(){
        skillTimer += Time.deltaTime;
        if(skillTimer >= skillDuration){
            startTimer = false;
            skillTimer = 0f;
            ClearGrazeCount();
            endSkillTimer.Invoke();
        } else {
            GrazeBarCountDown(skillTimer, skillDuration);
        }
   }
    private void GrazeBarCountDown(float time, float maxTime){
        float ev = curve.Evaluate(time/maxTime);
        float value = Mathf.Lerp(0f, (float)maxGrazeAmount, ev);
        updateGrazeValue.Invoke((int)value, maxGrazeAmount);
    }
   public void ClearGrazeCount(){
        projectileContact.Clear();
        grazeAmount = 0;
        updateGrazeValue.Invoke(grazeAmount, maxGrazeAmount);
   }

   public void RemoveGrazeCount(int instanceID){
        if(projectileContact.ContainsKey(instanceID)){
            projectileContact.Remove(instanceID);
        }
   }
   void OnDrawGizmos(){
       if(turnOnGizmos){
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, grazeRadius);
            Gizmos.color = Color.red;
            for(int i = 0; i < hits.Count ; i++){
                 Gizmos.DrawWireSphere(hits[i], .25f);
            }
           
       }
   }
}
