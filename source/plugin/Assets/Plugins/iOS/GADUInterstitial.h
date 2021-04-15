// Copyright 2014 Google Inc. All Rights Reserved.

#import <Foundation/Foundation.h>

#import <GoogleMobileAds/GoogleMobileAds.h>

#import "GADUTypes.h"

/// A wrapper around GADInterstitial. Includes the ability to create GADInterstitial objects, load
/// them with ads, show them, and listen for ad events.
@interface GADUInterstitial : NSObject

/// Initializes a GADUInterstitial.
- (id)initWithInterstitialClientReference:(GADUTypeInterstitialClientRef *)interstitialClient;

/// The interstitial ad.
@property(nonatomic, strong) GADInterstitialAd *interstitialAd;

/// A reference to the Unity interstitial client.
@property(nonatomic, assign) GADUTypeInterstitialClientRef *interstitialClient;

/// The ad received callback into Unity.
@property(nonatomic, assign) GADUInterstitialDidReceiveAdCallback adLoadedCallback;

/// The ad failed callback into Unity.
@property(nonatomic, assign) GADUInterstitialDidFailToReceiveAdWithErrorCallback adFailedCallback;

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

/// The paid event callback into Unity.
@property(nonatomic, assign) GADUInterstitialPaidEventCallback paidEventCallback;

// Returns the interstitial ad response info.
@property(nonatomic, readonly, copy) GADResponseInfo *responseInfo;

/// Makes an ad request. Additional targeting options can be supplied with a request object.
- (void)loadWithAdUnit:(NSString *)adUnit request:(GADRequest *)request;

/// Shows the interstitial ad.
- (void)show;

@end
