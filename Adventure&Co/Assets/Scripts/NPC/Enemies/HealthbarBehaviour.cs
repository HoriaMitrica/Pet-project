using UnityEngine;
using UnityEngine.UI;

namespace NPC.Enemies
{
    public class HealthbarBehaviour : MonoBehaviour
    { 
        [SerializeField] private Slider slider;

        [SerializeField] private Color low;

        [SerializeField] private Color high;
        
        
        public void SetHealth(float health, float maxHealth)
        {
            slider.gameObject.SetActive(health<maxHealth);
            slider.value = health;
            slider.maxValue = maxHealth; 
            
            slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high,slider.normalizedValue);
        }
    }
}
