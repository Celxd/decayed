using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockSystem : MonoBehaviour
{
    public static LevelUnlockSystem instance;
    public Button[] levelButtons; // Array to store level buttons
    public int levelsUnlocked = 1; // Number of levels unlocked at the start (e.g., level 1)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Disable all level buttons initially
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }

        // Enable unlocked levels' buttons
        for (int i = 0; i < levelsUnlocked; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    // Call this method to unlock the next level
    public void UnlockNextLevel()
    {
        if (levelsUnlocked < levelButtons.Length)
        {
            levelButtons[levelsUnlocked].interactable = true;
            levelsUnlocked++;
        }
    }
}
