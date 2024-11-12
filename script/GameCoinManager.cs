using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCoinManager : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform coinParent;
    [SerializeField] Transform endPos;
    [SerializeField] float duration;
    [SerializeField] TextMeshProUGUI coinText;
    private int totalCoin;
    [SerializeField] float minX, maxX, minY, maxY;
    [SerializeField] AudioSource coinSound;
    private List<GameObject> coinSpawnList = new();
    Tween myTween;


    private void Start()
    {
        totalCoin = 0;

        for (int i = 0; i < 50; i++)
        {
            coinSpawnList.Add(Instantiate(coinPrefab, coinParent));
            coinSpawnList[i].SetActive(false);
        }

    }

    public void CollectCoins(int coinAmount, Vector3 spawnPoint, int coinScore)
    {
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 vec = Camera.main.WorldToScreenPoint(spawnPoint);

            float xPos = vec.x + Random.Range(minX, maxX);
            float yPos = vec.y + Random.Range(minY, maxY);

            GameObject coinSpawn = CoinPool();
            coinSpawn.transform.position = new Vector2(xPos, yPos);

            coinSpawn.transform.DOPunchPosition(new Vector3(0, 35, 0), Random.Range(0, 1f)).SetEase(Ease.InExpo).OnComplete(() =>
            {
                coinSpawn.transform.DOMove(endPos.position, duration).SetEase(Ease.InExpo).OnComplete(() =>
                {

                    coinSound.PlayOneShot(coinSound.clip);

                    coinSpawn.SetActive(false);

                    myTween ??= endPos.transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.1f).SetEase(Ease.InOutElastic).OnComplete(() =>
                    {
                        endPos.localScale = Vector3.one;

                        myTween = null;
                    });

                    totalCoin += coinScore;
                    coinText.text = totalCoin.ToString("N0");
                });
                coinSpawn.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), duration).SetEase(Ease.InExpo).OnComplete(() =>
                coinSpawn.transform.localScale = Vector3.one);
            });
        }
    }

    private GameObject CoinPool()
    {
        foreach (var item in coinSpawnList)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                item.transform.SetLocalPositionAndRotation(Vector3.one, Quaternion.identity);
                return item;
            }
        }
        return null;
    }
}

