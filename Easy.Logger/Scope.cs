namespace Easy.Logger
{
    using System;
    using log4net;
    using log4net.Util;

    /// <summary>
    /// Provides scoped logging.
    /// </summary>
    public struct Scope : IDisposable
    {
        private const string ContextName = "Easy.Logger.Scope";
        private static readonly LogicalThreadContextStacks Stacks = LogicalThreadContext.Stacks;

        /// <summary>
        /// Creates an instance of the <see cref="Scope"/> with the given <paramref name="name"/>.
        /// </summary>
        internal Scope(string name) => Stacks[ContextName].Push(name);

        internal static string ScopeMessage => Stacks[ContextName].ToString();

        /// <summary>
        /// Removes the scope.
        /// </summary>
        public void Dispose() => Stacks[ContextName].Pop();

        /// <summary>
        /// Returns the current context information for this scope.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ScopeMessage;
    }
}