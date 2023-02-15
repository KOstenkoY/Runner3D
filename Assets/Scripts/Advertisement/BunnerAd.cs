using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BunnerAd : MonoBehaviour
{
    private string _bannerUnitId = "ca-app-pub-3940256099942544/6300978111";

    private BannerView _bannerView;

    private void OnEnable()
    {
        _bannerView = new BannerView(_bannerUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _bannerView.LoadAd(adRequest);
    }
    private IEnumerator ShowBanner()
    {
        yield return new WaitForSeconds(1f);

        _bannerView.Show();
    }
}
