/*
 * © Marcus van Houdt 2014
 */

using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SymMath
{
    public static class LetterMappings
    {
        public static readonly Dictionary<Key, Tuple<Char[], Char[]>> KeysToSymbols = new Dictionary<Key, Tuple<Char[], Char[]>>
        {
         { Key.A, Tuple.Create(CharacterConst.ARROWS_CHARS_PART1, CharacterConst.ARROWS_CHARS_PART2)},
         { Key.D, Tuple.Create(CharacterConst.DOUBLE_STRUCK_CHARS, CharacterConst.DOUBLE_STRUCK_CHARS)},
         { Key.E, Tuple.Create(CharacterConst.BASIC_MATH_PART1, CharacterConst.BASIC_MATH_PART2)},
         { Key.G, Tuple.Create(CharacterConst.GREEK_LETTERS_LOWERCASE, CharacterConst.GREEK_LETTERS_UPPERCASE)},
         { Key.Q, Tuple.Create(CharacterConst.RELATIONS_CHARS, CharacterConst.NEGATED_RELATIONS_CHARS)},
         { Key.R, Tuple.Create(CharacterConst.ADVANCED_RELATIONAL_OPERATIONS, CharacterConst.ADVANCED_RELATIONAL_OPERATIONS)},
         { Key.S, Tuple.Create(CharacterConst.NEGATED_ARROWS_CHARS, CharacterConst.NEGATED_ARROWS_CHARS)},
         { Key.W, Tuple.Create(CharacterConst.BINARY_OPERATIONS, CharacterConst.BINARY_OPERATIONS)},
         { Key.X, Tuple.Create(CharacterConst.BASIC_N_ARRAY_OPERATORS, CharacterConst.BASIC_N_ARRAY_OPERATORS)},
         { Key.Z, Tuple.Create(CharacterConst.GEOMETRY_CHARS, CharacterConst.GEOMETRY_CHARS )}
      };

        public static void UpdateKey(Key key, Char[] lowerCase, Char[] upperCase)
        {
            //if (lowerCase.Length != upperCase.Length) throw new ArgumentException("lower and upper case letter arrays must be of equal length");
            var pair = Tuple.Create(lowerCase, upperCase);
            KeysToSymbols[key] = pair;

            if (lowerCase.Length > 0)
            {
                KeyToWindowMap[key] = new LetterSelector(key, pair);
            }
        }

        public static Dictionary<Key, LetterSelector> KeyToWindowMap { get; private set; }

        /// <summary>
        /// Constructs a popup window for each of the letter mappings.
        /// </summary>
        public static void InitializeWindowsAndBindings()
        {
            KeyToWindowMap = new Dictionary<Key, LetterSelector>();

            foreach (var kvp in KeysToSymbols)
            {
                if (kvp.Value.Item1.Length > 0)
                    KeyToWindowMap.Add(kvp.Key, new LetterSelector(kvp.Key, kvp.Value)); ;
            }
        }
    }
}
