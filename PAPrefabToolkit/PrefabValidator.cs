using PAPrefabToolkit.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PAPrefabToolkit
{
    internal class PrefabValidator
    {
        private readonly Prefab prefab;

        public PrefabValidator(Prefab prefab)
        {
            this.prefab = prefab;
        }

        public void Validate()
        {
            ValidatePrefabMeta();
            ValidateId();
            ValidateEvents();
        }

        private void ValidatePrefabMeta()
        {
            if (prefab.Name == string.Empty) 
                Debug.WriteLine("Warning: Prefab does not have a name.");
            else if (prefab.Name == null) 
                throw new NullReferenceException($"Prefab name was null!");

            if (prefab.Objects == null)
                throw new NullReferenceException($"Prefab Object list was null!");
            else if (prefab.Objects.Count == 0)
                throw new Exception("Cannot create an empty prefab!");
        }

        private void ValidateId()
        {
            var ids = prefab.Objects.Select(x => x.Id);

            HashSet<string> idCheck = new HashSet<string>();

            foreach (var id in ids)
                if (!idCheck.Contains(id))
                    idCheck.Add(id);
                else
                    throw new Exception($"Duplicate ID ({id}) detected!");
        }

        private void ValidateEvents()
        {
            var events = prefab.Objects.Select(x => x.ObjectEvents);

            foreach (var e in events)
            {
                //idiot check for positions
                if (e.PositionEvents == null)
                    throw new NullReferenceException("Position Event list was null!");
                else if (e.PositionEvents.Count == 0)
                    throw new Exception("Cannot create object with 0 event!");

                e.PositionEvents.Sort((x, y) => x.Time.CompareTo(y.Time));

                //scales
                if (e.ScaleEvents == null)
                    throw new NullReferenceException("Scale Event list was null!");
                else if (e.ScaleEvents.Count == 0)
                    throw new Exception("Cannot create object with 0 event!");

                e.ScaleEvents.Sort((x, y) => x.Time.CompareTo(y.Time));

                //rotations
                if (e.RotationEvents == null)
                    throw new NullReferenceException("Rotation Event list was null!");
                else if (e.RotationEvents.Count == 0)
                    throw new Exception("Cannot create object with 0 event!");

                e.RotationEvents.Sort((x, y) => x.Time.CompareTo(y.Time));

                //colors
                if (e.ColorEvents == null)
                    throw new NullReferenceException("Color Event list was null!");
                else if (e.ColorEvents.Count == 0)
                    throw new Exception("Cannot create object with 0 event!");

                e.ColorEvents.Sort((x, y) => x.Time.CompareTo(y.Time));
            }
        }
    }
}
