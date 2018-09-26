namespace Easy.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Provides scoped logging.
    /// </summary>
    public sealed class Scope : IDisposable
    {
        private readonly string _name;

        /// <summary>
        /// Creates an instance of the <see cref="Scope"/> with the given <paramref name="name"/>.
        /// </summary>
        private Scope(string name)
        {
            _name = name;
            Scopes.Value.Add(name);
        }

        internal static Scope Add(string name) => new Scope(name);

        /// <summary>
        /// Gets all the scopes.
        /// </summary>
        internal static ThreadLocal<List<string>> Scopes { get; } 
            = new ThreadLocal<List<string>>(() => new List<string>(10));

        /// <summary>
        /// Gets the flag indicating whether there are any scopes.
        /// </summary>
        internal static bool IsEmpty => Scopes.Value.Count == 0;

        /// <summary>
        /// Removes the scope.
        /// </summary>
        public void Dispose() { Scopes.Value.Remove(_name); }
    }
}