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
    // key == ID of gameObject
    // value == # of times the projectile has touched the area.
   private Dictionary<int, int> projectileContact = new Dictionary<int,int>();

   [Header("Graze Radius")]
    public float grazeRadius = 5f;
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

   public void AddGrazeCount(int instanceID, Vector3 position){
    if(!projectileContact.ContainsKey(instanceID)){
        projectileContact.Add(instanceID, 1);
        grazeAmount = Mathf.Clamp(grazeAmount + 1, 0, maxGrazeAmount);
        updateGrazeValue.Invoke(grazeAmount, maxGrazeAmount);
        hits.Add(position);
    } else {
         return;
    }
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
