#if UNITY_IOS
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.iOS
{
    public class RewardedAdClient : IRewardedAdClient, IDisposable
    {
        private IntPtr rewardedAdClientPtr;
        private IntPtr rewardedAdPtr;

#region rewarded ad callback types

        internal delegate void GADURewardedAdLoadedCallback(IntPtr rewardedAdClient);

        internal delegate void GADURewardedAdFailedToLoadCallback(IntPtr rewardedAdClient, IntPtr error);

        internal delegate void GADUUserEarnedRewardCallback(
            IntPtr rewardedAdClient, string rewardType, double rewardAmount);

        internal delegate void GADURewardedAdPaidEventCallback(
            IntPtr rewardedAdClient, int precision, long value, string currencyCode);

#endregion

#region full screen content callback types

        internal delegate void GADUFailedToPresentFullScreenContentCallback(IntPtr rewardedAdClient, IntPtr error);

        internal delegate void GADUDidPresentFullScreenContentCallback(IntPtr rewardedAdClient);

        internal delegate void GADUDidDismissFullScreenContentCallback(IntPtr rewardedAdClient);

        internal delegate void GADUDidRecordImpressionCallback(IntPtr rewardedAdClient);

#endregion

        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad;

        public event EventHandler<Reward> OnUserEarnedReward;

        public event EventHandler<AdValueEventArgs> OnPaidEvent;

        public event EventHandler<AdErrorClientEventArgs> OnAdFailedToPresentFullScreenContent;

        public event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;

        public event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;

        public event EventHandler<EventArgs> OnAdDidRecordImpression;

#region IRewardedAdClient implementation

        public void CreateRewardedAd()
        {
            this.rewardedAdClientPtr = (IntPtr)GCHandle.Alloc(this);
            this.rewardedAdPtr = Externs.GADUCreateRewardedAd(this.rewardedAdClientPtr);

            Externs.GADUSetRewardedAdCallbacks(
                this.rewardedAdPtr,
                RewardedAdLoadedCallback,
                RewardedAdFailedToLoadCallback,
                RewardedAdUserDidEarnRewardCallback,
                RewardedAdPaidEventCallback,
                AdFailedToPresentFullScreenContentCallback,
                AdDidPresentFullScreenContentCallback,
                AdDidDismissFullScreenContentCallback,
                AdDidRecordImpressionCallback);
        }

        public void LoadAd(string adUnitID, AdRequest request) {
            IntPtr requestPtr = Utils.BuildAdRequest(request);
            Externs.GADULoadRewardedAd(this.rewardedAdPtr, adUnitID, requestPtr);
            Externs.GADURelease(requestPtr);
        }

        // Show the rewarded ad on the screen.
        public void Show()
        {
            Externs.GADUShowRewardedAd(this.rewardedAdPtr);
        }

        // Sets the server side verification options
        public void SetServerSideVerificationOptions(ServerSideVerificationOptions serverSideVerificationOptions)
        {
            IntPtr optionsPtr = Utils.BuildServerSideVerificationOptions(serverSideVerificationOptions);
            Externs.GADURewardedAdSetServerSideVerificationOptions(this.rewardedAdPtr, optionsPtr);
            Externs.GADURelease(optionsPtr);
        }

        // Returns the reward item for the loaded rewarded ad.
        public Reward GetRewardItem()
        {
            string type = Externs.GADURewardedAdGetRewardType(this.rewardedAdPtr);
            double amount = Externs.GADURewardedAdGetRewardAmount(this.rewardedAdPtr); ;
            return new Reward()
            {
                Type = type,
                Amount = amount
            };
        }

        public IResponseInfoClient GetResponseInfoClient()
        {
            return new ResponseInfoClient(ResponseInfoClientType.AdLoaded, this.rewardedAdPtr);
        }

        // Destroys the rewarded ad.
        public void DestroyRewardedAd()
        {
            this.rewardedAdPtr = IntPtr.Zero;
        }

        public void Dispose()
        {
            this.DestroyRewardedAd();
            ((GCHandle)this.rewardedAdClientPtr).Free();
        }

        ~RewardedAdClient()
        {
            this.Dispose();
        }

#endregion

#region rewarded ad callback methods

        [MonoPInvokeCallback(typeof(GADURewardedAdLoadedCallback))]
        private static void RewardedAdLoadedCallback(IntPtr rewardedAdClient)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(rewardedAdClient);
            if (client.OnAdLoaded != null)
            {
                client.OnAdLoaded(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardedAdFailedToLoadCallback))]
        private static void RewardedAdFailedToLoadCallback(
            IntPtr rewardedAdClient, IntPtr error)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(rewardedAdClient);
            if (client.OnAdFailedToLoad != null)
            {
                LoadAdErrorClientEventArgs args = new LoadAdErrorClientEventArgs()
                {
                    LoadAdErrorClient = new LoadAdErrorClient(error)
                };
                client.OnAdFailedToLoad(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADUUserEarnedRewardCallback))]
        private static void RewardedAdUserDidEarnRewardCallback(
            IntPtr rewardedAdClient, string rewardType, double rewardAmount)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(
                rewardedAdClient);
            if (client.OnUserEarnedReward != null)
            {
                Reward args = new Reward()
                {
                    Type = rewardType,
                    Amount = rewardAmount
                };
                client.OnUserEarnedReward(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardedAdPaidEventCallback))]
        private static void RewardedAdPaidEventCallback(
            IntPtr rewardedAdClient, int precision, long value, string currencyCode)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(rewardedAdClient);
            if (client.OnPaidEvent != null)
            {
                AdValue adValue = new AdValue()
                {
                    Precision = (AdValue.PrecisionType)precision,
                    Value = value,
                    CurrencyCode = currencyCode
                };
                AdValueEventArgs args = new AdValueEventArgs()
                {
                    AdValue = adValue
                };

                client.OnPaidEvent(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADUFailedToPresentFullScreenContentCallback))]
        private static void AdFailedToPresentFullScreenContentCallback(IntPtr rewardedAdClient, IntPtr error)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(rewardedAdClient);
            if (client.OnAdFailedToPresentFullScreenContent != null)
            {
                AdErrorClientEventArgs args = new AdErrorClientEventArgs()
                {
                    AdErrorClient = new AdErrorClient(error)
                };
                client.OnAdFailedToPresentFullScreenContent(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADUDidPresentFullScreenContentCallback))]
        private static void AdDidPresentFullScreenContentCallback(IntPtr rewardedAdClient)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(rewardedAdClient);
            if (client.OnAdDidPresentFullScreenContent != null)
            {
                client.OnAdDidPresentFullScreenContent(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADUDidDismissFullScreenContentCallback))]
        private static void AdDidDismissFullScreenContentCallback(IntPtr rewardedAdClient)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(rewardedAdClient);
            if (client.OnAdDidDismissFullScreenContent != null)
            {
                client.OnAdDidDismissFullScreenContent(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADUDidRecordImpressionCallback))]
        private static void AdDidRecordImpressionCallback(IntPtr rewardedAdClient)
        {
            RewardedAdClient client = IntPtrToRewardedAdClient(rewardedAdClient);
            if (client.OnAdDidRecordImpression != null)
            {
                client.OnAdDidRecordImpression(client, EventArgs.Empty);
            }
        }

        private static RewardedAdClient IntPtrToRewardedAdClient(
            IntPtr rewardedAdClient)
        {
            GCHandle handle = (GCHandle)rewardedAdClient;
            return handle.Target as RewardedAdClient;
        }

#endregion
    }
}
#endif
