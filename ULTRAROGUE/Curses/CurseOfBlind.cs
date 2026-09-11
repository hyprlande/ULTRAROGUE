using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ultrarogue.Curses
{
    public class CurseOfBlind : BaseCurse
    {
        public override string CurseName => "Curse of The Blind";

        public override void OnApply()
        {
            base.OnApply();
            ItemPickup[] allPickups = Object.FindObjectsByType<ItemPickup>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var item in allPickups)
            {
                item.BecomeBlind();
            }
        }
    }
}
