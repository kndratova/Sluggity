using System;
using System.Collections.Generic;
using Sluggity.Core.Objects;

namespace Sluggity.Core.Physics
{
    public class Collider : Component
    {
        private HashSet<Collider> m_contacts = new HashSet<Collider>();
        private HashSet<Collider> m_nextContacts = new HashSet<Collider>();
        
        public Collider(double offsetX, double offsetY, double width, double height, bool isSolid = false)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
            Width = width;
            Height = height;
            IsSolid = isSolid;
        }

        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public bool IsSolid { get; set; }

        public event Action<Collider> ContactEntered;
        public event Action<Collider> ContactExited;

        public IReadOnlyCollection<Collider> Contacts => m_contacts;

        public Box GetBounds()
        {
            return new Box(Owner.X + OffsetX, Owner.Y + OffsetY, Width, Height);
        }

        // calls by the scene for every overlap found this step
        internal void AddContact(Collider other)
        {
            m_nextContacts.Add(other);
        }

        // calls by the scene after all overlaps are collected
        internal void CommitContacts()
        {
            foreach (Collider other in m_nextContacts)
            {
                if (!m_contacts.Contains(other))
                {
                    ContactEntered?.Invoke(other);
                }
            }

            foreach (Collider other in m_contacts)
            {
                if (!m_nextContacts.Contains(other))
                {
                    ContactExited?.Invoke(other);
                }
            }

            // buffer swap
            (m_contacts, m_nextContacts) = (m_nextContacts, m_contacts);
            m_nextContacts.Clear();
        }
    }
}