using NaughtyAttributes;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] 
    private Faction _faction = Faction.Environment;
    [SerializeField, ShowIf("isPlayer")] 
    private bool    _controllable = true;
    [SerializeField, MinMaxSlider(1.0f, 60.0f)]
    private Vector2 _emoteCooldown = new Vector2(10.0f, 30.0f);

    public bool isPlayer => _faction == Faction.Player;

    float       _emoteTimer;
    Animator    _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _emoteTimer = _emoteCooldown.Random();
    }

    // Update is called once per frame
    void Update()
    {
        _emoteTimer -= Time.deltaTime;
        if (_emoteTimer < 0)
        {
            _emoteTimer = _emoteCooldown.Random();
            _animator.SetTrigger("Emote");
        }
    }
}
