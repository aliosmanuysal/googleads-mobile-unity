// Copyright (C) 2020 Google LLC.
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

using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Unity
{
    public class RewardedAdClient : RewardingAdBaseClient, IRewardedAdClient
    {
        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad;

        public event EventHandler<Reward> OnUserEarnedReward;

        public event EventHandler<AdValueEventArgs> OnPaidEvent;

        public event EventHandler<AdErrorClientEventArgs> OnAdFailedToPresentFullScreenContent;

        public event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;

        public event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;

        public event EventHandler<EventArgs> OnAdDidRecordImpression;

        // Load a rewarded ad.
        public override void LoadAd(string adUnitId, AdRequest request)
        {
            base.LoadAd(adUnitId, request);
            if (OnAdDidPresentFullScreenContent != null)
            {
                OnAdDidPresentFullScreenContent += OnAdDidPresentFullScreenContent;
            }
            if (OnAdDidDismissFullScreenContent != null)
            {
                OnAdDidDismissFullScreenContent += OnAdDidDismissFullScreenContent;
            }
            if (OnAdFailedToPresentFullScreenContent != null)
            {
                OnAdFailedToPresentFullScreenContent += OnAdFailedToPresentFullScreenContent;
            }
        }

        // Creates a rewarded ad.
        public void CreateRewardedAd(string adUnitId)
        {

        }

        public void DestroyRewardedAd()
        {
            
        }

    }
}
