﻿using OpenTracker.Models.Modes;

namespace OpenTracker.Models.UndoRedo
{
    /// <summary>
    /// This is the class for an undoable action to change the item placement setting.
    /// </summary>
    public class ChangeItemPlacement : IUndoable
    {
        private readonly ItemPlacement _itemPlacement;
        private ItemPlacement _previousItemPlacement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemPlacement">
        /// The new item placement setting.
        /// </param>
        public ChangeItemPlacement(ItemPlacement itemPlacement)
        {
            _itemPlacement = itemPlacement;
        }

        /// <summary>
        /// Returns whether the action can be executed.
        /// </summary>
        /// <returns>
        /// A boolean representing whether the action can be executed.
        /// </returns>
        public bool CanExecute()
        {
            return true;
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public void Execute()
        {
            _previousItemPlacement = Mode.Instance.ItemPlacement;
            Mode.Instance.ItemPlacement = _itemPlacement;
        }

        /// <summary>
        /// Undoes the action.
        /// </summary>
        public void Undo()
        {
            Mode.Instance.ItemPlacement = _previousItemPlacement;
        }
    }
}
