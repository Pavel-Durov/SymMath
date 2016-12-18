using SymMath.Core.Const;
using System.Collections.Generic;
using System.Windows.Input;
using static SymMath.Core.Services.CharectersService;
using System;

namespace SymMath.Parsers
{
    class LogicToPhysicalKeysParser
    {
        private readonly static Dictionary<LogicKey, Key> _logicalToPhysicalkeysMap;
        private readonly static Dictionary<Key, LogicKey> _physicalToLogicalkeysMap;

        static LogicToPhysicalKeysParser()
        {
            _logicalToPhysicalkeysMap = new Dictionary<LogicKey, Key>();
            _physicalToLogicalkeysMap = new Dictionary<Key, LogicKey>();

            SetMapings();
        }

        private static void SetMapings()
        {
            _logicalToPhysicalkeysMap[LogicKey.A] = Key.A;
            _logicalToPhysicalkeysMap[LogicKey.B] = Key.B;
            _logicalToPhysicalkeysMap[LogicKey.D] = Key.D;
            _logicalToPhysicalkeysMap[LogicKey.E] = Key.E;
            _logicalToPhysicalkeysMap[LogicKey.G] = Key.G;
            _logicalToPhysicalkeysMap[LogicKey.Q] = Key.Q;
            _logicalToPhysicalkeysMap[LogicKey.R] = Key.R;
            _logicalToPhysicalkeysMap[LogicKey.S] = Key.S;
            _logicalToPhysicalkeysMap[LogicKey.W] = Key.W;
            _logicalToPhysicalkeysMap[LogicKey.X] = Key.X;
            _logicalToPhysicalkeysMap[LogicKey.Z] = Key.Z;


            _physicalToLogicalkeysMap[Key.A] = LogicKey.A;
            _physicalToLogicalkeysMap[Key.B] = LogicKey.B;
            _physicalToLogicalkeysMap[Key.D] = LogicKey.D;
            _physicalToLogicalkeysMap[Key.E] = LogicKey.E;
            _physicalToLogicalkeysMap[Key.G] = LogicKey.G;
            _physicalToLogicalkeysMap[Key.Q] = LogicKey.Q;
            _physicalToLogicalkeysMap[Key.R] = LogicKey.R;
            _physicalToLogicalkeysMap[Key.S] = LogicKey.S;
            _physicalToLogicalkeysMap[Key.W] = LogicKey.W;
            _physicalToLogicalkeysMap[Key.X] = LogicKey.X;
            _physicalToLogicalkeysMap[Key.Z] = LogicKey.Z;
        }

        public static Key Convert(LogicKey key)
        {
            return _logicalToPhysicalkeysMap[key];
        }

        public static LogicKey Convert(Key key)
        {
            return _physicalToLogicalkeysMap[key];
        }

    }
}
