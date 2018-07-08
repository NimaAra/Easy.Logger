namespace Easy.Logger
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides scoped logging.
    /// </summary>
    public sealed class Scope : IDisposable
    {
        private readonly string _name;
        
        /// <summary>
        /// Gets all the scopes.
        /// </summary>
        internal static List<string> Scopes { get; } = new List<string>(10);

        /// <summary>
        /// Gets the flag indicating whether there are any scopes.
        /// </summary>
        internal static bool IsEmpty
        {
            get { lock (Scopes) { return Scopes.Count == 0; } }
        }

        /// <summary>
        /// Creates an instance of the <see cref="Scope"/> with the given <paramref name="name"/>.
        /// </summary>
        internal Scope(string name)
        {
            _name = name;
            lock (Scopes) { Scopes.Add(name); }
        }

        /// <summary>
        /// Removes the scope.
        /// </summary>
        public void Dispose() { lock (Scopes) { Scopes.Remove(_name); } }
    }
}