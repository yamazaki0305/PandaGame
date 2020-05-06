using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UGuiLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 押しっぱなし時に呼び出すイベント
    /// </summary>
    public UnityEvent onLongPress = new UnityEvent();
    /// <summary>
    /// 押しっぱなし判定の間隔（この間隔毎にイベントが呼ばれる）
    /// </summary>
    public float intervalAction = 0.3f;
    // 押下開始時にもイベントを呼び出すフラグ
    public bool callEventFirstPress;

    // 次の押下判定時間
    float nextTime = 0f;
    
    // 追加した要素
    bool longpressflg = true;
    private Animator m_animator;
    AudioSource audioData;

    /// <summary>
    /// 押下状態
    /// </summary>
    public bool pressed
    {
        get;
        private set;
    }

    void Start()
    {
        m_animator = GetComponent<Animator>();
        audioData = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (pressed && nextTime < Time.realtimeSinceStartup && longpressflg)
        {
            longpressflg = true;
            audioData.Play(0);
            m_animator.SetTrigger("Pressed");
            onLongPress.Invoke();
            nextTime = Time.realtimeSinceStartup + intervalAction;
            longpressflg = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        if (callEventFirstPress)
        {
            onLongPress.Invoke();
        }
        nextTime = Time.realtimeSinceStartup + intervalAction;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        longpressflg = true;

        pressed = false;
    }
}