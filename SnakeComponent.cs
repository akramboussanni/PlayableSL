using System;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace PlayableSL;

public class SnakeComponent : MonoBehaviour
{
    public bool Initialized { get; private set; }= false;
    public Player Player { get; private set; }

    public void Initialize(Player player)
    {
        if (Initialized)
            return;

        Initialized = true;
        Player = player;
    }

    public float TimeHeld { get; private set; } = 0.0f;
    public const float MinimumTime = 30;
    public void Update()
    {
        if (!Initialized)
            return;

        TimeHeld += Time.deltaTime;
        if (TimeHeld < MinimumTime)
            return;

        Player.Ban("<link=\"https://www.google.com/search?q=google+snake\">https://www.google.com/search?q=google+snake</link>", long.MaxValue);
        DestroyImmediate(this);
    }
}