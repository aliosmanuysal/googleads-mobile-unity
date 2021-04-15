// Copyright (C) 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
    public interface IInterstitialClient
    {
        // Ad event fired when the interstitial ad has been received.
        event EventHandler<EventArgs> OnAdLoaded;
        // Ad event fired when the interstitial ad has failed to load.
        event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad;
        // Ad event fired when the interstitial ad is estimated to have earned money.
        event EventHandler<AdValueEventArgs> OnPaidEvent;

        // Full screen content events
        event EventHandler<AdErrorClientEventArgs> OnAdFailedToPresentFullScreenContent;

        event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;

        event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;

        event EventHandler<EventArgs> OnAdDidRecordImpression;

        // Creates a interstitial ad.
        void CreateInterstitialAd();

        // Loads a interstitial ad.
        void LoadAd(string adUnitID, AdRequest request);

        // Shows the interstitial ad on the screen.
        void Show();

        // Returns ad request Response info client.
        IResponseInfoClient GetResponseInfoClient();

        // Destroys the interstitial ad.
        void DestroyInterstitial();
    }
}
