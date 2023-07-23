using System;
using System.Collections;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class ObtainedItem : MonoBehaviour
    {
        private float fadeInDuration = 1.0f;
        private float fadeOutDuration = 1.0f;
        
        [SerializeField] private float visibleDuration = 1.0f;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private TMP_Text obtainedText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image icon;
        [SerializeField] private Animator animator;
        [SerializeField] private MainWidget mainWidget;
        public void ShowObtainedItem()
        {
            StartCoroutine(PlayObtainedItemAnimation());
        }

        private IEnumerator PlayObtainedItemAnimation()
        {
            animator.SetTrigger("FadeIn");
            
            yield return new WaitForSeconds(fadeInDuration);
            
            yield return new WaitForSeconds(visibleDuration);
            
            animator.SetTrigger("FadeOut");
            
            yield return new WaitForSeconds(fadeOutDuration);
            gameObject.SetActive(false);
            mainWidget.OnObtainMessageEnd();
        }

        public void UpdateWidget(MasterItem itemClass,int amount)
        {
            nameText.color = itemClass.info.RarityColor;
            amountText.color = itemClass.info.RarityColor;
            obtainedText.color = itemClass.info.RarityColor;
            nameText.text = itemClass.info.Name;
            icon.sprite = itemClass.info.Icon;
            amountText.text = $"x{amount}";
        }
    }
}
