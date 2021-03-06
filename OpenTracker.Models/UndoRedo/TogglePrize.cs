﻿using OpenTracker.Models.Sections;

namespace OpenTracker.Models.UndoRedo
{
    /// <summary>
    /// This is the class for an undoable action to toggle a dungeon prize.
    /// </summary>
    public class TogglePrize : IUndoable
    {
        private readonly ISection _section;
        private readonly bool _force;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="section">
        /// The boss section data for the prize to be toggled.
        /// </param>
        /// <param name="force">
        /// A boolean representing whether to bypass logic on the prize.
        /// </param>
        public TogglePrize(ISection section, bool force)
        {
            _section = section;
            _force = force;
        }

        /// <summary>
        /// Returns whether the action can be executed.
        /// </summary>
        /// <returns>
        /// A boolean representing whether the action can be executed.
        /// </returns>
        public bool CanExecute()
        {
            return _section.CanBeUncleared() || _section.CanBeCleared(_force);
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public void Execute()
        {
            if (_section.IsAvailable())
            {
                _section.Available = 0;
            }
            else
            {
                _section.Available = 1;
            }
        }

        /// <summary>
        /// Undoes the action.
        /// </summary>
        public void Undo()
        {
            if (_section.IsAvailable())
            {
                _section.Available = 0;
            }
            else
            {
                _section.Available = 1;
            }
        }
    }
}
