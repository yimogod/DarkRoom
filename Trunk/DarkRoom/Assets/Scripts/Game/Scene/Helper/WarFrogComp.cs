using UnityEngine;
using System.Collections;
using DarkRoom.Game;
using Rayman;

namespace Sword
{
    public class WarFrogComp : MonoBehaviour
    {
        /*private WarFrogMap _map;
        private CUnitEntity _hero;

        private Regulator _reg;

        private int _lastCol = -1;
        private int _lastRow = -1;

        public void SetData(CUnitEntity hero, WarFrogMap map)
        {
            _hero = hero;
            _map = map;

            Vector3 p = _hero.posColRow;
            _lastCol = (int) p.x;
            _lastRow = (int) p.z;
            _map.OpenFrog(_lastRow, _lastCol, _hero.fieldOfView);
        }

        void Start()
        {
            _reg = new Regulator(5);
        }

        void Update()
        {
            if (!_reg.Update()) return;
            if (_hero == null) return;

            Vector3 p = _hero.posColRow;
            int x = (int) p.x;
            int z = (int) p.z;

            if (_lastCol == x && _lastRow == z) return;

            _lastCol = x;
            _lastRow = z;

            _map.OpenFrog(_lastRow, _lastCol, _hero.fieldOfView);
        }

        void OnDestroy()
        {
            _map = null;
            _hero = null;
        }*/
    }

}