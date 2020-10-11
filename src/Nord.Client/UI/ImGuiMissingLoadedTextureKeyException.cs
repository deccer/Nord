﻿using System;

namespace Nord.Client.UI
{
    public class ImGuiMissingLoadedTextureKeyException : InvalidOperationException
    {
        public ImGuiMissingLoadedTextureKeyException(IntPtr textureId)
        {
            _textureId = textureId;
        }

        public override string Message => $"Could not find a texture with id {_textureId}, please check your bindings";

        private readonly IntPtr _textureId;
    }
}
