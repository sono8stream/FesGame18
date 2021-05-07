using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonoHelpers
{
    static class EnumHelper
    {
        public static string Name<Type>(this int index) where Type : struct
        {
            if (Enum.IsDefined(typeof(Type), index))
            {
                return Enum.GetName(typeof(Type), index);
            }
            else
            {
                return "";
            }
        }

        public static int GetTypeLength<Type>() where Type : struct
        {
            return Enum.GetNames(typeof(Type)).Length;
        }
    }
}