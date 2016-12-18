using SymMath.Core.Const;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SymMath.Core.Services
{
    public static class CharectersService
    {
        public enum LogicKey { A, B, D, E, G, Q, R, S, W, X, Z }

        public static readonly Dictionary<LogicKey, Tuple<char[], char[]>> KeysMap = new Dictionary<LogicKey, Tuple<Char[], Char[]>>
        {
             { LogicKey.A, Tuple.Create(CharacterConst.ARROWS_CHARS_PART1, CharacterConst.ARROWS_CHARS_PART2)},
             { LogicKey.D, Tuple.Create(CharacterConst.DOUBLE_STRUCK_CHARS, CharacterConst.DOUBLE_STRUCK_CHARS)},
             { LogicKey.E, Tuple.Create(CharacterConst.BASIC_MATH_PART1, CharacterConst.BASIC_MATH_PART2)},
             { LogicKey.G, Tuple.Create(CharacterConst.GREEK_LETTERS_LOWERCASE, CharacterConst.GREEK_LETTERS_UPPERCASE)},
             { LogicKey.Q, Tuple.Create(CharacterConst.RELATIONS_CHARS, CharacterConst.NEGATED_RELATIONS_CHARS)},
             { LogicKey.R, Tuple.Create(CharacterConst.ADVANCED_RELATIONAL_OPERATIONS, CharacterConst.ADVANCED_RELATIONAL_OPERATIONS)},
             { LogicKey.S, Tuple.Create(CharacterConst.NEGATED_ARROWS_CHARS, CharacterConst.NEGATED_ARROWS_CHARS)},
             { LogicKey.W, Tuple.Create(CharacterConst.BINARY_OPERATIONS, CharacterConst.BINARY_OPERATIONS)},
             { LogicKey.X, Tuple.Create(CharacterConst.BASIC_N_ARRAY_OPERATORS, CharacterConst.BASIC_N_ARRAY_OPERATORS)},
             { LogicKey.Z, Tuple.Create(CharacterConst.GEOMETRY_CHARS, CharacterConst.GEOMETRY_CHARS )}
        };

        public static void OnCharSelected(LogicKey key, int index)
        {
            ReplaceCharLRU(index, KeysMap[key]);
        }

        private static void ReplaceCharLRU(int index, Tuple<char[], char[]> charCollection)
        {
            var charArr = ((char[])charCollection.Item1);
            var usedChar = charArr[index];
            charArr[index] = charArr[0];
            charArr[0] = usedChar;
        }
    }
}
