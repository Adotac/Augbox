using System;
using UnityEngine;

namespace Augbox
{
    public interface IUIPanel
    {
        string Id { get; }

        GameObject gameObject { get; }

        bool UIResult { get; }

        void DoShow();

        void DoHide(bool result = true);
    }
}
