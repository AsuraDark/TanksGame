using TanksGame.Tanks.Unit;

namespace TanksGame.Tanks.Unit
{
    internal class UnitSystem
    {
        private List<IUnit> _units;
        private List<IUnit> _unitsToAdd;
        private List<IUnit> _unitsToRemove;

        public UnitSystem()
        {
            _units = new List<IUnit>();
            _unitsToAdd = new List<IUnit>();
            _unitsToRemove = new List<IUnit>();
        }

        public void UpdateUnits(float deltaTime)
        {
            foreach (var obj in _units)
            {
                obj.Update(deltaTime);
            }
        }

        public void ProcessUnitsChanges()
        {
            if (_unitsToAdd.Count > 0 || _unitsToRemove.Count > 0)
            {
                foreach (var obj in _unitsToAdd)
                {
                    _units.Add(obj);
                }
                _unitsToAdd.Clear();

                foreach (var obj in _unitsToRemove)
                {
                    _units.Remove(obj);

                }
                _unitsToRemove.Clear();
            }
        }

        public void AddUnit(IUnit obj)
        {
            _unitsToAdd.Add(obj);
        }

        public void RemoveUnit(IUnit obj)
        {
            _unitsToRemove.Add(obj);

        }

        public List<IUnit> GetUnits()
        {
            return _units;
        }

        public void Clear()
        {
            _units.Clear();
            _unitsToAdd.Clear();
            _unitsToRemove.Clear();
        }
    }
}
