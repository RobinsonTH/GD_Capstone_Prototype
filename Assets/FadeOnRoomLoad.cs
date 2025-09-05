using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnRoomLoad : MonoBehaviour
{
    Room room;

    [SerializeField] float targetAlpha;
    [SerializeField] float duration;
    [SerializeField] AnimationCurve lerp;

    private void Awake()
    {
        room = GetComponentInParent<Room>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        room.OnRoomLoad += StartFade;
    }

    private void OnDisable()
    {
        room.OnRoomLoad -= StartFade;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartFade()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        float currentAlpha = color.a;
        float alphaDelta = targetAlpha - currentAlpha;


        float t = 0;
        while (t <= duration)
        {
            t += Time.deltaTime;
            color.a = currentAlpha + (alphaDelta * lerp.Evaluate(t / duration));
            sprite.color = color;
            yield return null;
        }
    }
}
