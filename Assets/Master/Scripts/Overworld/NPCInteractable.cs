using UnityEngine;
using Fungus;

using RPGTest.Interfaces;

namespace RPGTest.Overworld 
{
    public class NPCInteractable : MonoBehaviour, IInteractable
    {
        [Header("Fungus Setup")]
        public Flowchart flowchart;
        public string targetBlockName = "StartDialog";
    
        [Header("UI Ref")]
        public GameObject interactIconCanvas;
    
        protected Camera mainCam;
        protected Vector3 originalLocalScale;
    
        protected virtual void Start() 
        {
            mainCam = Camera.main;
            if (interactIconCanvas != null) 
            {
                interactIconCanvas.SetActive(false);
                originalLocalScale = interactIconCanvas.transform.localScale;
            }
        }
    
        protected virtual void LateUpdate()
        {
            if (interactIconCanvas != null && interactIconCanvas.activeInHierarchy && interactIconCanvas.transform.parent != null)
            {
                interactIconCanvas.transform.rotation = mainCam.transform.rotation;
                float parentScaleX = Mathf.Sign(interactIconCanvas.transform.parent.localScale.x); 
                interactIconCanvas.transform.localScale = new Vector3(originalLocalScale.x * parentScaleX, originalLocalScale.y, originalLocalScale.z);
            }
        }
    
        public virtual void Interact()
        {
            if (flowchart != null) 
            {
                flowchart.ExecuteBlock(targetBlockName);
            }
        }
    
        public void ShowInteractIcon() 
        {
            SetIcon(true);
        }
        
        public void HideInteractIcon()
        {
            SetIcon(false);
        }
    
        private void SetIcon(bool state)
        {
            if (interactIconCanvas != null) interactIconCanvas.SetActive(state);
        }
    }
}