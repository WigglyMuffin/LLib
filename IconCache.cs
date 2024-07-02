﻿using System;
using System.Collections.Generic;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Plugin.Services;

namespace LLib;

public sealed class IconCache : IDisposable
{
    private readonly ITextureProvider _textureProvider;
    private readonly Dictionary<uint, ISharedImmediateTexture> _textureWraps = new();

    public IconCache(ITextureProvider textureProvider)
    {
        _textureProvider = textureProvider;
    }

    public ISharedImmediateTexture GetIcon(uint iconId)
    {
        if (_textureWraps.TryGetValue(iconId, out ISharedImmediateTexture? iconTex))
            return iconTex;

        iconTex = _textureProvider.GetFromGameIcon(new GameIconLookup(iconId));
        _textureWraps[iconId] = iconTex;
        return iconTex;
    }

    public void Dispose()
    {
        _textureWraps.Clear();
    }
}
