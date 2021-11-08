using System;
using System.Collections;
using GameLevels.Domain;
using GameLevels.Presentation;
using UnityEngine;
using Zenject;

public class SimpleLevelsList : MonoBehaviour
{
    [Inject] private GameLevelsUseCases levelsUseCases;

    [Inject] private ILevelItemImageRepository imageRepository;

    [SerializeField] private LevelsListPagerAdapter.LevelItemView levelPrefab;

    
    void OnEnable()
    {
        StartCoroutine(FillList());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator FillList()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        while (transform.childCount > 0)
        {
            yield return null;
        }

        SpawnLevelsList();
    }

    private void SpawnLevelsList()
    {
        var levels = levelsUseCases.GetLevels();
        foreach (var level in levels)
        {
            var levelItem = CreateLevelItem();
            var sprite = imageRepository.GetSpriteForLevel(level.ID);
            var rt = levelItem.transform as RectTransform;
            levelItem.Bind(level.Number, sprite, level.Completed, rt, level.ID);
            rt.sizeDelta = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.SetParent(transform);
        }
    }

    private LevelsListPagerAdapter.LevelItemView CreateLevelItem()
    {
        return Instantiate(levelPrefab);
    }
}