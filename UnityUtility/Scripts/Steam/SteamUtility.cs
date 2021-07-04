using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SteamUtility
{

    public static Sprite GetAvatar(CSteamID steamID)
    {
        var avatar = SteamFriends.GetLargeFriendAvatar(steamID);

        Texture2D ret = null;
        uint ImageWidth;
        uint ImageHeight;
        bool bIsValid = SteamUtils.GetImageSize(avatar, out ImageWidth, out ImageHeight);

        if (bIsValid)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];

            bIsValid = SteamUtils.GetImageRGBA(avatar, Image, (int)(ImageWidth * ImageHeight * 4));
            if (bIsValid)
            {
                ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
                ret.LoadRawTextureData(Image);
                ret.Apply();

                Sprite sprite = Sprite.Create(ret, new Rect(0, 0, ret.width, ret.height), new Vector2(0.5f, 0.5f), 100);
                return sprite;
            }
        }

        return null;
    }

}
