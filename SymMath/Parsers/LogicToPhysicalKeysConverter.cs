using SymMath.Core.Const;
using System.Collections.Generic;
using System.Windows.Input;

namespace SymMath.Parsers
{
    class LogicToPhysicalKeysConverter
    {
        static LogicToPhysicalKeysConverter()
        {
            _keysMap = new Dictionary<LogicKey, Key>();

            _keysMap[LogicKey.A] = Key.A;
            _keysMap[LogicKey.B] = Key.B;
            _keysMap[LogicKey.D] = Key.D;
            _keysMap[LogicKey.E] = Key.E;
            _keysMap[LogicKey.G] = Key.G;
            _keysMap[LogicKey.Q] = Key.Q;
            _keysMap[LogicKey.R] = Key.R;
            _keysMap[LogicKey.S] = Key.S;
            _keysMap[LogicKey.W] = Key.W;
            _keysMap[LogicKey.X] = Key.X;
            _keysMap[LogicKey.Z] = Key.Z;
        }

        static Dictionary<LogicKey, Key> _keysMap;

        public static Key Convert(LogicKey key)
        {
            return _keysMap[key];
        }


    }
}
