﻿using UnityEngine;

namespace PC2D
{
    public class Input
    {

        public const string HORIZONTAL = "MoveX";
        public const string VERTICAL = "MoveY";
        public const string JUMP = "Jump";
        public const string DASH = "Fire1";
        public const string ATTACK = "Attack";
		public const string SUBATTACK = "SubAttack";
    }

    public class Globals
    {
        // Input threshold in order to take effect. Arbitarily set.
        public const float INPUT_THRESHOLD = 0.1f;//変更0.5→0.1 動き始めるのが速くなる
        public const float FAST_FALL_THRESHOLD = 0.5f;

        public const int ENV_MASK = 0x100;

        public const string PACKAGE_NAME = "PC2D";

        public const float MINIMUM_DISTANCE_CHECK = 0.01f;

        public static int GetFrameCount(float time)
        {
            float frames = time / Time.fixedDeltaTime;
            int roundedFrames = Mathf.RoundToInt(frames);

            if (Mathf.Approximately(frames, roundedFrames))
            {
                return roundedFrames;
            }

            return Mathf.CeilToInt(frames);

        }
    }
}
