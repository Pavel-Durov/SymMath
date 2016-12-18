/*
 * © Marcus van Houdt 2014
 */

using SymMath.Core.Services;
using SymMath.Parsers;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SymMath
{
    public static class LetterMappings
    {
        public static Dictionary<Key, LetterSelector> KeyToWindowMap { get; private set; }

        /// <summary>
        /// Constructs a popup window for each of the letter mappings.
        /// </summary>
        public static void InitializeWindowsAndBindings()
        {
            KeyToWindowMap = new Dictionary<Key, LetterSelector>();

            foreach (var kvp in CharectersService.KeysMap)
            {
                if (kvp.Value.Item1.Length > 0)
                {
                    var key = LogicToPhysicalKeysParser.Convert(kvp.Key);
                    KeyToWindowMap.Add(key, new LetterSelector(key, kvp.Value)); ;
                }
            }
        }
    }
}
