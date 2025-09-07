using UnityEngine;
using UnityEngine.UI;
public class GrazeCounterUI : MonoBehaviour
{
    public static GrazeCounterUI instance;
    [SerializeField] private Slider grazeBar;
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
        Graze.instance.updateGrazeValue.AddListener(UpdateSlider);
    }

    private void UpdateSlider(int grazeAmount, int maxGrazeAmount)
    {
        grazeBar.value = ((float)grazeAmount) / ((float)maxGrazeAmount);
    }

}
