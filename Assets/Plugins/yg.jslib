mergeInto(LibraryManager.library, {
  SDKInit: function () {
    if (typeof ysdk === 'undefined') {
        return false;
    }
    else {
        return true;
    }
  },

  PlayerInit: function () {
    if (typeof player === 'undefined') {
        return false;
    }
    else {
        return true;
    }
  },

  AuthCheck: function () {
    if (player.getMode() === 'lite') {
       return false; 
    }
    else {
       return true;
    }
  },

  GameReady: function () {
    ysdk.features.LoadingAPI.ready();
  },

  GetLang: function () {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  IsMobilePlatform: function () {
    var userAgent = navigator.userAgent;
        isMobile = (
                    /\b(BlackBerry|webOS|iPhone|IEMobile)\b/i.test(userAgent) ||
                    /\b(Android|Windows Phone|iPad|iPod)\b/i.test(userAgent) ||
                    // iPad on iOS 13 detection
                    (userAgent.includes("Mac") && "ontouchend" in document)
                );
    return isMobile;
  },

  ShowFullscreenAd : function() {
    console.log("Show ad request...");
    ysdk.adv.showFullscreenAdv({
        callbacks: {
            onClose: function(wasShown) {
                console.log("Ad shown");
                myGameInstance.SendMessage("--YANDEX_GAMES--", "AdShown");
            },
            onError: function(error) {
                console.log("Ad error:", error);
                myGameInstance.SendMessage("--YANDEX_GAMES--", "AdShown");
            }
        }
    })
  },

  ShowRewardedAd : function() {
    console.log("Rewarded ad request...");
    ysdk.adv.showRewardedVideo({
        callbacks: {
            onRewarded: () => {
                console.log('REWARDED');
                myGameInstance.SendMessage("--YANDEX_GAMES--", "Rewarded");
            },
            onClose: () => {
                console.log('Rewarded ad closed.');
                myGameInstance.SendMessage("--YANDEX_GAMES--", "RewardedClosed");
            }, 
            onError: (e) => {
                console.log('Error while open rewarded ad:', e);
            }
        }
    })
  },

  CheckPromoFlag : function () {
    ysdk.getFlags().then(flags => {
        if (flags.promo_active === "True") {
            myGameInstance.SendMessage("--YANDEX_GAMES--", "PromoActive");
        }
    });
  },

  SaveToLb : function (score) {
    lb.setLeaderboardScore('psscore', score);
  },

  GetPlayerName : function () {
    myGameInstance.SendMessage("--YANDEX_GAMES--", "SetPlayerName", player.getName());
  },

  GetPlayerAvatarURL : function () {
    myGameInstance.SendMessage("--YANDEX_GAMES--", "SetPlayerAvatarURL", player.getPhoto("medium"));
  },

  AuthRequest : function () {
     if (player.getMode() === 'lite') {
            // Игрок не авторизован.
            ysdk.auth.openAuthDialog().then(() => {
                    myGameInstance.SendMessage("--YANDEX_GAMES--", "AuthSuccessful");
                    initPlayer().catch(err => {
                        // Ошибка при инициализации объекта Player.
                    });
                }).catch(() => {
                    // Игрок не авторизован.
                });
        }
  },

  SaveCloudData : function (data) {
    var dataConverted = UTF8ToString(data);
    var dataObj = JSON.parse(dataConverted);
    player.setData(dataObj);
    myGameInstance.SendMessage("--YANDEX_GAMES--", "DataSaved");
  },

  LoadCloudData : function () {
    player.getData().then(_data => {
        const dataJSON = JSON.stringify(_data);
        myGameInstance.SendMessage("--YANDEX_GAMES--", "DataLoaded", dataJSON);
    });
  },
});