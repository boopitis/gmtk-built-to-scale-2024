using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerGun : MonoBehaviour
{
    public static PlayerGun Instance { get; private set; }
    
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;

    [SerializeField] private Transform firePointTransform;

    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private ScaleSO musicScaleSO;

    private Vector2 pointerPositionInput;

    private const float Delay = 0.3f;
    private bool attackBlocked;

    private int note;
    private int noteIndex;
    private int interval;

    private List<NoteSO> currentNoteSOList;

    private void Awake()
    {
        Instance = this;
        
        note = 0;
        noteIndex = 0;
    }

    private void Start()
    {
        currentNoteSOList = PlayerMusicScale.Instance.GetCurrentNoteSOList();
        PlayerMusicScale.Instance.OnCurrentNotesChanged += PlayerMusicScale_OnCurrentNotesChanged;
    }

    private void PlayerMusicScale_OnCurrentNotesChanged(object sender, EventArgs e)
    {
        currentNoteSOList = PlayerMusicScale.Instance.GetCurrentNoteSOList();
    }

    private void Update()
    {
        pointerPositionInput = GameInput.Instance.GetPlayerPointerPositionVector2InWorldSpace();
        Vector2 direction = (pointerPositionInput - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (PlayerVisual.Instance.GetLookDirection().x < 0)
        {
            scale.y = -1;
            scale.x = -1;
        }
        else if (PlayerVisual.Instance.GetLookDirection().x > 0)
        {
            scale.y = 1;
            scale.x = 1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z is > 0 and < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    public void Attack()
    {
        if (attackBlocked) return;

        attackBlocked = true;
        StartCoroutine(DelayAttack());

        var firedNoteSO = currentNoteSOList[noteIndex];
        Projectile.SpawnProjectile(projectilePrefabs[firedNoteSO.pitch], firePointTransform, transform.rotation);

        Debug.Log(noteIndex);
        Debug.Log(firedNoteSO.name);

        noteIndex++;
        if (noteIndex > currentNoteSOList.Count - 1) noteIndex = 0;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(Delay);
        attackBlocked = false;
    }
}