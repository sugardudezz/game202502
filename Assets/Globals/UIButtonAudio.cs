using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtonAudio : MonoBehaviour
{
    [Header("Default Sounds")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Sounds")] public AudioClip[] actionAssignSounds;
    
    [Header("Mixer")]
    public AudioMixer mixer;

    private AudioSource audioSource;
    private AudioSource hoverAudioSource;
    
    private static UIButtonAudio instance;
    
    private void Awake()
    {
        if (!instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = mixer.outputAudioMixerGroup;
            
            hoverAudioSource = gameObject.AddComponent<AudioSource>();
            hoverAudioSource.outputAudioMixerGroup = mixer.outputAudioMixerGroup;
            hoverAudioSource.volume = 0.6f;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        AddEventsToAllButtons();
        SceneManager.sceneLoaded += SceneChanged;
        EnemyAction.OnUIHover += PlayHoverSound;
        PlayerAction.OnUIHover += PlayHoverSound;
        PlayerAction.OnActionAssignedStatic += (data) =>
        {
            if (data.ID == 0) return;
            audioSource.PlayOneShot(actionAssignSounds[data.ID - 1]);
        };
    }

    void SceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        AddEventsToAllButtons();
    }

    void AddEventsToAllButtons()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        print(buttons);
        foreach (var btn in buttons)
        {
            var disable = btn.GetComponent<DisableButtonSFX>();
            if (disable != null) continue;

            AddButtonEvents(btn);
        }
    }

    void AddButtonEvents(Button btn)
    {
        EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = btn.gameObject.AddComponent<EventTrigger>();

        AddEvent(trigger, EventTriggerType.PointerEnter, PlayHoverSound);

        AddEvent(trigger, EventTriggerType.PointerDown, () =>
        {
            if (clickSound) audioSource.PlayOneShot(clickSound);
        });
    }

    private void PlayHoverSound()
    {
        if (!hoverSound) return;

        hoverAudioSource.Stop();
        hoverAudioSource.PlayOneShot(hoverSound);
    }

    void AddEvent(EventTrigger trigger, EventTriggerType eventType, System.Action action)
    {
        var entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }
}