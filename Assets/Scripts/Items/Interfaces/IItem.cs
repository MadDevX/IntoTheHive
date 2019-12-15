using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    void UseItem(CharacterFacade facade);
    bool IsActive { get; }
}
