using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour
{
    public Button unlockButton;   // Button that needs to be pressed to unlock another button
    public Button lockedButton;   // Button that is initially locked
    public Button unlockedButton; // Button that is initially locked but becomes unlocked after pressing unlockButton

 
    void Start()
    {
        
        lockedButton.interactable = false;
        unlockedButton.interactable = false;
    }

   
    public void PressUnlockButton()
    {
        unlockButton.interactable = false;   
        lockedButton.interactable = true;    
    }

   
    public void PressLockedButton()
    {   
        Debug.Log("Locked button pressed!");
    }
}
