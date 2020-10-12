using UnityEngine;

namespace Moonstorm.Cores.Components.Wyatt
{
    public class WyattComboScript : MonoBehaviour {
        public int count = 0;
        public void AddCombo(int combo) {
            count += combo;
            if (count == 4) {
                count = 0;
            }
        }
    }
}
