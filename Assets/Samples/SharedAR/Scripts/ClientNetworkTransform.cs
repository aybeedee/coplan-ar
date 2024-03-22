// Copyright 2023 Niantic, Inc. All Rights Reserved.

using Unity.Netcode.Components;
using UnityEngine;

// TODO: Wrap with Niantic.Lightship.AR.Samples namespace
public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
