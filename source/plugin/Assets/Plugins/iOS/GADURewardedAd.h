// Copyright 2018 Google Inc. All Rights Reserved.

#import <Foundation/Foundation.h>

#import <GoogleMobileAds/GoogleMobileAds.h>

#import "GADUTypes.h"

@interface GADURewardedAd : NSObject

/// Initializes a GADURewardedAd.
- (instancetype)initWithRewardedAdClientReference:(GADUTypeRewardedAdClientRef *)rewardedAdClient;

/// The rewarded ad.
@property(nonatomic, strong) GADRewardedAd *rewardedAd;

/// A reference to the Unity rewarded ad client.
@property(nonatomic, assign) GADUTypeRewardedAdClientRef *rewardedAdClient;

/// The ad received callback into Unity.
@property(nonatomic, assign) GADURewardedAdDidReceiveAdCallback adLoadedCallback;

/// The ad request failed callback into Unity.
@property(nonatomic, assign)
    GADURewardedAdDidFailToReceiveAdWithErrorCallback adFailedToLoadCallback;

/// The ad failed to present full screen content callback into Unity.
@property(nonatomic, assign)
GADUFailedToPresentFullScreenContentCallback adFailedToPresentFullScreenContentCallback;

/// The ad presented full screen content callback into Unity.
@property(nonatomic, assign)
GADUDidPresentFullScreenContentCallback adDidPresentFullScreenContentCallback;

/// The ad dismissed full screen content callback into Unity.
@property(nonatomic, assign)
GADUDidDismissFullScreenContentCallback adDidDismissFullScreenContentCallback;

/// The ad impression callback into Unity.
@property(nonatomic, assign)
GADUDidRecordImpressionCallback adDidRecordImpressionCallback;

/// The user was rewarded callback into Unity.
@property(nonatomic, assign) GADUUserEarnedRewardCallback didEarnRewardCallback;

/// The paid event callback into Unity.
@property(nonatomic, assign) GADURewardedAdPaidEventCallback paidEventCallback;

// Returns the rewarded ad response info.
@property(nonatomic, readonly, copy) GADResponseInfo *responseInfo;

/// Makes an ad request. Additional targeting options can be supplied with a request object.
- (void)loadWithAdUnitID:(NSString *)adUnit request:(GADRequest *)request;

/// Shows the rewarded ad.
- (void)show;

/// Options specified for server-to-server user reward verification.
- (void)setServerSideVerificationOptions:(GADServerSideVerificationOptions *)options;

@end
