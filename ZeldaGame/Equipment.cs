using System.Collections.Generic;

namespace ZeldaGame
{
    public class Equipment
    {
        private Dictionary<EquipmentSlots, IItem> _slots = new Dictionary<EquipmentSlots, IItem>();
        private Dictionary<string, IItem> _items = new Dictionary<string, IItem>();

        public IItem this[string itemName]
        {
            get
            {
                IItem item;

                _items.TryGetValue(itemName, out item);

                return item;
            }
            set
            {
                _items[itemName] = value;
            }
        }

        public IItem this[EquipmentSlots slot]
        {
            get
            {
                IItem item;

                _slots.TryGetValue(slot, out item);
                
                return item;
            }
            set
            {
                _slots[slot] = value;
            }
        }
    }
}
